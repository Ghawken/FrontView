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
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text.RegularExpressions;
using Setup;
using FrontView.Libs;

namespace FrontView.Classes
{

    public class Yatse2DB : IDisposable
    {
        private string _dbName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\FrontView+\FrontView+.db";


        private bool _debug;
        private SQLiteConnection _dbConnection;
        private SQLiteTransaction _dbTrans;
        private bool _connected;
        private bool _bulkInsert;
        private const int DBVersion = 4;

        [SQLiteFunction(FuncType = FunctionType.Collation, Name = "IGNORESORTTOKENS")]
        public class SqLiteIgnoreSortTokensCollation : SQLiteFunction
        {

            public static bool IgnoreTokens;
            public static string Tokens;

            public override int Compare(string x, string y)
            {
                if (IgnoreTokens)
                {
                    x = Regex.Replace(x, @"^(" + Tokens + ")", "");
                    y = Regex.Replace(y, @"^(" + Tokens + ")", "");
                    return string.Compare(x, y);
                }
                return string.Compare(x, y);
            }
        }



        public void SetDebug(bool debug)
        {
            _debug = debug;
        }

        public bool Open(string dbName, bool ignoreTokens, string tokens)
        {
            if (dbName != null)
                _dbName = dbName;

            SQLiteFunction.RegisterFunction(typeof(SqLiteIgnoreSortTokensCollation));
            SqLiteIgnoreSortTokensCollation.IgnoreTokens = ignoreTokens;
            SqLiteIgnoreSortTokensCollation.Tokens = tokens;
            Log("Opening database : " + _dbName);
            try
            {
                var sqlcsb = new SQLiteConnectionStringBuilder {DataSource = _dbName, FailIfMissing = false};
                _dbConnection = new SQLiteConnection(sqlcsb.ToString());
                _dbConnection.Open();
                _connected = true;
                return true;
            }
            catch (SQLiteException e)
            {
                Log(e.Message);
            }
            return false;
        }

        public void SetBulkInsert(bool bulk)
        {
            _bulkInsert = bulk;
        }

        public void BeginTransaction()
        {
            _dbTrans = _dbConnection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _dbTrans.Commit();
            _dbTrans.Dispose();
        }

        public bool Compress()
        {
            if (!_connected) return false;
            Log("Compress database");
            var sqlCmd = _dbConnection.CreateCommand();
            try
            {
                sqlCmd.CommandText = "VACUUM;";
                LogQuery(sqlCmd);
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                return true;
            }
            catch (SQLiteException e)
            {
                Log(e.Message);
            }
            sqlCmd.Dispose();
            return false;
        }

        private Collection<Yatse2AudioArtist> QueryAudioArtist(SQLiteCommand query)
        {
            var audioartists = new Collection<Yatse2AudioArtist>();
            if (!_connected) return audioartists;
            try
            {
                LogQuery(query);
                var sqldReader = query.ExecuteReader();
                while (sqldReader.Read())
                {
                    var audioArtist = new Yatse2AudioArtist
                    {
                        Id = GetLong(sqldReader, "Id"),
                        IdRemote = GetLong(sqldReader, "IdRemote"),
                        IdArtist = GetLong(sqldReader, "IdArtist"),
                        Name = GetString(sqldReader, "Name"),
                        Biography = GetString(sqldReader, "Biography"),
                        Thumb = GetString(sqldReader, "Thumb"),
                        Fanart = GetString(sqldReader, "Fanart"),
                        Hash = GetString(sqldReader, "Hash"),
                        IsFavorite = GetLong(sqldReader, "IsFavorite")
                    };
                    audioartists.Add(audioArtist);
                }
                sqldReader.Dispose();
                query.Dispose();
            }
            catch (SQLiteException e)
            {
                Log(e.Message);
            }
            return audioartists;
        }

        public Collection<Yatse2AudioArtist> GetAudioArtist(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `AudioArtists` WHERE IdRemote = @idRemote ORDER BY Name;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);

            return QueryAudioArtist(sqlCmd);
        }

        public Collection<Yatse2AudioArtist> GetAudioArtistNoCompilation(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `AudioArtists` WHERE IdRemote = @idRemote AND Name IN (SELECT Artist FROM `AudioAlbums` WHERE IdRemote = @idRemote) ORDER BY Name;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);

            return QueryAudioArtist(sqlCmd);
        }

        public Collection<Yatse2AudioArtist> GetAudioArtistFromGenre(long idRemote,string genre)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `AudioArtists` WHERE IdRemote = @idRemote AND Name IN (SELECT DISTINCT(Artist) FROM AudioSongs WHERE IdRemote = @idRemote AND Genre = @Genre ) ORDER BY Name;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);
            sqlCmd.Parameters.AddWithValue("@Genre", genre);

            return QueryAudioArtist(sqlCmd);
        }

        public Collection<Yatse2AudioArtist> GetAudioArtistFromGenreNoCompilation(long idRemote,string genre)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `AudioArtists` WHERE IdRemote = @idRemote AND Name IN (SELECT Artist FROM `AudioAlbums` WHERE IdRemote = @idRemote AND Genre = @Genre) ORDER BY Name;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);
            sqlCmd.Parameters.AddWithValue("@Genre", genre);

            return QueryAudioArtist(sqlCmd);
        }


        public Collection<Yatse2AudioArtist> GetAudioArtistFromName(long idRemote, string artist)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `AudioArtists` WHERE IdRemote = @idRemote AND Name = @Artist;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);
            sqlCmd.Parameters.AddWithValue("@Artist", artist);

            return QueryAudioArtist(sqlCmd);
        }

        public Collection<Yatse2AudioArtist> GetAudioArtistFavorites(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `AudioArtists` WHERE IdRemote = @idRemote AND IsFavorite > 0;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);

            return QueryAudioArtist(sqlCmd);
        }

        public long InsertAudioArtist(Yatse2AudioArtist audioArtist)
        {
            if (audioArtist == null)
                return 0;
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                @"INSERT INTO `AudioArtists` ( IdRemote ,IdArtist, Name, Thumb, Fanart, Hash, IsFavorite, Biography ) 
                  VALUES (?, ? ,? ,? ,? ,?, ?, ?);";
            sqlCmd.Parameters.AddWithValue("a1", audioArtist.IdRemote);
            sqlCmd.Parameters.AddWithValue("a2", audioArtist.IdArtist);
            sqlCmd.Parameters.AddWithValue("a3", audioArtist.Name);
            sqlCmd.Parameters.AddWithValue("a4", audioArtist.Thumb);
            sqlCmd.Parameters.AddWithValue("a5", audioArtist.Fanart);
            sqlCmd.Parameters.AddWithValue("a6", audioArtist.Hash);
            sqlCmd.Parameters.AddWithValue("a7", audioArtist.IsFavorite);
            sqlCmd.Parameters.AddWithValue("a8", audioArtist.Biography);
            
            return QueryInsert(sqlCmd);
        }

        public bool UpdateAudioArtist(Yatse2AudioArtist audioArtist)
        {
            if (audioArtist == null)
                return false;
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                @"UPDATE `AudioArtists` SET IdRemote = ?,IdArtist = ?, Name = ?, Thumb = ?, Fanart = ?, Hash = ?, IsFavorite = ?, Biography = ? WHERE Id = @Id;";
            sqlCmd.Parameters.AddWithValue("a1", audioArtist.IdRemote);
            sqlCmd.Parameters.AddWithValue("a2", audioArtist.IdArtist);
            sqlCmd.Parameters.AddWithValue("a3", audioArtist.Name);
            sqlCmd.Parameters.AddWithValue("a4", audioArtist.Thumb);
            sqlCmd.Parameters.AddWithValue("a5", audioArtist.Fanart);
            sqlCmd.Parameters.AddWithValue("a6", audioArtist.Hash);
            sqlCmd.Parameters.AddWithValue("a7", audioArtist.IsFavorite);
            sqlCmd.Parameters.AddWithValue("a8", audioArtist.Biography);
            sqlCmd.Parameters.AddWithValue("@Id", audioArtist.Id);

            return Query(sqlCmd);
        }

        public bool DeleteAudioArtist(long id)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText = "DELETE FROM `AudioArtists` WHERE Id = @id;";
            sqlCmd.Parameters.AddWithValue("@id", id);

            return Query(sqlCmd);
        }

        public bool DeleteRemoteAudioArtist(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText = "DELETE FROM `AudioArtists` WHERE IdRemote = @idRemote;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);

            return Query(sqlCmd);
        }

        private Collection<Yatse2AudioGenre> QueryAudioGenre(SQLiteCommand query)
        {
            var audiogenres = new Collection<Yatse2AudioGenre>();
            if (!_connected) return audiogenres;
            try
            {
                LogQuery(query);
                var sqldReader = query.ExecuteReader();
                while (sqldReader.Read())
                {
                    var audioGenre = new Yatse2AudioGenre
                    {
                        Id = GetLong(sqldReader, "Id"),
                        IdRemote = GetLong(sqldReader, "IdRemote"),
                        Name = GetString(sqldReader, "Name"),
                        IdGenre = GetLong(sqldReader, "IdGenre"),
                        Thumb = GetString(sqldReader, "Thumb"),
                        Fanart = GetString(sqldReader, "Fanart"),
                        AlbumCount = GetLong(sqldReader, "AlbumCount"),
                        IsFavorite = GetLong(sqldReader, "IsFavorite")
                    };
                    audiogenres.Add(audioGenre);
                }
                sqldReader.Dispose();
                query.Dispose();
            }
            catch (SQLiteException e)
            {
                Log(e.Message);
            }
            return audiogenres;
        }

        public Collection<Yatse2AudioGenre> GetAudioGenre(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `AudioGenres` WHERE IdRemote = @idRemote ORDER BY Name;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);

            return QueryAudioGenre(sqlCmd);
        }

        public Collection<Yatse2AudioGenre> GetAudioGenreNotEmpty(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `AudioGenres` WHERE IdRemote = @idRemote AND Name IN (SELECT Genre FROM `AudioAlbums` WHERE IdRemote = @idRemote) ORDER BY Name;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);

            return QueryAudioGenre(sqlCmd);
        }

        public Collection<Yatse2AudioGenre> GetAudioGenreFavorites(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `AudioGenres` WHERE IdRemote = @idRemote AND IsFavorite > 0 ORDER BY Name;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);

            return QueryAudioGenre(sqlCmd);
        }

        public long InsertAudioGenre(Yatse2AudioGenre audioGenre)
        {
            if (audioGenre == null)
                return 0;
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                @"INSERT INTO `AudioGenres` ( IdRemote ,Name,IdGenre,Thumb,Fanart,AlbumCount, IsFavorite ) 
                  VALUES (?, ? ,? ,? ,? ,? ,?);";
            sqlCmd.Parameters.AddWithValue("a1", audioGenre.IdRemote);
            sqlCmd.Parameters.AddWithValue("a2", audioGenre.Name);
            sqlCmd.Parameters.AddWithValue("a3", audioGenre.IdGenre);
            sqlCmd.Parameters.AddWithValue("a4", audioGenre.Thumb);
            sqlCmd.Parameters.AddWithValue("a5", audioGenre.Fanart);
            sqlCmd.Parameters.AddWithValue("a6", audioGenre.AlbumCount);
            sqlCmd.Parameters.AddWithValue("a7", audioGenre.IsFavorite);

            return QueryInsert(sqlCmd);
        }

        public bool UpdateAudioGenre(Yatse2AudioGenre audioGenre)
        {
            if (audioGenre == null)
                return false;
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                @"UPDATE `AudioGenres` SET IdRemote = ? ,Name = ? ,IdGenre = ? ,Thumb = ? ,Fanart = ? ,AlbumCount = ? , IsFavorite = ? WHERE Id = @Id;";
            sqlCmd.Parameters.AddWithValue("a1", audioGenre.IdRemote);
            sqlCmd.Parameters.AddWithValue("a2", audioGenre.Name);
            sqlCmd.Parameters.AddWithValue("a3", audioGenre.IdGenre);
            sqlCmd.Parameters.AddWithValue("a4", audioGenre.Thumb);
            sqlCmd.Parameters.AddWithValue("a5", audioGenre.Fanart);
            sqlCmd.Parameters.AddWithValue("a6", audioGenre.AlbumCount);
            sqlCmd.Parameters.AddWithValue("a7", audioGenre.IsFavorite);
            sqlCmd.Parameters.AddWithValue("@Id", audioGenre.Id);

            return Query(sqlCmd);
        }

        public bool DeleteAudioGenre(long id)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText = "DELETE FROM `AudioGenres` WHERE Id = @id;";
            sqlCmd.Parameters.AddWithValue("@id", id);

            return Query(sqlCmd);
        }

        public bool DeleteRemoteAudioGenre(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText = "DELETE FROM `AudioGenres` WHERE IdRemote = @idRemote;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);

            return Query(sqlCmd);
        }

        private Collection<Yatse2AudioSong> QueryAudioSong(SQLiteCommand query)
        {
            var audioSongs = new Collection<Yatse2AudioSong>();
            if (!_connected) return audioSongs;
            try
            {
                LogQuery(query);
                var sqldReader = query.ExecuteReader();
                while (sqldReader.Read())
                {
                    var audioSong = new Yatse2AudioSong
                    {
                        Id = GetLong(sqldReader, "Id"),
                        IdRemote = GetLong(sqldReader, "IdRemote"),
                        IdSong = GetLong(sqldReader, "IdSong"),
                        Title = GetString(sqldReader, "Title"),
                        Track = GetLong(sqldReader, "Track"),
                        Duration = GetLong(sqldReader, "Duration"),
                        Year = GetLong(sqldReader, "Year"),
                        FileName = GetString(sqldReader, "FileName"),
                        IdAlbum = GetLong(sqldReader, "IdAlbum"),
                        IdArtist = GetLong(sqldReader, "IdArtist"),
                        Artist = GetString(sqldReader, "Artist"),
                        Genre = GetString(sqldReader, "Genre"),
                        IdGenre = GetLong(sqldReader, "IdGenre"),
                        Album = GetString(sqldReader, "Album"),
                        Path = GetString(sqldReader, "Path"),
                        Thumb = GetString(sqldReader, "Thumb"),
                        Fanart = GetString(sqldReader, "Thumb"),
                        Hash = GetString(sqldReader, "Hash"),
                        IsFavorite = GetLong(sqldReader, "IsFavorite")
                    };
                    audioSongs.Add(audioSong);
                }
                sqldReader.Dispose();
                query.Dispose();
            }
            catch (SQLiteException e)
            {
                Log(e.Message);
            }
            return audioSongs;
        }

        public Collection<Yatse2AudioSong> GetAudioSong(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `AudioSongs` WHERE IdRemote = @idRemote ORDER BY Track;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);

            return QueryAudioSong(sqlCmd);
        }

        public Collection<Yatse2AudioSong> GetAudioSongFavorites(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `AudioSongs` WHERE IdRemote = @idRemote AND IsFavorite > 0 ORDER BY Track;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);

            return QueryAudioSong(sqlCmd);
        }

        public Collection<Yatse2AudioSong> GetAudioSongFromAlbumName(long idRemote, string album)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `AudioSongs` WHERE IdRemote = @idRemote AND Album = @Album ORDER BY Track;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);
            sqlCmd.Parameters.AddWithValue("@Album", album);

            return QueryAudioSong(sqlCmd);
        }

        public Collection<Yatse2AudioSong> GetAudioSongFromAlbumNameRandom(long idRemote, string album)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `AudioSongs` WHERE IdRemote = @idRemote AND Album = @Album ORDER BY RANDOM();";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);
            sqlCmd.Parameters.AddWithValue("@Album", album);

            return QueryAudioSong(sqlCmd);
        }

        public Collection<Yatse2AudioSong> GetAudioSongFromGenre(long idRemote, string genre)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `AudioSongs` WHERE IdRemote = @idRemote AND Genre = @Genre ORDER BY IdAlbum,Track;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);
            sqlCmd.Parameters.AddWithValue("@Genre", genre);

            return QueryAudioSong(sqlCmd);
        }

        public Collection<Yatse2AudioSong> GetAudioSongFromGenreRandom(long idRemote, string genre)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `AudioSongs` WHERE IdRemote = @idRemote AND Genre = @Genre ORDER BY RANDOM();";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);
            sqlCmd.Parameters.AddWithValue("@Genre", genre);

            return QueryAudioSong(sqlCmd);
        }

        public Collection<Yatse2AudioSong> GetAudioSongFromArtist(long idRemote, string artist)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `AudioSongs` WHERE IdRemote = @idRemote AND Artist = @Artist ORDER BY IdAlbum,Track;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);
            sqlCmd.Parameters.AddWithValue("@Artist", artist);

            return QueryAudioSong(sqlCmd);
        }

        public Collection<Yatse2AudioSong> GetAudioSongFromArtistRandom(long idRemote, string artist)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `AudioSongs` WHERE IdRemote = @idRemote AND Artist = @Artist ORDER BY RANDOM();";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);
            sqlCmd.Parameters.AddWithValue("@Artist", artist);

            return QueryAudioSong(sqlCmd);
        }

        public Collection<Yatse2AudioSong> GetAudioSongFromFile(long idRemote, string fileName)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                    "SELECT * FROM `AudioSongs` WHERE IdRemote = @idRemote AND (Path || FileName = @Filename OR FileName = @Filename );";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);
            sqlCmd.Parameters.AddWithValue("@Filename", fileName);

            return QueryAudioSong(sqlCmd);
        }

        public long InsertAudioSong(Yatse2AudioSong audioSong)
        {
            if (audioSong == null)
                return 0;
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                @"INSERT INTO `AudioSongs` ( IdRemote ,IdSong,Title,Track,Duration,Year,FileName,IdAlbum,Album,Path,IdArtist,Artist,IdGenre,Genre,Thumb,Fanart,Hash, IsFavorite) 
                  VALUES ( ?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?);";
            sqlCmd.Parameters.AddWithValue("a1", audioSong.IdRemote);
            sqlCmd.Parameters.AddWithValue("a2", audioSong.IdSong);
            sqlCmd.Parameters.AddWithValue("a3", audioSong.Title);
            sqlCmd.Parameters.AddWithValue("a4", audioSong.Track);
            sqlCmd.Parameters.AddWithValue("a5", audioSong.Duration);
            sqlCmd.Parameters.AddWithValue("a6", audioSong.Year);
            sqlCmd.Parameters.AddWithValue("a7", audioSong.FileName);
            sqlCmd.Parameters.AddWithValue("a8", audioSong.IdAlbum);
            sqlCmd.Parameters.AddWithValue("a9", audioSong.Album);
            sqlCmd.Parameters.AddWithValue("a10", audioSong.Path);
            sqlCmd.Parameters.AddWithValue("a11", audioSong.IdArtist);
            sqlCmd.Parameters.AddWithValue("a12", audioSong.Artist);
            sqlCmd.Parameters.AddWithValue("a13", audioSong.IdGenre);
            sqlCmd.Parameters.AddWithValue("a14", audioSong.Genre);
            sqlCmd.Parameters.AddWithValue("a15", audioSong.Thumb);
            sqlCmd.Parameters.AddWithValue("a16", audioSong.Fanart);
            sqlCmd.Parameters.AddWithValue("a17", audioSong.Hash);
            sqlCmd.Parameters.AddWithValue("a18", audioSong.IsFavorite);

            return QueryInsert(sqlCmd);
        }

        public bool UpdateAudioSong(Yatse2AudioSong audioSong)
        {
            if (audioSong == null)
                return false;
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                @"UPDATE `AudioSongs` SET IdRemote = ?,IdSong = ?,Title = ?,Track = ?,Duration = ?,Year = ?,FileName = ?,IdAlbum = ?,Album = ?,Path = ?,IdArtist = ?,Artist = ?,IdGenre = ?,Genre = ?,Thumb = ?,Fanart = ?,Hash = ?,IsFavorite = ? WHERE Id = @Id;";
            sqlCmd.Parameters.AddWithValue("a1", audioSong.IdRemote);
            sqlCmd.Parameters.AddWithValue("a2", audioSong.IdSong);
            sqlCmd.Parameters.AddWithValue("a3", audioSong.Title);
            sqlCmd.Parameters.AddWithValue("a4", audioSong.Track);
            sqlCmd.Parameters.AddWithValue("a5", audioSong.Duration);
            sqlCmd.Parameters.AddWithValue("a6", audioSong.Year);
            sqlCmd.Parameters.AddWithValue("a7", audioSong.FileName);
            sqlCmd.Parameters.AddWithValue("a8", audioSong.IdAlbum);
            sqlCmd.Parameters.AddWithValue("a9", audioSong.Album);
            sqlCmd.Parameters.AddWithValue("a10", audioSong.Path);
            sqlCmd.Parameters.AddWithValue("a11", audioSong.IdArtist);
            sqlCmd.Parameters.AddWithValue("a12", audioSong.Artist);
            sqlCmd.Parameters.AddWithValue("a13", audioSong.IdGenre);
            sqlCmd.Parameters.AddWithValue("a14", audioSong.Genre);
            sqlCmd.Parameters.AddWithValue("a15", audioSong.Thumb);
            sqlCmd.Parameters.AddWithValue("a16", audioSong.Fanart);
            sqlCmd.Parameters.AddWithValue("a17", audioSong.Hash);
            sqlCmd.Parameters.AddWithValue("a18", audioSong.IsFavorite);
            sqlCmd.Parameters.AddWithValue("@Id", audioSong.Id);

            return Query(sqlCmd);
        }

        public bool DeleteAudioSong(long id)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText = "DELETE FROM `AudioSongs` WHERE Id = @id;";
            sqlCmd.Parameters.AddWithValue("@id", id);

            return Query(sqlCmd);
        }

        public bool DeleteRemoteAudioSong(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText = "DELETE FROM `AudioSongs` WHERE IdRemote = @idRemote;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);

            return Query(sqlCmd);
        }

        private Collection<Yatse2AudioAlbum> QueryAudioAlbum(SQLiteCommand query)
        {
            var audioalbums = new Collection<Yatse2AudioAlbum>();
            if (!_connected) return audioalbums;

            try
            {
                LogQuery(query);
                var sqldReader = query.ExecuteReader();
                while (sqldReader.Read())
                {
                    var audioAlbum = new Yatse2AudioAlbum
                    {
                        Id = GetLong(sqldReader, "Id"),
                        IdRemote = GetLong(sqldReader, "IdRemote"),
                        Title = GetString(sqldReader, "Title"),
                        IdAlbum = GetLong(sqldReader, "IdAlbum"),
                        IdArtist = GetLong(sqldReader, "IdArtist"),
                        Artist = GetString(sqldReader, "Artist"),
                        Genre = GetString(sqldReader, "Genre"),
                        IdGenre = GetLong(sqldReader, "IdGenre"),
                        Year = GetLong(sqldReader, "Year"),
                        Thumb = GetString(sqldReader, "Thumb"),
                        Fanart = GetString(sqldReader, "Fanart"),
                        Hash = GetString(sqldReader, "Hash"),
                        IsFavorite = GetLong(sqldReader, "IsFavorite")
                    };
                    audioalbums.Add(audioAlbum);
                }
                sqldReader.Dispose();
                query.Dispose();
            }
            catch (SQLiteException e)
            {
                Log(e.Message);
            }
            return audioalbums;
        }

        public Collection<Yatse2AudioAlbum> GetAudioAlbum(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `AudioAlbums` WHERE IdRemote = @idRemote ORDER BY Title;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);

            return QueryAudioAlbum(sqlCmd);
        }

        public Collection<Yatse2AudioAlbum> GetAudioAlbumFavorites(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `AudioAlbums` WHERE IdRemote = @idRemote AND IsFavorite > 0 ORDER BY Title;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);

            return QueryAudioAlbum(sqlCmd);
        }

        public Collection<Yatse2AudioAlbum> GetAudioAlbumFromGenre(long idRemote, string genre)
        {

            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `AudioAlbums` WHERE IdRemote = @idRemote AND Genre = @Genre ORDER BY Title;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);
            sqlCmd.Parameters.AddWithValue("@Genre", genre);

            return QueryAudioAlbum(sqlCmd);
        }

        public Collection<Yatse2AudioAlbum> GetAudioAlbumFromArtist(long idRemote, string artist)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `AudioAlbums` WHERE IdRemote = @idRemote AND Title IN (SELECT Album FROM `AudioSongs` WHERE Artist = @Artist AND IdRemote = @idRemote)";

            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);
            sqlCmd.Parameters.AddWithValue("@Artist", artist);

            return QueryAudioAlbum(sqlCmd);
        }

        public Collection<Yatse2AudioAlbum> GetAudioAlbumFromArtistNoCompilation(long idRemote, string artist)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `AudioAlbums` WHERE IdRemote = @idRemote AND Artist = @Artist";

            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);
            sqlCmd.Parameters.AddWithValue("@Artist", artist);

            return QueryAudioAlbum(sqlCmd);
        }

        public long InsertAudioAlbum(Yatse2AudioAlbum audioAlbum)
        {
            if (audioAlbum == null)
                return 0;
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                @"INSERT INTO `AudioAlbums` ( IdRemote ,Title,IdAlbum,IdArtist,Artist,Genre,IdGenre,Year,Thumb,Fanart,Hash,IsFavorite ) 
                  VALUES ( ?,?,?,?,?,?,?,?,?,?,?,?);";
            sqlCmd.Parameters.AddWithValue("a1", audioAlbum.IdRemote);
            sqlCmd.Parameters.AddWithValue("a2", audioAlbum.Title);
            sqlCmd.Parameters.AddWithValue("a3", audioAlbum.IdAlbum);
            sqlCmd.Parameters.AddWithValue("a4", audioAlbum.IdArtist);
            sqlCmd.Parameters.AddWithValue("a5", audioAlbum.Artist);
            sqlCmd.Parameters.AddWithValue("a6", audioAlbum.Genre);
            sqlCmd.Parameters.AddWithValue("a7", audioAlbum.IdGenre);
            sqlCmd.Parameters.AddWithValue("a8", audioAlbum.Year);
            sqlCmd.Parameters.AddWithValue("a9", audioAlbum.Thumb);
            sqlCmd.Parameters.AddWithValue("a10", audioAlbum.Fanart);
            sqlCmd.Parameters.AddWithValue("a11", audioAlbum.Hash);
            sqlCmd.Parameters.AddWithValue("a12", audioAlbum.IsFavorite);

            return QueryInsert(sqlCmd);
        }

        public bool UpdateAudioAlbum(Yatse2AudioAlbum audioAlbum)
        {
            if (audioAlbum == null)
                return false;
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                @"UPDATE `AudioAlbums` SET IdRemote = ? ,Title = ? ,IdAlbum = ? ,IdArtist = ? ,Artist = ? ,Genre = ? ,IdGenre = ? ,Year = ? ,Thumb = ? ,Fanart = ? ,Hash = ? ,IsFavorite = ? WHERE Id = @Id;";
            sqlCmd.Parameters.AddWithValue("a1", audioAlbum.IdRemote);
            sqlCmd.Parameters.AddWithValue("a2", audioAlbum.Title);
            sqlCmd.Parameters.AddWithValue("a3", audioAlbum.IdAlbum);
            sqlCmd.Parameters.AddWithValue("a4", audioAlbum.IdArtist);
            sqlCmd.Parameters.AddWithValue("a5", audioAlbum.Artist);
            sqlCmd.Parameters.AddWithValue("a6", audioAlbum.Genre);
            sqlCmd.Parameters.AddWithValue("a7", audioAlbum.IdGenre);
            sqlCmd.Parameters.AddWithValue("a8", audioAlbum.Year);
            sqlCmd.Parameters.AddWithValue("a9", audioAlbum.Thumb);
            sqlCmd.Parameters.AddWithValue("a10", audioAlbum.Fanart);
            sqlCmd.Parameters.AddWithValue("a11", audioAlbum.Hash);
            sqlCmd.Parameters.AddWithValue("a12", audioAlbum.IsFavorite);
            sqlCmd.Parameters.AddWithValue("@Id", audioAlbum.Id);

            return Query(sqlCmd);
        }

        public bool DeleteAudioAlbum(long id)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText = "DELETE FROM `AudioAlbums` WHERE Id = @id;";
            sqlCmd.Parameters.AddWithValue("@id", id);

            return Query(sqlCmd);
        }

        public bool DeleteRemoteAudioAlbum(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText = "DELETE FROM `AudioAlbums` WHERE IdRemote = @idRemote;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);

            return Query(sqlCmd);
        }

        private Collection<Yatse2TvEpisode> QueryTvEpisode(SQLiteCommand query)
        {
            var tvEpisodes = new Collection<Yatse2TvEpisode>();
            if (!_connected) return tvEpisodes;
            try
            {
                LogQuery(query);
                var sqldReader = query.ExecuteReader();
                while (sqldReader.Read())
                {
                    var episode = new Yatse2TvEpisode
                    {
                        Id = GetLong(sqldReader, "Id"),
                        IdRemote = GetLong(sqldReader, "IdRemote"),
                        IdEpisode = GetLong(sqldReader, "IdEpisode"),
                        IdFile = GetLong(sqldReader, "IdFile"),
                        IdShow = GetLong(sqldReader, "IdShow"),
                        Title = GetString(sqldReader, "Title"),
                        ShowTitle = GetString(sqldReader, "ShowTitle"),
                        Rating = GetString(sqldReader, "Rating"),
                        Plot = GetString(sqldReader, "Plot"),
                        Director = GetString(sqldReader, "Director"),
                        Date = GetString(sqldReader, "Date"),
                        Mpaa = GetString(sqldReader, "Mpaa"),
                        Studio = GetString(sqldReader, "Studio"),
                        Season = GetLong(sqldReader, "Season"),
                        Episode = GetLong(sqldReader, "Episode"),
                        FileName = GetString(sqldReader, "FileName"),
                        Path = GetString(sqldReader, "Path"),
                        PlayCount = GetLong(sqldReader, "PlayCount"),
                        Hash = GetString(sqldReader, "Hash"),
                        Thumb = GetString(sqldReader, "Thumb"),
                        Fanart = GetString(sqldReader, "Fanart"),
                        IsStack = GetLong(sqldReader, "IsStack"),
                        IsFavorite = GetLong(sqldReader, "IsFavorite")
                    };
                    tvEpisodes.Add(episode);
                }
                sqldReader.Dispose();
                query.Dispose();
            }
            catch (SQLiteException e)
            {
                Log(e.Message);
            }
            return tvEpisodes;
        }

        public Collection<Yatse2TvEpisode> GetTvEpisode(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `TvEpisodes` WHERE IdRemote = @idRemote ORDER BY Season, Episode;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);

            return QueryTvEpisode(sqlCmd);
        }

        public Collection<Yatse2TvEpisode> GetTvEpisodeFromFile(long idRemote, string fileName)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                    "SELECT * FROM `TvEpisodes` WHERE IdRemote = @idRemote AND (Path || FileName = @Filename OR FileName = @Filename ) ORDER BY Title;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);
            sqlCmd.Parameters.AddWithValue("@Filename", fileName);

            return QueryTvEpisode(sqlCmd);
        }

        public Collection<Yatse2TvEpisode> GetTvEpisodeFavorites(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `TvEpisodes` WHERE IdRemote = @idRemote AND IsFavorite > 0 ORDER BY Season, Episode;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);
 
            return QueryTvEpisode(sqlCmd);
        }

        public Collection<Yatse2TvEpisode> GetTvEpisodeFromTvShow(long idRemote, string show)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `TvEpisodes` WHERE IdRemote = @idRemote AND ShowTitle = @Show ORDER BY Season, Episode;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);
            sqlCmd.Parameters.AddWithValue("@Show", show);

            return QueryTvEpisode(sqlCmd);
        }

        public Collection<Yatse2TvEpisode> GetTvEpisodeFromTvShowSeason(long idRemote, string show, long seasonNumber)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `TvEpisodes` WHERE IdRemote = @idRemote AND ShowTitle = @Show AND Season = @seasonNumber ORDER BY Season, Episode;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);
            sqlCmd.Parameters.AddWithValue("Show", show);
            sqlCmd.Parameters.AddWithValue("@seasonNumber", seasonNumber);

            return QueryTvEpisode(sqlCmd);
        }

        public long InsertTvEpisode(Yatse2TvEpisode episode)
        {
            if (episode == null)
                return 0;
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                @"INSERT INTO `TvEpisodes` ( IdRemote ,IdEpisode,IdFile,IdShow,Title,ShowTitle,Rating,Plot,Director,Date,Mpaa,Studio,Season,Episode,FileName,Path,PlayCount,Hash,Thumb,Fanart,IsStack,IsFavorite ) 
                  VALUES ( ?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?);";
            sqlCmd.Parameters.AddWithValue("a1", episode.IdRemote);
            sqlCmd.Parameters.AddWithValue("a2", episode.IdEpisode);
            sqlCmd.Parameters.AddWithValue("a3", episode.IdFile);
            sqlCmd.Parameters.AddWithValue("a4", episode.IdShow);
            sqlCmd.Parameters.AddWithValue("a5", episode.Title);
            sqlCmd.Parameters.AddWithValue("a6", episode.ShowTitle);
            sqlCmd.Parameters.AddWithValue("a7", episode.Rating);
            sqlCmd.Parameters.AddWithValue("a8", episode.Plot);
            sqlCmd.Parameters.AddWithValue("a9", episode.Director);
            sqlCmd.Parameters.AddWithValue("a10", episode.Date);
            sqlCmd.Parameters.AddWithValue("a11", episode.Mpaa);
            sqlCmd.Parameters.AddWithValue("a12", episode.Studio);
            sqlCmd.Parameters.AddWithValue("a13", episode.Season);
            sqlCmd.Parameters.AddWithValue("a14", episode.Episode);
            sqlCmd.Parameters.AddWithValue("a15", episode.FileName);
            sqlCmd.Parameters.AddWithValue("a16", episode.Path);
            sqlCmd.Parameters.AddWithValue("a17", episode.PlayCount);
            sqlCmd.Parameters.AddWithValue("a18", episode.Hash);
            sqlCmd.Parameters.AddWithValue("a19", episode.Thumb);
            sqlCmd.Parameters.AddWithValue("a20", episode.Fanart);
            sqlCmd.Parameters.AddWithValue("a21", episode.IsStack);
            sqlCmd.Parameters.AddWithValue("a22", episode.IsFavorite);

            return QueryInsert(sqlCmd);
        }

        public bool UpdateTvEpisode(Yatse2TvEpisode episode)
        {
            if (episode == null)
                return false;
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                @"UPDATE `TvEpisodes` SET IdRemote = ?,IdEpisode = ?,IdFile = ?,IdShow = ?,Title = ?,ShowTitle = ?,Rating = ?,Plot = ?,Director = ?,Date = ?,Mpaa = ?,Studio = ?,Season = ?,Episode = ?,FileName = ?,Path = ?,PlayCount = ?,Hash = ?,Thumb = ?,Fanart = ?,IsStack = ?,IsFavorite = ? WHERE Id = @Id;";
            sqlCmd.Parameters.AddWithValue("a1", episode.IdRemote);
            sqlCmd.Parameters.AddWithValue("a2", episode.IdEpisode);
            sqlCmd.Parameters.AddWithValue("a3", episode.IdFile);
            sqlCmd.Parameters.AddWithValue("a4", episode.IdShow);
            sqlCmd.Parameters.AddWithValue("a5", episode.Title);
            sqlCmd.Parameters.AddWithValue("a6", episode.ShowTitle);
            sqlCmd.Parameters.AddWithValue("a7", episode.Rating);
            sqlCmd.Parameters.AddWithValue("a8", episode.Plot);
            sqlCmd.Parameters.AddWithValue("a9", episode.Director);
            sqlCmd.Parameters.AddWithValue("a10", episode.Date);
            sqlCmd.Parameters.AddWithValue("a11", episode.Mpaa);
            sqlCmd.Parameters.AddWithValue("a12", episode.Studio);
            sqlCmd.Parameters.AddWithValue("a13", episode.Season);
            sqlCmd.Parameters.AddWithValue("a14", episode.Episode);
            sqlCmd.Parameters.AddWithValue("a15", episode.FileName);
            sqlCmd.Parameters.AddWithValue("a16", episode.Path);
            sqlCmd.Parameters.AddWithValue("a17", episode.PlayCount);
            sqlCmd.Parameters.AddWithValue("a18", episode.Hash);
            sqlCmd.Parameters.AddWithValue("a19", episode.Thumb);
            sqlCmd.Parameters.AddWithValue("a20", episode.Fanart);
            sqlCmd.Parameters.AddWithValue("a21", episode.IsStack);
            sqlCmd.Parameters.AddWithValue("a22", episode.IsFavorite);
            sqlCmd.Parameters.AddWithValue("@Id", episode.Id);

            return Query(sqlCmd);
        }

        public bool DeleteTvEpisodes(long id)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText = "DELETE FROM `TvEpisodes` WHERE Id = @id;";
            sqlCmd.Parameters.AddWithValue("@id", id);

            return Query(sqlCmd);
        }

        public bool DeleteRemoteTvEpisodes(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText = "DELETE FROM `TvEpisodes` WHERE IdRemote = @idRemote;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);

            return Query(sqlCmd);
        }

        private Collection<Yatse2TvSeason> QueryTvSeason(SQLiteCommand query)
        {
            var tvSeasons = new Collection<Yatse2TvSeason>();
            if (!_connected) return tvSeasons;
            try
            {
                LogQuery(query);
                var sqldReader = query.ExecuteReader();
                while (sqldReader.Read())
                {
                    var episode = new Yatse2TvSeason
                    {
                        Id = GetLong(sqldReader, "Id"),
                        IdRemote = GetLong(sqldReader, "IdRemote"),
                        IdShow = GetLong(sqldReader, "IdShow"),
                        SeasonNumber = GetLong(sqldReader, "SeasonNumber"),
                        EpisodeCount = GetLong(sqldReader, "EpisodeCount"),
                        Hash = GetString(sqldReader, "Hash"),
                        Thumb = GetString(sqldReader, "Thumb"),
                        Fanart = GetString(sqldReader, "Fanart"),
                        IsFavorite = GetLong(sqldReader, "IsFavorite"),
                        Show = GetString(sqldReader, "Show")
                    };
                    tvSeasons.Add(episode);
                }
                sqldReader.Dispose();
                query.Dispose();
            }
            catch (SQLiteException e)
            {
                Log(e.Message);
            }
            return tvSeasons;
        }

        public Collection<Yatse2TvSeason> GetTvSeason(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `TvSeasons` WHERE IdRemote = @idRemote ORDER BY SeasonNumber;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);
            return QueryTvSeason(sqlCmd);
        }

        public Collection<Yatse2TvSeason> GetTvSeasonFavorites(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `TvSeasons` WHERE IdRemote = @idRemote AND IsFavorite > 0 ORDER BY SeasonNumber;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);
            return QueryTvSeason(sqlCmd);
        }

        public Collection<Yatse2TvSeason> GetTvSeasonFromTvShow(long idRemote, string show)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `TvSeasons` WHERE IdRemote = @idRemote AND Show = @Show ORDER BY SeasonNumber;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);
            sqlCmd.Parameters.AddWithValue("@Show", show);

            return QueryTvSeason(sqlCmd);
        }

        public long InsertTvSeason(Yatse2TvSeason season)
        {
            if (season == null)
                return 0;
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                @"INSERT INTO `TvSeasons` ( IdRemote ,IdShow,SeasonNumber,EpisodeCount,Hash,Thumb,Fanart,IsFavorite,Show ) 
                  VALUES ( ?,?,?,?,?,?,?,?,?);";
            sqlCmd.Parameters.AddWithValue("a1", season.IdRemote);
            sqlCmd.Parameters.AddWithValue("a2", season.IdShow);
            sqlCmd.Parameters.AddWithValue("a3", season.SeasonNumber);
            sqlCmd.Parameters.AddWithValue("a4", season.EpisodeCount);
            sqlCmd.Parameters.AddWithValue("a5", season.Hash);
            sqlCmd.Parameters.AddWithValue("a6", season.Thumb);
            sqlCmd.Parameters.AddWithValue("a7", season.Fanart);
            sqlCmd.Parameters.AddWithValue("a8", season.IsFavorite);
            sqlCmd.Parameters.AddWithValue("a9", season.Show);
            return QueryInsert(sqlCmd);
        }

        public bool UpdateTvSeason(Yatse2TvSeason season)
        {
            if (season == null)
                return false;
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                @"UPDATE `TvShows` SET IdRemote = ?,IdShow = ?,SeasonNumber = ?,EpisodeCount = ?,Hash = ?,Thumb = ?,Fanart = ?,IsFavorite = ?, Show = ? WHERE Id = @Id;";
            sqlCmd.Parameters.AddWithValue("a1", season.IdRemote);
            sqlCmd.Parameters.AddWithValue("a2", season.IdShow);
            sqlCmd.Parameters.AddWithValue("a3", season.SeasonNumber);
            sqlCmd.Parameters.AddWithValue("a4", season.EpisodeCount);
            sqlCmd.Parameters.AddWithValue("a5", season.Hash);
            sqlCmd.Parameters.AddWithValue("a6", season.Thumb);
            sqlCmd.Parameters.AddWithValue("a7", season.Fanart);
            sqlCmd.Parameters.AddWithValue("a8", season.IsFavorite);
            sqlCmd.Parameters.AddWithValue("a9", season.Show);
            sqlCmd.Parameters.AddWithValue("@Id", season.Id);

            return Query(sqlCmd);
        }

        public bool DeleteTvSeason(long id)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText = "DELETE FROM `TvSeasons` WHERE Id = @id;";
            sqlCmd.Parameters.AddWithValue("@id", id);

            return Query(sqlCmd);
        }

        public bool DeleteRemoteTvSeasons(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText = "DELETE FROM `TvSeasons` WHERE IdRemote = @idRemote;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);

            return Query(sqlCmd);
        }

        private Collection<Yatse2TvShow> QueryTvShow(SQLiteCommand query)
        {
            var tvShows = new Collection<Yatse2TvShow>();
            if (!_connected) return tvShows;
            try
            {
                LogQuery(query);
                var sqldReader = query.ExecuteReader();
                while (sqldReader.Read())
                {
                    var movie = new Yatse2TvShow
                    {
                        Id = GetLong(sqldReader, "Id"),
                        IdRemote = GetLong(sqldReader, "IdRemote"),
                        IdShow = GetLong(sqldReader, "IdShow"),
                        IdScraper = GetString(sqldReader, "IdScraper"),
                        Title = GetString(sqldReader, "Title"),
                        Plot = GetString(sqldReader, "Plot"),
                        Premiered = GetString(sqldReader, "Premiered"),
                        Rating = GetString(sqldReader, "Rating"),
                        Genre = GetString(sqldReader, "Genre"),
                        Mpaa = GetString(sqldReader, "Mpaa"),
                        Studio = GetString(sqldReader, "Studio"),
                        Path = GetString(sqldReader, "Path"),
                        TotalCount = GetLong(sqldReader, "TotalCount"),
                        Hash = GetString(sqldReader, "Hash"),
                        Thumb = GetString(sqldReader, "Thumb"),
                        Fanart = GetString(sqldReader, "Fanart"),
                        IsFavorite = GetLong(sqldReader, "IsFavorite")
                    };
                    tvShows.Add(movie);
                }
                sqldReader.Dispose();
                query.Dispose();
            }
            catch (SQLiteException e)
            {
                Log(e.Message);
            }
            return tvShows;
        }

        public Collection<Yatse2TvShow> GetTvShow(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `TvShows` WHERE IdRemote = @idRemote ORDER BY Title COLLATE IGNORESORTTOKENS ;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);

            return QueryTvShow(sqlCmd);
        }

        public Collection<Yatse2TvShow> GetTvShowFromName(long idRemote, string show)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                    "SELECT * FROM `TvShows` WHERE IdRemote = @idRemote AND Title = @Show ORDER BY Title COLLATE IGNORESORTTOKENS;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);
            sqlCmd.Parameters.AddWithValue("@Show", show);

            return QueryTvShow(sqlCmd);
        }

        public Collection<Yatse2TvShow> GetTvShowFavorites(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `TvShows` WHERE IdRemote = @idRemote AND IsFavorite > 0 ORDER BY Title COLLATE IGNORESORTTOKENS;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);

            return QueryTvShow(sqlCmd);
        }

        public long InsertTvShow(Yatse2TvShow show)
        {
            if (show == null)
                return 0;
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                @"INSERT INTO `TvShows` ( IdRemote ,IdShow,IdScraper,Title,Plot,Premiered,Rating,Genre,Mpaa,Studio,Path,TotalCount,Hash,Thumb,Fanart,IsFavorite ) 
                  VALUES ( ?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?);";
            sqlCmd.Parameters.AddWithValue("a1", show.IdRemote);
            sqlCmd.Parameters.AddWithValue("a2", show.IdShow);
            sqlCmd.Parameters.AddWithValue("a3", show.IdScraper);
            sqlCmd.Parameters.AddWithValue("a4", show.Title);
            sqlCmd.Parameters.AddWithValue("a5", show.Plot);
            sqlCmd.Parameters.AddWithValue("a6", show.Premiered);
            sqlCmd.Parameters.AddWithValue("a7", show.Rating);
            sqlCmd.Parameters.AddWithValue("a8", show.Genre);
            sqlCmd.Parameters.AddWithValue("a9", show.Mpaa);
            sqlCmd.Parameters.AddWithValue("a10", show.Studio);
            sqlCmd.Parameters.AddWithValue("a11", show.Path);
            sqlCmd.Parameters.AddWithValue("a12", show.TotalCount);
            sqlCmd.Parameters.AddWithValue("a13", show.Hash);
            sqlCmd.Parameters.AddWithValue("a14", show.Thumb);
            sqlCmd.Parameters.AddWithValue("a15", show.Fanart);
            sqlCmd.Parameters.AddWithValue("a16", show.IsFavorite);

            return QueryInsert(sqlCmd);
        }

        public bool UpdateTvShow(Yatse2TvShow show)
        {
            if (show == null)
                return false;
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                @"UPDATE `TvShows` SET IdRemote = ?,IdShow = ?,IdScraper = ?,Title = ?,Plot = ?,Premiered = ?,Rating = ?,Genre = ?,Mpaa = ?,Studio = ?,Path = ?,TotalCount = ?,Hash = ?,Thumb = ?,Fanart = ?,IsFavorite = ? WHERE Id = @Id;";
            sqlCmd.Parameters.AddWithValue("a1", show.IdRemote);
            sqlCmd.Parameters.AddWithValue("a2", show.IdShow);
            sqlCmd.Parameters.AddWithValue("a3", show.IdScraper);
            sqlCmd.Parameters.AddWithValue("a4", show.Title);
            sqlCmd.Parameters.AddWithValue("a5", show.Plot);
            sqlCmd.Parameters.AddWithValue("a6", show.Premiered);
            sqlCmd.Parameters.AddWithValue("a7", show.Rating);
            sqlCmd.Parameters.AddWithValue("a8", show.Genre);
            sqlCmd.Parameters.AddWithValue("a9", show.Mpaa);
            sqlCmd.Parameters.AddWithValue("a10", show.Studio);
            sqlCmd.Parameters.AddWithValue("a11", show.Path);
            sqlCmd.Parameters.AddWithValue("a12", show.TotalCount);
            sqlCmd.Parameters.AddWithValue("a13", show.Hash);
            sqlCmd.Parameters.AddWithValue("a14", show.Thumb);
            sqlCmd.Parameters.AddWithValue("a15", show.Fanart);
            sqlCmd.Parameters.AddWithValue("a16", show.IsFavorite);
            sqlCmd.Parameters.AddWithValue("@Id", show.Id);

            return Query(sqlCmd);
        }

        public bool DeleteTvShow(long id)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText = "DELETE FROM `TvShows` WHERE Id = @id;";
            sqlCmd.Parameters.AddWithValue("@id", id);

            return Query(sqlCmd);
        }

        public bool DeleteRemoteTvShows(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText = "DELETE FROM `TvShows` WHERE IdRemote = @idRemote;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);
                
            return Query(sqlCmd);
        }

        private Collection<Yatse2Movie> QueryMovie(SQLiteCommand query)
        {
            var movies = new Collection<Yatse2Movie>();
            if (!_connected) return movies;
            try
            {
                LogQuery(query);
                var sqldReader = query.ExecuteReader();
                while (sqldReader.Read())
                {
                    var movie = new Yatse2Movie
                    {
                        Id = GetLong(sqldReader, "Id"),
                        IdRemote = GetLong(sqldReader, "IdRemote"),
                        IdMovie = GetLong(sqldReader, "IdMovie"),
                        IdFile = GetLong(sqldReader, "IdFile"),
                        IdScraper = GetString(sqldReader, "IdScraper"),
                        Title = GetString(sqldReader, "Title"),
                        OriginalTitle = GetString(sqldReader, "OriginalTitle"),
                        Genre = GetString(sqldReader, "Genre"),
                        Tagline = GetString(sqldReader, "Tagline"),
                        Plot = GetString(sqldReader, "Plot"),
                        Director = GetString(sqldReader, "Director"),
                        Year = GetLong(sqldReader, "Year"),
                        Length = GetString(sqldReader, "Length"),
                        Mpaa = GetString(sqldReader, "Mpaa"),
                        Studio = GetString(sqldReader, "Studio"),
                        Rating = GetString(sqldReader, "Rating"),
                        Votes = GetString(sqldReader, "Votes"),
                        FileName = GetString(sqldReader, "FileName"),
                        Path = GetString(sqldReader, "Path"),
                        PlayCount = GetLong(sqldReader, "PlayCount"),
                        Hash = GetString(sqldReader, "Hash"),
                        Thumb = GetString(sqldReader, "Thumb"),
                        Fanart = GetString(sqldReader, "Fanart"),
                        IsStack = GetLong(sqldReader, "IsStack"),
                        IsFavorite = GetLong(sqldReader, "IsFavorite")
                    };
                    movies.Add(movie);
                }
                sqldReader.Dispose();
                query.Dispose();
            }
            catch (SQLiteException e)
            {
                Log(e.Message);
            }
            return movies;
        }

        public Collection<Yatse2Movie> GetMovie(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `Movies` WHERE IdRemote = @idRemote ORDER BY Title COLLATE IGNORESORTTOKENS;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);

            return QueryMovie(sqlCmd);
        }

        public Collection<Yatse2Movie> GetMovieFromFile(long idRemote, string fileName)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `Movies` WHERE IdRemote = @idRemote AND (Path || FileName = @Filename OR FileName = @Filename );";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);
            sqlCmd.Parameters.AddWithValue("@Filename", fileName);

            return QueryMovie(sqlCmd);
        }

        public Collection<Yatse2Movie> GetMovieFavorites(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                "SELECT * FROM `Movies` WHERE IdRemote = @idRemote AND IsFavorite > 0 ORDER BY Title COLLATE IGNORESORTTOKENS;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);

            return QueryMovie(sqlCmd);
        }

        public long InsertMovie(Yatse2Movie movie)
        {
            if (movie == null)
                return 0;
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                @"INSERT INTO `Movies` ( IdRemote ,IdFile,IdScraper,Title,OriginalTitle,Genre,Tagline,Plot,Director,Year,Length,Mpaa,Studio,Rating,Votes,FileName,Path,PlayCount,Hash,Thumb,Fanart,IsStack,IdMovie,IsFavorite ) 
                  VALUES ( ?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?);";
            sqlCmd.Parameters.AddWithValue("a1", movie.IdRemote);
            sqlCmd.Parameters.AddWithValue("a2", movie.IdFile);
            sqlCmd.Parameters.AddWithValue("a3", movie.IdScraper);
            sqlCmd.Parameters.AddWithValue("a4", movie.Title);
            sqlCmd.Parameters.AddWithValue("a5", movie.OriginalTitle);
            sqlCmd.Parameters.AddWithValue("a6", movie.Genre);
            sqlCmd.Parameters.AddWithValue("a7", movie.Tagline);
            sqlCmd.Parameters.AddWithValue("a8", movie.Plot);
            sqlCmd.Parameters.AddWithValue("a9", movie.Director);
            sqlCmd.Parameters.AddWithValue("a10", movie.Year);
            sqlCmd.Parameters.AddWithValue("a11", movie.Length);
            sqlCmd.Parameters.AddWithValue("a12", movie.Mpaa);
            sqlCmd.Parameters.AddWithValue("a13", movie.Studio);
            sqlCmd.Parameters.AddWithValue("a14", movie.Rating);
            sqlCmd.Parameters.AddWithValue("a15", movie.Votes);
            sqlCmd.Parameters.AddWithValue("a16", movie.FileName);
            sqlCmd.Parameters.AddWithValue("a17", movie.Path);
            sqlCmd.Parameters.AddWithValue("a18", movie.PlayCount);
            sqlCmd.Parameters.AddWithValue("a19", movie.Hash);
            sqlCmd.Parameters.AddWithValue("a20", movie.Thumb);
            sqlCmd.Parameters.AddWithValue("a21", movie.Fanart);
            sqlCmd.Parameters.AddWithValue("a22", movie.IsStack);
            sqlCmd.Parameters.AddWithValue("a23", movie.IdMovie);
            sqlCmd.Parameters.AddWithValue("a24", movie.IsFavorite);

            return QueryInsert(sqlCmd);
        }

        public bool UpdateMovie(Yatse2Movie movie)
        {
            if (movie == null)
                return false;
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                @"UPDATE `Movies` SET IdRemote = ?,IdFile = ?,IdScraper = ?,Title = ?,OriginalTitle = ?,Genre = ?,Tagline = ?,Plot = ?,Director = ?,Year = ?,Length = ?,Mpaa = ?,Studio = ?,Rating = ?,Votes = ?,FileName = ?,Path = ?,PlayCount = ?,Hash = ?,Thumb = ?,Fanart = ?,IsStack = ?,IdMovie = ?,IsFavorite = ? WHERE Id = @Id;";
            sqlCmd.Parameters.AddWithValue("a1", movie.IdRemote);
            sqlCmd.Parameters.AddWithValue("a2", movie.IdFile);
            sqlCmd.Parameters.AddWithValue("a3", movie.IdScraper);
            sqlCmd.Parameters.AddWithValue("a4", movie.Title);
            sqlCmd.Parameters.AddWithValue("a5", movie.OriginalTitle);
            sqlCmd.Parameters.AddWithValue("a6", movie.Genre);
            sqlCmd.Parameters.AddWithValue("a7", movie.Tagline);
            sqlCmd.Parameters.AddWithValue("a8", movie.Plot);
            sqlCmd.Parameters.AddWithValue("a9", movie.Director);
            sqlCmd.Parameters.AddWithValue("a10", movie.Year);
            sqlCmd.Parameters.AddWithValue("a11", movie.Length);
            sqlCmd.Parameters.AddWithValue("a12", movie.Mpaa);
            sqlCmd.Parameters.AddWithValue("a13", movie.Studio);
            sqlCmd.Parameters.AddWithValue("a14", movie.Rating);
            sqlCmd.Parameters.AddWithValue("a15", movie.Votes);
            sqlCmd.Parameters.AddWithValue("a16", movie.FileName);
            sqlCmd.Parameters.AddWithValue("a17", movie.Path);
            sqlCmd.Parameters.AddWithValue("a18", movie.PlayCount);
            sqlCmd.Parameters.AddWithValue("a19", movie.Hash);
            sqlCmd.Parameters.AddWithValue("a20", movie.Thumb);
            sqlCmd.Parameters.AddWithValue("a21", movie.Fanart);
            sqlCmd.Parameters.AddWithValue("a22", movie.IsStack);
            sqlCmd.Parameters.AddWithValue("a23", movie.IdMovie);
            sqlCmd.Parameters.AddWithValue("a24", movie.IsFavorite);
            sqlCmd.Parameters.AddWithValue("@Id", movie.Id);

            return Query(sqlCmd);
        }

        public bool DeleteMovie(long id)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText = "DELETE FROM `Movies` WHERE Id = @id;";
            sqlCmd.Parameters.AddWithValue("@id", id);

            return Query(sqlCmd);
        }

        public bool DeleteRemoteMovie(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText = "DELETE FROM `Movies` WHERE IdRemote = @idRemote;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);

            return Query(sqlCmd);
        }

        private Collection<Yatse2Remote> QueryRemote(SQLiteCommand query)
        {
            var remotes = new Collection<Yatse2Remote>();
            if (!_connected) return remotes;
            try
            {
                LogQuery(query);
                var sqldReader = query.ExecuteReader();
                while (sqldReader.Read())
                {
                    var remote = new Yatse2Remote
                    {
                        Id = GetLong(sqldReader, "Id"),
                        Name = GetString(sqldReader, "Name"),
                        Api = GetString(sqldReader, "Api"),
                        ProcessName = GetString(sqldReader, "ProcessName"),
                        IP = GetString(sqldReader, "IP"),
                        Port = GetString(sqldReader, "Port"),
                        MacAddress = GetString(sqldReader, "MacAddress"),
                        Login = GetString(sqldReader, "Login"),
                        Password = GetString(sqldReader, "Password"),
                        Version = GetString(sqldReader, "Version"),
                        OS = GetString(sqldReader, "Os"),
                        Additional = GetString(sqldReader, "Additional"),
                        CacheFilled = GetLong(sqldReader, "CacheFilled")
                    };
                    remotes.Add(remote);
                }
                sqldReader.Dispose();
                query.Dispose();
            }
            catch (SQLiteException e)
            {
                Log(e.Message);
            }
            return remotes;
        }

        public Collection<Yatse2Remote> GetRemote(long idRemote)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText = "SELECT * FROM `Remotes` WHERE Id = @idRemote;";
            sqlCmd.Parameters.AddWithValue("@idRemote", idRemote);

            return QueryRemote(sqlCmd);
        }

        public Collection<Yatse2Remote> SelectAllRemote()
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText = "SELECT * FROM `Remotes`;";

            return QueryRemote(sqlCmd);
        }

        public long InsertRemote(Yatse2Remote remote)
        {
            if (remote == null)
                return 0;
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                @"INSERT INTO `Remotes` ( Name, Api, ProcessName, IP, Port, MacAddress, Login, Password, Version, Os, Additional, CacheFilled)  
                  VALUES ( ?,?,?,?,?,?,?,?,?,?,?,?);";
            sqlCmd.Parameters.AddWithValue("a1", remote.Name);
            sqlCmd.Parameters.AddWithValue("a2", remote.Api);
            sqlCmd.Parameters.AddWithValue("a3", remote.ProcessName);
            sqlCmd.Parameters.AddWithValue("a4", remote.IP);
            sqlCmd.Parameters.AddWithValue("a5", remote.Port);
            sqlCmd.Parameters.AddWithValue("a6", remote.MacAddress);
            sqlCmd.Parameters.AddWithValue("a7", remote.Login);
            sqlCmd.Parameters.AddWithValue("a8", remote.Password);
            sqlCmd.Parameters.AddWithValue("a9", remote.Version);
            sqlCmd.Parameters.AddWithValue("a10", remote.OS);
            sqlCmd.Parameters.AddWithValue("a11", remote.Additional);
            sqlCmd.Parameters.AddWithValue("a12", remote.CacheFilled);

            return QueryInsert(sqlCmd);
        }

        public bool UpdateRemote(Yatse2Remote remote)
        {
            if (remote == null)
                return false;
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText =
                @"UPDATE `Remotes` SET Name = ? , Api = ?, ProcessName = ?, IP = ?, Port = ?, MacAddress = ?, Login = ?, Password =  ?, Version = ?, Os = ?, Additional = ?, CacheFilled = ? WHERE Id = @Id;";
            sqlCmd.Parameters.AddWithValue("a1", remote.Name);
            sqlCmd.Parameters.AddWithValue("a2", remote.Api);
            sqlCmd.Parameters.AddWithValue("a3", remote.ProcessName);
            sqlCmd.Parameters.AddWithValue("a4", remote.IP);
            sqlCmd.Parameters.AddWithValue("a5", remote.Port);
            sqlCmd.Parameters.AddWithValue("a6", remote.MacAddress);
            sqlCmd.Parameters.AddWithValue("a7", remote.Login);
            sqlCmd.Parameters.AddWithValue("a8", remote.Password);
            sqlCmd.Parameters.AddWithValue("a9", remote.Version);
            sqlCmd.Parameters.AddWithValue("a10", remote.OS);
            sqlCmd.Parameters.AddWithValue("a12", remote.Additional);
            sqlCmd.Parameters.AddWithValue("a13", remote.CacheFilled);
            sqlCmd.Parameters.AddWithValue("@Id", remote.Id);

            return Query(sqlCmd);
        }

        public bool DeleteRemote(long id)
        {
            var sqlCmd = _dbConnection.CreateCommand();
            sqlCmd.CommandText = "DELETE FROM `Remotes` WHERE Id = @id;";
            sqlCmd.Parameters.AddWithValue("@id", id);

            return Query(sqlCmd);
        }

        public bool DeleteRemoteAndData(long id)
        {
            DeleteRemoteMovie(id);
            DeleteRemoteAudioArtist(id);
            DeleteRemoteAudioAlbum(id);
            DeleteRemoteAudioGenre(id);
            DeleteRemoteAudioSong(id);
            DeleteRemoteTvEpisodes(id);
            DeleteRemoteTvSeasons(id);
            DeleteRemoteTvShows(id);
            DeleteRemote(id);
            Compress();
            return true;
        }

        public bool UpdateDatabase()
        {
            if (!_connected) return false;

            var fromBuild = SelectDBVersion();

            var sqlCmd = _dbConnection.CreateCommand();
            try
            {
                if (fromBuild <= 2)
                {
                    Log("Applying database update 3");

                    sqlCmd.CommandText = @"CREATE TABLE `TvSeasons`
                    (
                        `Id` INTEGER NOT NULL PRIMARY KEY,
                        `IdRemote` INTEGER,
                        `IdShow` INTEGER,
                        `SeasonNumber` INTEGER,
                        `EpisodeCount` INTEGER,
                        `Hash` VARCHAR(50),
                        `Thumb` TEXT,
                        `Fanart` TEXT,
                        `IsFavorite` Integer
                    )";
                    LogQuery(sqlCmd);
                    sqlCmd.ExecuteNonQuery();
                }

                if (fromBuild <= 3)
                {
                    Log("Applying database update 4");

                    try // Quick Hack to correct error on build 117
                    {
                        sqlCmd.CommandText = @"CREATE TABLE `TvSeasons`
                        (
                            `Id` INTEGER NOT NULL PRIMARY KEY,
                            `IdRemote` INTEGER,
                            `IdShow` INTEGER,
                            `SeasonNumber` INTEGER,
                            `EpisodeCount` INTEGER,
                            `Hash` VARCHAR(50),
                            `Thumb` TEXT,
                            `Fanart` TEXT,
                            `IsFavorite` Integer
                        )";
                        LogQuery(sqlCmd);
                        sqlCmd.ExecuteNonQuery();
                    }
                    catch (SQLiteException)
                    {}

                    sqlCmd.CommandText = @"ALTER TABLE `TvSeasons` ADD COLUMN `Show` TEXT;";
                    LogQuery(sqlCmd);
                    sqlCmd.ExecuteNonQuery();
                }

                sqlCmd.CommandText = "UPDATE Version SET Version = " + DBVersion + ";";
                LogQuery(sqlCmd);
                sqlCmd.ExecuteNonQuery();

                sqlCmd.Dispose();
                return true;
            }
            catch (SQLiteException e)
            {
                Log(e.Message);
            }
            sqlCmd.Dispose();
            return false;
        }


        public bool CreateDatabase()
        {
            if (!_connected) return false;

            var sqlCmd = _dbConnection.CreateCommand();

            try
            {
                sqlCmd.CommandText =
                    @"CREATE TABLE `Version` 
                    ( 
                       Version INTEGER NOT NULL 
                    );
                    CREATE TABLE `Remotes`
                    (
                       Id INTEGER NOT NULL PRIMARY KEY,
                       Name VARCHAR(50) NOT NULL,
                       Api VARCHAR(50) NOT NULL,
                       ProcessName VARCHAR(50) NOT NULL,
                       IP VARCHAR(50) NOT NULL,
                       Port VARCHAR(50),
                       MacAddress VARCHAR(50),
                       Login VARCHAR(50),
                       Password VARCHAR(50),
                       Version VARCHAR(50),
                       Os VARCHAR(50),
                       Additional VARCHAR(50),
                       CacheFilled INTEGER
                    );
                    CREATE TABLE `Movies`
                    (
                       Id INTEGER NOT NULL PRIMARY KEY,
                       IdRemote INTEGER NOT NULL,
                       IdMovie INTEGER,
                       IdFile INTEGER,
                       IdScraper VARCHAR(50),
                       Title TEXT,
                       OriginalTitle TEXT,
                       Genre TEXT,
                       Tagline TEXT,
                       Plot TEXT,
                       Director TEXT,
                       Year INTEGER,
                       Length TEXT,
                       Mpaa TEXT,
                       Studio TEXT,
                       Rating VARCHAR(50),
                       Votes VARCHAR(50),
                       FileName TEXT,
                       Path TEXT,
                       PlayCount INTEGER,
                       Hash VARCHAR(50),
                       Thumb TEXT,
                       Fanart TEXT,
                       IsStack INTEGER,
                       IsFavorite INTEGER
                    );
                    CREATE TABLE `AudioGenres`
                    (
                       Id INTEGER NOT NULL PRIMARY KEY,
                       IdRemote INTEGER NOT NULL,
                       Name VARCHAR(50),
                       IdGenre INTEGER,
                       AlbumCount INTEGER,
                       Thumb TEXT,
                       Fanart TEXT,
                       IsFavorite INTEGER
                    );
                    CREATE TABLE `AudioAlbums`
                    (
                       Id INTEGER NOT NULL PRIMARY KEY,
                       IdRemote INTEGER NOT NULL,
                       Title TEXT,
                       IdAlbum INTEGER,
                       IdArtist INTEGER,
                       Artist TEXT,
                       Genre TEXT,
                       IdGenre INTEGER,
                       Year INTEGER,
                       Thumb TEXT,
                       Fanart TEXT,
                       Hash VARCHAR(50),
                       IsFavorite INTEGER
                    );
                    CREATE TABLE `AudioSongs`
                    (
                       Id INTEGER NOT NULL PRIMARY KEY,
                       IdRemote INTEGER NOT NULL,
                       IdSong INTEGER,
                       Title TEXT,
                       Track INTEGER,
                       Duration INTEGER,
                       Year INTEGER,
                       FileName TEXT,
                       IdAlbum INTEGER,
                       Album TEXT,
                       Path TEXT,
                       IdArtist INTEGER,
                       Artist TEXT,
                       IdGenre INTEGER,
                       Genre TEXT,
                       Thumb TEXT,
                       Fanart TEXT,
                       Hash VARCHAR(50),
                       IsFavorite INTEGER
                    );
                    CREATE TABLE `AudioArtists`
                    (
                       Id INTEGER NOT NULL PRIMARY KEY,
                       IdRemote INTEGER,
                       IdArtist INTEGER,
                       Name TEXT,
                       Biography TEXT,
                       Thumb TEXT,
                       Fanart TEXT,
                       Hash VARCHAR(50),
                       IsFavorite INTEGER
                    );
                    CREATE TABLE `TvShows`
                    (
                       Id INTEGER NOT NULL PRIMARY KEY,
                       IdRemote INTEGER,
                       IdShow INTEGER,
                       IdScraper VARCHAR(50),
                       Title TEXT,
                       Plot TEXT,
                       Premiered VARCHAR(50),
                       Rating VARCHAR(50),
                       Genre TEXT,
                       Mpaa TEXT,
                       Studio TEXT,
                       Path TEXT,
                       TotalCount INTEGER,
                       Hash VARCHAR(50),
                       Thumb TEXT,
                       Fanart TEXT,
                       IsFavorite INTEGER
                    );
                    CREATE TABLE `TvSeasons`
                    (
                        `Id` INTEGER NOT NULL PRIMARY KEY,
                        `IdRemote` INTEGER,
                        `IdShow` INTEGER,
                        `SeasonNumber` INTEGER,
                        `EpisodeCount` INTEGER,
                        `Hash` VARCHAR(50),
                        `Thumb` TEXT,
                        `Fanart` TEXT,
                        `IsFavorite` Integer,
                        `Show` TEXT
                    );
                    CREATE TABLE `TvEpisodes`
                    (
                       Id INTEGER NOT NULL PRIMARY KEY,
                       IdRemote INTEGER NOT NULL,
                       IdEpisode INTEGER,
                       IdFile INTEGER,
                       IdShow INTEGER,
                       Title TEXT,
                       ShowTitle TEXT,
                       Rating VARCHAR(50),
                       Plot TEXT,
                       Director TEXT,
                       Date VARCHAR(50),
                       Mpaa TEXT,
                       Studio TEXT,
                       Season INTEGER,
                       Episode INTEGER,
                       FileName TEXT,
                       Path TEXT,
                       PlayCount INTEGER,
                       Hash VARCHAR(50),
                       Thumb TEXT,
                       Fanart TEXT,
                       IsStack INTEGER,
                       IsFavorite INTEGER
                    );
                ";
                LogQuery(sqlCmd);
                sqlCmd.ExecuteNonQuery();

                sqlCmd.CommandText = "INSERT INTO `Version` (`Version`) VALUES (" + DBVersion + ");";
                LogQuery(sqlCmd);
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                return true;
            }
            catch (SQLiteException e)
            {
                Log(e.Message);
            }
            sqlCmd.Dispose();
            return false;
        }

        private long SelectDBVersion()
        {
            if (!_connected) return 0;

            long version = 0;

            using (var sqlCmd = _dbConnection.CreateCommand())
            {
                sqlCmd.CommandText = "SELECT MAX(Version) FROM `Version`;";
                LogQuery(sqlCmd);
                try
                {
                    version = (long) sqlCmd.ExecuteScalar();
                }
                catch (SQLiteException e)
                {
                    Log(e.Message);
                }
            }
            return version;
        }

        public int CheckDBVersion()
        {
            var version = SelectDBVersion();
            if (version == 0)
                return 0;
            return version == DBVersion ? 1 : 2;
        }

        public void Close()
        {
            _connected = false;
            _dbConnection.Close();
            _dbConnection.Dispose();
           
        }

        private long QueryInsert(SQLiteCommand query)
        {
            if (!_connected) return 0;
            try
            {
                LogQuery(query);
                query.ExecuteNonQuery();
                if (_bulkInsert)
                {
                    query.Dispose();
                    return 0;
                }
                query.CommandText = "SELECT last_insert_rowid()";
                query.Parameters.Clear();
                LogQuery(query);
                var result = (long)query.ExecuteScalar();
                query.Dispose();
                return result;
            }
            catch (SQLiteException e)
            {
                Log(e.Message);
            }
            query.Dispose();
            return 0;
        }

        private bool Query(SQLiteCommand query)
        {
            if (!_connected) return false;
            try
            {
                LogQuery(query);
                query.ExecuteNonQuery();
                query.Dispose();
                return true;
            }
            catch (SQLiteException e)
            {
                Log(e.Message);
            }
            query.Dispose();
            return false;
        }

        private static string GetString(IDataRecord reader, string field)
        {
            if (reader == null || String.IsNullOrEmpty(field)) 
                return "";
            var pos = reader.GetOrdinal(field);
            return reader.IsDBNull(pos) ? "" : reader.GetString(pos);
        }

        private static long GetLong(IDataRecord reader, string field)
        {
            if (reader == null || String.IsNullOrEmpty(field))
                return 0;
            var pos = reader.GetOrdinal(field);
            return reader.IsDBNull(pos) ? 0 : reader.GetInt64(pos);
        }

        public void LogQuery(SQLiteCommand query)
        {
            if (!_debug || ( query == null)) return;
            Log("Query : " + query.CommandText);
            if (query.Parameters.Count == 0) return;
            var parameters = query.Parameters.Cast<SQLiteParameter>().Aggregate("",
                                                                                (current, p) =>
                                                                                current +
                                                                                (p.ParameterName + "=" + p.Value + " , "));
            Log("Param : " + parameters.TrimEnd(new[] {' ',','}));
        }

        public void Log(string message)
        {
            if (!_debug) return;
            Logger.Instance().Log("FrontViewDB",message);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Close();
            }
        }
    }
}