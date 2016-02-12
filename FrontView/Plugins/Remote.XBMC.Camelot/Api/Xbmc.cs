// ------------------------------------------------------------------------
//    YATSE 2 - A touch screen remote controller for XBMC (.NET 3.5)
//    Copyright (C) 2010  Tolriq (http://yatse.leetzone.org)
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
// ------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Plugin;
using Timer = System.Timers.Timer;

namespace Remote.XBMC.Camelot.Api
{

    public class Xbmc : ApiConnection
    {

        private const string XbmcEventServerPort = "9777";
        private readonly XbmcEventClient _eventClient = new XbmcEventClient();
        private const string ApiPath = "/xbmcCmds/xbmcHttp";
        private const int ConnectedInterval = 2000;
        private const int DisconnectedInterval = 5000;
        private bool _configured;
        private Timer _checkTimer;
        private Timer _nowplayingTimer;
        private bool _isConnected;

        static readonly object Locker = new object();

        private class CommandInfo
        {
            public string Command { get; set; }
            public string Parameter { get; set; }
        }

        internal static class NativeMethods
        {
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetForegroundWindow(IntPtr hWnd);
        }

        public override void GiveFocus()
        {
            var processes = Process.GetProcessesByName("XBMC");
            foreach (var pFoundWindow in processes.Select(p => p.MainWindowHandle))
            {
                NativeMethods.SetForegroundWindow(pFoundWindow);
            }
        }

        public Xbmc()
        {
            File = new XbmcFile(this);
            VideoLibrary = new XbmcVideoLibrary(this);
            AudioLibrary = new XbmcAudioLibrary(this);
            AudioPlayer = new XbmcAudioPlayer(this);
            PicturePlayer = new XbmcPicturePlayer(this);
            VideoPlayer = new XbmcVideoPlayer(this);
            Player = new XbmcPlayer(this);
            System = new XbmcSystem(this);
            Remote = new XbmcRemote(this);
            ApiName = "XHT";
        }

        private string[] GetInfo(string infoid)
        {
            var ret = Command("GetSystemInfo", infoid);
            return ret;
        }

        public override string GetOS()
        {
            var infos = GetInfo("667");
            if (infos == null) return "";
            Thread.Sleep(500);
            infos = GetInfo("667");
            if (infos == null) return "";
            if (infos[0].Contains("Windows")) return "Windows";
            if (infos[0].Contains("Darwin")) return "AppleTv";
            if (infos[0].Contains("Linux")) return "Linux";
            return "Xbox";
        }

        public override string GetVersion()
        {
            var infos = GetInfo("120");
            if (infos == null) return "";
            return infos[0];
        }

        public override string GetAdditionalInfo()
        {
            var infos = GetInfo("146");
            if (infos == null) return "";
            return infos[0];
        }

        public override bool IsConnected()
        {
            return _isConnected;
        }

        public static long StringToNumber (string input)
        {
            return String.IsNullOrEmpty(input) ? 0 : Convert.ToInt32("0" + input,CultureInfo.InvariantCulture);
        }

        public static string Hash(string input)
        {
            if (input == null)
                return "";
            var chars = input.ToCharArray();
            for (var index = 0; index < chars.Length; index++)
            {
                if (chars[index] <= 127)
                    chars[index] = Char.ToLowerInvariant(chars[index]);
            }
            input = new string(chars);
            var mCrc = 0xffffffff;
            var bytes = Encoding.UTF8.GetBytes(input);
            foreach (var myByte in bytes)
            {
                mCrc ^= ((uint)(myByte) << 24);
                for (var i = 0; i < 8; i++)
                {
                    if ((Convert.ToUInt32(mCrc) & 0x80000000) == 0x80000000)
                    {
                        mCrc = (mCrc << 1) ^ 0x04C11DB7;
                    }
                    else
                    {
                        mCrc <<= 1;
                    }
                }
            }
            return String.Format(CultureInfo.InvariantCulture,"{0:x8}", mCrc);
        }

        public override bool CheckConnection()
        {
            var webserverEnabled = Command("SetResponseFormat");
            if (webserverEnabled == null)
                return false;
                
            return (webserverEnabled[0] == "OK") ? true : false;
        }

        public override bool CheckRemote(string os, string version, string additional, bool force)
        {
            var cVersion = GetVersion();
            var cAdditional = GetAdditionalInfo();

            if (cVersion != version)
                return false;
            if (cAdditional != additional && !force)
                return false;

            if (cAdditional != additional && force)
            {
                Command("ExecBuiltIn", "LoadProfile(" + additional + ")");
                Thread.Sleep(400);
                cAdditional = GetAdditionalInfo();
                return cAdditional == additional;
            }

            return true;
        }

        public override int TestConnection(string ip, string port, string user, string password)
        {
            if (String.IsNullOrEmpty(ip)) return 0;
            if (String.IsNullOrEmpty(port))
                Log("Test connection : " + ip);
            else
                Log("Test connection : " + ip + ":" + port);
            IP = ip;
            Port = port;
            UserName = user;
            Password = password;
            _configured = true;

            var webserverEnabled = Command("SetResponseFormat");
            if (webserverEnabled == null)
            {
                Log("Test connection : No response");
                return 0;
            }

            Log("Test connection : Response : " + webserverEnabled[0]);
            var res = (webserverEnabled[0] == "OK");
            if (res)
            {
                var version = GetVersion();
                var build = new Regex(@"r(\d+)");
                var m = build.Match(version);
                if (m.Success)
                {
                    var ver = Convert.ToInt32("0" + m.Groups[1], CultureInfo.InvariantCulture);
                    if (ver > 29000 || ver < 26017)
                    {
                        Log("Target version : " + ver + " not compatible !");
                        return 2; // Plugin not compatible
                    }
                }
                // If not build found just go will perhaps works
            }
            return res ? 1 : 0;

        }

        public override void Configure(string ip, string port, string user, string password)
        {
            if (String.IsNullOrEmpty(ip)) return;
            
            IP = ip;
            Port = port;
            UserName = user;
            Password = password;
            _configured = true;

            if (_checkTimer == null)
            {
                _checkTimer = new Timer { Interval = 1000, SynchronizingObject = null };
                _checkTimer.Elapsed += CheckTimerTick;
                _checkTimer.Start();
            }

            if (_nowplayingTimer == null)
            {
                _nowplayingTimer = new Timer { Interval = 1000, SynchronizingObject = null };
                _nowplayingTimer.Elapsed += NowPlayingTimerTick;
                _nowplayingTimer.Start();
            }
            Log("Configure : " + ip + ":" + port);
        }

        private void NowPlayingTimerTick(object sender, EventArgs e)
        {
            Player.RefreshNowPlaying();
        }

        private void CheckTimerTick(object sender, EventArgs e)
        {
            _checkTimer.Interval = (_isConnected) ? ConnectedInterval : DisconnectedInterval;
            var connect = _isConnected;
            _isConnected = CheckConnection();
            if (!_isConnected || connect == _isConnected) return;

            _eventClient.Connect(IP, Convert.ToInt32("0" + XbmcEventServerPort, CultureInfo.InvariantCulture));
            _eventClient.SendHelo("Yatse - Remote Control");
                //, IconType.IconPng, Helper.SkinPath + Helper.Instance.CurrentSkin + @"\Interface\RemoteControl.png");
        }

        public string GetApiPath()
        {
            if (!_configured) return null;
            return @"http://" + IP + ":" + Port + ApiPath;
        }

        public NetworkCredential GetCredentials()
        {
            if (UserName == null || Password == null)
                return null;
            
            return new NetworkCredential(UserName, Password);
        }

        public bool SetResponseFormat(string parameter)
        {
            if (!_configured) return false;

            var aResult = Command("SetResponseFormat", parameter);
            if (aResult == null)
                return false;
            return (aResult[0] == "OK") ? true : false;
        }

        public void AsyncEventButton(string cmd)
        {
            var bw = new BackgroundWorker();
            bw.DoWork += AsyncEventActionButton;
            bw.RunWorkerAsync(cmd);
        }

        private void AsyncEventActionButton(object sender, DoWorkEventArgs e)
        {
            var commandInfo = (string)e.Argument;
            _eventClient.SendButton(commandInfo, "R1", ButtonTypes.BtnDown | ButtonTypes.BtnNoRepeat);
        }

        public void AsyncEventAction(string cmd)
        {
            var bw = new BackgroundWorker();
            bw.DoWork += AsyncEventActionWorker;
            bw.RunWorkerAsync(cmd);
        }

        private void AsyncEventActionWorker(object sender, DoWorkEventArgs e)
        {
            var commandInfo = (string)e.Argument;
            _eventClient.SendAction(commandInfo);
        }

        public void AsyncCommand(string cmd, string parameter)
        {
            var commandInfo = new CommandInfo { Command = cmd, Parameter = parameter };

            var bw = new BackgroundWorker();
            bw.DoWork += AsyncCommandWorker;
            bw.RunWorkerAsync(commandInfo);
        }

        private void AsyncCommandWorker(object sender, DoWorkEventArgs e)
        {
            var commandInfo = (CommandInfo)e.Argument;
            Command(commandInfo.Command,commandInfo.Parameter);
        }

        public string[] Command(string cmd, string parameter)
        {
            lock (Locker)
            {
                if (!_configured) return null;
                HttpWebRequest request;
                string reqContent;
                string[] returnContent = null;
                var credentials = GetCredentials();

                var uri = GetApiPath() + @"?command=" + Uri.EscapeDataString(cmd);
                uri += String.IsNullOrEmpty(parameter) ? "" : "(" + Uri.EscapeDataString(parameter) + ")";

                try
                {
                    request = (HttpWebRequest) WebRequest.Create(new Uri(uri));
                    request.KeepAlive = false;
                    request.ReadWriteTimeout = 1000000;
                    request.Method = "GET";
                    request.Timeout = GetTimeout();
                    if (credentials != null)
                        request.Credentials = credentials;
                    if (cmd != "SetResponseFormat" && cmd != "GetCurrentlyPlaying" && cmd != "GetPercentage" &&
                        cmd != "GetVolume")
                    {
                        if (!String.IsNullOrEmpty(parameter))
                            Log("COMMAND : " + cmd + " - " + parameter);
                        else
                            Log("COMMAND : " + cmd);
                    }
                    Trace(uri);
                    using (var response = (HttpWebResponse) request.GetResponse())
                    {
                        using (var stream = response.GetResponseStream())
                        {
                            if (stream != null)
                                using (var reader = new StreamReader(stream, Encoding.UTF8))
                                {
                                    reqContent = reader.ReadToEnd();
                                    Trace(reqContent);
                                    reqContent = reqContent.Replace("<html>\n", "").Replace("\n</html>\n", "").Replace(
                                        "\n</html>", "").Replace("</html>\n", "").Replace("\n<li>", "<li>")
                                        .Replace("</html>", "");
                                    var temp = reqContent.Split(new[] {"<li>"}, StringSplitOptions.None).ToList();
                                    temp.RemoveAt(0);
                                    returnContent = temp.ToArray();
                                }
                        }
                    }

                }
                catch (WebException e)
                {
                    if (cmd != "SetResponseFormat")
                    {
                        if (!String.IsNullOrEmpty(parameter))
                            Log("ERROR - COMMAND : " + cmd + " : " + parameter + " - " + e.Message);
                        else
                            Log("ERROR - COMMAND : " + cmd + " - " + e.Message);
                    }
                }
                return returnContent;
            }
        }

        private static string[][] DBResultToArray(string requestReturn)
        {
            requestReturn = requestReturn.Replace("<field>", "").Replace("\n", "").Replace("<html>", "").Replace("</html>", "").Replace("<record>", "");

            var temp = requestReturn.Split(new[] { "</field></record>" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (temp.Count < 1) return null;

            var returnValue = temp.Select(temp2 => temp2.Split(new[] { "</field>" }, StringSplitOptions.None)).ToArray();
            return returnValue.Length < 1 ? null : returnValue;
        }

        public string[][] DBCommand(string db, string query)
        {
            if (!_configured) return null;
            SetResponseFormat("openRecord;<record>;closeRecord;</record>;openField;<field>;closeField;</field>");

            HttpWebRequest request;
            string[][] returnContent = null;
            var credentials = GetCredentials();

            var uri = GetApiPath() + @"?command=query" + db + "database";
            uri += "(" + Uri.EscapeDataString(query) + ")";

            try
            {
                request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
                request.KeepAlive = false;
                request.ReadWriteTimeout = 1000000;
                request.Method = "GET";
                request.Timeout = GetTimeout();
                if (credentials != null)
                    request.Credentials = credentials;

                Log("DBCOMMAND : " + db + " - " + query);
                Trace(uri);
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                            using (var reader = new StreamReader(stream, Encoding.UTF8))
                            {
                                var result = reader.ReadToEnd();
                                Trace(result);
                                returnContent = DBResultToArray(result);
                            }
                    }
                }
                
            }
            catch (WebException e)
            {
                Log("ERROR - DBCOMMAND : " + db + " - " + query + " - " + e.Message);
            }
            return returnContent;
        }


        public string[] Command(string cmd)
        {
            return Command(cmd, "");
        }

        public override void Close()
        {
            if (!_configured) return;
            _eventClient.SendBye();
            _eventClient.Disconnect();
            _checkTimer.Stop();
            _checkTimer.Dispose();
            _nowplayingTimer.Stop();
            _nowplayingTimer.Dispose();
            File.StopAsync();
            Log("Closing Remote");
        }

        public sealed override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                File.Dispose();
                _eventClient.Dispose();
            }
        }

    }
}