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

namespace Remote.XBMC.Dharma.Api
{
    class XbmcPlayer : IApiPlayer
    {
        private readonly Xbmc _parent;
        private string _currentMediaFile;
        private string _currentMediaTitle;
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
                    var data = (string[]) result2.ToArray(typeof (string));
                    if (data.Length > 6)
                    {
                        _nowPlaying.MediaType =  "Video";
                        _nowPlaying.Title = "Media Player Classic";
                        _nowPlaying.Time = new TimeSpan(0, 0, 0, Convert.ToInt32("0" + data[2])/1000);
                        _nowPlaying.Duration = new TimeSpan(0, 0, 0, Convert.ToInt32("0" + data[4])/1000);
                        var percent = Math.Floor(100.0 * Convert.ToInt32("0" + data[2], CultureInfo.InvariantCulture) / Convert.ToInt32("0" + data[4], CultureInfo.InvariantCulture));
                        if (Double.IsNaN(percent))
                            percent = 0;
                        _nowPlaying.Volume = Convert.ToInt32("0" + data[7], CultureInfo.InvariantCulture);
                        _nowPlaying.IsMuted = data[6] == "1";
                        _nowPlaying.Progress = (int)percent;
                    }
                }
                else
                {
                    if (!_parent.IsConnected())
                    {
                        _nowPlaying.FileName = "";
                        _nowPlaying.Title = "";
                        return;
                    }

                    var infos = (JsonObject)_parent.JsonCommand("System.GetInfoLabels", new[]
                                                                                {
                                                                                    "MusicPlayer.Title",
                                                                                    "MusicPlayer.Album",
                                                                                    "MusicPlayer.Artist",
                                                                                    "MusicPlayer.Property(Artist_Description)",
                                                                                    "MusicPlayer.Genre",
                                                                                    "MusicPlayer.Year",
                                                                                    "MusicPlayer.TrackNumber",
                                                                                    "MusicPlayer.Codec",
                                                                                    "Player.Time",
                                                                                    "Player.Duration",
                                                                                    "Player.Volume",
                                                                                    "Player.Filenameandpath",
                                                                                    "VideoPlayer.Title",
                                                                                    "VideoPlayer.TVShowTitle",
                                                                                    "VideoPlayer.Genre",
                                                                                    "VideoPlayer.Director",
                                                                                    "VideoPlayer.Year",
                                                                                    "VideoPlayer.Rating",
                                                                                    "VideoPlayer.Tagline",
                                                                                    "VideoPlayer.Studio",
                                                                                    "VideoPlayer.Plot",
                                                                                    "VideoPlayer.Season",
                                                                                    "VideoPlayer.Episode",
                                                                                    "VideoPlayer.VideoCodec"
                                                                                });

                    if (infos == null)
                        return;
                    JsonObject infos2 = null;
                    _nowPlaying.FileName = infos["Player.Filenameandpath"].ToString();

                    if (!String.IsNullOrEmpty(infos["VideoPlayer.TVShowTitle"].ToString()))
                    {
                        _nowPlaying.MediaType = "TvShow";
                    }
                    else if (!String.IsNullOrEmpty(infos["MusicPlayer.Title"].ToString()))
                    {
                        _nowPlaying.MediaType = "Audio";
                    }
                    else if (!String.IsNullOrEmpty(infos["VideoPlayer.Title"].ToString()))
                    {
                        _nowPlaying.MediaType = "Movie";
                    }
                    else
                    {
                        _nowPlaying.MediaType = "Unknown";
                        _nowPlaying.Title = "";
                        _nowPlaying.IsPlaying = false;
                        _nowPlaying.IsPaused = false;
                        _nowPlaying.Duration = new TimeSpan();
                        _nowPlaying.Time = new TimeSpan();
                    }

                    if (_nowPlaying.MediaType == "Movie" || _nowPlaying.MediaType == "TvShow")
                    {
                        infos2 = (JsonObject)_parent.JsonCommand("VideoPlayer.GetTime", null);
                        _nowPlaying.Genre = infos["VideoPlayer.Genre"].ToString();
                        _nowPlaying.Title = infos["VideoPlayer.Title"].ToString();
                        _nowPlaying.Year = Convert.ToInt32("0" + infos["VideoPlayer.Year"]);
                        _nowPlaying.SeasonNumber = Convert.ToInt32("0" + infos["VideoPlayer.Season"]);
                        _nowPlaying.EpisodeNumber = Convert.ToInt32("0" + infos["VideoPlayer.Episode"]);
                        _nowPlaying.ShowTitle = infos["VideoPlayer.TVShowTitle"].ToString();
                        _nowPlaying.Plot = infos["VideoPlayer.Plot"].ToString();
                        _nowPlaying.Director = infos["VideoPlayer.Director"].ToString();
                        _nowPlaying.Studio = infos["VideoPlayer.Studio"].ToString();
                        _nowPlaying.Tagline = infos["VideoPlayer.Tagline"].ToString();
                        _nowPlaying.Rating = infos["VideoPlayer.Rating"].ToString();
                        if (_nowPlaying.FileName.StartsWith("stack://", StringComparison.OrdinalIgnoreCase))
                        {
                            var temp = _nowPlaying.FileName.Split(new[] { " , " }, StringSplitOptions.None);
                            var hash = Xbmc.Hash(temp[0].Replace("stack://", ""));
                            _nowPlaying.ThumbURL = @"special://profile/Thumbnails/Video/" + hash[0] + "/" + hash + ".tbn";
                            _nowPlaying.FanartURL = @"special://profile/Thumbnails/Video/Fanart/" + Xbmc.Hash(_nowPlaying.FileName) + ".tbn";
                        }
                        else
                        {
                            var hash = Xbmc.Hash(_nowPlaying.FileName);
                            _nowPlaying.ThumbURL = @"special://profile/Thumbnails/Video/" + hash[0] + "/" + hash + ".tbn";
                            _nowPlaying.FanartURL = @"special://profile/Thumbnails/Video/Fanart/" + hash + ".tbn";
                        }
                    }
                    if (_nowPlaying.MediaType == "Audio")
                    {
                        infos2 = (JsonObject)_parent.JsonCommand("AudioPlayer.GetTime", null);
                        _nowPlaying.Genre = infos["MusicPlayer.Genre"].ToString();
                        _nowPlaying.Title = infos["MusicPlayer.Title"].ToString();
                        _nowPlaying.Year = Convert.ToInt32("0" + infos["MusicPlayer.Year"]);
                        _nowPlaying.Track = Convert.ToInt32("0" + infos["MusicPlayer.TrackNumber"]);
                        _nowPlaying.Artist = infos["MusicPlayer.Artist"].ToString();
                        _nowPlaying.Album = infos["MusicPlayer.Album"].ToString();
                        var hash = Xbmc.Hash(_nowPlaying.Album + _nowPlaying.Artist);
                        _nowPlaying.ThumbURL = @"special://profile/Thumbnails/Music/" + hash[0] + "/" + hash + ".tbn";
                        _nowPlaying.FanartURL = @"special://profile/Thumbnails/Music/Fanart/" + hash + ".tbn";
                    }

                    if (infos2 != null)
                    {
                        _nowPlaying.IsPaused = (bool)infos2["paused"];
                        _nowPlaying.IsPlaying = !(bool)infos2["paused"] && (bool)infos2["playing"];
                        _nowPlaying.Time = new TimeSpan(0, 0, 0, Convert.ToInt32("0" + infos2["time"]));
                        _nowPlaying.Duration = new TimeSpan(0, 0, 0, Convert.ToInt32("0" + infos2["total"]));
                    }

                    var percent = Math.Floor(100.0 * _nowPlaying.Time.TotalSeconds / _nowPlaying.Duration.TotalSeconds);
                    if (Double.IsNaN(percent))
                        percent = 0;
                    _nowPlaying.Progress = (int)percent;

                    var vol = (1 - Convert.ToDouble("0" + infos["Player.Volume"].ToString().Replace(" dB", "").Replace("-","").Replace(".",","))/60)*100;

                    _nowPlaying.Volume = (int)vol;
                    _nowPlaying.IsMuted = (vol == 0);

                    _parent.MpcLoaded = _nowPlaying.Duration == new TimeSpan(0, 0, 0, 1);


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
                    _parent.JsonCommand(
                        _nowPlaying.MediaType == "Audio" ? "AudioPlayer.SeekPercentage" : "VideoPlayer.SeekPercentage",
                        progress);
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
