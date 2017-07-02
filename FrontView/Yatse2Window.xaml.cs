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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using FrontView.Libs;
using System.Windows.Interop;
using System.Windows.Media;

namespace FrontView
{


    public partial class Yatse2Window
    {

        public Yatse2Window()
        {
            SetDPIState();

            InitializeComponent();

            

            ModalDialog.SetParent(grd_Contener);
            Init();
            ModalDialog.SetButtons(GetLocalizedString(125), GetLocalizedString(126), GetLocalizedString(127));
        }

        private void ShowPopup(string text)
        {
            _yatse2Properties.Popup = text;
            var stbDiaporamaHide = (Storyboard)TryFindResource("stb_Header_ShowPopup");
            if (stbDiaporamaHide != null)
                stbDiaporamaHide.Begin(this);
        }


        private void grd_Diaporama_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _isScreenSaver = false;
            _isfanart = false;
            ResetTimer();
            _diaporamaCurrentImage = 0;
            _fanartCurrentImage = 0;
            var stbDiaporamaHide = (Storyboard)TryFindResource("stb_HideDiaporama");
            if (stbDiaporamaHide != null)
            {
                stbDiaporamaHide.Begin(this);
                SetBrightnessContrast(true);
            }
        }

        private void grd_Dimming_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _isScreenSaver = false;
            _isfanart = false;
            ResetTimer();
            var stbDimmingHide = (Storyboard)TryFindResource("stb_HideDimming");
            if (stbDimmingHide != null)
            {
                stbDimmingHide.Begin(this);
                SetBrightnessContrast(true);
            }
        }

        private void rct_Header_Weather_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RefreshWeather();
        	ShowGrid(grd_Weather);
        }

        private void btn_Header_Remotes_Click(object sender, RoutedEventArgs e)
        {
            if (_remote == null)
                return;
            var nowPlaying = _remote.Player.NowPlaying(false);
            if (_remoteConnected && (nowPlaying.IsPlaying || nowPlaying.IsPaused) && _currentGrid != grd_Currently)
            {
                ShowGrid(grd_Currently);
                return;
            }

            if (_remoteConnected && (grd_Remote.Visibility != Visibility.Visible))
            {
                grd_Remote.Visibility = Visibility.Visible;
            }
            else
                ShowGrid(grd_Remotes);
        }


        private void img_Header_Home_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if( (grd_Settings_Remotes_Edit.Visibility != Visibility.Visible) 
                && (grd_Settings_Weather.Visibility != Visibility.Visible) 
                && (grd_Remote.Visibility != Visibility.Visible)
                && (grd_Filter.Visibility != Visibility.Visible))
            {
                GoBack();
            }
        }


        private void FanartOpacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            {
                // ... Get Slider reference.
                var slider = sender as Slider;
                // ... Get Value.
                double value = slider.Value;
                // ... Set Window Title.

                _config.FanArtOpacity = value;
                _yatse2Properties.FanArtOpacity = value;

            }
        }


        private void DDCBrightness_MouseDown(object sender, EventArgs e)
        {
            
            if (_config.TurnOffDDCControl == true)
            {
                   // Turn Monitor Off Completely
                SetMonitorState(2);
                return;
            }

      
            try
            {
                Window window = Window.GetWindow(this);
                var wih = new WindowInteropHelper(window);
                IntPtr hWnd = wih.Handle;
                brightnessControl = new FrontView.Libs.DDCControl.BrightnessControl(hWnd);

                // AGain Change just gets levels at startup.
                //brightnessInfo = brightnessControl.GetBrightnessCapabilities(0);
                //contrastInfo = brightnessControl.GetContrastCapabilities(0);

                if (brightnessControl != null && brightnessInfo.current != -1)
                {
                    brightnessControl.SetBrightness((short)brightnessInfo.minimum, 0);
                    brightnessControl.SetContrast((short)brightnessInfo.minimum, 0);
                }
            }
            catch (Exception )
            {
              
            }
        }

        private void DDCBrightness_MouseUp(object sender, EventArgs e)
        {

            Setup.Logger.Instance().LogDump("DDCControl", "DDCBrightness Mouse Up Called");
            if (_config.TurnOffDDCControl == true)
            {
                
                SetMonitorState(-1);
                SetMonitorWake();
                return;

            }


            try
            {

                Window window = Window.GetWindow(this);
                var wih = new WindowInteropHelper(window);
                IntPtr hWnd = wih.Handle;
                brightnessControl = new FrontView.Libs.DDCControl.BrightnessControl(hWnd);



                if (brightnessControl != null && brightnessInfo.current != -1)
                {
                    brightnessControl.SetBrightness((short)brightnessInfo.current, 0);
                    brightnessControl.SetContrast((short)brightnessInfo.current, 0);
                }
            }
            catch (Exception )
            {

            }
        }
        private void TimeSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            {
                // ... Get Slider reference.
                var slider = sender as Slider;
                // ... Get Value.
                double value = Math.Round(slider.Value, 0);
                // ... Set Window Title.

                _config.TimeSize = value;
                _yatse2Properties.TimeSize = value;

            }
        }


        private void LogoSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            {
                // ... Get Slider reference.
                var slider = sender as Slider;
                // ... Get Value.
                double value = Math.Round(slider.Value,0);
                // ... Set Window Title.

                _config.LogoSize = value;
                _yatse2Properties.LogoSize = value;

            }
        }

        private void SemiCircleOpacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            {
                // ... Get Slider reference.
                var slider = sender as Slider;
                // ... Get Value.
                double value = slider.Value;
                // ... Set Window Title.

                _config.SemiCircleOpacity = value;
                _yatse2Properties.SemiCircleOpacity = value;

            }
        }



        private void Window_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
        	//ResetTimer();
            //e.Handled = false;
        }

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ResetTimer();
            
            e.Handled = false;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        	if (_config.MouseMode && !_config.DisableScreenPositioning)
			    DragMove();
        }

        private void btn_Filter_Close_Click(object sender, RoutedEventArgs e)
        {
            grd_Filter.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _yatse2Properties.CollectionFilter = _yatse2Properties.CollectionFilter + ((Button) sender).Content;
        }

        private void btn_Movies_Filter_Click(object sender, RoutedEventArgs e)
        {
            switch (_currentGrid.Name)
            {
                case "grd_Movies":
                    _yatse2Properties.CollectionFilter = _filterMovie;
                    break;
                case "grd_AudioGenres":
                    _yatse2Properties.CollectionFilter = _filterAudioGenre;
                    break;
                case "grd_AudioAlbums":
                    _yatse2Properties.CollectionFilter = _filterAudioAlbum;
                    break;
                case "grd_AudioArtists":
                    _yatse2Properties.CollectionFilter = _filterAudioArtist;
                    break;
                case "grd_TvShows":
                    _yatse2Properties.CollectionFilter = _filterTvShow;
                    break;
            }

            grd_Filter.Visibility = Visibility.Visible;
        }

        private void btn_Filter_Clear_click(object sender, RoutedEventArgs e)
        {
            _yatse2Properties.CollectionFilter = "";
        }

        private void btn_Filter_Back_click(object sender, RoutedEventArgs e)
        {
            if (_yatse2Properties.CollectionFilter.Length > 0)
                _yatse2Properties.CollectionFilter = _yatse2Properties.CollectionFilter.Substring(0, _yatse2Properties.CollectionFilter.Length - 1);
        }

        private void btn_Filter_Go_click(object sender, RoutedEventArgs e)
        {
            if (_yatse2Properties.CollectionFilter.Length > 0)
            {
                var letter = _yatse2Properties.CollectionFilter[0];
                switch (_currentGrid.Name)
                {
                    case "grd_Movies":
                        MovieSelectNextLetter((char)(letter-1));
                        grd_Filter.Visibility = Visibility.Hidden;
                        break;
                    case "grd_AudioGenres":
                        AudioGenreSelectNextLetter((char)(letter-1));
                        grd_Filter.Visibility = Visibility.Hidden;
                        break;
                    case "grd_AudioAlbums":
                        AudioAlbumSelectNextLetter((char)(letter-1));
                        grd_Filter.Visibility = Visibility.Hidden;
                        break;
                    case "grd_AudioArtists":
                       AudioArtistSelectNextLetter((char)(letter-1));
                       grd_Filter.Visibility = Visibility.Hidden;
                       break;
                    case "grd_TvShows":
                        TvShowSelectNextLetter((char)(letter-1));
                        grd_Filter.Visibility = Visibility.Hidden;
                        break;
                }
            }
        }

        private void btn_Filter_OK_click(object sender, RoutedEventArgs e)
        {

            switch (_currentGrid.Name)
            {
                case "grd_Movies":
                    _filterMovie = _yatse2Properties.CollectionFilter;
                    _moviesCollectionView.Filter = new Predicate<object>(FilterMovies);
                    btn_Movies_Filter.Background = ( ! String.IsNullOrEmpty(_filterMovie)) ? GetSkinImageBrushSmall("Remote_FilterOn") : GetSkinImageBrushSmall("Remote_Search");
                    Helper.VirtFlowSelect(lst_Movies_flow, 0);
                    break;
                case "grd_AudioGenres":
                    _filterAudioGenre = _yatse2Properties.CollectionFilter;
                    btn_AudioGenre_Filter.Background = (! String.IsNullOrEmpty(_filterAudioGenre)) ? GetSkinImageBrushSmall("Remote_FilterOn") : GetSkinImageBrushSmall("Remote_Search");
                    _audioGenresCollectionView.Refresh();
                    Helper.VirtFlowSelect(lst_AudioGenres_flow, 0);
                    break;
                case "grd_AudioAlbums":
                    _filterAudioAlbum = _yatse2Properties.CollectionFilter;
                    btn_AudioAlbums_Filter.Background = (! String.IsNullOrEmpty(_filterAudioAlbum)) ? GetSkinImageBrushSmall("Remote_FilterOn") : GetSkinImageBrushSmall("Remote_Search");
                    _audioAlbumsCollectionView.Refresh();
                    Helper.VirtFlowSelect(lst_AudioAlbums_flow, 0);
                    break;
                case "grd_AudioArtists":
                    _filterAudioArtist = _yatse2Properties.CollectionFilter;
                    btn_AudioArtists_Filter.Background = (!String.IsNullOrEmpty(_filterAudioArtist)) ? GetSkinImageBrushSmall("Remote_FilterOn") : GetSkinImageBrushSmall("Remote_Search");
                    _audioArtistsCollectionView.Refresh();
                    Helper.VirtFlowSelect(lst_AudioArtists_flow, 0);
                    break;
                case "grd_TvShows":
                    _filterTvShow = _yatse2Properties.CollectionFilter;
                    btn_TvShows_Filter.Background = (!String.IsNullOrEmpty(_filterTvShow)) ? GetSkinImageBrushSmall("Remote_FilterOn") : GetSkinImageBrushSmall("Remote_Search");
                    _tvShowsCollectionView.Refresh();
                    Helper.VirtFlowSelect(lst_TvShows_flow, 0);
                    break;
            }

            grd_Filter.Visibility = Visibility.Hidden;

        }

        private void txb_Header_Time_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowGrid(grd_Home);
        }

        private void btn_PlayMenu_Close_Click(object sender,RoutedEventArgs e)
        {
            grd_PlayMenu.Visibility = Visibility.Hidden;
        }

        private void btn_PlayMenu_Play_click(object sender, RoutedEventArgs e)
        {
        	if (grd_AudioGenres.Visibility == Visibility.Visible)
                GenresPlay(0);
            if (grd_AudioAlbums.Visibility == Visibility.Visible)
                AlbumsPlay(0);
            if (grd_AudioArtists.Visibility == Visibility.Visible)
                ArtistsPlay(0);

            grd_PlayMenu.Visibility = Visibility.Hidden;
        }

        private void btn_PlayMenu_PlayRandom_click(object sender, RoutedEventArgs e)
        {
            if (grd_AudioGenres.Visibility == Visibility.Visible)
                GenresPlay(1);
            if (grd_AudioAlbums.Visibility == Visibility.Visible)
                AlbumsPlay(1);
            if (grd_AudioArtists.Visibility == Visibility.Visible)
                ArtistsPlay(1);

            grd_PlayMenu.Visibility = Visibility.Hidden;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}
