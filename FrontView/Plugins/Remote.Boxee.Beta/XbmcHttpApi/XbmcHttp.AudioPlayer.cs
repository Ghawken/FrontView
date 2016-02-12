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

using System.Collections.ObjectModel;
using Plugin;

namespace Remote.XBMC.Camelot.XbmcHttpApi
{
    class XbmcHttpAudioPlayer : IApiAudioPlayer
    {
        private readonly XbmcHttp _parent;

        public XbmcHttpAudioPlayer(XbmcHttp parent)
        {
            _parent = parent;
        }


        public void PlayFiles(Collection<ApiAudioSong> songs)
        {
            if (songs == null)
                return;
            if (!_parent.IsConnected())
                return;
            _parent.Command("ClearPlayList(0)");
            _parent.Command("SetCurrentPlaylist(0)");
            var pos = 0;
            foreach (var apiAudioSong in songs)
            {
                if (pos == 0)
                {
                    _parent.Command("PlayFile(" + apiAudioSong.Path + apiAudioSong.FileName + ";0)");
                    pos++;
                }
                else
                {
                    _parent.AsyncCommand("AddToPlayList", apiAudioSong.Path + apiAudioSong.FileName + ";0");
                }
            }
            
        }

        public void PlaySong(ApiAudioSong audio)
        {
            
        }
    }
}
