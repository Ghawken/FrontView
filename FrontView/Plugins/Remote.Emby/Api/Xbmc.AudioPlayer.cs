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

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using Jayrock.Json;
using Plugin;


using System;
using System.Net;
using System.Web.Script.Serialization;
using System.Web;
using System.Web.Script;
using System.Text;
using System.Collections.Generic;

namespace Remote.Emby.Api

{
    class XbmcAudioPlayer : IApiAudioPlayer
    {
        private readonly Xbmc _parent;
        //private readonly BackgroundWorker _bw = new BackgroundWorker{WorkerSupportsCancellation = true};

        public XbmcAudioPlayer(Xbmc parent)
        {
            _parent = parent;
            //_bw.DoWork += AsyncPlayFilesWorker;
        }

        public void AsyncPlayFiles(Collection<ApiAudioSong> songs)
        {
           // var songs = (Collection<ApiAudioSong>)e.Argument;


            _parent.Trace("-----------PLAYLIST Songs:  Trying to play songs #:" + songs.Count + "and Globals.SessionID:" + Globals.SessionIDClient);
            if (songs == null)
                return;
            if (!_parent.IsConnected())
                return;


            StringBuilder stringlistIds = new StringBuilder();



            foreach (var apiAudioSong in songs)
            {
                stringlistIds.Append(apiAudioSong.Path).Append(",");
            }

            string ListItems = stringlistIds.ToString(0, stringlistIds.Length-1);

            
            _parent.Trace("PLAYFILES Attempting to Play :" + ListItems);
            EmbyPlayPlayList(ListItems);



            /*
            _bw.CancelAsync();
            while (_bw.IsBusy)
            {
                Thread.Sleep(50);
                System.Windows.Forms.Application.DoEvents();
            }
            _bw.RunWorkerAsync(songs);
             */
        }

        public void AsyncPlayFilesWorker(Collection<ApiAudioSong> songs)
        {
           // var songs = (Collection<ApiAudioSong>)e.Argument;
            
            if (songs == null)
                return;
            if (!_parent.IsConnected())
                return;

            
            var stringlistIds="";


 
            foreach (var apiAudioSong in songs)
            {

               
                    stringlistIds = apiAudioSong.Path;
                

            }

            _parent.Trace("Attemping to Play IdEpsiode equals: " + stringlistIds);
            EmbyPlayPlayList(stringlistIds);

        }

        public void PlayFiles(Collection<ApiAudioSong> songs)
        {
            if (songs == null)
                return;
            if (!_parent.IsConnected())
                return;

            AsyncPlayFiles(songs);
        }

        public void PlaySong(ApiAudioSong audio)
        {
            if (audio == null)
                return;
            if (!_parent.IsConnected())
                return;

            EmbyPlayPlayList(audio.Path);

        }

        public string EmbyPlayPlayList(string param)
        {
            try
            {

                string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Sessions/" + Globals.SessionIDClient + "/Playing";
                var request = WebRequest.CreateHttp(NPurl);
                request.Method = "post";
                _parent.Trace("Play Playlist Selection: URL:   " + NPurl + "and String param:"+param);
                ASCIIEncoding encoding = new ASCIIEncoding();
                var postData = new Dictionary<string, string>();
                postData["ItemIds"] = param.ToString();
                postData["StartPositionTicks"] = "0";
                postData["PlayCommand"] = "PlayNow";
                var postArg = Jayrock.Json.Conversion.JsonConvert.ExportToString(postData);
                byte[] data = encoding.GetBytes(postArg);
                var authString = _parent.GetAuthString();
                request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
                request.Headers.Add("X-Emby-Authorization", authString);
                request.ContentType = "application/json; charset=utf-8";
                request.ContentLength = postArg.Length;
                request.Accept = "application/json; charset=utf-8";
                var response3 = request.GetRequestStream();
                response3.Write(data, 0, data.Length);

                var response = request.GetResponse();
                _parent.Trace("PlayList Play Response:");

                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {

                    System.IO.Stream dataStream = response.GetResponseStream();
                    //REMOVETHIS                       System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);

                    using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        string json = sr.ReadToEnd();
                        _parent.Trace("--------------GETTING PlayList Json Result ------" + json);

                    }
                }


                return null;
            }
            catch (Exception ex)
            {
                _parent.Trace("ERROR in PlayList Play Selection obtaining: " + ex);
                return "";

            }
        }



    }
}
