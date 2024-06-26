﻿// ------------------------------------------------------------------------
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
using System.Xml.Serialization;
using Setup;
using FrontView.Libs;

namespace FrontView.Classes
{
    
    public class FrontViewConfig
    {

        
        public bool IsConfigured { get; set; }
        public bool Debug { get; set; }
        public bool SecondScreen { get; set; }
        public bool MinimiseAlways { get; set; }

        public bool CoverArt { get; set; }
        
        // Http Send Variable Changes
        public string HttpUser { get; set; }

        public string HttpPassword { get; set; }

        public string HttpPlaystarted { get; set; }
        public string HttpPlaypaused { get; set; }

        public string HttpPlaystopped { get; set; }

        public string HttpFilename { get; set; }

        public string HttpType { get; set; }

        public string HttpMute { get; set; }
        public string HttpUnmute { get; set; }
        public string HttpMediatypeAudio { get; set; }
        public string HttpMediatypeVideo { get; set; }
        public string HttpPoweron { get; set; }
        public string HttpPoweroff { get; set; }
        public bool HttpSend { get; set; }
        public bool HttpUseDigest { get; set; }

        public string HttpPlayStartedDelayed { get; set; }
        public int HttpPlayStartedDelay { get; set; }
        public bool FanartAlways { get; set; }
        public bool Topmost { get; set; }
        public bool KeepFocus { get; set; }
        public bool ForceResolution { get; set; }
        public int ScreenPositionX { get; set; }
        public int ScreenPositionY { get; set; }
        public bool Diaporama { get; set; }
        public bool Dimming { get; set; }
        public double DimAmount { get; set; }
        public bool ShowInTaskbar { get; set; }
        public bool TVOrderbyNewEpsiodes { get; set; }
        public int DimTime { get; set; }
        public string Skin_Extra { get; set; }
        public string Skin_Extra_Text { get; set; }
        public string Skin_Extra_Logo { get; set; }
        public double FanArtOpacity { get; set; }
        public double LogoSize { get; set; }
        public double TimeSize { get; set; }
        public double EverythingSize { get; set; }
        public double MediaIconSize { get; set; }
        public double SemiCircleOpacity { get; set; }
        public bool Currently { get; set; }
        public bool CurrentlyMovie { get; set; }
        public bool HideCursor { get; set; }
        public bool UseLogo { get; set; }
        public bool UseNowPlayingMediaIcons { get; set; }
        public bool useNowPlayingRemainingTime { get; set; }

        public bool useNowPlayingEndTime { get; set; }
        public bool UseBanner { get; set; }
        public bool ShowOverlay { get; set; } // TODO : Use this
        public bool ShowEmptyMusicGenre { get; set; }
        public bool ManualRefresh { get; set; }
        public bool DisableAnimations { get; set; }
        public bool ShowEndTime { get; set; } // TODO : Use this
        public bool HideWatched { get; set; } // TODO : Use this
        public bool RefreshOnConnect { get; set; }
        public bool QuickRefreshEnable { get; set; }
        public bool AnimatedMusicCover { get; set; }
        public bool DimmingOnlyVideo { get; set; }
        public bool Hack480 { get; set; }
        public bool ShowAudioMenu { get; set; }
        public bool ShowVideoMenu { get; set; }
        public bool DebugTrace { get; set; }
        public bool ForceOnCheckRemote { get; set; }
        public bool HideCompilationArtists { get; set; }
        public bool GenreToArtists { get; set; }
        public bool MusicFanartRotation { get; set; }
        public long ScreensaverTimer { get; set; }
        public long DimmingTimer { get; set; }
        public long FanartTimer { get; set; }
        public bool GoHomeOnEndPlayback { get; set; }
        public bool CheckForUpdate { get; set; }
        public bool DiaporamaSubdirs { get; set; }
        public bool DisableScreenPositioning { get; set; }
        public bool MouseMode { get; set; }
        public int MinDMBitsPerPel { get; set; }
        public int MinDMPelsWidth { get; set; }
        public int DiaporamaTimer { get; set; }
        public Devmode Resolution { get; set; }
        public string SelectedDisplay { get; set; }
        public string ImageDirectory { get; set; }
        public string FanartDirectoryFixed { get; set; }
        public string FanartDirectory { get; set; }
        
        public int FanartNumberDirectories { get; set; }
        public string FanartCurrentPath { get; set; }
        public string FanartDirectoryTV { get; set; }
        public string FanartDirectoryMovie { get; set; }
        public string FanartDirectoryWeather { get; set; }
        public string FanartDirectoryMusic { get; set; }
        public string Language { get; set; }
        public string FanartDirectoryMyImages { get; set; }
        public bool FanartSwitch { get; set; }

        public int IPPort { get; set; }
        public string IPAddress { get; set; }
        public string Skin { get; set; }
        public string WeatherLoc { get; set; }
        public string WeatherLongitude { get; set; }
        public string WeatherLatitude { get; set; }

        public string WeatherAPI { get; set; }
        public string WeatherUnit { get; set; }
        public string Homepage { get; set; }
        public long DefaultRemote { get; set; }
        public bool CropCacheImage { get; set; }
        public bool IgnoreSortTokens { get; set; }
        public string SortTokens { get; set; }
        public bool StartWithWindows { get; set; }
        public bool StartFrontViewServer { get; set; }

        public bool UseReceiverIPforVolume { get; set; }

        public bool UseDDCControl { get; set; }
        public bool TurnOffDDCControl { get; set; }
        public bool DisableRemoteCheck { get; set; }
        public bool NoHomeScreenFanart { get; set; }

        public bool NoHomeScreenClock { get; set; }
        public string ReceiverIP { get; set; }

        public int ReceiverPort { get; set; }
        public int DefaultPlayMode { get; set; }
        public int LongKeyPress { get; set; }
        public int DiaporamaMode { get; set; }
        public bool DisableResolutionDetection { get; set; }
       //For Source.xml file
        public string[] KodiSources { get; set; }

        public FrontViewConfig()
        {
            IsConfigured = false;
            Debug = true;
            SecondScreen = false;
            FanartAlways = false;
            MinimiseAlways = false;
            CoverArt = false;
            Topmost = true;
            Hack480 = false;
            ShowAudioMenu = true;
            ShowVideoMenu = true;
            KeepFocus = false;
            ForceResolution = false;
            Diaporama = false;
            Dimming = false;
            DimAmount = 0.9;
            TVOrderbyNewEpsiodes = true;
            ShowInTaskbar = false;
            DimTime = 2;
            Currently = true;
            CurrentlyMovie = true;
            HideCursor = false;
            UseLogo = true;
            UseNowPlayingMediaIcons = true;
            useNowPlayingRemainingTime = false;
            useNowPlayingEndTime = false;
            UseBanner = false;
            ShowOverlay = true;
            ShowEmptyMusicGenre = false;
            ManualRefresh = true;
            DisableAnimations = false;
            ShowEndTime = false;
            HideWatched = false;
            RefreshOnConnect = false;
            AnimatedMusicCover = true;
            QuickRefreshEnable = true;
            DimmingOnlyVideo = true;
            DebugTrace = false;
            ForceOnCheckRemote = true;
            HideCompilationArtists = false;
            GenreToArtists = false;
            CheckForUpdate = true;
            Language = "English";
            Skin = "Default";
            WeatherLoc = "Unknown";
            WeatherLatitude = "42.3601";
            WeatherLongitude = "-71.0589";  //Default to New York
            WeatherUnit = "metric";
            WeatherAPI = "1111";
            DefaultRemote = 0;
            Homepage = "Home";
            ScreensaverTimer = 120;
            GoHomeOnEndPlayback = true;
            MusicFanartRotation = false;
            DiaporamaSubdirs = true;
            MinDMBitsPerPel = 0;
            MinDMPelsWidth = 480;
            DiaporamaTimer = 10;
            FanartTimer = 5;
            DimmingTimer = 15;
            Skin_Extra = "Large NowPlaying";
            Skin_Extra_Text = "Large";
            Skin_Extra_Logo = "Large";
            ScreenPositionX = 0;
            ScreenPositionY = 0;
            DisableScreenPositioning = false;
            MouseMode = false;
            CropCacheImage = true;
            SortTokens = "Le |La |Les |The |A |An |L'";
            IgnoreSortTokens = false;
            StartWithWindows = false;
            StartFrontViewServer = false;
            SelectedDisplay = "";
            DefaultPlayMode = 0;
            LongKeyPress = 500;
            DiaporamaMode = 1;
            DisableResolutionDetection = false;
            FanartSwitch = true;
            FanartDirectory = @"addon_data\script.artworkorganizer\MovieFanart\";
            FanartDirectoryTV = @"addon_data\script.artworkorganizer\TVShowFanart\";
            FanartDirectoryWeather = @"addon_data\skin.aeonmq5.extrapack\backgrounds_weather\";
            FanartDirectoryMovie = @"addon_data\script.artworkorganizer\MovieFanart\";
            FanartDirectoryMusic = @"addon_data\script.artworkorganizer\ArtistFanart\";
            FanartDirectoryMyImages = @"addon_data\script.artworkorganizer\OwnFanart\";
            IPAddress = "127.0.0.1";
            IPPort = 5000;
            FanartNumberDirectories = 3;
            HttpSend = false;
            HttpUseDigest = false;
            LogoSize = 100.0;
            TimeSize = 22;
            EverythingSize = 1.0;
            MediaIconSize = 50;
            FanArtOpacity = 1.0;
            SemiCircleOpacity = 0.65;
            HttpUser = "";
            HttpPassword = "";
            UseReceiverIPforVolume = false;
            ReceiverIP = "";
            ReceiverPort = 23;
            UseDDCControl = false;
            TurnOffDDCControl = false;
            DisableRemoteCheck = false;
            NoHomeScreenFanart = false;
            NoHomeScreenClock = false;
           // Skin_Extra_Text = ;
           // Skin_Extra_Logo = ;
        }

        public bool Load(string configFile)
        {
            Logger.Instance().Log("FrontViewConfig","Loading config : " + configFile);
            FrontViewConfig config;
            try
            {
                var deserializer = new XmlSerializer(typeof(FrontViewConfig));
                using (TextReader textReader = new StreamReader(configFile))
                {
                    config = (FrontViewConfig) deserializer.Deserialize(textReader);
                }
            }
            catch (Exception ex)
            {
                if (ex is IOException || ex is InvalidOperationException )
                {
                    Logger.Instance().Log("FrontViewConfig", "Error loading settings : " + ex.Message);
                    return false;
                }
                throw;
            }

            IsConfigured = config.IsConfigured;
            Debug = config.Debug;
            SecondScreen = config.SecondScreen;
            MinimiseAlways = config.MinimiseAlways;
            CoverArt = config.CoverArt;
            // Changes for Http Sending
            HttpSend = config.HttpSend;
            HttpUseDigest = config.HttpUseDigest;
            LogoSize = config.LogoSize;
            TimeSize = config.TimeSize;
            EverythingSize = config.EverythingSize;
            MediaIconSize = config.MediaIconSize;
            FanArtOpacity = config.FanArtOpacity;
            SemiCircleOpacity = config.SemiCircleOpacity;
            HttpUser = config.HttpUser;
            ReceiverIP = config.ReceiverIP;
            ReceiverPort = config.ReceiverPort;
            HttpPassword = config.HttpPassword;
            HttpPlaypaused = config.HttpPlaypaused;
            HttpPlaystarted = config.HttpPlaystarted;
            HttpMute = config.HttpMute;
            HttpUnmute = config.HttpUnmute;
            HttpMediatypeVideo = config.HttpMediatypeVideo;
            HttpMediatypeAudio = config.HttpMediatypeAudio;
            HttpPoweron = config.HttpPoweron;
            HttpPoweroff = config.HttpPoweroff;
            HttpPlayStartedDelayed = config.HttpPlayStartedDelayed;
            HttpPlayStartedDelay = config.HttpPlayStartedDelay;
            HttpPlaystopped = config.HttpPlaystopped;
            // Shouldnt need to save others
            FanartAlways = config.FanartAlways;
            Topmost = config.Topmost;
            ScreenPositionX = config.ScreenPositionX;
            ScreenPositionY = config.ScreenPositionY;
            KeepFocus = config.KeepFocus;
            ForceResolution = config.ForceResolution;
            Diaporama = config.Diaporama;
            QuickRefreshEnable = config.QuickRefreshEnable;
            Dimming = config.Dimming;
            ShowInTaskbar = config.ShowInTaskbar;
            DimAmount = config.DimAmount;
            Skin_Extra = config.Skin_Extra;
            Skin_Extra_Text = config.Skin_Extra_Text;
            Skin_Extra_Logo = config.Skin_Extra_Logo;
            TVOrderbyNewEpsiodes = config.TVOrderbyNewEpsiodes;
            DimTime = config.DimTime;
            Currently = config.Currently;
            CurrentlyMovie = config.CurrentlyMovie;
            HideCursor = config.HideCursor;
            UseNowPlayingMediaIcons = config.UseNowPlayingMediaIcons;
            useNowPlayingRemainingTime = config.useNowPlayingRemainingTime;
            useNowPlayingEndTime = config.useNowPlayingEndTime;
            UseLogo = config.UseLogo;
            UseBanner = config.UseBanner;
            ShowOverlay = config.ShowOverlay;
            ShowEmptyMusicGenre = config.ShowEmptyMusicGenre;
            ManualRefresh = config.ManualRefresh;
            if (File.Exists(Helper.LangPath + config.Language + ".xaml"))
                Language = config.Language;
            if (Directory.Exists(Helper.SkinPath + config.Skin))
                Skin = config.Skin;
            Resolution = config.Resolution;
            SelectedDisplay = config.SelectedDisplay;
            ImageDirectory = config.ImageDirectory;
            FanartCurrentPath = "";  // config.FanartCurrentPath;
            FanartDirectory = config.FanartDirectory;
            //FanartSwitch = config.FanartSwitch;
            FanartDirectoryTV = config.FanartDirectoryTV;
            FanartDirectoryMovie = config.FanartDirectoryMovie;
            FanartDirectoryWeather = config.FanartDirectoryWeather;
            FanartDirectoryMyImages = config.FanartDirectoryMyImages;
            FanartDirectoryMusic = config.FanartDirectoryMusic;
            FanartNumberDirectories = config.FanartNumberDirectories;
            IPAddress = config.IPAddress;
            IPPort = config.IPPort;
            DefaultRemote = config.DefaultRemote;
            WeatherLoc = config.WeatherLoc;
            WeatherLongitude = config.WeatherLongitude;
            WeatherLatitude = config.WeatherLatitude;

            WeatherUnit = config.WeatherUnit;
            WeatherAPI = config.WeatherAPI;
            DisableAnimations = config.DisableAnimations;
            ShowEndTime = config.ShowEndTime;
            HideWatched = config.HideWatched;
            RefreshOnConnect = config.RefreshOnConnect;
            AnimatedMusicCover = config.AnimatedMusicCover;
            DimmingOnlyVideo = config.DimmingOnlyVideo;
            Hack480 = config.Hack480;
            ShowAudioMenu = config.ShowAudioMenu;
            ShowVideoMenu = config.ShowVideoMenu;
            Homepage = config.Homepage;
            DebugTrace = config.DebugTrace;
            ForceOnCheckRemote = config.ForceOnCheckRemote;
            GenreToArtists = config.GenreToArtists;
            HideCompilationArtists = config.HideCompilationArtists;
            MusicFanartRotation = config.MusicFanartRotation;
            ScreensaverTimer = config.ScreensaverTimer;
            GoHomeOnEndPlayback = config.GoHomeOnEndPlayback;
            CheckForUpdate = config.CheckForUpdate;
            DiaporamaSubdirs = config.DiaporamaSubdirs;
            MinDMBitsPerPel = config.MinDMBitsPerPel;
            MinDMPelsWidth = config.MinDMPelsWidth;
            DisableScreenPositioning = config.DisableScreenPositioning;
            DiaporamaTimer = config.DiaporamaTimer;
            MouseMode = config.MouseMode;
            DimmingTimer = config.DimmingTimer;
            FanartTimer = config.FanartTimer;
            CropCacheImage = config.CropCacheImage;
            IgnoreSortTokens = config.IgnoreSortTokens;
            SortTokens = config.SortTokens;
            StartWithWindows = config.StartWithWindows;
            StartFrontViewServer = config.StartFrontViewServer;
            UseReceiverIPforVolume = config.UseReceiverIPforVolume;
            UseDDCControl = config.UseDDCControl;
            TurnOffDDCControl = config.TurnOffDDCControl;
            DisableRemoteCheck = config.DisableRemoteCheck;
            NoHomeScreenFanart = config.NoHomeScreenFanart;
            NoHomeScreenClock = config.NoHomeScreenClock;
            DefaultPlayMode = config.DefaultPlayMode;
            LongKeyPress = config.LongKeyPress;
            DiaporamaMode = config.DiaporamaMode;
            DisableResolutionDetection = config.DisableResolutionDetection;
            FanartDirectoryFixed = config.FanartDirectory;

            Helper.Instance.UseCoverArt = (bool)config.CoverArt;

            return true;
        }

        public void Save(string configFile)
        {
            Logger.Instance().Log("FrontViewConfig", "Saving settings : " + configFile);

            try
            {
                var res = Resolution;
                res.DMFormName = "";
                Resolution = res;
                FanartDirectory = FanartDirectoryFixed;
                var serializer = new XmlSerializer(typeof(FrontViewConfig));
                using (TextWriter textWriter = new StreamWriter(configFile))
                {
                    serializer.Serialize(textWriter, this);
                }
            }
            catch (IOException e)
            {
                Logger.Instance().Log("FrontViewConfig", "Error saving settings : " + e.Message);
            }
            return;
        }

    }
}


