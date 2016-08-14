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
            var seasons = new Collection<ApiTvSeason>();

            try
            {


                _parent.Trace("Getting TV Seasons from New TV Episodes Somehow:" + _parent.IP);
                //          string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items?Limit=30&Recursive=true&ExcludeLocationTypes=Virtual&SortBy=DateCreated&SortOrder=Descending&IncludeItemTypes=Season";
                string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items?Limit=30&Recursive=true&ExcludeLocationTypes=Virtual&SortBy=DateCreated&SortOrder=Descending&IncludeItemTypes=Episode";

                var request = WebRequest.CreateHttp(NPurl);

                request.Method = "get";
                request.Timeout = 150000;
                _parent.Trace("Single TV Season from TV Episodes NEW: Selection: " + _parent.IP + ":" + _parent.Port);

                var authString = _parent.GetAuthString();

                request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
                request.Headers.Add("X-Emby-Authorization", authString);
                request.ContentType = "application/json; charset=utf-8";
                request.Accept = "application/json; charset=utf-8";

                var response = request.GetResponse();

                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {

                    System.IO.Stream dataStream = response.GetResponseStream();

                    using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        string json = sr.ReadToEnd();
                        _parent.Trace("--------------GETTING Single TV Season RefreshTVSeasons Selection Result ------" + json);

                        var deserializer = new JavaScriptSerializer();

                        var ItemData = deserializer.Deserialize<TVEpisodes.Rootobject>(json);
                        //   _parent.Trace("---------------Get Single TV Season Selection:  Issue: Results.Taglines: " + ItemData.TotalRecordCount);

                        foreach (var genre in ItemData.Items)
                        {
                            try
                            {

                                // var SingleTVData = GetSingleTVFromSeries(genre.Id);
                                _parent.Trace("---Emby QuickRefresh GetTVSeasons--- Season Number:" + genre.ParentIndexNumber);
                                _parent.Trace("---Emby QuickRefresh GetTVSeasons--- ID Show:" + Xbmc.IDtoNumber(genre.SeriesId));
                                _parent.Trace("---Emby QuickRefresh GetTVSeasons--- Series Name:" + genre.SeriesName);
                                _parent.Trace("---Emby QuickRefresh GetTVSeasons--- Thumb:" + "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.SeriesId + "/Images/Primary" ?? "");
                                _parent.Trace("---Emby QuickRefresh GetTVSeasons--- Child Count:" + (long)(int)genre.IndexNumber);
                                _parent.Trace("---Emby QuickRefresh GetTVSeasons--- Fanart:" + "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.SeriesId + "/Images/Backdrop" ?? "");
                                _parent.Trace("---Emby QuickRefresh GetTVSeasons--- Hash:" + Xbmc.Hash(genre.SeasonId));

                                var tvShow = new ApiTvSeason
                                {
                                    SeasonNumber = genre.ParentIndexNumber,
                                    IdShow = Xbmc.IDtoNumber(genre.SeriesId),
                                    Show = genre.SeriesName ?? "",
                                    Thumb = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.SeriesId + "/Images/Primary" ?? "",
                                    EpisodeCount = (long)(int)genre.IndexNumber,   //bit of a hack but if date sorted - latest episode should be highest - so for most should be right.
                                    Fanart = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.SeriesId + "/Images/Backdrop" ?? "",
                                    Hash = Xbmc.Hash(genre.SeasonId)
                                };
                                seasons.Add(tvShow);

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



            return seasons;
        }

        //Change main TV Seasons pull to Episode based one.
        // One below/Old one - keep timing out - not answer on Emby Forums - no clear issue at this end
        // ? Issue with IncludeItemTypes=Season - or possibly local database problem/corrupt at my end
        // See how this one goes/also how quick it is.

        public Collection<ApiTvSeason> GetTvSeasons()
        {
            var seasons = new Collection<ApiTvSeason>();

            try
            {


                _parent.Trace("Getting TV Seasons (NEW Code Base Change) from New TV Episodes Somehow:" + _parent.IP);
                //          string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items?Limit=30&Recursive=true&ExcludeLocationTypes=Virtual&SortBy=DateCreated&SortOrder=Descending&IncludeItemTypes=Season";
                string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items?Recursive=true&ExcludeLocationTypes=Virtual&IncludeItemTypes=Episode";

                var request = WebRequest.CreateHttp(NPurl);

                request.Method = "get";
                request.Timeout = 700000;
                _parent.Trace("Single TV Season from TV Episodes NEW: Selection: " + _parent.IP + ":" + _parent.Port);

                var authString = _parent.GetAuthString();

                request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
                request.Headers.Add("X-Emby-Authorization", authString);
                request.ContentType = "application/json; charset=utf-8";
                request.Accept = "application/json; charset=utf-8";

                var response = request.GetResponse();

                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {

                    System.IO.Stream dataStream = response.GetResponseStream();

                    using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        string json = sr.ReadToEnd();
                        _parent.Trace("--------------GETTING Single TV Season Change to Code Base- Based on Episode Data -- tion Result ------" + json);

                        var deserializer = new JavaScriptSerializer();

                        deserializer.MaxJsonLength = Int32.MaxValue;
                        // Hopefully fix above.
                        var ItemData = deserializer.Deserialize<TVEpisodes.Rootobject>(json);

                        //   _parent.Trace("---------------Get Single TV Season Selection:  Issue: Results.Taglines: " + ItemData.TotalRecordCount);

                        foreach (var genre in ItemData.Items)
                        {
                            try
                            {

                                // var SingleTVData = GetSingleTVFromSeries(genre.Id);
                                _parent.Trace("---Emby  GetTVSeasons NEW --- Season Number:" + genre.ParentIndexNumber);
                                _parent.Trace("---Emby  GetTVSeasons NEW --- ID Show:" + Xbmc.IDtoNumber(genre.SeriesId));
                                _parent.Trace("---Emby  GetTVSeasons NEW --- Series Name:" + genre.SeriesName);
                                _parent.Trace("---Emby  GetTVSeasons NEW --- Thumb:" + "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.SeriesId + "/Images/Primary" ?? "");
                                _parent.Trace("---Emby  GetTVSeasons NEW --- Child Count:" + (long)(int)genre.IndexNumber);
                                _parent.Trace("---Emby  GetTVSeasons NEW --- Fanart:" + "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.SeriesId + "/Images/Backdrop" ?? "");
                                _parent.Trace("---Emby  GetTVSeasons NEW --- Hash:" + Xbmc.Hash(genre.SeasonId));

                                var tvShow = new ApiTvSeason
                                {
                                    SeasonNumber = genre.ParentIndexNumber,
                                    IdShow = Xbmc.IDtoNumber(genre.SeriesId),
                                    Show = genre.SeriesName ?? "",
                                    Thumb = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.SeriesId + "/Images/Primary" ?? "",
                                    EpisodeCount = (long)(int)genre.IndexNumber,   //bit of a hack but if date sorted - latest episode should be highest - so for most should be right.
                                    Fanart = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.SeriesId + "/Images/Backdrop" ?? "",
                                    Hash = Xbmc.Hash(genre.SeasonId)
                                };
                                seasons.Add(tvShow);

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
                _parent.Trace("Another TV Shows:NEW Seasons (from Episode Data) exception" + Ex);
            }



            return seasons;
        }

        /**
        public Collection<ApiTvSeason> GetTvSeasons()
        {
            var seasons = new Collection<ApiTvSeason>();

            try
            {


                _parent.Trace("Getting TV Seasons:" + _parent.IP);
                string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items?Recursive=true&IncludeItemTypes=Season";

                var request = WebRequest.CreateHttp(NPurl);

                request.Method = "get";
                request.Timeout = 150000;
                request.KeepAlive = false;
                _parent.Trace("Get TVSeasons TV Season Selection: " + NPurl);

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
                        deserializer.MaxJsonLength = Int32.MaxValue;

                        var ItemData = deserializer.Deserialize<TVSeasons.Rootobject>(json);
                        _parent.Trace("---------------Get Single TV Season Selection:  Issue: Results.Taglines: " + ItemData.TotalRecordCount);

                        foreach (var genre in ItemData.Items)
                        {
                            try
                            {

                                //var SingleTVData = GetSingleTVFromSeries(genre.Id);
                                _parent.Trace("---Emby GetTVSeasons--- Season Number:" + (long)(int)genre.IndexNumber);
                                _parent.Trace("---Emby GetTVSeasons--- ID Show:" + Xbmc.IDtoNumber(genre.SeriesId));
                                _parent.Trace("---Emby GetTVSeasons--- Series Name:" + genre.SeriesName);
                                _parent.Trace("---Emby GetTVSeasons--- Thumb:" + "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.Id + "/Images/Primary" ?? "");
                                _parent.Trace("---Emby GetTVSeasons--- Child Count:" + (long)(int)genre.ChildCount);
                                _parent.Trace("---Emby GetTVSeasons--- Fanart:" + "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.SeriesId + "/Images/Backdrop" ?? "");
                                _parent.Trace("---Emby GetTVSeasons--- Hash:" + Xbmc.Hash(genre.Id));

                                var tvShow = new ApiTvSeason
                                {
                                    SeasonNumber = genre.IndexNumber,
                                    IdShow = Xbmc.IDtoNumber(genre.SeriesId),
                                    Show = genre.SeriesName ?? "",
                                    Thumb = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.Id + "/Images/Primary" ?? "",
                                    EpisodeCount = (long)(int)genre.ChildCount,
                                    Fanart = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.SeriesId + "/Images/Backdrop" ?? "",
                                    Hash = Xbmc.Hash(genre.Id)
                                };
                                seasons.Add(tvShow);

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


            return seasons;
        }
    **/
        public Collection<ApiTvEpisode> GetTvEpisodes()
        {
            var episodes = new Collection<ApiTvEpisode>();

            try
            {


                _parent.Trace("GetTVEpisodes : Parent IP: " + _parent.IP);
                string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items?Recursive=true&ExcludeLocationTypes=Virtual&IncludeItemTypes=Episode";

                var request = WebRequest.CreateHttp(NPurl);

                request.Method = "get";
                request.Timeout = 700000;
                _parent.Trace("GetTVEpisodes: Single TV Episode Selection: " + _parent.IP + ":" + _parent.Port);

                var authString = _parent.GetAuthString();

                request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
                request.Headers.Add("X-Emby-Authorization", authString);
                request.ContentType = "application/json; charset=utf-8";
                request.Accept = "application/json; charset=utf-8";

                var response = request.GetResponse();

                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {

                    System.IO.Stream dataStream = response.GetResponseStream();


                    using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        string json = sr.ReadToEnd();
                        _parent.Trace("--------------GETTING TVEpisodes Selection Result ------" + json + "---END---");

                        var deserializer = new JavaScriptSerializer();
                        deserializer.MaxJsonLength = Int32.MaxValue;
                        var ItemData = deserializer.Deserialize<TVEpisodes.Rootobject>(json);
                        _parent.Trace("---------------Get TVEpisode Selection:  Issue: Results.Taglines: " + ItemData.TotalRecordCount);

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
                                        PlayCount = Convert.ToInt64(genre.UserData.PlayCount),
                                        Studio = "",
                                        IdEpisode = Xbmc.IDtoNumber(genre.Id),
                                        IdShow = Xbmc.IDtoNumber(genre.SeriesId),
                                        Season = (long)(int)genre.ParentIndexNumber,
                                        Episode = (long)(int)genre.IndexNumber,
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

            return episodes;
        }

        public Collection<ApiTvShow> GetTvShowsRefresh()
        {
            //var MovieId = GetMainSelection("TV");
            var shows = new Collection<ApiTvShow>();

            try
            {
                _parent.Trace("Getting TV Shows REFRESH in TVSHOWS Parent IP: " + _parent.IP);
                string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items?Limit=30&Recursive=true&ExcludeLocationTypes=Virtual&SortBy=DateCreated&SortOrder=Descending&IncludeItemTypes=Episode";
                var request = WebRequest.CreateHttp(NPurl);
                request.Method = "get";
                request.Timeout = 700000;
                _parent.Trace("Single REFRESH TV Episode Selection in TVSHOWS: " + _parent.IP + ":" + _parent.Port);
                var authString = _parent.GetAuthString();
                request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
                request.Headers.Add("X-Emby-Authorization", authString);
                request.ContentType = "application/json; charset=utf-8";
                request.Accept = "application/json; charset=utf-8";
                var response = request.GetResponse();
                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {
                    System.IO.Stream dataStream = response.GetResponseStream();
                    using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        string json = sr.ReadToEnd();
                        _parent.Trace("--------------GETTING REFRESH TV Episodes: in TV SHOWS Selection Result ------" + json);
                        var deserializer = new JavaScriptSerializer();
                        deserializer.MaxJsonLength = Int32.MaxValue;
                        var ItemData = deserializer.Deserialize<TVEpisodes.Rootobject>(json);
                        _parent.Trace("---------------Get Single REFRESH TV Shows in TVSHOWS REfresh Selection:  Issue: Results.Taglines: " + ItemData.Items.Count());

                        foreach (var genre in ItemData.Items)
                        {
                            try
                            {
                                //Use Path to pass data on Item Number to play as API Long can't hold
                                //var SingleTVData = GetSingleTVFromSeries(genre.Id);

                                //Convert Date to sql date to allow sql date sort
                                //
                                //               DateTime myDateTime = genre.PremiereDate;
                                //                string sqlFormattedDate = myDateTime.ToString("s");

                                var SingleTVData = GetSingleTVFromSeries(genre.SeriesId);

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

                                    Title = genre.SeriesName ?? "Unknown",
                                    Plot = SingleTVData.Overview ?? "",
                                    Rating = genre.CommunityRating.ToString() ?? "",
                                    IdScraper = "",
                                    Mpaa = SingleTVData.OfficialRating ?? "Unknown",
                                    Genre = TempTVGenre,
                                    Studio = TempTVStudios,
                                    IdShow = Xbmc.IDtoNumber(genre.SeriesId),
                                    TotalCount = (long)(int)SingleTVData.RecursiveItemCount,
                                    Path = SingleTVData.Path ?? "",
                                    Premiered = SingleTVData.PremiereDate.ToString("D") ?? "",
                                    Thumb = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.SeriesId + "/Images/Primary" ?? "",
                                    Fanart = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.SeriesId + "/Images/Backdrop" ?? "",
                                    Banner = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.ParentBackdropItemId + "/Images/Banner" ?? "",
                                    Logo = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.ParentBackdropItemId + "/Images/Logo" ?? "",
                                    Hash = Xbmc.Hash(genre.SeriesId)

                                };

                                shows.Add(tvShow);
                            }
                            catch (Exception ex)
                            {
                                _parent.Trace("TV Shows REFRESH Exception Caught " + ex);
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


        public Collection<ApiTvShow> GetTvShows()
        {
            //var MovieId = GetMainSelection("TV");
            var shows = new Collection<ApiTvShow>();

            try
            {

                _parent.Trace("Getting TV Shows" + _parent.IP);
                string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items?ExcludeLocationTypes=Virtual&Recursive=true&IncludeItemTypes=Series";

                var request = WebRequest.CreateHttp(NPurl);

                request.Method = "get";
                request.Timeout = 150000;
                _parent.Trace("Single TV Show Selection: " + NPurl);

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
                        _parent.Trace("--------------GETTING Single TV Show Selection Result ------" + json + "---***--- END --- END GetTvShows:");

                        var deserializer = new JavaScriptSerializer();

                        var ItemData = deserializer.Deserialize<TVShows.Rootobject>(json);
                        _parent.Trace("---------------GetTVShows:  TV Show Selection:  Issue: Results.Taglines: " + ItemData.TotalRecordCount);

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
                                    Rating = genre.CommunityRating.ToString() ?? "",
                                    IdScraper = "",
                                    Mpaa = SingleTVData.OfficialRating ?? "Unknown",
                                    Genre = TempTVGenre,
                                    Studio = TempTVStudios,
                                    IdShow = Xbmc.IDtoNumber(genre.Id),
                                    TotalCount = (long)(int)genre.RecursiveItemCount,
                                    Path = SingleTVData.Path ?? "",
                                    Premiered = genre.PremiereDate.ToString("D") ?? "",
                                    Thumb = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.Id + "/Images/Primary" ?? "",
                                    Fanart = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.Id + "/Images/Backdrop" ?? "",
                                    Banner = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.Id + "/Images/Banner" ?? "",
                                    Logo = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.Id + "/Images/Logo" ?? "",
                                    Hash = Xbmc.Hash(genre.Id)

                                };

                                shows.Add(tvShow);
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

            return shows;
        }





        public List<string> GetMainSelection(string param)
        {

            List<string> result = new List<string>();

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


                            if (id.CollectionType == param)
                            {
                                _parent.Trace("----------- Get Main Selection Run ---" + param + " ID Result ---------------equals--------------:  " + id.Id);
                                result.Add(id.Id);
                            }
                        }

                    }
                }


                return result;
            }
            catch (Exception ex)
            {
                _parent.Trace("ERROR in Main Selection obtaining: " + ex);
                return null;

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
                    System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);

                    using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        string json = sr.ReadToEnd();
                        _parent.Trace("--------------GETTING Single Movie Selection Result ------" + json);

                        var deserializer = new JavaScriptSerializer();

                        var ItemData = deserializer.Deserialize<SingleMovieItem.Rootobject>(json);
                        _parent.Trace("---------------Get Single Movie Selection:  Issue: Results.Taglines: " + ItemData.Taglines.ToString());

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
            {
                var movies = new Collection<ApiMovie>();
                List<string> MovieId = GetMainSelection("movies");

                try
                {
                    foreach (string MovieDirectory in MovieId)
                    {


                        try
                        {

                            _parent.Trace("Getting Main Movie Database Result" + _parent.IP);
                            string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items?Limit=30&SortBy=DateCreated&SortOrder=Descending&ParentId=" + MovieDirectory;
                            _parent.Trace("Getting Main Movie Database Refresh URL Called" + NPurl);
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
                                            if (Movieitem.Taglines != null && Movieitem.Taglines.Length != 0)
                                            {
                                                if (Movieitem.Taglines.FirstOrDefault() != null)
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
                                            _parent.Trace(Movieitem.Overview ?? "Unknown");
                                            _parent.Trace(Movieitem.VoteCount.ToString() ?? "0");
                                            _parent.Trace(id.CommunityRating.ToString() ?? "0");
                                            _parent.Trace(id.ProductionYear.ToString() ?? "nil Production Year:");
                                            _parent.Trace(Taglines);
                                            _parent.Trace(Movieitem.ProviderIds.Imdb ?? "");
                                            _parent.Trace(new TimeSpan(0, 0, 0, Convert.ToInt32(RoundSeconds)).ToString() ?? "Unknown");
                                            _parent.Trace(id.OfficialRating ?? "Unknown");
                                            _parent.Trace(Movieitem.Genres.FirstOrDefault() ?? "Unknown");
                                            _parent.Trace(newDirector ?? "");
                                            _parent.Trace(id.Name ?? "");
                                            _parent.Trace(Studios);
                                            _parent.Trace(Xbmc.IDtoNumber(Movieitem.Id).ToString());
                                            _parent.Trace(Movieitem.Path.ToString() ?? "");
                                            _parent.Trace(Movieitem.Id ?? "");
                                            _parent.Trace(Movieitem.UserData.PlayCount.ToString() ?? "nilPlayCount");
                                            _parent.Trace("http://" + _parent.IP + ":" + _parent.Port + "/Items/" + id.Id + "/Images/Primary");
                                            _parent.Trace("http://" + _parent.IP + ":" + _parent.Port + "/Items/" + id.Id + "/Images/Backdrop");
                                            _parent.Trace(Xbmc.Hash(id.Id));



                                            var movie = new ApiMovie
                                            {
                                                Title = Movieitem.Name ?? "Unknown",
                                                Plot = Movieitem.Overview ?? "Unknown",
                                                Votes = Movieitem.VoteCount.ToString() ?? "0",
                                                Rating = id.CommunityRating.ToString() ?? "0",
                                                Year = id.ProductionYear ?? 1901,
                                                Tagline = Taglines,
                                                IdScraper = Movieitem.ProviderIds.Imdb ?? "",
                                                Length = new TimeSpan(0, 0, 0, Convert.ToInt32(RoundSeconds)).ToString() ?? "Unknown",
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
                                                Banner = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + id.Id + "/Images/Banner",
                                                Logo = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + id.Id + "/Images/Logo",
                                                Fanart = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + id.Id + "/Images/Backdrop",
                                                Hash = Xbmc.Hash(id.Id),
                                                DateAdded = Movieitem.DateCreated.ToString("s")
                                            };
                                            movies.Add(movie);
                                        }

                                        catch (Exception ex)
                                        {
                                            _parent.Trace("Exception with Movie Name :" + ex);

                                        }
                                    }




                                }

                            }


                        }
                        catch (Exception ex)
                        {
                            _parent.Trace("ERROR in Main Movies obtaining: " + ex);


                        }


                    }
                    return movies;
                }
                catch (Exception ex)
                {
                    _parent.Trace("Error in List Array for Main Selection:" + ex);
                    return null;
                }
            }
        }



        public Collection<ApiTvEpisode> GetTvEpisodesRefresh()
        {
            {
                var episodes = new Collection<ApiTvEpisode>();

                try
                {
                    _parent.Trace("Getting TV Episodes: Parent IP: " + _parent.IP);
                    string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items?Limit=30&Recursive=true&ExcludeLocationTypes=Virtual&SortBy=DateCreated&SortOrder=Descending&IncludeItemTypes=Episode";
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

                                    //Remove Embys Virtual Episodes from the Database  /also now done above in url ExcludeLocation finding

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
                                            PlayCount = (long)(int)genre.UserData.PlayCount,
                                            Studio = "",
                                            IdEpisode = Xbmc.IDtoNumber(genre.Id),
                                            IdShow = Xbmc.IDtoNumber(genre.SeriesId),
                                            Season = (long)(int)genre.ParentIndexNumber,
                                            Episode = (long)(int)genre.IndexNumber,
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

                return episodes;
            }
        }

        public Collection<ApiMovie> GetMovies()
        {
            var movies = new Collection<ApiMovie>();
            List<string> MovieId = GetMainSelection("movies");

            foreach (string MovieDirectory in MovieId)
            {

                try
                {

                    _parent.Trace("Getting Main Movie Database Result" + _parent.IP);
                    string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items?ParentId=" + MovieDirectory;
                    _parent.Trace("Getting Main Movie DB NPurl Called " + NPurl);

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
                                    if (Movieitem.Taglines != null && Movieitem.Taglines.Length != 0)
                                    {
                                        if (Movieitem.Taglines.FirstOrDefault() != null)
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
                                    _parent.Trace(Movieitem.Overview ?? "Unknown");
                                    _parent.Trace(Movieitem.VoteCount.ToString() ?? "0");
                                    _parent.Trace(id.CommunityRating.ToString() ?? "0");
                                    _parent.Trace(id.ProductionYear.ToString() ?? "nil Production Year:");
                                    _parent.Trace(Taglines);
                                    _parent.Trace(Movieitem.ProviderIds.Imdb ?? "");
                                    _parent.Trace(new TimeSpan(0, 0, 0, Convert.ToInt32(RoundSeconds)).ToString() ?? "Unknown");
                                    _parent.Trace(id.OfficialRating ?? "Unknown");
                                    _parent.Trace(Movieitem.Genres.FirstOrDefault() ?? "Unknown");
                                    _parent.Trace(newDirector ?? "");
                                    _parent.Trace(id.Name ?? "");
                                    _parent.Trace(Studios);
                                    _parent.Trace(Xbmc.IDtoNumber(Movieitem.Id).ToString());
                                    _parent.Trace(Movieitem.Path.ToString() ?? "");
                                    _parent.Trace(Movieitem.Id ?? "");
                                    _parent.Trace(Movieitem.UserData.PlayCount.ToString() ?? "nilPlayCount");
                                    _parent.Trace("http://" + _parent.IP + ":" + _parent.Port + "/Items/" + id.Id + "/Images/Primary");
                                    _parent.Trace("http://" + _parent.IP + ":" + _parent.Port + "/Items/" + id.Id + "/Images/Backdrop");
                                    _parent.Trace("http://" + _parent.IP + ":" + _parent.Port + "/Items/" + id.Id + "/Images/Banner");
                                    _parent.Trace("http://" + _parent.IP + ":" + _parent.Port + "/Items/" + id.Id + "/Images/Logo");
                                    _parent.Trace(Xbmc.Hash(id.Id));



                                    var movie = new ApiMovie
                                    {
                                        Title = Movieitem.Name ?? "Unknown",
                                        Plot = Movieitem.Overview ?? "Unknown",
                                        Votes = Movieitem.VoteCount.ToString() ?? "0",
                                        Rating = id.CommunityRating.ToString() ?? "0",
                                        Year = id.ProductionYear ?? 1901,
                                        Tagline = Taglines,
                                        IdScraper = Movieitem.ProviderIds.Imdb ?? "",
                                        Length = new TimeSpan(0, 0, 0, Convert.ToInt32(RoundSeconds)).ToString() ?? "Unknown",
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
                                        Banner = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + id.Id + "/Images/Banner",
                                        Logo = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + id.Id + "/Images/Logo",
                                        Hash = Xbmc.Hash(id.Id),
                                        DateAdded = Movieitem.DateCreated.ToString("s")
                                    };
                                    movies.Add(movie);
                                }

                                catch (Exception ex)
                                {
                                    _parent.Trace("Exception with Movie Name :" + ex);

                                }
                            }



                        }

                    }


                }
                catch (Exception ex)
                {
                    _parent.Trace("ERROR in Main Movies obtaining: " + ex);


                }

            }


            return movies;            
        }
    }
}
