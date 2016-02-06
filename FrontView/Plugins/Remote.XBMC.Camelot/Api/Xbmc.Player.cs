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
using Plugin;

namespace Remote.XBMC.Camelot.Api
{
    class XbmcPlayer : IApiPlayer
    {
        private readonly Xbmc _parent;
        private string _currentMediaFile;
        private string _currentMediaTitle;
        private bool _isNewMedia;
        private int _volume;
        private int _progress;
        private readonly Hashtable _currentInfo = new Hashtable();
        private ApiCurrently _nowPlaying = new ApiCurrently();

        static readonly object Locker = new object();

        public XbmcPlayer(Xbmc parent)
        {
            _parent = parent;
        }

        public void RefreshNowPlaying()
        {
            lock (Locker)
            {
                if (!_parent.IsConnected()) return;

                var infos = _parent.Command("GetCurrentlyPlaying");
                if (infos == null)
                    return;

                _currentInfo.Clear();

                foreach (var info in infos)
                {
                    var splitIndex = info.IndexOf(':') + 1;
                    if (splitIndex <= 2) continue;
                    var key = info.Substring(0, splitIndex - 1).Replace(" ", "").ToLower(CultureInfo.InvariantCulture);
                    var value = info.Substring(splitIndex, info.Length - splitIndex);
                    _currentInfo.Add(key, value);
                }
                if (GetInfo("thumb") != null)
                {
                    var thumbparts = GetInfo("thumb").Split('/');
                    var hash = thumbparts[thumbparts.Length - 1].Trim().Replace(".tbn", "");
                    _currentInfo.Add("fanart", GetInfo("thumb").Replace("/" + hash[0] + "/", "/Fanart/"));
                }


                string[] aVolume = null;
                string[] aProgress = null;

                if (GetInfo("playstatus") != null)
                {
                    aVolume = _parent.Command("GetVolume");
                    if (aVolume == null)
                        return;
                    aProgress = _parent.Command("GetPercentage");
                    if (aProgress == null)
                        return;
                }

                try
                {
                    _volume = aVolume != null ? Convert.ToInt32("0" + aVolume[0], CultureInfo.InvariantCulture) : 0;
                }
                catch (Exception)
                {
                    _volume = 0;
                }

                _volume = Math.Min(100, _volume);
                try
                {
                    _progress = aProgress != null ? Convert.ToInt32("0" + aProgress[0], CultureInfo.InvariantCulture) : 1;
                }
                catch (Exception)
                {
                    _progress = 1;
                }
                _progress = Math.Min(99, _progress);


                var nowPlaying = new ApiCurrently
                {
                    IsPlaying = (GetInfo("playstatus") == "Playing") ? true : false,
                    IsPaused = (GetInfo("playstatus") == "Paused") ? true : false,
                    IsNewMedia = _isNewMedia,
                    IsMuted = (_volume == 0) ? true : false
                };

                if (GetInfo("showtitle") != null)
                    nowPlaying.MediaType = "TvShow";
                else if (GetInfo("title") != null)
                {
                    if (GetInfo("type") == "Audio")
                        nowPlaying.MediaType = "Audio";
                    if (GetInfo("type") == "Video")
                        nowPlaying.MediaType = "Movie";
                }
                else
                {
                    nowPlaying.MediaType = "Unknown";
                }

                nowPlaying.FileName = GetInfo("filename");
                nowPlaying.Title = GetInfo("title");
                nowPlaying.Artist = GetInfo("artist");
                nowPlaying.Album = GetInfo("album");
                try
                {
                    nowPlaying.Year = Convert.ToInt32("0" + GetInfo("year"), CultureInfo.InvariantCulture);
                }
                catch
                {
                    nowPlaying.Year = 0;
                }
                try
                {

                    nowPlaying.Track = Convert.ToInt32("0" + GetInfo("track"), CultureInfo.InvariantCulture);
                }
                catch
                {
                    nowPlaying.Track = 0;
                }
                try
                {
                    nowPlaying.SeasonNumber = Convert.ToInt32("0" + GetInfo("season"), CultureInfo.InvariantCulture);
                }
                catch
                {
                    nowPlaying.SeasonNumber = 0;
                }
                try
                {
                    nowPlaying.EpisodeNumber = Convert.ToInt32("0" + GetInfo("episode"), CultureInfo.InvariantCulture);
                }
                catch
                {
                    nowPlaying.EpisodeNumber = 0;
                }

                nowPlaying.Genre = GetInfo("genre");
                nowPlaying.ThumbURL = GetInfo("thumb");
                nowPlaying.FanartURL = GetInfo("fanart");
                nowPlaying.ShowTitle = GetInfo("showtitle");
                nowPlaying.Plot = GetInfo("plot");
                nowPlaying.Director = GetInfo("director");
                nowPlaying.Volume = GetVolume();
                nowPlaying.Progress = GetProgress();

                nowPlaying.Studio = GetInfo("studio");
                nowPlaying.Tagline = GetInfo("tagline");
                nowPlaying.Rating = GetInfo("rating");

                var time = GetInfo("time");
                if (!string.IsNullOrEmpty(time))
                {
                    var splittime = time.Split(':');
                    switch (splittime.Length)
                    {
                        case 1:
                            nowPlaying.Time = new TimeSpan(0, 0, 0,
                                                           Convert.ToInt32("0" + splittime[0], CultureInfo.InvariantCulture));
                            break;
                        case 2:
                            nowPlaying.Time = new TimeSpan(0, 0,
                                                           Convert.ToInt32("0" + splittime[0], CultureInfo.InvariantCulture),
                                                           Convert.ToInt32("0" + splittime[1], CultureInfo.InvariantCulture));
                            break;
                        case 3:
                            nowPlaying.Time = new TimeSpan(0,
                                                           Convert.ToInt32("0" + splittime[0], CultureInfo.InvariantCulture),
                                                           Convert.ToInt32("0" + splittime[1], CultureInfo.InvariantCulture),
                                                           Convert.ToInt32("0" + splittime[2], CultureInfo.InvariantCulture));
                            break;
                        default:
                            nowPlaying.Time = new TimeSpan(0);
                            break;
                    }
                }

                var duration = GetInfo("duration");
                if (duration != null)
                {
                    var splitduration = duration.Split(':');
                    switch (splitduration.Length)
                    {
                        case 1:
                            nowPlaying.Duration = new TimeSpan(0, 0, 0,
                                                               Convert.ToInt32("0" + splitduration[0],
                                                                               CultureInfo.InvariantCulture));
                            break;
                        case 2:
                            nowPlaying.Duration = new TimeSpan(0, 0,
                                                               Convert.ToInt32("0" + splitduration[0],
                                                                               CultureInfo.InvariantCulture),
                                                               Convert.ToInt32("0" + splitduration[1],
                                                                               CultureInfo.InvariantCulture));
                            break;
                        case 3:
                            nowPlaying.Duration = new TimeSpan(0,
                                                               Convert.ToInt32("0" + splitduration[0],
                                                                               CultureInfo.InvariantCulture),
                                                               Convert.ToInt32("0" + splitduration[1],
                                                                               CultureInfo.InvariantCulture),
                                                               Convert.ToInt32("0" + splitduration[2],
                                                                               CultureInfo.InvariantCulture));
                            break;
                        default:
                            nowPlaying.Duration = new TimeSpan(0);
                            break;
                    }
                }

                var firstaired = GetInfo("firstaired");
                if (firstaired != null)
                {
                    var splitaired = firstaired.Split('-');
                    if (splitaired.Length > 1)
                        nowPlaying.FirstAired =
                            new DateTime(Convert.ToInt32("0" + splitaired[0], CultureInfo.InvariantCulture),
                                         Convert.ToInt32("0" + splitaired[1], CultureInfo.InvariantCulture),
                                         Convert.ToInt32("0" + splitaired[2], CultureInfo.InvariantCulture));

                }
                _nowPlaying = nowPlaying;
            }

        }

        public ApiCurrently NowPlaying(bool checkNewMedia)
        {
            
            lock (Locker)
            {
                if (checkNewMedia)
                {
                    if (_currentMediaFile != _nowPlaying.FileName || (_currentMediaTitle != _nowPlaying.Title))
                    {
                        _currentMediaTitle = _nowPlaying.Title;
                        _currentMediaFile = _nowPlaying.FileName;
                        _isNewMedia = true;
                    }
                    else
                        _isNewMedia = false;
                }

                return _nowPlaying;
            }
        }

        public int GetVolume()
        {
            return _volume;
        }

        public int GetProgress()
        {
            return _progress;
        }

        public string GetInfo(string field)
        {
            return _currentInfo.ContainsKey(field) ? _currentInfo[field].ToString() : null;
        }

        public void PlayPause()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventAction("PlayerControl(Play)");
        }

        public void Stop()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventAction("PlayerControl(Stop)");
        }

        public void SkipPrevious()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventAction("PlayerControl(Previous)");
        }
        
        public void SkipNext()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventAction("PlayerControl(Next)");
        }

        public void SeekPercentage(int progress)
        {
            if (_parent.IsConnected())
                _parent.AsyncCommand("SeekPercentage", Convert.ToString(progress, CultureInfo.InvariantCulture));
        }

        public void SetVolume(int percentage)
        {
            if (_parent.IsConnected())
                _parent.AsyncEventAction("SetVolume(" + Convert.ToString(percentage, CultureInfo.InvariantCulture) + ")");
        }

        public void ToggleMute()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventAction("Mute");
        }
    }
}
