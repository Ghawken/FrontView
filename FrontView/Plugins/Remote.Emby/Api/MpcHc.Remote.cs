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
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace Remote.Emby.Api
{
    public class MpcHcRemote
    {
        private readonly Xbmc _parent;
        private class CommandInfo
        {
            public string Command { get; set; }
            public string Parameter { get; set; }
        }

        public void AsyncCommand(string cmd,string parameter)
        {
            var commandInfo = new CommandInfo { Command = cmd, Parameter = parameter};

            var bw = new BackgroundWorker();
            bw.DoWork += AsyncCommandWorker;
            bw.RunWorkerAsync(commandInfo);
        }
        public void AsyncGeneralCommand(string cmd, string parameter)
        {
            var commandInfo = new CommandInfo { Command = cmd, Parameter = parameter };

            var bw = new BackgroundWorker();
            bw.DoWork += AsyncCommandGeneralWorker;
            bw.RunWorkerAsync(commandInfo);
        }

        private void AsyncCommandGeneralWorker(object sender, DoWorkEventArgs e)
        {
            var commandInfo = (CommandInfo)e.Argument;
            CommandGeneral(commandInfo.Command,commandInfo.Parameter);
        }

        private void AsyncCommandWorker(object sender, DoWorkEventArgs e)
        {
            var commandInfo = (CommandInfo)e.Argument;
            Command(commandInfo.Command, commandInfo.Parameter);
        }


        public bool Command(string cmd,string parameter)
        {
            if (Globals.ClientSupportsRemoteControl == false)
            {
                _parent.Log("-----EMBY COMMAND:   Current Client DOES NOT SUPPORT REMOTE CONTROL -- No Command Sent");
                _parent.Log("-----EMBY COMMAND:  GLobals SupportsRemote " + Globals.ClientSupportsRemoteControl + "Global ClientID:" + Globals.SessionIDClient);
                return false;
            }

            HttpWebRequest request;
            var returnContent = false;
            var authString = _parent.GetAuthString();

            var uri = @"http://" + _parent.IP + ":" + _parent.Port+"/emby/Sessions/"+Globals.SessionIDClient+"/Playing/";
            
            if (!String.IsNullOrEmpty(cmd))
            {
                uri += cmd;
            }


            _parent.Log(" ---------EMBY PLAY COMMAND: TESTING URL:" + uri+":::::");

            try
            {
                request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
                request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
                request.Headers.Add("X-Emby-Authorization", authString);
                request.ContentType = "application/json";

                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] data = encoder.GetBytes("");


                request.ContentLength = data.Length;
                request.Expect = "application/json";

                request.Method = "POST";
                request.Timeout = 1000;
                _parent.Log("Emby COMMAND  : " + cmd);
                _parent.Trace(uri);


                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                            using (var reader = new StreamReader(stream, Encoding.UTF8))
                            {
                                var reqContent = reader.ReadToEnd();
                                _parent.Trace(reqContent);
                                returnContent = true;
                            }
                    }
                }
            }
            catch (WebException e)
            {
                _parent.Log("ERROR - EMBY  Command : " + cmd + " - " + e.Message);
                if (e.Status == WebExceptionStatus.Timeout)
                {

                   // _parent.MpcLoaded = false;
                }
            }
            return returnContent;
        }

        public bool CommandGeneral(string cmd, string parameter)
        {
            HttpWebRequest request;
            var returnContent = false;
            var authString = _parent.GetAuthString();

            var uri = @"http://" + _parent.IP + ":" + _parent.Port + "/emby/Sessions/" + Globals.SessionIDClient + "/Command/";

            if (!String.IsNullOrEmpty(cmd))
            {
                uri += cmd;
            }


            _parent.Log(" ---------EMBY GENERAL COMMAND COMMAND: TESTING URL:" + uri + ":::::");

            try
            {
                request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
                request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
                request.Headers.Add("X-Emby-Authorization", authString);
                request.ContentType = "application/json";

                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] data = encoder.GetBytes("");


                request.ContentLength = data.Length;
                request.Expect = "application/json";

                request.Method = "POST";
                request.Timeout = 1000;
                _parent.Log("Emby COMMAND  : " + cmd);
                _parent.Trace(uri);


                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                            using (var reader = new StreamReader(stream, Encoding.UTF8))
                            {
                                var reqContent = reader.ReadToEnd();
                                _parent.Trace(reqContent);
                                returnContent = true;
                            }
                    }
                }
            }
            catch (WebException e)
            {
                _parent.Log("ERROR - EMBY  Command : " + cmd + " - " + e.Message);
                if (e.Status == WebExceptionStatus.Timeout)
                {

                    // _parent.MpcLoaded = false;
                }
            }
            return returnContent;
        }


        public string GetStatus()
        {
            HttpWebRequest request;
            var returnContent = "";

            var uri = @"http://" + _parent.IP + ":32400/system/players/"+_parent.ClientIPAddress+"/playback";
            try
            {
                request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
                request.Method = "GET";
                request.Timeout = 1000;
                _parent.Trace(uri);
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                            using (var reader = new StreamReader(stream, Encoding.UTF8))
                            {
                                var reqContent = reader.ReadToEnd();
                                _parent.Trace(reqContent);
                                returnContent = reqContent;
                                returnContent = returnContent.Replace("OnStatus(", "").TrimEnd(')').Replace("'", "\"").Replace("\\\"", "'").Replace(", ", ",");
                            }
                    }
                }
            }
            catch (WebException e)
            {
                _parent.Log("ERROR - MPCCOMMAND : Status" + " - " + e.Message);
                if (e.Status == WebExceptionStatus.Timeout)
                {
                       _parent.Log("ERROR - MPCCOMMAND : Web Exception Thrown and MpcLoad now false");  
                      
                }
                
            }
            return returnContent;
        }
        public MpcHcRemote(Xbmc parent)
        {
            _parent = parent;
        }

        public void SetVolume(int volumepercent)
        {
            if (_parent.MpcLoaded)
                AsyncCommand("-2", "volume=" + Convert.ToString(volumepercent, CultureInfo.InvariantCulture));
        }

        public void SeekPercentage(int percent)
        {
            if (_parent.MpcLoaded)
                AsyncCommand("-1", "percent=" + Convert.ToString(percent, CultureInfo.InvariantCulture));
        }

        public void SkipPrevious()
        {
            if (_parent.MpcLoaded)
                AsyncCommand("PreviousTrack", "");
        }

        public void SkipNext()
        {
            if (_parent.MpcLoaded)
                AsyncCommand("NextTrack", "");
        }

        public void ToggleMute()
        {
            AsyncGeneralCommand("ToggleMute", "");
        }

        public void Return()
        {
            if (_parent.MpcLoaded)
                AsyncGeneralCommand("Select", "");
        }

        public void Enter()
        {
            if (_parent.MpcLoaded)
                AsyncGeneralCommand("Select", "");
        }

        public void Info()
        {
            AsyncGeneralCommand("ToggleOsdMenu", "");
        }

        public void Home()
        {
            AsyncGeneralCommand("GoHome", "");
        }

        public void Video()
        {

        }

        public void Music()
        {

        }

        public void Pictures()
        {

        }

        public void Tv()
        {

        }

        public void VolUp()
        {
            AsyncGeneralCommand("VolumeUp", "");
        }

        public void VolDown()
        {
            AsyncGeneralCommand("VolumeDown", "");
        }

        public void Menu()
        {

        }
               
        public void Title()
        {
            if (_parent.MpcLoaded)
                _parent.AsyncEventButton("title");
        }

        public void Down()
        {

        }

        public void Up()
        {
            AsyncGeneralCommand("MoveUp", "");
        }

        public void Left()
        {
            AsyncGeneralCommand("MoveLeft", "");
        }

        public void Right()
        {
            AsyncGeneralCommand("MoveRight", "");
        }

        public void Mute()
        {
            AsyncGeneralCommand("Mute", "");
        }

        public void PlayDrive()
        {

        }
                
        public void EjectDrive()
        {

        }

        public void Subtitles()
        {

        }

        public void Previous()
        {
            AsyncGeneralCommand("Back", "");
        }

        public void Rewind()
        {

        }

        public void Play()
        {
            //if (_parent.MpcLoaded)
               
            
            
            AsyncCommand("Unpause", "");
        }

        public void Pause()
        {
            AsyncCommand("Pause", "");
        }

        public void Stop()
        {
            //if (_parent.MpcLoaded)
                AsyncCommand("Stop", "");
        }

        public void Forward()
        {

        }

        public void Next()
        {

        }

        public void One()
        {
;
        }

        public void Two()
        {

        }

        public void Three()
        {

        }

        public void Four()
        {

        }

        public void Five()
        {

        }

        public void Six()
        {

        }

        public void Seven()
        {

        }

        public void Eight()
        {
        }

        public void Nine()
        {

        }

        public void Zero()
        {

        }

        public void Star()
        {

        }

        public void Hash()
        {

        }

        public static void ParseCSVFields(ArrayList result, string data)
        {
            var pos = -1;
            while (pos < data.Length)
                result.Add(ParseCSVField(data, ref pos));
        }

        private static string ParseCSVField(string data, ref int startSeparatorPosition)
        {
            if (startSeparatorPosition == data.Length - 1)
            {
                startSeparatorPosition++;
                return "";
            }
            var fromPos = startSeparatorPosition + 1;
            if (data[fromPos] == '"')
            {
                if (fromPos == data.Length - 1)
                {
                    return "\"";
                }
                var nextSingleQuote = FindSingleQuote(data, fromPos + 1);
                startSeparatorPosition = nextSingleQuote + 1;
                return data.Substring(fromPos + 1, nextSingleQuote - fromPos - 1).Replace("\"\"", "\"");
            }
            var nextComma = data.IndexOf(',', fromPos);
            if (nextComma == -1)
            {
                startSeparatorPosition = data.Length;
                return data.Substring(fromPos);
            }
            startSeparatorPosition = nextComma;
            return data.Substring(fromPos, nextComma - fromPos);
        }

        private static int FindSingleQuote(string data, int startFrom)
        {
            var i = startFrom - 1;
            while (++i < data.Length)
                if (data[i] == '"')
                {
                    if (i < data.Length - 1 && data[i + 1] == '"')
                    {
                        i++;
                        continue;
                    }
                    return i;
                }
            return i;
        }
    }
}
