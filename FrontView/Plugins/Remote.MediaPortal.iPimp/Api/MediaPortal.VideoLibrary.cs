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
using System.Globalization;
using Jayrock.Json;
using Plugin;

namespace Remote.MediaPortal.iPimp.Api
{
    class MediaPortalVideoLibrary : IApiVideoLibrary
    {
        private readonly MediaPortal _parent;

        public MediaPortalVideoLibrary(MediaPortal parent)
        {
            _parent = parent;
        }

        public Collection<ApiTvSeason> GetTvSeasons()
        {
            var seasons = new Collection<ApiTvSeason>();
            if (!_parent.IsConnected())
                return seasons;
            var dblines = _parent.IPimpDBCommand(new CommandInfoIPimp { Action = "getallseasons" }, "seasons");
            if (dblines == null) return seasons;

            foreach (JsonObject dbline in dblines)
            {
                var tvSeason = new ApiTvSeason
                {
                    Fanart = (string)dbline["fanart"],
                    Hash = MediaPortal.Hash((string)dbline["id"]),
                    IdShow = 0,
                    Thumb = (string)dbline["thumb"],
                    EpisodeCount = Convert.ToInt32(dbline["episodecount"], CultureInfo.InvariantCulture),
                    SeasonNumber =  Convert.ToInt32(dbline["seasonnumber"], CultureInfo.InvariantCulture),
                    Show = (string)dbline["show"]
                };

                seasons.Add(tvSeason);
            }

            return seasons;
        }

        public Collection<ApiTvEpisode> GetTvEpisodes() 
        {
            var episodes = new Collection<ApiTvEpisode>();
            if (!_parent.IsConnected())
                return episodes;
            var dblines = _parent.IPimpDBCommand(new CommandInfoIPimp { Action = "getallepisodes" }, "episodes");
            if (dblines == null) return episodes;

            foreach (JsonObject dbline in dblines)
            {
                var tvEpisode = new ApiTvEpisode
                {
                    Fanart = (string)dbline["fanart"],
                    Hash = MediaPortal.Hash((string)dbline["id"]),
                    IdShow = Convert.ToInt32(dbline["idshow"], CultureInfo.InvariantCulture),
                    Thumb = (string)dbline["thumb"],
                    Date = (string)dbline["aired"],
                    Director = (string)dbline["director"],
                    Episode = Convert.ToInt32(dbline["episode"], CultureInfo.InvariantCulture),
                    Season = Convert.ToInt32(dbline["season"], CultureInfo.InvariantCulture),
                    FileName = (string)dbline["filename"],
                    IdEpisode = Convert.ToInt32(dbline["id"], CultureInfo.InvariantCulture),
                    IdFile = Convert.ToInt32(dbline["id"], CultureInfo.InvariantCulture),
                    IsStack = 0,
                    Mpaa = "",
                    Path = (string)dbline["path"],
                    PlayCount = Convert.ToInt32(dbline["watched"], CultureInfo.InvariantCulture),
                    Plot = (string)dbline["plot"],
                    Rating = (string)dbline["rating"],
                    ShowTitle = (string)dbline["show"],
                    Studio = (string)dbline["studio"],
                    Title = (string)dbline["name"]
                };

                episodes.Add(tvEpisode);
            }

            return episodes;
        }

        public Collection<ApiTvShow> GetTvShows()
        {
            var shows = new Collection<ApiTvShow>();
            if (!_parent.IsConnected())
                return shows;
            var dblines = _parent.IPimpDBCommand(new CommandInfoIPimp { Action = "getallseries" }, "series");
            if (dblines == null) return shows;

            foreach (JsonObject dbline in dblines)
            {
                var tvshow = new ApiTvShow
                {
                    Fanart = (string)dbline["fanart"],
                    Hash = MediaPortal.Hash((string)dbline["name"]),
                    Genre = ((string)dbline["genre"]).Trim('|').Replace("|"," / "),
                    IdShow = Convert.ToInt32(dbline["id"], CultureInfo.InvariantCulture),
                    Mpaa = (string)dbline["mpaa"],
                    Plot = (string)dbline["plot"],
                    Premiered = (string)dbline["firstaired"],
                    Rating = ((string)dbline["rating"]).Replace(',', '.'),
                    Studio = (string)dbline["studio"],
                    Title = (string)dbline["name"],
                    TotalCount = Convert.ToInt32(dbline["episodecount"], CultureInfo.InvariantCulture),
                    Thumb = (string)dbline["thumb"],
                    Path = "",
                    IdScraper = ""
                };

                shows.Add(tvshow);
            }

            return shows;
        }

        public Collection<ApiMovie> GetMovies()
        {
            var movies = new Collection<ApiMovie>();
            if (!_parent.IsConnected())
                return movies;
            var dblines = _parent.IPimpDBCommand(new CommandInfoIPimp { Action = "getallmovies" }, "movies");
            if (dblines == null) return movies;

            foreach (JsonObject dbline in dblines)
            {
                var movie = new ApiMovie
                {
                    Title = (string)dbline["title"],
                    Plot = (string)dbline["plot"],
                    Tagline = (string)dbline["tagline"],
                    Votes = (string)dbline["votes"],
                    Rating = Convert.ToDouble(((string)dbline["rating"]).Replace(',','.'), CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture),
                    Year = Convert.ToInt32(dbline["year"], CultureInfo.InvariantCulture),
                    IdScraper = (string)dbline["imdbNumber"],
                    Length = "",
                    Mpaa = (string)dbline["mpaa"],
                    Genre = (string)dbline["genre"],
                    Director = (string)dbline["director"],
                    OriginalTitle = (string)dbline["originaltitle"],
                    Studio = "",
                    IdFile = 0,
                    IdMovie = Convert.ToInt32(dbline["id"], CultureInfo.InvariantCulture),
                    FileName = (string)dbline["files"],
                    Path = (string)dbline["path"],
                    PlayCount = Convert.ToInt32(dbline["watched"], CultureInfo.InvariantCulture),
                    Thumb = (string)dbline["thumb"],
                    Fanart = (string)dbline["fanart"],
                    Hash = MediaPortal.Hash((string)dbline["files"])
                };

                movies.Add(movie);
            }

            dblines = _parent.IPimpDBCommand(new CommandInfoIPimp { Action = "getallmovingpicture" }, "movies");
            if (dblines == null) return movies;

            foreach (JsonObject dbline in dblines)
            {
                var movie = new ApiMovie
                {
                    Title = (string)dbline["title"],
                    Plot = (string)dbline["plot"],
                    Tagline = (string)dbline["tagline"],
                    Votes = (string)dbline["votes"],
                    Rating = Convert.ToDouble(((string)dbline["rating"]).Replace(',', '.'), CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture),
                    Year = Convert.ToInt32(dbline["year"], CultureInfo.InvariantCulture),
                    IdScraper = (string)dbline["imdbNumber"],
                    Length = "",
                    Mpaa = (string)dbline["mpaa"],
                    Genre = (string)dbline["genre"],
                    Director = (string)dbline["director"],
                    OriginalTitle = (string)dbline["originaltitle"],
                    Studio = "",
                    IdFile = 1,
                    IdMovie = Convert.ToInt32(dbline["id"], CultureInfo.InvariantCulture),
                    FileName = (string)dbline["files"],
                    Path = (string)dbline["path"],
                    PlayCount = Convert.ToInt32(dbline["watched"], CultureInfo.InvariantCulture),
                    Thumb = (string)dbline["thumb"],
                    Fanart = (string)dbline["fanart"],
                    Hash = MediaPortal.Hash((string)dbline["files"])
                };

                movies.Add(movie);
            }

            return movies;
        }


    }
}
