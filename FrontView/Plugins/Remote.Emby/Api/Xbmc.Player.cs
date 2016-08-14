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
using Setup;

namespace Remote.Emby.Api
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
               if (!_parent.IsConnected())
               {
                        _nowPlaying.FileName = "";
                        _nowPlaying.Title = "";
                        _nowPlaying.IsPlaying = false;
                        _nowPlaying.IsPaused = false;
                        _parent.Log("Emby PLAYER REMOTE:   Returning as no !Player Connected");
                        
                        return;
               }



                try
                {  

                    _parent.Trace("Emby: Using Parent IP equals: " + _parent.IP);
                    string NPurl = "http://" + _parent.IP + ":" + _parent.Port;
                    var request = WebRequest.CreateHttp(NPurl + "/FrontView");

                    request.Method = "get";
                    //request.Timeout = 5000;
                    _parent.Log("--------------- PLAYER CONNECTION: IP " + _parent.IP + ":" + _parent.Port);



                    var authString = _parent.GetAuthString();



                    _parent.Log("------------------- Username Parent :" + _parent.UserName);
                    _parent.Log("------------------- CurrentUserID Parent :" + _parent.CurrentUserID);
                    _parent.Log("------------------- EMBY TOKEN EQUALS :" + Globals.EmbyAuthToken);

                    //_parent.Log("AuthString " + authString);


                    request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);


                    request.KeepAlive = false;
                    request.Headers.Add("Authorization", authString);
                    request.ContentType = "application/json; charset=utf-8";
                    //  request.ContentLength = postArg.Length;
                    request.Accept = "application/json";


                    var response = request.GetResponse();


                    if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                    {

                        //  Use MPC Remote

                        _parent.MpcLoaded = true;


                        System.IO.Stream dataStream = response.GetResponseStream();

                        //REMOVETHIS                           System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);

                        using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                        {
                            string json = sr.ReadToEnd();
                            _parent.Trace("--------------Using FrontView Emby Plugin Data   ------" + json);
                            var deserializer = new JavaScriptSerializer();

                            var server = deserializer.Deserialize<EmbyServerPlugin.ApiInfo>(json);
                            _parent.Trace("------------- FrontView Emby Plugin: Now Checking Results :results.Count:" + server.Filename);
                            




                                //Check Something is Playing and Return
                                //If Doesnt return - something must be playing.
                                _parent.Trace("------------- FrontView Emby Plugin:  server.Filename equals:" + server.Filename);
                                if (String.IsNullOrEmpty(server.Filename))
                                {
                                    _nowPlaying.FileName = "";
                                    _nowPlaying.Title = "";
                                    _nowPlaying.IsPlaying = false;
                                    _nowPlaying.IsPaused = false;
                                    _nowPlaying.IsPlaying = false;
                                    _parent.Trace("--------------EMBY NOW PLAYING Log: Nothing is Playing");
                                    response.Close();
                                    return;
                                }
                            // OK - Ignore these filenames theme.mp3 for one
                            // ? Video Backgrounds as well will figure out
                            //
                                if (server.Filename.EndsWith("theme.mp3") || server.Filename.EndsWith("theme.mp4"))
                                {
                                    _nowPlaying.FileName = "";
                                    _nowPlaying.Title = "";
                                    _nowPlaying.IsPlaying = false;
                                    _nowPlaying.IsPaused = false;
                                    _nowPlaying.IsPlaying = false;
                                    _parent.Trace("--------------Emby Now Playing Theme Video NowPlaying IGNORE This. Filename:"+server.Filename);
                                    response.Close();
                                    return;
                                }

                                //Something must be playing- at least something with a filename
                                //We will find out how full proof that is later.
                                
                                //_nowPlaying.IsNewMedia = true;
                                
                                _parent.Trace("-------------- Emby Emby Plugin:  IsNewMedia:" + _nowPlaying.IsNewMedia);
                                if (server.Filename != null)
                                {



                                    if (server.IsPaused == true)
                                    {
                                        _nowPlaying.IsPlaying = false;
                                        _nowPlaying.IsPaused = true;
                                    }
                                    if (server.IsPaused == false)
                                    {
                                        _nowPlaying.IsPlaying = true;
                                        _nowPlaying.IsPaused = false;
                                    }

                                    if (server.MediaType == "Audio")
                                    {
                                        _nowPlaying.MediaType = "Audio";

                                        _nowPlaying.Album = server.Album;
                                        _nowPlaying.Year = Convert.ToInt32("0" + server.Year);
                                        _nowPlaying.Track = Convert.ToInt32("0" + server.EpisodeNumber);

                                        _nowPlaying.Title = server.Title;
                                        _nowPlaying.FileName = server.Filename;
                                        _parent.Log("------------- EMBY Trying to get Audio Images");
                                        _parent.Log("------------- EMBY IMAGES: FanartURL " + "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + server.PrimaryItemId + "/Images/Backdrop");

                                        _nowPlaying.FanartURL = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + server.BackdropItemId + "/Images/Primary";

                                        _nowPlaying.ThumbURL = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + server.PrimaryItemId + "/Images/Primary";
                                       
                                        _nowPlaying.Artist = server.Artist;

                                    }


                                    if (server.MediaType == "Episode")
                                    {
                                        _nowPlaying.MediaType = "TvShow";
                                        _nowPlaying.EpisodeNumber = Convert.ToInt32("0" + server.EpisodeNumber);
                                        _nowPlaying.SeasonNumber = Convert.ToInt32("0" + server.SeasonNumber);
                                        _nowPlaying.ShowTitle = String.IsNullOrEmpty(server.ShowTitle) ? "" : server.ShowTitle;
                                        _nowPlaying.Title = String.IsNullOrEmpty(server.Title) ? "Blank" : server.Title;
                                        _nowPlaying.Plot = String.IsNullOrEmpty(server.Overview) ? "" : server.Overview ;
                                        _nowPlaying.FileName = server.Filename;  //Filename
                                        _parent.Log("------------- EMBY Trying to get Images");
                                        _parent.Log("------------- EMBY IMAGES: FanartURL " + "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + server.PrimaryItemId + "/Images/Backdrop");
                                        _nowPlaying.FanartURL = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + server.BackdropItemId + "/Images/Backdrop";
                                        _nowPlaying.ThumbURL = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + server.PrimaryItemId + "/Images/Primary";
                                   //     _nowPlaying.LogoURL = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + server.BackdropItemId + "/Images/Logo";
                                }

                                    if (server.MediaType == "Movie")
                                    {
                                        _nowPlaying.MediaType = "Movie";
                                        //_nowPlaying.EpisodeNumber = server.NowPlayingItem.IndexNumber;
                                        // _nowPlaying.SeasonNumber = server.NowPlayingItem.ParentIndexNumber;
                                        // _nowPlaying.ShowTitle = server.NowPlayingItem.SeriesName;
                                        _nowPlaying.Title = server.Title;
                                        _nowPlaying.Plot = server.Overview;
                                        _nowPlaying.Director = server.Director;
                                        _nowPlaying.Rating = server.Rating;
                                        _nowPlaying.FileName = server.Filename; 
                                        _parent.Log("------------- EMBY Trying to get Images");
                                        _parent.Log("------------- EMBY IMAGES: FanartURL " + "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + server.PrimaryItemId + "/Images/Backdrop");
                                        _nowPlaying.FanartURL = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + server.PrimaryItemId + "/Images/Backdrop";
                                        _nowPlaying.ThumbURL = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + server.PrimaryItemId + "/Images/Primary";
                                        _nowPlaying.LogoURL = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + server.PrimaryItemId + "/Images/Logo";

                                }

                                    if (server.MediaType == "Video")
                                    {
                                        _nowPlaying.MediaType = "Movie";
                                        //_nowPlaying.EpisodeNumber = server.NowPlayingItem.IndexNumber;
                                        // _nowPlaying.SeasonNumber = server.NowPlayingItem.ParentIndexNumber;
                                        // _nowPlaying.ShowTitle = server.NowPlayingItem.SeriesName;
                                        _nowPlaying.Title = server.Title;
                                        _nowPlaying.Plot = server.Overview;
                                        _nowPlaying.Director = server.Director;
                                        _nowPlaying.Rating = server.Rating;
                                        _nowPlaying.FileName = server.Filename; 
                                        _parent.Log("------------- EMBY Trying to get Images");
                                        _parent.Log("------------- EMBY IMAGES: FanartURL " + "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + server.PrimaryItemId + "/Images/Backdrop");
                                        _nowPlaying.FanartURL = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + server.PrimaryItemId + "/Images/Backdrop";
                                        _nowPlaying.ThumbURL = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + server.PrimaryItemId + "/Images/Primary";
                                    }

                                    if (server.MediaType == "ChannelVideoItem" || server.MediaType == "Trailer")
                                    {
                                        _nowPlaying.MediaType = "Movie";
                                        //_nowPlaying.ShowTitle = server.NowPlayingItem.SeriesName;
                                        _nowPlaying.Title = server.Title;
                                        _nowPlaying.FileName = server.Filename;  //No Filename as yet try ID
                                        _parent.Log("------------- EMBY Trying to get Images");
                                        _parent.Log("------------- EMBY IMAGES: FanartURL " + "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + server.PrimaryItemId + "/Images/Backdrop");
                                        _nowPlaying.FanartURL = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + server.PrimaryItemId + "/Images/Backdrop";
                                        _nowPlaying.ThumbURL = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + server.PrimaryItemId + "/Images/Primary";
                                    }

                                    var Seconds = Convert.ToInt64(server.Duration);
                                    var TimePosition = Convert.ToInt64(server.TimePosition);
                                    var RoundSeconds = Math.Round(Seconds / 10000000.00, 1);
                                    var RoundTime = Math.Round(TimePosition / 10000000.00, 2);
                                    //_parent.Log("--------------TIME CONVERSION BUGGER: RoundSeconds:" + RoundSeconds + " Orginal Time RunTimeTicks:"+server.NowPlayingItem.RunTimeTicks);
                                    
                                    _nowPlaying.Duration = new TimeSpan(0, 0, 0, Convert.ToInt32(RoundSeconds));

                                    _nowPlaying.Time = new TimeSpan(0, 0, 0, Convert.ToInt32(RoundTime));


                                    double percent = (100.0 * TimePosition) / Seconds;
                                    percent = Math.Round(percent, 0);

                                    //Change to Primary Image - seems to be better format.
                                    _parent.Log("Percent of Time equals:" + percent);
                                    if (Double.IsNaN(percent))
                                        percent = 0;

                                    _nowPlaying.Progress = (int)percent;

                                    _nowPlaying.IsMuted = server.IsMuted;
                                    _nowPlaying.Volume = Convert.ToInt32(server.Volume);

                                    
                                    _nowPlaying.Tagline = server.Tagline;
                                    _nowPlaying.Studio = server.Studio;
                                    if (server.AirDate.HasValue)
                                    {
                                        _nowPlaying.FirstAired = server.AirDate.Value;
                                    }
                                    response.Close();
                                    return;


                                }








                        }
                    }
                    else   //Hopefully is HTTP Status Not OK
                    {
                        _nowPlaying.FileName = "";
                        _nowPlaying.Title = "";
                        _nowPlaying.IsPlaying = false;
                        _nowPlaying.IsPaused = false;
                        _nowPlaying.IsPlaying = false;
                        _parent.Trace("--------------EMBY NOW PLAYING Log: Nothing is Playing");
                        response.Close();
                        return;
                    }

                }
                catch (Exception ex)
                {
                    _parent.Log("Exception in NowPlaying EMBY System" + ex);
                    _parent.MpcLoaded = false;
                   
                    return;
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
                    _parent.Trace("--------------NowPlaying API checkNewMedia: currentMediafile:"+_currentMediaFile+" nowPlaying.Filename"+_nowPlaying.FileName+" currentMediaTitle:"+_currentMediaTitle+" nowPlaying.Title"+_nowPlaying.Title);
                    
                    if (_currentMediaFile != _nowPlaying.FileName || (_currentMediaTitle != _nowPlaying.Title))
                    {
                        _currentMediaTitle = _nowPlaying.Title;
                        _currentMediaFile = _nowPlaying.FileName;
                        _parent.Trace("------------------ EMBY NowPlaying API:  Is New Media now true");
                        _nowPlaying.IsNewMedia = true;
                    }
                }

                return _nowPlaying;
            }
        }

        public void PlayPause()
        {
            // if (_parent.MpcLoaded)
            if (_nowPlaying.IsPlaying == true)
            {
                _parent.MpcHcRemote.Pause();
            }
            else
            {
                _parent.MpcHcRemote.Play();
            }
                //else
             //   if (_parent.IsConnected())
              //      _parent.AsyncEventAction("PlayerControl(Play)");
        }

        public void Stop()
        {
           // if (_parent.MpcLoaded)
                _parent.MpcHcRemote.Stop();
           // else
             //   if (_parent.IsConnected())
               //     _parent.AsyncEventAction("PlayerControl(Stop)");
        }

        public void SkipPrevious()
        {
            //if (_parent.MpcLoaded)
                _parent.MpcHcRemote.SkipPrevious();
            //else
              //  if (_parent.IsConnected())
                //    _parent.AsyncEventAction("PlayerControl(Previous)");
        }

        public void SkipNext()
        {
           // if (_parent.MpcLoaded)
                _parent.MpcHcRemote.SkipNext();
            //else
              //  if (_parent.IsConnected())
              //      _parent.AsyncEventAction("PlayerControl(Next)");
        }

        public void SeekPercentage(int progress)
        {
            if (_parent.MpcLoaded)
            {
                //     Convert Percentage back to Ticks by multiplication by Duration- should be in ticks.
                decimal percentage = progress / 100m;
                //var tempdata = Convert.ToInt64(_nowPlaying.Duration.Ticks);

                var Place = _nowPlaying.Duration.Ticks * percentage;

                var PlaceLong = Convert.ToInt64(Place);

                _parent.MpcHcRemote.SeekPercentage(PlaceLong);
            }
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
