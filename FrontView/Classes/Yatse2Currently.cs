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

namespace FrontView.Classes
{
    
    public class Yatse2Currently: DependencyObject
    {

        public static readonly DependencyProperty UnknownFileProperty =
            DependencyProperty.Register("UnknownFile", typeof(string), typeof(Yatse2Currently));

        public string UnknownFile
        {
            get { return (string)GetValue(UnknownFileProperty); }
            set { SetValue(UnknownFileProperty, value); }
        }

        public static readonly DependencyProperty MusicYearProperty =
            DependencyProperty.Register("MusicYear", typeof(string), typeof(Yatse2Currently));

        public string MusicYear
        {
            get { return (string)GetValue(MusicYearProperty); }
            set { SetValue(MusicYearProperty, value); }
        }

        public static readonly DependencyProperty MusicGenreProperty =
            DependencyProperty.Register("MusicGenre", typeof(string), typeof(Yatse2Currently));

        public string MusicGenre
        {
            get { return (string)GetValue(MusicGenreProperty); }
            set { SetValue(MusicGenreProperty, value); }
        }

        public static readonly DependencyProperty MusicBiographyProperty =
            DependencyProperty.Register("MusicBiography", typeof(string), typeof(Yatse2Currently));

        public string MusicBiography
        {
            get { return (string)GetValue(MusicBiographyProperty); }
            set { SetValue(MusicBiographyProperty, value); }
        }

        public static readonly DependencyProperty MusicTrackProperty =
            DependencyProperty.Register("MusicTrack", typeof(string), typeof(Yatse2Currently));

        public string MusicTrack
        {
            get { return (string)GetValue(MusicTrackProperty); }
            set { SetValue(MusicTrackProperty, value); }
        }

        public static readonly DependencyProperty MusicAlbumProperty =
            DependencyProperty.Register("MusicAlbum", typeof(string), typeof(Yatse2Currently));

        public string MusicAlbum
        {
            get { return (string)GetValue(MusicAlbumProperty); }
            set { SetValue(MusicAlbumProperty, value); }
        }

        public static readonly DependencyProperty MusicSongProperty =
            DependencyProperty.Register("MusicSong", typeof(string), typeof(Yatse2Currently));

        public string MusicSong
        {
            get { return (string)GetValue(MusicSongProperty); }
            set { SetValue(MusicSongProperty, value); }
        }

        public static readonly DependencyProperty MusicArtistProperty =
            DependencyProperty.Register("MusicArtist", typeof(string), typeof(Yatse2Currently));

        public string MusicArtist
        {
            get { return (string)GetValue(MusicArtistProperty); }
            set { SetValue(MusicArtistProperty, value); }
        }

        public static readonly DependencyProperty MovieTitleProperty =
            DependencyProperty.Register("MovieTitle", typeof(string), typeof(Yatse2Currently));

        public string MovieTitle
        {
            get { return (string)GetValue(MovieTitleProperty); }
            set { SetValue(MovieTitleProperty, value); }
        }

        public static readonly DependencyProperty MovieYearProperty =
            DependencyProperty.Register("MovieYear", typeof(string), typeof(Yatse2Currently));

        public string MovieYear
        {
            get { return (string)GetValue(MovieYearProperty); }
            set { SetValue(MovieYearProperty, value); }
        }

        public static readonly DependencyProperty MovieDirectorProperty =
            DependencyProperty.Register("MovieDirector", typeof(string), typeof(Yatse2Currently));

        public string MovieDirector
        {
            get { return (string)GetValue(MovieDirectorProperty); }
            set { SetValue(MovieDirectorProperty, value); }
        }

        public static readonly DependencyProperty MoviePlotProperty =
            DependencyProperty.Register("MoviePlot", typeof(string), typeof(Yatse2Currently));

        public string MoviePlot
        {
            get { return (string)GetValue(MoviePlotProperty); }
            set { SetValue(MoviePlotProperty, value); }
        }

        public static readonly DependencyProperty MovieNoteProperty =
            DependencyProperty.Register("MovieNote", typeof(string), typeof(Yatse2Currently));

        public string MovieNote
        {
            get { return (string)GetValue(MovieNoteProperty); }
            set { SetValue(MovieNoteProperty, value); }
        }


        public static readonly DependencyProperty MovieVotesProperty =
            DependencyProperty.Register("MovieVotes", typeof(string), typeof(Yatse2Currently));

        public string MovieVotes
        {
            get { return (string)GetValue(MovieVotesProperty); }
            set { SetValue(MovieVotesProperty, value); }
        }

        public static readonly DependencyProperty MovieStudioProperty =
            DependencyProperty.Register("MovieStudio", typeof(string), typeof(Yatse2Currently));

        public string MovieStudio
        {
            get { return (string)GetValue(MovieStudioProperty); }
            set { SetValue(MovieStudioProperty, value); }
        }

        public static readonly DependencyProperty TvTitleProperty =
            DependencyProperty.Register("TvTitle", typeof(string), typeof(Yatse2Currently));

        public string TvTitle
        {
            get { return (string)GetValue(TvTitleProperty); }
            set { SetValue(TvTitleProperty, value); }
        }

        public static readonly DependencyProperty TvPlotProperty =
            DependencyProperty.Register("TvPlot", typeof(string), typeof(Yatse2Currently));

        public string TvPlot
        {
            get { return (string)GetValue(TvPlotProperty); }
            set { SetValue(TvPlotProperty, value); }
        }

        public static readonly DependencyProperty TvDirectorProperty =
            DependencyProperty.Register("TvDirector", typeof(string), typeof(Yatse2Currently));

        public string TvDirector
        {
            get { return (string)GetValue(TvDirectorProperty); }
            set { SetValue(TvDirectorProperty, value); }
        }

        public static readonly DependencyProperty TvStudioProperty =
            DependencyProperty.Register("TvStudio", typeof(string), typeof(Yatse2Currently));

        public string TvStudio
        {
            get { return (string)GetValue(TvStudioProperty); }
            set { SetValue(TvStudioProperty, value); }
        }

        public static readonly DependencyProperty TvAiredProperty =
            DependencyProperty.Register("TvAired", typeof(string), typeof(Yatse2Currently));

        public string TvAired
        {
            get { return (string)GetValue(TvAiredProperty); }
            set { SetValue(TvAiredProperty, value); }
        }

        public static readonly DependencyProperty TvEpisodeProperty =
            DependencyProperty.Register("TvEpisode", typeof(string), typeof(Yatse2Currently));

        public string TvEpisode
        {
            get { return (string)GetValue(TvEpisodeProperty); }
            set { SetValue(TvEpisodeProperty, value); }
        }

        public static readonly DependencyProperty TvYearProperty =
            DependencyProperty.Register("TvYear", typeof(string), typeof(Yatse2Currently));

        public string TvYear
        {
            get { return (string)GetValue(TvYearProperty); }
            set { SetValue(TvYearProperty, value); }
        }

        public static readonly DependencyProperty TvNoteProperty =
            DependencyProperty.Register("TvNote", typeof(string), typeof(Yatse2Currently));

        public string TvNote
        {
            get { return (string)GetValue(TvNoteProperty); }
            set { SetValue(TvNoteProperty, value); }
        }

        public static readonly DependencyProperty TvVotesProperty =
            DependencyProperty.Register("TvVotes", typeof(string), typeof(Yatse2Currently));

        public string TvVotes
        {
            get { return (string)GetValue(TvVotesProperty); }
            set { SetValue(TvVotesProperty, value); }
        }

        public static readonly DependencyProperty TvShowProperty =
            DependencyProperty.Register("TvShow", typeof(string), typeof(Yatse2Currently));

        public string TvShow
        {
            get { return (string)GetValue(TvShowProperty); }
            set { SetValue(TvShowProperty, value); }
        }

        public static readonly DependencyProperty ThumbProperty =
            DependencyProperty.Register("Thumb", typeof(string), typeof(Yatse2Currently));

        public string Thumb
        {
            get { return (string)GetValue(ThumbProperty); }
            set { SetValue(ThumbProperty, value); }
        }

        public static readonly DependencyProperty FanartProperty =
            DependencyProperty.Register("Fanart", typeof(string), typeof(Yatse2Currently));

        public string Fanart
        {
            get { return (string)GetValue(FanartProperty); }
            set { SetValue(FanartProperty, value); }
        }


        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(string), typeof(Yatse2Currently));

        public string Time
        {
            get { return (string)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(string), typeof(Yatse2Currently));

        public string Duration
        {
            get { return (string)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        public static readonly DependencyProperty ProgressProperty =
            DependencyProperty.Register("Progress", typeof(long), typeof(Yatse2Currently));

        public long Progress
        {
            get { return (long)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }

        public static readonly DependencyProperty VolumeProperty =
            DependencyProperty.Register("Volume", typeof(long), typeof(Yatse2Currently));

        public long Volume
        {
            get { return (long)GetValue(VolumeProperty); }
            set { SetValue(VolumeProperty, value); }
        }

        public static readonly DependencyProperty IsPlayingProperty =
            DependencyProperty.Register("IsPlaying", typeof(bool), typeof(Yatse2Currently));

        public bool IsPlaying
        {
            get { return (bool)GetValue(IsPlayingProperty); }
            set { SetValue(IsPlayingProperty, value); }
        }

        public static readonly DependencyProperty IsPausedProperty =
            DependencyProperty.Register("IsPaused", typeof(bool), typeof(Yatse2Currently));

        public bool IsPaused
        {
            get { return (bool)GetValue(IsPausedProperty); }
            set { SetValue(IsPausedProperty, value); }
        }

       

        public static readonly DependencyProperty IsTvDetailsProperty =
            DependencyProperty.Register("IsTvDetails", typeof(bool), typeof(Yatse2Currently));

        public bool IsTvDetails
        {
            get { return (bool)GetValue(IsTvDetailsProperty); }
            set {
                SetValue(IsTvDetailsProperty, value);
                SetValue(IsNotTvDetailsProperty, !value);
                SetValue(IsTvDetails480Property, false);
            }
        }

        public static readonly DependencyProperty IsTvDetails480Property =
            DependencyProperty.Register("IsTvDetails480", typeof(bool), typeof(Yatse2Currently));

        public bool IsTvDetails480
        {
            get { return (bool)GetValue(IsTvDetails480Property); }
            set
            {
                SetValue(IsTvDetails480Property, value);
                SetValue(IsTvDetailsProperty, false);
                SetValue(IsNotTvDetailsProperty, !value);
            }
        }

        public static readonly DependencyProperty IsNotTvDetailsProperty =
            DependencyProperty.Register("IsNotTvDetails", typeof(bool), typeof(Yatse2Currently));

        public bool IsNotTvDetails
        {
            get { return (bool)GetValue(IsNotTvDetailsProperty); }
            set { SetValue(IsNotTvDetailsProperty, value); }
        }

        public static readonly DependencyProperty IsMusicDetailsProperty =
            DependencyProperty.Register("IsMusicDetails", typeof(bool), typeof(Yatse2Currently));

        public bool IsMusicDetails
        {
            get { return (bool)GetValue(IsMusicDetailsProperty); }
            set
            {
                SetValue(IsMusicDetailsProperty, value);
                SetValue(IsMusicDetails480Property, false);
                SetValue(IsNotMusicDetailsProperty, !value);
            }
        }

        public static readonly DependencyProperty IsMusicDetails480Property =
            DependencyProperty.Register("IsMusicDetails480", typeof(bool), typeof(Yatse2Currently));

        public bool IsMusicDetails480
        {
            get { return (bool)GetValue(IsMusicDetails480Property); }
            set
            {
                SetValue(IsMusicDetails480Property, value);
                SetValue(IsMusicDetailsProperty, false);
                SetValue(IsNotMusicDetailsProperty, !value);
            }
        }

        public static readonly DependencyProperty IsNotMusicDetailsProperty =
            DependencyProperty.Register("IsNotMusicDetails", typeof(bool), typeof(Yatse2Currently));

        public bool IsNotMusicDetails
        {
            get { return (bool)GetValue(IsNotMusicDetailsProperty); }
            set { SetValue(IsNotMusicDetailsProperty, value); }
        }

        public static readonly DependencyProperty IsMovieDetailsProperty =
            DependencyProperty.Register("IsMovieDetails", typeof(bool), typeof(Yatse2Currently));

        public bool IsMovieDetails
        {
            get { return (bool)GetValue(IsMovieDetailsProperty); }
            set
            {
                SetValue(IsMovieDetailsProperty, value);
                SetValue(IsMovieDetails480Property, false);
                SetValue(IsNotMovieDetailsProperty, !value);
            }
        }

        public static readonly DependencyProperty IsMovieDetails480Property =
            DependencyProperty.Register("IsMovieDetails480", typeof(bool), typeof(Yatse2Currently));

        public bool IsMovieDetails480
        {
            get { return (bool)GetValue(IsMovieDetails480Property); }
            set
            {
                SetValue(IsMovieDetails480Property, value);
                SetValue(IsMovieDetailsProperty, false);
                SetValue(IsNotMovieDetailsProperty, !value);
            }
        }

        public static readonly DependencyProperty IsNotMovieDetailsProperty =
            DependencyProperty.Register("IsNotMovieDetails", typeof(bool), typeof(Yatse2Currently));

        public bool IsNotMovieDetails
        {
            get { return (bool)GetValue(IsNotMovieDetailsProperty); }
            set { SetValue(IsNotMovieDetailsProperty, value); }
        }


        public static readonly DependencyProperty IsMovieProperty =
            DependencyProperty.Register("IsMovie", typeof(bool), typeof(Yatse2Currently));

        public bool IsMovie
        {
            get { return (bool)GetValue(IsMovieProperty); }
            set { 
                SetValue(IsMovieProperty, value);
                SetValue(IsMusicProperty, !value);
                SetValue(IsTvProperty, !value);
                SetValue(IsNothingProperty, !value);
                SetValue(IsUnknownProperty, !value);
            }
        }

        public static readonly DependencyProperty IsMusicProperty =
            DependencyProperty.Register("IsMusic", typeof(bool), typeof(Yatse2Currently));

        public bool IsMusic
        {
            get { return (bool)GetValue(IsMusicProperty); }
            set {
                SetValue(IsMovieProperty, !value);
                SetValue(IsMusicProperty, value);
                SetValue(IsTvProperty, !value);
                SetValue(IsNothingProperty, !value);
                SetValue(IsUnknownProperty, !value);
            }
        }

        public static readonly DependencyProperty IsTvProperty =
            DependencyProperty.Register("IsTv", typeof(bool), typeof(Yatse2Currently));

        public bool IsTv
        {
            get { return (bool)GetValue(IsTvProperty); }
            set { 
                SetValue(IsMovieProperty, !value);
                SetValue(IsMusicProperty, !value);
                SetValue(IsTvProperty, value);
                SetValue(IsNothingProperty, !value);
                SetValue(IsUnknownProperty, !value);
            }
        }


        public static readonly DependencyProperty IsUnknownProperty =
            DependencyProperty.Register("IsUnknown", typeof(bool), typeof(Yatse2Currently));

        public bool IsUnknown
        {
            get { return (bool)GetValue(IsUnknownProperty); }
            set
            {
                SetValue(IsMovieProperty, !value);
                SetValue(IsMusicProperty, !value);
                SetValue(IsTvProperty, !value);
                SetValue(IsNothingProperty, !value);
                SetValue(IsUnknownProperty, value);
            }
        }

        public static readonly DependencyProperty IsNothingProperty =
            DependencyProperty.Register("IsNothing", typeof(bool), typeof(Yatse2Currently));

        public bool IsNothing
        {
            get { return (bool)GetValue(IsNothingProperty); }
            set {
                SetValue(IsMovieProperty, !value);
                SetValue(IsMusicProperty, !value);
                SetValue(IsTvProperty, !value);
                SetValue(IsNothingProperty, value);
                SetValue(IsUnknownProperty, !value);
            }
        }
    }
}