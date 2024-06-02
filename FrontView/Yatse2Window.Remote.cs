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
using System.IO;
using System.Globalization;
using System.Windows;
using System.Windows.Media.Animation;
using Setup;
using FrontView.Classes;
using FrontView.Libs;

namespace FrontView
{
    public partial class Yatse2Window
    {

        private void InitRemote()
        {
            if (_remote != null)
            {
                _remote.Close();
                _remote.Dispose();
            }
            _remoteConnected = false;
            _remoteLibraryRefreshed = !_config.RefreshOnConnect;

            _remote = ApiHelper.Instance().GetRemoteByApi(null);
            if (_currentRemoteId == 0)
            {
                Logger.Instance().Log("FrontView+", "No current Remote");
                _remoteInfo = new Yatse2Remote { Id = 0, CacheFilled = 1 };
                return;
            }

            var remotes = _database.GetRemote(_currentRemoteId);
            if (remotes.Count < 1)
            {
                Logger.Instance().Log("FrontView+", "No remote found");
                return;
            }

            var remoteInfo = remotes[0];
            Logger.Instance().Log("FrontView+", "Init remote : " + remoteInfo.Id + " - " + remoteInfo.Name + " (" + remoteInfo.Api + " / " + remoteInfo.Version + ")");

            _remote = ApiHelper.Instance().GetRemoteByApi(remoteInfo.Api);
            if (_remote == null)
            {
                Logger.Instance().Log("FrontView+", "Error plugin not loaded for API : " + remoteInfo.Api);
                return;
            }

            _remotePlugin = ApiHelper.Instance().GetRemotePluginByApi(remoteInfo.Api);

            txb_Home_Movies.Visibility = (_remotePlugin.SupportedFunctions().MovieLibrary) ? Visibility.Visible : Visibility.Hidden;
            txb_Home_TvShows.Visibility = (_remotePlugin.SupportedFunctions().TvShowLibrary) ? Visibility.Visible : Visibility.Hidden;
            txb_Home_Artists.Visibility = (_remotePlugin.SupportedFunctions().AudioLibrary) ? Visibility.Visible : Visibility.Hidden;
            txb_Home_Albums.Visibility = (_remotePlugin.SupportedFunctions().AudioLibrary) ? Visibility.Visible : Visibility.Hidden;
            txb_Home_Genres.Visibility = (_remotePlugin.SupportedFunctions().AudioLibrary) ? Visibility.Visible : Visibility.Hidden;

//SupportsRemoteControl
            UpdateButtonVisibility();



            _failedRemoteCheck = false;

            _remote.Configure(remoteInfo.IP, remoteInfo.Port, remoteInfo.Login, remoteInfo.Password);
            _remoteInfo = remoteInfo;

            Helper.Instance.CurrentApi = _remoteInfo.Api;

            //Helper.Instance.UseCoverArt = (bool)_config.CoverArt;

            _yatse2Properties.Api = _remoteInfo.Api;

            ClearFiltersAndDataSource();

            if (_currentGrid.Name == "grd_Home")
            {
                _gridHistory.Clear();
            }
            else
            {
                _gridHistory.Clear();
                _gridHistory.Add("grd_Home");
            }

        }
        
        private void UpdateButtonVisibility()
        {
            //SuportsRemoteControl Changes
            var supportsremote = _remotePlugin.SupportedFunctions().SupportsRemoteControl;

            Logger.Instance().Log("FrontView+:UpdateButton Visibility", "Checking Buttons Visibility: Supports Remote Equals:"+supportsremote);

            if (supportsremote == true)
            {
                txb_SupportsRemoteControlMusic.Visibility = Visibility.Visible;
                txb_SupportsRemoteControlMovie.Visibility = Visibility.Visible;
                txb_SupportsRemoteControlTv.Visibility = Visibility.Visible;
            }


        }
        

        private void ClearFiltersAndDataSource()
        {
            _filterAudioArtist = "";
            _filterAudioGenre = "";
            _filterAudioAlbum = "";
            _filterMovie = "";
            _filterTvShow = "";

            btn_Movies_Filter.Background = GetSkinImageBrushSmall("Remote_Search");
            btn_AudioGenre_Filter.Background = GetSkinImageBrushSmall("Remote_Search");
            btn_AudioAlbums_Filter.Background = GetSkinImageBrushSmall("Remote_Search");
            btn_AudioArtists_Filter.Background = GetSkinImageBrushSmall("Remote_Search");
            btn_TvShows_Filter.Background = GetSkinImageBrushSmall("Remote_Search");


            _audioSongsDataSource.Clear();
            _audioGenresDataSource.Clear();
            _audioArtistsDataSource.Clear();
            _moviesDataSource.Clear();
            _tvShowsDataSource.Clear();
            _tvSeasonsDataSource.Clear();
            _tvEpisodesDataSource.Clear();

            _videoFavoritesFilter = false;
            btn_Home_Video_FilterFavorites.Background = GetSkinImageBrush("Filter_Favorites");
            /*_moviesCollectionView.Filter = null;
            _tvShowsCollectionView.Filter = null;*/

            _audioFavoritesFilter = false;
            btn_Home_Music_FilterFavorites.Background = GetSkinImageBrush("Filter_Favorites");
            /*_audioGenresCollectionView.Filter = null;
            _audioAlbumsCollectionView.Filter = null;
            _audioArtistsCollectionView.Filter = null;*/
        }


        private void AudioStarting()
        {

            
            if (grd_Dimming.Visibility == Visibility.Visible)
            {
                grd_Dimming_MouseDown(null, null);
            }
            if (grd_Diaporama.Visibility == Visibility.Visible)
            {
                grd_Diaporama_MouseDown(null, null);
            }
            if (_config.Currently)
            {
                Logger.Instance().Log("FrontView+", "Music starting : Switch to currently");
                ShowGrid(grd_Currently);
                _isScreenSaver = true;
            }
            else
            {
                if (_config.Dimming && !_config.DimmingOnlyVideo)
                {
                    Logger.Instance().Log("FrontView+", "Video starting : start screen saver : Dimming");
                    var stbDimmingShow = (Storyboard)TryFindResource("stb_ShowDimming");
                    
                   
                    if (stbDimmingShow != null)
                    {
                        Logger.Instance().Trace("NewDim*", "Within Video Starting seeing ");
                        var animation = (DoubleAnimationUsingKeyFrames)stbDimmingShow.Children[0];
                        Logger.Instance().Trace("NewDim*", "Within Video Starting animation:Name ");
                        var keyframe1 = animation.KeyFrames[1];
                        Logger.Instance().Trace("NewDim*", "Within Video Starting keyframe1:Value " + keyframe1.Value);
                        keyframe1.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, _config.DimTime));
                        stbDimmingShow.Begin(this);
                    

                        _isScreenSaver = true;
                    }
                }
                else
                {
                    StartDiaporama();
                    _isScreenSaver = true;
                }

            }

        }

        private void VideoStarting()
        {
            if (grd_Dimming.Visibility == Visibility.Visible)
            {
                grd_Dimming_MouseDown(null, null);
            }
            if (grd_Diaporama.Visibility == Visibility.Visible)
            {
                grd_Diaporama_MouseDown(null, null);
            }
            if (_config.CurrentlyMovie)
            {
                Logger.Instance().Log("FrontView+", "Video starting : Switch to currently");
                ShowGrid(grd_Currently);
                _isScreenSaver = true;
            }
            else
            {
                if (_config.Dimming ) //&& _config.DimmingOnlyVideo)
                {
                    Logger.Instance().Log("FrontView+", "Video starting : start screen saver : Dimming");
                    var stbDimmingShow = (Storyboard)TryFindResource("stb_ShowDimming");




                    if (stbDimmingShow != null)
                    {
                        Logger.Instance().Trace("NewDim*", "Within Video Starting seeing ");
                        var animation = (DoubleAnimationUsingKeyFrames)stbDimmingShow.Children[0];
                        Logger.Instance().Trace("NewDim*", "Within Video Starting animation:Name ");
                        var keyframe1 = animation.KeyFrames[1];
                        Logger.Instance().Trace("NewDim*", "Within Video Starting keyframe1:Value " + keyframe1.Value);
                        keyframe1.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, _config.DimTime));
                        stbDimmingShow.Begin(this);
                        _isScreenSaver = true;
                    }
                }
                else
                {
                    StartDiaporama();
                    _isScreenSaver = true;
                }
            }
        }
        protected virtual bool IsFileLocked(string file)
        {
            FileStream stream = null;

            try
            {
                stream = new FileStream(file, FileMode.Open);
            }
            catch (IOException ex)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                Logger.Instance().LogDump("FileLOCK", "Locked and exception equals :" + ex, true);
                return true;
            }
            catch (Exception e)
            { 
            // other error of access
                Logger.Instance().LogDump("FileLOCK", "Another exception " + e, true);
            return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
        private void UpdateCurrently(Plugin.ApiCurrently nowPlaying )
        {
            _isPlaying = true;
            //SupportsRemoteControl Changes - Check Below:
            UpdateButtonVisibility();
            Logger.Instance().LogDump("Frontview PVR", "MediaType equals:" + nowPlaying.MediaType, true);
            
            
            switch (nowPlaying.MediaType)
            {
                case "Audio":
                    Logger.Instance().Log("FrontView+", "New Music Media : " + nowPlaying.FileName);
                    bool DefaultThumb = false;
                    bool usingExtrafanart = false;
                    // Add below to reset Thumb and Fanart when new file starts
                    //_yatse2Properties.Currently.Fanart = nowPlaying.FanartURL;
                        //Helper.SkinorDefault(Helper.SkinPath, _config.Skin, @"\Interface\Default_Diaporama.png");
                    _yatse2Properties.Currently.Thumb =  Helper.SkinorDefault(Helper.SkinPath, _config.Skin, @"\Interface\Default_Music-Thumbs.png");
                    _yatse2Properties.Currently.IsMusic = true;
                    _yatse2Properties.Currently.MusicAlbum = nowPlaying.Album;
                    _yatse2Properties.Currently.MusicSong = nowPlaying.Title;
                    _yatse2Properties.Currently.MusicArtist = nowPlaying.Artist;
                    _yatse2Properties.Currently.Logo = GetVideoThumbPath(nowPlaying.LogoURL);
                    Logger.Instance().LogDump("LogoUpdate [310 Y2W.Remote.cs]", "nowPlaying LogoURL equals:" + nowPlaying.LogoURL +" and Yatse2Properties.Currently.Logo equals:"+ _yatse2Properties.Currently.Logo, true);
                    _yatse2Properties.Currently.Fanart = _config.MusicFanartRotation ? GetRandomImagePath(Helper.CachePath + @"Music\Fanarts") : GetMusicFanartPath(nowPlaying.FanartURL);
                    _yatse2Properties.Currently.MovieIcons = nowPlaying.MovieIcons;

                    // Wholesale change coming up......  Move away from DB for Currently Info, instead move to extrafanart for the artist detected....
                    // Looks good - should do the same for Video Now Playing info screen
                    if (_remotePlugin.Name.Contains("Kodi"))
                    {
                        Logger.Instance().LogDump("UpdateAUDIO", "Config Rotation:Currently Fanart URL equals:" + nowPlaying.FanartURL, true);
                        Logger.Instance().LogDump("UpdateAUDIO", "Config GetMusicFanartPath equals:" + GetMusicFanartPath(nowPlaying.FanartURL), true);
                        Logger.Instance().LogDump("UpdateAUDIO", "*********** nowPlaying Artist Equals:" + nowPlaying.Artist + " and Currently MusicArtist:" + _yatse2Properties.Currently.MusicArtist, true);
                        Logger.Instance().LogDump("UpdateAUDIO", "*********** nowPlaying Album quals:" + nowPlaying.Album + " and Currently MusicArtist:" + _yatse2Properties.Currently.MusicAlbum, true);
                        Logger.Instance().LogDump("UpdateAUDIO", "*********** nowPlaying Song Equals:" + nowPlaying.Title + " and Currently MusicArtist:" + _yatse2Properties.Currently.MusicSong, true);

                        var testaudiofanart = KodiSourceData.KodiMusicSources[0] + nowPlaying.Artist + @"\extrafanart\";

                        // Change to checking first Kodi Audio source for extrafanart artist fanart - rather than all which time consuming

                        if (KodiSourceData.KodiMusicSources[0] != null || KodiSourceData.KodiMusicSources[0] != "")
                        {
                            var testaudiofanartcheck = KodiSourceData.KodiMusicSources[0] + @"\" + nowPlaying.Artist + @"\extrafanart\";
                            Logger.Instance().LogDump("UpdateAUDIO ARRAY", "Checking all sources " + testaudiofanartcheck);
                            if (System.IO.Directory.Exists(testaudiofanartcheck))
                            {
                                Logger.Instance().LogDump("UpdateAUDIO ARRAY", "Directory Exists Usings - No check for contents though " + testaudiofanartcheck);
                                testaudiofanart = testaudiofanartcheck;
                            }
                        }

                        /**
                                           foreach (var musicsource in KodiSourceData.KodiMusicSources)
                                           {
                                                    if (musicsource != null)
                                                    { 
                                                    var testaudiofanartcheck = musicsource + nowPlaying.Artist + @"\extrafanart\";
                                                    Logger.Instance().LogDump("UpdateAUDIO ARRAY", "Checking all sources " + testaudiofanartcheck);
                                                    if (System.IO.Directory.Exists(testaudiofanartcheck)) 
                                                        {
                                                         Logger.Instance().LogDump("UpdateAUDIO ARRAY", "Directory Exists Usings - No check for contents though " + testaudiofanartcheck);
                                                         testaudiofanart = testaudiofanartcheck;
                                                         break;
                                                        }
                                                    }    
                                               
                                            
                                            }
                        **/
                        string resultextrafanart = GetRandomImagePath(testaudiofanart);

                        //var testaudiofanart = KodiSourceData.KodiMusicSources[0] + nowPlaying.Artist + @"\extrafanart\";
                        Logger.Instance().LogDump("UpdateAUDIO", "testfanart equals:" + testaudiofanart, true);
                        Logger.Instance().LogDump("UpdateAUDIO", "GetRandomImagePath ==:" + resultextrafanart, true);

                        // check for extrafanart -- if no images found will not run and default should apply


                        if (!_config.MusicFanartRotation && !resultextrafanart.EndsWith("Default_Diaporama.png"))
                        {
                            Logger.Instance().LogDump("UpdateAUDIO", "Currently.Fanart set to testaudiofanart:", true);
                            var FanartPathFilename = resultextrafanart;
                            var FanartisLocked = true;
                            FanartisLocked = IsFileLocked(FanartPathFilename);
                            if (FanartisLocked == false)
                            {
                                Logger.Instance().LogDump("UpdateAUDIO", "Fanart File is not locked using " + FanartPathFilename, true);
                                _yatse2Properties.Currently.Fanart = FanartPathFilename;
                                usingExtrafanart = true;
                            }
                            if (FanartisLocked == true)
                            {
                                Logger.Instance().LogDump("UpdateAUDIO", "Fanart File is locked/using Default", true);
                                _yatse2Properties.Currently.Fanart = Helper.SkinorDefault(Helper.SkinPath, _config.Skin, @"\Interface\Default_Diaporama.png");
                                usingExtrafanart = false;
                            }

                        }

                    }

                    Logger.Instance().LogDump("UpdateAUDIO", "nowPlaying FanartURL:" + nowPlaying.FanartURL, true);
                    Logger.Instance().LogDump("UpdateAUDIO", "Currently Fanart equals:" + _yatse2Properties.Currently.Fanart, true);
                    _yatse2Properties.Currently.Thumb = GetMusicThumbPath(nowPlaying.ThumbURL); // TODO : Change to converter
                    Logger.Instance().LogDump("UpdateAUDIO", "Currently Thumb equals:" + _yatse2Properties.Currently.Thumb, true);
                    _yatse2Properties.Currently.MusicYear = nowPlaying.Year.ToString(CultureInfo.InvariantCulture);
                    _yatse2Properties.Currently.MusicTrack = nowPlaying.Track.ToString(CultureInfo.InvariantCulture);
                    _yatse2Properties.Currently.MusicGenre = nowPlaying.Genre;


                    var NowPlayingFile = SortOutPath(nowPlaying.FileName);

                    if (!NowPlayingFile.Contains("googleusercontent"))
                    {
                        try
                        {
                            var pathfilename = Path.GetDirectoryName(NowPlayingFile);
                            Logger.Instance().LogDump("UpdateAUDIO", "Thumbnail:  Nowplaying Get DirectoryName" + pathfilename, true);
                            if (File.Exists(pathfilename + @"\cdart.png"))
                            {
                                _yatse2Properties.Currently.Thumb = pathfilename + @"\cdart.png";
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Instance().LogDump("Thumb Exception", "Thumbnail Exception Caught" + ex, true);
                        }
                    }

                    //Change Thumb if using Google play - recognise googleusercontent in string
                    
                    try
                    {
                       
                        var pathfilename = NowPlayingFile;
                        if (pathfilename.Contains("googleusercontent"))
                        {
                            Logger.Instance().LogDump("UpdateAUDIO", "Thumb GooglePlay:  Update to Skin Thumb", true);
                          
                            if (_yatse2Properties.Currently.Thumb.EndsWith("Default_Music-Thumbs.png"))
                            {
                                 if (File.Exists(Helper.SkinorDefault(Helper.SkinPath , _config.Skin , @"\Interface\Default_Music-ThumbGoogle.png")) )
                                {
                                    _yatse2Properties.Currently.Thumb = Helper.SkinorDefault(Helper.SkinPath , _config.Skin , @"\Interface\Default_Music-ThumbGoogle.png");
                                    Logger.Instance().LogDump("UpdateAUDIO", "Thumb GooglePlay:  Changing to Google Thumb" + _yatse2Properties.Currently.Thumb, true);
                                }
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        Logger.Instance().LogDump("Thumb Exception", "Thumbnail Exception Caught" + ex, true);
                    }




                    var songinfo = _database.GetAudioSongFromFile(_remoteInfo.Id, nowPlaying.FileName);
                    if (songinfo.Count > 0)
                    {
                        var artistinfo = _database.GetAudioArtistFromName(_remoteInfo.Id, songinfo[0].Artist);
                        _yatse2Properties.Currently.MusicBiography = artistinfo.Count > 0 ? artistinfo[0].Biography : "No information";
                        if (!_config.MusicFanartRotation)
                            _yatse2Properties.Currently.Fanart = artistinfo.Count > 0 ? GetMusicFanartPath(artistinfo[0].Fanart) : "";
                        Logger.Instance().LogDump("UpdateAUDIO", "Config Rotation:Currently Fanart equals:" + _yatse2Properties.Currently.Fanart, true);
                    }

                    AudioStarting();

                    break;


                case "TvShow":
                    Logger.Instance().Log("FrontView+", "New TvShow Media : " + nowPlaying.FileName);
                    _yatse2Properties.Currently.IsTv = true;
                    _yatse2Properties.Currently.Thumb = GetVideoThumbPath(nowPlaying.ThumbURL); // TODO : Change to converter
                    _yatse2Properties.Currently.Logo = GetVideoLogoPath(nowPlaying.LogoURL);
                    Logger.Instance().LogDump("LogoUpdate.3", "nowPlaying LogoURL equals:" + nowPlaying.LogoURL + " and Yatse2Properties.Currently.Logo equals:" + _yatse2Properties.Currently.Logo, true);
                    var epinfo = _database.GetTvEpisodeFromFile(_remoteInfo.Id, nowPlaying.FileName);
                    if (epinfo.Count > 0)
                    {
                        _yatse2Properties.Currently.TvShow = epinfo[0].ShowTitle + " - " + epinfo[0].Title;
                        _yatse2Properties.Currently.TvTitle = epinfo[0].Title;
                        _yatse2Properties.Currently.TvAired = epinfo[0].Date;
                        _yatse2Properties.Currently.TvPlot = epinfo[0].Plot;
                        _yatse2Properties.Currently.TvEpisode = GetLocalizedString(77) + " " + epinfo[0].Season + " " + GetLocalizedString(78) + " " + epinfo[0].Episode;
                        _yatse2Properties.Currently.TvNote = epinfo[0].Rating;
                        var showinfo = _database.GetTvShowFromName(_remoteInfo.Id, epinfo[0].ShowTitle);
                        _yatse2Properties.Currently.Fanart = GetVideoFanartPath(showinfo.Count > 0 ? showinfo[0].Fanart : nowPlaying.FanartURL); // TODO : Change to Covnerter
                        _yatse2Properties.Currently.TvStudio = Helper.SkinorDefault(Helper.SkinPath , _yatse2Properties.Skin , @"\Studios\" + epinfo[0].Studio + ".png");
                        _yatse2Properties.Currently.TvDirector = epinfo[0].Director;
                        _yatse2Properties.Currently.TvYear = epinfo[0].Date.Length > 3 ? epinfo[0].Date.Substring(0, 4) : epinfo[0].Date;
                        _yatse2Properties.Currently.TvVotes = "";
                        _yatse2Properties.Currently.MovieIcons = nowPlaying.MovieIcons;
                    }
                    else
                    {
                        _yatse2Properties.Currently.TvShow = nowPlaying.ShowTitle + " - S" + nowPlaying.SeasonNumber + " E" + nowPlaying.EpisodeNumber;
                        _yatse2Properties.Currently.TvTitle = nowPlaying.Title;
                        _yatse2Properties.Currently.TvAired = nowPlaying.FirstAired.ToShortDateString();
                        _yatse2Properties.Currently.TvPlot = nowPlaying.Plot;
                        _yatse2Properties.Currently.TvEpisode = GetLocalizedString(77) + " " + nowPlaying.SeasonNumber + " " + GetLocalizedString(78) + " " + nowPlaying.EpisodeNumber;
                        _yatse2Properties.Currently.TvNote = nowPlaying.Rating;
                        _yatse2Properties.Currently.Fanart = GetVideoFanartPath(nowPlaying.FanartURL); // TODO : Change to Converter
                        _yatse2Properties.Currently.TvStudio = Helper.SkinorDefault(Helper.SkinPath , _yatse2Properties.Skin , @"\Studios\" + nowPlaying.Studio + ".png");
                        _yatse2Properties.Currently.TvDirector = nowPlaying.Director;
                        _yatse2Properties.Currently.TvYear = nowPlaying.Year.ToString(CultureInfo.InvariantCulture);
                        _yatse2Properties.Currently.TvVotes = "";
                        _yatse2Properties.Currently.MovieIcons = nowPlaying.MovieIcons;
                    }
                    VideoStarting();
                    break;
                
                case "Movie":
                    _yatse2Properties.Currently.Thumb = GetVideoThumbPath(nowPlaying.ThumbURL); // TODO : Change to converter
                    //_yatse2Properties.Currently.Logo = GetVideoThumbPath(nowPlaying.LogoURL);

                    Logger.Instance().LogDump("LogoUpdate.4", "nowPlaying LogoURL equals:" + nowPlaying.LogoURL + " and Yatse2Properties.Currently.Logo equals:" + _yatse2Properties.Currently.Logo, true);

                    Logger.Instance().Log("FrontView+", "New Movie Media : " + nowPlaying.FileName);
                    var movieinfo = _database.GetMovieFromFile(_remoteInfo.Id, nowPlaying.FileName);
                    _yatse2Properties.Currently.IsMovie = true;

                    Logger.Instance().LogDump("MovieIcons Currently", "nowPlaying MovieIcons equals:" + nowPlaying.MovieIcons +":", true);
                    _yatse2Properties.Currently.MovieIcons = nowPlaying.MovieIcons;
                    if (movieinfo.Count > 0)
                    {
                        _yatse2Properties.Currently.MovieTitle = movieinfo[0].Title;
                        _yatse2Properties.Currently.Fanart = GetVideoFanartPath(movieinfo[0].Fanart); // TODO : Change to Covnerter
                        if (movieinfo[0].Logo == null) 
                        {
                            _yatse2Properties.Currently.Logo = GetVideoLogoPath(movieinfo[0].Logo);
                        }
                        else if (movieinfo[0].Logo == "NONE")
                        {
                            _yatse2Properties.Currently.Logo = GetVideoLogoPath(nowPlaying.LogoURL);
                        }
                        else
                        {
                            _yatse2Properties.Currently.Logo = GetVideoLogoPath(movieinfo[0].Logo);
                        }
                        // = movieinfo[0].Logo == "NONE" : GetVideoLogoPath(movieinfo[0].Logo) ? GetVideoLogoPath(nowPlaying.LogoURL);
                        Logger.Instance().LogDump("LogoUpdate", "nowPlaying 232 LogoURL equals:" + movieinfo[0].Logo + " and Yatse2Properties.Currently.Logo equals:" + _yatse2Properties.Currently.Logo, true);
                        _yatse2Properties.Currently.MovieYear = movieinfo[0].Year.ToString(CultureInfo.InvariantCulture);
                        _yatse2Properties.Currently.MoviePlot = movieinfo[0].Plot;
                        _yatse2Properties.Currently.MovieDirector = movieinfo[0].Director;
                        _yatse2Properties.Currently.MovieNote = movieinfo[0].Rating;
                        _yatse2Properties.Currently.MovieVotes = movieinfo[0].Votes + " " + GetLocalizedString(82);
                        _yatse2Properties.Currently.MovieStudio = Helper.SkinPath + _yatse2Properties.Skin + @"\Studios\" + movieinfo[0].Studio + ".png";
                    }
                    else
                    {
                        _yatse2Properties.Currently.MovieTitle = nowPlaying.Title;
                        _yatse2Properties.Currently.Fanart = GetVideoFanartPath(nowPlaying.FanartURL); // TODO : Change to Converter
                        _yatse2Properties.Currently.Logo = GetVideoLogoPath(nowPlaying.LogoURL);
                        _yatse2Properties.Currently.MovieYear = nowPlaying.Year.ToString(CultureInfo.InvariantCulture);
                        Logger.Instance().LogDump("LogoUpdate", "nowPlaying LogoURL -123- equals:" + nowPlaying.LogoURL + " and Yatse2Properties.Currently.Logo equals:" + _yatse2Properties.Currently.Logo, true);
                        _yatse2Properties.Currently.MoviePlot = nowPlaying.Plot;
                        _yatse2Properties.Currently.MovieDirector = nowPlaying.Director;
                        _yatse2Properties.Currently.MovieNote = nowPlaying.Rating;
                        _yatse2Properties.Currently.MovieStudio = Helper.SkinPath + _yatse2Properties.Skin + @"\Studios\" + nowPlaying.Studio + ".png";
                    }

                    VideoStarting();
                    break;
                
                case "Unknown":
                    _yatse2Properties.Currently.IsUnknown = true;
                    _yatse2Properties.Currently.Thumb = GetVideoThumbPath(nowPlaying.ThumbURL); // TODO : Change to converter
                    _yatse2Properties.Currently.UnknownFile = nowPlaying.FileName;
                    _yatse2Properties.Currently.Fanart = GetVideoFanartPath(nowPlaying.FanartURL); // TODO : Change to Covnerter
                    _yatse2Properties.Currently.Logo = GetVideoLogoPath(nowPlaying.LogoURL);
                    _yatse2Properties.Currently.MovieIcons = nowPlaying.MovieIcons;
                    VideoStarting();
                    break;
                //Attempt PVR support
                
                case "Pvr":
                    Logger.Instance().LogDump("FrontView PVR:", "Case is PVR: ", true);

                    _yatse2Properties.Currently.Thumb = GetVideoThumbPath(nowPlaying.ThumbURL);
                    Logger.Instance().LogDump("FrontView PVR:", "Thumb Set to " + _yatse2Properties.Currently.Thumb, true);
                    _yatse2Properties.Currently.Logo = GetVideoLogoPath(nowPlaying.LogoURL); 
                    _yatse2Properties.Currently.IsMovie = true;
                    _yatse2Properties.Currently.MovieTitle = nowPlaying.Title;
                    nowPlaying.FileName = nowPlaying.Title;
                    _yatse2Properties.Currently.MovieIcons = nowPlaying.MovieIcons;
                    Logger.Instance().LogDump("FrontView PVR:", "nowPlaying Title: " + _yatse2Properties.Currently.MovieTitle, true);


                    if (_yatse2Properties.Currently.Thumb != "" && nowPlaying.FanartURL == "")
                    // if thumbnail not blank  and fanart is blank
                    {
                        _yatse2Properties.Currently.Fanart = _yatse2Properties.Currently.Thumb;
                        Logger.Instance().LogDump("Frontview PVR","nowPlaying_fanart Modified to Thumb:" + _yatse2Properties.Currently.Fanart, true);
                        Logger.Instance().LogDump("Frontview PVR","nowPlaying_fanart Prior Fanart Received was::" + nowPlaying.FanartURL, true);
                    }
                    else
                    {
                        _yatse2Properties.Currently.Fanart = GetVideoThumbPath(nowPlaying.FanartURL);
                        Logger.Instance().LogDump("Frontview PVR","nowPlaying_fanart Not Modified:" + nowPlaying.FanartURL, true);
                    }

                   // _yatse2Properties.Currently.Fanart = GetVideoThumbPath(nowPlaying.ThumbURL);
                    Logger.Instance().LogDump("FrontView PVR", "Fanart is : " + _yatse2Properties.Currently.Fanart, true);


                    VideoStarting();
                    break;

                default:
                    break;
            }
        }

        private void ResfreshCurrently()
        {
            if (_remoteInfo == null || !_remoteConnected)
                return;
            
            var nowPlaying = _remote.Player.NowPlaying(true);

            Logger.Instance().LogDump("FrontView ResfreshCurrently", "nowPlaying.Filename is : " + nowPlaying.FileName +" nowPlaying.IsPlaying = " +nowPlaying.IsPlaying + " isPlaying " + _isPlaying + " isNewMedia:" +nowPlaying.IsNewMedia , true);

            if (nowPlaying.IsNewMedia && (nowPlaying.IsPlaying || nowPlaying.IsPaused) && !String.IsNullOrEmpty(nowPlaying.FileName) && !nowPlaying.FileName.EndsWith("theme.mp3"))
            {
                //Glenn Added
                Window glennwindow = Window.GetWindow(this);
                if (glennwindow.WindowState != WindowState.Normal)
                {
                    Logger.Instance().Log("FrontView 2 NEW DEBUG", "Window State Miniminsed Restore as Playing " + nowPlaying.FileName);
                    //glennwindow.Visibility = Visibility.Visible;
                    //Restore with no activation or focus
                    RestoreWindowNoActivateExtension.RestoreNoActivate(glennwindow);
                    glennwindow.Show();
                    //glennwindow.WindowState = WindowState.Normal;
                    //Show();
                }
                UpdateCurrently(nowPlaying);
            }
            //////////////////////////////////////////////////////////////////////////
            // Further Check to avoid very occasional Currently Screen not showing  //
            //////////////////////////////////////////////////////////////////////////


            if ( _currentGrid != grd_Currently && 
               (nowPlaying.IsPlaying || nowPlaying.IsPaused)  
                && !String.IsNullOrEmpty(nowPlaying.FileName )
                && !nowPlaying.FileName.EndsWith("theme.mp3") 
                && ( (nowPlaying.MediaType=="Audio" && _config.Currently == true) 
                || ( nowPlaying.MediaType!="Audio" && _config.CurrentlyMovie == true )
                ))
            {
                Logger.Instance().Log("Currently Check","New Check Currently Screen Called...");
                UpdateCurrently(nowPlaying);
            }




            if ((nowPlaying.IsPlaying || nowPlaying.IsPaused))
            {
                btn_Header_Remotes.Background = GetSkinImageBrush("Menu_Remote_Connected_Playing");
                _yatse2Properties.Currently.Progress = nowPlaying.Progress;            
                _yatse2Properties.Currently.Time = nowPlaying.Time.ToString();
                _yatse2Properties.Currently.Duration = nowPlaying.Duration.ToString();

                _yatse2Properties.Currently.RemainingTime = (nowPlaying.Duration - nowPlaying.Time).ToString();
                TimeSpan remainingTime = nowPlaying.Duration - nowPlaying.Time;
                DateTime currentTime = DateTime.Now;
                DateTime finishTime = currentTime + remainingTime;
                _yatse2Properties.Currently.FinishTime = finishTime.ToString("h:mm tt");

                _yatse2Properties.Currently.IsPlaying = nowPlaying.IsPlaying;
                _yatse2Properties.Currently.IsPaused = nowPlaying.IsPaused;
                if (_config.UseReceiverIPforVolume == false)
                {
                  _yatse2Properties.Currently.Volume = nowPlaying.Volume;
                  _yatse2Properties.Currently.IsMuted = nowPlaying.IsMuted;
                }

                if (_config.UseReceiverIPforVolume == true)
                {
                    _yatse2Properties.Currently.Volume = receiver.WhatisVolume();
                    bool isMutedReceiver = receiver.WhatisMute();
                    _yatse2Properties.Currently.IsMuted = isMutedReceiver;
                    nowPlaying.IsMuted = isMutedReceiver;
                }
                                
                if (nowPlaying.IsPlaying)
                {
                    if (_config.MusicFanartRotation)
                        if (nowPlaying.MediaType == "Audio" && _timerHeader == 1)
                        {
                            _yatse2Properties.Currently.Fanart = GetRandomImagePath(Helper.CachePath + @"Music\Fanarts");
                        }
                }
            }
            else
            {
                if (_isPlaying)
                {
                    _isPlaying = false;
                    if (_remote.IsConnected())
                        btn_Header_Remotes.Background = GetSkinImageBrush("Menu_Remote_Connected");
                    else
                        btn_Header_Remotes.Background = GetSkinImageBrush("Menu_Remote_Disconnected");
                    if (_config.GoHomeOnEndPlayback)
                        ShowHome();
                    else
                    {
                        if (_currentGrid == grd_Currently)
                        {
                            Logger.Instance().Log("Debug", "Nothing playing go Back");
                            GoBack();
                        }
                    }
                }
                _yatse2Properties.Currently.IsNothing = true;
                if ((_config.Currently || _config.CurrentlyMovie) && ((grd_Dimming.Visibility != Visibility.Visible) && (grd_Diaporama.Visibility != Visibility.Visible)))
                   _isScreenSaver = false;
                   //_isfanart = false;
            }
        }

        private void updateCacheSizes()
        {
            //Set Coverflow size once at beginning based on Random Coverart
            // or fix to 574x800 equivalent size if using CoverART Kodi
            double cachewidth = 110;
            if (_config.CoverArt == true)
            {
                _yatse2Properties.MovieCacheHeight = 154;

            }
            else
            {
                _yatse2Properties.MovieCacheHeight = getMovieCacheImageHeight(cachewidth);
            }

            _yatse2Properties.MovieCacheWidth = cachewidth+1;

            _yatse2Properties.TVCacheHeight = getTVCacheImageHeight(cachewidth);
            _yatse2Properties.TVCacheWidth = cachewidth+1;
        }


        private void UpdateRemote()
        {
            if (_remote == null)
            {
                
                RunningServerThread = false;
                return;
            }
            if (!_remoteConnected && _remote.IsConnected() && !_failedRemoteCheck)
            {
                Logger.Instance().Log("FrontView+", "Remote connected : " + _remoteInfo.Id + " - " + _remoteInfo.Name + " (" + _remoteInfo.Api + " / " + _remoteInfo.Version + ")");
                var check = _remote.CheckRemote(_remoteInfo.OS, _remoteInfo.Version, _remoteInfo.Additional, _config.ForceOnCheckRemote);
                if (!check)
                {
                    Logger.Instance().Log("FrontView+", "Remote " + _remoteInfo.Id + " - " + _remoteInfo.Name + " : Failed check, some params have changed" + " (" + _remoteInfo.Api + " / " + _remoteInfo.Version + ")");
                    ShowOkDialog(GetLocalizedString(105));
                    _failedRemoteCheck = true;
                    if (_currentGrid != grd_Remotes)
                        ShowGrid(grd_Remotes, false);
                    return;
                }
                if (_currentGrid == grd_Remotes)
                {
                    ShowGrid(grd_Home);
                }
                ShowPopup(GetLocalizedString(96) + " " + _remoteInfo.Name);
                btn_Header_Remotes.Background = GetSkinImageBrush("Menu_Remote_Connected");
                _remoteConnected = true;

                //Add Startup Connection QUick Refresh & Kodi Source Data change
                if (_remoteInfo.Additional != "")
                {
                    UpdateKodiSource(_remoteInfo.Additional);
                }
                if (_config.QuickRefreshEnable == true)
                {
                    QuickRefreshLibrary();
                }


                /////////////////////////////////////////////////////////////////////////////////////
                // Start Fanart Server with Remote Connection and use Remote Parameters/Ip Address //
                // Ignore the Config File Data for IP use it for Port                             
                // Only Runs with first connection of Remote Control/Kodi                          //
                ////////////////////////////////////////////////////////////////////////////////////
                if (_config.StartFrontViewServer )               
                {
                    Logger.Instance().LogDump("Fanart-Server", "Starting/Checking Server:" + _remote.IP);
                    StartServer(_remote.IP);
                }



                updateCacheSizes();

                if (!_remoteLibraryRefreshed)
                {
                    RefreshLibrary();
                }
                else
                {
                    if (_remoteInfo.CacheFilled == 0 && _remote.File.AsyncDownloadFinished() )
                    {
                        RefreshThumbsFanarts();
                        ShowPopup(GetLocalizedString(101));
                    }
                }
            }

            if (!_remoteConnected && !_showRemoteSelect && _config.IsConfigured && _config.DefaultRemote != 0 && !_config.DisableRemoteCheck)
            {
                if (_timer > 20)
                {
                    _showRemoteSelect = true;
                    ShowGrid(grd_Remotes);
                   
                }
            }

            if (_remoteInfo != null)
            {
                if (_remote.File.AsyncDownloadFinished() && _remoteInfo.CacheFilled == 0 && _remote.IsConnected())
                {
                    Logger.Instance().Log("FrontView+", "Image cache filling completed");
                    _remoteInfo.CacheFilled = 1;
                    _yatse2Properties.IsSyncing = false;
                    _database.UpdateRemote(_remoteInfo);
                    ShowPopup(GetLocalizedString(102));
                    //ShowHome();
                    _audioSongsDataSource.Clear();
                    _audioGenresDataSource.Clear();
                    _audioArtistsDataSource.Clear();
                    _moviesDataSource.Clear();
                    _tvShowsDataSource.Clear();
                    _tvSeasonsDataSource.Clear();
                    _tvEpisodesDataSource.Clear();
                    updateCacheSizes();
                }
            }

            if (_remoteConnected && !_remote.IsConnected())
            {
                if (_remoteInfo != null)
                {
                    Logger.Instance().Log("FrontView+",
                                          "Remote disconnected : " + _remoteInfo.Id + " - " + _remoteInfo.Name + " (" +
                                          _remoteInfo.Api + " / " + _remoteInfo.Version + ")");
                    ShowPopup(GetLocalizedString(97) + " " + _remoteInfo.Name);
                    _remote.File.StopAsync();
                    _yatse2Properties.IsSyncing = false;
                    RunningServerThread = false;  // Remote disconnected kill server thread
                }
                btn_Header_Remotes.Background = GetSkinImageBrush("Menu_Remote_Disconnected");
                _remoteConnected = false;
                _isPlaying = false;
                ShowHome();
                _yatse2Properties.Currently.IsNothing = true;
            }
            else
            {
                ResfreshCurrently();
            }

        }

        private void RefreshRemotes()
        {
            var remotes = _database.SelectAllRemote();
            lst_Remotes.Items.Clear();

            foreach (var remote in remotes)
            {
                remote.IsDefault = _config.DefaultRemote == remote.Id ? 1 : 0;

                remote.IsSelected = _currentRemoteId == remote.Id ? 1 : 0;
                
                var index = lst_Remotes.Items.Add(remote);

                if (_currentRemoteId == remote.Id)
                {
                    lst_Remotes.SelectedIndex = index;
                }

            }

            if (remotes.Count > 0)
            {
                if (lst_Remotes.SelectedIndex == -1)
                    lst_Remotes.SelectedIndex = 0;
                brd_Remotes_Actions.Visibility = Visibility.Visible;
                brd_Remotes_NoActions.Visibility = Visibility.Hidden;
            }
            else
            {
                brd_Remotes_Actions.Visibility = Visibility.Hidden;
                brd_Remotes_NoActions.Visibility = Visibility.Visible;
            }
        }
    }
}