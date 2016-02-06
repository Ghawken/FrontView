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

using Plugin;

namespace Remote.MediaPortal.iPimp.Api
{
    class MediaPortalRemote : IApiRemote
    {
        private readonly MediaPortal _parent;

        public MediaPortalRemote(MediaPortal parent)
        {
            _parent = parent;
        }

        public void ToggleMute()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("volmute");
        }

        public void Return()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("back");
        }

        public void Enter()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("ok");
        }

        public void Info()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("info");
        }

        public void Home()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("home");
        }

        public void Video()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("videos");
        }

        public void Music()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("music");
        }

        public void Pictures()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("pictures");
        }

        public void Tv()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("tvseries");
        }

        public void VolUp()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("volup");
        }

        public void VolDown()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("voldown");
        }


        public void Menu()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("info");
        }
               
        public void Title()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("home");
        }

        public void Down()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("down");
        }

        public void Up()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("up");
        }

        public void Left()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("left");
        }

        public void Right()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("right");
        }

        public void Mute()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("volmute");
        }

        public void PlayDrive()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("dvd");
        }
                
        public void EjectDrive()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("");
        }

        public void Subtitles()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("teletext");
        }

        public void Previous()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("replay");
        }

        public void Rewind()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("rewind");
        }

        public void Play()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("pause");
        }

        public void Stop()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("stop");
        }

        public void Forward()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("forward");
        }

        public void Next()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("skip");
        }

        public void One()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("1");
        }

        public void Two()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("2");
        }

        public void Three()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("3");
        }

        public void Four()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("4");
        }

        public void Five()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("5");
        }

        public void Six()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("6");
        }

        public void Seven()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("7");
        }

        public void Eight()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("8");
        }

        public void Nine()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("9");
        }

        public void Zero()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("0");
        }

        public void Star()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("*");
        }

        public void Hash()
        {
            if (_parent.IsConnected())
                _parent.AsyncIPimpButton("#");
        }
    }
}
