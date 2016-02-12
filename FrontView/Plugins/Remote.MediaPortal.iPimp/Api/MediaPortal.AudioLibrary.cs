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
using System.Collections.ObjectModel;
using System.Globalization;
using Jayrock.Json;
using Plugin;

namespace Remote.MediaPortal.iPimp.Api
{
    class MediaPortalAudioLibrary : IApiAudioLibrary
    {
        private readonly MediaPortal _parent;

        public MediaPortalAudioLibrary(MediaPortal parent)
        {
            _parent = parent;
        }

        public Collection<ApiAudioGenre> GetGenres()
        {
            var genres = new Collection<ApiAudioGenre>();

            if (!_parent.IsConnected())
                return genres;
            var dblines = _parent.IPimpDBCommand(new CommandInfoIPimp { Action = "getallmusicgenres" }, "genres");
            if (dblines == null) return genres;

            foreach (JsonObject dbline in dblines)
            {
                var genre = new ApiAudioGenre
                {
                    Thumb = (string)dbline["thumb"],
                    Fanart = (string)dbline["fanart"],
                    Name = (string)dbline["genre"],
                    AlbumCount = Convert.ToInt32("0" + (string)dbline["numalbums"], CultureInfo.InvariantCulture),
                    IdGenre = Convert.ToInt32("0" + (string)dbline["id"], CultureInfo.InvariantCulture)
                };

                genres.Add(genre);
            }

            return genres;
        }

        public Collection<ApiAudioArtist> GetArtists()
        {
            var artists = new Collection<ApiAudioArtist>();
            if (!_parent.IsConnected())
                return artists;
            var dblines = _parent.IPimpDBCommand(new CommandInfoIPimp { Action = "getallmusicartists" }, "artists");
            if (dblines == null) return artists;

            foreach (JsonObject dbline in dblines)
            {
                var artist = new ApiAudioArtist
                {
                    Thumb = (string)dbline["thumb"],
                    Fanart = (string)dbline["fanart"],
                    Hash = MediaPortal.Hash((string)dbline["files"]),
                    Biography = (string)dbline["biography"],
                    IdArtist = Convert.ToInt32("0" + (string)dbline["id"], CultureInfo.InvariantCulture),
                    Name = (string)dbline["name"]
                };

                artists.Add(artist);
            }

            
            return artists;
        }


        public Collection<ApiAudioAlbum> GetAlbums()
        {
            var albums = new Collection<ApiAudioAlbum>();
            if (!_parent.IsConnected())
                return albums;
            var dblines = _parent.IPimpDBCommand(new CommandInfoIPimp { Action = "getallmusicalbums" }, "albums");
            if (dblines == null) return albums;

            foreach (JsonObject dbline in dblines)
            {
                var album = new ApiAudioAlbum
                {
                    IdAlbum = Convert.ToInt32("0" + (string)dbline["id"], CultureInfo.InvariantCulture),
                    Thumb = (string)dbline["thumb"],
                    Fanart = (string)dbline["fanart"],
                    IdGenre = 0,
                    Artist = (string)dbline["albumartist"],
                    Genre = (string)dbline["genre"],
                    Title = (string)dbline["name"],
                    Year = Convert.ToInt32("0" + (string)dbline["year"], CultureInfo.InvariantCulture),
                    Hash = MediaPortal.Hash((string)dbline["name"]),
                    IdArtist = 0
                };

                albums.Add(album);
            }

            return albums;
        }

        public Collection<ApiAudioSong> GetSongs()
        {
            var songs = new Collection<ApiAudioSong>();
            if (!_parent.IsConnected())
                return songs;
            var dblines = _parent.IPimpDBCommand(new CommandInfoIPimp { Action = "getallmusicsongs" }, "songs");
            if (dblines == null) return songs;

            foreach (JsonObject dbline in dblines)
            {
                var song = new ApiAudioSong
                {
                    IdAlbum = Convert.ToInt32("0" + (string)dbline["id"], CultureInfo.InvariantCulture),
                    Thumb = (string)dbline["thumb"],
                    Fanart = (string)dbline["fanart"],
                    IdGenre = 0,
                    Artist = (string)dbline["albumartist"],
                    Genre = (string)dbline["genre"],
                    Title = (string)dbline["song"],
                    Year = Convert.ToInt32("0" + (string)dbline["year"], CultureInfo.InvariantCulture),
                    Hash = MediaPortal.Hash((string)dbline["song"]),
                    Album = (string)dbline["album"],
                    Duration = Convert.ToInt32("0" + (string)dbline["duration"], CultureInfo.InvariantCulture),
                    FileName = (string)dbline["filename"],
                    Track = Convert.ToInt32("0" + (string)dbline["tracknumber"], CultureInfo.InvariantCulture),
                    IdArtist = 0,
                    IdSong = Convert.ToInt32("0" + (string)dbline["id"], CultureInfo.InvariantCulture),
                    Path = ""
                };

                songs.Add(song);
            }

            return songs;
        }

    }
}
