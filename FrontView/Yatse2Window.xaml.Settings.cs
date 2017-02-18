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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.Win32;
using Setup;
using FrontView.Libs;

namespace FrontView
{
    public partial class Yatse2Window
    {
        private static void Donate()
        {
            const string url = @"https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=ghawken%40hotkey%2enet%2eau&lc=AU&item_name=FrontView&no_note=0&currency_code=USD&bn=PP%2dDonationsBF%3abtn_donate_LG%2egif%3aNonHostedGuest";
            Process.Start(new ProcessStartInfo(url));
        }

        private static void WebSite()
        {
            const string url = @"https://github.com/Ghawken/FrontView/releases";
            Process.Start(new ProcessStartInfo(url));
        }

        private void btn_Settings_CheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            CheckUpdate(true);
        }

        private void Image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Donate();
        }

        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            WebSite();
        }

        private void btn_Settings_SelectImagesDirectory_Click(object sender, RoutedEventArgs e)
        {
            ResetTimer();
            using (var dialog = new FolderBrowserDialog() )
            {
                dialog.SelectedPath = txt_Settings_ImagesDirectory.Text;
                var result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    txt_Settings_ImagesDirectory.Text = dialog.SelectedPath;
                }
           }
        }

        private void btn_Home_Settings_Click(object sender, RoutedEventArgs e)
        {
            if (_currentGrid == grd_Settings) return;
            // chk_Settings_SecondScreen.IsChecked = _config.SecondScreen;
            chk_Settings_Topmost.IsChecked = _config.Topmost;
            chk_Settings_KeepFocus.IsChecked = _config.KeepFocus;
            //chk_Settings_FanartAlways.IsChecked = _config.FanartAlways;
            //chk_Settings_MinimiseAlways.IsChecked = _config.MinimiseAlways;
            chk_Settings_ForceResolution.IsChecked = _config.ForceResolution;
            chk_Settings_Diaporama.IsChecked = _config.Diaporama;
            chk_Settings_Dimming.IsChecked = _config.Dimming;
            
            chk_Settings_Currently.IsChecked = _config.Currently;
            chk_Settings_CurrentlyMovie.IsChecked = _config.CurrentlyMovie;
            chk_Settings_HideCursor.IsChecked = _config.HideCursor;
            chk_Settings_UseLogo.IsChecked = _config.UseLogo;

            chk_Settings_UseNowPlayingMediaIcons.IsChecked = _config.UseNowPlayingMediaIcons;

            chk_Settings_UseBanner.IsChecked = _config.UseBanner;
            chk_Settings_ShowOverlay.IsChecked = _config.ShowOverlay;
            chk_Settings_ShowEmptyMusicGenre.IsChecked = _config.ShowEmptyMusicGenre;
            chk_Settings_ShowEndTime.IsChecked = _config.ShowEndTime;

            chk_Settings_DisableAnimations.IsChecked = _config.DisableAnimations;
            chk_Settings_Debug.IsChecked = _config.Debug;
            chk_Settings_HideWatched.IsChecked = _config.HideWatched;
            chk_Settings_RefreshOnConnect.IsChecked = _config.RefreshOnConnect;

            chk_Settings_QuickRefreshEnable.IsChecked = _config.QuickRefreshEnable;
            chk_Settings_ShowAudioMenu.IsChecked = _config.ShowAudioMenu;


            //chk_Settings_AnimatedMusicCover.IsChecked = _config.AnimatedMusicCover;
            chk_Settings_DimmingOnlyVideo.IsChecked = _config.DimmingOnlyVideo;
            chk_Settings_CheckUpdate.IsChecked = _config.FanartAlways;

            chk_Settings_HideCompilationArtists.IsChecked = _config.HideCompilationArtists;
            chk_Settings_GenreToArtists.IsChecked = _config.GenreToArtists;
            chk_Settings_MusicFanartRotation.IsChecked = _config.MusicFanartRotation;


            txt_Settings_ImagesDirectory.Text = _config.ImageDirectory;


            txt_Settings_WeatherLocId.Text = _config.WeatherLoc;

            txt_Settings_ReceiverIP.Text = _config.ReceiverIP;
            txt_Settings_ReceiverPort.Text = _config.ReceiverPort.ToString();
            txt_Settings_HttpUser.Text = _config.HttpUser;
            txt_Settings_HttpPassword.Text = _config.HttpPassword;
            txt_Settings_HttpPlaystarted.Text = _config.HttpPlaystarted;
            txt_Settings_HttpPlaypaused.Text = _config.HttpPlaypaused;
            txt_Settings_HttpPlaystopped.Text = _config.HttpPlaystopped;
            txt_Settings_HttpMute.Text = _config.HttpMute;
            txt_Settings_HttpUnmute.Text = _config.HttpUnmute;
            txt_Settings_HttpMediatypeAudio.Text = _config.HttpMediatypeAudio;
            txt_Settings_HttpMediatypeVideo.Text = _config.HttpMediatypeVideo;
            txt_Settings_HttpPoweron.Text = _config.HttpPoweron;
            txt_Settings_HttpPoweroff.Text = _config.HttpPoweroff;
            txt_Settings_WeatherAPI.Text = _config.WeatherAPI;
            txt_Settings_HttpPlayStartedDelayed.Text = _config.HttpPlayStartedDelayed;
            txt_Settings_HttpPlayStartedDelay.Text = _config.HttpPlayStartedDelay.ToString();
            


            var assem = Assembly.GetEntryAssembly();
            var assemName = assem.GetName();
            var ver = assemName.Version;

            txt_Settings_VersionNo.Text =  "Version "+ ver.Major +"." + ver.Minor + " Build:" + ver.Build;

            /**
            if (UpdateAvailable == true)
            {
                txt_Settings_VersionNo.Foreground = System.Windows.Media.Brushes.White;
                txt_Settings_VersionNo.Text = "*** Update Available *** Click --->";
            }
    **/
            chk_Settings_MouseMode.IsChecked = _config.MouseMode;

            chk_Settings_DebugTrace.IsChecked = _config.DebugTrace;
            chk_Settings_CropCacheImage.IsChecked = _config.CropCacheImage;
            chk_Settings_IgnoreSortTokens.IsChecked = _config.IgnoreSortTokens;
            chk_Settings_StartWithWindows.IsChecked = _config.StartWithWindows;
            chk_Settings_StartFrontViewServer.IsChecked = _config.StartFrontViewServer;
            chk_Settings_MinimiseAlways.IsChecked = _config.MinimiseAlways;
            chk_Settings_CoverArt.IsChecked = _config.CoverArt;
            chk_Settings_HttpSend.IsChecked = _config.HttpSend;
            chk_Settings_UseReceiver.IsChecked = _config.UseReceiverIPforVolume;
            chk_Settings_HttpUseDigest.IsChecked = _config.HttpUseDigest;

            chk_Settings_UseDDCControl.IsChecked = _config.UseDDCControl;
            chk_Settings_DisableRemoteCheck.IsChecked = _config.DisableRemoteCheck;
            chk_Settings_TVOrderbyNewEpisodes.IsChecked = _config.TVOrderbyNewEpsiodes;

            LoadSettingsResolutions();

            lst_Settings_Skin.Items.Clear();
            var skins = Directory.GetDirectories(Helper.SkinPath);
            Logger.Instance().Log("FrontView+", "Detected skins : " + skins.Length);
            foreach (var skin in skins)
            {
                var skinname = skin.Replace(Helper.SkinPath, "");
                var index = lst_Settings_Skin.Items.Add(skinname);
                Logger.Instance().Log("FrontView+", "Adding skin : " + skinname );
                if (skinname == _config.Skin)
                {
                    lst_Settings_Skin.SelectedIndex = index;
                }
            }

            lst_Settings_HomePage.Items.Clear();
            var en = _yatse2Pages.GetEnumerator();
            while (en.MoveNext())
            {
                var index = lst_Settings_HomePage.Items.Add(en.Key.ToString());
                if (en.Key.ToString() == _config.Homepage)
                    lst_Settings_HomePage.SelectedIndex = index;
            }

            lst_Settings_Skin_extra.Items.Clear();
            lst_Settings_Skin_extra.Items.Add("V Large NowPlaying");
            lst_Settings_Skin_extra.Items.Add("Large NowPlaying");
            lst_Settings_Skin_extra.Items.Add("Small NowPlaying");
            lst_Settings_Skin_extra.SelectedItem = _config.Skin_Extra;

            lst_Settings_Skin_Extra_Text.Items.Clear();
            lst_Settings_Skin_Extra_Text.Items.Add("V Large");
            lst_Settings_Skin_Extra_Text.Items.Add("Large");
            lst_Settings_Skin_Extra_Text.Items.Add("Small");
            lst_Settings_Skin_Extra_Text.SelectedItem = _config.Skin_Extra_Text;

            lst_Settings_LogoSize.Value = _config.LogoSize;

            

            lst_Settings_MediaIconSize.Value = _config.MediaIconSize;

           // lst_Settings_Skin_Extra_Logo.Items.Clear();
           // lst_Settings_Skin_Extra_Logo.Items.Add("Large");
           // lst_Settings_Skin_Extra_Logo.Items.Add("Medium");
          //  lst_Settings_Skin_Extra_Logo.Items.Add("Small");
          //  lst_Settings_Skin_Extra_Logo.SelectedItem = _config.Skin_Extra_Logo;

            lst_Settings_DimAmount.Items.Clear();
            //lst_Settings_DimAmount.Items.CurrentItem = _config.DimAmount;
            lst_Settings_DimAmount.Items.Add(0.1);
            lst_Settings_DimAmount.Items.Add(0.2);
            lst_Settings_DimAmount.Items.Add(0.3);
            lst_Settings_DimAmount.Items.Add(0.4);
            lst_Settings_DimAmount.Items.Add(0.5);
            lst_Settings_DimAmount.Items.Add(0.6);
            lst_Settings_DimAmount.Items.Add(0.7);
            lst_Settings_DimAmount.Items.Add(0.8);
            lst_Settings_DimAmount.Items.Add(0.9);
            lst_Settings_DimAmount.Items.Add(1.0);

            lst_Settings_DimAmount.SelectedItem = _config.DimAmount;

            lst_Settings_DimTime.Items.Clear();
            lst_Settings_DimTime.Items.Add(1);
            lst_Settings_DimTime.Items.Add(2);
            lst_Settings_DimTime.Items.Add(3);
            lst_Settings_DimTime.Items.Add(4);
            lst_Settings_DimTime.Items.Add(5);
            lst_Settings_DimTime.Items.Add(6);
            lst_Settings_DimTime.Items.Add(7);
            lst_Settings_DimTime.Items.Add(8);
            lst_Settings_DimTime.Items.Add(9);
            lst_Settings_DimTime.Items.Add(10);
            lst_Settings_DimTime.Items.Add(11);
            lst_Settings_DimTime.Items.Add(12);
            lst_Settings_DimTime.Items.Add(13);
            lst_Settings_DimTime.Items.Add(14);
            lst_Settings_DimTime.Items.Add(15);
            lst_Settings_DimTime.Items.Add(16);
            lst_Settings_DimTime.Items.Add(17);
            lst_Settings_DimTime.Items.Add(18);
            lst_Settings_DimTime.Items.Add(19);
            lst_Settings_DimTime.Items.Add(20);

            lst_Settings_DimTime.SelectedItem = _config.DimTime;


            lst_Settings_DefaultPlay.Items.Clear();
            en = _yatse2PlayModes.GetEnumerator();
            while (en.MoveNext())
            {
                var index = lst_Settings_DefaultPlay.Items.Add(GetLocalizedString(Convert.ToInt32(en.Value.ToString())));
                if (Convert.ToInt32(en.Key.ToString()) == _config.DefaultPlayMode)
                    lst_Settings_DefaultPlay.SelectedIndex = index;
            }

            lst_Settings_WeatherUnit.Items.Clear();
            lst_Settings_WeatherUnit.Items.Add("°C");
            lst_Settings_WeatherUnit.Items.Add("°F");
            lst_Settings_WeatherUnit.SelectedIndex = _config.WeatherUnit == "c" ? 0 : 1;

            LoadSettingsLangs();

            ShowGrid(grd_Settings);
        }

        private void LoadSettingsResolutions()
        {
            var screens = Screen.AllScreens;

            lst_Settings_Displays.Items.Clear();

            lst_Settings_LogoSize.Value = _config.LogoSize;

            

            lst_Settings_MediaIconSize.Value = _config.MediaIconSize;

            lst_Settings_FanartOpacity.Value = _config.FanArtOpacity;
            lst_Settings_SemiCircleOpacity.Value = _config.SemiCircleOpacity;

            foreach (var scr in screens)
            {
                lst_Settings_Displays.Items.Add(scr.DeviceName);   
            }

            lst_Settings_Displays.SelectedItem = _config.SelectedDisplay;

            var modes = ScreenResolution.EnumModes(screens.Length == 1 ? 0 : 1);
            if (screens.Length == 1)
                Logger.Instance().Log("FrontView+", "Detected main screen resolutions : " + modes.Length);
            else
                Logger.Instance().Log("FrontView+", "Detected screen resolutions : " + modes.Length);

            Logger.Instance().TraceDump("FrontView+", modes);

            lst_Settings_Resolution.Items.Clear();

            foreach (var mode in modes.Where(mode => mode.DMBitsPerPel >= _config.MinDMBitsPerPel && mode.DMPelsWidth >= _config.MinDMPelsWidth))
            {
                var index = lst_Settings_Resolution.Items.Add(new ScreenRes(mode));
                Logger.Instance().Trace("FrontView+", "Detected resolution : " + lst_Settings_Resolution.Items[index]);
                if (mode == _config.Resolution)
                {
                    lst_Settings_Resolution.SelectedIndex = index;
                }
            }
            Logger.Instance().Log("FrontView+", "Added resolutions : " + lst_Settings_Resolution.Items.Count);
        }

        private void LoadSettingsLangs()
        {
            lst_Settings_Language.Items.Clear();
            var langs = Directory.GetFiles(Helper.LangPath, "*.xaml");
            Logger.Instance().Log("FrontView+", "Detected language files : " + langs.Length);
            var langVersion = new Regex(@"Version:(\d+)");
            foreach (var lang in langs)
            {
                var langname = lang.Replace(Helper.LangPath, "").Replace(".xaml", "");
                var index = lst_Settings_Language.Items.Add(langname);
                var data = File.ReadAllText(lang);
                var m = langVersion.Match(data);
                if (m.Success)
                    Logger.Instance().Log("FrontView+", "Adding lang : " + langname + " | Version : " + m.Groups[1]);
                else
                    Logger.Instance().Log("FrontView+", "Adding lang : " + langname + " | No version");

                if (langname == _config.Language)
                {
                    lst_Settings_Language.SelectedIndex = index;
                }
            }
        }

        private void GetSettingsVars()
        {

            _config.IsConfigured = true;
            try
            {
                // ReSharper disable PossibleInvalidOperationException
          //      _config.SecondScreen = (bool)chk_Settings_SecondScreen.IsChecked;
                _config.Topmost = (bool)chk_Settings_Topmost.IsChecked;
                _config.KeepFocus = (bool)chk_Settings_KeepFocus.IsChecked;
                _config.ForceResolution = (bool)chk_Settings_ForceResolution.IsChecked;
                _config.Diaporama = (bool)chk_Settings_Diaporama.IsChecked;
                _config.Dimming = (bool)chk_Settings_Dimming.IsChecked;
                
                _config.Currently = (bool)chk_Settings_Currently.IsChecked;
                _config.CurrentlyMovie = (bool)chk_Settings_CurrentlyMovie.IsChecked;
                _config.UseNowPlayingMediaIcons = (bool)chk_Settings_UseNowPlayingMediaIcons.IsChecked;
                _config.HideCursor = (bool)chk_Settings_HideCursor.IsChecked;
                _config.UseLogo = (bool)chk_Settings_UseLogo.IsChecked;
                _config.UseBanner = (bool)chk_Settings_UseBanner.IsChecked;
                _config.ShowOverlay = (bool)chk_Settings_ShowOverlay.IsChecked;
                _config.ShowEmptyMusicGenre = (bool)chk_Settings_ShowEmptyMusicGenre.IsChecked;
                _config.DisableAnimations = (bool)chk_Settings_DisableAnimations.IsChecked;
                _config.ShowEndTime = (bool)chk_Settings_ShowEndTime.IsChecked;
                _config.Debug = (bool)chk_Settings_Debug.IsChecked;
                _config.HideWatched = (bool)chk_Settings_HideWatched.IsChecked;
                _config.RefreshOnConnect = (bool)chk_Settings_RefreshOnConnect.IsChecked;

                _config.QuickRefreshEnable = (bool)chk_Settings_QuickRefreshEnable.IsChecked;
                _config.ShowAudioMenu = (bool)chk_Settings_ShowAudioMenu.IsChecked;
                _config.DimmingOnlyVideo = (bool)chk_Settings_DimmingOnlyVideo.IsChecked;
                _config.HideCompilationArtists = (bool)chk_Settings_HideCompilationArtists.IsChecked;
                _config.GenreToArtists = (bool)chk_Settings_GenreToArtists.IsChecked;
                _config.MusicFanartRotation = (bool)chk_Settings_MusicFanartRotation.IsChecked;
                _config.FanartAlways = (bool)chk_Settings_CheckUpdate.IsChecked;
                _config.MouseMode = (bool)chk_Settings_MouseMode.IsChecked;
                _config.CropCacheImage = (bool)chk_Settings_CropCacheImage.IsChecked;
                _config.DebugTrace = (bool)chk_Settings_DebugTrace.IsChecked;
                _config.IgnoreSortTokens = (bool) chk_Settings_IgnoreSortTokens.IsChecked;
                _config.StartWithWindows = (bool)chk_Settings_StartWithWindows.IsChecked;
                _config.StartFrontViewServer = (bool)chk_Settings_StartFrontViewServer.IsChecked;
                _config.MinimiseAlways = (bool)chk_Settings_MinimiseAlways.IsChecked;
                _config.CoverArt = (bool)chk_Settings_CoverArt.IsChecked;
                _config.HttpSend = (bool)chk_Settings_HttpSend.IsChecked;
                _config.UseReceiverIPforVolume = (bool)chk_Settings_UseReceiver.IsChecked;

                _config.UseDDCControl = (bool)chk_Settings_UseDDCControl.IsChecked;
                _config.DisableRemoteCheck = (bool)chk_Settings_DisableRemoteCheck.IsChecked;
                _config.HttpUseDigest = (bool)chk_Settings_HttpUseDigest.IsChecked;
                // ReSharper restore PossibleInvalidOperationException
                _config.TVOrderbyNewEpsiodes = (bool)chk_Settings_TVOrderbyNewEpisodes.IsChecked;
            }
            catch (InvalidOperationException) { }

            _config.ImageDirectory = txt_Settings_ImagesDirectory.Text;
            _config.WeatherLoc = txt_Settings_WeatherLocId.Text;
            
            _config.HttpUser = txt_Settings_HttpUser.Text;
            _config.HttpPassword = txt_Settings_HttpPassword.Text;

            _config.ReceiverIP = txt_Settings_ReceiverIP.Text;
            _config.ReceiverPort = Convert.ToInt16(txt_Settings_ReceiverPort.Text);

            _config.HttpPlaystarted = txt_Settings_HttpPlaystarted.Text;
            _config.HttpPlaypaused = txt_Settings_HttpPlaypaused.Text;
            _config.HttpPlaystopped = txt_Settings_HttpPlaystopped.Text;
            _config.HttpMute = txt_Settings_HttpMute.Text;
            _config.HttpUnmute = txt_Settings_HttpUnmute.Text;
            _config.HttpMediatypeVideo = txt_Settings_HttpMediatypeVideo.Text;
            _config.HttpMediatypeAudio = txt_Settings_HttpMediatypeAudio.Text;
            _config.HttpPoweron = txt_Settings_HttpPoweron.Text;
            _config.HttpPoweroff = txt_Settings_HttpPoweroff.Text;
            _config.HttpPlayStartedDelayed = txt_Settings_HttpPlayStartedDelayed.Text;
            _config.HttpPlayStartedDelay = Convert.ToInt32(txt_Settings_HttpPlayStartedDelay.Text);
            _config.WeatherAPI = txt_Settings_WeatherAPI.Text;

            _config.WeatherUnit = lst_Settings_WeatherUnit.SelectedIndex == 0 ? "c" : "f";
            if (lst_Settings_Language.SelectedItem != null)
                _config.Language = lst_Settings_Language.SelectedItem.ToString();
            if (lst_Settings_Skin.SelectedItem != null)
                _config.Skin = lst_Settings_Skin.SelectedItem.ToString();

            var en = _yatse2Pages.GetEnumerator();
            while (en.MoveNext())
            {
                if (en.Key.ToString() == lst_Settings_HomePage.SelectedItem.ToString())
                    _config.Homepage = en.Key.ToString();
            }

             _config.DimAmount = Convert.ToDouble(lst_Settings_DimAmount.SelectedItem);

            _config.DimTime = Convert.ToInt32(lst_Settings_DimTime.SelectedItem);

            if (lst_Settings_Skin_extra.SelectedItem != null)
            {
                _config.Skin_Extra = lst_Settings_Skin_extra.SelectedItem.ToString();
            }

            _config.LogoSize = lst_Settings_LogoSize.Value;

            _config.MediaIconSize = lst_Settings_MediaIconSize.Value;

            //  _config.Skin_Extra_Logo = lst_Settings_Skin_Extra_Logo.SelectedItem.ToString();
            if (lst_Settings_Skin_Extra_Text.SelectedItem != null)
            {
                _config.Skin_Extra_Text = lst_Settings_Skin_Extra_Text.SelectedItem.ToString();
            }

            en = _yatse2PlayModes.GetEnumerator();
            while (en.MoveNext())
            {
                if (GetLocalizedString(Convert.ToInt32(en.Value.ToString())) == lst_Settings_DefaultPlay.SelectedItem.ToString())
                    _config.DefaultPlayMode = Convert.ToInt32(en.Key.ToString());
            }

            var resolution = (ScreenRes)lst_Settings_Resolution.SelectedItem;
            if (resolution != null)
            {
                _config.Resolution = resolution.Mode;
            }

            var Displayselect = lst_Settings_Displays.SelectedItem;
            if (Displayselect != null)
            {
                _config.SelectedDisplay = Displayselect.ToString();
            }
        }

        private void btn_Settings_CheckforUpdates_Click(object sender, RoutedEventArgs e)
        {
            WebSite();
            //Process.Start(new ProcessStartInfo(Helper.AppPath + @"\WyUpdate.exe"));
            //Disbale WyUpdate.exe


        }

        private void btn_Settings_SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            GetSettingsVars();

            Logger.Instance().Debug = _config.Debug;
            Logger.Instance().DebugTrace = _config.DebugTrace;

            Cursor = _config.HideCursor ? System.Windows.Input.Cursors.None : System.Windows.Input.Cursors.Arrow;

            _config.Save(_configFile);
            InitWeather();

            Helper.Instance.CurrentSkin = _config.Skin;
            _yatse2Properties.Skin = _config.Skin;
            _yatse2Properties.Language = _config.Language;
            _yatse2Properties.DimAmount = _config.DimAmount;
            _yatse2Properties.FanArtOpacity = _config.FanArtOpacity;
            _yatse2Properties.LogoSize = _config.LogoSize;
            _yatse2Properties.MediaIconSize = _config.MediaIconSize;
            _yatse2Properties.SemiCircleOpacity = _config.SemiCircleOpacity;
            _yatse2Properties.Skin_Extra = _config.Skin_Extra;
            _yatse2Properties.Skin_Extra_Text = _config.Skin_Extra_Text;
            _yatse2Properties.Skin_Extra_Logo = _config.Skin_Extra_Logo;
            _yatse2Properties.UseNowPlayingMediaIcons = _config.UseNowPlayingMediaIcons;
            _yatse2Properties.UseLogo = _config.UseLogo;
            RefreshDictionaries();

            _yatse2Properties.Currently.HideAudioMenu = _config.ShowAudioMenu;

            Helper.Instance.UseCoverArt = (bool)_config.CoverArt;

            RefreshHeader();

            if (_config.DefaultRemote < 1)
            {
                ShowGrid(grd_Remotes);
            }

            ShowPopup(GetLocalizedString(98));
            Change_Display_Settings(null, null);

            if (!_config.DisableAnimations)
            {
                trp_Transition.Transition = TryFindResource("GridTransition") as FluidKit.Controls.Transition;
            }
            else
            {
                trp_Transition.Transition = new FluidKit.Controls.NoTransition();
            }

            if (_config.StartWithWindows)
            {
                var rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (rkApp != null) rkApp.SetValue("Yatse2", System.Windows.Forms.Application.ExecutablePath);
            }
            else
            {
                var rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (rkApp != null) rkApp.DeleteValue("Yatse2", false);
            }

            _audioGenresDataSource.Clear();
            _audioArtistsDataSource.Clear();
            _audioArtistsDataSource.Clear();
            _moviesDataSource.Clear();
            _tvShowsDataSource.Clear();
        }
        private void btn_Settings_HttpOpen_Click(object sender, RoutedEventArgs e)
        {
            //Window1 httpWindow = new Window1();
           // httpWindow.Show();

        }
        private void Hyperlink_RequestNavigate(object sender,
                                       System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.wunderground.com/weather/api/?apiref=8d0e3e3f8bf9e135");
        }

        private void btn_Settings_WeatherLocId_Click(object sender, RoutedEventArgs e)
        {
            grd_Settings_Weather.Visibility = Visibility.Visible;
            //_yatse2Properties.ShowSettingsWeather = true;
        }

        private void btn_Settings_Weather_SelectLocId_Click(object sender, RoutedEventArgs e)
        {
            if (lst_Settings_Weather_LocationId.SelectedItem != null)
                txt_Settings_WeatherLocId.Text = ((WeatherLocation)lst_Settings_Weather_LocationId.SelectedItem).LocId;
                
            grd_Settings_Weather.Visibility = Visibility.Hidden;
            //_yatse2Properties.ShowSettingsWeather = false;
        }

        private void txt_Settings_Weather_WeatherLocId_TextChanged(object sender, TextChangedEventArgs e)
        {
            lst_Settings_Weather_LocationId.Items.Clear();
            if (txt_Settings_Weather_WeatherLocId.Text.Length < 3) return;

            var cities = Weather.SearchCity(txt_Settings_Weather_WeatherLocId.Text);
            foreach (var weatherLocation in cities)
            {
                lst_Settings_Weather_LocationId.Items.Add(weatherLocation);
            }
        }

        private void btn_Settings_Weather_Cancel_Click(object sender, RoutedEventArgs e)
        {
            grd_Settings_Weather.Visibility = Visibility.Hidden;
            //_yatse2Properties.ShowSettingsWeather = false;
        }

       
    }
}