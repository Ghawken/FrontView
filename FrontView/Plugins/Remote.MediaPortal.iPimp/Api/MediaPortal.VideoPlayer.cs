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

using System.Globalization;
using Plugin;

namespace Remote.MediaPortal.iPimp.Api
{
    class MediaPortalVideoPlayer : IApiVideoPlayer
    {
        private readonly MediaPortal _parent;

        public MediaPortalVideoPlayer(MediaPortal parent)
        {
            _parent = parent;
        }

        public void PlayMovie(ApiMovie video)
        {
            if (video == null)
                return;
            if (!_parent.IsConnected())
                return;
            if (video.IdFile == 0)
                _parent.AsyncIPimpCommand(new CommandInfoIPimp { Action = "playmovie", Filter = video.IdMovie.ToString(CultureInfo.InvariantCulture) , Value = "force" , Tracks = "no" });
            if (video.IdFile == 1)
                _parent.AsyncIPimpCommand(new CommandInfoIPimp { Action = "playmovingpicture", Filter = video.IdMovie.ToString(CultureInfo.InvariantCulture), Value = "force", Tracks = "no" });
        }

        public void PlayTvEpisode(ApiTvEpisode tvepisode)
        {
            if (tvepisode == null)
                return;
            if (!_parent.IsConnected())
                return;
            _parent.AsyncIPimpCommand(new CommandInfoIPimp { Action = "playepisode", Value = tvepisode.Path, Tracks = "no" });
        }
    }
}
