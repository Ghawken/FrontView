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

using System.Windows;
using System;

namespace FrontView.Classes
{
    
    public class Yatse2Properties : DependencyObject
    {
        public static readonly DependencyProperty ApiProperty =
            DependencyProperty.Register("Api", typeof(string), typeof(Yatse2Properties));

        public string Api
        {
            get { return (string)GetValue(ApiProperty); }
            set { SetValue(ApiProperty, value); }
        }

        public static readonly DependencyProperty LanguageProperty =
            DependencyProperty.Register("Language", typeof(string), typeof(Yatse2Properties));


        /// <summary>
        ///  Change for Opacity Settings
        /// </summary>

        public static readonly DependencyProperty DimAmountProperty =
            DependencyProperty.Register("DimAmount", typeof(double), typeof(Yatse2Properties));

        public double DimAmount
        {
            get { return (double)GetValue(DimAmountProperty); }
            set
            {
                SetValue(DimAmountProperty, value);

            }
        }

        public static readonly DependencyProperty FanArtOpacityProperty =
    DependencyProperty.Register("FanArtOpacity", typeof(double), typeof(Yatse2Properties));

        public double FanArtOpacity
        {
            get { return (double)GetValue(FanArtOpacityProperty); }
            set
            {
                SetValue(FanArtOpacityProperty, value);

            }
        }


        public static readonly DependencyProperty LogoSizeProperty =
    DependencyProperty.Register("LogoSize", typeof(double), typeof(Yatse2Properties));

        public double LogoSize
        {
            get { return (double)GetValue(LogoSizeProperty); }
            set
            {
                SetValue(LogoSizeProperty, value);

            }
        }

        public static readonly DependencyProperty SemiCircleOpacityProperty =
    DependencyProperty.Register("SemiCircleOpacity", typeof(double), typeof(Yatse2Properties));

        public double SemiCircleOpacity
        {
            get { return (double)GetValue(SemiCircleOpacityProperty); }
            set
            {
                SetValue(SemiCircleOpacityProperty, value);

            }
        }


        public string Language
        {
            get { return (string)GetValue(LanguageProperty); }
            set { SetValue(LanguageProperty, value); }
        }

        public static readonly DependencyProperty PopupProperty =
            DependencyProperty.Register("Popup", typeof(string), typeof(Yatse2Properties));

        public string Popup
        {
            get { return (string)GetValue(PopupProperty); }
            set { SetValue(PopupProperty, value); }
        }

        public static readonly DependencyProperty Skin_ExtraProperty =
    DependencyProperty.Register("Skin_Extra", typeof(string), typeof(Yatse2Properties));

        public string Skin_Extra
        {
            get { return (string)GetValue(Skin_ExtraProperty); }
            set { SetValue(Skin_ExtraProperty, value); }
        }

        public static readonly DependencyProperty Skin_Extra_TextProperty =
DependencyProperty.Register("Skin_Extra_Text", typeof(string), typeof(Yatse2Properties));

        public string Skin_Extra_Text
        {
            get { return (string)GetValue(Skin_Extra_TextProperty); }
            set { SetValue(Skin_Extra_TextProperty, value); }
        }

        public static readonly DependencyProperty UseLogoProperty =
DependencyProperty.Register("UseLogo", typeof(bool), typeof(Yatse2Properties));

        public bool UseLogo
        {
            get { return (bool)GetValue(UseLogoProperty); }
            set { SetValue(UseLogoProperty, value); }
        }


        public static readonly DependencyProperty Skin_Extra_LogoProperty =
DependencyProperty.Register("Skin_Extra_Logo", typeof(string), typeof(Yatse2Properties));

        public string Skin_Extra_Logo
        {
            get { return (string)GetValue(Skin_Extra_LogoProperty); }
            set { SetValue(Skin_Extra_LogoProperty, value); }
        }

        public static readonly DependencyProperty SkinProperty =
            DependencyProperty.Register("Skin", typeof(string), typeof(Yatse2Properties));

        public string Skin
        {
            get { return (string)GetValue(SkinProperty); }
            set { SetValue(SkinProperty, value); }
        }

        public static readonly DependencyProperty RefreshWhatProperty =
            DependencyProperty.Register("RefreshWhat", typeof(string), typeof(Yatse2Properties));

        public string RefreshWhat
        {
            get { return (string)GetValue(RefreshWhatProperty); }
            set { SetValue(RefreshWhatProperty, value); }
        }

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(string), typeof(Yatse2Properties));

        public string Date
        {
            get { return (string)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(string), typeof(Yatse2Properties));

        public string Time
        {
            get { return (string)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public static readonly DependencyProperty VideoPovProperty =
            DependencyProperty.Register("VideoPov", typeof(double), typeof(Yatse2Properties));

        public double VideoPov
        {
            get { return (double)GetValue(VideoPovProperty); }
            set { SetValue(VideoPovProperty, value); }
        }

        public static readonly DependencyProperty MovieCacheHeightProperty =
       DependencyProperty.Register("MovieCacheHeight", typeof(double), typeof(Yatse2Properties));

        public double MovieCacheHeight
        {
            get { return (double)GetValue(MovieCacheHeightProperty); }
            set { SetValue(MovieCacheHeightProperty, value); }
        }

        public static readonly DependencyProperty MovieCacheWidthProperty =
DependencyProperty.Register("MovieCacheWidth", typeof(double), typeof(Yatse2Properties));

        public double MovieCacheWidth
        {
            get { return (double)GetValue(MovieCacheWidthProperty); }
            set { SetValue(MovieCacheWidthProperty, value); }
        }



        public static readonly DependencyProperty TVCacheHeightProperty =
       DependencyProperty.Register("TVCacheHeight", typeof(double), typeof(Yatse2Properties));

        public double TVCacheHeight
        {
            get { return (double)GetValue(TVCacheHeightProperty); }
            set { SetValue(TVCacheHeightProperty, value); }
        }

        public static readonly DependencyProperty TVCacheWidthProperty =
DependencyProperty.Register("TVCacheWidth", typeof(double), typeof(Yatse2Properties));

        public double TVCacheWidth
        {
            get { return (double)GetValue(TVCacheWidthProperty); }
            set { SetValue(TVCacheWidthProperty, value); }
        }



        public static readonly DependencyProperty TvShowPosterPovProperty =
            DependencyProperty.Register("TvShowPosterPov", typeof(double), typeof(Yatse2Properties));

        public double TvShowPosterPov
        {
            get { return (double)GetValue(TvShowPosterPovProperty); }
            set { SetValue(TvShowPosterPovProperty, value); }
        }

        public static readonly DependencyProperty AudioGenresPovProperty =
            DependencyProperty.Register("AudioGenresPov", typeof(double), typeof(Yatse2Properties));

        public double AudioGenresPov
        {
            get { return (double)GetValue(AudioGenresPovProperty); }
            set { SetValue(AudioGenresPovProperty, value); }
        }

        public static readonly DependencyProperty AudioAlbumsPovProperty =
            DependencyProperty.Register("AudioAlbumsPov", typeof(double), typeof(Yatse2Properties));

        public double AudioAlbumsPov
        {
            get { return (double)GetValue(AudioAlbumsPovProperty); }
            set { SetValue(AudioAlbumsPovProperty, value); }
        }

        public static readonly DependencyProperty AudioArtistsPovProperty =
            DependencyProperty.Register("AudioArtistsPov", typeof(double), typeof(Yatse2Properties));

        public double AudioArtistsPov
        {
            get { return (double)GetValue(AudioArtistsPovProperty); }
            set { SetValue(AudioArtistsPovProperty, value); }
        }

        public static readonly DependencyProperty WeatherProperty =
            DependencyProperty.Register("Weather", typeof(Yatse2Weather), typeof(Yatse2Properties));

        public Yatse2Weather Weather
        {
            get { return (Yatse2Weather)GetValue(WeatherProperty); }
            set { SetValue(WeatherProperty, value); }
        }

        public static readonly DependencyProperty CurrentlyProperty =
            DependencyProperty.Register("Currently", typeof(Yatse2Currently), typeof(Yatse2Properties));

        public Yatse2Currently Currently
        {
            get { return (Yatse2Currently)GetValue(CurrentlyProperty); }
            set { SetValue(CurrentlyProperty, value); }
        }

        public static readonly DependencyProperty DiaporamaImage1Property =
            DependencyProperty.Register("DiaporamaImage1", typeof(string), typeof(Yatse2Properties));

        public string DiaporamaImage1
        {
            get { return (string)GetValue(DiaporamaImage1Property); }
            set { SetValue(DiaporamaImage1Property, value); }
        }

        public static readonly DependencyProperty DiaporamaImage2Property =
            DependencyProperty.Register("DiaporamaImage2", typeof(string), typeof(Yatse2Properties));

        public string DiaporamaImage2
        {
            get { return (string)GetValue(DiaporamaImage2Property); }
            set { SetValue(DiaporamaImage2Property, value); }
        }

        public static readonly DependencyProperty IsSyncingProperty =
    DependencyProperty.Register("IsSyncing", typeof(bool), typeof(Yatse2Properties));

        public bool IsSyncing
        {
            get { return (bool)GetValue(IsSyncingProperty); }
            set { SetValue(IsSyncingProperty, value); }
        }


        public static readonly DependencyProperty ShowSettingsRemotesEditProperty =
            DependencyProperty.Register("ShowSettingsRemotesEdit", typeof(bool), typeof(Yatse2Properties));

        public bool ShowSettingsRemotesEdit
        {
            get { return (bool)GetValue(ShowSettingsRemotesEditProperty); }
            set { SetValue(ShowSettingsRemotesEditProperty, value); }
        }

        public static readonly DependencyProperty ShowRefreshLibraryProperty =
            DependencyProperty.Register("ShowRefreshLibrary", typeof(bool), typeof(Yatse2Properties));

        public bool ShowRefreshLibrary
        {
            get { return (bool)GetValue(ShowRefreshLibraryProperty); }
            set { SetValue(ShowRefreshLibraryProperty, value); }
        }

        public static readonly DependencyProperty ShowSettingsWeatherProperty =
            DependencyProperty.Register("ShowSettingsWeather", typeof(bool), typeof(Yatse2Properties));

        public bool ShowSettingsWeather
        {
            get { return (bool)GetValue(ShowSettingsWeatherProperty); }
            set { SetValue(ShowSettingsWeatherProperty, value); }
        }

        public static readonly DependencyProperty ShowHomeButtonProperty =
            DependencyProperty.Register("ShowHomeButton", typeof(bool), typeof(Yatse2Properties));

        public bool ShowHomeButton
        {
            get { return (bool)GetValue(ShowHomeButtonProperty); }
            set { SetValue(ShowHomeButtonProperty, value); }
        }

        public static readonly DependencyProperty CurrentAlbumArtistFanartProperty =
            DependencyProperty.Register("CurrentAlbumArtistFanart", typeof(string), typeof(Yatse2Properties));

        public string CurrentAlbumArtistFanart
        {
            get { return (string)GetValue(CurrentAlbumArtistFanartProperty); }
            set { SetValue(CurrentAlbumArtistFanartProperty, value); }
        }

        public static readonly DependencyProperty TvSeasonEpCountProperty =
            DependencyProperty.Register("TvSeasonEpCount", typeof(string), typeof(Yatse2Properties));

        public string TvSeasonEpCount
        {
            get { return (string)GetValue(TvSeasonEpCountProperty); }
            set { SetValue(TvSeasonEpCountProperty, value); }
        }

        public static readonly DependencyProperty TvSeasonNameProperty =
            DependencyProperty.Register("TvSeasonName", typeof(string), typeof(Yatse2Properties));

        public string TvSeasonName
        {
            get { return (string)GetValue(TvSeasonNameProperty); }
            set { SetValue(TvSeasonNameProperty, value); }
        }

        public static readonly DependencyProperty CollectionFilterProperty =
            DependencyProperty.Register("CollectionFilter", typeof(string), typeof(Yatse2Properties));

        public string CollectionFilter
        {
            get { return (string)GetValue(CollectionFilterProperty); }
            set { SetValue(CollectionFilterProperty, value); }
        }
    }
}