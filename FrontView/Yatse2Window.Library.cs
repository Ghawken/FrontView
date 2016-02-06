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
using System.Collections.Generic;
using System.IO;
using Setup;
using FrontView.Classes;
using FrontView.Libs;
using Application = System.Windows.Forms.Application;
using Plugin;

namespace FrontView
{
    public partial class Yatse2Window
    {

        private void RefreshTvShowLibrary()
        {
            Logger.Instance().Log("FrontView+", "Start Refresh : TvShows");
            var res = _remote.VideoLibrary.GetTvShows();
            Logger.Instance().Log("FrontView+", "Remote TvShows : " + res.Count);

            var oldData = _database.GetTvShowFavorites(_remoteInfo.Id);

            _database.SetBulkInsert(true);
            _database.BeginTransaction();
            _database.DeleteRemoteTvShows(_remoteInfo.Id);
            foreach (var apiTvShow in res)
            {
                long oldFavorite = 0;
                foreach (var show in oldData)
                {
                    if (show.IdShow == apiTvShow.IdShow)
                        oldFavorite = show.IsFavorite;
                }
                var tvShow = new Yatse2TvShow(apiTvShow) {IdRemote = _remoteInfo.Id, IsFavorite = oldFavorite};
                _database.InsertTvShow(tvShow);
            }
            _database.CommitTransaction();
            _database.SetBulkInsert(false);
            Logger.Instance().Log("FrontView+", "End Refresh : TvShows");
        }

        private void RefreshTvSeasonLibrary()
        {
            Logger.Instance().Log("FrontView+", "Start Refresh : TvSeasons");
            var res = _remote.VideoLibrary.GetTvSeasons();
            Logger.Instance().Log("FrontView+", "Remote TvSeasons : " + res.Count);

            var oldData = _database.GetTvSeasonFavorites(_remoteInfo.Id);

            _database.SetBulkInsert(true);
            _database.BeginTransaction();
            _database.DeleteRemoteTvSeasons(_remoteInfo.Id);
            foreach (var apiTvSeason in res)
            {
                long oldFavorite = 0;
                foreach (var show in oldData)
                {
                    if (show.IdShow == apiTvSeason.IdShow)
                        oldFavorite = show.IsFavorite;
                }
                var tvSeason = new Yatse2TvSeason(apiTvSeason) {IdRemote = _remoteInfo.Id, IsFavorite = oldFavorite};
                _database.InsertTvSeason(tvSeason);
            }
            _database.CommitTransaction();
            _database.SetBulkInsert(false);
            Logger.Instance().Log("FrontView+", "End Refresh : TvSeasons");
        }


        private void RefreshTvEpisodesLibrary()
        {
            Logger.Instance().Log("FrontView+", "Start Refresh : TvEpisodes");
            var res = _remote.VideoLibrary.GetTvEpisodes();
            Logger.Instance().Log("FrontView+", "Remote TvEpisodes : " + res.Count);

            var oldData = _database.GetTvEpisodeFavorites(_remoteInfo.Id);

            _database.SetBulkInsert(true);
            _database.BeginTransaction();
            _database.DeleteRemoteTvEpisodes(_remoteInfo.Id);
            foreach (var apiTvEpisode in res)
            {
                long oldFavorite = 0;
                foreach (var episode in oldData)
                {
                    if (episode.IdEpisode == apiTvEpisode.IdEpisode)
                        oldFavorite = episode.IsFavorite;
                }
                var tvEpisode = new Yatse2TvEpisode(apiTvEpisode) {IdRemote = _remoteInfo.Id, IsFavorite = oldFavorite};
                _database.InsertTvEpisode(tvEpisode);
            }
            _database.CommitTransaction();
            _database.SetBulkInsert(false);
            Logger.Instance().Log("FrontView+", "End Refresh : TvEpisodes");
        }

        private void RefreshMovieLibrary()
        {
            Logger.Instance().Log("FrontView+", "Start Refresh : Movies");
            var res = _remote.VideoLibrary.GetMovies();
            Logger.Instance().Log("FrontView+", "Remote Movies : " + res.Count);

            var oldData = _database.GetMovieFavorites(_remoteInfo.Id);

            _database.SetBulkInsert(true);
            _database.BeginTransaction();
            _database.DeleteRemoteMovie(_remoteInfo.Id);
            foreach (var apiMovie in res)
            {
                long oldFavorite = 0;
                foreach (var omovie in oldData)
                {
                    if (omovie.IdMovie == apiMovie.IdMovie)
                        oldFavorite = omovie.IsFavorite;
                }

                var movie = new Yatse2Movie(apiMovie) {IdRemote = _remoteInfo.Id, IsFavorite = oldFavorite};
                _database.InsertMovie(movie);
            }
            _database.CommitTransaction();
            _database.SetBulkInsert(false);
            Logger.Instance().Log("FrontView+", "End Refresh : Movies");
        }

        private void RefreshMusicGenreLibrary()
        {
            Logger.Instance().Log("FrontView+", "Start Refresh : MusicGenres");
            var res = _remote.AudioLibrary.GetGenres();
            Logger.Instance().Log("FrontView+","Remote MusicGenres : " + res.Count);
            var oldData = _database.GetAudioGenreFavorites(_remoteInfo.Id);

            _database.SetBulkInsert(true);
            _database.BeginTransaction();
            _database.DeleteRemoteAudioGenre(_remoteInfo.Id);
            foreach (var apiMusicGenre in res)
            {
                long oldFavorite = 0;
                foreach (var ogenre in oldData)
                {
                    if (ogenre.IdGenre == apiMusicGenre.IdGenre)
                        oldFavorite = ogenre.IsFavorite;
                }
                var musicGenre = new Yatse2AudioGenre(apiMusicGenre) { IdRemote = _remoteInfo.Id, IsFavorite = oldFavorite };
                _database.InsertAudioGenre(musicGenre);
            }
            _database.CommitTransaction();
            _database.SetBulkInsert(false);
            Logger.Instance().Log("FrontView+","End Refresh : MusicGenres");
        }

        private void RefreshMusicArtistsLibrary()
        {
            Logger.Instance().Log("FrontView+","Start Refresh : MusicArtists");
            var res = _remote.AudioLibrary.GetArtists();
            Logger.Instance().Log("FrontView+","Remote MusicArtists : " + res.Count);

            var oldData = _database.GetAudioArtistFavorites(_remoteInfo.Id);

            _database.SetBulkInsert(true);
            _database.BeginTransaction();
            _database.DeleteRemoteAudioArtist(_remoteInfo.Id);
            foreach (var apiMusicArtist in res)
            {
                long oldFavorite = 0;
                foreach (var oartist in oldData)
                {
                    if (oartist.IdArtist == apiMusicArtist.IdArtist)
                        oldFavorite = oartist.IsFavorite;
                }
                var musicArtist = new Yatse2AudioArtist(apiMusicArtist) { IdRemote = _remoteInfo.Id, IsFavorite = oldFavorite };
                _database.InsertAudioArtist(musicArtist);
            }
            _database.CommitTransaction();
            _database.SetBulkInsert(false);
            Logger.Instance().Log("FrontView+","End Refresh : MusicArtists");
        }

        private void RefreshMusicAlbumsLibrary()
        {
            Logger.Instance().Log("FrontView+","Start Refresh : MusicAlbums");
            var res = _remote.AudioLibrary.GetAlbums();
            Logger.Instance().Log("FrontView+","Remote MusicAlbums : " + res.Count);
            var oldData = _database.GetAudioAlbumFavorites(_remoteInfo.Id);

            _database.SetBulkInsert(true);
            _database.BeginTransaction();
            _database.DeleteRemoteAudioAlbum(_remoteInfo.Id);
            foreach (var apiMusicAlbum in res)
            {
                long oldFavorite = 0;
                foreach (var oalbum in oldData)
                {
                    if (oalbum.IdAlbum == apiMusicAlbum.IdAlbum)
                        oldFavorite = oalbum.IsFavorite;
                }
                var musicAlbum = new Yatse2AudioAlbum(apiMusicAlbum) { IdRemote = _remoteInfo.Id, IsFavorite = oldFavorite };
                _database.InsertAudioAlbum(musicAlbum);
            }
            _database.CommitTransaction();
            _database.SetBulkInsert(false);
            Logger.Instance().Log("FrontView+","End Refresh : MusicAlbums");
        }

        private void RefreshMusicSongsLibrary()
        {
            Logger.Instance().Log("FrontView+","Start Refresh : MusicSongs");
            var res = _remote.AudioLibrary.GetSongs();
            Logger.Instance().Log("FrontView+","Remote MusicSongs : " + res.Count);
            var oldData = _database.GetAudioSongFavorites(_remoteInfo.Id);

            _database.SetBulkInsert(true);
            _database.BeginTransaction();
            _database.DeleteRemoteAudioSong(_remoteInfo.Id);
            foreach (var apiMusicSong in res)
            {
                long oldFavorite = 0;
                foreach (var osong in oldData)
                {
                    if (osong.IdSong == apiMusicSong.IdSong)
                        oldFavorite = osong.IsFavorite;
                }
                var musicSong = new Yatse2AudioSong(apiMusicSong) {IdRemote = _remoteInfo.Id, IsFavorite = oldFavorite};
                _database.InsertAudioSong(musicSong);
            }
            _database.CommitTransaction();
            _database.SetBulkInsert(false);
            Logger.Instance().Log("FrontView+","End Refresh : MusicSongs");
        }

        private void RefreshTFAudioAlbums(ref List<ApiImageDownloadInfo> dlinfo)
        {
            var lines = _database.GetAudioAlbum(_remoteInfo.Id);
            foreach (var line in lines)
            {
                if (line.Thumb == "NONE" || String.IsNullOrEmpty(line.Thumb)) continue;
                var path = Helper.CachePath + @"Music\Thumbs\" + _remotePlugin.GetHashFromFileName(line.Thumb) + ".jpg";
                if (File.Exists(path)) continue;
                var info = new ApiImageDownloadInfo
                               {
                                   Destination = path,
                                   Source = line.Thumb,
                                   ToThumb = _config.CropCacheImage,
                                   MaxHeight = (int)Height/2
                               };
                dlinfo.Add(info);
            }
        }

        private void RefreshTFAudioArtists(ref List<ApiImageDownloadInfo> dlinfo)
        {
            var lines = _database.GetAudioArtist(_remoteInfo.Id);
            foreach (var line in lines)
            {
                if (line.Thumb != "NONE" && ! String.IsNullOrEmpty(line.Thumb))
                {
                    var path = Helper.CachePath + @"Music\Artists\" + _remotePlugin.GetHashFromFileName(line.Thumb) + ".jpg";
                    if (!File.Exists(path))
                    {
                        var info = new ApiImageDownloadInfo
                        {
                            Destination = path,
                            Source = line.Thumb,
                            ToThumb = _config.CropCacheImage,
                            MaxHeight = (int)Height / 2
                        };
                        dlinfo.Add(info);
                    }
                }
                if (line.Fanart != "NONE" && !String.IsNullOrEmpty(line.Fanart))
                {
                    var path = Helper.CachePath + @"Music\Fanarts\" + _remotePlugin.GetHashFromFileName(line.Fanart) + ".jpg";
                    if (!File.Exists(path))
                    {
                        var info = new ApiImageDownloadInfo
                        {
                            Destination = path,
                            Source = line.Fanart,
                            ToThumb = _config.CropCacheImage,
                            MaxHeight = (int)Height
                        };
                        dlinfo.Add(info);
                    }
                }
            }
        }

        private void RefreshTFMovies(ref List<ApiImageDownloadInfo> dlinfo)
        {
            var lines = _database.GetMovie(_remoteInfo.Id);
            foreach (var line in lines)
            {
                if (line.Thumb != "NONE" && !String.IsNullOrEmpty(line.Thumb))
                {
                    var path = Helper.CachePath + @"Video\Thumbs\" + _remotePlugin.GetHashFromFileName(line.Thumb) + ".jpg";
                    if (!File.Exists(path))
                    {
                        var info = new ApiImageDownloadInfo
                        {
                            Destination = path,
                            Source = line.Thumb,
                            ToThumb = _config.CropCacheImage,
                            MaxHeight = (int)Height / 2
                        };
                        dlinfo.Add(info);
                    }
                }
                if (line.Fanart != "NONE" && !String.IsNullOrEmpty(line.Fanart))
                {
                    var path = Helper.CachePath + @"Video\Fanarts\" + _remotePlugin.GetHashFromFileName(line.Fanart) + ".jpg";
                    if (!File.Exists(path))
                    {
                        var info = new ApiImageDownloadInfo
                        {
                            Destination = path,
                            Source = line.Fanart,
                            ToThumb = _config.CropCacheImage,
                            MaxHeight = (int)Height
                        };
                        dlinfo.Add(info);
                    }
                }
            }
        }

        private void RefreshTFTvShows(ref List<ApiImageDownloadInfo> dlinfo)
        {
            var lines = _database.GetTvShow(_remoteInfo.Id);
            foreach (var line in lines)
            {
                if (line.Thumb != "NONE" && !String.IsNullOrEmpty(line.Thumb))
                {
                    var path = Helper.CachePath + @"Video\Thumbs\" + _remotePlugin.GetHashFromFileName(line.Thumb) + ".jpg";
                    if (!File.Exists(path))
                    {
                        var info = new ApiImageDownloadInfo
                        {
                            Destination = path,
                            Source = line.Thumb,
                            ToThumb = _config.CropCacheImage,
                            MaxHeight = (int)Height / 2
                        };
                        dlinfo.Add(info);
                    }
                }
                if (line.Fanart != "NONE" && !String.IsNullOrEmpty(line.Fanart))
                {
                    var path = Helper.CachePath + @"Video\Fanarts\" + _remotePlugin.GetHashFromFileName(line.Fanart) + ".jpg";
                    if (!File.Exists(path))
                    {
                        var info = new ApiImageDownloadInfo
                        {
                            Destination = path,
                            Source = line.Fanart,
                            ToThumb = _config.CropCacheImage,
                            MaxHeight = (int)Height
                        };
                        dlinfo.Add(info);
                    }
                }
            }
        }

        private void RefreshTFTvEpisodes(ref List<ApiImageDownloadInfo> dlinfo)
        {
            var lines = _database.GetTvEpisode(_remoteInfo.Id);
            foreach (var line in lines)
            {
                if (line.Thumb != "NONE" && !String.IsNullOrEmpty(line.Thumb))
                {
                    var path = Helper.CachePath + @"Video\Thumbs\" + _remotePlugin.GetHashFromFileName(line.Thumb) + ".jpg";
                    if (!File.Exists(path))
                    {
                        var info = new ApiImageDownloadInfo
                        {
                            Destination = path,
                            Source = line.Thumb,
                            ToThumb = _config.CropCacheImage,
                            MaxHeight = (int)Height / 2
                        };
                        dlinfo.Add(info);
                    }
                }
                if (line.Fanart != "NONE" && !String.IsNullOrEmpty(line.Fanart))
                {
                    var path = Helper.CachePath + @"Video\Fanarts\" + _remotePlugin.GetHashFromFileName(line.Fanart) + ".jpg";
                    if (!File.Exists(path))
                    {
                        var info = new ApiImageDownloadInfo
                        {
                            Destination = path,
                            Source = line.Fanart,
                            ToThumb = _config.CropCacheImage,
                            MaxHeight = (int)Height
                        };
                        dlinfo.Add(info);
                    }
                }
            }
        }

        private void RefreshTFTvSeasons(ref List<ApiImageDownloadInfo> dlinfo)
        {
            var lines = _database.GetTvSeason(_remoteInfo.Id);
            foreach (var line in lines)
            {
                if (line.Thumb != "NONE" && !String.IsNullOrEmpty(line.Thumb))
                {
                    var path = Helper.CachePath + @"Video\Thumbs\" + _remotePlugin.GetHashFromFileName(line.Thumb) + ".jpg";
                    if (!File.Exists(path))
                    {
                        var info = new ApiImageDownloadInfo
                        {
                            Destination = path,
                            Source = line.Thumb,
                            ToThumb = _config.CropCacheImage,
                            MaxHeight = (int)Height / 2
                        };
                        dlinfo.Add(info);
                    }
                }
                if (line.Fanart != "NONE" && !String.IsNullOrEmpty(line.Fanart))
                {
                    var path = Helper.CachePath + @"Video\Fanarts\" + _remotePlugin.GetHashFromFileName(line.Fanart) + ".jpg";
                    if (!File.Exists(path))
                    {
                        var info = new ApiImageDownloadInfo
                        {
                            Destination = path,
                            Source = line.Fanart,
                            ToThumb = _config.CropCacheImage,
                            MaxHeight = (int)Height
                        };
                        dlinfo.Add(info);
                    }
                }
            }
        }

        private void RefreshThumbsFanarts()
        {
            Logger.Instance().Log("FrontView+","Start Refresh : Thumbs/Fanarts");
            _remote.File.StopAsync();

            var dlinfo = new List<ApiImageDownloadInfo>();

            RefreshTFAudioAlbums(ref dlinfo);
            RefreshTFAudioArtists(ref dlinfo);
            RefreshTFMovies(ref dlinfo);
            RefreshTFTvShows(ref dlinfo);
            RefreshTFTvEpisodes(ref dlinfo);
            RefreshTFTvSeasons(ref dlinfo);

            _yatse2Properties.IsSyncing = true;
            Logger.Instance().Log("FrontView+", "Refresh : Thumbs/Fanarts : " + dlinfo.Count + " files");
            _remote.File.AsyncDownloadImages(dlinfo.ToArray());
            _remoteInfo.CacheFilled = 0;
            _database.UpdateRemote(_remoteInfo);
            Logger.Instance().Log("FrontView+", "End Refresh : Thumbs/Fanarts");

        }

        private void RefreshLibrary()
        {
            if (! _remote.IsConnected())
                return;
            _yatse2Properties.ShowRefreshLibrary = true;
            _remote.File.StopAsync();
            Application.DoEvents();
            Logger.Instance().Log("FrontView+", "Starting Library Refresh", true);
            _database.SetDebug(false);
            _yatse2Properties.RefreshWhat = GetLocalizedString(1);
            Application.DoEvents();
            RefreshMovieLibrary();
            _yatse2Properties.RefreshWhat = GetLocalizedString(2);
            Application.DoEvents();
            RefreshTvShowLibrary();
            RefreshTvSeasonLibrary();
            _yatse2Properties.RefreshWhat = GetLocalizedString(3);
            Application.DoEvents();
            RefreshTvEpisodesLibrary();
            _yatse2Properties.RefreshWhat = GetLocalizedString(4);
            Application.DoEvents();
            RefreshMusicGenreLibrary();
            _yatse2Properties.RefreshWhat = GetLocalizedString(5);
            Application.DoEvents();
            RefreshMusicAlbumsLibrary();
            _yatse2Properties.RefreshWhat = GetLocalizedString(6);
            Application.DoEvents();
            RefreshMusicSongsLibrary();
            _yatse2Properties.RefreshWhat = GetLocalizedString(7);
            Application.DoEvents();
            RefreshMusicArtistsLibrary();
            _yatse2Properties.RefreshWhat = GetLocalizedString(8);
            Application.DoEvents();
            _database.Compress();
            RefreshThumbsFanarts();
            _database.SetDebug(_config.Debug);
            Logger.Instance().Log("FrontView+", "End Library Refresh", true);
            _remoteLibraryRefreshed = true;
            ShowPopup(GetLocalizedString(101));
            _yatse2Properties.ShowRefreshLibrary = false;
        }

    }

}