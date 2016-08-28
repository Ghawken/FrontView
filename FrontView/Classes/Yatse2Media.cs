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
using System.Collections.Generic;
using Plugin;

namespace FrontView.Classes
{

    public abstract class Yatse2Media : DependencyObject
    {
        public long Id { get; set; }
        public long IdRemote { get; set; }

        public static readonly DependencyProperty IsFavoriteProperty =
            DependencyProperty.Register("IsFavorite", typeof(long), typeof(Yatse2Media));

        public long IsFavorite
        {
            get { return (long)GetValue(IsFavoriteProperty); }
            set { SetValue(IsFavoriteProperty, value); }
        }
    }

    public class Yatse2Remote : DependencyObject
    {
        public long Id { get; set; }

        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(Yatse2Remote));
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public static readonly DependencyProperty ApiProperty =
            DependencyProperty.Register("Api", typeof(string), typeof(Yatse2Remote));
        public string Api
        {
            get { return (string)GetValue(ApiProperty); }
            set { SetValue(ApiProperty, value); }
        }

        public static readonly DependencyProperty ProcessNameProperty =
            DependencyProperty.Register("ProcessName", typeof(string), typeof(Yatse2Remote));
        public string ProcessName
        {
            get { return (string)GetValue(ProcessNameProperty); }
            set { SetValue(ProcessNameProperty, value); }
        }

        public static readonly DependencyProperty IPProperty =
           DependencyProperty.Register("IP", typeof(string), typeof(Yatse2Remote));
        public string IP
        {
            get { return (string)GetValue(IPProperty); }
            set { SetValue(IPProperty, value); }
        }

        public static readonly DependencyProperty PortProperty =
            DependencyProperty.Register("Port", typeof(string), typeof(Yatse2Remote));
        public string Port
        {
            get { return (string)GetValue(PortProperty); }
            set { SetValue(PortProperty, value); }
        }

        public static readonly DependencyProperty LoginProperty =
            DependencyProperty.Register("Login", typeof(string), typeof(Yatse2Remote));
        public string Login
        {
            get { return (string)GetValue(LoginProperty); }
            set { SetValue(LoginProperty, value); }
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(Yatse2Remote));
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public static readonly DependencyProperty VersionProperty =
            DependencyProperty.Register("Version", typeof(string), typeof(Yatse2Remote));
        public string Version
        {
            get { return (string)GetValue(VersionProperty); }
            set { SetValue(VersionProperty, value); }
        }

        public static readonly DependencyProperty MacAddressProperty =
            DependencyProperty.Register("MacAddress", typeof(string), typeof(Yatse2Remote));
        public string MacAddress
        {
            get { return (string)GetValue(MacAddressProperty); }
            set { SetValue(MacAddressProperty, value); }
        }

        public static readonly DependencyProperty OSProperty =
            DependencyProperty.Register("OS", typeof(string), typeof(Yatse2Remote));
        public string OS
        {
            get { return (string)GetValue(OSProperty); }
            set { SetValue(OSProperty, value); }
        }

        public static readonly DependencyProperty AdditionalProperty =
            DependencyProperty.Register("Additional", typeof(string), typeof(Yatse2Remote));
        public string Additional
        {
            get { return (string)GetValue(AdditionalProperty); }
            set { SetValue(AdditionalProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(string), typeof(Yatse2Remote));
        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty ValidatedProperty =
            DependencyProperty.Register("Validated", typeof(bool), typeof(Yatse2Remote));
        public bool Validated
        {
            get { return (bool)GetValue(ValidatedProperty); }
            set { SetValue(ValidatedProperty, value); }
        }

        public static readonly DependencyProperty CacheFilledProperty =
            DependencyProperty.Register("CacheFilled", typeof(long), typeof(Yatse2Remote));
        public long CacheFilled
        {
            get { return (long)GetValue(CacheFilledProperty); }
            set { SetValue(CacheFilledProperty, value); }
        }

        public static readonly DependencyProperty IsDefaultProperty =
            DependencyProperty.Register("IsDefault", typeof(long), typeof(Yatse2Remote));
        public long IsDefault
        {
            get { return (long)GetValue(IsDefaultProperty); }
            set { SetValue(IsDefaultProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(long), typeof(Yatse2Remote));
        public long IsSelected
        {
            get { return (long)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
    }

    public class Yatse2Movie : Yatse2Media
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
        public string Hash { get; set; }
        public string Thumb { get; set; }
        public string Banner { get; set; }
        public string Logo { get; set; }
        public string Fanart { get; set; }
        public long IsStack { get; set; }
        public string DateAdded { get; set; }
        public string MovieIcons { get; set; }

        public static readonly DependencyProperty PlayCountProperty =
            DependencyProperty.Register("PlayCount", typeof(long), typeof(Yatse2Movie));
        public long PlayCount
        {
            get { return (long)GetValue(PlayCountProperty); }
            set { SetValue(PlayCountProperty, value); }
        }

        public ApiMovie ToApi()
        {
            var api = new ApiMovie
                          {
                              Director = Director,
                              Fanart = Fanart,
                              FileName = FileName,
                              Genre = Genre,
                              Hash = Hash,
                              IdFile = IdFile,
                              IdMovie = IdMovie,
                              IdScraper = IdScraper,
                              IsStack = IsStack,
                              Length = Length,
                              Mpaa = Mpaa,
                              OriginalTitle = OriginalTitle,
                              Path = Path,
                              PlayCount = PlayCount,
                              Plot = Plot,
                              Rating = Rating,
                              Studio = Studio,
                              Tagline = Tagline,
                              Thumb = Thumb,
                              Banner = Banner,
                              Logo = Logo,
                              Title = Title,
                              Votes = Votes,
                              Year = Year,
                              DateAdded = DateAdded,
                              MovieIcons = MovieIcons
                          };
            return api;
        }
        
        public Yatse2Movie() {}

        public Yatse2Movie(ApiMovie apiMovie)
        {
            if (apiMovie == null)
                return;
            FileName = apiMovie.FileName;
            Path = apiMovie.Path;
            Plot = apiMovie.Plot;
            Title = apiMovie.Title;
            Mpaa = apiMovie.Mpaa;
            OriginalTitle = apiMovie.OriginalTitle;
            Genre = apiMovie.Genre;
            Tagline = apiMovie.Tagline;
            Director = apiMovie.Director;
            IdFile = apiMovie.IdFile;
            IdScraper = apiMovie.IdScraper;
            IdMovie = apiMovie.IdMovie;
            Thumb = apiMovie.Thumb;
            Fanart = apiMovie.Fanart;
            Banner = apiMovie.Banner;
            Logo = apiMovie.Logo;
            Hash = apiMovie.Hash;
            IsStack = apiMovie.IsStack;
            Length = apiMovie.Length;
            PlayCount = apiMovie.PlayCount;
            Rating = apiMovie.Rating;
            Studio = apiMovie.Studio;
            Votes = apiMovie.Votes;
            Year = apiMovie.Year;
            DateAdded = apiMovie.DateAdded;
            MovieIcons = apiMovie.MovieIcons;
        }
    }

    public class Yatse2AudioGenre : Yatse2Media
    {
        public string Name { get; set; }
        public long IdGenre { get; set; }
        public long AlbumCount { get; set; }
        public string Thumb { get; set; }
        public string Fanart { get; set; }

        public ApiAudioGenre ToApi()
        {
            var api = new ApiAudioGenre
                          {
                              Fanart = Fanart,
                              AlbumCount = AlbumCount,
                              IdGenre = IdGenre,
                              Name = Name,
                              Thumb = Thumb
                          };
            return api;
        }

        public Yatse2AudioGenre() { }

        public Yatse2AudioGenre(ApiAudioGenre apiAudioGenre)
        {
            if (apiAudioGenre == null)
                return;
            IdGenre = apiAudioGenre.IdGenre;
            Name = apiAudioGenre.Name;
            AlbumCount = apiAudioGenre.AlbumCount;
            Thumb = apiAudioGenre.Thumb;
            Fanart = apiAudioGenre.Fanart;
        }
    }

    public class Yatse2AudioAlbum : Yatse2Media
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

        public ApiAudioAlbum ToApi()
        {
            var api = new ApiAudioAlbum
                          {
                              Artist = Artist,
                              Fanart = Fanart,
                              Genre = Genre,
                              Hash = Hash,
                              IdAlbum = IdAlbum,
                              IdArtist = IdArtist,
                              IdGenre = IdGenre,
                              Thumb = Thumb,
                              Title = Title,
                              Year = Year
                          };


            return api;
        }

        public Yatse2AudioAlbum() { }

        public Yatse2AudioAlbum(ApiAudioAlbum apiAudioAlbum)
        {
            if (apiAudioAlbum == null)
                return;
            Title = apiAudioAlbum.Title;
            IdAlbum = apiAudioAlbum.IdAlbum;
            IdArtist = apiAudioAlbum.IdArtist;
            Artist = apiAudioAlbum.Artist;
            Genre = apiAudioAlbum.Genre;
            IdGenre = apiAudioAlbum.IdGenre;
            Year = apiAudioAlbum.Year;
            Thumb = apiAudioAlbum.Thumb;
            Fanart = apiAudioAlbum.Fanart;
            Hash = apiAudioAlbum.Hash;
        }
    }

    public class Yatse2AudioArtist : Yatse2Media
    {
        public long IdArtist { get; set; }
        public string Name { get; set; }
        public string Biography { get; set; }
        public string Thumb { get; set; }
        public string Fanart { get; set; }
        public string Hash { get; set; }

        public ApiAudioArtist ToApi()
        {
            var api = new ApiAudioArtist
                          {
                              Biography = Biography,
                              Fanart = Fanart,
                              Hash = Hash,
                              IdArtist = IdArtist,
                              Name = Name,
                              Thumb = Thumb
                          };
            return api;
        }

        public Yatse2AudioArtist() { }

        public Yatse2AudioArtist(ApiAudioArtist apiAudioArtist)
        {
            if (apiAudioArtist == null)
                return;
            IdArtist = apiAudioArtist.IdArtist;
            Name = apiAudioArtist.Name;
            Biography = apiAudioArtist.Biography;
            Thumb = apiAudioArtist.Thumb;
            Fanart = apiAudioArtist.Fanart;
            Hash = apiAudioArtist.Hash;
        }
    }

    public class Yatse2AudioSong : Yatse2Media
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

        public ApiAudioSong ToApi()
        {
            var api = new ApiAudioSong
                          {
                            Album = Album,
                            Artist = Artist,
                            Duration = Duration,
                            Fanart = Fanart,
                            Hash = Hash,
                            IdArtist = IdArtist,
                            Thumb = Thumb,
                            FileName = FileName,
                            Genre = Genre,
                            IdAlbum = IdAlbum,
                            IdGenre = IdGenre,
                            IdSong = IdSong,
                            Path = Path,
                            Title = Title,
                            Track = Track,
                            Year = Year
            };
            return api;

        }

        public Yatse2AudioSong() { }

        public Yatse2AudioSong(ApiAudioSong apiAudioSong)
        {
            if (apiAudioSong == null)
                return;
            Title = apiAudioSong.Title;
            IdAlbum = apiAudioSong.IdAlbum;
            IdArtist = apiAudioSong.IdArtist;
            Artist = apiAudioSong.Artist;
            Genre = apiAudioSong.Genre;
            IdGenre = apiAudioSong.IdGenre;
            Year = apiAudioSong.Year;
            Thumb = apiAudioSong.Thumb;
            Fanart = apiAudioSong.Fanart;
            Hash = apiAudioSong.Hash;
            Album = apiAudioSong.Album;
            Duration = apiAudioSong.Duration;
            FileName = apiAudioSong.FileName;
            IdSong = apiAudioSong.IdSong;
            Path = apiAudioSong.Path;
            Track = apiAudioSong.Track;
        }

    }

    public class Yatse2TvShow : Yatse2Media
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
        public string Logo { get; set; }
        
        public ApiTvShow ToApi()
        {
            var api = new ApiTvShow
            {
                Fanart = Fanart,
                Hash = Hash,
                Thumb = Thumb,
                Genre = Genre,
                Path = Path,
                Title = Title,
                IdScraper = IdScraper,
                IdShow = IdShow,
                Mpaa = Mpaa,
                Plot = Plot,
                Premiered = Premiered,
                Rating = Rating,
                Studio = Studio,
                TotalCount = TotalCount,
                Banner = Banner,
                Logo = Logo
            };
            return api;

        }

        public Yatse2TvShow() { }

        public Yatse2TvShow(ApiTvShow apiTvShow)
        {
            if (apiTvShow == null)
                return;
            Title = apiTvShow.Title;
            IdShow = apiTvShow.IdShow;
            IdScraper = apiTvShow.IdScraper;
            Plot = apiTvShow.Plot;
            Premiered = apiTvShow.Premiered;
            Rating = apiTvShow.Rating;
            Genre = apiTvShow.Genre;
            Mpaa = apiTvShow.Mpaa;
            Studio = apiTvShow.Studio;
            Path = apiTvShow.Path;
            TotalCount = apiTvShow.TotalCount;
            Hash = apiTvShow.Hash;
            Thumb = apiTvShow.Thumb;
            Fanart = apiTvShow.Fanart;
            Banner = apiTvShow.Banner;
            Logo = apiTvShow.Logo;
        }
    }

    public class Yatse2TvEpisode : Yatse2Media
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
        public string Hash { get; set; }
        public string Thumb { get; set; }
        public string Fanart { get; set; }
        public long IsStack { get; set; }

        public static readonly DependencyProperty PlayCountProperty =
            DependencyProperty.Register("PlayCount", typeof(long), typeof(Yatse2TvEpisode));
        public long PlayCount
        {
            get { return (long)GetValue(PlayCountProperty); }
            set { SetValue(PlayCountProperty, value); }
        }

        public ApiTvEpisode ToApi()
        {
            var api = new ApiTvEpisode
            {
                Fanart = Fanart,
                Hash = Hash,
                Thumb = Thumb,
                Path = Path,
                Title = Title,
                IdShow = IdShow,
                Mpaa = Mpaa,
                Plot = Plot,
                Rating = Rating,
                Studio = Studio,
                Date = Date,
                Director = Director,
                Episode = Episode,
                FileName = FileName,
                IdEpisode = IdEpisode,
                IdFile = IdFile,
                IsStack = IsStack,
                PlayCount = PlayCount,
                Season = Season,
                ShowTitle = ShowTitle,

            };
            return api;
        }

        public Yatse2TvEpisode() { }

        public Yatse2TvEpisode(ApiTvEpisode apiTvEpisode)
        {
            if (apiTvEpisode == null)
                return;
            IdEpisode = apiTvEpisode.IdEpisode;
            IdFile = apiTvEpisode.IdFile;
            IdShow = apiTvEpisode.IdShow;
            Title = apiTvEpisode.Title;
            ShowTitle = apiTvEpisode.ShowTitle;
            Rating = apiTvEpisode.Rating;
            Plot = apiTvEpisode.Plot;
            Director = apiTvEpisode.Director;
            Date = apiTvEpisode.Date;
            Mpaa = apiTvEpisode.Mpaa;
            Studio = apiTvEpisode.Studio;
            Season = apiTvEpisode.Season;
            Episode = apiTvEpisode.Episode;
            FileName = apiTvEpisode.FileName;
            Path = apiTvEpisode.Path;
            PlayCount = apiTvEpisode.PlayCount;
            Hash = apiTvEpisode.Hash;
            Thumb = apiTvEpisode.Thumb;
            Fanart = apiTvEpisode.Fanart;
            IsStack = apiTvEpisode.IsStack;
        }


    }

    public class Yatse2TvSeason : Yatse2Media
    {
        public long SeasonNumber { get; set; }
        public long EpisodeCount { get; set; }
        public long IdShow { get; set; }
        public string Show { get; set; }
        public string Hash { get; set; }
        public string Thumb { get; set; }
        public string Fanart { get; set; }

        public ApiTvSeason ToApi()
        {
            var api = new ApiTvSeason
            {
                Fanart = Fanart,
                Hash = Hash,
                Show = Show,
                Thumb = Thumb,
                IdShow = IdShow,
                EpisodeCount = EpisodeCount,
                SeasonNumber = SeasonNumber
            };
            return api;
        }

        public Yatse2TvSeason() {}

        public Yatse2TvSeason(ApiTvSeason apiTvSeason)
        {
            if (apiTvSeason == null)
                return;
            IdShow = apiTvSeason.IdShow;
            SeasonNumber = apiTvSeason.SeasonNumber;
            Thumb = apiTvSeason.Thumb;
            Fanart = apiTvSeason.Fanart;
            EpisodeCount = apiTvSeason.EpisodeCount;
            Hash = apiTvSeason.Hash;
            Show = apiTvSeason.Show;
        }
    }

}