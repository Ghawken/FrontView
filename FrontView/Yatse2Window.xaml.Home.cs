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

using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Animation;
using Setup;
using FrontView.Classes;
using FrontView.Libs;

namespace FrontView
{
    public partial class Yatse2Window
    {
        private void Load_TvShows()
        {
            if (_remoteInfo == null)
                return;
            if (_tvShowsDataSource.Count < 1 && _config.TVOrderbyNewEpsiodes == true)
            {
                _tvShowsDataSource.Load(_database.GetTvShowOrderNewEpisode(_remoteInfo.Id));
                Helper.VirtFlowSelect(lst_TvShows_flow, 0);
            }
            else if (_tvShowsDataSource.Count <1 && _config.TVOrderbyNewEpsiodes == false)
            {
                _tvShowsDataSource.Load(_database.GetTvShow(_remoteInfo.Id));
                Helper.VirtFlowSelect(lst_TvShows_flow, 0);

            }
        }

        private void Load_TvEpisodes(string idShow)
        {
            if (_remoteInfo == null)
                return;

            _tvEpisodesDataSource.Load(_database.GetTvEpisodeFromTvShow(_remoteInfo.Id,idShow));

            if (_tvEpisodesCollectionView.Count <= 0) return;

            lst_TvEpisodes_flow.SelectedIndex = 0;
            if (_tvEpisodesDataSource.Count > 0)
                lst_TvEpisodes_flow.ScrollIntoView(_tvEpisodesDataSource[0]);
        }

        private void Load_TvEpisodesSeason(string idShow, long idSeason)
        {
            if (_remoteInfo == null)
                return;

            _tvEpisodesDataSource.Load(_database.GetTvEpisodeFromTvShowSeason(_remoteInfo.Id, idShow,idSeason));
            if (_tvEpisodesCollectionView.Count > 0)
            {
                lst_TvEpisodes_flow.SelectedIndex = 0;
                if (_tvEpisodesDataSource.Count > 0)
                    lst_TvEpisodes_flow.ScrollIntoView(_tvEpisodesDataSource[0]);
            }
        }

        private void Load_TvSeasons(string idShow)
        {
            if (_remoteInfo == null)
                return;



            _tvSeasonsDataSource.Load(_database.GetTvSeasonFromTvShow(_remoteInfo.Id, idShow));

            Logger.Instance().Trace("FrontView+", "Load TV Seasons:  SHOW:"+ idShow +" tv Episodes: DataSource Count:" + _tvEpisodesDataSource.Count );

            Logger.Instance().Trace("FrontView+", "Load TV Seasons:  SHOW:"+ idShow +" tv Seasons: DataSource Count:" + _tvSeasonsDataSource.Count);

            _tvSeasonsDataSource.Insert(0, new Yatse2TvSeason { IdShow  = 0, Show = idShow, SeasonNumber = -1, EpisodeCount = 0, Fanart = "", Thumb = "" });
            //above - dont understand the purpose of this?

            Logger.Instance().Trace("FrontView+","Load TV Seasons:  tvSeasons CollectionView Count:"+_tvSeasonsCollectionView.Count);
            
            if (_tvSeasonsCollectionView.Count > 0)
            {
                lst_TvSeasons_flow.SelectedIndex = 0;
                if (_tvSeasonsDataSource.Count > 0)
                {
                    lst_TvSeasons_flow.ScrollIntoView(_tvSeasonsDataSource[0]);
                    Helper.VirtFlowSelect(lst_TvSeasons_flow, 0);
                }
            }
        }

        private void Load_Movies()
        {
            if (_remoteInfo == null)
                return;

            if (_moviesDataSource.Count < 1 && _config.TVOrderbyNewEpsiodes == false)
            {
                _moviesDataSource.Load(_database.GetMovie(_remoteInfo.Id));
                Helper.VirtFlowSelect(lst_Movies_flow, 0);
            }
            else if (_moviesDataSource.Count < 1 && _config.TVOrderbyNewEpsiodes == true)
            {
                _moviesDataSource.Load(_database.GetMovieSortDateAdded(_remoteInfo.Id));
                Helper.VirtFlowSelect(lst_Movies_flow, 0);
            }
        }


        private void Load_AudioGenres()
        {
            if (_remoteInfo == null)
                return;
            if (_audioGenresDataSource.Count < 1)
            {
                _audioGenresDataSource.Load(_config.ShowEmptyMusicGenre
                                                ? _database.GetAudioGenre(_remoteInfo.Id)
                                                : _database.GetAudioGenreNotEmpty(_remoteInfo.Id));
                Helper.VirtFlowSelect(lst_AudioGenres_flow, 0);
            }
        }


        private void Load_AudioSongs(string album)
        {
            if (_remoteInfo == null)
                return;
            
            _audioSongsDataSource.Load(_database.GetAudioSongFromAlbumName(_remoteInfo.Id,album));
            if (_audioSongsDataSource.Count > 0)
            {
                lst_AudioAlbums_Details.SelectedIndex = 0;
                lst_AudioAlbums_Details.ScrollIntoView(_audioSongsDataSource[0]);
            }

        }

        private void Load_AudioAlbums()
        {
            if (_remoteInfo == null)
                return;

            if (_audioAlbumsDataSource.Count < 1 || _filteredAlbums)
            {
                _audioAlbumsDataSource.Load(_database.GetAudioAlbum(_remoteInfo.Id));
                Helper.VirtFlowSelect(lst_AudioAlbums_flow, 0);
                _filteredAlbums = false;
            }  
        }

        private void Load_AudioArtists()
        {
            if (_remoteInfo == null)
                return;

            if (_audioArtistsDataSource.Count < 1 || _filteredArtists)
            {
                _audioArtistsDataSource.Load(_config.HideCompilationArtists
                                                 ? _database.GetAudioArtistNoCompilation(_remoteInfo.Id)
                                                 : _database.GetAudioArtist(_remoteInfo.Id));
                Helper.VirtFlowSelect(lst_AudioArtists_flow, 0);
                _filteredArtists = false;
            }
        }

        private void Load_AudioAlbumsGenre(string genre)
        {
            if (_remoteInfo == null)
                return;

            _audioAlbumsDataSource.Load(_database.GetAudioAlbumFromGenre(_remoteInfo.Id,genre));
            Helper.VirtFlowSelect(lst_AudioAlbums_flow, 0);
        }

        private void Load_AudioAlbumsArtists(string artist)
        {
            if (_remoteInfo == null)
                return;

            _audioAlbumsDataSource.Load(_config.HideCompilationArtists
                                 ? _database.GetAudioAlbumFromArtistNoCompilation(_remoteInfo.Id, artist)
                                 : _database.GetAudioAlbumFromArtist(_remoteInfo.Id, artist));
            Helper.VirtFlowSelect(lst_AudioAlbums_flow, 0);
        }

        private void Load_AudioArtistsGenre(string genre)
        {
            if (_remoteInfo == null)
                return;

            _audioArtistsDataSource.Load(_config.HideCompilationArtists
                                 ? _database.GetAudioArtistFromGenreNoCompilation(_remoteInfo.Id,genre)
                                 : _database.GetAudioArtistFromGenre(_remoteInfo.Id,genre));

            Helper.VirtFlowSelect(lst_AudioArtists_flow, 0);
            
        }


        private void txb_Home_Movies_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Load_Movies();
            if (_moviesCollectionView.Count >0)
                ShowGrid(grd_Movies);
            else
            {
                if (_moviesDataSource.Count < 1)
                {
                    Logger.Instance().Log("FrontView+", "No Movie Data");
                    var stbDimmingShow = (Storyboard) TryFindResource("stb_NoVideoData");
                    if (stbDimmingShow != null)
                        stbDimmingShow.Begin(this);
                }
                else
                {
                    Logger.Instance().Log("FrontView+", "No Movie Favorites");
                    var stbDimmingShow = (Storyboard)TryFindResource("stb_NoVideoFavorites");
                    if (stbDimmingShow != null)
                        stbDimmingShow.Begin(this);
                }
            }
        }

        private void txb_Home_TvShows_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Load_TvShows();
            if (_tvShowsCollectionView.Count > 0)
                ShowGrid(grd_TvShows);
            else
            {
                if (_tvShowsDataSource.Count < 1)
                {
                    Logger.Instance().Log("FrontView+", "No TvShow Data");
                    var stbDimmingShow = (Storyboard)TryFindResource("stb_NoVideoData");
                    if (stbDimmingShow != null)
                        stbDimmingShow.Begin(this);
                }
                else
                {
                    Logger.Instance().Log("FrontView+", "No TvShow Favorites");
                    var stbDimmingShow = (Storyboard)TryFindResource("stb_NoVideoFavorites");
                    if (stbDimmingShow != null)
                        stbDimmingShow.Begin(this);
                }
            }
        }

        private void txb_Home_MusicArtists_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Load_AudioArtists();
            if (_audioArtistsCollectionView.Count > 0)
                ShowGrid(grd_AudioArtists);
            else
            {
                if (_audioArtistsDataSource.Count < 1)
                {
                    Logger.Instance().Log("FrontView+", "No Artists Data");
                    var stbDimmingShow = (Storyboard)TryFindResource("stb_NoMusicData");
                    if (stbDimmingShow != null)
                        stbDimmingShow.Begin(this);
                }
                else
                {
                    Logger.Instance().Log("FrontView+", "No Artists Favorites");
                    var stbDimmingShow = (Storyboard)TryFindResource("stb_NoMusicFavorites");
                    if (stbDimmingShow != null)
                        stbDimmingShow.Begin(this);
                }
            }
        }

        private void txb_Home_MusicAlbums_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Load_AudioAlbums();
            if (_audioAlbumsCollectionView.Count > 0)
                ShowGrid(grd_AudioAlbums);
            else
            {
                if (_audioAlbumsDataSource.Count < 1)
                {
                    Logger.Instance().Log("FrontView+", "No Albums Data");
                    var stbDimmingShow = (Storyboard)TryFindResource("stb_NoMusicData");
                    if (stbDimmingShow != null)
                        stbDimmingShow.Begin(this);
                }
                else
                {
                    Logger.Instance().Log("FrontView+", "No Albums Favorites");
                    var stbDimmingShow = (Storyboard)TryFindResource("stb_NoMusicFavorites");
                    if (stbDimmingShow != null)
                        stbDimmingShow.Begin(this);
                }
            }
        }

        private void txb_Home_MusicGenres_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Load_AudioGenres();
            if (_audioGenresCollectionView.Count > 0)
                ShowGrid(grd_AudioGenres);
            else
            {
                if (_audioGenresDataSource.Count < 1)
                {
                    Logger.Instance().Log("FrontView+", "No Genre Data");
                    var stbDimmingShow = (Storyboard)TryFindResource("stb_NoMusicData");
                    if (stbDimmingShow != null)
                        stbDimmingShow.Begin(this);
                }
                else
                {
                    Logger.Instance().Log("FrontView+", "No Genre Favorites");
                    var stbDimmingShow = (Storyboard)TryFindResource("stb_NoMusicFavorites");
                    if (stbDimmingShow != null)
                        stbDimmingShow.Begin(this);
                }
            }
        }

        private void btn_Home_ShowQuit_Click(object sender, RoutedEventArgs e)
        {
            var stbDimmingShow = (Storyboard)TryFindResource("stb_Home_ShowQuit");
            if (stbDimmingShow != null)
                stbDimmingShow.Begin(this);
        }

        private void btn_Home_Remotes_Click(object sender, RoutedEventArgs e)
        {
            ShowGrid(grd_Remotes);
        }

        private void btn_Home_Video_FilterFavorites_Click(object sender, RoutedEventArgs e)
        {
            if (!_videoFavoritesFilter)
            {
                _videoFavoritesFilter = true;
                btn_Home_Video_FilterFavorites.Background = GetSkinImageBrush("Filter_FavoritesOn");
                /*_moviesCollectionView.Filter = IsFavorite;
                _tvShowsCollectionView.Filter = IsFavorite;*/
            }
            else
            {
                _videoFavoritesFilter = false;
                btn_Home_Video_FilterFavorites.Background = GetSkinImageBrush("Filter_Favorites");
                /*_moviesCollectionView.Filter = null;
                _tvShowsCollectionView.Filter = null;*/
            }
            _moviesCollectionView.Refresh();
            _tvShowsCollectionView.Refresh();
            Helper.VirtFlowSelect(lst_Movies_flow, 0);
            Helper.VirtFlowSelect(lst_TvShows_flow, 0);
        }

        private void btn_Home_Music_FilterFavorites_Click(object sender, RoutedEventArgs e)
        {
            if (!_audioFavoritesFilter)
            {
                _audioFavoritesFilter = true;
                btn_Home_Music_FilterFavorites.Background = GetSkinImageBrush("Filter_FavoritesOn");
               /* _audioGenresCollectionView.Filter = IsFavorite;
                _audioAlbumsCollectionView.Filter = IsFavorite;
                _audioArtistsCollectionView.Filter = IsFavorite;*/
            }
            else
            {
                _audioFavoritesFilter = false;
                btn_Home_Music_FilterFavorites.Background = GetSkinImageBrush("Filter_Favorites");
                /*_audioGenresCollectionView.Filter = null;
                _audioAlbumsCollectionView.Filter = null;
                _audioArtistsCollectionView.Filter = null;*/
            }
            _audioArtistsCollectionView.Refresh();
            _audioGenresCollectionView.Refresh();
            _audioAlbumsCollectionView.Refresh();

            Helper.VirtFlowSelect(lst_AudioGenres_flow, 0);
            Helper.VirtFlowSelect(lst_AudioArtists_flow, 0);
            Helper.VirtFlowSelect(lst_AudioAlbums_flow, 0);
        }


        private void btn_Home_Close_Click(object sender, RoutedEventArgs e)
        {
            Logger.Instance().Log("FrontView+", "Closing", true);
            // Turn Off Button for testing subroutines
            //SetMonitorState();
           // ScreenResolution.TurnOffDevice()

            //ni.Dispose();
            //ni = null;
            //Close();
            System.Windows.Application.Current.Shutdown();
        }

        private void btn_Home_Reboot_Click(object sender, RoutedEventArgs e)
        {
            //Close running Remote Program.

            var selitem = (Yatse2Remote)lst_Remotes.SelectedItem;
                if (selitem == null) return;

                var remote = ApiHelper.Instance().GetRemoteByApi(selitem.Api);
                remote.Configure(selitem.IP, selitem.Port, selitem.Login, selitem.Password);
                remote.SystemRunning.Quit();
                remote.Close(); 
            
          //  Logger.Instance().Log("FrontView+", "Reboot", true);
          //  Process.Start("shutdown.exe", "-r -t 01");
        }

        private void btn_Home_Hibernate_Click(object sender, RoutedEventArgs e)
        {
              Logger.Instance().Log("FrontView+", "Reboot", true);
              Process.Start("shutdown.exe", "-s -t 01");


        }
        
        private void btn_Time_Click(object sender, RoutedEventArgs e)
        {
            ShowGrid(grd_Time);
        }

        private void btn_Home_Diaporama_Click(object sender, RoutedEventArgs e)
        {
            StartDiaporama();
        }

    }
}