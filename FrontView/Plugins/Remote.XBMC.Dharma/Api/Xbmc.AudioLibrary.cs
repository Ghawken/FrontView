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
using System.Globalization;
using Plugin;

namespace Remote.XBMC.Dharma.Api
{
    class XbmcAudioLibrary : IApiAudioLibrary
    {
        private readonly Xbmc _parent;

        public XbmcAudioLibrary(Xbmc parent)
        {
            _parent = parent;
        }

        public Collection<ApiAudioGenre> GetGenres()
        {
            var genres = new Collection<ApiAudioGenre>();
            var dblines = _parent.DBCommand("music", "SELECT idGenre,strGenre,COUNT(DISTINCT strAlbum) AS albumCount FROM albumview GROUP BY idGenre");
            if (dblines == null) return genres;
            foreach (var dbline in dblines)
            {
                if (dbline.Length < 3)
                {
                    _parent.Log("Invalid request DATA : " + dbline);
                    continue;
                }
                var genre = new ApiAudioGenre { IdGenre = Xbmc.StringToNumber(dbline[0]), Name = dbline[1], AlbumCount = Xbmc.StringToNumber(dbline[2]) };
                genres.Add(genre);
            }
            return genres;
        }

        public Collection<ApiAudioArtist> GetArtists()
        {
            var artists = new Collection<ApiAudioArtist>();
            var dblines = _parent.DBCommand("music", "SELECT idArtist,strArtist FROM artist");
            if (dblines == null) return artists;
            foreach (var dbline in dblines)
            {
                if (dbline.Length < 2)
                {
                    _parent.Log("Invalid request DATA : " + dbline);
                    continue;
                }
                var hash = Xbmc.Hash(dbline[1]);
                var hash2 = Xbmc.Hash("artist"+dbline[1]);
                var genre = new ApiAudioArtist
                                {
                                    IdArtist = Xbmc.StringToNumber(dbline[0]), 
                                    Name = dbline[1], 
                                    Thumb =  @"special://profile/Thumbnails/Music/Artists/" + hash2 + ".tbn",
                                    Fanart = @"special://profile/Thumbnails/Music/Fanart/" + hash + ".tbn",
                                    Hash = hash2
                                };
                artists.Add(genre);
            }

            dblines = _parent.DBCommand("music", "SELECT idArtist,strBiography FROM artistinfo");
            if (dblines == null) return artists;

            foreach (var dbline in dblines)
            {
                if (dbline.Length < 2)
                {
                    _parent.Log("Invalid request DATA : " + dbline);
                    continue;
                }
                foreach (var artist in artists)
                {
                    if (artist.IdArtist.ToString(CultureInfo.InvariantCulture) == dbline[0])
                    {
                        artist.Biography = dbline[1];
                    }
                }
            }

            return artists;
        }


        public Collection<ApiAudioAlbum> GetAlbums()
        {
            var albums = new Collection<ApiAudioAlbum>();
            const string req = "SELECT idAlbum,strAlbum,idGenre,idArtist,strArtist,strGenre,iYear,strThumb FROM albumview GROUP BY strAlbum";

            var dblines = _parent.DBCommand("music", req);
            if (dblines == null) return albums;
            foreach (var dbline in dblines)
            {
                if (dbline.Length < 8)
                {
                    _parent.Log("Invalid request DATA : " + dbline);
                    continue;
                }
                var album = new ApiAudioAlbum
                                {
                                    IdAlbum = Xbmc.StringToNumber(dbline[0]), 
                                    Title = dbline[1],
                                    IdGenre = Xbmc.StringToNumber(dbline[2]),
                                    IdArtist = Xbmc.StringToNumber(dbline[3]),
                                    Artist = dbline[4],
                                    Genre = dbline[5],
                                    Year = Xbmc.StringToNumber(dbline[6]),
                                    Thumb = dbline[7]
                                };
                var thumbparts = dbline[7].Split('/');
                var hash = thumbparts[thumbparts.Length - 1].Replace(".tbn", "");
                album.Hash = hash;

                albums.Add(album);
            }
            return albums;
        }

        public Collection<ApiAudioSong> GetSongs()
        {
            var songs = new Collection<ApiAudioSong>();
            const string req = "SELECT idSong,strTitle,iTrack,iDuration,iYear,strFileName,idAlbum,strAlbum,strPath,idArtist,strArtist,idGenre,strGenre,strThumb FROM songview";


            var dblines = _parent.DBCommand("music", req);
            if (dblines == null) return songs;
            foreach (var dbline in dblines)
            {
                if (dbline.Length < 14)
                {
                    _parent.Log("Invalid request DATA : " + dbline);
                    continue;
                }
                var song = new ApiAudioSong
                {
                    IdSong = Xbmc.StringToNumber(dbline[0]),
                    Title = dbline[1],
                    Track = Xbmc.StringToNumber(dbline[2]),
                    Duration = Xbmc.StringToNumber(dbline[3]),
                    Year = Xbmc.StringToNumber(dbline[4]),
                    FileName = dbline[5],
                    IdAlbum = Xbmc.StringToNumber(dbline[6]),
                    Album = dbline[7],
                    Path = dbline[8],
                    IdArtist = Xbmc.StringToNumber(dbline[9]),
                    Artist = dbline[10],
                    IdGenre = Xbmc.StringToNumber(dbline[11]),
                    Genre = dbline[12],
                    Thumb = dbline[13],
                };
                var thumbparts = dbline[13].Split('/');
                var hash = thumbparts[thumbparts.Length - 1].Replace(".tbn", "");
                song.Hash = hash;

                songs.Add(song);
            }
            return songs;
        }

    }
}
