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
//using System.Collections;
using System.Globalization;
using Jayrock.Json;
using Plugin;
//using System.Web.Script.Serialization;
//using System.Xml;
using System.Xml.Serialization;
using System.Net;
//using System.Text.RegularExpressions;
//using Setup;
using System.IO;
using System.Collections.Generic;
//using System.Diagnostics;
//using System.Text;

namespace Remote.Jriver.Api
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


        public string ReplaceInvalidChars(string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }


        public void RefreshNowPlaying()
        {
            lock (Locker)
            {
                if (_parent.IsConnected())
                {
                    try
                    {

                        _parent.Log("JRiver: Commencing NowPlaying Check for NowPlaying Screen Update");
                        string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/MCWS/v1/Playback/Info?Zone=-1"+"&Token="+_parent.JRiverAuthToken;
                        var request = WebRequest.Create(NPurl);
                        //var newPlayback = false;

                        request.Headers.Add("Token", _parent.JRiverAuthToken);
                        var response = request.GetResponse();

                        if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                        {
                            //  Use MPC Remote
                            _parent.MpcLoaded = true;
                            // Get the stream containing content returned by the server.
                            System.IO.Stream dataStream = response.GetResponseStream();
                            // Open the stream using a StreamReader.
                            System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);
                            XmlSerializer serializer = new XmlSerializer(typeof(Jriver.Api.InfoZone.Response));
                            var deserialized = (Jriver.Api.InfoZone.Response)serializer.Deserialize(reader);
                            //_parent.Log("InfoZone: " + reader.ReadToEnd().ToString());
                            var playbackState = getItemName(deserialized, "State");

                            _parent.Log("Jriver: Player State:" + playbackState);

                            if (playbackState == "1")
                            {
                                _nowPlaying.IsPaused = true;
                                _nowPlaying.IsPlaying = false;
                            }
                            if (playbackState == "2")
                            {
                                _nowPlaying.IsPaused = false;
                                _nowPlaying.IsPlaying = true;
                            }
                            if (playbackState == "0")
                            {
                                _nowPlaying.FileName = "";
                                _nowPlaying.Title = "";
                                _nowPlaying.IsPaused = false;
                                _nowPlaying.IsPlaying = false;
                               // _nowPlaying.FanartURL = "";
                               // _nowPlaying.ThumbURL = "";
                               // _nowPlaying.IsNewMedia = false;
                                _parent.Log("JRiver Log: Nothing is Playing");
                                return;
                            }

                            _nowPlaying.Title = String.IsNullOrEmpty(getItemName(deserialized, "Name")) ? "Blank" : getItemName(deserialized, "Name");
                            _parent.Log("JRiver:NP NowPlaying.title:" + _nowPlaying.Title);
                            var fileFields = getFileInfo(getItemName(deserialized, "FileKey"));
                            _nowPlaying.FileName = String.IsNullOrEmpty(getFieldValue(fileFields, "Filename")) ? "NotGiven" : getFieldValue(fileFields, "Filename");

                            if (_nowPlaying.IsNewMedia)
                            {
                                _parent.Log("JRiver:  newPlayback is True:");
                                //        //    Console.WriteLine("" + server.art);
                                //        //    Console.WriteLine("" + server.chapterSource);
                                _nowPlaying.Director = String.IsNullOrEmpty(getFieldValue(fileFields, "Director")) ? "Unknown" : getFieldValue(fileFields, "Director"); ;
                                _parent.Log("JRiver:NP NowPlaying.Director:" + _nowPlaying.Director);
                                //        //     Console.WriteLine("" + server.duration);
                                //        //    Console.WriteLine("" + server.grandparentArt);
                                _nowPlaying.Plot = String.IsNullOrEmpty(getFieldValue(fileFields, "Tag Line")) ? "" : getFieldValue(fileFields, "Tag Line");
                                _parent.Log("JRiver:NP NowPlaying.Plot:" + _nowPlaying.Plot);
                                _parent.Log("JRiver:NP NowPlaying.FileName:" + _nowPlaying.FileName);

                                //        _nowPlaying.Title = String.IsNullOrEmpty(server.title) ? "" : server.title;
                                //        _parent.Log("Plex:NP NowPlaying.Title:" + _nowPlaying.Title);
                                //        // Make changes here to recognise ts recordings as TV and use regex to populate season/episode dat

                                var mediaSubType = getFieldValue(fileFields, "Media Sub Type");
                                var mediaType = getFieldValue(fileFields, "Media Type");

                                _parent.Log("JRiver: mediaSubType:" + mediaSubType);
                                _parent.Log("JRiver: mediaType:" + mediaType);

                                if (mediaType == "Video")
                                {
                                    if (mediaSubType == "TV Show")
                                    {
                                        _nowPlaying.MediaType = "TvShow";
                                    }
                                    else
                                    {
                                        _nowPlaying.MediaType = "Movie";
                                    }
                                }
                                else if (mediaType == "Audio")
                                {
                                    _nowPlaying.MediaType = "Audio";
                                }
                                else if (mediaType == "TV")
                                {
                                    _nowPlaying.MediaType = "Movie";
                                    _nowPlaying.FileName = ReplaceInvalidChars(_nowPlaying.FileName);
                                }
                                else
                                {
                                    _nowPlaying.MediaType = "Movie";
                                }

                                _parent.Log("JRiver:NP NowPlaying.MediaType:" + _nowPlaying.MediaType);

                                if (_nowPlaying.MediaType == "TvShow")
                                {
                                    int SeasonNumber = 0;
                                    int EpisodeNumber = 0;
                                    int.TryParse(getFieldValue(fileFields, "Season"), out SeasonNumber);
                                    int.TryParse(getFieldValue(fileFields, "Episode"), out EpisodeNumber);
                                    _nowPlaying.SeasonNumber = SeasonNumber;
                                    _nowPlaying.EpisodeNumber = EpisodeNumber;
                                    _nowPlaying.Plot = getFieldValue(fileFields, "Description");
                                    _nowPlaying.ShowTitle = String.IsNullOrEmpty(getFieldValue(fileFields, "Series")) ? "Blank" : getFieldValue(fileFields, "Series");
                                    _parent.Log("JRiver:NP NowPlaying.Showtitle:" + _nowPlaying.ShowTitle);
                                }

                                // _nowPlaying.FirstAired = String.IsNullOrEmpty(getFieldValue(fileFields, "Date")) ? "NotGiven" : getFieldValue(fileFields, "Filename"); ;
                                var serverArt = getItemName(deserialized, "ImageURL");
                                _parent.Log("JRiver: server.Art EQUALS ===========" + serverArt);

                               // var sw = Stopwatch.StartNew();

                                var extrafanart = false;
                                var filePath = "BAD";
                                try
                                {
                                    filePath = Path.GetDirectoryName(_nowPlaying.FileName);
                                    extrafanart = true;  //Path Seems to be okay
                                    _parent.Log("JRIVER:   nowPlaying filePath appears intact ");
                                }
                                catch
                                {
                                    _parent.Log("JRIVER:   nowPlaying Bad filePath: Caught: ");
                                    extrafanart = false;
                                    filePath = "BAD";
                                }

                                string[] files = new string[] { };
                                if (extrafanart)
                                {
                                    files = System.IO.Directory.GetFiles(filePath);
                                }
                                else
                                {
                                    files = new string[] { "Empty" };
                                }
                                
                                string extrafanartbackdropfile = "";

                                if (extrafanart)
                                {
                                    var extrafanartpath = Path.Combine(filePath, "extrafanart");
                                    _parent.Log("JRiver:  nowPlaying extrafanartPath to check equals:" + extrafanartpath);
                                    if (Directory.Exists(extrafanartpath))
                                    {
                                        var extrafanartFiles = System.IO.Directory.GetFiles(extrafanartpath, "*.jpg");
                                        _parent.Log("JRiver:  nowPlaying extrafanartPath Exists:");
                                        _parent.Log("JRiver:  extrafanart Files: " + string.Join(",  ", extrafanartFiles));
                                        // check files
                                        if (extrafanartFiles != null && extrafanartFiles.Length > 0)
                                        {
                                            extrafanart = true;
                                            // get random item
                                            var idx = new Random().Next(extrafanartFiles.Length);
                                            _parent.Log("JRiver:  ExtrafanartFiles Index ==============" + idx);
                                            extrafanartbackdropfile = extrafanartFiles[idx];
                                            _parent.Log("JRiver:  extrabackdropfile  ==============" + extrafanartbackdropfile);
                                        }
                                    }
                                }

                                if (filePath != "BAD")
                                {
                                    var fanartPath = Path.Combine(filePath, "fanart.jpg");
                                    var LogoPath = Path.Combine(filePath, "logo.png");
                                    var ThumbPath = Path.Combine(filePath, "poster.jpg");
                                    var ThumbPath2 = Path.Combine(filePath, "folder.jpg");

                                    string CustomArt = "Default.";
                                    if (!String.IsNullOrEmpty(_nowPlaying.FileName))
                                    {
                                        CustomArt = _nowPlaying.FileName.Remove(_nowPlaying.FileName.LastIndexOf('.'));   // remove characters up to . deals with .ts and .mp4
                                    }

                                    CustomArt = CustomArt + ".jpg";
                                    _parent.Log("Jriver: CustomArt: Check: " + CustomArt);

                                    _parent.Log("JRiver: ** filePath ** :" + filePath);
                                    //_parent.Log("JRiver: Files in path:" + string.Join(",  ", files) );
                                    _parent.Log("JRiver: Checking.... ** fanArt.Jpg ** ** Logo.Png ** ** poster.Jpg **  ** folder.jpg ** :");
                                    int pos = Array.IndexOf(files, fanartPath);

                                    if (extrafanart == true && extrafanartbackdropfile != "")
                                    {
                                        _nowPlaying.FanartURL = extrafanartbackdropfile;
                                    }
                                    else if (pos > -1)
                                    {
                                        _nowPlaying.FanartURL = files[pos];  //if fanart.jpg exisits in directory with movie use this otherwise default to JRiver Thumb
                                    }
                                    else
                                    {
                                        _nowPlaying.FanartURL = @"http://" + _parent.IP + ":" + _parent.Port + @"/" + serverArt + "&Type=Full&Token=" + _parent.JRiverAuthToken;
                                    }


                                    pos = Array.IndexOf(files, LogoPath);
                                    if (pos > -1)
                                    {
                                        _nowPlaying.LogoURL = files[pos];
                                    }
                                    else
                                    {
                                        _nowPlaying.LogoURL = "";
                                    }
                                    pos = Array.IndexOf(files, ThumbPath);
                                    int pos2 = Array.IndexOf(files, ThumbPath2);
                                    int pos3 = Array.IndexOf(files, CustomArt);

                                    if (pos > -1)
                                    {
                                        _nowPlaying.ThumbURL = files[pos];
                                    }
                                    else if (pos2 > -1)
                                    {
                                        _nowPlaying.ThumbURL = files[pos2];
                                    }
                                    else if (pos3 > -1)
                                    {
                                        _nowPlaying.ThumbURL = files[pos3];
                                    }
                                    else
                                    {
                                        // _nowPlaying.FanartURL = @"http://" + _parent.IP + ":" + _parent.Port + @"/"+ serverArt + "&Type=Full&Token="+_parent.JRiverAuthToken;
                                        _nowPlaying.ThumbURL = @"http://" + _parent.IP + ":" + _parent.Port + @"/" + serverArt + "&Type=Full&Token=" + _parent.JRiverAuthToken;
                                    }

                                    //sw.Stop();
                                }
                                else   // No filePath information.  Nothing to get except http file from Jrvier

                                {
                                    _nowPlaying.FanartURL = @"http://" + _parent.IP + ":" + _parent.Port + @"/" + serverArt + "&Type=Full&Token=" + _parent.JRiverAuthToken;
                                    _nowPlaying.ThumbURL = @"http://" + _parent.IP + ":" + _parent.Port + @"/" + serverArt + "&Type=Full&Token=" + _parent.JRiverAuthToken;
                                    _nowPlaying.LogoURL = "";
                                }
                               // _parent.Log("JRIVER: STOPWATCH: Elapsed MS " + sw.ElapsedMilliseconds);


                                _parent.Log("JRiver:  nowPlaying Fanart equals:" + _nowPlaying.FanartURL);
                                _parent.Log("JRiver:  nowPlaying Logo equals:" + _nowPlaying.LogoURL);
                                _parent.Log("JRiver:  nowPlaying Thumb equals:" + _nowPlaying.ThumbURL);

                                _nowPlaying.Studio = String.IsNullOrEmpty(getFieldValue(fileFields, "Studios")) ? "" : getFieldValue(fileFields, "Studios");
                                _parent.Log("JRiver:NP NowPlaying.Studio:" + _nowPlaying.Studio);

                                _nowPlaying.Genre = String.IsNullOrEmpty(getFieldValue(fileFields, "Genre")) ? "" : getFieldValue(fileFields, "Genre");
                                _parent.Log("JRiver:NP NowPlaying.Genre:" + _nowPlaying.Genre);

                                if (_nowPlaying.MediaType == "TvShow" || _nowPlaying.MediaType == "Movie")
                                {
                                    List<string> MovieIcons = new List<string>();
                                    MovieIcons = GetMovieIcons(fileFields);

                                    _nowPlaying.MovieIcons = String.Join(",", MovieIcons);
                                    _parent.Log("JRiver:NP NowPlaying.MovieIcons:" + _nowPlaying.MovieIcons);
                                }
                            }

                            var Volume = getItemName(deserialized, "VolumeDisplay");  //Volume

                            if (Volume == "Muted")
                            {
                                _nowPlaying.IsMuted = true;
                            }
                            else
                            {
                                _nowPlaying.IsMuted = false;
                                int VolumeInt = 0;
                                int.TryParse(Volume.Remove(Volume.Length - 1), out VolumeInt);

                                try
                                {
                                    _parent.Log("JRiver:Volume: VolumeDisplay Equals:" + Volume);
                                    _nowPlaying.Volume = VolumeInt;

                                }
                                catch (Exception ex)
                                {
                                    _parent.Log("Exception in Volume Conversion:" + ex);
                                }
                            }
                            _parent.Log("JRiver:NP NowPlaying.title:" + _nowPlaying.Title);

                            if (Convert.ToUInt64(getItemName(deserialized, "DurationMS")) > 0)
                            {
                                // duration for Plex given in millseconds - convert to seconds and round
                                // Convert Duration to Timespan with seconds only
                                var RoundSeconds = Math.Round(Convert.ToInt64(getItemName(deserialized, "DurationMS")) / 1000.00, 1);
                                _nowPlaying.Duration = new TimeSpan(0, Convert.ToInt32("0"), Convert.ToInt32("0"), Convert.ToInt32(RoundSeconds));
                            }

                            _parent.Log("JRiver:NP server.Media.Duration:" + getItemName(deserialized, "DurationMS") + ":  _nowPlaying.Duration (calculated) :" + _nowPlaying.Duration);

                            var RoundOffset = Math.Round(Convert.ToInt64(getItemName(deserialized, "PositionMS")) / 1000.00, 1);
                            _nowPlaying.Time = new TimeSpan(0, 0, 0, Convert.ToInt32(RoundOffset));

                            _parent.Log("JRiver:NP NowPlaying.Time:" + _nowPlaying.Time + "Calcuated on server.viewOffset:" + getItemName(deserialized, "PositionMS"));

                            var percent = Math.Floor(100.0 * Convert.ToInt32("0" + getItemName(deserialized, "PositionMS"), CultureInfo.InvariantCulture) / Convert.ToInt32("0" + getItemName(deserialized, "DurationMS"), CultureInfo.InvariantCulture));
                            if (Double.IsNaN(percent))
                            {
                                percent = 0;
                            }
                            _nowPlaying.Progress = (int)percent;

                            _parent.Log("JRiver:NP NowPlaying.Progress:" + _nowPlaying.Progress);

                                  

                            //        if (server.type == "episode")
                            //        {
                            //            _nowPlaying.EpisodeNumber = Convert.ToInt32(server.index);
                            //            _nowPlaying.SeasonNumber = Convert.ToInt32(server.parentIndex);


                            //        }



                            if (_nowPlaying.MediaType == "Audio")
                            {
                                _nowPlaying.Artist = String.IsNullOrEmpty(getItemName(deserialized, "Artist")) ? "Unknown" : getItemName(deserialized, "Artist");
                                _nowPlaying.Album = String.IsNullOrEmpty(getItemName(deserialized, "Album")) ? "Unknown" : getItemName(deserialized, "Album");
                                _nowPlaying.Track  = String.IsNullOrEmpty(getItemName(deserialized, "PlayingNowTracks")) ? 1 : Convert.ToInt32(getItemName(deserialized, "PlayingNowTracks"));
                            }

                            // _nowPlaying.LogoURL = "";

                          //  _nowPlaying.MovieIcons = "";

                              _parent.Log("JRiver Remote:  Filename" + _nowPlaying.FileName + " IsPlaying :" + _nowPlaying.IsPlaying + " IsPaused :" + _nowPlaying.IsPaused + " MediaType :" + _nowPlaying.MediaType);

                            //        // check for endwith ts filename
                            //        // actually check for mpegts
                            //        // may be recorded movies in mpegts format as well.....
                            //        // check for regex s00e00 contents - and when both present swap

                            //        // Change of plans - run for all files regardless of type and regardless of mediacontainer


                            //        if (1==1)     //(server.Media.container == "mpegts" && server.type == "movie")
                            //        {
                            //            _parent.Log("Plex Remote:  New changing recorded TV to TV Shows and extracting season/episode data");

                            //            Regex regex = new Regex(@"[Ss](?<season>\d{1,2})[Ee](?<episode>\d{1,2})");

                            //            _parent.Log("Plex Remote:  Server Title Equals:" + server.title);

                            //            if (server.title != null)
                            //            {
                            //                Match match = regex.Match(server.title);
                            //                if (match.Success)
                            //                {
                            //                    _nowPlaying.MediaType = "TvShow";
                            //                    _nowPlaying.SeasonNumber = Convert.ToInt32(match.Groups["season"].Value);
                            //                    _nowPlaying.EpisodeNumber = Convert.ToInt32(match.Groups["episode"].Value);
                            //                    _parent.Log("Plex Remote: Regex TV Conversion: From :" + server.title);


                            //                    //string replacement = "`$``";
                            //                    //string newtitle = replacement;
                            //                    // string newtitle = regex.Replace(server.title, replacement);

                            //                    string[] lines = Regex.Split(server.title, @"[Ss](?<season>\d{1,2})[Ee](?<episode>\d{1,2})");
                            //                    string title1 = lines[0];
                            //                    string newtitle = title1.Replace(".", " ");
                            //                    _parent.Log("Plex Remote:  New title equals :" + newtitle);
                            //                    //_nowPlaying.ShowTitle = newtitle;
                            //                    _nowPlaying.Title = newtitle;
                            //                }

                            //            }
                            //        }



                            //        return;
                            //    }

                            //}
                            //    // if no local Client Playback then nothing Playing
                            //    _nowPlaying.FileName = "";
                            //    _nowPlaying.Title = "";
                            //    _nowPlaying.IsPlaying = false;
                            //    _nowPlaying.IsPaused = false;
                            //    _nowPlaying.IsPlaying = false;
                            //    _nowPlaying.IsNewMedia = false;
                            //    _parent.Log("Plex Log 2nd: Nothing is Playing");
                            //    return;

                            //}


                        }
                    }
                    catch (Exception ex)
                    {
                        _parent.Log("Exception in NowPlaying Jriver System" + ex);
                    }






                }
            }
        }
        public List<string> GetMovieIcons(Jriver.Api.getFileInfo.MPL Movieitem)
        {
            List<string> MovieIcons = new List<string>();
            _parent.Trace("MovieIcons Generating List:");
            // Make sure not null; 

            MovieIcons.Add("");

            try
            {
                //Container
                if (Movieitem != null)
                {
                    var Compression = getFieldValue(Movieitem, "Compression");

                    if (Compression != null && Compression != "")
                    {
                        var video = Compression.Split(' ');
                        var videoCodec = video[3].Remove(video[3].Length - 1).ToUpper();
                        var audioCodec = video[5].Remove(video[5].Length - 1).ToUpper();

                        videoCodec = "codec" + videoCodec;

                        MovieIcons.Add(videoCodec);
                        MovieIcons.Add(audioCodec);
                        _parent.Trace("MovieIcons Adding:" + videoCodec);
                        _parent.Trace("MovieIcons Adding:" + audioCodec);
                    }


                    var Channels = getFieldValue(Movieitem, "Channels");

                    if (Channels != "0" && !string.IsNullOrWhiteSpace(Channels) && Channels !="")
                    {
                        MovieIcons.Add("Channels" + Channels.ToString());
                        _parent.Trace("MovieIcons Adding Channels:" + Channels.ToString());
                    }

                    var fileType = getFieldValue(Movieitem, "File Type");
                    MovieIcons.Add(fileType);
                    _parent.Trace("MovieIcons Adding FileType:" + fileType);

                    var getVideoHeight = getFieldValue(Movieitem, "Height");
          
                    if (getVideoHeight != null && getVideoHeight != "")
                    {
                        var VideoHeight = Convert.ToInt32(getVideoHeight);
                        _parent.Trace("MovieIcons VideoHeight Converted:" + VideoHeight);
                        if (VideoHeight > 1200)
                        {
                            MovieIcons.Add("4K");
                            _parent.Trace("MoviesIcons Adding 4K");
                        }
                        else if (VideoHeight >= 790)
                        {
                            MovieIcons.Add("1080p");
                            _parent.Trace("MoviesIcons Adding 1080p");
                        }
                        else if (VideoHeight >= 700)
                        {
                            MovieIcons.Add("720p");
                            _parent.Trace("MoviesIcons Adding 720p");
                        }
                        else if (VideoHeight >= 400)
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

                    var aspect = getFieldValue(Movieitem, "Aspect Ratio");
                    if (aspect != "" && aspect != null)
                    {
                        MovieIcons.Add(aspect);
                        _parent.Trace("MovieIcons Adding Aspect:" + aspect.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                _parent.Trace("MovieIcons Exception Caught Within NowPlaying Codec Check:" + ex);
                return MovieIcons;
            }

            return MovieIcons;

        }



        public Jriver.Api.getFileInfo.MPL getFileInfo(string fileKey)
        {
            try
            {
                _parent.Log("JRiver: Using Parent IP equals: " + _parent.IP);
                string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/MCWS/v1/File/GetInfo?File="+fileKey+"&Token=" + _parent.JRiverAuthToken;
                var requestnew = WebRequest.Create(NPurl);
                requestnew.Headers.Add("Token", _parent.JRiverAuthToken);
                var response = requestnew.GetResponse();

                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {
                    // Get the stream containing content returned by the server.
                    System.IO.Stream dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader.
                    System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);
                    XmlSerializer serializer = new XmlSerializer(typeof(Jriver.Api.getFileInfo.MPL));
                    var deserialized = (Jriver.Api.getFileInfo.MPL)serializer.Deserialize(reader);
                    return deserialized;
                }
                return null;
            }
            catch (Exception ex)
            {
                _parent.Log("Exception in JRiver getFileInfo:" + ex);
                return null;
            }

        }
        public string getFieldValue(Jriver.Api.getFileInfo.MPL fileInfo, string itemName)
        {
            try
            {
                foreach (var item in fileInfo.Item )
                {              
                    if (item.Name == itemName)
                    {
                        return item.Value;
                    }
                }

                _parent.Log("getItemName: No such item found ");
                return "";
            }
            catch (Exception e)
            {
                _parent.Log("Exception in getItemName" + e);
                return "";
            }
        }


        public ApiCurrently NowPlaying(bool checkNewMedia)
        {
            lock (Locker)
            {
                if (checkNewMedia)
                {
                    _nowPlaying.IsNewMedia = false;
                    _parent.Log("JRiver: [Xbmc.Player.cs 610] NewMedia:False");
                    if (_currentMediaFile != _nowPlaying.FileName || (_currentMediaTitle != _nowPlaying.Title))
                    {
                        _currentMediaTitle = _nowPlaying.Title;
                        _currentMediaFile = _nowPlaying.FileName;
                        _parent.Log("JRiver: [Xbmc.Player.cs 807] setting NewMedia to True");
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
                _parent.MpcHcRemote.SeekPercentage(progress, _nowPlaying.Duration.TotalMilliseconds);
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

        public string getItemName(Jriver.Api.InfoZone.Response InfoZone, string itemName)
        {
            try
            {
                foreach (var item in InfoZone.Item)
                {
                    if (item.Name == itemName){
                        return item.Value;
                    }
                }
                
                _parent.Log("getItemName: No such item found ");
                return "";
            }
            catch (Exception e)
            {
                _parent.Log("Exception in getItemName" + e);
                return "";
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
