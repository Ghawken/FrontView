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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Plugin;

namespace Remote.MediaPortal.iPimp.Api
{
    class MediaPortalAudioPlayer : IApiAudioPlayer
    {
        private readonly MediaPortal _parent;

        public MediaPortalAudioPlayer(MediaPortal parent)
        {
            _parent = parent;
        }


        public void PlayFiles(Collection<ApiAudioSong> songs)
        {
            if (songs == null)
                return;
            if (!_parent.IsConnected())
                return;

            var tracks = new List<string>();

            foreach (var apiAudioSong in songs)
            {
                tracks.Add(apiAudioSong.IdSong.ToString(CultureInfo.InvariantCulture));
            }

            _parent.IPimpCommand(new CommandInfoIPimp { Action = "playtracks", Tracks = string.Join(",", tracks.ToArray()), Shuffle = false, Enqueue = false });
        }

        public void PlaySong(ApiAudioSong audio)
        {
            
        }
    }
}
