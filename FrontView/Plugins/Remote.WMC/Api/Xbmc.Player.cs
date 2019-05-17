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
using System.Globalization;
using Jayrock.Json;
using Plugin;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using System.Text.RegularExpressions;
using Setup;

namespace Remote.WMC.Api
{
    class XbmcPlayer : IApiPlayer
    {
        private readonly Xbmc _parent;
        private string _currentMediaFile;
        private string _currentMediaTitle;
        //public string _currentMenu;
        private readonly ApiCurrently _nowPlaying = new ApiCurrently();

        static readonly object Locker = new object();

        public XbmcPlayer(Xbmc parent)
        {
            _parent = parent;
        }

        public void RefreshNowPlaying()
        {
            lock (Locker)
            {
                /*if (_parent.MpcLoaded)
                {
                    var result = _parent.MpcHcRemote.GetStatus();
                    var result2 = new ArrayList();
                    MpcHcRemote.ParseCSVFields(result2, result);
                    var data = (string[])result2.ToArray(typeof(string));
                    if (data.Length > 6)
                    {
                        _nowPlaying.MediaType = "Video";
                        _nowPlaying.Title = "Media Player Classic";
                        _nowPlaying.Time = new TimeSpan(0, 0, 0, Convert.ToInt32("0" + data[2]) / 1000);
                        _nowPlaying.Duration = new TimeSpan(0, 0, 0, Convert.ToInt32("0" + data[4]) / 1000);
                        var percent = Math.Floor(100.0 * Convert.ToInt32("0" + data[2], CultureInfo.InvariantCulture) / Convert.ToInt32("0" + data[4], CultureInfo.InvariantCulture));
                        if (Double.IsNaN(percent))
                            percent = 0;
                        _nowPlaying.Volume = Convert.ToInt32("0" + data[7], CultureInfo.InvariantCulture);
                        _nowPlaying.IsMuted = data[6] == "1";
                        _nowPlaying.Progress = (int)percent;

                        if (data[1] == "Playing")
                        {
                            _parent.Log("XBMC PLAYER REMOTE:   Playing given Changing NowPlaying to true " + data[1]);
                            _nowPlaying.IsPlaying = true;
                            _nowPlaying.IsPaused = false;
                        }
                        if (data[1] == "Paused")
                        {
                            _parent.Log("XBMC PLAYER REMOTE:   Paused given Changing NowPlaying to true " + data[1]);
                            _nowPlaying.IsPaused = true;
                            _nowPlaying.IsPlaying = !_nowPlaying.IsPaused;
                        }

                    }
                    if (_parent.MpcLoaded == false)
                    {
                        _nowPlaying.FileName = "Glenn MPC Stopped";
                        _nowPlaying.Title = "";
                        _nowPlaying.IsPlaying = false;
                        _nowPlaying.IsPaused = false;
                    }
                }
                else
                {
                    if (!_parent.IsConnected())
                    {
                        _nowPlaying.FileName = "";
                        _nowPlaying.Title = "";
                        _nowPlaying.IsPlaying = false;
                        _nowPlaying.IsPaused = false;
                        _parent.Log("XBMC PLAYER REMOTE:   Returning as no !Player Connected");

                        return;
                    }
                    //_parent.Log("XBMC PLAYER REMOTE:   Check with MPC Doesnt make it here");
                    /*
                */
                /*   
                   var GUIproperties = new JsonObject();
                   GUIproperties["properties"] = new[]
                                                     {
                                                         "currentwindow"
                                                             
                                                      
                                                       };

                   var menuresult = (JsonObject)_parent.JsonCommand("GUI.GetProperties", GUIproperties);
                   var GUIdeeper = (JsonObject)menuresult["currentwindow"];
                   _nowPlaying.CurrentMenuLabel = GUIdeeper["label"].ToString();
                   _nowPlaying.CurrentMenuID = GUIdeeper["id"].ToString();
                    
                                     
                   var current = -1;
                   var players = (JsonArray)_parent.JsonCommand("Player.GetActivePlayers", null);


                   if (players.Count > 0)
                   {
                       foreach (JsonObject player in players)
                       {
                           if (player["type"].ToString() == "picture")
                               continue;
                           current = Int32.Parse(player["playerid"].ToString());
                           _nowPlaying.MediaType = (string)player["type"];
                       }
                   }

                   if (current == -1)
                   {
                       _nowPlaying.FileName = "";
                       _nowPlaying.Title = "";
                       _nowPlaying.IsPlaying = false;
                       _nowPlaying.IsPaused = false;
                       return;
                   }

                   var items = new JsonObject();
                   items["playerid"] = current;
                   items["properties"] = new[]{
                                           "file",
                                           "comment",
                                           "tvshowid",
                                           "albumartist",
                                           "duration",
                                           //"id",
                                           "album",
                                           //"votes",
                                          // "mpaa",
                                          // "writer",
                                          //"albumid",
                                           //"type",
                                           "genre",
                                           "year",
                                           //"plotoutline",
                                           "track",
                                           "artist",
                                           //"season",
                                           //"imdbnumber",
                                          // "studio",
                                           //"showlink",
                                           "showtitle",
                                           "episode",
                                           "season",
                                           "plot",
                                           "director",
                                           "studio",
                                           "rating",
                                           //"productioncode",
                                           //"country",
                                           //"premiered",
                                           //"originaltitle",
                                           //"artistid",
                                           //"firstaired",
                                           "tagline",
                                           "thumbnail",
                                           "fanart"
                                           //"top250",
                                           //"trailer"
                                       };

                   var properties = new JsonObject();
                   properties["playerid"] = current;
                   properties["properties"] = new[]{
                                           "totaltime",
                                           "percentage",
                                           "time",
                                           "speed"
                                       };

                   var appproperties = new JsonObject();
                   appproperties["properties"] = new[]
                                                     {
                                                         "muted",
                                                         "volume"
                                                     };

                   var result1 = (JsonObject)_parent.JsonCommand("Player.GetProperties", properties);
                   var result2 = (JsonObject)_parent.JsonCommand("Player.GetItem", items);
                   var result3 = (JsonObject)_parent.JsonCommand("Application.GetProperties", appproperties);


                   if (result1 == null || result2 == null || result3 == null)
                   {
                       _nowPlaying.FileName = "";
                       _nowPlaying.Title = "";
                       _nowPlaying.IsPlaying = false;
                       _nowPlaying.IsPaused = false;
                       return;
                   }

                   result2 = (JsonObject)(result2)["item"];

                  
                     
                  
                     
                     
                    
                   if (_nowPlaying.MediaType == "video")
                   {
                       if (result2["type"].ToString() == "channel")  //if PVR Needs to go high otherwise exception
                       {
                           _nowPlaying.MediaType = "Pvr";
                           _nowPlaying.IsNewMedia = true;
                           _nowPlaying.FileName = result2["label"].ToString();
                           _nowPlaying.ThumbURL = result2["thumbnail"].ToString();
                           _nowPlaying.FanartURL = result2["fanart"].ToString();
                           _nowPlaying.Title = result2["label"].ToString();
                           _nowPlaying.IsPaused = Convert.ToInt32("0" + result1["speed"].ToString().Replace("-", "")) == 0;
                           _nowPlaying.IsPlaying = !_nowPlaying.IsPaused;
                           var pvrtime = (JsonObject)result1["time"];
                           var pvrtotal = (JsonObject)result1["totaltime"];
                           _nowPlaying.Time = new TimeSpan(0, Convert.ToInt32("0" + pvrtime["hours"]), Convert.ToInt32("0" + pvrtime["minutes"]), Convert.ToInt32("0" + pvrtime["seconds"]));
                           _nowPlaying.Duration = new TimeSpan(0, Convert.ToInt32("0" + pvrtotal["hours"]), Convert.ToInt32("0" + pvrtotal["minutes"]), Convert.ToInt32("0" + pvrtotal["seconds"]));
                           _nowPlaying.Progress = Convert.ToInt32("0" + result1["percentage"].ToString().Split('.')[0]);
                           _nowPlaying.Volume = Convert.ToInt32("0" + result3["volume"]);
                           _nowPlaying.IsMuted = (bool)result3["muted"];
                           return;
                       }
                   }

                   _nowPlaying.IsPaused = Convert.ToInt32("0" + result1["speed"].ToString().Replace("-", "")) == 0;
                   _nowPlaying.IsPlaying = !_nowPlaying.IsPaused;
                   var time = (JsonObject)result1["time"];
                   var total = (JsonObject)result1["totaltime"];
                   _nowPlaying.Time = new TimeSpan(0, Convert.ToInt32("0" + time["hours"]), Convert.ToInt32("0" + time["minutes"]), Convert.ToInt32("0" + time["seconds"]));
                   _nowPlaying.Duration = new TimeSpan(0, Convert.ToInt32("0" + total["hours"]), Convert.ToInt32("0" + total["minutes"]), Convert.ToInt32("0" + total["seconds"]));
                   _nowPlaying.Progress = Convert.ToInt32("0" + result1["percentage"].ToString().Split('.')[0]);
                   _nowPlaying.Volume = Convert.ToInt32("0" + result3["volume"]);
                   _nowPlaying.IsMuted = (bool)result3["muted"];

                   _parent.MpcLoaded = _nowPlaying.Duration == new TimeSpan(0, 0, 0, 1);

                   _nowPlaying.FileName = result2["file"].ToString();

                   if (_nowPlaying.MediaType == "audio")
                   {
                       _nowPlaying.MediaType = "Audio";
                       _nowPlaying.Genre = _parent.JsonArrayToString((JsonArray)result2["genre"]);
                       _nowPlaying.Title = result2["label"].ToString();
                       _nowPlaying.Year = Convert.ToInt32("0" + result2["year"]);
                       _nowPlaying.Track = Convert.ToInt32("0" + result2["track"]);
                       _nowPlaying.Artist = _parent.JsonArrayToString((JsonArray)result2["artist"]);
                       _nowPlaying.Album = result2["album"].ToString();
                       _nowPlaying.ThumbURL = result2["thumbnail"].ToString();
                       _nowPlaying.FanartURL = result2["fanart"].ToString();
                   }
                    
                   if (_nowPlaying.MediaType == "video")
                   {
                       _nowPlaying.MediaType = result2["type"].ToString() == "episode" ? "TvShow" : "Movie";
                        

                        
                       _nowPlaying.Genre = _parent.JsonArrayToString((JsonArray)result2["genre"]);
                       _nowPlaying.Title = result2["label"].ToString();
                       _nowPlaying.Year = Convert.ToInt32("0" + result2["year"]);
                       _nowPlaying.SeasonNumber = Convert.ToInt32("0" + result2["season"].ToString().Replace("-", ""));
                       _nowPlaying.EpisodeNumber = Convert.ToInt32("0" + result2["episode"].ToString().Replace("-", ""));
                       _nowPlaying.ShowTitle = result2["showtitle"].ToString();
                       _nowPlaying.Plot = result2["plot"].ToString();
                       _nowPlaying.Director = _parent.JsonArrayToString((JsonArray)result2["director"]);
                       _nowPlaying.Studio = _parent.JsonArrayToString((JsonArray)result2["studio"]);
                       _nowPlaying.Tagline = result2["tagline"].ToString();
                       _nowPlaying.Rating = result2["rating"].ToString();
                       _nowPlaying.ThumbURL = result2["thumbnail"].ToString();
                       _nowPlaying.FanartURL = result2["fanart"].ToString();
                   }
               */
                if (_parent.IsConnected())
                {
                    try
                    {

                        _parent.Log("Plex: Using Parent IP equals: " + _parent.IP);
                        string NPurl = "http://" + _parent.IP + ":32400/status/sessions";
                        var request = WebRequest.Create(NPurl);


                        request.Headers.Add("X-Plex-Token", _parent.PlexAuthToken);
                        var response = request.GetResponse();

                        if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                        {

                            //  Use MPC Remote

                            _parent.MpcLoaded = true;


                            // Get the stream containing content returned by the server.
                            System.IO.Stream dataStream = response.GetResponseStream();
                            // Open the stream using a StreamReader.
                            System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);

                        
                                                                                 

                            XmlSerializer serializer = new XmlSerializer(typeof(MediaContainer));
                            MediaContainer deserialized = (MediaContainer)serializer.Deserialize(reader);

                            _parent.Log("status/sessions: " + reader.ReadToEnd().ToString()); 
                           
                            var length = Convert.ToInt32(deserialized.size);
                            _parent.Log("Number of playing Videos: " + length);

                            if (length == 0)
                            {
                                _nowPlaying.FileName = "";
                                _nowPlaying.Title = "";
                                _nowPlaying.IsPlaying = false;
                                _nowPlaying.IsPaused = false;
                                _nowPlaying.IsPlaying = false;
                                _nowPlaying.IsNewMedia = false;
                                _parent.Log("Plex Log: Nothing is Playing");
                                return;
                            }

                            //_nowPlaying.IsPlaying = true;
                            //_nowPlaying.IsPaused = false;

                            _nowPlaying.IsNewMedia = true;
                            _nowPlaying.MediaType = "Movie";
                            //_nowPlaying.Title = "Plex Playing";

                            foreach (var server in deserialized.Video)
                            {
                                _parent.Log("Checking against Local Playback only Client IP: " + _parent.ClientIPAddress);
                                _parent.Log("IP Address Playing now are:" + server.Player.address);

                                if (server.Player.address == _parent.ClientIPAddress)
                                {

                                    _parent.Log("Plex: Found Local Playback");

                                    _parent.Log("Plex: server.Art EQUALS ===========" + server.art);

                                    if (!String.IsNullOrEmpty(server.art))
                                    {
                                        if (server.art.StartsWith("http:"))
                                        {
                                            _nowPlaying.FanartURL = server.art;
                                        }
                                        else
                                        {
                                            _nowPlaying.FanartURL = @"http://" + _parent.IP + ":" + _parent.ServerPort + server.art;
                                        }
                                    }
                                    else
                                    {
                                        // If no fanart url use Thumb for background as well
                                        _nowPlaying.FanartURL = String.IsNullOrEmpty(server.thumb) ? "" : @"http://" + _parent.IP + ":" + _parent.ServerPort + server.thumb;
                                    }



                                    _parent.Log("Plex: Fanart URL sorting Out:  " + _parent.IP + ":" + _parent.ServerPort + server.art);
                                    _parent.Log("Plex:  nowPlaying Fanart equals:" + _nowPlaying.FanartURL);




                                        //Console.WriteLine("Grandparent art is {0} and Players is {1}", server.grandparentArt, server.Player);



                                    _nowPlaying.Title = String.IsNullOrEmpty(server.title) ? "Blank" : server.title;

                                    _parent.Log("Plex:NP NowPlaying.title:" + _nowPlaying.Title);


                                    //    Console.WriteLine("" + server.art);
                                    //    Console.WriteLine("" + server.chapterSource);
                                    //_nowPlaying.Director = server.Director.tag;
                                    //     Console.WriteLine("" + server.duration);
                                    //    Console.WriteLine("" + server.grandparentArt);


                                    _nowPlaying.ShowTitle = String.IsNullOrEmpty(server.grandparentTitle) ? "Blank" : server.grandparentTitle;
                                    _parent.Log("Plex:NP NowPlaying.Showtitle:" + _nowPlaying.ShowTitle);

                                    //     Console.WriteLine("" + server.grandparentThumb);
                                    /*     Console.WriteLine("" + server.guid);
                                         Console.WriteLine("" + server.index);
                                         Console.WriteLine("" + server.indexString);
                                         Console.WriteLine("" + server.key);
                                         Console.WriteLine("" + server.lastViewedAt);
                                         Console.WriteLine("Filename: " + server.Media.Part.file);
                                     //    Console.WriteLine("" + server.Media.Part.duration);
                                   // */
                                    //     Console.WriteLine("Player Product: " + server.Player.product);



                                    _nowPlaying.Plot = String.IsNullOrEmpty(server.summary) ? "" : server.summary;
                                    _parent.Log("Plex:NP NowPlaying.Plot:" + _nowPlaying.Plot);

                                    _parent.Log("Plex:NP NowPlaying.ThumbURL:" + @"http://" + _parent.IP + ":" + _parent.ServerPort + server.thumb);


                                    _nowPlaying.ThumbURL = String.IsNullOrEmpty(server.thumb)? "" : @"http://" + _parent.IP + ":" + _parent.ServerPort + server.thumb;




                                    _nowPlaying.FileName = String.IsNullOrEmpty(server.Media.Part.file) ? "NotGiven" : server.Media.Part.file;
                                    _parent.Log("Plex:NP NowPlaying.FileName:" + _nowPlaying.FileName);




                                    _nowPlaying.Title = String.IsNullOrEmpty(server.title) ? "" : server.title;
                                    _parent.Log("Plex:NP NowPlaying.Title:" + _nowPlaying.Title);


                                    // Make changes here to recognise ts recordings as TV and use regex to populate season/episode data



                                    _nowPlaying.MediaType = server.type == "episode" ? "TvShow" : "Movie";
                                    _parent.Log("Plex:NP NowPlaying.MediaType:" + _nowPlaying.MediaType);





                                    if (Convert.ToUInt64(server.Media.duration) > 0)
                                    {
                                        // duration for Plex given in millseconds - convert to seconds and round
                                        // Convert Duration to Timespan with seconds only
                                        var RoundSeconds = Math.Round(Convert.ToInt64(server.Media.duration) / 1000.00, 1);

                                        _nowPlaying.Duration = new TimeSpan(0, Convert.ToInt32("0"), Convert.ToInt32("0"), Convert.ToInt32(RoundSeconds));
                                    }

                                    _parent.Log("Plex:NP server.Media.Duration:" + server.Media.duration + ":  _nowPlaying.Duration (calculated) :" + _nowPlaying.Duration);

                                    var RoundOffset = Math.Round(Convert.ToInt64(server.viewOffset) / 1000.00, 1);
                                    _nowPlaying.Time = new TimeSpan(0, 0, 0, Convert.ToInt32(RoundOffset));

                                    _parent.Log("Plex:NP NowPlaying.Time:" + _nowPlaying.Time + "Calcuated on server.viewOffset:" + server.viewOffset);

                                    var percent = Math.Floor(100.0 * Convert.ToInt32("0" + server.viewOffset, CultureInfo.InvariantCulture) / Convert.ToInt32("0" + server.Media.duration, CultureInfo.InvariantCulture));
                                    if (Double.IsNaN(percent))
                                    {
                                        percent = 0;
                                    }
                                    _nowPlaying.Progress = (int)percent;

                                    _parent.Log("Plex:NP NowPlaying.Progress:" + _nowPlaying.Progress);

                                    //_nowPlaying.FirstAired = server.originallyAvailableAt;

                                    if (server.type == "episode")
                                    {
                                        _nowPlaying.EpisodeNumber = Convert.ToInt32(server.index);
                                        _nowPlaying.SeasonNumber = Convert.ToInt32(server.parentIndex);


                                    }

                                    _parent.Log("Plex:NP server.Player.state:" + server.Player.state);


                                    if (server.Player.state == "paused")
                                    {
                                        _nowPlaying.IsPaused = true;
                                        _nowPlaying.IsPlaying = false;
                                    }
                                    if (server.Player.state == "playing")
                                    {
                                        _nowPlaying.IsPaused = false;
                                        _nowPlaying.IsPlaying = true;
                                    }

                                    if (server.Player.state == "buffering")
                                    {
                                        _nowPlaying.IsPaused = false;
                                        _nowPlaying.IsPlaying = true;
                                    }

                                    _nowPlaying.LogoURL = "";
                                    _nowPlaying.MovieIcons = "";

                                    _parent.Log("Plex Remote:  Filename" + _nowPlaying.FileName + " IsPlaying :" + _nowPlaying.IsPlaying + " IsPaused :" + _nowPlaying.IsPaused + " MediaType :" + _nowPlaying.MediaType);

                                    // check for endwith ts filename
                                    // actually check for mpegts
                                    // may be recorded movies in mpegts format as well.....
                                    // check for regex s00e00 contents - and when both present swap

                                    // Change of plans - run for all files regardless of type and regardless of mediacontainer
                                    

                                    if (1==1)     //(server.Media.container == "mpegts" && server.type == "movie")
                                    {
                                        _parent.Log("Plex Remote:  New changing recorded TV to TV Shows and extracting season/episode data");

                                        Regex regex = new Regex(@"[Ss](?<season>\d{1,2})[Ee](?<episode>\d{1,2})");

                                        _parent.Log("Plex Remote:  Server Title Equals:" + server.title);

                                        if (server.title != null)
                                        {
                                            Match match = regex.Match(server.title);
                                            if (match.Success)
                                            {
                                                _nowPlaying.MediaType = "TvShow";
                                                _nowPlaying.SeasonNumber = Convert.ToInt32(match.Groups["season"].Value);
                                                _nowPlaying.EpisodeNumber = Convert.ToInt32(match.Groups["episode"].Value);
                                                _parent.Log("Plex Remote: Regex TV Conversion: From :" + server.title);


                                                //string replacement = "`$``";
                                                //string newtitle = replacement;
                                                // string newtitle = regex.Replace(server.title, replacement);

                                                string[] lines = Regex.Split(server.title, @"[Ss](?<season>\d{1,2})[Ee](?<episode>\d{1,2})");
                                                string title1 = lines[0];
                                                string newtitle = title1.Replace(".", " ");
                                                _parent.Log("Plex Remote:  New title equals :" + newtitle);
                                                //_nowPlaying.ShowTitle = newtitle;
                                                _nowPlaying.Title = newtitle;
                                            }

                                        }
                                    }



                                    return;
                                }

                            }
                                // if no local Client Playback then nothing Playing
                                _nowPlaying.FileName = "";
                                _nowPlaying.Title = "";
                                _nowPlaying.IsPlaying = false;
                                _nowPlaying.IsPaused = false;
                                _nowPlaying.IsPlaying = false;
                                _nowPlaying.IsNewMedia = false;
                                _parent.Log("Plex Log 2nd: Nothing is Playing");
                                return;

                            }

                       
                    }
                    catch (Exception ex)
                    {
                        _parent.Log("Exception in NowPlaying Plex System" + ex);
                    }






                }
            }
        }

        public ApiCurrently NowPlaying(bool checkNewMedia)
        {
            lock (Locker)
            {
                if (checkNewMedia)
                {
                    _nowPlaying.IsNewMedia = false;
                    if (_currentMediaFile != _nowPlaying.FileName || (_currentMediaTitle != _nowPlaying.Title))
                    {
                        _currentMediaTitle = _nowPlaying.Title;
                        _currentMediaFile = _nowPlaying.FileName;
                        _nowPlaying.IsNewMedia = true;
                    }
                }

                return _nowPlaying;
            }
        }

        public void PlayPause()
        {
            if (_parent.MpcLoaded)
                _parent.MpcHcRemote.Play();
            else
                if (_parent.IsConnected())
                    _parent.AsyncEventAction("PlayerControl(Play)");
        }

        public void Stop()
        {
            if (_parent.MpcLoaded)
                _parent.MpcHcRemote.Stop();
            else
                if (_parent.IsConnected())
                    _parent.AsyncEventAction("PlayerControl(Stop)");
        }

        public void SkipPrevious()
        {
            if (_parent.MpcLoaded)
                _parent.MpcHcRemote.SkipPrevious();
            else
                if (_parent.IsConnected())
                    _parent.AsyncEventAction("PlayerControl(Previous)");
        }

        public void SkipNext()
        {
            if (_parent.MpcLoaded)
                _parent.MpcHcRemote.SkipNext();
            else
                if (_parent.IsConnected())
                    _parent.AsyncEventAction("PlayerControl(Next)");
        }

        public void SeekPercentage(int progress)
        {
            if (_parent.MpcLoaded)
                _parent.MpcHcRemote.SeekPercentage(progress);
            else
                if (_parent.IsConnected())
                {
                    var players = (JsonArray)_parent.JsonCommand("Player.GetActivePlayers", null);
                    if (players.Count > 0)
                    {
                        foreach (JsonObject player in players)
                        {
                            if (player["type"].ToString() == "picture")
                                continue;
                            var current = Int32.Parse(player["playerid"].ToString());
                            var par = new JsonObject();
                            par["playerid"] = current;
                            par["value"] = progress;
                            _parent.AsyncJsonCommand("Player.Seek", par);
                        }
                    }

                    _parent.JsonCommand(
                        _nowPlaying.MediaType == "Audio" ? "AudioPlayer.SeekPercentage" : "VideoPlayer.SeekPercentage",
                        progress);
                }
        }

        public void SetVolume(int percentage)
        {
            if (_parent.MpcLoaded)
                _parent.MpcHcRemote.SetVolume(percentage);
            else
                if (_parent.IsConnected())
                    _parent.AsyncEventAction("SetVolume(" + Convert.ToString(percentage, CultureInfo.InvariantCulture) + ")");
        }

        public void ToggleMute()
        {
            if (_parent.MpcLoaded)
                _parent.MpcHcRemote.ToggleMute();
            else
                if (_parent.IsConnected())
                    _parent.AsyncEventAction("Mute");
        }
    }
}
