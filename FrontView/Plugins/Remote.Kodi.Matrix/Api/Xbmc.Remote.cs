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

using Jayrock.Json;
using Plugin;

namespace Remote.XBMC.Matrix.Api
{
    class XbmcRemote : IApiRemote
    {
        private readonly Xbmc _parent;

        public XbmcRemote(Xbmc parent)
        {
            _parent = parent;
        }

        public void ToggleMute()
        {
            if (_parent.MpcLoaded)
                _parent.MpcHcRemote.ToggleMute();
            else
                if (_parent.IsConnected())
                {
                    var mute = new JsonObject();
                    mute["mute"] = "toggle";
                    _parent.AsyncJsonCommand("Application.SetMute", mute);
                }
        }

        public void Return()
        {
            if (_parent.MpcLoaded)
                _parent.MpcHcRemote.Return();
            else
                if (_parent.IsConnected())
                    _parent.AsyncEventButton("back");
        }

        public void Enter()
        {
            if (_parent.MpcLoaded)
                _parent.MpcHcRemote.Enter();
            else
                if (_parent.IsConnected())
                    _parent.AsyncEventButton("select");
        }

        public void Info()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventButton("info");
        }

        public void Home()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventButton("start");
        }

        public void Video()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventButton("myvideo");
        }

        public void Music()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventButton("mymusic");
        }

        public void Pictures()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventButton("mypictures");
        }

        public void Tv()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventButton("mytv");
        }

        public void VolUp()
        {
            if (_parent.MpcLoaded)
                _parent.MpcHcRemote.VolUp();
            else
                if (_parent.IsConnected())
                    _parent.AsyncEventButton("volumeplus");
        }

        public void VolDown()
        {
            if (_parent.MpcLoaded)
                _parent.MpcHcRemote.VolDown();
            else
                if (_parent.IsConnected())
                    _parent.AsyncEventButton("volumeminus");
        }


        public void Menu()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventButton("menu");
        }
               
        public void Title()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventButton("title");
        }

        public void Down()
        {
            if (_parent.MpcLoaded)
                _parent.MpcHcRemote.Down();
            else
                if (_parent.IsConnected())
                    _parent.AsyncEventButton("down");
        }

        public void Up()
        {
            if (_parent.MpcLoaded)
                _parent.MpcHcRemote.Up();
            else
                if (_parent.IsConnected())
                    _parent.AsyncEventButton("up");
        }

        public void Left()
        {
            if (_parent.MpcLoaded)
                _parent.MpcHcRemote.Left();
            else
                if (_parent.IsConnected())
                    _parent.AsyncEventButton("left");
        }

        public void Right()
        {
            if (_parent.MpcLoaded)
                _parent.MpcHcRemote.Right();
            else
                if (_parent.IsConnected())
                    _parent.AsyncEventButton("right");
        }

        public void Mute()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventAction("Mute");
        }

        public void PlayDrive()
        {
            if (_parent.MpcLoaded)
                _parent.MpcHcRemote.PlayDrive();
            else
                if (_parent.IsConnected())
                    _parent.AsyncEventAction("PlayDVD");
        }
                
        public void EjectDrive()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventAction("EjectTray");
        }

        public void Subtitles()
        {
            if (_parent.MpcLoaded)
                _parent.MpcHcRemote.Subtitles();
            else
                if (_parent.IsConnected())
                    _parent.AsyncEventButton("subtitle");
        }

        public void Previous()
        {
            if (_parent.MpcLoaded)
                _parent.MpcHcRemote.SkipPrevious();
            else
                if (_parent.IsConnected())
                    _parent.AsyncEventButton("skipminus");
        }

        public void Rewind()
        {
            if (_parent.MpcLoaded)
                _parent.MpcHcRemote.Rewind();
            else
                if (_parent.IsConnected())
                    _parent.AsyncEventButton("reverse");
        }

        public void Play()
        {
            if (_parent.MpcLoaded)
                _parent.MpcHcRemote.Play();
            else
                if (_parent.IsConnected())
                    _parent.AsyncEventButton("play");
        }

        public void Stop()
        {
            if (_parent.MpcLoaded)
                _parent.MpcHcRemote.Stop();
            else
                if (_parent.IsConnected())
                    _parent.AsyncEventButton("stop");
        }

        public void Forward()
        {
            if (_parent.MpcLoaded)
                _parent.MpcHcRemote.Forward();
            else
                if (_parent.IsConnected())
                    _parent.AsyncEventButton("forward");
        }

        public void Next()
        {
            if (_parent.MpcLoaded)
                _parent.MpcHcRemote.SkipNext();
            else
                if (_parent.IsConnected())
                    _parent.AsyncEventButton("skipplus");
        }

        public void One()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventButton("one");
        }

        public void Two()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventButton("two");
        }

        public void Three()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventButton("three");
        }

        public void Four()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventButton("four");
        }

        public void Five()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventButton("five");
        }

        public void Six()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventButton("six");
        }

        public void Seven()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventButton("seven");
        }

        public void Eight()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventButton("eight");
        }

        public void Nine()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventButton("nine");
        }

        public void Zero()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventButton("zero");
        }

        public void Star()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventButton("star");
        }

        public void Hash()
        {
            if (_parent.IsConnected())
                _parent.AsyncEventButton("hash");
        }
    }
}
