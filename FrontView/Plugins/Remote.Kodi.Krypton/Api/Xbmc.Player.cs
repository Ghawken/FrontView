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
using System.Linq;
using System.Web.Script.Serialization;
using System.Collections.Generic;

namespace Remote.XBMC.Krypton.Api
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
                if (_parent.MpcLoaded)
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
                                            "streamdetails",
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
                                            "fanart",
                                            "art"
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

                    var clearlogo = "NONE";
                    var banner = "NONE";
                    // go through art and see.

                    JsonObject artresults = (JsonObject)result2["art"];

                    if (artresults != null)
                    {
                        if (artresults["clearlogo"] != null)
                        {
                            clearlogo = artresults["clearlogo"].ToString();
                        }
                        if (artresults["banner"] != null)
                        {
                            banner = artresults["banner"].ToString();
                        }

                    }



                    if (_nowPlaying.MediaType == "video")
                    {
                        if (result2["type"].ToString() == "channel")  //if PVR Needs to go high otherwise exception
                        {
                            _nowPlaying.MediaType = "Pvr";
                            _nowPlaying.IsNewMedia = true;
                            _nowPlaying.FileName = result2["label"].ToString();
                            _nowPlaying.ThumbURL = result2["thumbnail"].ToString();
                            _nowPlaying.FanartURL = result2["fanart"].ToString();
                            _nowPlaying.LogoURL = clearlogo;
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

                        if (_nowPlaying.Track > 0)
                        {
                            _nowPlaying.Track = Convert.ToInt32("0" + result2["track"]);
                        }
                        else
                        {
                            _nowPlaying.Track = 0;
                        }

                        if (_nowPlaying.Year > 0)
                        {
                            _nowPlaying.Year = Convert.ToInt32("0" + result2["year"]);
                        }
                        else
                        {
                            _nowPlaying.Year = 0;
                        }

                        _nowPlaying.Artist = _parent.JsonArrayToString((JsonArray)result2["artist"]);
                        _nowPlaying.Album = result2["album"].ToString();
                        _nowPlaying.ThumbURL = result2["thumbnail"].ToString();
                        _nowPlaying.FanartURL = result2["fanart"].ToString();

                        // add to remove theme files
                        
                        
                        _parent.Log("---THEME MP3 NOWPLAYING FILENAME EQUALS:" + _nowPlaying.FileName);
                        if (_nowPlaying.FileName.EndsWith("theme.mp3"))
                        {
                            _nowPlaying.FileName = "";
                            _nowPlaying.Title = "";
                            _nowPlaying.IsPlaying = false;
                            _nowPlaying.IsPaused = false;

                            _nowPlaying.IsNewMedia = false;
                            _parent.Log("Kodi Remote:   Theme.mp3 playing stopping" + _nowPlaying.FileName);
                            return;


                        }
                       
                    }
                    
                    if (_nowPlaying.MediaType == "video")
                    {
                        _nowPlaying.MediaType = result2["type"].ToString() == "episode" ? "TvShow" : "Movie";

                        JsonObject streamdetails = (JsonObject)result2["streamdetails"];
                        List<string> MovieIcons = new List<string>();
                        MovieIcons = GetMovieIcons(streamdetails);


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
                        _nowPlaying.LogoURL = clearlogo;
                    }
                }
            }
        }
        public List<string> GetMovieIcons(JsonObject streamdetails)
        {



            List<string> MovieIcons = new List<string>();
            _parent.Trace("MovieIcons Generating List:");

            // Make sure not null; 
            MovieIcons.Add("");

            try
            {


                var stringJsonResults = Jayrock.Json.Conversion.JsonConvert.ExportToString(streamdetails);

                _parent.Trace("MovieIcons" + stringJsonResults);
                var deserializer = new JavaScriptSerializer();
                StreamDetails.Rootobject ItemData = deserializer.Deserialize<StreamDetails.Rootobject>(stringJsonResults);





                //Container
                if (ItemData != null)
                {


                    if (ItemData.audio != null)
                    {


                        //Sorry Non-English Speakers defaults to English stream

                        int CountStreams = ItemData.audio.Where(i => i.language == "eng").Count();
                        _parent.Trace("MovieIcons CountStreams : " + CountStreams.ToString());

                        if (CountStreams > 0)
                        {
                            var isDefaultMediaStream = ItemData.audio.FirstOrDefault(i => i.language == "eng");

                            // added check - make sure is default stream being checked
                            // often multiples streams commentary etc with ac3 and other codecs - would not make sense to have all shown

                            try
                            {

                                var MovieCodec = isDefaultMediaStream.codec;

                                if (MovieCodec != null && !string.IsNullOrEmpty(MovieCodec))
                                {
                                    if (MovieCodec.Equals("dca") == true || MovieCodec.Equals("dts"))
                                    {
                                        MovieIcons.Add("DTS");
                                        _parent.Trace("MovieIcons adding DTS Profile: DTS :  Codecequals" + MovieCodec.ToUpper().ToString());
                                    }
                                    else if (MovieCodec.Contains("dts") && !MovieCodec.Equals("dts"))
                                    {
                                        string Profile = MovieCodec.ToString();
                                        Profile = Profile.Replace("dts", "dst");
                                        MovieIcons.Add(Profile.ToUpper());
                                        _parent.Trace("MovieIcons dca adding DST Plus Profile:" + Profile.ToUpper());
                                    }
                                    else
                                    {
                                        MovieIcons.Add(MovieCodec.ToString());
                                        _parent.Trace("MovieIcons Adding Codec:" + MovieCodec.ToString());
                                    }
                                }

                                var Channels = ItemData.audio.Where(i => i.language == "eng").FirstOrDefault().channels;
                                if (Channels > 0)
                                {
                                    MovieIcons.Add("Channels" + Channels.ToString());
                                    _parent.Trace("MovieIcons Adding Channels:" + Channels.ToString());
                                }


                            }
                            catch (Exception ex)
                            {
                                _parent.Trace("MovieIcons Exception Caught Within AudioStream Codec Check:" + ex);
                                return MovieIcons;
                            }

                        }


                        var VideoInfo = ItemData.video.FirstOrDefault();

                        if (VideoInfo != null)
                        {
                            if (!string.IsNullOrWhiteSpace(VideoInfo.codec))
                            {
                                MovieIcons.Add("codec" + VideoInfo.codec.ToString());
                                _parent.Trace("MovieIcons Adding codec" + VideoInfo.codec.ToString());

                            }
                            if (VideoInfo.aspect > 0)
                            {

                                if (VideoInfo.aspect > 0)
                                {
                                    MovieIcons.Add(VideoInfo.aspect.ToString("0.00") + ":1");
                                    _parent.Trace("MovieIcons Adding Ratio:" + VideoInfo.aspect.ToString("0.00") + ":1");
                                }

                            }

                            if (VideoInfo.width.HasValue)
                            {
                                if (VideoInfo.width > 3800)
                                {
                                    MovieIcons.Add("4K");
                                    _parent.Trace("MoviesIcons Adding 4K");
                                }
                                else if (VideoInfo.width >= 1900)
                                {
                                    MovieIcons.Add("1080p");
                                    _parent.Trace("MoviesIcons Adding 1080p");
                                }
                                else if (VideoInfo.width >= 1270)
                                {
                                    MovieIcons.Add("720p");
                                    _parent.Trace("MoviesIcons Adding 720p");
                                }
                                else if (VideoInfo.width >= 700)
                                {
                                    MovieIcons.Add("480P");
                                    _parent.Trace("MoviesIcons Adding 480p");
                                }
                                else
                                {
                                    MovieIcons.Add("SD");
                                    _parent.Trace("MoviesIcons Adding SD");
                                }
                            }
                        }
                    }



                }
            }
            catch (Exception ex)
            {
                _parent.Trace("MovieIcons Exception Caught Within VideoInfo Codec Check:" + ex);
                return MovieIcons;
            }




            return MovieIcons;



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
