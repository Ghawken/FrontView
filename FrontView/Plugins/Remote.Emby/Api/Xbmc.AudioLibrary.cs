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

using System.Collections.ObjectModel;
using Plugin;
using Jayrock.Json;
using System;
using System.Net;
using System.Web.Script.Serialization;
using System.Web;
using System.Web.Script;
using System.Linq;

namespace Remote.Emby.Api
{
    class XbmcAudioLibrary : IApiAudioLibrary
    {
        private readonly Xbmc _parent;

        public XbmcAudioLibrary(Xbmc parent)
        {
            _parent = parent;
        }
        public string GetMainSelection(string param)
        {
            try
            {
                _parent.Trace("Getting Music Selection Result" + _parent.IP);
                string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Views";
                var request = WebRequest.CreateHttp(NPurl);
                var MusicID = "";
                request.Method = "get";
                request.Timeout = 10000;
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

                    using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        string json = sr.ReadToEnd();
                        _parent.Trace("--------------GETTING Main Selection Result ------" + json);
                        var deserializer = new JavaScriptSerializer();
                        var ItemData = deserializer.Deserialize<MainSelectionforMusic.Rootobject>(json);
                        _parent.Trace("---------------Get Main Selection:  Issue: Results.Count: " + ItemData.Items.Length);
                        foreach (var id in ItemData.Items)
                        {
                            if (id.Name == "Music")
                            {
                                _parent.Trace("----------- Get Main Selection Run ---" + param + " ID Result equals:  " + id.Id);
                                MusicID = id.Id;
                                return MusicID;
                            }
                        }

                    }
                }

                return "";


                /*
                // Do again to get Album, Genre etc results
                // these come from param - above is fixed to Music
                // Options to pass are Latest, Playlists, Albums, Album Artists, Songs, Genres
                _parent.Trace("Getting Music Next Selection  Result" + _parent.IP);
                NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items?parentId=" + MusicID;
                var request2 = WebRequest.CreateHttp(NPurl);
                request2.Method = "get";
                request2.Timeout = 100000;
                _parent.Trace("Main Selection: " + _parent.IP + ":" + _parent.Port);
                //var authString = _parent.GetAuthString();
                request2.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
                request2.Headers.Add("X-Emby-Authorization", authString);
                request2.ContentType = "application/json; charset=utf-8";
                request2.Accept = "application/json; charset=utf-8";
                var response2 = request2.GetResponse();
                if (((HttpWebResponse)response2).StatusCode == HttpStatusCode.OK)
                {
                    System.IO.Stream dataStream = response2.GetResponseStream();
                    System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);
                    using (var sr = new System.IO.StreamReader(response2.GetResponseStream()))
                    {
                        string json = sr.ReadToEnd();
                        _parent.Trace("--------------GETTING Music Next Selection Result ------" + json);
                        var deserializer = new JavaScriptSerializer();
                        var ItemData = deserializer.Deserialize<MusicSelection.Rootobject>(json);
                        _parent.Trace("---------------Get Next  Selection:  Issue: Results.Count: " + ItemData.TotalRecordCount);
                        foreach (var id in ItemData.Items)
                        {
                            if (id.Name == param)
                            {
                                _parent.Trace("----------- Next Music Next Selection Run ---" + param + " ID Result equals:  " + id.Id);
                                return id.Id;
                            }
                        }

                    }
                }




                return null;
                 */
            }
            catch (Exception ex)
            {
                _parent.Trace("ERROR in Main Music Selection obtaining: " + ex);
                return "";

            }
        }
        public Collection<ApiAudioGenre> GetGenres()
        {

            var MusicID = GetMainSelection("Genres");  //Genres bit no longer needed
            // Change away from GetMainSelection which is no longer working
            // Use Emby Genres to get info.

            var genres = new Collection<ApiAudioGenre>();

            try
            {
                _parent.Trace("Getting Music Genres: Parent IP: " + _parent.IP);
                string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/MusicGenres?ParentId="+ MusicID;
                var request = WebRequest.CreateHttp(NPurl);
                request.Method = "get";
                request.Timeout = 150000;
                _parent.Trace("Genre Music Selection: " + NPurl);
                var authString = _parent.GetAuthString();
                request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
                request.Headers.Add("X-Emby-Authorization", authString);
                request.ContentType = "application/json; charset=utf-8";
                request.Accept = "application/json; charset=utf-8";

                var response = request.GetResponse();

                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {

                    System.IO.Stream dataStream = response.GetResponseStream();
//REMOTETHIS        System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);

                    using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        string json = sr.ReadToEnd();
                        _parent.Trace("--------------GETTING Music Genres Selection Result ------" + json);

                        var deserializer = new JavaScriptSerializer();
                        var ItemData = deserializer.Deserialize<MusicGenres.Rootobject>(json);
                        _parent.Trace("---------------Get Music Genres:  Issue: Results.Record Count: " + ItemData.TotalRecordCount);

                        foreach (var genre in ItemData.Items)
                        {
                            try
                            {
                                var gen = new ApiAudioGenre
                                {
                                    IdGenre = Xbmc.IDtoNumber(genre.Id),
                                    Name = genre.Name ?? "",
                                    AlbumCount = 0, //genre.ChildCount,
                                    Thumb = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.Id + "/Images/Primary" ?? ""
                                };
                                genres.Add(gen);
                            }
                            catch (Exception ex)
                            {
                                _parent.Trace("Music Genres Exception Caught " + ex);
                            }
                        }

                    }
                }
            }
            catch (Exception Ex)
            {
                _parent.Trace("Another Music Genres exception" + Ex);
            }
            
            
            return genres;
        }

        public Collection<ApiAudioArtist> GetArtists()
        {
            var artists = new Collection<ApiAudioArtist>();


            //var MusicID = GetMainSelection("Album Artists");


            try
            {
                _parent.Trace("Getting Album ARtists: Parent IP: " + _parent.IP);
                string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Artists"; // /" + Globals.CurrentUserID + "/Items?ParentId=" + ;
                var request = WebRequest.CreateHttp(NPurl);
                request.Method = "get";
                request.Timeout = 150000;
                _parent.Trace("Album Artists Music Selection: " + NPurl);
                var authString = _parent.GetAuthString();
                request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
                request.Headers.Add("X-Emby-Authorization", authString);
                request.ContentType = "application/json; charset=utf-8";
                request.Accept = "application/json; charset=utf-8";

                var response = request.GetResponse();

                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {

                    System.IO.Stream dataStream = response.GetResponseStream();
//REMOVETHIS                    System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);

                    using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        string json = sr.ReadToEnd();
                        _parent.Trace("--------------GETTING Album Artists Genres Selection Result ------" + json);

                        var deserializer = new JavaScriptSerializer();
                        var ItemData = deserializer.Deserialize<AlbumArtists.Rootobject>(json);
                        _parent.Trace("---------------Get Album Artists :  Issue: Results.Record Count: " + ItemData.TotalRecordCount);

                        foreach (var genre in ItemData.Items)
                        {
                            MusicSingleArtistInfo.Rootobject ArtistItem = GetSingleArtist(genre.Id);

                            try
                            {
                                var artist = new ApiAudioArtist
                                  {
                                      IdArtist = Xbmc.IDtoNumber(genre.Id),
                                      Name = genre.Name ?? "",
                                      Thumb = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.Id + "/Images/Primary" ?? "",
                                      Fanart = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.Id + "/Images/Backdrop" ?? "",
                                      Biography = ArtistItem.Overview
                                  };
                                artists.Add(artist);
                            }

                            catch (Exception ex)
                            {
                                _parent.Trace("Another Album Artists Exception Caught " + ex);
                            }
                        }

                    }
                }
            }
            catch (Exception Ex)
            {
                _parent.Trace("Another Album Artists  exception" + Ex);
            }
            
            
            return artists;
        }




        public Collection<ApiAudioAlbum> GetAlbums()
        {
            var albums = new Collection<ApiAudioAlbum>();

            //var AlbumID = GetMainSelection("Albums");


            try
            {
                _parent.Trace("Getting Album : Parent IP: " + _parent.IP);
                string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items?Recursive=true&IncludeItemTypes=MusicAlbum&EnableImages=true";
                var request = WebRequest.CreateHttp(NPurl);
                request.Method = "get";
                request.Timeout = 150000;
                _parent.Trace("Genre Music Selection: " + NPurl);
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
                        _parent.Trace("--------------GETTING Albums Selection Result ------" + json);

                        var deserializer = new JavaScriptSerializer();
                        var ItemData = deserializer.Deserialize<MusicAlbums.Rootobject>(json);
                        _parent.Trace("---------------Get Album  :  Issue: Results.Record Count: " + ItemData.TotalRecordCount);

                        foreach (var genre in ItemData.Items)
                        {

                            try
                            {
                               
                                var album = new ApiAudioAlbum
                                          {
                                              IdAlbum = Xbmc.IDtoNumber(genre.Id),
                                              Title = genre.Name ?? "",
                                              IdGenre = 0,
                                              IdArtist = Xbmc.IDtoNumber(genre.AlbumArtists.FirstOrDefault().Id),
                                              Artist = genre.AlbumArtist ?? "",
                                              Genre = "",
                                              Year = genre.ProductionYear,
                                              Thumb = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.Id + "/Images/Primary" ?? "",
                                          };
                                albums.Add(album);
                            }
                            catch (Exception ex)
                            {
                                _parent.Trace("Music Album exception" + ex);
                            }
                        }

                    }
                }
            }
            catch (Exception Ex)
            {
                _parent.Trace("Music Album   exception" + Ex);
            }
            return albums;
        }

        public MusicSingleArtistInfo.Rootobject GetSingleArtist(string itemId)
        {
            try
            {

                _parent.Trace("Getting Single Artist From ItemData" + _parent.IP);
                string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items/" + itemId;

                var request = WebRequest.CreateHttp(NPurl);

                request.Method = "get";
                request.Timeout = 150000;
                _parent.Trace("Single Song Selection: " + NPurl);

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
                        _parent.Trace("--------------GETTING Single Artist From Series Selection Result ------" + json);

                        var deserializer = new JavaScriptSerializer();

                        var ItemData = deserializer.Deserialize<MusicSingleArtistInfo.Rootobject>(json);
                        _parent.Trace("---------------Get Single Artist From ItemData Selection:  Issue: Results.Taglines: " + ItemData.Taglines);

                        return ItemData;

                    }
                }


                return null;
            }
            catch (Exception ex)
            {
                _parent.Trace("ERROR in Single Artist Selection obtaining: " + ex);
                return null;

            }
        }


        public MusicSongSingleItem.Rootobject GetSingleSong(string itemId)
        {
            try
            {

                _parent.Trace("Getting Single Song From ItemData" + _parent.IP);
                string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items/" + itemId;

                var request = WebRequest.CreateHttp(NPurl);

                request.Method = "get";
                request.Timeout = 150000;
                _parent.Trace("Single Song Selection: " + NPurl);

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
                        _parent.Trace("--------------GETTING Single Artist From Series Selection Result ------" + json);

                        var deserializer = new JavaScriptSerializer();

                        var ItemData = deserializer.Deserialize<MusicSongSingleItem.Rootobject>(json);
                        _parent.Trace("---------------Get Single Artist From ItemData Selection:  Issue: Results.Taglines: " + ItemData.Taglines);

                        return ItemData;

                    }
                }


                return null;
            }
            catch (Exception ex)
            {
                _parent.Trace("ERROR in Single Song Selection obtaining: " + ex);
                return null;

            }
        }


        public Collection<ApiAudioSong> GetSongs()
        {
            var songs = new Collection<ApiAudioSong>();
           // var MusicID = GetMainSelection("Songs");


            try
            {
                _parent.Trace("Getting Songs: Parent IP: " + _parent.IP);
                string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items?Recursive=true&IncludeItemTypes=Audio&EnableImages=true"; 
                var request = WebRequest.CreateHttp(NPurl);
                request.Method = "get";
                request.Timeout = 150000;
                _parent.Trace("Songs Selection: " + NPurl);
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
                        _parent.Trace("--------------GETTING Songs Selection Result ------" + json);

                        var deserializer = new JavaScriptSerializer();
                        deserializer.MaxJsonLength = Int32.MaxValue;
                        var ItemData = deserializer.Deserialize<MusicSongs.Rootobject>(json);
                        
                        foreach (var genre in ItemData.Items)
                        {
                           
                            // Do get more data - but takes FOREVER !!
                             MusicSongSingleItem.Rootobject Songitem = GetSingleSong(genre.Id);
                            
                            var RoundSeconds = genre.RunTimeTicks / 10000000.00;

                            try
                            {
                                 var song = new ApiAudioSong
                                 {
                                     IdSong = Xbmc.IDtoNumber(genre.Id),
                                     Title = genre.Name ?? "",
                                     Track = Convert.ToInt64(genre.IndexNumber),
                                     Duration = Convert.ToInt64(RoundSeconds),
                                     Year = Convert.ToInt64(genre.ProductionYear),
                                     FileName = Songitem.Path,
                                     IdAlbum = Xbmc.IDtoNumber(genre.AlbumId),
                                     Album = genre.Album ?? "",
                                     Path = genre.Id,
                                     IdArtist = Xbmc.IDtoNumber(genre.AlbumArtists.FirstOrDefault().Id),
                                     Artist = genre.Artists.FirstOrDefault() ?? "",
                                     IdGenre = 0,
                                     Genre = Songitem.Genres.FirstOrDefault().ToString(),
                                     Thumb = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.Id + "/Images/Primary" ?? "",
                                 };
                                songs.Add(song);
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }

            return songs;
            }


            catch (Exception ex)
            {
                _parent.Trace("Exception Caught: " + ex);
                return null;
            }

        }
    }



}

