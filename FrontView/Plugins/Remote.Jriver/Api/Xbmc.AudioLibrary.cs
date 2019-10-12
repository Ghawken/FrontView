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
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Net;
using System.Linq;
using System.IO;

namespace Remote.Jriver.Api
{
  class XbmcAudioLibrary : IApiAudioLibrary
  {
    private readonly Xbmc _parent;

    public List<Dictionary<string, string>> Allitems = new List<Dictionary<string, string>>();

        public XbmcAudioLibrary(Xbmc parent)
    {
      _parent = parent;
    }

        public void getallItems(string url)
        {

            _parent.Trace("***** Jriver getallItems: Allitems Count =" + Allitems.Count.ToString());

            // Delete now misnamed allitems
            _parent.Trace("Deleting allItems as rerun...");
            Allitems.Clear();
            return;

            try
            {
                _parent.Trace(System.Environment.NewLine + "getAllItems:  Need to pull the whole Database...." + _parent.IP + System.Environment.NewLine);
                //string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/MCWS/v1/Files/Search?Action=mpl&ActiveFile=-1&Zone=-1&ZoneType=ID&Token=" + _parent.JRiverAuthToken;
                // http://192.168.1.97:52199/MCWS/v1/Files/Search?Action=mpl&ActiveFile=-1&Zone=-1&ZoneType=ID

                string NPurl = "http://" + _parent.IP + ":" + _parent.Port + url + "&Token=" + _parent.JRiverAuthToken;
                _parent.Trace(" Trying this:" + NPurl + System.Environment.NewLine);

                var request = WebRequest.CreateHttp(NPurl);
                request.Method = "get";
                request.Timeout = 15000000;
                var response = request.GetResponse();

                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {
                    //public List<Jriver.Api.AllDatabase.MPLItemField> items = new List<Jriver.Api.AllDatabase.MPLItemField>();
                    // Get the stream containing content returned by the server.
                    System.IO.Stream dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader.        
                    XmlSerializer serializer = new XmlSerializer(typeof(Jriver.Api.AllDatabase.MPL));
                    System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);
                    var deserialized = (Jriver.Api.AllDatabase.MPL)serializer.Deserialize(reader);

                    foreach (var entry in deserialized.Item)
                    {
                        Dictionary<string, string> newItem = new Dictionary<string, string>();
                        foreach (var Field in entry.Field)
                        {
                            newItem.Add(Field.Name, Field.Text);
                            //   _parent.Trace("*** Adding:" + Field.Name + "  and Value=" + Field.Text);
                        }
                        Allitems.Add(newItem);
                    }
                }
            }
            catch (Exception Ex)
            {
                _parent.Trace(System.Environment.NewLine + "Another getAllitems exception" + Ex);
            }

        }

        public Collection<ApiAudioGenre> GetGenres()
        {
            _parent.Trace("JRiver Get Genres Running");
            var genres = new Collection<ApiAudioGenre>();
            return genres;


            try
            {
                getallItems("/MCWS/v1/Files/Search?Action=mpl&ActiveFile=-1&Zone=-1&ZoneType=ID&Query=[Media%20Type]=[Audio]");
                foreach (var Field in Allitems)
                {
                    string result = "";
                    if (Field.TryGetValue("Genre", out result))
                    {
                        if (result != "" && result !=null)
                        {
                            //Only take Genre from those entries that have Album Name -

                            string GenreName = Field.ValueOrDefault("Genre");

                            if (GenreName != "" && GenreName !=null)
                            {
                                var gen = new ApiAudioGenre
                                {
                                    IdGenre = (long)Xbmc.IDstringtoNumber(GenreName),
                                    Name = GenreName.ToString(),
                                    AlbumCount = 0,
                                    Thumb = ""
                                };

                                if (!genres.Any(a => a.Name == GenreName))  //check Genre doesnt already exisit
                                {
                                    genres.Add(gen);
                                }
                            }
                        }
                    }
                }

                return genres;
            }
            catch (Exception ex)
            {
                _parent.Trace("JRiver GetGenre:" + ex);
                return genres;
            }
    }

        public Collection<ApiAudioArtist> GetArtists()
        {

            _parent.Trace("JRiver Get Artists Running");
            var artists = new Collection<ApiAudioArtist>();
            return artists;

            try
            {


                getallItems("/MCWS/v1/Files/Search?Action=mpl&ActiveFile=-1&Zone=-1&ZoneType=ID&Query=[Media%20Type]=[Audio]");
                foreach (var Field in Allitems)
                {

                    string result = "";
                    if (Field.TryGetValue("Album", out result))
                    {
                        if (result != "" & result !=null)
                        {
                            //Only take Artists from those entries that have Album Name -

                            string GenreName = Field.ValueOrDefault("Genre");
                            string Artist = Field.ValueOrDefault("Artist");

                            if (Artist != "" && Artist !=null)
                            {
                                var artist = new ApiAudioArtist
                                {
                                    IdArtist = (long)Xbmc.IDstringtoNumber(Artist),
                                    Name = Artist,
                                    Thumb = "",
                                    Fanart = "",
                                    Biography = ""
                                };


                                if (!artists.Any(a => a.Name == Artist))
                                {
                                    artists.Add(artist);
                                }
                            }
                        }
                    }


                }
                return artists;
            }
            catch (Exception ex)
            {
                _parent.Trace("JRiver GetArtists:" + ex);
                return artists;
            }
        }


    public Collection<ApiAudioAlbum> GetAlbums()
    {
            _parent.Trace("JRiver GetAlbums Running");
            var albums = new Collection<ApiAudioAlbum>();
            return albums;

            try
            {
                getallItems("/MCWS/v1/Files/Search?Action=mpl&ActiveFile=-1&Zone=-1&ZoneType=ID&Query=[Media%20Type]=[Audio]");
                foreach (var Field in Allitems)
                {
                    string result = "";
                    if (Field.TryGetValue("Album", out result))
                    {
                        if (result != "" & result !=null)
                        {
                            //Only take Artists from those entries that have Album Name -
                            string Name = Field.ValueOrDefault("Name");
                            long Year;
                            if (Field.ValueOrDefault("Year") != "")
                            {
                                Year = Convert.ToInt64(Field.ValueOrDefault("Date (readable)"));
                            }
                            else
                            {
                                Year = 1900;
                            }
                            string FileKey = "";
                            string Thumb2 = "";
                            string Fanart2 = "";
                            string cdart = "";
                            string Frontjpg = "";
                            if (Field.TryGetValue("Key", out FileKey))
                            {
                                Thumb2 = "http://" + _parent.IP + ":" + _parent.Port + "/MCWS/v1/File/GetImage?File=" + FileKey + "&FileType=Key&Type=Thumbnail&Format=jpg&Token=" + _parent.JRiverAuthToken ?? "";
                                Fanart2 = "http://" + _parent.IP + ":" + _parent.Port + "/MCWS/v1/File/GetImage?File=" + FileKey + "&FileType=Key&Type=Full&Format=jpg&Token=" + _parent.JRiverAuthToken ?? "";
                            }
                            // use directory for fanart if exists
                            var filename = Field.ValueOrDefault("Filename");
                            if (filename != null & filename != "")
                            {
                                FileInfo[] fiJpgs = new System.IO.FileInfo(filename).Directory.GetFiles("*.jpg");
                                FileInfo[] fiPngs = new System.IO.FileInfo(filename).Directory.GetFiles("*.png");
                                if (fiJpgs.Length == 0 && fiPngs.Length == 0)
                                {
                                    _parent.Trace("No Jpgs or PngsFound");
                                }
                                else
                                {
                                    if (fiJpgs.Any(item => item.FullName.Contains("Front")) )
                                    {
                                        Thumb2 = fiJpgs.First(item => item.FullName.Contains("Front")).FullName;
                                    }
                                    else if (fiJpgs.Any(item => item.FullName.Contains("folder")))
                                    {
                                        Thumb2 = fiJpgs.First(item => item.FullName.Contains("folder")).FullName;
                                    }
                                    else if (fiPngs.Any(item => item.FullName.Contains("cdart")))
                                    {
                                        Thumb2 = fiPngs.First(item => item.FullName.Contains("cdart")).FullName;
                                    }
                                    else if (fiJpgs.Any(item => item.FullName.Contains("jpg")))
                                    {
                                        Thumb2 = fiJpgs.First(item => item.FullName.Contains("jpg")).FullName;
                                    }
                                    // ****************************************************
                                    if (fiJpgs.Any(item => item.FullName.Contains("fanart")))
                                    {
                                        Fanart2 = fiJpgs.First(item => item.FullName.Contains(@"*fanart*")).FullName;
                                    }
                                    else if (fiPngs.Any(item => item.FullName.Contains("fanart.png")))
                                    {
                                        Fanart2 = fiPngs.First(item => item.FullName.Contains("fanart")).FullName;
                                    }
                                    else if (fiJpgs.Any(item => item.FullName.Contains("jpg")))
                                    {
                                        Fanart2 = fiJpgs.First(item => item.FullName.Contains("jpg")).FullName;
                                    }

                                    //var filePath = Path.GetDirectoryName(Field.ValueOrDefault("Filename"));
                                    //var fanartPath = Path.Combine(filePath, "fanart.jpg");
                                    //var cdartpath = Path.Combine(filePath, "cdart.png");
                                    //var ThumbPath = Path.Combine(filePath, "folder.jpg");
                                    //var BannerPath = Path.Combine(filePath, "banner.jpg");
                                    //var Frontpath = Path.Combine(filePath, "*Front.jpg");
                                    //System.IO.FileInfo fi = new System.IO.FileInfo(fanartPath);
                                    //System.IO.FileInfo ficdart = new System.IO.FileInfo(cdartpath);
                                    //System.IO.FileInfo fiThumb = new System.IO.FileInfo(ThumbPath);

                                    //if (fi.Exists)
                                    //{
                                    //    Fanart2 = fanartPath;  //if fanart.jpg exisits in directory with movie use this otherwise default to JRiver Thumb
                                    //}

                                    //if (fiThumb.Exists)
                                    //{
                                    //    Thumb2 = ThumbPath;
                                    //}

                                }
                            }


                            _parent.Trace("GetAlbum Art: " + Thumb2);
                            string GenreName = "Unknown";
                            if (Field.TryGetValue("Genre", out result))
                            {
                                GenreName = Field.ValueOrDefault("Genre");
                            }
                            string Album = "Unknown";
                            if (Field.TryGetValue("Album", out result))
                            {
                                Album = Field.ValueOrDefault("Album");
                            }
                            string Artist = "Unknown";
                            if (Field.TryGetValue("Artist", out result))
                            {
                                Artist = Field.ValueOrDefault("Artist");
                            }
                            if (Album != "" && Album !=null)
                            {
                                var album = new ApiAudioAlbum
                                {
                                    IdAlbum = (long)Xbmc.IDstringtoNumber(Album),
                                    Title = Album,
                                    IdGenre = (long)Xbmc.IDstringtoNumber(GenreName),
                                    IdArtist = (long)Xbmc.IDstringtoNumber(Artist),
                                    Artist = Artist,
                                    Genre = GenreName,
                                    Year = Year,
                                    Thumb = Thumb2,
                                    Fanart = Fanart2,
                                    Hash = Xbmc.Hash(Album)
                                };
                                if (!albums.Any(a => a.Title == Album))
                                {
                                    albums.Add(album);
                                }
                            }
                        }
                    }


                }
                return albums;
            }
            catch (Exception ex)
            {
                _parent.Trace("JRiver GetALbums:" + ex);
                return albums;
            }
        }

    public Collection<ApiAudioSong> GetSongs()
    {
            _parent.Trace("JRiver GetSongs Running");
            var songs = new Collection<ApiAudioSong>();
            return songs;

            try
            {
                getallItems("/MCWS/v1/Files/Search?Action=mpl&ActiveFile=-1&Zone=-1&ZoneType=ID&Query=[Media%20Type]=[Audio]");
                foreach (var Field in Allitems)
                {
                    string result = "";
                    if (Field.TryGetValue("Name", out result))
                    {
                        if (result != "" && result != null)
                        {
                            //Only take Artists from those entries that have Album Name -
                            string GenreName = Field.ValueOrDefault("Genre");
                            string Artist = Field.ValueOrDefault("Artist");
                            string Name = Field.ValueOrDefault("Name");
                            string Album = Field.ValueOrDefault("Album");
                            string Filename = Field.ValueOrDefault("Filename");
                            long Year;
                            try
                            {
                                if (Field.ValueOrDefault("Year") != "")
                                {
                                    Year = Convert.ToInt64(Field.ValueOrDefault("Date (readable)"));
                                }
                                else
                                {
                                    Year = 1900;
                                }
                            }
                            catch
                            {
                                _parent.Trace("Error Date Conversion: Using 1900");
                                Year = 1900;
                            }
                            long TrackNo;
                            try
                            {
                                if (Field.ValueOrDefault("Track #") != "")
                                {
                                    TrackNo = Convert.ToInt64(Field.ValueOrDefault("Track #"));
                                }
                                else
                                {
                                    TrackNo = 1;
                                }
                            }
                            catch
                            {
                                _parent.Trace("Error TrackNo Using 1");
                                TrackNo = 1;
                            }
                            int DurationNumber = 0;
                            string Duration = "";
                            if (Field.TryGetValue("Duration", out Duration))
                            {
                                DurationNumber = int.TryParse(Duration, out DurationNumber) ? DurationNumber : 0;
                            }

                            string FileKey = "";
                            string Thumb2 = "";
                            string Fanart2 = "";
                            string cdart = "";
                            if (Field.TryGetValue("Key", out FileKey))
                            {
                                Thumb2 = "http://" + _parent.IP + ":" + _parent.Port + "/MCWS/v1/File/GetImage?File=" + FileKey + "&FileType=Key&Type=Thumbnail&Format=jpg&Token=" + _parent.JRiverAuthToken ?? "";
                                Fanart2 = "http://" + _parent.IP + ":" + _parent.Port + "/MCWS/v1/File/GetImage?File=" + FileKey + "&FileType=Key&Type=Full&Format=jpg&Token=" + _parent.JRiverAuthToken ?? "";
                            }
                            // use directory for fanart if exists
                            var filename = Field.ValueOrDefault("Filename");
                            if (filename != null & filename != "")
                            {
                                var filePath = Path.GetDirectoryName(Field.ValueOrDefault("Filename"));
                                var fanartPath = Path.Combine(filePath, "fanart.jpg");
                                var cdartpath = Path.Combine(filePath, "cdart.png");
                                var ThumbPath = Path.Combine(filePath, "folder.jpg");
                                var BannerPath = Path.Combine(filePath, "banner.jpg");
                                
                                System.IO.FileInfo fi = new System.IO.FileInfo(fanartPath);
                                System.IO.FileInfo ficdart = new System.IO.FileInfo(cdartpath);
                                System.IO.FileInfo fiThumb = new System.IO.FileInfo(ThumbPath);

                                if (fi.Exists)
                                {
                                    Fanart2 = fanartPath;  //if fanart.jpg exisits in directory with movie use this otherwise default to JRiver Thumb
                                }

                                if (fiThumb.Exists)
                                {
                                    Thumb2 = ThumbPath;
                                }

                                if (ficdart.Exists)
                                {
                                    cdart = cdartpath;
                                }

                                // use cdart or album
                                if (cdart != "")  // use this else Thumb (folder.jpg currently) or failing that Thumb from JRiver 
                                                  // can access mp3 file data??
                                {
                                    Thumb2 = cdart;
                                }
                            }

                            if (Name != "")
                            {
                                var song = new ApiAudioSong
                                {
                                    IdSong = (long)Xbmc.IDstringtoNumber(Name),
                                    Title = Name,
                                    Track = TrackNo,
                                    Duration = (long)DurationNumber,
                                    Year = (long)Year,
                                    FileName = "",
                                    IdAlbum = (long)Xbmc.IDstringtoNumber(Album),
                                    Album = Album,
                                    Path = Filename,
                                    IdArtist = (long)Xbmc.IDstringtoNumber(Artist),
                                    Artist = Artist,
                                    IdGenre = (long)Xbmc.IDstringtoNumber(GenreName),
                                    Genre = GenreName,
                                    Thumb = Thumb2,
                                };
                                songs.Add(song);
                            }
                        }
                    }

                }

                return songs;
            }
            catch (Exception ex)
            {
                _parent.Trace("JRiver GetSongs:" + ex);
                return songs;
            }
        }

  }
}
