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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using Setup;
using DIE = System.Drawing.Imaging.Encoder;

namespace Plugin
{
    public class ApiImageDownloadInfo
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public bool ToThumb { get; set; }
        public int MaxHeight { get; set; }
    }

    public interface IYatse2RemotePlugin
    {
        int Version { get; }
        bool IsCompatible(int yatseVersion);
        string Name { get; set; }
        ApiConnection GetRemote();
        string GetHashFromFileName(string fileName);
        ApiSupportedFunctions SupportedFunctions();
    }

    public interface IApiFile : IDisposable
    {
        void AsyncDownloadImages(ApiImageDownloadInfo[] apiImageDownloadInfos);
        bool DownloadImages(ApiImageDownloadInfo apiImageDownloadInfo);
        bool AsyncDownloadFinished();
        void StopAsync();
    }

    public interface IApiVideoLibrary
    {
        Collection<ApiTvSeason> GetTvSeasons();
        Collection<ApiTvSeason> GetTvSeasonsRefresh();

        Collection<ApiTvEpisode> GetTvEpisodes();
        Collection<ApiTvShow> GetTvShows();

        Collection<ApiTvShow> GetTvShowsRefresh();
        Collection<ApiMovie> GetMovies();

        Collection<ApiMovie> GetMoviesRefresh();

        Collection<ApiTvEpisode> GetTvEpisodesRefresh();
    }

    public interface IApiAudioLibrary
    {
        Collection<ApiAudioGenre> GetGenres();
        Collection<ApiAudioArtist> GetArtists();
        Collection<ApiAudioAlbum> GetAlbums();
        Collection<ApiAudioSong> GetSongs();
    }

    public interface IApiAudioPlayer
    {
        void PlayFiles(Collection<ApiAudioSong> songs);
        void PlaySong(ApiAudioSong audio);
    }

    public interface IApiPicturePlayer
    {
    }

    public interface IApiVideoPlayer
    {
        void PlayMovie(ApiMovie video);
        void PlayTvEpisode(ApiTvEpisode tvepisode);
    }

    public interface IApiPlayer
    {
        void RefreshNowPlaying();
        ApiCurrently NowPlaying(bool checkNewMedia);
        void SeekPercentage(int progress);
        void PlayPause();
        void Stop();
        void SkipPrevious();
        void SkipNext();
        void SetVolume(int percentage);
        void ToggleMute();
    }

    public interface IApiSystem
    {
        void Quit();
        void Shutdown();
        void Reboot();
    }

    public interface IApiRemote 
    {
        void Return();
        void Enter();
        void Info();
        void Home();
        void Video();
        void Music();
        void Pictures();
        void Tv();

        void VolUp();
        void VolDown();
        void ToggleMute();

        void Menu();
        void Title();
        void Down();
        void Up();
        void Left();
        void Right();
        void Mute();
        void PlayDrive();
        void EjectDrive();
        void Subtitles();

        void Previous();
        void Rewind();
        void Play();
        void Stop();
        void Forward();
        void Next();

        void One();
        void Two();
        void Three();
        void Four();
        void Five();
        void Six();
        void Seven();
        void Eight();
        void Nine();
        void Zero();
        void Star();
        void Hash();


    }

    public abstract class ApiConnection : IDisposable
    {
        public IApiFile File { get; set; }
        public IApiVideoLibrary VideoLibrary { get; set; }
        public IApiAudioLibrary AudioLibrary { get; set; }
        public IApiAudioPlayer AudioPlayer { get; set; }
        public IApiPicturePlayer PicturePlayer { get; set; }
        public IApiVideoPlayer VideoPlayer { get; set; }
        public IApiPlayer Player { get; set; }
        public IApiSystem SystemRunning { get; set; }
        public IApiRemote Remote { get; set; }

        private int _connectionTimeout = 2500;
        public string IP { get; set; }
        public string Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        protected string ApiName { get; set; }

        public abstract void GiveFocus();

        public abstract string GetOS();
        public abstract string GetVersion();
        public abstract string GetAdditionalInfo();
        public abstract bool IsConnected();
        public abstract void Close();

        public virtual void Configure(string ip, string port, string user, string password)
        {
            IP = ip;
            Port = port;
            UserName = user;
            Password = password;
            Log("Configure : " + IP + ":" + Port + " - " + UserName + ":" + Password);
        }

        public virtual bool CheckConnection()
        {
            return false;
        }

        public virtual bool CheckRemote(string os, string version, string additional, bool force)
        {
            return false;
        }

        public virtual int TestConnection(string ip, string port, string user, string password)
        {
            return 0;
        }

        public void Log(string message)
        {
            Logger.Instance().Log("FrontView+Api-" + ApiName + "", message);
        }

        public void Trace(string message)
        {
            Logger.Instance().Trace("FrontVIew+Api-" + ApiName + "", message);
        }

        public void SetTimeout(int timeout)
        {
            _connectionTimeout = timeout;
        }

        public int GetTimeout()
        {
            return _connectionTimeout;
        }

        public void Configure(string ip, string port)
        {
            Configure(ip, port, null, null);
        }

        public abstract void Dispose();


        public void GenerateThumb(string originalFile, string newFile, int maxHeight)
        {
            try
            {
                var fullsizeImage = Image.FromFile(originalFile);

                fullsizeImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
                fullsizeImage.RotateFlip(RotateFlipType.Rotate180FlipNone);

                var newHeight = fullsizeImage.Height;
                var newWidth = fullsizeImage.Width;
                if (newHeight > maxHeight)
                {
                    newWidth = fullsizeImage.Width * maxHeight / fullsizeImage.Height;
                    newHeight = maxHeight;
                }

                var result = new Bitmap(newWidth, newHeight);

                using (var graphics = Graphics.FromImage(result))
                {
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.DrawImage(fullsizeImage, 0, 0, result.Width, result.Height);
                }

                fullsizeImage.Dispose();

                var codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (var codec in codecs.Where(codec => codec.MimeType == "image/png"))
                {
                    ici = codec;
                }

                var ep = new EncoderParameters();
                ep.Param[0] = new EncoderParameter(DIE.Quality, (long)85);

                result.Save(newFile, ici, ep);
                result.Dispose();
            }
            catch
            {}

            
        }


    }

    public class ApiSupportedFunctions
    {
        public bool MovieLibrary { get; set; }
        public bool TvShowLibrary { get; set; }
        public bool AudioLibrary { get; set; }
        public bool PictureLibrary { get; set; }
        public bool SupportsRemoteControl { get; set; }
    }

    public class ApiCurrently
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public int Year { get; set; }
        public int Track { get; set; }
        public string Genre { get; set; }
        public string ThumbURL { get; set; }
        public string FanartURL { get; set; }
        public string ShowTitle { get; set; }
        public DateTime FirstAired { get; set; }
        public string Plot { get; set; }
        public string Tagline { get; set; }
        public string Rating { get; set; }
        public string Director { get; set; }
        public int SeasonNumber { get; set; }
        public int EpisodeNumber { get; set; }
        public string Studio { get; set; }
        public int Progress { get; set; }
        public int Volume { get; set; }
        public TimeSpan Time { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsPlaying { get; set; }
        public bool IsPaused { get; set; }
        public string MediaType { get; set; }
        public string CurrentMenuLabel { get; set; }
        public string CurrentMenuID { get; set; }
        public bool IsNewMedia{ get; set; }
        public bool IsMuted { get; set; }
    }

    //ok about to change
    public class ApiMovie
    {
        public long IdMovie { get; set; }
        public long IdFile { get; set; }
        public string IdScraper { get; set; }
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string Genre { get; set; }
        public string Tagline { get; set; }
        public string Plot { get; set; }
        public string Director { get; set; }
        public long Year { get; set; }
        public string Length { get; set; }
        public string Mpaa { get; set; }
        public string Studio { get; set; }
        public string Rating { get; set; }
        public string Votes { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public long PlayCount { get; set; }
        public string Hash { get; set; }
        public string Thumb { get; set; }
        public string Banner { get; set; }
        public string Logo { get; set; }
        public string Fanart { get; set; }
        public long IsStack { get; set; }
        public string DateAdded { get; set; }
    }

    public class ApiTvShow
    {
        public long IdShow { get; set; }
        public string IdScraper { get; set; }
        public string Title { get; set; }
        public string Plot { get; set; }
        public string Premiered { get; set; }
        public string Rating { get; set; }
        public string Genre { get; set; }
        public string Mpaa { get; set; }
        public string Studio { get; set; }
        public string Path { get; set; }
        public long TotalCount { get; set; }
        public string Hash { get; set; }
        public string Thumb { get; set; }
        public string Fanart { get; set; }
        public string Banner { get; set; }
    }

    public class ApiTvEpisode
    {
        public long IdEpisode { get; set; }
        public long IdFile { get; set; }
        public long IdShow { get; set; }
        public string Title { get; set; }
        public string ShowTitle { get; set; }
        public string Rating { get; set; }
        public string Plot { get; set; }
        public string Director { get; set; }
        public string Date { get; set; }
        public string Mpaa { get; set; }
        public string Studio { get; set; }
        public long Season { get; set; }
        public long Episode { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public long PlayCount { get; set; }
        public string Hash { get; set; }
        public string Thumb { get; set; }
        public string Fanart { get; set; }
        public long IsStack { get; set; }
    }

    public class ApiTvSeason
    {
        public long SeasonNumber { get; set; }
        public long EpisodeCount { get; set; }
        public long IdShow { get; set; }
        public string Show { get; set; }
        public string Hash { get; set; }
        public string Thumb { get; set; }
        public string Fanart { get; set; }
    }

    public class ApiAudioGenre
    {
        public string Name { get; set; }
        public long IdGenre { get; set; }
        public long AlbumCount { get; set; }
        public string Thumb { get; set; }
        public string Fanart { get; set; }
    }

    public class ApiAudioArtist
    {
        public string Name { get; set; }
        public string Biography { get; set; }
        public long IdArtist { get; set; }
        public string Thumb { get; set; }
        public string Fanart { get; set; }
        public string Hash { get; set; }
    }

    public class ApiAudioAlbum
    {
        public string Title { get; set; }
        public long IdAlbum { get; set; }
        public long IdArtist { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
        public long IdGenre { get; set; }
        public long Year { get; set; }
        public string Thumb { get; set; }
        public string Fanart { get; set; }
        public string Hash { get; set; }
    }

    public class ApiAudioSong
    {
        public long IdSong { get; set; }
        public string Title { get; set; }
        public long Track { get; set; }
        public long Duration { get; set; }
        public long Year { get; set; }
        public string FileName { get; set; }
        public long IdAlbum { get; set; }
        public string Album { get; set; }
        public string Path { get; set; }
        public long IdArtist { get; set; }
        public string Artist { get; set; }
        public long IdGenre { get; set; }
        public string Genre { get; set; }
        public string Thumb { get; set; }
        public string Fanart { get; set; }
        public string Hash { get; set; }
    }

}