﻿// ------------------------------------------------------------------------
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
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Jayrock.Json;
using Jayrock.Json.Conversion;
using Plugin;
using Setup;
using Timer = System.Timers.Timer;
using System.Web;


//using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
//using System.Text.RegularExpressions;
//using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using System.Collections.Specialized;

namespace Remote.Jriver.Api
{
    public class Xbmc : ApiConnection
    {
        private const string XbmcEventServerPort = "9777";
        public string MpcHcPort = "13579";
        public string ClientIPAddress = "";
        public string ServerPort = "52199";
        public string JRiverAuthToken = "";

        private readonly XbmcEventClient _eventClient = new XbmcEventClient();
        //private const string ApiPath = "/xbmcCmds/xbmcHttp";
        private const string JsonPath = "/jsonrpc";
        private const int ConnectedInterval = 2000;
        private const int DisconnectedInterval = 5000;
        private bool _configured;
        private Timer _checkTimer;
        private Timer _nowplayingTimer;
        private bool _isConnected;

        static readonly object Locker = new object();

        public MpcHcRemote MpcHcRemote { get; set; }

        public bool MpcLoaded  { get; set; }

        private class JsonCommandInfo
        {
            public string Command { get; set; }
            public Object Parameter { get; set; }
        }

        internal static class NativeMethods
        {
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetForegroundWindow(IntPtr hWnd);

        }

        public override void GiveFocus()
        {
            if (!MpcLoaded)
            {
                var processes = Process.GetProcessesByName("JRiver");
                foreach (var pFoundWindow in processes.Select(p => p.MainWindowHandle))
                {
                    NativeMethods.SetForegroundWindow(pFoundWindow);
                }
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
            SystemRunning = new XbmcSystem(this);
            Remote = new XbmcRemote(this);
            MpcHcRemote = new MpcHcRemote(this);
            ApiName = "JRV";
        }

        private JsonObject GetApplicationProperties(string label)
        {
            //var items = new JsonObject();
            //items["properties"] = new[] { label };
            //var res = JsonCommand("Application.GetProperties", items);
            //return res == null ? null : (JsonObject)((JsonObject)res)[label];
            return null;
        }

/*
        private JsonObject GetSystemProperties(string label)
        {
            var items = new JsonObject();
            items["properties"] = new[] { label };
            var res = JsonCommand("System.GetProperties", items);
            return res == null ? null : (JsonObject)((JsonObject)res)[label];
        }
*/

        public override string GetOS()
        {
            return "JRiver";
        }

        public override string GetVersion()
        {
            var infos = GetApplicationProperties("version");
            if (infos == null) return "";
            var ver = "" + infos["major"] + "." + infos["minor"] +"." + infos["revision"];
            return ver;
        }

        public override string GetAdditionalInfo()
        {
            return "";
            /*var infos = GetSystemProperties("profile");
            if (infos == null) return "";
            return infos.ToString();*/
        }

        public override bool IsConnected()
        {
            return _isConnected;
        }

        public string GetDownloadPath(string fileName)
        {
            try
            {
                if (!_configured) return null;
                if (fileName == "") return null;
                if (fileName.StartsWith(@"\\"))
                {
                    Log("JRiver:  GetDownloadPath: \\ Found Returing FileName:" + fileName);
                    return fileName;
                }
                else if ( fileName[1] == ':')
                {
                    Log("JRiver:  GetDownloadPath: : - Yes Semi-Colon - presume local file - Found Returing FileName:" + fileName);
                    return fileName;
                }
                    
                // return @"http://" + HttpUtility.UrlEncode(fileName);
                Log("Line 171: Plex API - Trying to sortout fanart URL: and Returning FileName: " + HttpUtility.UrlEncode(fileName));
                return HttpUtility.UrlEncode(fileName);
            }
            catch (Exception ex)
            {
                Log("GetDownloadPath Exception:" + ex);
                Log("GetDOwnloadPath Returning unmodifed Filename:" + fileName);
                return fileName;
            }
        }

        public static long StringToNumber (string input)
        {
            int res;

            try
            {
                res = String.IsNullOrEmpty(input) ? 0 : Convert.ToInt32(input,CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                Logger.Instance().Trace("ERROR : ","Convert Error" + input);
                return 0;
            }

            return res;
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

            if (!MpcLoaded)
            {

                string url =  "http://" + IP + ":" + Port + "/MCWS/v1/Alive?Token="+JRiverAuthToken;
                // PMS Server Clients Page - to connect to and see whether local player is in effect.

                try
                {
                    var request = WebRequest.CreateHttp(url);
                    request.Headers.Add("Token", JRiverAuthToken);
                    Log("CheckConnection: JRiverToken Equals:" + JRiverAuthToken);                

                    request.Method = "get";
                    request.Timeout = 5000;
                    request.ContentType = "application/json; charset=utf-8";

                    request.Accept = "application/json; charset=utf-8";

                    request.Host = IP + ":" + Port;
                    request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.143 Safari/537.36";

                    var values = new NameValueCollection();
                    values["Upgrade-Insecure-Requests"] = "1";

                    request.Headers.Add(values);

                    var response = request.GetResponse();

                    if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                    {

                        // Get the stream containing content returned by the server.
                        System.IO.Stream dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader.
                        System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);

                        XmlSerializer serializer = new XmlSerializer(typeof(Remote.Jriver.Api.Alive2.Response));
                        var deserialized = (Remote.Jriver.Api.Alive2.Response)serializer.Deserialize(reader);

                        string json = reader.ReadToEnd().ToString();
                        Log("CheckConnection JRiver:" + json);
                       

                        foreach (var server in deserialized.Item)
                        {
                            Log("name is " + server.Name + " and Other Value" + server.Value);
                            return true;

                        }

                        Log("Local Client not found - disconnecting");
                        return false;


                    }
                    Log("HTTP Status Not Okay - no exception failed - disconnecting");
                    return false;

                }
                catch (Exception ex)
                {
                    Log("Cannot connect are server details right " + ex);
                    return false;
                }
            }
            return true;
        }

        public static long IDtoNumber(string input)
        {
            long res;

            input = input.ToUpper();

            try
            {
                res = String.IsNullOrEmpty(input) ? 0 : Convert.ToInt64(input);
               // res = String.IsNullOrEmpty(input) ? 0 : Convert.ToInt64(input.Substring(0,15), 16);

            }
            catch (Exception ex)
            {
                Logger.Instance().Trace("Jriver ERROR : ", "IDtoNumber Error:" + input );
                Logger.Instance().Trace("Jriver ERROR: ", " IDtoNumber Exception" + ex);
                return 0;
            }

            return (long)res;
        }

        public static long IDstringtoNumber(string input)
        {
            long res;

            //input = input.ToUpper();

            try
            {
                res = input.GetHashCode();
                //Logger.Instance().Trace("JRiver"," IDstringtoNumber: Name to Number: Name:" + input + " and Number:" + res);
                // res = String.IsNullOrEmpty(input) ? 0 : Convert.ToInt64(input.Substring(0,15), 16);

            }
            catch (Exception ex)
            {
                Logger.Instance().Trace("Jriver ERROR : ", "ID String to Number Error:" + input);
                Logger.Instance().Trace("Jriver ERROR: ", " ID String to Number Exception:" + ex);
                return 0;
            }

            return (long)res;
        }
        public override bool CheckRemote(string os, string version, string additional, bool force)
        {
            var cOs = GetOS();
            var cVersion = GetVersion();
            var cAdditional = GetAdditionalInfo();

           if (cOs != os)
                return false;
            if (cVersion != version)
                return false;
            if (cAdditional != additional && !force)
                return false;

            if (cAdditional != additional && force)
            {
                _eventClient.SendAction( "LoadProfile(" + additional + ")");
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

            JRiverAuthToken = GetPlexAuthToken(ip, port, user, password);

            if (String.IsNullOrEmpty(JRiverAuthToken))
            {
                Log("No JRiver Token - Not checking for clients.");
                return 0;
                // Not Connected - failed Setup.
                //Still need to check local player there - rather than internet server which gives Auth
            }

            string url = "http://" + ip + ":" + port + "/MCWS/v1/Playback/Info?Token="+JRiverAuthToken;
            // PMS Server Clients Page - to connect to and see whether local player is in effect.

            Log("JRiver: Url" + url);

            try
            {

                var request = WebRequest.CreateHttp(url);
                request.Headers.Add("Token", JRiverAuthToken);
             
      //          request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.143 Safari/537.36";
                
                var response = request.GetResponse();

                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {

                    // Get the stream containing content returned by the server.
                    System.IO.Stream dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader.
                    System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);

                    //string json = reader.ReadToEnd().ToString();
                    //Log("Plex Returned:" + json);   


                    XmlSerializer serializer = new XmlSerializer(typeof(Remote.Jriver.Api.InfoZone.Response));
                    Remote.Jriver.Api.InfoZone.Response deserialized = (Remote.Jriver.Api.InfoZone.Response)serializer.Deserialize(reader);


                    string json = reader.ReadToEnd().ToString();
                    Log("Testing Client URL:" + url);
                    Log("TestConnection JRiver: ip:"+ip+":Port:"+port+" Result:" + json);

                    foreach (var server in deserialized.Item)
                    {
                        Log("Clients FOUND: " + deserialized.Item.ToString() + " Status" + deserialized.Status);
                        if (server.Name =="ZoneID")
                        {
                            Log("Using ZoneID: "+server.Value);
                            ClientIPAddress = GetLocalIPAddress();
                            ServerPort = port;
                            return 1;
                        }

                    }

                    Log("Local Client not found - disconnecting");
                    return 0;


                }
                Log("HTTP Status Not Okay - no exception failed - disconnecting");
                return 0;

            }
            catch (Exception ex)
            {
                Log("Cannot connect is server details right " + ex);
                return 0;
            }





        }
        public static string GetLocalIPAddress()
        {
            try
            {

                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return "No IP Obtainable";
            }
            catch (Exception ex)
            {
                return "NO IP Obtainable." + ex;
            }
        }
       public string GetPlexAuthToken(string ip, string port, string user, string password)
{

            IP = ip;
            Port = port;
            UserName = user;
            Password = password;
            _configured = true;
            string tokenReturn = "NotFound";

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(UserName + ":" + Password);
            string auth = System.Convert.ToBase64String(plainTextBytes);

            try
            {
                string authInfo = UserName + ":" + Password;
                String encoded = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

                Log("Jriver: Using Parent IP equals: " + IP);
                string NPurl = "http://" + IP + ":" + Port + "/MCWS/v1/Authenticate";

                var request = WebRequest.CreateHttp(NPurl);

                request.Headers.Add("Authorization", "Basic " + encoded);
                request.Method = "get";
                request.Timeout = 5000;
                request.ContentType = "application/json; charset=utf-8";
                request.Accept = "application/json; charset=utf-8";
                request.Host = IP + ":" + Port;
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.143 Safari/537.36";

                var response = request.GetResponse();

                Log("JRiver: Authenication Encoded:" + encoded);
;
                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {

                    // Get the stream containing content returned by the server.
                    System.IO.Stream dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader.
                    System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);

                    XmlSerializer serializer = new XmlSerializer(typeof(Jriver.Api.Authenicate.Response));
                    Jriver.Api.Authenicate.Response deserialized = (Jriver.Api.Authenicate.Response)serializer.Deserialize(reader);

                    foreach (var item in deserialized.Item)
                    {
                        Log("JRiver: Authenicate:  Item.Name: " + item.Name + " and Value:" + item.Value);
                        if (item.Name=="Token")
                        {
                            JRiverAuthToken = item.Value.ToString();
                            tokenReturn = item.Value.ToString();
                        }
                    }

                    return tokenReturn;
                }
                _isConnected = false;
                return "";
                
            }
            catch (Exception ex)
            {
                Log("Jriver Connection Failed : " + ip + ":" + port);
                Log("Jriver Connection Failed : Error/Exception " + ex);
                _isConnected = false;
                return "";
            }

        }
            /*
            var check = JsonCommand("JSONRPC.Ping", null);
            if (check == null)
            {
                Log("Test connection : No response");
                return 0;
            }
            Log("Test connection : Response : " + check);
            var res = (Convert.ToString(check,CultureInfo.InvariantCulture) == "pong");
            if (res)
            {
                var version = GetVersion();
                var build = new Regex(@"r(\d+)");
                var m = build.Match(version);
                if (m.Success)
                {
                    var ver = Convert.ToInt32("0" + m.Groups[1],CultureInfo.InvariantCulture);
                    if (ver < 29000 )
                    {
                        Log("Target version : " + ver + " not compatible !");
                        return 2; // Plugin not compatible
                    }
                }
                // If not build found just go will perhaps works
            }
            return res ? 1 : 0;
            */


        public override void Configure(string ip, string port, string user, string password)
        {
            if (String.IsNullOrEmpty(ip)) return;
            
            IP = ip;
            Port = port;
            UserName = user;
            Password = password;
            _configured = true;

            if (String.IsNullOrEmpty(JRiverAuthToken))
            {
                JRiverAuthToken = GetPlexAuthToken(ip, port, user, password);
            }

            if (_checkTimer == null)
            {
                _checkTimer = new Timer { Interval = 1500, SynchronizingObject = null };
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
            _eventClient.SendHelo("FrontView+ - Remote Control");//, IconType.IconPng, Helper.SkinPath + Helper.Instance.CurrentSkin + @"\Interface\RemoteControl.png");
        }

        /*public string GetApiPath()
        {
            if (!_configured) 
                return null;
            return @"http://" + IP + ":" + Port + ApiPath;
        }*/

        public string GetJsonPath()
        {
            if (!_configured) 
                return null;
            return @"http://" + IP + ":" + Port ;
            //Changed Path for Plex -
        }

        public NetworkCredential GetCredentials()
        {
            if (UserName == null || Password == null)
                return null;

            return new NetworkCredential(UserName, Password);
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

        public void AsyncJsonCommand(string cmd, Object parameter)
        {
            var commandInfo = new JsonCommandInfo { Command = cmd, Parameter = parameter };

            var bw = new BackgroundWorker();
            bw.DoWork += AsyncJsonCommandWorker;
            bw.RunWorkerAsync(commandInfo);
        }

        private void AsyncJsonCommandWorker(object sender, DoWorkEventArgs e)
        {
            var commandInfo = (JsonCommandInfo)e.Argument;
            JsonCommand(commandInfo.Command, commandInfo.Parameter);
        }

        public Object JsonCommand(string cmd, Object parameter)
        {
            if (!_configured)
            {
                Log("Plex:  Something not configured.");
                return null;
            }

            if (JRiverAuthToken == "")
            {
                Log("No Jriver Token - Not checking for clients.");
                return 0;
                // Not Connected - failed Setup.
                //Still need to check local player there - rather than internet server which gives Auth
            }

            string url = "http://" + IP + ":" + Port + "/MCWS/v1/Alive?Token="+JRiverAuthToken;
            Log("JRiver COMMAND: Sending to " + url);

            // PMS Server Clients Page - to connect to and see whether local player is in effect.

            try
            {

                var request = WebRequest.Create(url);
                var response = request.GetResponse();

                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {

                    // Get the stream containing content returned by the server.
                   // System.IO.Stream dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader.
                 //   System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);
                    Log("Command HTTP Response OK");
                    return 1;
                }
                Log("HTTP Status Not Okay - no exception failed - disconnecting");
                return 0;

            }
            catch (Exception ex)
            {
                Log("Cannot send Command Error is server details right " + ex);
                return 0;
            }         
           
 
/*            var client = new JsonRpcClient { Url = GetJsonPath(), Timeout = GetTimeout()};
                      
            client.C
            var creds = GetCredentials();
            
            if (creds != null)
                client.Credentials = creds;
            object retval = null;
            var logparam = "";
            if (parameter != null)
            {
                logparam = parameter.GetType().ToString() == "System.String[]" ? String.Join(",", (string[])parameter) : parameter.ToString();
            }
            try
            {
                lock (Locker)
                {
                    retval = JsonConvert.Import(client.Invoke(cmd, parameter).ToString());
                }
                if (cmd != "JSONRPC.Ping" && cmd != "VideoPlayer.GetTime" && cmd != "System.GetInfoLabels" && cmd != "AudioPlayer.GetTime")
                    Log("JSONCMD : " + cmd + ((parameter == null) ? "" : " - " + logparam));
                else
                    Trace("JSONCMD : " + cmd + ((parameter == null) ? "" : " - " + logparam));
            }
            catch (JsonException e)
            {
                if (cmd != "JSONRPC.Ping")
                    Log("JSONCMD : " + cmd + ((parameter == null) ? "" : " : " + logparam) + " - " + e.Message);
                else
                    Trace("JSONCMD : " + cmd + ((parameter == null) ? "" : " : " + logparam) + " - " + e.Message);
            }
            finally
            {
                client.Dispose();
            }
            if (retval != null)
                Trace(retval.ToString());
            return retval;
            */
        }

        public String JsonArrayToString(JsonArray array)
        {
            String result = "";
            foreach (var item in array)
            {
                if (result == "")
                {
                    result = item.ToString();
                }
                else
                {
                    result = result + " / " + item;
                }
            }
            return result;
        }

        public Object JsonArrayCommand(string[] cmd, Object[] parameter)
        {
            if (!_configured)
                return null;

            var client = new JsonRpcClient { Url = GetJsonPath(), Timeout = GetTimeout() };
            var creds = GetCredentials();
            if (creds != null)
                client.Credentials = creds;
            object retval = null;
            var logparam = "";
            /*if (parameter != null)
            {
                logparam = parameter.GetType().ToString() == "System.String[]" ? String.Join(",", (string[])parameter) : parameter.ToString();
            }*/
            try
            {
                lock (Locker)
                {
                    retval = JsonConvert.Import(client.InvokeArray(AnyType.Value, cmd, parameter).ToString());
                }
                //if (cmd != "JSONRPC.Ping" && cmd != "VideoPlayer.GetTime" && cmd != "System.GetInfoLabels" && cmd != "AudioPlayer.GetTime")
                    Log("JSONCMD : " + cmd + ((parameter == null) ? "" : " - " + logparam));
                //else
                    Trace("JSONCMD : " + cmd + ((parameter == null) ? "" : " - " + logparam));
            }
            catch (JsonException e)
            {
                //if (cmd != "JSONRPC.Ping")
                    Log("JSONCMD : " + cmd + ((parameter == null) ? "" : " : " + logparam) + " - " + e.Message);
                //else
                    Trace("JSONCMD : " + cmd + ((parameter == null) ? "" : " : " + logparam) + " - " + e.Message);
            }
            finally
            {
                client.Dispose();
            }
            if (retval != null)
                Trace(retval.ToString());
            return retval;
        }


        /*public string[] Command(string cmd, string parameter)
        {
            lock (Locker)
            {
                if (!_configured) return null;
                string[] returnContent = null;
                var crFrodotials = GetCredentials();

                var uri = GetApiPath() + @"?command=" + Uri.EscapeDataString(cmd);
                uri += String.IsNullOrEmpty(parameter) ? "" : "(" + Uri.EscapeDataString(parameter) + ")";

                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
                    request.KeepAlive = false;
                    request.ReadWriteTimeout = 1000000;
                    request.Method = "GET";
                    request.Timeout = GetTimeout();
                    if (crFrodotials != null)
                        request.Credentials = crFrodotials;
                    if (cmd != "SetResponseFormat" && cmd != "GetCurrentlyPlaying" && cmd != "GetPercentage" && cmd != "GetVolume")
                    {
                        if (!String.IsNullOrEmpty(parameter))
                            Log("COMMAND : " + cmd + " - " + parameter);
                        else
                            Log("COMMAND : " + cmd);
                    }
                    Trace(uri);
                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        using (var stream = response.GetResponseStream())
                        {
                            if (stream != null)
                                using (var reader = new StreamReader(stream, Encoding.UTF8))
                                {
                                    var reqContent = reader.ReadToEnd();
                                    Trace(reqContent);
                                    reqContent = reqContent.Replace("<html>\n", "").Replace("\n</html>\n", "").Replace("\n</html>", "").Replace("</html>\n", "").Replace("\n<li>", "<li>")
                                        .Replace("</html>", "");
                                    var temp = reqContent.Split(new[] { "<li>" }, StringSplitOptions.None).ToList();
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
            if (!_configured) 
                return null;
            Command("SetResponseFormat", "openRecord;<record>;closeRecord;</record>;openField;<field>;closeField;</field>");
            string[][] returnContent = null;
            var credentials = GetCredentials();

            var uri = GetApiPath() + @"?command=query" + db + "database";
            uri += "(" + Uri.EscapeDataString(query) + ")";

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
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
        }*/

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
