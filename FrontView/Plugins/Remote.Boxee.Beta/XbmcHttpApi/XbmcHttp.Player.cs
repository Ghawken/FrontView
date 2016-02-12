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

namespace Remote.XBMC.Camelot.XbmcHttpApi
{
    class XbmcHttpPlayer : IApiPlayer
    {
        private readonly XbmcHttp _parent;
        private string _currentMediaFile;
        private string _currentMediaTitle;
        private bool _isNewMedia;
        private bool _isPlaying;
        private bool _isPaused;
        private bool _isMuted;
        private int _volume;
        private int _progress;
        private readonly Hashtable _currentInfo = new Hashtable();

        public XbmcHttpPlayer(XbmcHttp parent)
        {
            _parent = parent;
        }

        private void RefreshCurrent()
        {
            _currentInfo.Clear();
            if (!_parent.IsConnected()) return;

            var infos = _parent.Command("GetCurrentlyPlaying");
            if (infos == null)
                return;
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
                //var hash = ApiHelper.GetHashFromFileName(GetInfo("thumb"), "Xbmc HTTP");
                _currentInfo.Add("fanart", GetInfo("thumb").Replace("/"+hash[0]+"/","/Fanart/"));
            }
        }

        public void RefreshNowPlaying()
        {
            if (!_parent.IsConnected()) return;
            RefreshCurrent();
            var aVolume = _parent.Command("GetVolume");
            var aProgress = _parent.Command("GetPercentage");

            if (_currentMediaFile != GetInfo("filename") || (_currentMediaTitle != GetInfo("title")))
            {
                _currentMediaTitle = GetInfo("title");
                _currentMediaFile = GetInfo("filename");
                _isNewMedia = true;
            }
            else
                _isNewMedia = false;
            _isPlaying = (GetInfo("playstatus") == "Playing") ? true : false;
            _isPaused = (GetInfo("playstatus") == "Paused") ? true : false;

            if (aVolume == null || aVolume.Length == 0 || aVolume[0] == "Error")
                _volume = 0;
            else
                _volume = Convert.ToInt32(aVolume[0],CultureInfo.InvariantCulture);

            if (aProgress == null || aProgress.Length == 0 || aProgress[0] == "Error" || aProgress[0] == "0" || Convert.ToInt32(aProgress[0], CultureInfo.InvariantCulture) > 99)
                _progress = 1;
            else
                _progress = Convert.ToInt32(aProgress[0], CultureInfo.InvariantCulture);

            _isMuted = (_volume == 0) ? true : false;
        }

        public ApiCurrently NowPlaying(bool checkNewMedia)
        {
            var nowPlaying = new ApiCurrently();
            // TODO : Correct implementation
            return nowPlaying;
        }

        public bool IsPlaying()
        {
            return _isPlaying;
        }

        public bool IsPaused()
        {
            return _isPaused;
        }

        public bool IsMuted()
        {
            return _isMuted;
        }

        public bool IsNewMedia()
        {
            return _isNewMedia;
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
            _parent.AsyncEventAction("PlayerControl(Play)");
        }

        public void Stop()
        {
            _parent.AsyncEventAction("PlayerControl(Stop)");
        }

        public void SkipPrevious()
        {
            _parent.AsyncEventAction("PlayerControl(Previous)");
        }
        
        public void SkipNext()
        {
            _parent.AsyncEventAction("PlayerControl(Next)");
        }

        public void PlayFile(string filename)
        {
            _parent.AsyncCommand("PlayFile" , filename);
        }



        public void SeekPercentage(int progress)
        {
            _parent.AsyncCommand("SeekPercentage", Convert.ToString(progress, CultureInfo.InvariantCulture));
        }

        public void SetVolume(int percentage)
        {
            _parent.AsyncEventAction("SetVolume(" + Convert.ToString(percentage, CultureInfo.InvariantCulture) + ")");
        }

        public void ToggleMute()
        {
            _parent.AsyncEventAction("Mute");
        }
    }
}
