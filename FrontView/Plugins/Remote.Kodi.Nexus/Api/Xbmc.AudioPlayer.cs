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
using System.ComponentModel;
using System.Threading;
using Jayrock.Json;
using Plugin;

namespace Remote.XBMC.Nexus.Api
{
    class XbmcAudioPlayer : IApiAudioPlayer
    {
        private readonly Xbmc _parent;
        private readonly BackgroundWorker _bw = new BackgroundWorker{WorkerSupportsCancellation = true};

        public XbmcAudioPlayer(Xbmc parent)
        {
            _parent = parent;
            _bw.DoWork += AsyncPlayFilesWorker;
        }

        private void AsyncPlayFiles(Collection<ApiAudioSong> songs)
        {
            _bw.CancelAsync();
            while (_bw.IsBusy)
            {
                Thread.Sleep(50);
                System.Windows.Forms.Application.DoEvents();
            }
            _bw.RunWorkerAsync(songs);
        }

        private void AsyncPlayFilesWorker(object sender, DoWorkEventArgs e)
        {
            var songs = (Collection<ApiAudioSong>)e.Argument;

            if (songs == null)
                return;
            if (!_parent.IsConnected())
                return;

            var plId = new JsonObject();
            plId["playlistid"] = 1;
            _parent.JsonCommand("Playlist.Clear", plId);
            var i = 0;
            var args = new JsonObject();
            var items = new JsonObject();
            foreach (var apiAudioSong in songs)
            {
                if (((BackgroundWorker)sender).CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                args["songid"] = apiAudioSong.IdSong;
                items["item"] = args;
                items["playlistid"] = 1;
                _parent.JsonCommand("Playlist.Add", items);
                if (i != 0) continue;
                var item = new JsonObject();
                item["item"] = plId;
                _parent.JsonCommand("Player.Open", item);
                i++;
            }
        }

        public void PlayFiles(Collection<ApiAudioSong> songs)
        {
            if (songs == null)
                return;
            if (!_parent.IsConnected())
                return;

            AsyncPlayFiles(songs);
        }

        public void PlaySong(ApiAudioSong audio)
        {
            if (audio == null)
                return;
            if (!_parent.IsConnected())
                return;
            var args = new JsonObject();
            var items = new JsonObject();
            args["songid"] = audio.IdSong;
            items["item"] = args;
            items["playlistid"] = 1;
            var plId = new JsonObject();
            plId["playlistid"] = 1;
            var item = new JsonObject();
            item["item"] = plId;
            _parent.JsonCommand("Playlist.Clear", plId);
            _parent.JsonCommand("Playlist.Add", items);
            _parent.JsonCommand("Playlist.Play", item);
        }
    }
}
