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
using System.Globalization;
using Plugin;

namespace Remote.MediaPortal.iPimp.Api
{
    class MediaPortalPlayer : IApiPlayer
    {
        private readonly MediaPortal _parent;
        private string _currentMediaFile;
        private string _currentMediaTitle;
        private readonly ApiCurrently _nowPlaying = new ApiCurrently();

        static readonly object Locker = new object();

        public MediaPortalPlayer(MediaPortal parent)
        {
            _parent = parent;
        }

        public void RefreshNowPlaying()
        {
            if (!_parent.IsConnected()) return;
            lock (Locker)
            {
                var data = _parent.IPimpCommand(new CommandInfoIPimp {Action = "nowplaying"});

                if (!Convert.ToBoolean(data["result"], CultureInfo.InvariantCulture))
                {
                    return;
                }

                _nowPlaying.FileName = Convert.ToString(data["filename"], CultureInfo.InvariantCulture);
                _nowPlaying.Title = Convert.ToString(data["title"], CultureInfo.InvariantCulture);

                _nowPlaying.Artist = Convert.ToString(data["artist"], CultureInfo.InvariantCulture);
                _nowPlaying.Album = Convert.ToString(data["album"], CultureInfo.InvariantCulture);
                _nowPlaying.Year = Convert.ToInt32(data["year"], CultureInfo.InvariantCulture);
                _nowPlaying.Track = Convert.ToInt32(data["track"], CultureInfo.InvariantCulture);
                _nowPlaying.Genre = Convert.ToString(data["genre"], CultureInfo.InvariantCulture);
                _nowPlaying.ThumbURL = Convert.ToString(data["thumb"], CultureInfo.InvariantCulture);
                _nowPlaying.FanartURL = Convert.ToString(data["fanart"], CultureInfo.InvariantCulture);
                _nowPlaying.ShowTitle = Convert.ToString(data["showtitle"], CultureInfo.InvariantCulture);
               // _nowPlaying.FirstAired = new DateTime(); // Convert.ToString(data["firstaired"]);
                _nowPlaying.Plot = Convert.ToString(data["plot"], CultureInfo.InvariantCulture);
                _nowPlaying.Tagline = Convert.ToString(data["tagline"], CultureInfo.InvariantCulture);
                _nowPlaying.Rating = Convert.ToString(data["rating"], CultureInfo.InvariantCulture);
                _nowPlaying.Director = Convert.ToString(data["director"], CultureInfo.InvariantCulture);
                _nowPlaying.SeasonNumber = Convert.ToInt32(data["season"], CultureInfo.InvariantCulture);
                _nowPlaying.EpisodeNumber = Convert.ToInt32(data["episode"], CultureInfo.InvariantCulture);
                _nowPlaying.Studio = Convert.ToString(data["studio"], CultureInfo.InvariantCulture);

                _nowPlaying.IsPlaying = (Convert.ToString(data["playstatus"], CultureInfo.InvariantCulture) == "playing") ? true : false;
                _nowPlaying.IsPaused = (Convert.ToString(data["playstatus"], CultureInfo.InvariantCulture) == "paused") ? true : false;

                var percent = Math.Floor(100.0 * Convert.ToDouble("0" + data["position"], CultureInfo.CurrentCulture) / Convert.ToDouble("0" + data["duration"], CultureInfo.CurrentCulture));
                if (Double.IsNaN(percent))
                    percent = 0;

                _nowPlaying.Progress = (int) percent;
                var type = Convert.ToString(data["media"], CultureInfo.InvariantCulture);
                switch (type)
                {
                    case "video":
                        _nowPlaying.MediaType = "Movie";
                        break;
                    case "movingpicture":
                        _nowPlaying.MediaType = "Movie";
                        break;
                    case "tvepisode":
                        _nowPlaying.MediaType = "TvShow";
                        break;
                    case "music":
                        _nowPlaying.MediaType = "Audio";
                        break;
                    default:
                        _nowPlaying.MediaType = "Unknown";
                        break;
                }

                var firstaired = Convert.ToString(data["firstaired"], CultureInfo.InvariantCulture);
                if (! string.IsNullOrEmpty(firstaired))
                {
                    var splitaired = firstaired.Split('-');
                    if (splitaired.Length > 2)
                        _nowPlaying.FirstAired =
                            new DateTime(Convert.ToInt32("0" + splitaired[0], CultureInfo.InvariantCulture),
                                         Convert.ToInt32("0" + splitaired[1], CultureInfo.InvariantCulture),
                                         Convert.ToInt32("0" + splitaired[2], CultureInfo.InvariantCulture));

                }
                _nowPlaying.Volume = Convert.ToInt32(data["volume"], CultureInfo.InvariantCulture);
                _nowPlaying.Time = new TimeSpan(0, 0, (int)Convert.ToDouble("0" + ((string)data["position"])));
                _nowPlaying.Duration = new TimeSpan(0, 0, (int)Convert.ToDouble("0" + ((string)data["duration"])));

                _nowPlaying.IsMuted = (_nowPlaying.Volume == 0) ? true : false;
            }


        }

        public ApiCurrently NowPlaying(bool checkNewMedia)
        {
            lock (Locker)
            {
                if (checkNewMedia)
                {
                    if ((_currentMediaFile != _nowPlaying.FileName) || (_currentMediaTitle != _nowPlaying.Title))
                    {
                        _currentMediaTitle = _nowPlaying.Title;
                        _currentMediaFile = _nowPlaying.FileName;
                        _nowPlaying.IsNewMedia = true;
                    }
                    else
                        _nowPlaying.IsNewMedia = false;
                }

                return _nowPlaying;
            }
        }

        public void PlayPause()
        {
            if (_parent.IsConnected())
                _parent.Remote.Play();
        }

        public void Stop()
        {
            if (_parent.IsConnected())
                _parent.Remote.Stop();
        }

        public void SkipPrevious()
        {
            if (_parent.IsConnected())
                _parent.Remote.Previous();
        }
        
        public void SkipNext()
        {
            if (_parent.IsConnected())
                _parent.Remote.Next();
        }

        public void SeekPercentage(int progress)
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpCommand(new CommandInfoIPimp { Action = "seekpercentage", Value = Convert.ToString(progress, CultureInfo.InvariantCulture) });
        }

        public void SetVolume(int percentage)
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpCommand(new CommandInfoIPimp { Action = "volume", Value = Convert.ToString(percentage, CultureInfo.InvariantCulture) });
        }

        public void ToggleMute()
        {
            if (_parent.IsConnected())
                _parent.Remote.ToggleMute();
        }
    }
}
