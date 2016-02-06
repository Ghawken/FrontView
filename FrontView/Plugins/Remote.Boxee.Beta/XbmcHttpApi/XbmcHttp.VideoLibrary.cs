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
using Plugin;

namespace Remote.XBMC.Camelot.XbmcHttpApi
{
    class XbmcHttpVideoLibrary : IApiVideoLibrary
    {
        private readonly XbmcHttp _parent;

        public XbmcHttpVideoLibrary(XbmcHttp parent)
        {
            _parent = parent;
        }

        public Collection<ApiTvSeason> GetTvSeasons()
        {
            var seasons = new Collection<ApiTvSeason>();
            const string req = "SELECT idShow, COUNT(idShow), c12, strPath FROM episodeview GROUP BY idShow,c12";

            var dblines = _parent.DBCommand("video", req);
            if (dblines == null) return seasons;
            foreach (var dbline in dblines)
            {
                if (dbline.Length < 4)
                {
                    _parent.Log("Invalid request DATA : " + dbline);
                    continue;
                }
                var season = new ApiTvSeason
                {
                    EpisodeCount = XbmcHttp.StringToNumber(dbline[1]),
                    IdShow = XbmcHttp.StringToNumber(dbline[0]),
                    SeasonNumber = XbmcHttp.StringToNumber(dbline[2])
                };

                var temp = dbline[3];
                season.Fanart = @"special://profile/Thumbnails/Video/Fanart/" + XbmcHttp.Hash(temp) + ".tbn";
                char[] charsToTrim = { '/' };
                temp = temp.TrimEnd(charsToTrim);
                var hash = XbmcHttp.Hash("season" + temp);
                season.Thumb = @"special://profile/Thumbnails/Video/" + hash[0] + "/" + hash + ".tbn";
                season.Hash = temp;
                seasons.Add(season);
            }
            return seasons;
        }

        public Collection<ApiTvEpisode> GetTvEpisodes() 
        {
            var episodes = new Collection<ApiTvEpisode>();
            const string req = "SELECT idEpisode,c00,c01,c03,c05,c10,c12,c13,idFile,strFileName,strPath,playCount,strTitle,strStudio,idShow,mpaa FROM episodeview ";

            var dblines = _parent.DBCommand("video", req);
            if (dblines == null) return episodes;
            foreach (var dbline in dblines)
            {
                if (dbline.Length < 15)
                {
                    _parent.Log("Invalid request DATA : " + dbline);
                    continue;
                }
                dbline[3] = dbline[3].Length > 3 ? dbline[3] : "0.0";
                var episode = new ApiTvEpisode
                                  {
                                      IdEpisode = XbmcHttp.StringToNumber(dbline[0]),
                                      Title = dbline[1],
                                      Plot = dbline[2],
                                      Rating = dbline[3].Substring(0, 3).Trim('.'),
                                      Date = dbline[4],
                                      Director = dbline[5],
                                      Season = XbmcHttp.StringToNumber(dbline[6]),
                                      Episode = XbmcHttp.StringToNumber(dbline[7]),
                                      IdFile = XbmcHttp.StringToNumber(dbline[8]),
                                      FileName = dbline[9],
                                      Path = dbline[10],
                                      PlayCount = XbmcHttp.StringToNumber(dbline[11]),
                                      ShowTitle = dbline[12],
                                      Studio = dbline[13],
                                      IdShow = XbmcHttp.StringToNumber(dbline[14]),
                                      Mpaa = dbline[15]
                                  };
                if (episode.FileName.StartsWith("stack://",StringComparison.OrdinalIgnoreCase))
                {
                    var temp = episode.FileName.Split(new[] { " , " }, StringSplitOptions.None);
                    episode.IsStack = 1;
                    episode.Hash = XbmcHttp.Hash(temp[0].Replace("stack://", ""));
                    episode.Thumb = @"special://profile/Thumbnails/Video/" + episode.Hash[0] + "/" + episode.Hash + ".tbn";
                    episode.Fanart = @"special://profile/Thumbnails/Video/Fanart/" + XbmcHttp.Hash(episode.Path) + ".tbn";
                }
                else
                {
                    episode.IsStack = 0;
                    episode.Hash = XbmcHttp.Hash(episode.Path + episode.FileName);
                    episode.Thumb = @"special://profile/Thumbnails/Video/" + episode.Hash[0] + "/" + episode.Hash + ".tbn";
                    episode.Fanart = @"special://profile/Thumbnails/Video/Fanart/" + XbmcHttp.Hash(episode.Path) + ".tbn";
                }
                episodes.Add(episode);
            }
            return episodes;
        }

        public Collection<ApiTvShow> GetTvShows()
        {
            var shows = new Collection<ApiTvShow>();
            var dblines = _parent.DBCommand("video","SELECT idShow,c00,c01,c04,c05,c08,c12,c13,c14,strPath,totalCount FROM tvshowview");
            if (dblines == null) return shows;
            foreach (var dbline in dblines)
            {
                if (dbline.Length < 11)
                {
                    _parent.Log("Invalid request DATA : " + dbline);
                    continue;
                }
                dbline[3] = dbline[3].Length > 3 ? dbline[3] : "0.0";
                var show = new ApiTvShow
                               {
                                   IdShow = XbmcHttp.StringToNumber(dbline[0]),
                                   Title = dbline[1],
                                   Plot = dbline[2],
                                   Rating = dbline[3].Substring(0, 3).Trim('.'),
                                   Premiered = dbline[4],
                                   Genre = dbline[5],
                                   IdScraper = dbline[6],
                                   Mpaa = dbline[7],
                                   Studio = dbline[8],
                                   Path = dbline[9],
                                   TotalCount = XbmcHttp.StringToNumber(dbline[10])
                               };
                show.Hash = XbmcHttp.Hash(show.Path);
                show.Thumb = @"special://profile/Thumbnails/Video/" + show.Hash[0] + "/" + show.Hash + ".tbn";
                show.Fanart = @"special://profile/Thumbnails/Video/Fanart/" + show.Hash + ".tbn";
                shows.Add(show);
            }
            return shows;
        }

        public Collection<ApiMovie> GetMovies()
        {
            var movies = new Collection<ApiMovie>();
            var dblines = _parent.DBCommand("video","SELECT c00,c01,c03,c04,c05,c07,C09,c11,c12,c14,c15,c16,c18,idFile,idMovie,strFileName,strPath,playCount FROM movieview");
            if (dblines == null) return movies;
            foreach (var dbline in dblines)
            {
                if (dbline.Length < 18)
                {
                    _parent.Log("Invalid request DATA : " + dbline);
                    continue;
                }
                dbline[4] = dbline[4].Length > 3 ? dbline[4] : "0.0";
                var movie = new ApiMovie
                                {
                                    Title = dbline[0],
                                    Plot = dbline[1],
                                    Tagline = dbline[2],
                                    Votes = dbline[3],
                                    Rating = dbline[4].Substring(0, 3).Trim('.'),
                                    Year = XbmcHttp.StringToNumber(dbline[5]),
                                    IdScraper = dbline[6],
                                    Length = dbline[7],
                                    Mpaa = dbline[8],
                                    Genre = dbline[9],
                                    Director = dbline[10],
                                    OriginalTitle = dbline[11],
                                    Studio = dbline[12],
                                    IdFile = XbmcHttp.StringToNumber(dbline[13]),
                                    IdMovie = XbmcHttp.StringToNumber(dbline[14]),
                                    FileName = dbline[15],
                                    Path = dbline[16],
                                    PlayCount = XbmcHttp.StringToNumber(dbline[17])
                                };

                if (movie.FileName.StartsWith("stack://",StringComparison.OrdinalIgnoreCase))
                {
                    var temp = movie.FileName.Split(new[] { " , " }, StringSplitOptions.None);
                    movie.IsStack = 1;
                    movie.Hash = XbmcHttp.Hash(temp[0].Replace("stack://", ""));
                    movie.Thumb = @"special://profile/Thumbnails/Video/" + movie.Hash[0] + "/" + movie.Hash + ".tbn";
                    movie.Fanart = @"special://profile/Thumbnails/Video/Fanart/" + XbmcHttp.Hash(movie.FileName) + ".tbn";
                }
                else
                {
                    movie.IsStack = 0;
                    movie.Hash = XbmcHttp.Hash(movie.Path + movie.FileName);
                    movie.Thumb = @"special://profile/Thumbnails/Video/" + movie.Hash[0] + "/" + movie.Hash + ".tbn";
                    movie.Fanart = @"special://profile/Thumbnails/Video/Fanart/" + movie.Hash + ".tbn";
                }

                movies.Add(movie);
            }
            return movies;
        }


    }
}
