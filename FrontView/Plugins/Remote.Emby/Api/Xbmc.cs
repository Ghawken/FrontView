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


namespace Remote.Emby.Api
{
    public class Globals
    {
        public static String EmbyAuthToken = ""; // Modifiable in Code
        public static String DeviceID = "9DA94EFB-EFF0-4144-9A18-46B046C450C6";
        public static string SessionID = "";
        public static string SessionIDClient = "";
        public static bool ClientSupportsRemoteControl = false;
        public static string CurrentUserID ="";
    }
    
    
    
    public class Xbmc : ApiConnection
    {
        private const string XbmcEventServerPort = "9777";
        public string MpcHcPort = "13579";
        public string ClientIPAddress = "";
        public string ServerPort = "32400";
       // public string EmbyAuthToken ="Testing";
        public string CurrentUserID = "";
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
        //private string EmbyAuthToken;

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
                var processes = Process.GetProcessesByName("Kodi");
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
            ApiName = "XXX";
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
            return "Emby";
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
                // return @"http://" + HttpUtility.UrlEncode(fileName);
                if (String.IsNullOrEmpty(fileName)) return null;

                Log("Emby API - Trying to sortout fanart URL: " + HttpUtility.UrlEncode(fileName));
                return HttpUtility.UrlEncode(fileName);
            }
            catch (Exception ex)
            {
                Log("-!-!-!-!-!-!-!-!-!---------- EMBY API: Getdownloadpath exception xbmc.cs " + ex);
                return null;
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


        public static long IDtoNumber(string input)
        {
            long res;

            input = input.ToUpper();

            try
            {
                res = String.IsNullOrEmpty(input) ? 0 : Convert.ToInt64(input.Substring(0,15), 16);
            }
            catch (Exception ex)
            {
                Logger.Instance().Trace("ERROR : ", "IDtoNumber Error" + input + "Result ");
                Logger.Instance().Trace("ERROR: ", " IDtoNumber Exception" + ex);
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

          /*  if (!MpcLoaded)
            {
            */
            

            string url = @"http://"+GetJsonPath() + "/FrontView";
                // PMS Server Clients Page - to connect to and see whether local player is in effect.

                try
                {

                    var request = WebRequest.Create(url);
                    request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
                    var response = request.GetResponse();

                    if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                    {

                        Log("EMBY Server found - FrontView Checked");
                        response.Close();
                        return true;


                    }
                    Log("HTTP Status Not Okay - no exception failed - disconnecting");
                    return false;

                }
                catch (Exception)
                {
                    Log("Cannot connect to Server: In CheckConnection are server details right? ");
                    return false;
                }
            
            //return true;
        }
        public bool CheckPing()
        {
            try
            {
                Log("CheckPing Running:--------------------------%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");

            string url = GetJsonPath() + "/Users/Public";
            
            TcpClient client = new TcpClient(IP,Convert.ToInt32(Port));
            return true;
            }
            catch (Exception ex)
            {
                Log("CheckPing ERROR is server details right " + ex);
                return false;
            }

            //return true;
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

        public string GetAuthString()
        {
            string clientname = "FrontView";
            string devicename = "Windows Application";
            string deviceID = Globals.DeviceID; // "9DA94EFB-EFF0-4144-9A18-46B046C450C6";
            string appVersion = "1.100";

            if (String.IsNullOrEmpty(Globals.CurrentUserID))
            {
                CurrentUserID = GetCurrentUserID();
            }

            string AuthString = @"MediaBrowser Client=""" + clientname + "\", Device=\"" + devicename + "\", DeviceId=\"" + deviceID + "\", Version=\"" + appVersion + "\", UserId=\"" + CurrentUserID + "\"";
            Trace("--------- GetAuthString Returns:" + AuthString);
            return AuthString;
       }


        public string GetCurrentUserID()
        {
           // string authString = GetAuthString();

            string url = "http://" + IP + ":" + Port + "/Users/Public";
            Trace("---------------- Getting Current User ID -------------------------");
            Trace("URL is " + url);
            try
            {

                var request = WebRequest.CreateHttp(url);
                request.Method = "get";
               // request.Timeout = 5000;
              //  request.Headers.Add("X-MediaBrowser-Token", EmbyAuthToken);
              //  request.Headers.Add("Authorization", authString);
                request.ContentType = "application/json; charset=utf-8";

                request.Accept = "application/json";

                var response = request.GetResponse();

                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {

                    // Get the stream containing content returned by the server.
                    //REMOVETHIS                       System.IO.Stream dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader.
//REMOVETHIS                    System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);

                    //XmlSerializer serializer = new XmlSerializer(typeof(Public_Users_Folder.Class1));
                   // Public_Users_Folder.Class1 deserialized = (Public_Users_Folder.Class1)serializer.Deserialize(reader);
//
                   

                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        string json = sr.ReadToEnd();
                        Trace(json);
                        
                        var deserializer = new JavaScriptSerializer();
                      
                        var results = deserializer.Deserialize<List<Public_Users_Folder.Class1>>(json);
                        Trace("-----------------  " +results[1].ConnectUserId);
                        
                        
                        foreach (var server in results)
                        {
                        Trace("------ CurrentUSERID --  " + server.Name + " Server.ID: " + server.Id + " Current Username:"+UserName);
                            if (server.Name == UserName)
                            {
                                  Trace("----------------- Returning CurrentUserID based on Username from Public/Users: UserID:"+server.Name +" Username "+server.Id);
                                  Globals.CurrentUserID = server.Id;
                                  return  server.Id;
                            }
                            
                            

                        }
                        Trace("  ----------------------- No CurrentUSER MATCHING FOUND ------------: Current Username:" + UserName);

                    }


                    
                    
                    
                    /*

                    */

                }
                return "";


            }
            catch (Exception ex)
            {
                Log("Exception: " + ex);
                return "";
            }


        }
        public override int TestConnection(string ip, string port, string user, string password)
        {
            if (String.IsNullOrEmpty(ip)) return 0;
            if (String.IsNullOrEmpty(port))
                Log("Test connection : " + ip);
            else
                Log("Test connection : " + ip + ":" + port);

            Globals.EmbyAuthToken = GetEmbyAuthToken(ip, port, user, password);

            if (String.IsNullOrEmpty(Globals.EmbyAuthToken))
            {
                Log("No Emby Token - Not checking for clients.");
                return 0;
                // Not Connected - failed Setup.
                //Still need to check local player there - rather than internet server which gives Auth
            }
            
            string authString = GetAuthString();

            Log("EMBY -- TEST CONNECTION: RUNINNG CHECK FOR YATSE Information");
            Globals.SessionIDClient = GetYatseInfoPlayingClient();
            
            Globals.ClientSupportsRemoteControl = GetPlaybackClientSupportsRemote();
            Log("EMBY -- TEST CONNECTION: Has run PlayBack Remote Check");

            try
            {

                var request = WebRequest.CreateHttp("http://"+ip+":"+port+"/Sessions");
                request.Method = "get";
                request.Timeout = 5000;
                Log("--------------- TEST CONNECTION: IP " + ip + ":" + port);

                request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);

                Log("------------------- Emby Token:" + Globals.EmbyAuthToken);

                request.Headers.Add("X-Emby-Authorization", authString);
                request.ContentType = "application/json; charset=utf-8";
                //  request.ContentLength = postArg.Length;
                request.Accept = "application/json";

                // var response3 = request.GetRequestStream();
                // response3.Write(data, 0, data.Length);

                // Console.WriteLine(response2.Headers);



                var response = request.GetResponse();


                
                Log("Test Connection : URI Requested = :" + response.ResponseUri);
                Log(((HttpWebResponse)response).StatusDescription);
             
                
                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {
                    return 1;
                }


                return 0;
                /*
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    string json = sr.ReadToEnd();
                    //Console.WriteLine(json);
                    // Gets the Whole JSON return if password etc good
                    // dynamic object fantastic - does need Class defined to return single Object

                    return 1;
                    Trace(json);

                    //  dynamic obj = SimpleJson.DeserializeObject(json);
                    // Console.WriteLine("Access Token EQUALS!   " + obj.AccessToken);
                    // accessToken = obj.AccessToken;
                }
                */

            }
            catch (Exception ex)
            {
                Log("Exception: " + ex);
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


        public static string HashSha1(string stringToHash)
        {
            using (var sha1 = new SHA1Managed())
            {
                return BitConverter.ToString(sha1.ComputeHash(Encoding.Default.GetBytes(stringToHash)));
            }
        }
        public static string HashMd5(string stringToHash)
        {
            using (var Md5 = new MD5CryptoServiceProvider())
            {
                return BitConverter.ToString(Md5.ComputeHash(Encoding.Default.GetBytes(stringToHash)));
            }
        }
        
        public string GetEmbyAuthToken(string ip, string port, string user, string password)
{

            Log("-------------------------Getting Emby TOKEN----------------------------------");
            
            IP = ip;
            Port = port;
            UserName = user;
            Password = password;

            if (String.IsNullOrEmpty(Password))
            {
                Password = string.Empty;
            }
            _configured = true;



            string hostbase = @"http://" + ip + ":" + port;
            string path = "/mediabrowser/Users/AuthenticateByName";
            string host = hostbase + path;
         //   string clientname = "Yatse3";
         //   string devicename = "Windows";
         //   string deviceID = "9DA94EFB-EFF0-4144-9A18-46B046C450C6";
         //   string applicationVersion = "1.0.0";
            
          //  if (CurrentUserID =="")
         //   {
         //       CurrentUserID = GetCurrentUserID();
        //    }
         //  /
            //string currentUserId = ""; ;

            var authString = GetAuthString();

            var postData = new Dictionary<string, string>();
            
            postData["username"] = Uri.EscapeDataString(UserName);

//REMOVETHIS            var bytes = Encoding.UTF8.GetBytes(password ?? string.Empty);

            //Log("---------------" + HashSha1(Password));

            postData["password"] = HashSha1(Password);

            postData["passwordMD5"] = HashMd5(Password);

            
            //var postArg = SimpleJson.SerializeObject(postData);

            var postArg = JsonConvert.ExportToString(postData);


            //REMOVETHIS               var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(UserName + ":" + Password);
//REMOVETHIS            string auth = System.Convert.ToBase64String(plainTextBytes);

         //   var authString = @"MediaBrowser Client=""" + clientname + "\", Device=\"" + devicename + "\", DeviceId=\"" + deviceID + "\", Version=\"" + applicationVersion + "\", UserId=\"" + CurrentUserID + "\"";

            Log(authString);

            try
            {

                // Connecting to AuthenicateUSerbyName to get AccessToken
                // Key here was to get data returned as JSON - key to that was the Request.Accept setting
                var request = WebRequest.CreateHttp(host);
                request.Method = "post";
                request.Timeout = 5000;
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] data = encoding.GetBytes(postArg);

                

                //request.Headers.Add("X-MediaBrowser-Token", accessToken);
                request.Headers.Add("X-Emby-Authorization", authString);
                request.ContentType = "application/json; charset=utf-8";
                request.ContentLength = postArg.Length;
                request.Accept = "application/xml";

                var response3 = request.GetRequestStream();
                response3.Write(data, 0, data.Length);

                var response2 = request.GetResponse();

                // Console.WriteLine(response2.Headers);



                var response = request.GetResponse();


                Log("HTTP WEB RESPONSE GIVEN:" + ((HttpWebResponse)response).StatusDescription);
//PERFORMANCE CHANGE
                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {

                    // Get the stream containing content returned by the server.
                    System.IO.Stream dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader.
                    System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);

                    //string json = reader.ReadToEnd();
                    //Log("--------------GETTING Authenication ID JSON------" + json);

                    XmlSerializer serializer = new XmlSerializer(typeof(AuthenicateByUser.Root.AuthenticationResult));


                    AuthenicateByUser.Root.AuthenticationResult deserialized = (AuthenicateByUser.Root.AuthenticationResult)serializer.Deserialize(reader);

                    Log("-------------- EMBY Access Token:" + deserialized.AccessToken);
                    Log("-------------- EMBY User ID: " + CurrentUserID);
                   // Log("-------------- EMBY Supports Remote Control: " + deserialized.SessionInfo.SupportsRemoteControl);
                    Log("-------------- EMBY Sessions ID: " + deserialized.SessionInfo.Id);


                    if (!String.IsNullOrEmpty(deserialized.AccessToken))
                    {
                        Log("------------------ EMBY ACCESS TOKEN FOUND:" + deserialized.AccessToken);
                        Log("------------------- Emby Sessions ID Set:" + deserialized.SessionInfo.Id);
                        //Log("------------------- Emby Client Supports Remote:" + deserialized.SessionInfo.SupportsRemoteControl);
                        Globals.EmbyAuthToken = deserialized.AccessToken;
                        Globals.SessionID = deserialized.SessionInfo.Id;
                        _isConnected = true;
                        MpcLoaded = true;
                     //   Globals.ClientSupportsRemoteControl = deserialized.SessionInfo.SupportsRemoteControl;
                        return deserialized.AccessToken ;
                    }
                }



                return "";




            }
            catch (Exception ex)
            {
                Log("Emby Connection Failed : "+host );
                Log("Exception" + ex);
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

            if (String.IsNullOrEmpty(Globals.EmbyAuthToken))
            {
                Globals.EmbyAuthToken = GetEmbyAuthToken(ip, port, user, password);
            }
            if (String.IsNullOrEmpty(Globals.SessionIDClient))
            {
                Globals.SessionIDClient = GetYatseInfoPlayingClient();
            }
            
            Globals.ClientSupportsRemoteControl = GetPlaybackClientSupportsRemote();



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

        public string GetYatseInfoPlayingClient()
        {
            try
            {

                Log("Emby: Using Parent IP equals: " + IP);
                string NPurl = "http://" + IP + ":" + Port;
                var request = WebRequest.CreateHttp(NPurl + "/FrontView");

                request.Method = "get";
                //request.Timeout = 5000;
                Log("--------------- PLAYER CONNECTION: IP " + NPurl);


                var authString = GetAuthString();



                Log("------------Get FrontView Info------- Username Parent :" + UserName);
                Log("------------Get FrontView Info------ CurrentUserID Parent :" + CurrentUserID);
                Log("------------Get FrontView Info------- EMBY TOKEN EQUALS :" + Globals.EmbyAuthToken);


                request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);

                request.Headers.Add("Authorization", authString);
                request.ContentType = "application/json; charset=utf-8";
                request.Accept = "application/json";
                var response = request.GetResponse();

                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {

                    System.IO.Stream dataStream = response.GetResponseStream();

                    using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        string json = sr.ReadToEnd();
                        Trace("--------------Using FrontView Info Emby Plugin Data   ------" + json);
                        var deserializer = new JavaScriptSerializer();

                        var server = deserializer.Deserialize<EmbyServerPlugin.ApiInfo>(json);
                        Trace("------------- FrontView Emby Plugin: Now Checking Results :results.Count:" + server.Filename);
                        Globals.SessionIDClient = server.PlayingClientID;
                        Log("-------------------------------PLAYING CLIENT ID GOT AND SET TO :" + Globals.SessionIDClient);
                        return server.PlayingClientID;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Log("--------------------------PLAYING CLIENT EXCEPTION" + ex);
                return null;
            }
        }



        private bool GetPlaybackClientSupportsRemote()
        {
            
            
            try
            {

                Log("Emby:  Get SESSION DATA ID Using Parent IP equals: " + IP);
                string NPurl = "http://" + IP + ":" + Port;
                var request = WebRequest.CreateHttp(NPurl + "/Sessions");

                request.Method = "get";
                //request.Timeout = 5000;
                Log("--------------- PLAYER CONNECTION: IP " + IP + ":" + Port);



                var authString = GetAuthString();



                Log("Client SESSIONAL:------------------- Username Parent :" + UserName);
                Log("Client Id:------------------- CurrentUserID Parent :" + CurrentUserID);
                Log("Client ID:------------------- EMBY TOKEN EQUALS :" + Globals.EmbyAuthToken);

                request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
                request.Headers.Add("X-Emby-Authorization", authString);
                request.ContentType = "application/json; charset=utf-8";
                request.Accept = "application/json";


                var response = request.GetResponse();


                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {

                    //  Use MPC Remote

                    //REMOVETHIS                       System.IO.Stream dataStream = response.GetResponseStream();

                    //REMOVETHIS                       System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);

                    using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        string json = sr.ReadToEnd();
                        Log("--------------GETTING CLIENT SESSIONAL JSON------" + json);
                        
                        var deserializer = new JavaScriptSerializer();

                        var results = deserializer.Deserialize<System.Collections.Generic.List<Sessions.Class1>>(json);

                        foreach (var server in results)
                        {
                                              

                            if (server.Id == Globals.SessionIDClient)
                            {
                                Log("Returning Client ID:" + server.Id);
                                Log("Returning Client Supports Remote Control:" + server.SupportsRemoteControl);
                                //Globals.SessionIDClient = server.Id;
                                Globals.ClientSupportsRemoteControl = server.SupportsRemoteControl;
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            
            catch (Exception ex)
            {
                Log("ERROR in Client ID obtaining" + ex);
                return false;
            }
        }

        private void CheckTimerTick(object sender, EventArgs e)
        {
            
            _checkTimer.Interval = (_isConnected) ? ConnectedInterval : DisconnectedInterval;
            var connect = _isConnected;
            _isConnected = CheckConnection();

            Log("EMBY -- CheckTimerTick: IsConnected:" + _isConnected + " connect:" + connect);


            if (_isConnected == true && connect == false)
            {
                //if just connected update remote Globals
            //    Globals.EmbyAuthToken = GetEmbyAuthToken(ip, port, user, password);
                Log("EMBY -- CheckTimerTick: Isconnected True and Connect False Updating EMBY Remotes");
                Globals.SessionIDClient = GetYatseInfoPlayingClient();
                Globals.ClientSupportsRemoteControl = GetPlaybackClientSupportsRemote();

            }

            if (!_isConnected || connect == _isConnected) 
            {

                return;


            }

            return;

           /*
            _eventClient.Connect(IP, Convert.ToInt32("0" + XbmcEventServerPort, CultureInfo.InvariantCulture));
            _eventClient.SendHelo("FrontView+ - Remote Control");//, IconType.IconPng, Helper.SkinPath + Helper.Instance.CurrentSkin + @"\Interface\RemoteControl.png");
            */
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
            return IP + ":" + Port ;
            //Changed Path for Emby -
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
                Log("Emby:  Something not configured.");
                return null;


            if (String.IsNullOrEmpty(Globals.EmbyAuthToken))
            {
                Log("Not Emby Token - Not checking for clients.");
                return 0;
                // Not Connected - failed Setup.
                //Still need to check local player there - rather than internet server which gives Auth
            }

            string url = "http://" + IP + ":" + Port + "/";
            Log("Emby COMMAND: Sending to " + url);

            // PMS Server Clients Page - to connect to and see whether local player is in effect.

            try
            {

                var request = WebRequest.Create(url);
                request.Headers.Add("X-Plex-Token", Globals.EmbyAuthToken);
                request.Headers.Add("X-Plex-Client-Identifier", "FrontView");
                request.Headers.Add("X-Plex-Product","FontView");
                request.Headers.Add("X-Plex-Version","1.101");
                var response = request.GetResponse();

                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {

                    // Get the stream containing content returned by the server.
                   // System.IO.Stream dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader.
                 //   System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);
                    Log("Command HTTP Response OK");
                }
                Log("HTTP Status Not Okay - no exception failed - disconnecting");
                return 0;

            }
            catch (Exception ex)
            {
                Log("Cannot send Command ERROr is server details right " + ex);
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
                if (String.IsNullOrEmpty(result))
               
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
