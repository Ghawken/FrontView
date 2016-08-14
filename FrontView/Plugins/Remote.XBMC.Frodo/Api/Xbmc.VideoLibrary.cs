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
using Jayrock.Json;

namespace Remote.XBMC.Frodo.Api
{
  class XbmcVideoLibrary : IApiVideoLibrary
  {
    private readonly Xbmc _parent;

    public XbmcVideoLibrary(Xbmc parent)
    {
      _parent = parent;
    }

    public Collection<ApiTvSeason> GetTvSeasons()
    {
      var seasons = new Collection<ApiTvSeason>();

      var properties = new JsonArray(new[] { "title" });
      var param = new JsonObject();
      param["properties"] = properties;
      var result = (JsonObject)_parent.JsonCommand("VideoLibrary.GetTVShows", param);
      if (result != null)
      {
        if (result.Contains("tvshows"))
        {
          foreach (JsonObject show in (JsonArray)result["tvshows"])
          {
            var properties2 =
              new JsonArray(new[] { "tvshowid", "fanart", "thumbnail", "season", "showtitle", "episode" });
            var param2 = new JsonObject();
            param2["properties"] = properties2;
            param2["tvshowid"] = (long)(JsonNumber)show["tvshowid"];
            var result2 = (JsonObject)_parent.JsonCommand("VideoLibrary.GetSeasons", param2);
            if (result2 == null) continue;
            if (!result2.Contains("seasons")) continue;
            foreach (JsonObject genre in (JsonArray)result2["seasons"])
            {
              try
              {
                var tvShow = new ApiTvSeason
                  {
                    SeasonNumber = (long)(JsonNumber)genre["season"],
                    IdShow = (long)(JsonNumber)genre["tvshowid"],
                    Show = genre["showtitle"].ToString(),
                    Thumb = genre["thumbnail"].ToString(),
                    EpisodeCount = (long)(JsonNumber)genre["episode"],
                    Fanart = genre["fanart"].ToString(),
                    Hash = Xbmc.Hash(genre["thumbnail"].ToString())
                  };
                seasons.Add(tvShow);
              }
              catch (Exception)
              {
              }
            }
          }
        }
      }
      return seasons;
    }

    public Collection<ApiTvSeason> GetTvSeasonsRefresh()
    {
        var seasons = new Collection<ApiTvSeason>();

        var properties = new JsonArray(new[] { "title" });
        var param = new JsonObject();
        param["properties"] = properties;

        // First 100 Date sorted
        var param2 = new JsonObject();
        param2.Add("start", 0);
        param2.Add("end", 10);
        var param3 = new JsonObject();
        param3.Add("order", "descending");
        param3.Add("method", "dateadded");
        param.Add("sort", param3);
        param.Add("limits", param2);

        var result = (JsonObject)_parent.JsonCommand("VideoLibrary.GetTVShows", param);
        if (result != null)
        {
            if (result.Contains("tvshows"))
            {
                foreach (JsonObject show in (JsonArray)result["tvshows"])
                {
                    var properties2 =
                      new JsonArray(new[] { "tvshowid", "fanart", "thumbnail", "season", "showtitle", "episode" });
                    var param22 = new JsonObject();
                    param22["properties"] = properties2;
                    param22["tvshowid"] = (long)(JsonNumber)show["tvshowid"];

                    var result2 = (JsonObject)_parent.JsonCommand("VideoLibrary.GetSeasons", param22);
                    if (result2 == null) continue;
                    if (!result2.Contains("seasons")) continue;
                    foreach (JsonObject genre in (JsonArray)result2["seasons"])
                    {
                        try
                        {

                            _parent.Trace("Kodi QuickRefresh Seasons:  SeasonNumber:" + (long)(JsonNumber)genre["season"]);
                            _parent.Trace("Kodi QuickRefresh Seasons:  IdShow:" + (long)(JsonNumber)genre["tvshowid"]);
                            _parent.Trace("Kodi QuickRefresh Seasons:  Show:" + genre["showtitle"].ToString());
                            _parent.Trace("Kodi QuickRefresh Seasons:  Thumb:" + genre["thumbnail"].ToString());
                            _parent.Trace("Kodi QuickRefresh Seasons:  EpisodeCount:" + (long)(JsonNumber)genre["episode"]);
                            _parent.Trace("Kodi QuickRefresh Seasons:  Fanart:" + genre["fanart"].ToString());
                            _parent.Trace("Kodi QuickRefresh Seasons:  Hash:" + genre["thumbnail"].ToString());

                            var tvShow = new ApiTvSeason
                            {
                                SeasonNumber = (long)(JsonNumber)genre["season"],
                                IdShow = (long)(JsonNumber)genre["tvshowid"],
                                Show = genre["showtitle"].ToString(),
                                Thumb = genre["thumbnail"].ToString(),
                                EpisodeCount = (long)(JsonNumber)genre["episode"],
                                Fanart = genre["fanart"].ToString(),
                                Hash = Xbmc.Hash(genre["thumbnail"].ToString())
                            };
                            seasons.Add(tvShow);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }
        return seasons;
    }

    public Collection<ApiTvEpisode> GetTvEpisodesRefresh()
    {
      var episodes = new Collection<ApiTvEpisode>();
        
       var properties = new JsonArray(new[] { "title", "plot", "season", "episode", "showtitle", "tvshowid", "fanart", "thumbnail", "rating", "playcount", "firstaired" });
      var param = new JsonObject();
      param["properties"] = properties;
        // First 100 Date sorted
      var param2 = new JsonObject();
      param2.Add("start",0);
        param2.Add("end",30);  //Number of Episodes QUick Refresh Gets
        var param3 = new JsonObject();
          param3.Add("order", "descending");     
        param3.Add("method", "dateadded");
        param.Add("sort", param3);
        param.Add("limits", param2);
        var result = (JsonObject)_parent.JsonCommand("VideoLibrary.GetEpisodes", param);
        
        if (result != null)
        {
            if (result.Contains("episodes"))
            {
                foreach (JsonObject genre in (JsonArray)result["episodes"])
                {
                    try
                    {
                        var tvShow = new ApiTvEpisode
                        {
                            Title = genre["title"].ToString(),
                            Plot = genre["plot"].ToString(),
                            Rating = genre["rating"].ToString(),
                            Mpaa = "",
                            Date = genre["firstaired"].ToString(),
                            Director = "",
                            PlayCount = 0,
                            Studio = "",
                            IdEpisode = (long)(JsonNumber)genre["episodeid"],
                            IdShow = (long)(JsonNumber)genre["tvshowid"],
                            Season = (long)(JsonNumber)genre["season"],
                            Episode = (long)(JsonNumber)genre["episode"],
                            Path = "",
                            ShowTitle = genre["showtitle"].ToString(),
                            Thumb = genre["thumbnail"].ToString(),
                            Fanart = genre["fanart"].ToString(),
                            Hash = Xbmc.Hash(genre["thumbnail"].ToString())
                        };
                        episodes.Add(tvShow);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
        return episodes;
    }



    public Collection<ApiTvEpisode> GetTvEpisodes()
    {
      var episodes = new Collection<ApiTvEpisode>();

      var properties = new JsonArray(new[] { "title", "plot", "season", "episode", "showtitle", "tvshowid", "fanart", "thumbnail", "rating", "playcount", "firstaired" });
      var param = new JsonObject();
      param["properties"] = properties;
      var result = (JsonObject)_parent.JsonCommand("VideoLibrary.GetEpisodes", param);
      if (result != null)
      {
        if (result.Contains("episodes"))
        {
          foreach (JsonObject genre in (JsonArray)result["episodes"])
          {
            try
            {
              var tvShow = new ApiTvEpisode
                {
                  Title = genre["title"].ToString(),
                  Plot = genre["plot"].ToString(),
                  Rating = genre["rating"].ToString(),
                  Mpaa = "",
                  Date = genre["firstaired"].ToString(),
                  Director = "",
                  PlayCount = (long)(JsonNumber)genre["playcount"],
                  Studio = "",
                  IdEpisode = (long)(JsonNumber)genre["episodeid"],
                  IdShow = (long)(JsonNumber)genre["tvshowid"],
                  Season = (long)(JsonNumber)genre["season"],
                  Episode = (long)(JsonNumber)genre["episode"],
                  Path = "",
                  ShowTitle = genre["showtitle"].ToString(),
                  Thumb = genre["thumbnail"].ToString(),
                  Fanart = genre["fanart"].ToString(),
                  Hash = Xbmc.Hash(genre["thumbnail"].ToString())
                };
              episodes.Add(tvShow);
            }
            catch (Exception)
            {
            }
          }
        }
      }
      return episodes;
    }

    public Collection<ApiTvShow> GetTvShows()
    {
      var shows = new Collection<ApiTvShow>();

      var properties = new JsonArray(new[] { "title", "art", "plot", "genre", "fanart", "thumbnail", "rating", "mpaa", "studio", "playcount", "premiered", "episode","file" });
      var param = new JsonObject();
      param["properties"] = properties;
      var result = (JsonObject)_parent.JsonCommand("VideoLibrary.GetTVShows", param);
      if (result != null)
      {
        if (result.Contains("tvshows"))
        {
          foreach (JsonObject genre in (JsonArray)result["tvshows"])
          {
            try
            {
                            var clearlogo = "NONE";
                            var banner = "NONE";
                            // go through art and see.

                            JsonObject results = (JsonObject)genre["art"];

                            if (results != null)
                            {
                                if (results["clearlogo"] != null)
                                {
                                    clearlogo = results["clearlogo"].ToString();
                                }
                                if (results["banner"] != null)
                                {
                                    banner = results["banner"].ToString();
                                }

                            }



                            var tvShow = new ApiTvShow
                {
                  Title = genre["title"].ToString(),
                  Plot = genre["plot"].ToString(),
                  Rating = genre["rating"].ToString(),
                  IdScraper = "",
                  Mpaa = genre["mpaa"].ToString(),
                  Genre = _parent.JsonArrayToString((JsonArray)genre["genre"]),
                  Studio = _parent.JsonArrayToString((JsonArray)genre["studio"]),
                  IdShow = (long)(JsonNumber)genre["tvshowid"],
                  TotalCount = (long)(JsonNumber)genre["episode"],
                  Path = genre["file"].ToString(),
                  Premiered = genre["premiered"].ToString(),
                  Thumb = genre["thumbnail"].ToString(),
                  Fanart = genre["fanart"].ToString(),
                  Logo = clearlogo,
                  Banner = banner,
                  Hash = Xbmc.Hash(genre["thumbnail"].ToString())
                };
              shows.Add(tvShow);
            }
            catch (Exception ex)
            {
                            _parent.Log("GetTVShows : Exception Caught: Json seems to equal :" + ex);

                        }
          }
        }
      }
      return shows;
    }
    public Collection<ApiTvShow> GetTvShowsRefresh()
    {
        var shows = new Collection<ApiTvShow>();

        var properties = new JsonArray(new[] { "title", "art","plot", "genre", "fanart", "thumbnail", "rating", "mpaa", "studio", "playcount", "premiered", "episode", "file" });
        var param = new JsonObject();
        param["properties"] = properties;

        // First 100 Date sorted
        var param2 = new JsonObject();
        param2.Add("start", 0);
        param2.Add("end", 10);   //Number of TV Shows Quick Refresh Gets
        var param3 = new JsonObject();
        param3.Add("order", "descending");
        param3.Add("method", "dateadded");
        param.Add("sort", param3);
        param.Add("limits", param2);


        var result = (JsonObject)_parent.JsonCommand("VideoLibrary.GetTVShows", param);
        if (result != null)
        {
            if (result.Contains("tvshows"))
            {
                foreach (JsonObject genre in (JsonArray)result["tvshows"])
                {
                    try
                    {
                        var clearlogo = "NONE";
                        var banner = "NONE";
                            // go through art and see.

                        JsonObject results = (JsonObject)genre["art"];

                        if (results != null)
                        {
                                if (results["clearlogo"] != null)
                                {
                                    clearlogo = results["clearlogo"].ToString();
                                }
                                if (results["banner"] != null)
                                {
                                    banner = results["banner"].ToString();
                                }

                         }


                        var tvShow = new ApiTvShow
                        {
                            Title = genre["title"].ToString(),
                            Plot = genre["plot"].ToString(),
                            Rating = genre["rating"].ToString(),
                            IdScraper = "",
                            Mpaa = genre["mpaa"].ToString(),
                            Genre = _parent.JsonArrayToString((JsonArray)genre["genre"]),
                            Studio = _parent.JsonArrayToString((JsonArray)genre["studio"]),
                            IdShow = (long)(JsonNumber)genre["tvshowid"],
                            TotalCount = (long)(JsonNumber)genre["episode"],
                            Path = genre["file"].ToString(),
                            Premiered = genre["premiered"].ToString(),
                            Thumb = genre["thumbnail"].ToString(),
                            Fanart = genre["fanart"].ToString(),
                            Logo = clearlogo,
                            Banner = banner,
                            Hash = Xbmc.Hash(genre["thumbnail"].ToString())
                        };
                        shows.Add(tvShow);
                    }
                    catch (Exception ex)
                    {
                            _parent.Log("GetTVShowsRefresh : Exception Caught: Json seems to equal :" + ex);
                     }
                }
            }
        }
        return shows;
    }
        public Collection<ApiMovie> GetMoviesRefresh()
        {
            var movies = new Collection<ApiMovie>();

            var properties = new JsonArray(new[] { "title", "art", "plot", "dateadded", "genre", "year", "fanart", "thumbnail", "playcount", "studio", "rating", "runtime", "mpaa", "originaltitle", "director", "votes", "file" });
            var param = new JsonObject();
            param["properties"] = properties;
            // First 100 Date sorted
            var param2 = new JsonObject();
            param2.Add("start", 0);
            param2.Add("end", 30);  //Number of Movies Quick refresh gets
            var param3 = new JsonObject();
            param3.Add("order", "descending");
            param3.Add("method", "dateadded");
            param.Add("sort", param3);
            param.Add("limits", param2);


            var result = (JsonObject)_parent.JsonCommand("VideoLibrary.GetMovies", param);

            if (result != null)
            {
                if (result.Contains("movies"))
                {
                    foreach (JsonObject genre in (JsonArray)result["movies"])
                    {
                        try
                        {
                            var t = TimeSpan.FromSeconds((long)(JsonNumber)genre["runtime"]);
                            var duration = string.Format("{0:D2}:{1:D2}", t.Hours, t.Minutes);
                            var clearlogo = "NONE";
                            var banner = "NONE";
                            // go through art and see.

                            JsonObject results = (JsonObject)genre["art"];

                            if (results != null)
                            {
                                if (results["clearlogo"] != null)
                                {
                                    clearlogo = results["clearlogo"].ToString();
                                }
                                if (results["banner"] != null)
                                {
                                    banner = results["banner"].ToString();
                                }

                            }

              
                            var movie = new ApiMovie
                            {

                                Title = genre["title"].ToString(),
                                Plot = genre["plot"].ToString(),
                                Votes = genre["votes"].ToString(),
                                Rating = genre["rating"].ToString(),
                                Year = (long)(JsonNumber)genre["year"],
                                IdScraper = "",
                                Length = duration,
                                Mpaa = genre["mpaa"].ToString(),
                                Genre = _parent.JsonArrayToString((JsonArray)genre["genre"]),
                                Director = _parent.JsonArrayToString((JsonArray)genre["director"]),
                                OriginalTitle = genre["originaltitle"].ToString(),
                                Studio = _parent.JsonArrayToString((JsonArray)genre["studio"]),
                                IdFile = 0,
                                IdMovie = (long)(JsonNumber)genre["movieid"],
                                FileName = genre["file"].ToString(),
                                Path = "",
                                PlayCount = 0,
                                Thumb = genre["thumbnail"].ToString(),
                                Fanart = genre["fanart"].ToString(),
                                Logo = clearlogo,
                                Banner = banner,
                                Hash = Xbmc.Hash(genre["thumbnail"].ToString()),
                                DateAdded = genre["dateadded"].ToString()
                            };
                            movies.Add(movie);
                        }
                        catch (Exception ex)
                        {
                            _parent.Log("Exception Caught: Json seems to equal :" + ex);
                        }
                    }

                }
                return movies;

                
            }
            return movies;
        }

    public Collection<ApiMovie> GetMovies()
    {
      var movies = new Collection<ApiMovie>();

        //add dateadded

      var properties = new JsonArray(new[] { "title", "art", "plot", "dateadded", "genre", "year", "fanart", "thumbnail", "playcount", "studio", "rating", "runtime", "mpaa", "originaltitle", "director", "votes", "file" });
      var param = new JsonObject();
      param["properties"] = properties;
      var result = (JsonObject)_parent.JsonCommand("VideoLibrary.GetMovies", param);
      if (result != null)
      {
        if (result.Contains("movies"))
        {
          foreach (JsonObject genre in (JsonArray)result["movies"])
          {
            try
            {
              var t = TimeSpan.FromSeconds((long)(JsonNumber)genre["runtime"]);
              var duration = string.Format("{0:D2}:{1:D2}", t.Hours, t.Minutes);
              var clearlogo = "NONE";
              var banner = "NONE";
                            // go through art and see.

               JsonObject results = (JsonObject)genre["art"];

              if (results != null)
              {
                                if (results["clearlogo"] != null)
                                {
                                    clearlogo = results["clearlogo"].ToString();
                                }
                                if (results["banner"] != null)
                                {
                                    banner = results["banner"].ToString();
                                }
               }



              var movie = new ApiMovie
                {

                  Title = genre["title"].ToString(),
                  Plot = genre["plot"].ToString(),
                  Votes = genre["votes"].ToString(),
                  Rating = genre["rating"].ToString(),
                  Year = (long)(JsonNumber)genre["year"],
                  IdScraper = "",
                  Length = duration,
                  Mpaa = genre["mpaa"].ToString(),
                  Genre = _parent.JsonArrayToString((JsonArray)genre["genre"]),
                  Director = _parent.JsonArrayToString((JsonArray)genre["director"]),
                  OriginalTitle = genre["originaltitle"].ToString(),
                  Studio = _parent.JsonArrayToString((JsonArray)genre["studio"]),
                  IdFile = 0,
                  IdMovie = (long)(JsonNumber)genre["movieid"],
                  FileName = genre["file"].ToString(),
                  Path = "",
                  PlayCount = 0,
                  Thumb = genre["thumbnail"].ToString(),
                  Fanart = genre["fanart"].ToString(),
                  Logo = clearlogo,
                  Banner = banner,
                  Hash = Xbmc.Hash(genre["thumbnail"].ToString()),
                  DateAdded = genre["dateadded"].ToString()
                };
              movies.Add(movie);
            }
            catch (Exception ex)
            {
                            _parent.Log("Exception Caught: Json Clearlogo seems to equal :" + ex);
            }
          }
        }
      }

      return movies;
    }
  }
}
