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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Plugin;
using FrontView.Classes;
using FrontView.Libs;
using Setup;
using System.Windows.Automation.Peers;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Forms;

namespace FrontView
{



    public static class RestoreWindowNoActivateExtension
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, UInt32 nCmdShow);

        private const int SW_SHOWNOACTIVATE = 4;

        public static void RestoreNoActivate(this Window win)
        {
            WindowInteropHelper winHelper = new WindowInteropHelper(win);
            ShowWindow(winHelper.Handle, SW_SHOWNOACTIVATE);
        }
    }
    public static class KodiSourceData
    {
        public static string[] KodiSources = new string[20];
        public static string[] KodiMusicSources = new string[20];
    }

  

    public partial class Yatse2Window : IDisposable
    {
        private const string Repository = @"http://yatse.leetzone.org/repository";
        private bool _allowBeta;

        //changed below to public
        
        public readonly FrontViewConfig _config = new FrontViewConfig();
        private readonly string _configFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\FrontView+\settings.xml";
        private readonly Yatse2DB _database = new Yatse2DB();
        private readonly Weather _weather = new Weather();
        private long _timerScreenSaver = 120;
        private bool _startLetterDrag;
        private Point _mouseDownPoint;
        private DateTime _mouseDownTime;

        public delegate void UpdateTextCallback(string message);

        private readonly Hashtable _yatse2Pages = new Hashtable
                            {
                                {"Home", "grd_Home"},
                                {"Movies", "grd_Movies"},
                                {"Tv Shows", "grd_TvShows"},
                                {"Music Artists", "grd_AudioArtists"},
                                {"Music Album", "grd_AudioAlbums"},
                                {"Music Genres", "grd_AudioGenres"},
                                {"Weather", "grd_Weather"}
                            };
        private readonly Hashtable _yatse2PlayModes = new Hashtable
                            {
                                {0, 139},
                                {1, 140}
                            };

        private bool _videoFavoritesFilter;
        private bool _audioFavoritesFilter;
        private bool _failedRemoteCheck;
        //private bool _updatecheck;
        private Yatse2Properties _yatse2Properties;
        private MoviesCollection _moviesDataSource;
        private CollectionView _moviesCollectionView;
        private TvShowsCollection _tvShowsDataSource;
        private CollectionView _tvShowsCollectionView;
        private TvSeasonsCollection _tvSeasonsDataSource;
        private CollectionView _tvSeasonsCollectionView;
        private TvEpisodesCollection _tvEpisodesDataSource;
        private CollectionView _tvEpisodesCollectionView;
        private AudioAlbumsCollection _audioAlbumsDataSource;
        private CollectionView _audioAlbumsCollectionView;
        private AudioArtistsCollection _audioArtistsDataSource;
        private CollectionView _audioArtistsCollectionView;
        private AudioGenresCollection _audioGenresDataSource;
        private CollectionView _audioGenresCollectionView;
        private AudioSongsCollection _audioSongsDataSource;
        public System.Windows.Forms.NotifyIcon ni = null;

        private readonly Collection<string> _gridHistory = new Collection<string>();
        private long _currentRemoteId;
        private ApiConnection _remote;
        private IYatse2RemotePlugin _remotePlugin;
        private bool _remoteConnected;
        private bool _remoteLibraryRefreshed;
        private Yatse2Remote _remoteInfo;
        private Yatse2Remote _remoteInfoEdit;
        private Grid _currentGrid;
        private long _timer;
        //private Array KodiDirectories;
        private long _timerHeader;
        private bool _isScreenSaver;
        private bool _isfanart;
        private int _diaporamaCurrentImage;
        private int _fanartCurrentImage;
        private bool _showRemoteSelect;
        private bool _showHomePage;
        private bool _isPlaying;
        private bool _disableFocus;

        private bool HttpisPlaying;
        private bool HttpisPaused;
        private bool HttpisMuted;
        private bool HttpisStopped;
        private bool HttpisDelayed;

        private string _filterMovie = "";
        private string _filterTvShow = "";
        private string _filterAudioGenre = "";
        private string _filterAudioAlbum = "";
        private string _filterAudioArtist = "";
        private bool _filteredArtists;
        private bool _filteredAlbums;
        private bool _setPov;
        public string[] KodiSources;

        IAvReceiverControl receiver = new VSX1123();


        private bool UpdateAvailable = false;

        public string GetLocalizedString(int id)
        {
            var ret = (string)TryFindResource("Localized_" + id);
            return ret ?? "Non localized string";
        }
        
        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        private void SetMonitorState()
        {
            Form frm = new Form();

            SendMessage(frm.Handle, 0x0112, 0xF170, 2);

        }
        
        private void StartDiaporama()
        {
            switch (_config.DiaporamaMode)
            {
                case 0:
                    img_Diaporama1.Stretch = Stretch.None;
                    img_Diaporama2.Stretch = Stretch.None;
                    break;
                case 1:
                    img_Diaporama1.Stretch = Stretch.Uniform;
                    img_Diaporama2.Stretch = Stretch.Uniform;
                    break;
                case 2:
                    img_Diaporama1.Stretch = Stretch.UniformToFill;
                    img_Diaporama2.Stretch = Stretch.UniformToFill;
                    break;
                case 3:
                    img_Diaporama1.Stretch = Stretch.Fill;
                    img_Diaporama2.Stretch = Stretch.Fill;
                    break;
            }

            _yatse2Properties.DiaporamaImage1 = GetRandomImagePath(_config.ImageDirectory);
            _diaporamaCurrentImage = 1;
            var stbDiaporamaShow = (Storyboard)TryFindResource("stb_ShowDiaporama");
            if (stbDiaporamaShow != null)
            {
                stbDiaporamaShow.Begin(this);
                _isScreenSaver = true;
            }
        }

        private void SwitchDiaporama()
        {
            if (_diaporamaCurrentImage == 1)
            {
                _diaporamaCurrentImage = 2;
                _yatse2Properties.DiaporamaImage2 = GetRandomImagePath(_config.ImageDirectory);
                var stbDiaporamaSwap = (Storyboard)TryFindResource("stb_Diaporama_12_1");
                if (stbDiaporamaSwap != null)
                    stbDiaporamaSwap.Begin(this);
            }
            else
            {
                _diaporamaCurrentImage = 1;
                _yatse2Properties.DiaporamaImage1 = GetRandomImagePath(_config.ImageDirectory);
                var stbDiaporamaSwap = (Storyboard)TryFindResource("stb_Diaporama_21_1");
                if (stbDiaporamaSwap != null)
                    stbDiaporamaSwap.Begin(this);
            }
        }
        private void StartFanart()
        {
            
            switch (_config.DiaporamaMode)
            {
                case 0:
                    img_Diaporama1.Stretch = Stretch.None;
                    img_Diaporama2.Stretch = Stretch.None;
                    break;
                case 1:
                    img_Diaporama1.Stretch = Stretch.Uniform;
                    img_Diaporama2.Stretch = Stretch.Uniform;
                    break;
                case 2:
                    img_Diaporama1.Stretch = Stretch.UniformToFill;
                    img_Diaporama2.Stretch = Stretch.UniformToFill;
                    break;
                case 3:
                    img_Diaporama1.Stretch = Stretch.Fill;
                    img_Diaporama2.Stretch = Stretch.Fill;
                    break;
            }
          //  var appdatadirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
          //  _config.FanartDirectory = appdatadirectory + @"\Kodi\userdata\addon_data\script.artworkorganizer\";
         
       //     Logger.Instance().Log("FanART DEBUG", "Fanart Directory equals " + _config.FanartDirectory, true);
            



            _yatse2Properties.DiaporamaImage1 = GetRandomImagePathNew(_config.FanartDirectory);
            if (_yatse2Properties.DiaporamaImage1 == null)
            {
                //isfanart = false;
                var stbDiaporamaHide = (Storyboard)TryFindResource("stb_HideDiaporama");
                if (stbDiaporamaHide != null)
                {
                    stbDiaporamaHide.Begin(this);
                }
                return;
            }



            _fanartCurrentImage = 1;
            var stbDiaporamaShow = (Storyboard)TryFindResource("stb_ShowDiaporama");
            if (stbDiaporamaShow != null)
            {
                stbDiaporamaShow.Begin(this);
                _isfanart = true;
                _isScreenSaver = true;
            }
        }

        private void SwitchFanart()
        {
           /*( if (grd_Diaporama.Visibility == Visibility.Hidden )
            {
                var stbDiaporamaShow = (Storyboard)TryFindResource("stb_ShowDiaporama");
                if (stbDiaporamaShow != null)
                {
                    stbDiaporamaShow.Begin(this);
                }
            }*/
            
            if (_fanartCurrentImage == 1)
            {
                _fanartCurrentImage = 2;
                _yatse2Properties.DiaporamaImage2 = GetRandomImagePathNew(_config.FanartDirectory);
                var stbDiaporamaSwap = (Storyboard)TryFindResource("stb_Diaporama_12_1");
                if (stbDiaporamaSwap != null)
                    stbDiaporamaSwap.Begin(this);
            }
            else
            {
                _fanartCurrentImage = 1;
                _yatse2Properties.DiaporamaImage1 = GetRandomImagePathNew(_config.FanartDirectory);
                var stbDiaporamaSwap = (Storyboard)TryFindResource("stb_Diaporama_21_1");
                if (stbDiaporamaSwap != null)
                    stbDiaporamaSwap.Begin(this);
            }
        }
        private void InitDatabase()
        {            
            _database.SetDebug(_config.Debug);
            _database.Open(null,_config.IgnoreSortTokens,_config.SortTokens);
            
            var check = _database.CheckDBVersion();
            if (check == 1) return;
            if (check == 0)
            {
                Logger.Instance().Log("FrontView+", "Database Create");
                _database.CreateDatabase();
                return;
            }
            Logger.Instance().Log("FrontView+", "Database Update");
            
            if ( _database.UpdateDatabase() == true)
            {
                Logger.Instance().Log("FrontView+", "Database Updated - Running Full Refresh.....");
               // RefreshLibrary();
                // Not connected - so can't run this here - could set variable - but will see
            }

        }

        private void InitTimer()
        {
            Logger.Instance().Log("FrontView+", "Init Timer");
            var dispatchTimer = new DispatcherTimer(DispatcherPriority.Normal);
            dispatchTimer.Tick += Timer_Tick;
            dispatchTimer.Interval = new TimeSpan(0, 0, 1);
            dispatchTimer.Start();
        }

        private static bool GetUpdater()
        {
            using (var client = new WebClient())
            {
                try
                {
                    if (File.Exists(Helper.AppPath + @"Temp\Yatse2Setup.exe"))
                        File.Delete(Helper.AppPath + @"Temp\Yatse2Setup.exe");
                    client.DownloadFile(Repository + "/Download/File/Yatse2Setup.exe",
                                        Helper.AppPath + @"Temp\Yatse2Setup.exe");
                }
                catch (WebException)
                {
                    return false;
                }
            }
            return true;
        }

        static string RemoveDiacritics(string stIn)
        {
            var stFormD = stIn.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var t in stFormD)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(t);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(t);
                }
            }
            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        static string RemoveDiacriticsUpper(string stIn)
        {
            return RemoveDiacritics(stIn).ToUpperInvariant();
        }

        /*private static bool IsFavorite(object obj)
        {
            return ((Yatse2Media)obj).IsFavorite > 0;
        }*/

        public bool FilterMovies(object item)
        {
            var gg = item as Yatse2Movie;
            if (_videoFavoritesFilter)
                return (gg != null) && RemoveDiacriticsUpper(gg.Title).Contains(_filterMovie.ToUpperInvariant()) &&
                       (gg.IsFavorite > 0);

            return (gg != null) && RemoveDiacriticsUpper(gg.Title).Contains(_filterMovie.ToUpperInvariant());
        }

        public bool FilterTvShows(object item)
        {
            var gg = item as Yatse2TvShow;
            if (_videoFavoritesFilter)
                return (gg != null) && RemoveDiacriticsUpper(gg.Title).Contains(_filterTvShow.ToUpperInvariant()) &&
                       (gg.IsFavorite > 0);
            return (gg != null) && RemoveDiacriticsUpper(gg.Title).Contains(_filterTvShow.ToUpperInvariant());
        }

        public bool FilterAudioArtist(object item)
        {
            var gg = item as Yatse2AudioArtist;
            if (_audioFavoritesFilter)
                return (gg != null) && RemoveDiacriticsUpper(gg.Name).Contains(_filterAudioArtist.ToUpperInvariant()) &&
                       (gg.IsFavorite > 0);
            return (gg != null) && RemoveDiacriticsUpper(gg.Name).Contains(_filterAudioArtist.ToUpperInvariant());
        }

        public bool FilterAudioGenre(object item)
        {
            var gg = item as Yatse2AudioGenre;
            if (_audioFavoritesFilter)
                return (gg != null) && RemoveDiacriticsUpper(gg.Name).Contains(_filterAudioGenre.ToUpperInvariant()) &&
                       (gg.IsFavorite > 0);
            return (gg != null) && RemoveDiacriticsUpper(gg.Name).Contains(_filterAudioGenre.ToUpperInvariant());
        }

        public bool FilterAudioAlbum(object item)
        {
            var gg = item as Yatse2AudioAlbum;
            if (_audioFavoritesFilter)
                return (gg != null) && RemoveDiacriticsUpper(gg.Title).Contains(_filterAudioAlbum.ToUpperInvariant()) &&
                       (gg.IsFavorite > 0);
            return (gg != null) && RemoveDiacriticsUpper(gg.Title).Contains(_filterAudioAlbum.ToUpperInvariant());
        }

        private void ShowOkDialog(string message)
        {
            ModalDialog.ShowDialog(message);
        }

        private bool ShowYesNoDialog(string message)
        {
            return ModalDialog.ShowYesNoDialog(message);
        }
        
        private static void PreInit()
        {
            ServicePointManager.CheckCertificateRevocationList = false;
            ServicePointManager.DnsRefreshTimeout = 4 * 3600 * 1000;
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.DefaultConnectionLimit = 10000; 
            Directory.CreateDirectory(Helper.LogPath);
            Directory.CreateDirectory(Helper.CachePath);
            Directory.CreateDirectory(Helper.CachePath + @"Video");
            Directory.CreateDirectory(Helper.CachePath + @"Music");
            Directory.CreateDirectory(Helper.CachePath + @"Weather");
            Directory.CreateDirectory(Helper.CachePath + @"Video\Thumbs");
            Directory.CreateDirectory(Helper.CachePath + @"Video\Fanarts");
            Directory.CreateDirectory(Helper.CachePath + @"Music\Thumbs");
            Directory.CreateDirectory(Helper.CachePath + @"Music\Artists");
            Directory.CreateDirectory(Helper.CachePath + @"Music\Fanarts");
        }

        private void StartReceiverServer()
        {
            Logger.Instance().Log("Receiver", "Start - Starting Server Thread... ", true);
            Thread receiverthread = new Thread(ReceiverServer) { IsBackground = true };
            receiverthread.IsBackground = true;
            receiverthread.Start();
        }

        private void ReceiverServer()
        {
            IPHostEntry ipHostInfo = Dns.Resolve(_config.ReceiverIP);
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            receiver.Connect(ipAddress, _config.ReceiverPort);

            if (receiver.SocketConnected() == false)
            {
                _config.UseReceiverIPforVolume = false;
                receiver.Disconnect();

            }
            else
            {
                receiver.TurnOn();
                receiver.QueryVolume();
                receiver.QueryMute();
            }
        }

        
        private void StartServer()
        { 
            Logger.Instance().Log("SERVER", "STARTSERVER - Starting Server Thread... ", true);
            //Logger.Instance().LogDump("SERVER THREAD    : Attempting to start new Thread ", true);
            Thread t = new Thread(NewThread) { IsBackground = true };
            t.IsBackground = true;
            t.Start();
            
            Logger.Instance().Log("SERVER", "STARTSERVER after thread started... ", true);
        }

        private void NewThread()
        {

            IPAddress localAdd = IPAddress.Parse(_config.IPAddress);
            TcpListener listener = new TcpListener(IPAddress.Any, _config.IPPort);
            Logger.Instance().Log("SERVER", "Within New Thread running Listener... ", true);
            listener.Start();
           // _config.FanartCurrentPath = null;

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream nwStream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];
                int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
                string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
               // Logger.Instance().LogDump("FrontView FANART    : Timer Result", _timer);
                
                Logger.Instance().LogDump("SERVER", "Data Received  " + dataReceived, true);
                

                // Receive data from Kodi thread to deal with theme.mp3
                // Basically checks for onplaybackstarted info which is sent when Kodi is playinbg
                // occurs on every playback - but when theme started - stops sending ListItem.Path info and sends
                // playback info for theme.
                // At Remote End Kodi ignores theme files so no NowPlaying screen for them
                // BUT - resorts to generic fanart as the Fanart Path info that was sent is no longer sent
                // This change basically ignores sent info from Kodi via Plugin -- if a onstartplayback event noted
                // Phew.
                // Also does not reset FanartCurrentPath info every time run / second / hopefully no unforseen issues
               
                if (!dataReceived.Contains(@"<event>onplaybackstarted</event>")) 
                {
                    Logger.Instance().LogDump("SERVER","NOT Playback Started Event - change Fanart as required", true);
                    _config.FanartCurrentPath = dataReceived;
                }
                
                
                
                //  onfig.FanartCurrentPath = dataReceived;
                // Console.WriteLine("The resulting messages on the server" + dataReceived);
                //  nwStream.Write(buffer, 0, bytesRead);
               // Console.WriteLine("\n");
                client.Close();
            }
            //   listener.Stop();
        }

        private void UpdateText(string message)
        {
            Logger.Instance().Log("SERVER",message, true);
        }


        private void InitProperties()
        {

            _yatse2Properties = TryFindResource("Yatse2Properties") as Yatse2Properties;
            _moviesDataSource = TryFindResource("MoviesDataSource") as MoviesCollection;
            _moviesCollectionView =
                (CollectionView)CollectionViewSource.GetDefaultView(lst_Movies_flow.ItemsSource);
            _tvShowsDataSource = TryFindResource("TvShowsDataSource") as TvShowsCollection;
            _tvShowsCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(lst_TvShows_flow.ItemsSource);
            _tvSeasonsDataSource = TryFindResource("TvSeasonsDataSource") as TvSeasonsCollection;
            _tvSeasonsCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(lst_TvSeasons_flow.ItemsSource);
            _tvEpisodesDataSource = TryFindResource("TvEpisodesDataSource") as TvEpisodesCollection;
            _tvEpisodesCollectionView =
                (CollectionView)CollectionViewSource.GetDefaultView(lst_TvEpisodes_flow.ItemsSource);
            _audioAlbumsDataSource = TryFindResource("AudioAlbumsDataSource") as AudioAlbumsCollection;
            _audioAlbumsCollectionView =
                (CollectionView)CollectionViewSource.GetDefaultView(lst_AudioAlbums_flow.ItemsSource);
            _audioArtistsDataSource = TryFindResource("AudioArtistsDataSource") as AudioArtistsCollection;
            _audioArtistsCollectionView =
                (CollectionView)CollectionViewSource.GetDefaultView(lst_AudioArtists_flow.ItemsSource);
            _audioGenresDataSource = TryFindResource("AudioGenresDataSource") as AudioGenresCollection;
            _audioGenresCollectionView =
                (CollectionView)CollectionViewSource.GetDefaultView(lst_AudioGenres_flow.ItemsSource);
            _audioSongsDataSource = TryFindResource("AudioSongsDataSource") as AudioSongsCollection;

            _moviesCollectionView.Filter = new Predicate<object>(FilterMovies);
            _tvShowsCollectionView.Filter = new Predicate<object>(FilterTvShows);
            _audioAlbumsCollectionView.Filter = new Predicate<object>(FilterAudioAlbum);
            _audioArtistsCollectionView.Filter = new Predicate<object>(FilterAudioArtist);
            _audioGenresCollectionView.Filter = new Predicate<object>(FilterAudioGenre);
        }
        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);

            //Loop through the running processes in with the same name 
            foreach (Process process in processes)
            {
                //Ignore the current process 
                if (process.Id != current.Id)
                {
                    //Make sure that the process is running from the exe file. 
                    if (Assembly.GetExecutingAssembly().Location.
                         Replace("/", "\\") == current.MainModule.FileName)
                    {
                        //Return the other process instance.  
                        return process;

                    }
                }
            }
            //No other instance was found, return null.  
            return null;
        }

        private void Init()
        {
            
            try
            {
                PreInit();
                // Attempting to start server socket on seperate thread
                
                
                //Change refresh rate to limit CPU usage - try off
                /*
                Timeline.DesiredFrameRateProperty.OverrideMetadata(
                   typeof(Timeline),
                   new FrameworkPropertyMetadata { DefaultValue = 20 }
                   );
                */
                
                Logger.Instance().Log("SERVER", "Starting Server Thread... ",true);
                


                var assem = Assembly.GetEntryAssembly();
                var assemName = assem.GetName();
                var ver = assemName.Version;



                Logger.Instance().Log("FrontView+", "Starting version :" + ver.Major +"." + ver.Minor, true);
                Logger.Instance().Log("FrontView+","Starting build : " + ver.Build,true);


             //   Logger.Instance().Log("FrontView+", "Starting Major : " + ver.Major, true);
             //   Logger.Instance().Log("FrontView+", "Starting MajorRevision : " + ver.MajorRevision, true);
             //   Logger.Instance().Log("FrontView+", "Starting Minor : " + ver.Minor, true);
             //   Logger.Instance().Log("FrontView+", "Starting MinorRevision : " + ver.MinorRevision, true);
             //   Logger.Instance().Log("FrontView+", "Starting Revision : " + ver.Revision, true);

                Logger.Instance().Log("OSInfo", "Name = " + OSInfo.Name, true);
                Logger.Instance().Log("OSInfo", "Edition = " + OSInfo.Edition, true);
                Logger.Instance().Log("OSInfo", "Service Pack =" + OSInfo.ServicePack, true);
                Logger.Instance().Log("OSInfo", "Version = " + OSInfo.VersionString, true);
                Logger.Instance().Log("OSInfo", "Bits = " + OSInfo.RealBits, true);
                Logger.Instance().Log("OSInfo", "Culture = " + Thread.CurrentThread.CurrentCulture.Name, true);
                Logger.Instance().Log("FrontView+ Debug :", "Checking for another instance", true);
              
                if (Yatse2Window.RunningInstance() != null)
                {
                    Logger.Instance().Log("NEW FrontView Debug:", "Duplicate FrontView+ Running Closing... ");
                    System.Windows.MessageBox.Show("Duplicate Instance of FrontView+, Closing....");
                    System.Windows.Application.Current.Shutdown();

                    //TODO:
                    //Your application logic for duplicate 
                    //instances would go here.
                }
                //load local kodi source xml file to get directories base
                //trial
                LoadKodiSource();

                _config.Load(_configFile);
                _timerScreenSaver = _config.ScreensaverTimer;
                Logger.Instance().Debug = _config.Debug;
                Logger.Instance().DebugTrace = _config.DebugTrace;

                  
                
                Logger.Instance().Log("FrontView+", "End load config");
                Logger.Instance().LogDump("FrontView",_config);

                ApiHelper.Instance().LoadRemotePlugins(ver.Build);

                _currentRemoteId = _config.DefaultRemote;
                _currentGrid = grd_Home;


                if (_config.HideCursor)
                    Cursor = System.Windows.Input.Cursors.None;

                InitProperties();

                if (_yatse2Properties != null)
                {
                    Helper.Instance.CurrentSkin = _config.Skin;
                    _yatse2Properties.Skin = _config.Skin;
                    _yatse2Properties.Language = _config.Language;
                    _yatse2Properties.ShowHomeButton = false;
                    _yatse2Properties.DimAmount = _config.DimAmount;
                    
                    _yatse2Properties.Weather = new Yatse2Weather();
                    _yatse2Properties.Currently = new Yatse2Currently
                                                        {
                                                            IsNotMovieDetails = true,
                                                            IsNotMusicDetails = true,
                                                            IsNotTvDetails = true
                                                        };
                    RefreshDictionaries();
                    _yatse2Properties.Currently.HideAudioMenu = _config.ShowAudioMenu;
                }

                InitDatabase();
                InitWeather();
                InitRemote();
                InitTimer();
                RefreshRemotes();

                RefreshHeader();

     
                
                System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
                string sPath2Icon = Path.Combine(Helper.AppPath, "Yatse2.ico");

                if (_config.ShowInTaskbar == false)
                {
                this.ShowInTaskbar = false;
                }

                ni.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
                ni.BalloonTipText = "FrontView+ MinimiseAlways is On";
                ni.BalloonTipTitle = "FrontView+";
                ni.Text = "FrontView+ Click to Enable/Disable Minimise Always";
                ni.Icon = new System.Drawing.Icon(sPath2Icon);
                ni.Visible = true;


                ni.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
                
                Logger.Instance().Log("Taskbar:","Create new Taskbar Icon located: " + sPath2Icon);

                if (!_config.DisableAnimations)
                {
                    trp_Transition.Transition = TryFindResource("GridTransition") as FluidKit.Controls.Transition;
                }
                else
                {
                    trp_Transition.Transition = new FluidKit.Controls.NoTransition();
                }

                //disable automatic resolution dection - except at start of


                if (!_config.DisableResolutionDetection && !_config.MinimiseAlways)
                {
                    Microsoft.Win32.SystemEvents.DisplaySettingsChanged += Change_Display_Settings;
                }
                
                Change_Display_Settings(null, null);
                

                if (_config.StartFrontViewServer)
                {
                    StartServer();
                }
                HttpisStopped = true;

                CheckSilentUpdate();


                if (_config.HttpSend == true && _config.HttpPoweron != "")
                {
                    Logger.Instance().Log("HttpSend", "PowerON URL " + _config.HttpPoweron + "Sent.");
                    gotoHttpsimple(_config.HttpPoweron);
                }

                if (!_config.StartFrontViewServer)
                {
                    Logger.Instance().Log("SERVER:", "Start FrontView Server FALSE - SERVER NOT STARTED");
                    _config.FanartCurrentPath = null;
                }

                if (_config.UseReceiverIPforVolume)
                {
                    try
                    {

                        StartReceiverServer();
                    }
                    catch (Exception e)
                    {
                        Logger.Instance().Log("RECEIVER FAIL", "Exception in attempted connection " + e);
                    }

                 
                    
                }

            }
            catch (Exception e)
            {
                 
                Logger.Instance().LogException("FrontView2Init",e);
           
                //System.Windows.Application.Current.Shutdown();
                Logger.Instance().Log("FrontViewInit","Forcing close");
                System.Windows.Application.Current.Shutdown();
            }

            Logger.Instance().Log("FrontView+", "End init", true);
           
        }

        //load Kodi Source xml and populate values to be checked against
        //working

        public void CheckSilentUpdate()
        {
            try
            {

                var appPath = Helper.AppPath + @"wyUpdate.exe";
                Logger.Instance().Trace("FrontView+", "CheckSilent appPath equals" + appPath);
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = appPath;
                psi.Arguments = "/quickcheck /justcheck /noerr";
                var proc = Process.Start(psi);
                               
                proc.WaitForExit();
                var ExitCode = proc.ExitCode;
                Logger.Instance().Trace("FrontView+", "CheckSilent Update Returned:" + ExitCode.ToString());
                
                if (ExitCode == 2)
                {
                    UpdateAvailable = true;
                }

            
            }
            catch (Exception e)
            {
                Logger.Instance().Log("FrontView+", "Exception in Silent Update Check" + e);
            }
            
        }


        private void LoadKodiSource()
        {

              
            try
            {

                Logger.Instance().Log("Kodi Source", "Checking for Kodi Source xml file", true);
                var appdatadirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                XmlDocument kodisource = new XmlDocument();
                kodisource.Load(@appdatadirectory + @"\Kodi\userdata\sources.xml");
                XmlNodeList KodiDirectories = kodisource.GetElementsByTagName("path");
                //string[] KodiSources = new string[20];
                
                //Changes below
                
                


                int i = 0;
                foreach (XmlNode node in KodiDirectories)
                {

                    Logger.Instance().Log("Load Kodi Source", "Xml Data ==  " + node.InnerText, true);
                    KodiSourceData.KodiSources[i] = SortOutPath(node.InnerText);
                    Logger.Instance().Log("Load Kodi Source", "KodiSources Array " + i + "  " + KodiSourceData.KodiSources[i], true);
                    i++;

                }


                XmlNodeList KodiMusic = kodisource.SelectNodes("sources/music/source");
                i = 0;
                foreach (XmlNode node in KodiMusic)
                {

                    Logger.Instance().Log("Load Kodi Music Source", "Xml Data ==  " + node["path"].InnerText, true);
                    KodiSourceData.KodiMusicSources[i] = SortOutPath(node["path"].InnerText);
                    Logger.Instance().Log("Load Kodi Music Source", "KodiSources Array " + i + "  " + KodiSourceData.KodiMusicSources[i], true);
                    i++;
                    

                }



            }
            catch (Exception e)
            {
                if (e is FileNotFoundException )
                {
                    Logger.Instance().Trace(" Kodi Sources Not Found. "," Continue.");
                }
                if (e is DirectoryNotFoundException)
                {
                    Logger.Instance().Trace(" Kodi Directory Sources Not Found. ", " Continue.");
                }
                
                Logger.Instance().Trace("Kodi Source Error:","Not important unless using Kodi :"+ e);
            }

        }

        static string SortOutPath(string path)
        {
            try
            {
                if (String.IsNullOrEmpty(path))
                {
                    Logger.Instance().LogDump("SortOUT", "path Empty set to null  " + path, true);
                    return "";
                }

                if (path.Length >= 4 && TruncatePath(path, 4) == "smb:")
                {
                    //Okay - this runs if path starts with Smb: and converts to UNC path format.  Which means System.IO.path commands run correctly.
                    Logger.Instance().LogDump("SortOUT", "path Smb changing to UNC " + path, true);
                    char[] MyChar2 = { 's', 'm', 'b', ':' };
                    path = path.TrimStart(MyChar2);
                    Logger.Instance().LogDump("SortOUT", "Path Smb Begining Trimmed " + path, true);
                    path = path.Replace(@"/", @"\");
                    Logger.Instance().LogDump("SortOUT", "Escapes replaced Final Result:" + path, true);
                    return path;
                }
                Logger.Instance().LogDump("SortOUT", "Neither End Result :" + path, true);

                return path;
            }
            catch (Exception ex)
            {
                Logger.Instance().LogDump("SortOUT", "Exception caught " + ex, true);
                return "";
            }
        }

        static string TruncatePath(string str, int maxLength)
        {
            return str.Substring(0, Math.Min(str.Length, maxLength));
        }


        private void PositionScreen()
        {

            if (!_setPov)
            {
                _yatse2Properties.TvShowPosterPov = (lst_TvShows_flow.ActualWidth/lst_TvShows_flow.ActualHeight)/10*85;
                _yatse2Properties.VideoPov = (lst_Movies_flow.ActualWidth/lst_Movies_flow.ActualHeight)/10*85;
                _yatse2Properties.AudioGenresPov = (lst_AudioGenres_flow.ActualWidth/lst_AudioGenres_flow.ActualHeight)/
                                                   10*85;
                _yatse2Properties.AudioAlbumsPov = (lst_AudioAlbums_flow.ActualWidth/lst_AudioAlbums_flow.ActualHeight)/
                                                   10*85;
                _yatse2Properties.AudioArtistsPov = (lst_AudioArtists_flow.ActualWidth/
                                                     lst_AudioArtists_flow.ActualHeight)/10*85;
                _setPov = true;
            }

            if (_config.DisableScreenPositioning)
                return;

            if (_config.MouseMode)
                return;

            var dx = 1.0;
            var dy = 1.0;
            var temp = PresentationSource.FromVisual(this);

            if (temp != null)
            {
                if (temp.CompositionTarget != null)
                {
                    var m = temp.CompositionTarget.TransformToDevice;
                    dx = m.M11;
                    dy = m.M22;
                }
            }
            var screens = System.Windows.Forms.Screen.AllScreens;
            if (screens.Length == 1 || !_config.SecondScreen)
            {
                if (Top != 0 || Left != 0)
                {
                    Top = 0;
                    Left = 0;
                }
            }
            else
            {
                foreach (var scr in
                    screens.Where(scr => !scr.Primary).Where(scr => Top != (scr.Bounds.Top / dy) || Left != (scr.Bounds.Left / dx)))
                {
                    Top = scr.Bounds.Top / dy;
                    Left = scr.Bounds.Left / dx;
                    break;
                }
            }

        }

        private void CheckUpdate(bool showResult)
        {
            var assem = Assembly.GetEntryAssembly();
            var assemName = assem.GetName();
            var ver = assemName.Version;
            var platform = "x86";
            if (Tools.Is64BitProcess)
                platform = "x64";
            Logger.Instance().Log("FrontView+", "Checking for updates. Current version : " + ver, true);
            _allowBeta = File.Exists(Helper.AppPath + "Yatse2.beta");

            var repo = new RemoteRepository();
            repo.SetDebug(_config.Debug);
            if (!repo.LoadRepository(Repository, platform, Helper.AppPath + "Updates"))
            {
                if (showResult)
                    ShowOkDialog(GetLocalizedString(114));
                return;
            }
                
            var result = repo.UpdateTranslations(Helper.LangPath);
            if (result)
            {
                if (showResult)
                    ShowOkDialog(GetLocalizedString(115));
                RefreshDictionaries();
            }
            var versions = repo.GetBuildList(_allowBeta);
            if (versions == null)
            {
                Logger.Instance().Log("FrontView+", "Build list empty !", true);
                if (showResult)
                    ShowOkDialog(GetLocalizedString(114));
            }
            else
            {
                if (versions.Version.Count < 1)
                {
                    Logger.Instance().Log("FrontView+", "Build list empty !", true);
                    if (showResult)
                        ShowOkDialog(GetLocalizedString(114));
                }
                else
                {
                    var lastBuild = versions.Version[versions.Version.Count - 1];

                    if (ver.Build >= lastBuild.Build)
                    {
                        Logger.Instance().Log("FrontView+", "Version is up2date!", true);
                        if (showResult)
                            ShowOkDialog(GetLocalizedString(113));
                    }
                    else
                    {
                        Logger.Instance().Log("FrontView+", "Update available : " + lastBuild.Build, true);
                        var res = ShowYesNoDialog(GetLocalizedString(109));
                        if (res)
                        {
                            Directory.CreateDirectory(Helper.AppPath + "Temp");
                            if (GetUpdater())
                            {
                                Process.Start(Helper.AppPath + @"Temp\Yatse2Setup.exe");
                                Close();
                            }
                            else
                            {
                                ShowOkDialog(GetLocalizedString(101));
                            }
                        }

                    }
                }
            }
            repo.CleanTemporary();
        }
        
        private string GetFanartDirectory(string pathname)
        {
            try
            {
                foreach (string path in KodiSourceData.KodiSources)
                {
                    Logger.Instance().LogDump("XML", "XML Data from foreach  " + path, true);


                    if (path == null)
                    {
                        return null;
                    }

                    if (path == pathname)
                    {
                        return path;
                    }

                    if (pathname.Contains(path) == true)
                    {
                        Logger.Instance().LogDump("XML", "Contains equals true for   " + pathname + " and path " + path, true);

                        string PathDifference = pathname.Replace(path, "");

                        string DirectoriesLeft = Path.GetDirectoryName(PathDifference);

                        string[] elements = DirectoriesLeft.Split('\\');
                        string result = elements[0];

                        string ExtrafanartDirectory = path + result + @"\";
                        Logger.Instance().LogDump("XML", "Returned ExtraFanArt Directory=" + ExtrafanartDirectory, true);
                        return ExtrafanartDirectory;

                    }

                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.Instance().LogDump("XML", "Exception caught" + ex, true);
                return null;
            }
        }

        private string ReturnContainingSource(string pathname)
        {
            try
            {
                foreach (string path in KodiSourceData.KodiSources)
                {
                    Logger.Instance().LogDump("XML", "XML Data from foreach  " + path, true);


                    if (path == null)
                    {
                        return null;
                    }

                    if (path == pathname)
                    {
                        return path;
                    }

                    if (pathname.Contains(path) == true)
                    {
                        Logger.Instance().LogDump("ContainingSource", "Contains equals true for   " + pathname + " and path " + path, true);


                        return path;

                    }

                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.Instance().LogDump("XML", "Exception caught" + ex, true);
                return null;
            }
        }
        private string cleanPath(string toCleanPath, string replaceWith = "-")
        {
            try
            {
                //get just the filename - can't use Path.GetFileName since the path might be bad!  
                string[] pathParts = toCleanPath.Split(new char[] { '\\' });
                string newFileName = pathParts[pathParts.Length - 1];
                //get just the path  
                string newPath = toCleanPath.Substring(0, toCleanPath.Length - newFileName.Length);
                //clean bad path chars  
                foreach (char badChar in Path.GetInvalidPathChars())
                {
                    newPath = newPath.Replace(badChar.ToString(), replaceWith);
                }
                //clean bad filename chars  
                foreach (char badChar in Path.GetInvalidFileNameChars())
                {
                    newFileName = newFileName.Replace(badChar.ToString(), replaceWith);
                }
                //remove duplicate "replaceWith" characters. ie: change "test-----file.txt" to "test-file.txt"  
                if (string.IsNullOrEmpty(replaceWith) == false)
                {
                    newPath = newPath.Replace(replaceWith.ToString() + replaceWith.ToString(), replaceWith.ToString());
                    newFileName = newFileName.Replace(replaceWith.ToString() + replaceWith.ToString(), replaceWith.ToString());
                }
                //return new, clean path:  
                return newPath + newFileName;
            }
            catch (Exception ex)
            {
                Logger.Instance().LogDump("cleanPath", "Exception caught" + ex, true);
                return toCleanPath;
            }
        }  

        static bool IsFileURI(String path)
        {
            try
            {
                Path.GetDirectoryName(path);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Instance().LogDump("IsFileURI", "Fails Path Test" + path , true);
                return false;

            }
        }
            

       /* Have to sort out this later - presently will not deal with none smb:\\ paths.
        * private static string MakeValidFileName(string name)
        {
            string illegal = "\"M\"\\a/ry/ h**ad:>> a\\/:*?\"| li*tt|le|| la\"mb.?";
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            illegal = r.Replace(illegal, "");
        }*/

        private void CheckAudioFanart()
        {
            var nowPlaying2 = _remote != null ? _remote.Player.NowPlaying(false) : new ApiCurrently();
            Logger.Instance().LogDump("theme.mp3", "NowPlaying Filename equals:" + nowPlaying2.FileName);

            var appdatadirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var FanartDirectory = appdatadirectory + @"\Kodi\userdata\"; 
            
            if (nowPlaying2.FileName.EndsWith("theme.mp3"))
            {
                string CurrentPath = SortOutPath(nowPlaying2.FileName);
                CurrentPath = cleanPath(CurrentPath, string.Empty);
                Logger.Instance().LogDump("theme.mp3 Playing", "Current Path Equals:" + CurrentPath, true);
                string CurrentPath2 = GetFanartDirectory(CurrentPath);
                Logger.Instance().LogDump("theme.mp3 Playing", "CurrentPath2 now Equals:" + CurrentPath2, true);
                _config.FanartDirectory = CurrentPath2 + @"extrafanart\";

                if (GetRandomImagePathNew(_config.FanartDirectory) == null)
                {
                    _config.FanartDirectory = FanartDirectory + _config.FanartDirectoryTV;
                }
                
                
                return;
            }

     

            Logger.Instance().LogDump("FrontView Check AudioFanart    : Audio Playing", nowPlaying2.CurrentMenuLabel, true);

            if (grd_Diaporama.Visibility == Visibility.Hidden)
            {
                var stbDiaporamaShow = (Storyboard)TryFindResource("stb_ShowDiaporama");
                if (stbDiaporamaShow != null)
                {
                    stbDiaporamaShow.Begin(this);
                }
            }



            var ArtistExtrafanart = KodiSourceData.KodiMusicSources[0] + nowPlaying2.Artist + @"\extrafanart\";

            foreach (var musicsource in KodiSourceData.KodiMusicSources)
            {
                if (musicsource != null)
                {
                    var testaudiofanartcheck = musicsource + nowPlaying2.Artist + @"\extrafanart\";
                    Logger.Instance().LogDump("UpdateAUDIO ARRAY", "Checking all sources " + testaudiofanartcheck);
                    if (System.IO.Directory.Exists(testaudiofanartcheck))
                    {
                        Logger.Instance().LogDump("UpdateAUDIO ARRAY", "Directory Exists Usings - No check for contents though " + testaudiofanartcheck);
                        ArtistExtrafanart = testaudiofanartcheck;
                        break;
                    }
                }


            }
            

           // Change to kodiMusic.Source Usage
           //var ArtistExtrafanart = KodiSourceData.KodiMusicSources[0] + nowPlaying2.Artist + @"\extrafanart\";
            Logger.Instance().LogDump("MUSIC", "Fanart File Artist Name:  " + nowPlaying2.Artist, true);
            Logger.Instance().LogDump("MUSIC", "Fanart File   " + ArtistExtrafanart, true);
            _config.FanartDirectory = ArtistExtrafanart;
            Logger.Instance().LogDump("MUSIC", "Fanart location    " + _config.FanartDirectory, true);
            if (GetRandomImagePathNew(_config.FanartDirectory)==null)
            {
               Logger.Instance().LogDump("MUSIC", "CheckAudio Defaulting  " + nowPlaying2.Artist, true);
               _config.FanartDirectory = FanartDirectory + _config.FanartDirectoryMusic;
            }

       }

        private void CheckFanArt()
        {
            var nowPlaying2 = _remote != null ? _remote.Player.NowPlaying(false) : new ApiCurrently();
            var FanartAlways = _config.FanartAlways;
            //_config.FanartDirectory = null;
            int numberofdirectoriesdeep = _config.FanartNumberDirectories;
            var appdatadirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var FanartDirectory = appdatadirectory + @"\Kodi\userdata\"; 


            Logger.Instance().LogDump("FrontView FANART    : Check FanART Run & Current Menu prior", nowPlaying2.CurrentMenuLabel, true);

            Logger.Instance().LogDump("FrontView FANART    : Check Remote Type if EMBY Dont check any further set to Default", nowPlaying2.CurrentMenuLabel, true);

            if (_remote.GetOS()=="Emby" && FanartAlways == true)
            {
                Logger.Instance().LogDump("Frontview FANART", "Emby is Remote Connection - Ignoring Relevant Fanart", true);

                Logger.Instance().LogDump("Frontview FANART", "Emby: Path to Fanart " + _config.FanartDirectory, true);

                // Emby No point currently to do menu checks as will not work
                // Need to figure out the path however - change path if not correct - enable compatiblity with both
                if ( GetRandomImagePathNew(_config.FanartDirectory) == null  )
                {
                    _config.FanartDirectory = FanartDirectory + _config.FanartDirectoryMovie;
                    Logger.Instance().LogDump("Frontview FANART", "Default Fanart Directory is null - Add Directory", true);
                    
                }
                
            }


            if (grd_Diaporama.Visibility == Visibility.Hidden && nowPlaying2.CurrentMenuID != "10004")
            {
                var stbDiaporamaShow = (Storyboard)TryFindResource("stb_ShowDiaporama");
                if (stbDiaporamaShow != null)
                {
                    stbDiaporamaShow.Begin(this);
                }
            }

            
            if (FanartAlways == true)
            {
                
                if (nowPlaying2.FileName=="IGNORE")
                {
                    Logger.Instance().LogDump("IGNORE", "IGNORE given ignoring Fanart Socket", true);
                    return;
                }
                string CurrentPath = SortOutPath(_config.FanartCurrentPath);
                CurrentPath = cleanPath(CurrentPath, string.Empty);

            //    var appdatadirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //    var FanartDirectory = appdatadirectory + @"\Kodi\userdata\"; 

               Logger.Instance().LogDump("SERVER", "Fanart Directory from Socket =:" + _config.FanartCurrentPath, true);
               Logger.Instance().LogDump("SERVER", "Fanart Directory NORMALISED =:" + CurrentPath, true);

               if (IsFileURI(CurrentPath) == true)
               {
                   if (nowPlaying2.CurrentMenuID == "10025")
                   {
                       try
                       {
                           string CurrentPath2 = CurrentPath;
                           Logger.Instance().LogDump("SERVER", "Video Directory Socket returned path - CurrentPath2 equals  " + @CurrentPath2, true);

                           string CurrentPath3 = GetFanartDirectory(CurrentPath2);
                           _config.FanartDirectory = @CurrentPath3 + @"extrafanart\";
                           Logger.Instance().LogDump("SERVER", "FanartDirectory Performed and equals:" + _config.FanartDirectory, true);
                       }
                       catch (Exception ex)
                       {
                           Logger.Instance().LogDump("SERVER", "Fanart Video Menu 10025 - Exception occured   " + ex, true);
                           _config.FanartDirectory = FanartDirectory + _config.FanartDirectoryTV;
                       }

                   }

                   if (nowPlaying2.CurrentMenuID == "10002")
                   {
                       try
                       {

                           string CurrentPath2 = CurrentPath;
                           Logger.Instance().LogDump("SERVER", "Image Directory Selected - path equals  " + CurrentPath2, true);
                           _config.FanartDirectory = @CurrentPath2;
                           Logger.Instance().LogDump("SERVER", "Image Directory Selected & fanart equals  " + _config.FanartDirectory, true);
                           if (GetRandomImagePathNew(_config.FanartDirectory) == null) //Empty directory or root etc
                           {
                               _config.FanartDirectory = FanartDirectory + _config.FanartDirectoryMyImages;
                               Logger.Instance().LogDump("SERVER", "Image Directory & no Images: Reset to default :" + _config.FanartDirectory, true);
                           }
                       }
                       catch (Exception ex)
                       {
                           Logger.Instance().LogDump("SERVER", "Fanart Image - Exception occured   " + ex, true);
                           _config.FanartDirectory = FanartDirectory + _config.FanartDirectoryMyImages; 
                       }

                   }
               }
                // if no directory or no files afte above then move to default menu based settings                

             if (nowPlaying2.CurrentMenuID == "10025" && IsFileURI(CurrentPath) != true)
             {
                        _config.FanartDirectory = FanartDirectory + _config.FanartDirectoryMovie; // +@"MovieFanart\";
             }
                    
             if (nowPlaying2.CurrentMenuID == "10501")
             {
                        _config.FanartDirectory = FanartDirectory + _config.FanartDirectoryMusic; // +@"ArtistFanart\";

             }

             if (nowPlaying2.CurrentMenuID == "10501" )
             {
                         _config.FanartDirectory = FanartDirectory + _config.FanartDirectoryMusic;

             }
                
                   
                  //  if (nowPlaying2.CurrentMenuID == "10002")
                  //  {
                  //      _config.FanartDirectory = FanartDirectory + _config.FanartDirectoryMyImages; // +@"OwnFanart\";
                  //  }
             if (nowPlaying2.CurrentMenuID == "12600")
             {
                        _config.FanartDirectory = FanartDirectory + _config.FanartDirectoryWeather; // ppdatadirectory + @"\Kodi\userdata\addon_data\skin.aeonmq5.extrapack\backgrounds_weather\";
             
             }
             if (nowPlaying2.CurrentMenuID == "10000")  //Equals the home menu
             {
                       //
                     _config.FanartDirectory = FanartDirectory + _config.FanartDirectoryTV; 
             }
             if (nowPlaying2.CurrentMenuID == "10502")
             {
                        _config.FanartDirectory = FanartDirectory + _config.FanartDirectoryMusic; // +@"ArtistFanart\";

             }

                    //If directory empty and fanart show being displayed - change to default - which is Movies
             if (GetRandomImagePathNew(_config.FanartDirectory) == null && grd_Diaporama.Visibility != Visibility.Hidden )
             {
                        
                 
                 _config.FanartDirectory = FanartDirectory + _config.FanartDirectoryMovie;
             }
                



                
            if (nowPlaying2.CurrentMenuID == "10004" && grd_Diaporama.Visibility != Visibility.Hidden)
            {

                    var stbDiaporamaHide = (Storyboard)TryFindResource("stb_HideDiaporama");
                    if (stbDiaporamaHide != null)
                    {
                        stbDiaporamaHide.Begin(this);
                    }

            }

            }
        }
        
        
        
        static string BreakDirectory(string path3, int dirnumber)
        {

            string[] directories = path3.Split(Path.DirectorySeparatorChar);
            string previousEntry = string.Empty;
            var count = -2;
            if (null != directories)
            {
                foreach (string direc in directories)
                {
                    count++;
                    string newEntry = previousEntry + Path.DirectorySeparatorChar + direc;
                    if (!string.IsNullOrEmpty(newEntry))
                    {
                        if (!newEntry.Equals(Convert.ToString(Path.DirectorySeparatorChar), StringComparison.OrdinalIgnoreCase))
                        {

                            previousEntry = newEntry;
                        }
                    }

                    if (count == dirnumber)
                    {
   
                        return @"\" + newEntry + @"\";
                    }
                }
            }
            return null;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            _timerHeader++;
            _timer++;
            Logger.Instance().LogDump("FrontView FANART    : Timer Result", _timer);
            
            UpdateRemote();
            
            Window glennwindow = Window.GetWindow(this);
            

            //if (_config.CheckForUpdate && !_updatecheck)
            //{
             //   _updatecheck = true;
              //  CheckUpdate(false);
            //}

            if (!_showHomePage)
            {
                ShowHome();
                _showHomePage = true;
            }

            if (_timerHeader > 15)
            {
                RefreshHeader();
                _timerHeader = 0;
            }
            var nowPlaying = _remote != null ? _remote.Player.NowPlaying(false) : new ApiCurrently();
            var GlennMinimise = (_config.MinimiseAlways);

            //Http Send Setup

            if ((_config.HttpSend))
            
            {
                Logger.Instance().LogDump("HttpSend", "Checking Playback Conditions isNewMedia: "+nowPlaying.IsNewMedia, true);

                if (nowPlaying.IsNewMedia == true)
                {
                    HttpisDelayed = false;
                }

                if (nowPlaying.IsPlaying == true && HttpisPlaying == false)
                {
                    gotoHttp(_config.HttpPlaystarted, nowPlaying);
                    HttpisPlaying = true;
                    HttpisPaused = false;
                    HttpisStopped = false;
                    

                }
                if (nowPlaying.IsPlaying== true && (nowPlaying.Time.Seconds >= _config.HttpPlayStartedDelay) && HttpisDelayed == false )
                {
                    gotoHttp(_config.HttpPlayStartedDelayed, nowPlaying);
                    HttpisDelayed = true;
                }
                
                if (nowPlaying.IsPaused == true && HttpisPaused == false)
                {
                    gotoHttp(_config.HttpPlaypaused, nowPlaying);
                    HttpisPaused = true;
                    HttpisPlaying = false;
                    HttpisStopped = false;
                }
                if (nowPlaying.IsMuted == true && HttpisMuted == false)
                {
                    gotoHttp(_config.HttpMute, nowPlaying);
                    HttpisMuted = true;
                }
                if (nowPlaying.IsPaused == false && nowPlaying.IsPlaying == false  && HttpisStopped == false)
                {
                    gotoHttp(_config.HttpPlaystopped, nowPlaying);
                    HttpisStopped = true;
                    HttpisPlaying = false;
                    HttpisPaused = false;
                }

                if (nowPlaying.IsMuted == false && HttpisMuted == true)
                {
                    gotoHttp(_config.HttpUnmute, nowPlaying);
                    HttpisMuted = false;
                }

                if (nowPlaying.IsNewMedia== true && nowPlaying.MediaType == "Video")
                {
                    gotoHttp(_config.HttpMediatypeVideo, nowPlaying);
                }
                if (nowPlaying.IsNewMedia == true && nowPlaying.MediaType == "Audio" && !nowPlaying.FileName.EndsWith("theme.mp3"))
                {
                    gotoHttp(_config.HttpMediatypeAudio, nowPlaying);
                }
               
            }

            //Logger.Instance().Log("FrontView+", "About to CALL CheckFanARt");
           // CheckFanArt();
            //Logger.Instance().Log("FrontView+", "After CALL CheckFanARt");


            if ((_timer > _config.DimmingTimer) && _config.Dimming && nowPlaying.IsPlaying && !nowPlaying.IsMuted)
            {
                if (!(!_yatse2Properties.Currently.IsTv && !_yatse2Properties.Currently.IsMovie && _config.DimmingOnlyVideo))
                {
                    if (grd_Dimming.Visibility != Visibility.Visible)
                    {
                        Logger.Instance().Log("FrontView+", "Start screen saver : Dimming here 2");
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
                        }
                    }
                }
                Logger.Instance().LogDump("FrontView FANART    : ResetTimer Run from 2", _timer);
                ResetTimer();
            }

            if (!nowPlaying.IsPaused && !nowPlaying.IsPlaying )
            {
                if (grd_Dimming.Visibility == Visibility.Visible && glennwindow.WindowState == WindowState.Normal)
                {
                    var stbDimmingShow = (Storyboard)TryFindResource("stb_HideDimming");
                    if (stbDimmingShow != null)
                        stbDimmingShow.Begin(this);
                    Logger.Instance().LogDump("FrontView NEW DEBUG:", "Playback Paused or Playing - Dim on Undim");
                }
                
                
                if (glennwindow.WindowState == WindowState.Normal)
                //     if (this.Visibility == Visibility.Visible)
                {
                    if (GlennMinimise == true)
                    {
                        notifyIcon1_DoubleClick(null, null);
                        // this.ShowInTaskbar = false;
                        // this.WindowState = WindowState.Minimized;
                        // Hide();
                        // this.ShowInTaskbar = false;
                        // _config.MinimiseAlways = true;
                        Logger.Instance().LogDump("NEW Yastse  Debug    : DBL click tasbar event/Normal Window, Minimise Window and set MinimiseAlways to true ", _config.MinimiseAlways);
                        // this.Activate();

                    }
                    if (!_isfanart && GlennMinimise == false && _config.FanartAlways == true && nowPlaying.CurrentMenuID != "10004" && !nowPlaying.IsPaused && !nowPlaying.IsPlaying && ( _timer % _config.FanartTimer) == 0 ) 
                    {
                         Logger.Instance().LogDump("FrontView FANART    : StartFanART Run & Fanart Timer result", _timer);
                         CheckFanArt();
                         StartFanart();
                         Logger.Instance().LogDump("FrontView FANART    : StartFanART Finsihed & _timer result", _timer);
                         //Fanart Routine shoudl go here
                        
                    }

                }
            }
                
            if (nowPlaying.IsPaused)
            {
                    Logger.Instance().LogDump("FrontView", "nowPlaying.Paused is called");    
                    if (grd_Dimming.Visibility == Visibility.Visible)
                    {
                        var stbDimmingShow = (Storyboard)TryFindResource("stb_HideDimming");
                        if (stbDimmingShow != null)
                            stbDimmingShow.Begin(this);
                        Logger.Instance().LogDump("FrontView NEW Debug:", "Playback Paused undim ",true);
                        Logger.Instance().LogDump("FrontView FANART    : ResetTimer Run from 3", _timer);
                        ResetTimer();
                   }

           }
           
            
           if (_config.FanartAlways== true && _config.Currently==false && !_isfanart && nowPlaying.IsPlaying && nowPlaying.MediaType=="Audio"  &&  ( _timer % _config.FanartTimer) == 0 ) 
           {
               CheckAudioFanart();
               StartFanart();
               Logger.Instance().LogDump("AUDIO", "nowPlaying playing Audio Starting fanart " + nowPlaying.MediaType, true);
           }

           if (nowPlaying.IsPlaying && nowPlaying.MediaType == "Audio" && nowPlaying.FileName.Contains("googleusercontent")&& (_yatse2Properties.Currently.Fanart == Helper.SkinorDefault(Helper.SkinPath,  _config.Skin,  @"\Interface\Default_Diaporama.png")) && (_timer % _config.FanartTimer) == 0)
           {
               Logger.Instance().LogDump("GOOGLEPLAY", "Playing, Media type is AUdio, googlesusercontent filename, Currently Fanart equals" + _yatse2Properties.Currently.Fanart, true);
               UpdateCurrently(nowPlaying);
           }
            
          if (nowPlaying.IsMuted)
          {
                    if (grd_Dimming.Visibility == Visibility.Visible)
                    {
                        var stbDimmingShow = (Storyboard)TryFindResource("stb_HideDimming");
                        if (stbDimmingShow != null)
                            stbDimmingShow.Begin(this);
                        Logger.Instance().LogDump("FrontView NEW Debug:", "Playback Muted undim ",true);
                        Logger.Instance().LogDump("FrontView FANART    : ResetTimer Run from nowPlaying.IsMuted ", _timer);
                        ResetTimer();
                    }
                    
          }

          if (_timer > _timerScreenSaver && !nowPlaying.IsPaused && _config.FanartAlways == false )
          {
                    Logger.Instance().LogDump("FrontView FANART    : StartScreen Saver Called", _timer);
                    StartScreensaver();
          }


          if (_isScreenSaver && _diaporamaCurrentImage != 0 && (_timer % _config.DiaporamaTimer) == 0)
          {
                    SwitchDiaporama();
          }
          
          if (glennwindow.WindowState == WindowState.Normal && _isfanart && _fanartCurrentImage != 0 && (_timer % _config.FanartTimer) == 0 && !nowPlaying.IsPlaying)
          {
                    CheckFanArt();
                    SwitchFanart();
                    _yatse2Properties.Currently.Fanart = GetVideoFanartPath(nowPlaying.FanartURL);
          }


//add theme.mp3 aspect
          if (glennwindow.WindowState == WindowState.Normal && _isfanart && _config.FanartAlways == true && _config.Currently == false && _fanartCurrentImage != 0 && (_timer % _config.FanartTimer) == 0 && nowPlaying.IsPlaying && nowPlaying.MediaType == "Audio" && !nowPlaying.FileName.EndsWith("theme.mp3"))
          {
              Logger.Instance().LogDump("AUDIO", "nowPlaying Switching Fanart" + nowPlaying.MediaType, true);
              CheckAudioFanart();
              SwitchFanart();

          }

          PositionScreen();

          CheckFirstLaunch();
          
        }
        

        public void gotoHttpsimple(string url)
        {
            try
            {
                 if (_config.HttpUseDigest == false)
                {
                    Logger.Instance().LogDump("HttpSimpleSend", "Using Basic: Url:  " + url, true);

                    var logon = _config.HttpUser;
                    var password = _config.HttpPassword;
                    var Auth = "Basic";


                    Logger.Instance().LogDump("HttpSimpleSend", "Using " + Auth + " Authorisation:   URL " + url, true);

                    WebRequest request = WebRequest.Create(url);
                    request.Method = WebRequestMethods.Http.Get;
                    NetworkCredential networkCredential = new NetworkCredential(logon, password); // logon in format "domain\username"
                    CredentialCache myCredentialCache = new CredentialCache { { new Uri(url), Auth, networkCredential } };
                    request.PreAuthenticate = true;
                    request.Credentials = myCredentialCache;
                    using (WebResponse response = request.GetResponse())
                    {

                        //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                        Logger.Instance().LogDump("HttpSimpleSend", "Response: " + url + " Response: " + (((HttpWebResponse)response).StatusDescription), true);
                        //using (Stream dataStream = response.GetResponseStream())
                        // {
                        //     using (StreamReader reader = new StreamReader(dataStream))
                        //     {
                        //         string responseFromServer = reader.ReadToEnd();
                        //         Logger.Instance().LogDump("HttpSend", "url: " + url + " Response: " + responseFromServer, true);
                        //Console.WriteLine(responseFromServer);
                        //     }
                        // }
                    }
                }
                if (_config.HttpUseDigest == true)
                {
                     Uri myurl = new Uri(url);
                     string baseurl = myurl.GetComponents(UriComponents.SchemeAndServer | UriComponents.UserInfo, UriFormat.Unescaped);
                     //var baseurl = myurl.Host;
                     var dir = myurl.PathAndQuery;
                     Logger.Instance().LogDump("HttpSimpleSend", "Using Digest:  Url: " + url + " BaseURL: " +baseurl + "  Dir: " + dir, true);
                     DigestAuthFixer digest = new DigestAuthFixer(baseurl, _config.HttpUser, _config.HttpPassword);
                     string strReturn = digest.GrabResponse(dir);
                }           
               
            
            }

            
            catch (Exception ex)
            {
                Logger.Instance().Log("HttpSimpleSend", "ERROR: For URL: " + url + "   Exception: " + ex, true);
            }

            
        }

        public void gotoHttp(string url, Plugin.ApiCurrently nowPlaying)
        {
            try
            {
                //Add Variable Support to URL passing - mainly useful for filename?
                var newurl = url;
                
                if (url =="")
                {
                    Logger.Instance().LogDump("HttpSend", "Called - URL Empty:  URL: " + url, true);
                    return;
                }

                if (nowPlaying.FileName != null) 
                { 
                newurl = url.Replace("%HTTPFILENAME%", Uri.EscapeUriString(nowPlaying.FileName)); 
                }
                if (nowPlaying.Artist !=null)
                {
                newurl = newurl.Replace("%HTTPARTIST%", Uri.EscapeUriString(nowPlaying.Artist));
                }
                if (nowPlaying.Album != null)
                {
                newurl = newurl.Replace("%HTTPALBUM%", Uri.EscapeUriString(nowPlaying.Album));
                }
                if (nowPlaying.FanartURL != null)
                {
                newurl = newurl.Replace("%HTTPFANARTURL%", Uri.EscapeUriString(nowPlaying.FanartURL));
                }
                if (nowPlaying.MediaType != null)
                {
                newurl = newurl.Replace("%HTTPMEDIATYPE%", Uri.EscapeUriString(nowPlaying.MediaType));
                }
                if (nowPlaying.ShowTitle != null)
                {
                newurl = newurl.Replace("%HTTPTITLE%", Uri.EscapeUriString(nowPlaying.ShowTitle));
                }
                if (nowPlaying.Plot != null)
                {
                newurl = newurl.Replace("%HTTPPLOT%", Uri.EscapeUriString(nowPlaying.Plot));
                }
                           
                
                newurl = newurl.Replace("%HTTPSEASONNO%", Uri.EscapeUriString(nowPlaying.SeasonNumber.ToString()));
                           
                
                newurl = newurl.Replace("%HTTPPROGRESS%", Uri.EscapeUriString(nowPlaying.Progress.ToString()));

                newurl = newurl.Replace("%HTTPTIME%", Uri.EscapeUriString(nowPlaying.Time.ToString()));

                newurl = newurl.Replace("%HTTPEPISODENO%", Uri.EscapeUriString(nowPlaying.EpisodeNumber.ToString()));









               

                Logger.Instance().LogDump("HttpSend", "Variables " +url + " newURL " + newurl, true);

                url = newurl;


                if (_config.HttpUseDigest == false)
                {
                    Logger.Instance().LogDump("HttpSend", "Using Basic: Url:  " + url, true);

                    var logon = _config.HttpUser;
                    var password = _config.HttpPassword;
                    var Auth = "Basic";


                    Logger.Instance().LogDump("HttpSend", "Using " + Auth + " Authorisation:   URL " + url, true);

                    WebRequest request = WebRequest.Create(url);
                    request.Method = WebRequestMethods.Http.Get;
                    NetworkCredential networkCredential = new NetworkCredential(logon, password); // logon in format "domain\username"
                    CredentialCache myCredentialCache = new CredentialCache { { new Uri(url), Auth, networkCredential } };
                    request.PreAuthenticate = true;
                    request.Credentials = myCredentialCache;
                    using (WebResponse response = request.GetResponse())
                    {

                        //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                        Logger.Instance().LogDump("HttpSend", "Response: " + url + " Response: " + (((HttpWebResponse)response).StatusDescription), true);
                        //using (Stream dataStream = response.GetResponseStream())
                        // {
                        //     using (StreamReader reader = new StreamReader(dataStream))
                        //     {
                        //         string responseFromServer = reader.ReadToEnd();
                        //         Logger.Instance().LogDump("HttpSend", "url: " + url + " Response: " + responseFromServer, true);
                        //Console.WriteLine(responseFromServer);
                        //     }
                        // }
                    }
                }
                if (_config.HttpUseDigest == true)
                {
                     Uri myurl = new Uri(url);
                     string baseurl = myurl.GetComponents(UriComponents.SchemeAndServer | UriComponents.UserInfo, UriFormat.Unescaped);
                     //var baseurl = myurl.Host;
                     var dir = myurl.PathAndQuery;
                     Logger.Instance().LogDump("HttpSend", "Using Digest:  Url: " + url + " BaseURL: " +baseurl + "  Dir: " + dir, true);
                     DigestAuthFixer digest = new DigestAuthFixer(baseurl, _config.HttpUser, _config.HttpPassword);
                     string strReturn = digest.GrabResponse(dir);
                }           
               
            
            }

            
            catch (Exception ex)
            {
                Logger.Instance().Log("HttpSend", "ERROR: For URL: " + url + "   Exception: " + ex, true);
            }

        
        }
         
        private void StartScreensaver()
        {
            if (!_isScreenSaver)
            {
                _isScreenSaver = true;
                if (_config.Dimming && !_config.DimmingOnlyVideo)
                {
                    Logger.Instance().Log("FrontView+", "Start screen saver : Dimming Here as well");
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
                    }
                }
                if (_config.Diaporama && (!_config.Dimming || _config.DimmingOnlyVideo))
                {
                    Logger.Instance().Log("FrontView+", "Start screen saver : Diaporama");
                    StartDiaporama();
                }
            }
            Logger.Instance().LogDump("FrontView FANART    : ResetTimer Run from StartScreenSaver", _timer);
            ResetTimer();
        }

        private void CheckFirstLaunch()
        {
            if (!_config.IsConfigured && _currentGrid.Name != "grd_Settings")
            {
                Logger.Instance().Log("FrontView+", "Not configured, Go to settings", true);
                btn_Home_Settings_Click(null, null);
            }
        }


        private void Change_Display_Settings(object sender, EventArgs e)
        {
            Logger.Instance().Log("FrontView+", "Dispay settings changed");
            Topmost = _config.Topmost;
            WindowStartupLocation = WindowStartupLocation.Manual;
            //REMOVE OR CHANGE ACTIVATE()
            //Activate();
            var dx = 1.0;
            var dy = 1.0;
            var temp = PresentationSource.FromVisual(this);
            if (temp != null)
            {
                if (temp.CompositionTarget != null)
                {
                    var m = temp.CompositionTarget.TransformToDevice;
                    dx = m.M11;
                    dy = m.M22;
                }
            }
            var screens = System.Windows.Forms.Screen.AllScreens;
            Logger.Instance().LogDump("Var Screens", true);
    //        ni.BalloonTipTitle = "Minimise Setting";
    //        ni.BalloonTipText = " Minimise Always On";

        //    ni.DoubleClick +=

        //        delegate(object sender, EventArgs args)
         //       {
        //            this.Show();
        //            this.WindowState = WindowState.Normal;
        // //      
            Logger.Instance().LogDump("Screens Length", screens.Length);
            if (screens.Length == 1 || !_config.SecondScreen)
            {
                if (_config.ForceResolution)
                {
                    var currentRes = ScreenResolution.GetDevmode(0, -1);
                    Logger.Instance().LogDump("CurrentResolutionMonoScreen", currentRes);
                    if (currentRes.DMPelsHeight != _config.Resolution.DMPelsHeight || currentRes.DMPelsWidth != _config.Resolution.DMPelsWidth || currentRes.DMBitsPerPel != _config.Resolution.DMBitsPerPel)
                    {
                        ScreenResolution.ChangeResolutionMode(0, _config.Resolution);
                        Logger.Instance().LogDump("ChangeResolutionMonoScreen", _config.Resolution);
                    }
                }
                Top = 0;
                Left = 0;
            }
            else
            {
                if (_config.ForceResolution)
                {
                    var currentRes = ScreenResolution.GetDevmode(1, -1);
                    Logger.Instance().LogDump("Screens current Res", currentRes);
                    Logger.Instance().LogDump("CurrentResolutionMultiScreen", currentRes, true);
                    if (currentRes.DMPelsHeight != _config.Resolution.DMPelsHeight || currentRes.DMPelsWidth != _config.Resolution.DMPelsWidth || currentRes.DMBitsPerPel != _config.Resolution.DMBitsPerPel)
                    {
                        ScreenResolution.ChangeResolutionMode(1, _config.Resolution);
                        Logger.Instance().LogDump("ChangeResolutionMultiScreen", _config.Resolution);
                    }
                }
                screens = System.Windows.Forms.Screen.AllScreens;
                foreach (var scr in screens.Where(scr => !scr.Primary))
                {
                    Top = scr.Bounds.Top / dy;
                    Left = scr.Bounds.Left / dx;
                    Logger.Instance().LogDump("Screen Device Name", scr.DeviceName);
                    Logger.Instance().LogDump("2nd Screen Details", ScreenResolution.GetDevmode(1,-1));
                    break;
                }
            }
            if (_config.Resolution.DMPelsWidth > 0)
            {
                Width = _config.Resolution.DMPelsWidth / dx;
                Height = _config.Resolution.DMPelsHeight / dy;
                Logger.Instance().LogDump("Screens Width", Width);
                Logger.Instance().LogDump("Screens Height", Height);
            }

            if (_config.Resolution.DMPelsHeight == 480)
            {
                _config.Hack480 = true;
                Logger.Instance().LogDump("DMPelHeight equals 480", _config.Resolution.DMPelsHeight);
                brd_Home_Video.Margin = new Thickness(0, 0, 100, 180);
                brd_Home_Music.Margin = new Thickness(0, 70, 100, 0);
                brd_Home_Other.Margin = new Thickness(0, 320, 100, 0);



                
                grd_Settings.ClipToBounds = false;
                ScaleTransform myScaleTransform = new ScaleTransform();
                myScaleTransform.ScaleX = 0.85;
                myScaleTransform.ScaleY = 0.85;
            //    grd_Settings.RenderTransformOrigin = new Point(0.5, 0.5);
                grd_Settings.RenderTransform = myScaleTransform;


    


            }
            else
            {
                brd_Home_Video.Margin = new Thickness(0, 0, 100, 250);
                brd_Home_Music.Margin = new Thickness(0, 70, 100, 0);
                brd_Home_Other.Margin = new Thickness(0, 390, 100, 0);

                grd_Settings.ClipToBounds = true;
                ScaleTransform myScaleTransform = new ScaleTransform();
                myScaleTransform.ScaleX = 1.0;
                myScaleTransform.ScaleY = 1.0;
             //   grd_Settings.RenderTransformOrigin = new Point(0.5, 0.5);
                grd_Settings.RenderTransform = myScaleTransform;
            }

            
            if (_config.ShowAudioMenu == false)
            {
                brd_Home_Video.Margin = new Thickness(0, 0, 100, 100);
              //  brd_Home_Music.Margin = new Thickness(0, 70, 100, 0);
                brd_Home_Other.Margin = new Thickness(0, 200, 100, 0);


            }
            
            _setPov = false;
        }

        private void notifyIcon1_DoubleClick(object Sender, EventArgs e)
        {
            // Show the form when the user double clicks on the notify icon. 

            // Set the WindowState to normal if the form is minimized. 
            if (this.WindowState == WindowState.Minimized)
            {
                 this.ShowInTaskbar = _config.ShowInTaskbar;
                 _config.MinimiseAlways = false;
                _config.FanartAlways = true;

                
                RestoreWindowNoActivateExtension.RestoreNoActivate(this);
                this.Show();                
                //Use No activation or focus change as bove - functioing 

                //this.WindowState = WindowState.Normal;

                //this.Activate();

                

                Logger.Instance().LogDump("NEW Yastse Debug    : DBL click tasbar event/Min Window, Open Window and set MinimiseAlways to false ", _config.MinimiseAlways);
                return;
            }
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Minimized;
                this.ShowInTaskbar = _config.ShowInTaskbar;
                _config.MinimiseAlways = true;
                _config.FanartAlways = false;
                this.Hide();
                Logger.Instance().LogDump("NEW Yastse Debug    : DBL click tasbar event/Normal Window, Minimise Window and set MinimiseAlways to true ", _config.MinimiseAlways);
            }
            // Activate the form. 
            //this.Activate();



        }

        private void ShowHome()
        {
            
            var en = _yatse2Pages.GetEnumerator();
            while (en.MoveNext())
            {
                if (en.Key.ToString() != _config.Homepage)
                    continue;
                Logger.Instance().Log("FrontView+", "Home Page : " + en.Value);
                switch (en.Value.ToString())
                {
                    case "grd_Movies":
                        Load_Movies();
                        if (_moviesDataSource.Count < 1)
                            return;
                        break;
                    case "grd_TvShows" :
                        Load_TvShows();
                        if (_tvShowsDataSource.Count < 1)
                            return;
                        break;
                    case "grd_AudioAlbums":
                        Load_AudioAlbums();
                        if (_audioAlbumsDataSource.Count < 1)
                            return;
                        break;
                    case "grd_AudioArtists":
                        Load_AudioArtists();
                        if (_audioArtistsDataSource.Count < 1)
                            return;
                        break;
                    case "grd_AudioGenres":
                        Load_AudioGenres();
                        if (_audioGenresDataSource.Count < 1)
                            return;
                        break;
                    case "grd_Weather" :
                        RefreshWeather();
                        break;
                    default:
                        break;
                }

                foreach (Grid grid in trp_Transition.Items)
                {
                    if (grid.Name != en.Value.ToString()) continue;
                    ShowGrid(grid);
                    return;
                }
            }
        }

        private void GoBack()
        {
            if (_gridHistory.Count < 1)
                return;
            foreach (Grid grid in trp_Transition.Items)
            {
                if (grid.Name != _gridHistory[_gridHistory.Count-1]) continue;
                if (ShowGrid(grid, false) && _gridHistory.Count > 0)
                    _gridHistory.RemoveAt(_gridHistory.Count - 1);
                return;
            }
            
        }

        private void ShowGrid(Grid newGrid)
        {
            ShowGrid(newGrid, true);
        }

        private bool ShowGrid(Grid newGrid , bool history)
        {
            Logger.Instance().LogDump("FrontView FANART    : ResetTimer Run from ShowGrid1", _timer);
            ResetTimer();
            if ((_currentGrid.Name == newGrid.Name) || (trp_Transition.IsTransitioning)) return false;
            Logger.Instance().Log("FrontView+", "Show Grid : " + newGrid.Name);

            grd_PlayMenu.Visibility = Visibility.Hidden;
            grd_Filter.Visibility = Visibility.Hidden;
            grd_Settings_Weather.Visibility = Visibility.Hidden;
            grd_Settings_Remotes_Edit.Visibility = Visibility.Hidden;
            grd_Movies_Details.Visibility = Visibility.Hidden;
            grd_TvShows_Details.Visibility = Visibility.Hidden;
            grd_AudioAlbums_Details.Visibility = Visibility.Hidden;
            grd_Remote.Visibility = Visibility.Hidden;

            _yatse2Properties.ShowHomeButton = newGrid.Name != "grd_Home";

            _disableFocus = ((newGrid.Name == "grd_Settings") || (newGrid.Name == "grd_Remotes"));

            trp_Transition.ApplyTransition(_currentGrid.Name, newGrid.Name);

            if (newGrid.Name == "grd_Home")
                _gridHistory.Clear();
            else
                if (history)
                    if (_currentGrid.Name != "grd_Currently")
                        _gridHistory.Add(_currentGrid.Name);
            _currentGrid = newGrid;
            return true;
        }


        private void ResetTimer()
        {
            
            Logger.Instance().Trace("FrontView+", "FOCUS::: config.KeepFocus: " +_config.KeepFocus+" FOCUSS::: _remoteInfo"+_remoteInfo.ProcessName+ "FOCUS:::: _disableFocus"+_disableFocus); 
            
            if (_config.KeepFocus && _remoteInfo != null && !_disableFocus)
            {
                if (_remote != null)
                // No focus until press button
                {
                    _remote.GiveFocus();
                }

             }


            Logger.Instance().LogDump("FrontView FANART    : ResetTimer Run", _timer);
            _timer = 0;
        }

        private void RefreshHeader()
        {
            var now = DateTime.Now;
            _yatse2Properties.Date = now.ToString("dd MMMM yyy", CultureInfo.CurrentUICulture.DateTimeFormat);
            _yatse2Properties.Time = now.ToShortTimeString();
            
            var weatherData = _weather.GetWeatherData(_config.WeatherLoc);
            
            
            if (weatherData == null)
            {
                Logger.Instance().Log("FrontView+", "RefreshHeader : No weather data");
                return;
            }
            
            if (String.IsNullOrEmpty(_yatse2Properties.Weather.Day1Name))
                RefreshWeather();
            _yatse2Properties.Weather.LoadCurrentData(weatherData,_yatse2Properties.Skin);
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
                _weather.Dispose();
                _database.Close();
                ni.Visible = false;
                ni.Icon = null;
                ni.Dispose();
                ni = null;
             }
        }


        private class FakeWindowsPeer : WindowAutomationPeer
        {
            public FakeWindowsPeer(Window window) : base(window)
            {
                
            }

            protected override List<AutomationPeer> GetChildrenCore()
            {
                return null;
            }

        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new FakeWindowsPeer(this);
        }

    }

}