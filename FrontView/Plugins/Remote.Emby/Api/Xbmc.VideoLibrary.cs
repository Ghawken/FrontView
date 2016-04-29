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
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Net;
using System.Web.Script.Serialization;
using System.Web;
using System.Web.Script;
using Plugin;
//using Jayrock.Json;

using System.Xml;
using System.Xml.Serialization;
using System.Linq;


namespace Remote.Emby.Api
{
  class XbmcVideoLibrary : IApiVideoLibrary
  {
    private readonly Xbmc _parent;

    public XbmcVideoLibrary(Xbmc parent)
    {
      _parent = parent;
    }
    public Collection<ApiTvSeason> GetTvSeasonsRefresh()
    {
        return null;
    }

    public Collection<ApiTvSeason> GetTvSeasons()
    {
      var seasons = new Collection<ApiTvSeason>();

      try
      {


          _parent.Trace("Getting TV Seasons" + _parent.IP);
          string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items?Recursive=true&IncludeItemTypes=Season";

          var request = WebRequest.CreateHttp(NPurl);

          request.Method = "get";
          request.Timeout = 150000;
          _parent.Trace("Single TV Season Selection: " + _parent.IP + ":" + _parent.Port);

          var authString = _parent.GetAuthString();

          request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
          request.Headers.Add("X-Emby-Authorization", authString);
          request.ContentType = "application/json; charset=utf-8";
          request.Accept = "application/json; charset=utf-8";

          var response = request.GetResponse();

          if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
          {

              System.IO.Stream dataStream = response.GetResponseStream();
              //REMOVETHIS                System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);

              using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
              {
                  string json = sr.ReadToEnd();
                  _parent.Trace("--------------GETTING Single TV Season Selection Result ------" + json);

                  var deserializer = new JavaScriptSerializer();

                  var ItemData = deserializer.Deserialize<TVSeasons.Rootobject>(json);
                  _parent.Trace("---------------Get Single TV Season Selection:  Issue: Results.Taglines: " + ItemData.TotalRecordCount);

                  foreach (var genre in ItemData.Items)
                  {
                      try
                      {

                          //var SingleTVData = GetSingleTVFromSeries(genre.Id);

                          var tvShow = new ApiTvSeason
                          {
                              SeasonNumber = genre.IndexNumber,
                              IdShow = Xbmc.IDtoNumber(genre.SeriesId),
                              Show = genre.SeriesName ?? "",
                              Thumb = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.Id + "/Images/Primary" ?? "",
                              EpisodeCount = genre.ChildCount,
                              Fanart = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.SeriesId + "/Images/Backdrop" ?? "",
                              Hash = Xbmc.Hash(genre.Id)
                          };
                          seasons.Add(tvShow);
/*
                          var tvShow = new ApiTvShow
                          {

                              Title = genre.Name ?? "Unknown",
                              Plot = SingleTVData.Overview ?? "",
                              Rating = genre.CommunityRating.ToString() ?? "",
                              IdScraper = "",
                              Mpaa = SingleTVData.OfficialRating ?? "Unknown",
                              Genre = SingleTVData.Genres.FirstOrDefault().ToString() ?? "",
                              Studio = SingleTVData.Studios.FirstOrDefault().Name.ToString() ?? "",
                              IdShow = Xbmc.IDtoNumber(genre.Id),
                              TotalCount = genre.RecursiveItemCount,
                              Path = SingleTVData.Path ?? "",
                              Premiered = genre.PremiereDate.ToString() ?? "",
                              Thumb = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.Id + "/Images/Primary" ?? "",
                              Fanart = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.Id + "/Images/Backdrop" ?? "",
                              Hash = Xbmc.Hash(genre.Id)

                          };

                          shows.Add(tvShow);*/
                      }
                      catch (Exception ex)
                      {
                          _parent.Trace("TV Shows Exception Caught " + ex);
                      }
                  }

              }
          }
      }
      catch (Exception Ex)
      {
          _parent.Trace("Another tV SHows exception" + Ex);
      }


    /*
      var properties = new Jayrock.Json.JsonArray(new[] { "title" });
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
    */
      return seasons;
    }

    public Collection<ApiTvEpisode> GetTvEpisodes()
    {
      var episodes = new Collection<ApiTvEpisode>();

      try
      {


          _parent.Trace("Getting TV Episodes: Parent IP: " + _parent.IP);
          string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items?Recursive=true&IncludeItemTypes=Episode";

          var request = WebRequest.CreateHttp(NPurl);

          request.Method = "get";
          request.Timeout = 150000;
          _parent.Trace("Single TV Episode Selection: " + _parent.IP + ":" + _parent.Port);

          var authString = _parent.GetAuthString();

          request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
          request.Headers.Add("X-Emby-Authorization", authString);
          request.ContentType = "application/json; charset=utf-8";
          request.Accept = "application/json; charset=utf-8";

          var response = request.GetResponse();

          if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
          {

              System.IO.Stream dataStream = response.GetResponseStream();
              //REMOVETHIS                System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);

              using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
              {
                  string json = sr.ReadToEnd();
                  _parent.Trace("--------------GETTING TV Episodes Selection Result ------" + json);

                  var deserializer = new JavaScriptSerializer();
                  deserializer.MaxJsonLength = Int32.MaxValue;
                  var ItemData = deserializer.Deserialize<TVEpisodes.Rootobject>(json);
                  _parent.Trace("---------------Get Single TV Episode Selection:  Issue: Results.Taglines: " + ItemData.TotalRecordCount);

                  foreach (var genre in ItemData.Items)
                  {
                      try
                      {
                          //Use Path to pass data on Item Number to play as API Long can't hold
                          //var SingleTVData = GetSingleTVFromSeries(genre.Id);
                          
                          //Convert Date to sql date to allow sql date sort

                          DateTime myDateTime = genre.PremiereDate;
                          string sqlFormattedDate = myDateTime.ToString("s");

                          //Remove Embys Virtual Episodes from the Database

                          if (genre.LocationType != "Virtual")
                          {
                              var tvShow = new ApiTvEpisode
                              {
                                  Title = genre.Name ?? "",
                                  Plot = "",
                                  Rating = genre.OfficialRating ?? "",
                                  Mpaa = "",
                                  Date = sqlFormattedDate,
                                  Director = "",
                                  PlayCount = genre.UserData.PlayCount,
                                  Studio = "",
                                  IdEpisode = Xbmc.IDtoNumber(genre.Id),
                                  IdShow = Xbmc.IDtoNumber(genre.SeriesId),
                                  Season = genre.ParentIndexNumber,
                                  Episode = genre.IndexNumber,
                                  Path = genre.Id ?? "",
                                  ShowTitle = genre.SeriesName ?? "",
                                  Thumb = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.Id + "/Images/Primary" ?? "",
                                  Fanart = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.SeriesId + "/Images/Backdrop" ?? "",
                                  Hash = Xbmc.Hash(genre.Id)
                              };
                              episodes.Add(tvShow);
                          }
                      }
                      catch (Exception ex)
                      {
                          _parent.Trace("TV Episodes Exception Caught " + ex);
                      }
                  }

              }
          }
      }
      catch (Exception Ex)
      {
          _parent.Trace("Another tV Episodes exception" + Ex);
      }
      /*
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
        */
      return episodes;
    }

    public Collection<ApiTvShow> GetTvShowsRefresh()
    {
        return null;
    }

    public Collection<ApiTvShow> GetTvShows()
    {
        //var MovieId = GetMainSelection("TV");
        var shows = new Collection<ApiTvShow>();

        try
        {


            _parent.Trace("Getting TV Shows" + _parent.IP);
            string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items?Recursive=true&IncludeItemTypes=Series";

            var request = WebRequest.CreateHttp(NPurl);

            request.Method = "get";
            request.Timeout = 150000;
            _parent.Trace("Single TV Show Selection: " + _parent.IP + ":" + _parent.Port);

            var authString = _parent.GetAuthString();

            request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
            request.Headers.Add("X-Emby-Authorization", authString);
            request.ContentType = "application/json; charset=utf-8";
            request.Accept = "application/json; charset=utf-8";

            var response = request.GetResponse();

            if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
            {

                System.IO.Stream dataStream = response.GetResponseStream();
                //REMOVETHIS                   System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);

                using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    string json = sr.ReadToEnd();
                    _parent.Trace("--------------GETTING Single TV Show Selection Result ------" + json);

                    var deserializer = new JavaScriptSerializer();

                    var ItemData = deserializer.Deserialize<TVShows.Rootobject>(json);
                    _parent.Trace("---------------Get Single TV Show Selection:  Issue: Results.Taglines: " + ItemData.TotalRecordCount);

                    foreach (var genre in ItemData.Items)
                    {
                        try
                        {

                            var SingleTVData = GetSingleTVFromSeries(genre.Id);

                            string TempTVGenre = "";
                            if (SingleTVData.Genres != null && SingleTVData.Genres.Length != 0)
                            {
                                if (SingleTVData.Genres.FirstOrDefault() != null)
                                {
                                    TempTVGenre = SingleTVData.Genres.FirstOrDefault().ToString();
                                }
                            }
                            string TempTVStudios = "";
                            if (SingleTVData.Studios != null && SingleTVData.Studios.Length != 0)
                            {
                                if (SingleTVData.Studios.FirstOrDefault() != null)
                                {
                                    TempTVStudios = SingleTVData.Studios.FirstOrDefault().Name.ToString();
                                }
                            }


                            var tvShow = new ApiTvShow
                            {
                                
                                Title = genre.Name ?? "Unknown",
                                Plot = SingleTVData.Overview ?? "",
                                Rating = genre.CommunityRating.ToString() ?? "" ,
                                IdScraper = "",
                                Mpaa = SingleTVData.OfficialRating ?? "Unknown",
                                Genre = TempTVGenre,
                                Studio = TempTVStudios,
                                IdShow = Xbmc.IDtoNumber(genre.Id),
                                TotalCount = genre.RecursiveItemCount,
                                Path = SingleTVData.Path ?? "",
                                Premiered = genre.PremiereDate.ToString() ?? "",
                                Thumb = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.Id + "/Images/Primary" ?? "",
                                Fanart = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.Id + "/Images/Backdrop" ?? "",
                                Hash = Xbmc.Hash(genre.Id)
                                
                            };

                            shows.Add(tvShow);
                        }
                        catch (Exception ex)
                        {
                            _parent.Trace("TV Shows Exception Caught " + ex );
                        }
                    }

                }
            }
        }
        catch (Exception Ex)
        {
            _parent.Trace("Another tV SHows exception" + Ex);
        }
            
            return shows;
      }
    
    



    public string GetMainSelection(string param)
    {
        try
        {

            _parent.Trace("Getting Main Selection Result" + _parent.IP);
            string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items";

            var request = WebRequest.CreateHttp(NPurl);

            request.Method = "get";
            request.Timeout = 150000;
            _parent.Trace("Main Selection: " + _parent.IP + ":" + _parent.Port);

            var authString = _parent.GetAuthString();

            request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
            request.Headers.Add("X-Emby-Authorization", authString);
            request.ContentType = "application/json; charset=utf-8";
            request.Accept = "application/json; charset=utf-8";

            var response = request.GetResponse();

            if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
            {

                System.IO.Stream dataStream = response.GetResponseStream();
                //REMOVETHIS                   System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);

                using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    string json = sr.ReadToEnd();
                    _parent.Trace("--------------GETTING Main Selection Result ------" + json);
                    
                    var deserializer = new JavaScriptSerializer();
                   
                    var ItemData = deserializer.Deserialize<MainSelectionItems.Rootobject>(json);
                    _parent.Trace("---------------Get Main Selection:  Issue: Results.Count: " + ItemData.TotalRecordCount);

                    foreach (var id in ItemData.Items)
                    {
                       
                        
                        if (id.Name == param)
                        {
                            _parent.Trace("----------- Get Main Selection Run ---" + param + " ID Result equals:  " + id.Id);
                            return id.Id;
                        }
                    }

                }
            }


            return null;
        }
        catch (Exception ex)
        {
            _parent.Trace("ERROR in Main Selection obtaining: "+ex);
            return "";

        }
    }
    public TVSingleItemSeries.Rootobject GetSingleTVFromSeries(string itemId)
    {
        try
        {

            _parent.Trace("Getting Single TV From Series Data" + _parent.IP);
            string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items/" + itemId;

            var request = WebRequest.CreateHttp(NPurl);

            request.Method = "get";
            request.Timeout = 150000;
            _parent.Trace("Single Movie Selection: " + _parent.IP + ":" + _parent.Port);

            var authString = _parent.GetAuthString();

            request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
            request.Headers.Add("X-Emby-Authorization", authString);
            request.ContentType = "application/json; charset=utf-8";
            request.Accept = "application/json; charset=utf-8";

            var response = request.GetResponse();

            if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
            {

                System.IO.Stream dataStream = response.GetResponseStream();
                //REMOVETHIS                System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);

                using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    string json = sr.ReadToEnd();
                    _parent.Trace("--------------GETTING Single TV From Series Selection Result ------" + json);

                    var deserializer = new JavaScriptSerializer();

                    var ItemData = deserializer.Deserialize<TVSingleItemSeries.Rootobject>(json);
                    _parent.Trace("---------------Get Single TV From Series Selection:  Issue: Results.Taglines: " + ItemData.Taglines);

                    return ItemData;

                }
            }


            return null;
        }
        catch (Exception ex)
        {
            _parent.Trace("ERROR in Single TV From Series Selection obtaining: " + ex);
            return null;

        }
    }



    public SingleMovieItem.Rootobject GetSingleMovieItem(string itemId)
    {
        try
        {

            _parent.Trace("Getting Single Movie Data" + _parent.IP);
            string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items/"+itemId;

            var request = WebRequest.CreateHttp(NPurl);

            request.Method = "get";
            request.Timeout = 150000;
            _parent.Trace("Single Movie Selection: " + _parent.IP + ":" + _parent.Port);

            var authString = _parent.GetAuthString();

            request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
            request.Headers.Add("X-Emby-Authorization", authString);
            request.ContentType = "application/json; charset=utf-8";
            request.Accept = "application/json; charset=utf-8";

            var response = request.GetResponse();

            if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
            {

                System.IO.Stream dataStream = response.GetResponseStream();
                System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);

                using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    string json = sr.ReadToEnd();
                    _parent.Trace("--------------GETTING Single Movie Selection Result ------" + json);

                    var deserializer = new JavaScriptSerializer();

                    var ItemData = deserializer.Deserialize<SingleMovieItem.Rootobject>(json);
                    _parent.Trace("---------------Get Single Movie Selection:  Issue: Results.Taglines: " + ItemData.Taglines);

                    return ItemData;

                }
            }


            return null;
        }
        catch (Exception ex)
        {
            _parent.Trace("ERROR in Single Movie Selection obtaining: " + ex);
            return null;

        }
    }
    public Collection<ApiMovie> GetMoviesRefresh()
    {
        return null;
        //To be done after Kodi one
    }


    public Collection<ApiTvEpisode> GetTvEpisodesRefresh()
    {
        return null;
        //To be done
    }

    public Collection<ApiMovie> GetMovies()
    {
      var movies = new Collection<ApiMovie>();
      var MovieId = GetMainSelection("Movies");

      try
      {

          _parent.Trace("Getting Main Movie Database Result" + _parent.IP);
          string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items?ParentId=" + MovieId;

          var request = WebRequest.CreateHttp(NPurl);

          request.Method = "get";
          request.Timeout = 150000;
          _parent.Trace("Main Selection: " + _parent.IP + ":" + _parent.Port);

          var authString = _parent.GetAuthString();

          request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
          request.Headers.Add("X-Emby-Authorization", authString);
          request.ContentType = "application/json; charset=utf-8";
          request.Accept = "application/json; charset=utf-8";

          var response = request.GetResponse();

          if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
          {

              System.IO.Stream dataStream = response.GetResponseStream();
              //REMOVETHIS                 System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);

              using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
              {
                  string json = sr.ReadToEnd();
                  _parent.Trace("--------------GETTING All Movies Results ------" + json);

                  var deserializer = new JavaScriptSerializer();

                  var ItemData = deserializer.Deserialize<Movies.Rootobject>(json);

                  _parent.Trace("---------------Get Worlds Result:  Issue: Results.Count: " + ItemData.Items.Count);

                  foreach (var id in ItemData.Items)
                  {
                      try
                      {

                          SingleMovieItem.Rootobject Movieitem = GetSingleMovieItem(id.Id);
                          string newDirector = "";
                         
                          bool index = Movieitem.People.Any(item => item.Type == "Director");
                          if (index == true)
                          {
                              newDirector = Movieitem.People.First(i => i.Type == "Director").Name.ToString();
                          }

                          string Taglines = "";
                          if (Movieitem.Taglines != null && Movieitem.Taglines.Length != 0 ) 
                          {
                              if (Movieitem.Taglines.FirstOrDefault() != null )
                              {
                                  Taglines = Movieitem.Taglines.FirstOrDefault().ToString();
                              }
                          }
                          string Studios = "";
                          if (Movieitem.Studios != null && Movieitem.Studios.Length != 0)
                          {
                              if (Movieitem.Studios.FirstOrDefault() != null)
                              {
                                  Studios = Movieitem.Studios.FirstOrDefault().Name.ToString();
                              }
                          }

                          var Seconds = Convert.ToInt64(id.RunTimeTicks ?? 0);
                          var RoundSeconds = Math.Round(Seconds / 10000000.00, 1);

                          _parent.Trace("VideoLibrary Check:");
                          _parent.Trace(Movieitem.Name ?? "Unknown");
                          _parent.Trace(Movieitem.Overview ?? "Unknown"   );
                          _parent.Trace(Movieitem.VoteCount.ToString() ?? "0"   );
                          _parent.Trace(id.CommunityRating.ToString() ?? "0"   );
                          _parent.Trace( id.ProductionYear.ToString() ?? "nil Production Year:"  );
                          _parent.Trace(Taglines  );
                           _parent.Trace(Movieitem.ProviderIds.Imdb ?? ""   );                       
                          _parent.Trace(new TimeSpan(0,0,0, Convert.ToInt32(RoundSeconds)).ToString() ?? "Unknown"   );
                          _parent.Trace( id.OfficialRating ?? "Unknown"  );
                          _parent.Trace(Movieitem.Genres.FirstOrDefault() ?? "Unknown"   );
                          _parent.Trace( newDirector ?? ""  );
                          _parent.Trace( id.Name ?? ""      );
                          _parent.Trace( Studios      );
                          _parent.Trace( Xbmc.IDtoNumber(Movieitem.Id).ToString() );
                           _parent.Trace(Movieitem.Path.ToString() ?? ""            );
                           _parent.Trace(Movieitem.Id ?? ""            );
                           _parent.Trace( Movieitem.UserData.PlayCount.ToString() ?? "nilPlayCount" );
                           _parent.Trace("http://" + _parent.IP + ":" + _parent.Port + "/Items/" + id.Id + "/Images/Primary");
                          _parent.Trace( "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + id.Id + "/Images/Backdrop"           );
                          _parent.Trace( Xbmc.Hash(id.Id)           );



                          var movie = new ApiMovie
                          {
                              Title = Movieitem.Name ?? "Unknown",
                              Plot = Movieitem.Overview ?? "Unknown",
                              Votes = Movieitem.VoteCount.ToString() ?? "0",
                              Rating = id.CommunityRating.ToString() ?? "0",
                              Year = id.ProductionYear ?? 1999,
                              Tagline = Taglines,
                              IdScraper = Movieitem.ProviderIds.Imdb ?? "",
                              Length = new TimeSpan(0,0,0, Convert.ToInt32(RoundSeconds)).ToString() ?? "Unknown",
                              Mpaa = id.OfficialRating ?? "Unknown",
                              Genre = Movieitem.Genres.FirstOrDefault() ?? "Unknown",
                              Director = newDirector ?? "",
                              OriginalTitle = id.Name ?? "",
                              Studio = Studios,
                              IdFile = 0,
                              IdMovie = Xbmc.IDtoNumber(Movieitem.Id),
                              FileName = Movieitem.Path.ToString() ?? "",
                              Path = Movieitem.Id ?? "",
                              PlayCount = Movieitem.UserData.PlayCount ?? 0,
                              Thumb = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + id.Id + "/Images/Primary",
                              Fanart = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + id.Id + "/Images/Backdrop",
                              Hash = Xbmc.Hash(id.Id),
                              DateAdded = Movieitem.PremiereDate.ToString("s")
                          };
                          movies.Add(movie);
                      }

                      catch (Exception ex)
                      {
                          _parent.Trace("Exception with Movie Name :" + ex);
                          
                      }
                  }


                  /*
                                    _nowPlaying.FanartURL = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + server.PrimaryItemId + "/Images/Backdrop";
                                    _nowPlaying.ThumbURL = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + server.PrimaryItemId + "/Images/Primary";
                  */

              }

          }


      }
      catch (Exception ex)
      {
          _parent.Trace("ERROR in Main Movies obtaining: " + ex);


      }


      /*

      var properties = new JsonArray(new[] { "title", "plot", "genre", "year", "fanart", "thumbnail", "playcount", "studio", "rating", "runtime", "mpaa", "originaltitle", "director", "votes" });
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
                  FileName = "",
                  Path = "",
                  PlayCount = 0,
                  Thumb = genre["thumbnail"].ToString(),
                  Fanart = genre["fanart"].ToString(),
                  Hash = Xbmc.Hash(genre["thumbnail"].ToString())
                };
              movies.Add(movie);
            }
            catch (Exception)
            {
            }
          }
        }
      }
     */
      return movies;
    }
  }
}
