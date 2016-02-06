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
using Jayrock.Json;
using Jayrock.Json.Conversion;
using Plugin;
using Timer = System.Timers.Timer;

namespace Remote.MediaPortal.iPimp.Api
{

    public class CommandInfoIPimp
    {
        public string Action { get; set; }
        public string Filter { get; set; }
        public string Value { get; set; }
        public string Start { get; set; }
        public string PageSize { get; set; }
        public bool Shuffle { get; set; }
        public bool Enqueue { get; set; }
        public string Tracks { get; set; }

        public CommandInfoIPimp()
        {
            Action = "";
            Filter = "";
            Value = "";
            Start = "0";
            PageSize = "0";
            Shuffle = false;
            Enqueue = false;
            Tracks = "";
        }
    }

    public class MediaPortal : ApiConnection
    {
        private const string DefaultPort = "55668";
        private const int ConnectedInterval = 2000;
        private const int DisconnectedInterval = 5000;
        private bool _configured;
        private Timer _checkTimer;
        private Timer _nowplayingTimer;
        private bool _isConnected;

        static readonly object Locker = new object();

        internal static class NativeMethods
        {
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetForegroundWindow(IntPtr hWnd);
        }

        public override void GiveFocus()
        {
            var processes = Process.GetProcessesByName("MediaPortal");
            foreach (var pFoundWindow in processes.Select(p => p.MainWindowHandle))
            {
                NativeMethods.SetForegroundWindow(pFoundWindow);
            }
        }

        public MediaPortal()
        {
            File = new MediaPortalFile(this);
            VideoLibrary = new MediaPortalVideoLibrary(this);
            AudioLibrary = new MediaPortalAudioLibrary(this);
            AudioPlayer = new MediaPortalAudioPlayer(this);
            PicturePlayer = new MediaPortalPicturePlayer(this);
            VideoPlayer = new MediaPortalVideoPlayer(this);
            Player = new MediaPortalPlayer(this);
            System = new MediaPortalSystem(this);
            Remote = new MediaPortalRemote(this);
            ApiName = "MPI";
        }

        public override string GetOS()
        {
            return "iPiMP";
        }

        public override string GetVersion()
        {
            var result = IPimpCommand(new CommandInfoIPimp {Action = "version"});
            return Convert.ToString(result["version"], CultureInfo.InvariantCulture);
        }

        public override string GetAdditionalInfo()
        {
            return "iPiMP powered";
        }

        public override bool IsConnected()
        {
            return _isConnected;
        }

        public static long StringToNumber (string input)
        {
            return String.IsNullOrEmpty(input) ? 0 : Convert.ToInt32("0" + input, CultureInfo.InvariantCulture);
        }

        public static double StringToDouble(string input)
        {
            return String.IsNullOrEmpty(input) ? 0.0 : Convert.ToDouble("0" + input, CultureInfo.InvariantCulture);
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
            var res = IPimpCommand(new CommandInfoIPimp { Action = "ping" });
            return Convert.ToString(res["data"], CultureInfo.InvariantCulture) == "pong";
        }

        public override bool CheckRemote(string os, string version, string additional, bool force)
        {
            var cVersion = GetVersion();
            var cAdditional = GetAdditionalInfo();

            if (cVersion != version)
                return false;
            if (cAdditional != additional && !force)
                return false;
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
            Port = String.IsNullOrEmpty(port) ? DefaultPort : port;
            UserName = user;
            Password = password;
            _configured = true;

            var res = IPimpCommand(new CommandInfoIPimp {Action = "ping"});
            Log(res["result"].ToString());

            // TODO : Check version (Return 2 if not compatible version

            return Convert.ToBoolean(res["result"], CultureInfo.InvariantCulture) ? 1 : 0;
        }



        public override void Configure(string ip, string port, string user, string password)
        {
            if (String.IsNullOrEmpty(ip)) return;
            
            IP = ip;
            Port = String.IsNullOrEmpty(port) ? DefaultPort : port;
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
            _isConnected = CheckConnection();
        }

        public Uri GetApiPath()
        {
            if (!_configured) return null;
            var uri = new Uri(@"http://" + IP + ":" + Port + "/mpcc/");
            return uri;
        }

        public NetworkCredential GetCredentials()
        {
            if (UserName == null || Password == null)
                return null;
            
            return new NetworkCredential(UserName, Password);
        }

        public  void AsyncIPimpCommand(CommandInfoIPimp command)
        {
            var bw = new BackgroundWorker();
            bw.DoWork += AsyncIPimpCommandWorker;
            bw.RunWorkerAsync(command);
        }

        private void AsyncIPimpCommandWorker(object sender, DoWorkEventArgs e)
        {
            var command = (CommandInfoIPimp)e.Argument;
            IPimpCommand(command);
        }

        public void AsyncIPimpButton(string button)
        {
            var bw = new BackgroundWorker();
            bw.DoWork += AsyncIPimpButtonWorker;
            bw.RunWorkerAsync(button);
        }

        private void AsyncIPimpButtonWorker(object sender, DoWorkEventArgs e)
        {
            IPimpButton((string)e.Argument);
        }

        public bool IPimpButton(string button)
        {
            var args = new CommandInfoIPimp
                           {
                               Action = "button",
                               Filter = button,
                           };

            var result = IPimpCommand(args);
            return Convert.ToBoolean(result["result"],CultureInfo.InvariantCulture);
        }

        public JsonObject IPimpCommand(CommandInfoIPimp command)
        {
            var error = new JsonObject();
            error["result"] = false;

            if (command == null)
                return error;

            lock (Locker)
            {
                var args = new JsonObject();
                args["action"] = command.Action;
                args["filter"] = command.Filter;
                args["value"] = command.Value;
                args["start"] = command.Start;
                args["pagesize"] = command.PageSize;
                args["shuffle"] = command.Shuffle;
                args["enqueue"] = command.Enqueue;
                args["tracks"] = command.Tracks;

                if (command.Action != "ping" && command.Action != "nowplaying")
                    Log("COMMAND : " + args);
                else
                    Trace("COMMAND : " + args);

                var myCompleteMessage = "";

                var webRequest = WebRequest.Create(GetApiPath());
                webRequest.Method = "POST";

                var byteArray = Encoding.UTF8.GetBytes(args.ToString());
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = byteArray.Length;

                try
                {
                    var datastream = webRequest.GetRequestStream();
                    {
                        datastream.Write(byteArray, 0, byteArray.Length);
                        datastream.Close();

                        var webResponse = webRequest.GetResponse();
                        if (webResponse != null)
                        {
                            using (var datastream2 = webResponse.GetResponseStream())
                            {
                                if (datastream2 != null)
                                {
                                    var reader = new StreamReader(datastream2);
                                    myCompleteMessage = reader.ReadToEnd();
                                }
                            }
                            webResponse.Close();
                        }
                    }
                }
                catch (Exception)
                {
                    if (command.Action != "ping" && command.Action != "nowplaying")
                        Log("ERROR - COMMAND : " + args);
                    else
                        Trace("ERROR - COMMAND : " + args);
                    myCompleteMessage = error.ToString();
                }

                Trace(myCompleteMessage);
                return (JsonObject)JsonConvert.Import(myCompleteMessage);
            }
        }

        public JsonArray IPimpDBCommand(CommandInfoIPimp cmd, string name)
        {
            if (!_configured) return null;

            var result = IPimpCommand(cmd);
            if (result == null)
                return null;

            try
            {
                return (JsonArray)result[name];
            }
            catch (JsonException e)
            {
                Log("ERROR - DBCOMMAND : " + cmd + " - " + e.Message);
            }
            return null;
        }

        public override void Close()
        {
            if (!_configured) return;
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
            }
        }

    }
}