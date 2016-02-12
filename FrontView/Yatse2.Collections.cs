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
using FrontView.Classes;

namespace FrontView
{
    public class MoviesCollection : ObservableCollection<Yatse2Movie>
    {
        public void Load(Collection<Yatse2Movie> what)
        {
            Clear();
            if (what == null) return;
            foreach (var item in what)
            {
                Add(item);
            }
        }
    }

    public class TvShowsCollection : ObservableCollection<Yatse2TvShow>
    {
        public void Load(Collection<Yatse2TvShow> what)
        {
            Clear();
            if (what == null) return;
            foreach (var item in what)
            {
                Add(item);
            }
        }
    }

    public class TvSeasonsCollection : ObservableCollection<Yatse2TvSeason>
    {
        public void Load(Collection<Yatse2TvSeason> what)
        {
            Clear();
            if (what == null) return;
            foreach (var item in what)
            {
                Add(item);
            }
        }
    }

    public class TvEpisodesCollection : ObservableCollection<Yatse2TvEpisode>
    {
        public void Load(Collection<Yatse2TvEpisode> what)
        {
            Clear();
            if (what == null) return;
            foreach (var item in what)
            {
                Add(item);
            }
        }
    }

    public class AudioGenresCollection : ObservableCollection<Yatse2AudioGenre>
    {
        public void Load(Collection<Yatse2AudioGenre> what)
        {
            Clear();
            if (what == null) return;
            foreach (var item in what)
            {
                Add(item);
            }
        }
    }

    public class AudioArtistsCollection : ObservableCollection<Yatse2AudioArtist>
    {
        public void Load(Collection<Yatse2AudioArtist> what)
        {
            Clear();
            if (what == null) return;
            foreach (var item in what)
            {
                Add(item);
            }
        }
    }

    public class AudioAlbumsCollection : ObservableCollection<Yatse2AudioAlbum>
    {
        public void Load(Collection<Yatse2AudioAlbum> what)
        {
            Clear();
            if (what == null) return;
            foreach (var item in what)
            {
                Add(item);
            }
        }
    }

    public class AudioSongsCollection : ObservableCollection<Yatse2AudioSong>
    {
        public void Load(Collection<Yatse2AudioSong> what)
        {
            Clear();
            if (what == null) return;
            foreach (var item in what)
            {
                Add(item);
            }
        }
    }




}