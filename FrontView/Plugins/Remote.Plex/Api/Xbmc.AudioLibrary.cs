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
using Jayrock.Json;
using System;

namespace Remote.Plex.Api
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

      var properties = new JsonArray(new[] { "title", "thumbnail" });
      var param = new JsonObject();
      param["properties"] = properties;
      var result = (JsonObject)_parent.JsonCommand("AudioLibrary.GetGenres", param);
      if (result != null)
      {
        if (result.Contains("genres"))
        {
          foreach (JsonObject genre in (JsonArray)result["genres"])
          {
            try
            {
              var gen = new ApiAudioGenre
                {
                  IdGenre = (long)(JsonNumber)genre["genreid"],
                  Name = genre["title"].ToString(),
                  AlbumCount = 0,
                  Thumb = genre["thumbnail"].ToString()
                };
              genres.Add(gen);
            }
            catch (Exception)
            {
            }
          }
        }
      }
      return genres;
    }

    public Collection<ApiAudioArtist> GetArtists()
    {
      var artists = new Collection<ApiAudioArtist>();

      var properties = new JsonArray(new[] { "thumbnail", "fanart", "description" });
      var param = new JsonObject();
      param["properties"] = properties;
      var result = (JsonObject)_parent.JsonCommand("AudioLibrary.GetArtists", param);
      if (result != null)
      {
        if (result.Contains("artists"))
        {
          foreach (JsonObject genre in (JsonArray)result["artists"])
          {
            try
            {
              var artist = new ApiAudioArtist
                {
                  IdArtist = (long)(JsonNumber)genre["artistid"],
                  Name = genre["artist"].ToString(),
                  Thumb = genre["thumbnail"].ToString(),
                  Fanart = genre["fanart"].ToString(),
                  Biography = genre["description"].ToString()
                };
              artists.Add(artist);
            }
            catch (Exception)
            {
            }
          }
        }
      }
      return artists;
    }


    public Collection<ApiAudioAlbum> GetAlbums()
    {
      var albums = new Collection<ApiAudioAlbum>();

      var properties = new JsonArray(new[] { "title", "thumbnail", "genre", "genreid", "artist", "year" });
      var param = new JsonObject();
      param["properties"] = properties;
      var result = (JsonObject)_parent.JsonCommand("AudioLibrary.GetAlbums", param);
      if (result != null)
      {
        if (result.Contains("albums"))
        {
          foreach (JsonObject genre in (JsonArray)result["albums"])
          {
            try
            {
              var album = new ApiAudioAlbum
                {
                  IdAlbum = (long)(JsonNumber)genre["albumid"],
                  Title = genre["title"].ToString(),
                  IdGenre = 0,
                  IdArtist = 0,
                  Artist = _parent.JsonArrayToString((JsonArray)genre["artist"]),
                  Genre = _parent.JsonArrayToString((JsonArray)genre["genre"]),
                  Year = (long)(JsonNumber)genre["year"],
                  Thumb = genre["thumbnail"].ToString()
                };
              albums.Add(album);
            }
            catch (Exception)
            {
            }
          }
        }
      }
      return albums;
    }

    public Collection<ApiAudioSong> GetSongs()
    {
      var songs = new Collection<ApiAudioSong>();

      var properties = new JsonArray(new[] { "title", "thumbnail", "genre", "genreid", "artist", "year", "duration", "album", "albumid", "track" });
      var param = new JsonObject();
      param["properties"] = properties;
      var result = (JsonObject)_parent.JsonCommand("AudioLibrary.GetSongs", param);
      if (result != null)
      {
        if (result.Contains("songs"))
        {
          foreach (JsonObject genre in (JsonArray)result["songs"])
          {
            try
            {
              var song = new ApiAudioSong
                {
                  IdSong = (long)(JsonNumber)genre["songid"],
                  Title = genre["title"].ToString(),
                  Track = (long)(JsonNumber)genre["track"],
                  Duration = (long)(JsonNumber)genre["duration"],
                  Year = (long)(JsonNumber)genre["year"],
                  FileName = "",
                  IdAlbum = (long)(JsonNumber)genre["albumid"],
                  Album = genre["album"].ToString(),
                  Path = "",
                  IdArtist = 0,
                  Artist = _parent.JsonArrayToString((JsonArray)genre["artist"]),
                  IdGenre = 0,
                  Genre = _parent.JsonArrayToString((JsonArray)genre["genre"]),
                  Thumb = genre["thumbnail"].ToString(),
                };
              songs.Add(song);
            }
            catch (Exception)
            {
            }
          }
        }
      }

      return songs;
    }

  }
}
