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

namespace Remote.XBMC.Camelot.XbmcHttpApi
{
    class XbmcHttpRemote : IApiRemote
    {
        private readonly XbmcHttp _parent;

        public XbmcHttpRemote(XbmcHttp parent)
        {
            _parent = parent;
        }

        public int GetVolume()
        {
            var ret = _parent.Command("GetVolume()");
            return ret == null ? 0 : Convert.ToInt32(ret[0], CultureInfo.InvariantCulture);
        }

        public void SetVolume(int volumepercent)
        {
            _parent.AsyncCommand("ExecBuiltIn", "SetVolume(" + Convert.ToString(volumepercent, CultureInfo.InvariantCulture) + ")");
        }

        public void ToggleMute()
        {
            _parent.AsyncCommand("Mute()", "");
        }

        public void Return()
        {
            _parent.AsyncEventButton("back");
        }

        public void Enter()
        {
            _parent.AsyncEventButton("select");
        }

        public void Info()
        {
            _parent.AsyncEventButton("info");
        }

        public void Home()
        {
            _parent.AsyncEventButton("start");
        }

        public void Video()
        {
            _parent.AsyncEventButton("myvideo");
        }

        public void Music()
        {
            _parent.AsyncEventButton("mymusic");
        }

        public void Pictures()
        {
            _parent.AsyncEventButton("mypictures");
        }

        public void Tv()
        {
            _parent.AsyncEventButton("mytv");
        }

        public void VolUp()
        {
            _parent.AsyncEventButton("volumeplus");
        }

        public void VolDown()
        {
            _parent.AsyncEventButton("volumeminus");
        }


        public void Menu()
        {
            _parent.AsyncEventButton("menu");
        }
               
        public void Title()
        {
            _parent.AsyncEventButton("title");
        }

        public void Down()
        {
            _parent.AsyncEventButton("down");
        }

        public void Up()
        {
            _parent.AsyncEventButton("up");
        }

        public void Left()
        {
            _parent.AsyncEventButton("left");
        }

        public void Right()
        {
            _parent.AsyncEventButton("right");
        }

        public void Mute()
        {
            _parent.AsyncEventAction("Mute");
        }

        public void PlayDrive()
        {
            _parent.AsyncEventAction("PlayDVD");
        }
                
        public void EjectDrive()
        {
            _parent.AsyncEventAction("EjectTray");
        }

        public void Subtitles()
        {
            _parent.AsyncEventButton("subtitle");
        }

        public void Previous()
        {
            _parent.AsyncEventButton("skipminus");
        }

        public void Rewind()
        {
            _parent.AsyncEventButton("reverse");
        }

        public void Play()
        {
            _parent.AsyncEventButton("play");
        }

        public void Stop()
        {
            _parent.AsyncEventButton("stop");
        }

        public void Forward()
        {
            _parent.AsyncEventButton("forward");
        }

        public void Next()
        {
            _parent.AsyncEventButton("skipplus");
        }

        public void One()
        {
            _parent.AsyncEventButton("one");
        }

        public void Two()
        {
            _parent.AsyncEventButton("two");
        }

        public void Three()
        {
            _parent.AsyncEventButton("three");
        }

        public void Four()
        {
            _parent.AsyncEventButton("four");
        }

        public void Five()
        {
            _parent.AsyncEventButton("five");
        }

        public void Six()
        {
            _parent.AsyncEventButton("six");
        }

        public void Seven()
        {
            _parent.AsyncEventButton("seven");
        }

        public void Eight()
        {
            _parent.AsyncEventButton("eight");
        }

        public void Nine()
        {
            _parent.AsyncEventButton("nine");
        }

        public void Zero()
        {
            _parent.AsyncEventButton("zero");
        }

        public void Star()
        {
            _parent.AsyncEventButton("star");
        }

        public void Hash()
        {
            _parent.AsyncEventButton("hash");
        }
    }
}
