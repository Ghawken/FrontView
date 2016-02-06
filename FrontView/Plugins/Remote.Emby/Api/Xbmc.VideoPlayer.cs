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
    class XbmcVideoPlayer : IApiVideoPlayer
    {
        private readonly Xbmc _parent;

        public XbmcVideoPlayer(Xbmc parent)
        {
            _parent = parent;
        }

        public void PlayMovie(ApiMovie video)
        {
            if (video == null)
                return;
            if (!_parent.IsConnected())
                return;

            EmbyPlayPlayList(video.Path);
            _parent.Trace("Attemping to Play IdEpsiode equals" + video.Path);

            /*
            var args = new JsonObject();
            var items = new JsonObject();
            args["movieid"] = video.IdMovie;
            items["item"] = args;
            items["playlistid"] = 1;
            var plId = new JsonObject();
            plId["playlistid"] = 1;
            var item = new JsonObject();
            item["item"] = plId;
            _parent.JsonCommand("Playlist.Clear", plId);
            _parent.JsonCommand("Playlist.Add", items);
            _parent.JsonCommand("Player.Open", item);
            */

        }

        public void PlayTvEpisode(ApiTvEpisode tvepisode)
        {
            if (tvepisode == null)
                return;
            if (!_parent.IsConnected())
                return;

            EmbyPlayPlayList(tvepisode.Path);
            _parent.Log("Attemping to Play IdEpsiode equals: " + tvepisode.Path);

            /*

            var args = new JsonObject();
            var items = new JsonObject();
            args["episodeid"] = tvepisode.IdEpisode;
            items["item"] = args;
            items["playlistid"] = 1;
            var plId = new JsonObject();
            plId["playlistid"] = 1;
            var item = new JsonObject();
            item["item"] = plId;
            _parent.JsonCommand("Playlist.Clear", plId);
            _parent.JsonCommand("Playlist.Add", items);
            _parent.JsonCommand("Player.Open", item);                        
        
             */
       }

        public string EmbyPlayPlayList(string param)
        {
            try
            {

                string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Sessions/" + Globals.SessionIDClient + "/Playing";

                var request = WebRequest.CreateHttp(NPurl);

                request.Method = "post";
                //request.Timeout = 000;
                _parent.Trace("Play Playlist Selection: URL:   " + NPurl);

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
