//------------------------------------------------------------------------
//   YATSE 2 - A touch screen remote controller for XBMC(.NET 3.5)
//   Copyright(C) 2010  Tolriq(http://yatse.leetzone.org)


//  This program is free software: you can redistribute it and/or modify

//  it under the terms of the GNU General Public License as published by

//  the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.

//  This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//   GNU General Public License for more details.

//   You should have received a copy of the GNU General Public License
//   along with this program.If not, see<http://www.gnu.org/licenses/>.
//------------------------------------------------------------------------

using System;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Net;
using System.Web.Script.Serialization;
using System.Web;
using System.Web.Script;
using Plugin;
using Jayrock.Json;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.IO;

namespace Remote.Jriver.Api
{
    public static class DictionaryExtensions
    {
        public static T1 ValueOrDefault<T, T1>(this IDictionary<T, T1> dictionary, T key)
        {
            if (key == null || dictionary == null)
                return default(T1);

            T1 value;
            return dictionary.TryGetValue(key, out value) ? value : default(T1);
        }
    }

    class XbmcVideoLibrary : IApiVideoLibrary
    {
        private readonly Xbmc _parent;

        public List<Dictionary<string, string>> Allitems = new List<Dictionary<string, string>>();

        public XbmcVideoLibrary(Xbmc parent)
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

                string NPurl = "http://" + _parent.IP + ":" + _parent.Port + url+"&Token="+_parent.JRiverAuthToken;
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

        public Collection<ApiTvSeason> GetTvSeasonsRefresh()
        {
            var seasons = new Collection<ApiTvSeason>();

            return seasons;
            // no refresh possible for Jrvier

            try
            {
                // Load full database...
                getallItems("/MCWS/v1/Files/Search?Action=mpl&ActiveFile=-1&Zone=-1&ZoneType=ID&Query=[Media%20Sub%20Type]=[TV%20Show]");

                // iterate through Items
                foreach (var Field in Allitems)
                {
                    var result = "";
                    var isTVshow = false;
                    if (Field.TryGetValue("Media Sub Type", out result))
                    {
                        if (result == "TV Show"){
                            isTVshow = true;
                        }
                    }
                    var SeriesName = "";
                    if (Field.TryGetValue("Series", out result))
                    {
                        SeriesName = result;
                    }                 
                    long SeasonNumber = 0;
                    string Season = "";
                    if (Field.TryGetValue("Season", out Season))
                    {
                        SeasonNumber = long.TryParse(Season, out SeasonNumber) ? SeasonNumber : 0;
                    }
                    string tvdb = "0";
                    if (Field.TryGetValue("TheTVDB Series ID", out tvdb))
                    {
                        tvdb = tvdb;
                    }

                    string FileKey = "";
                    string Thumb = "";
                    string Fanart = "";
                    if (Field.TryGetValue("Key", out FileKey))
                    {
                        Thumb = "http://" + _parent.IP + ":" + _parent.Port + "/MCWS/v1/File/GetImage?File=" + FileKey + "&FileType=Key&Type=Thumbnail&Format=jpg&Token=" + _parent.JRiverAuthToken ?? "";
                        Fanart = "http://" + _parent.IP + ":" + _parent.Port + "/MCWS/v1/File/GetImage?File=" + FileKey + "&FileType=Key&Type=Full&Format=jpg&Token=" + _parent.JRiverAuthToken ?? "";

                    }
                    //var SingleTVData = GetSingleTVFromSeries(genre.Id);
                    //_parent.Trace("---Emby QuickRefresh GetTVSeasons--- Season Number:" + SeasonNumber);
                    //_parent.Trace("---Emby QuickRefresh GetTVSeasons--- ID Show:" + Xbmc.IDtoNumber(SeriesName));
                    //_parent.Trace("---Emby QuickRefresh GetTVSeasons--- Series Name:" + SeriesName);
                    //_parent.Trace("---Emby QuickRefresh GetTVSeasons--- Thumb:" + Thumb);
                    //_parent.Trace("---Emby QuickRefresh GetTVSeasons--- Child Count:" + 1);
                    //_parent.Trace("---Emby QuickRefresh GetTVSeasons--- Fanart:" + Fanart);
                    //_parent.Trace("---Emby QuickRefresh GetTVSeasons--- Hash:" + Xbmc.Hash(SeriesName));

                    var tvShow = new ApiTvSeason
                    {
                        SeasonNumber = SeasonNumber,
                        IdShow = Xbmc.IDtoNumber(tvdb),
                        Show = SeriesName ?? "",
                        Thumb = Thumb ?? "",
                        EpisodeCount = 1,   //bit of a hack but if date sorted - latest episode should be highest - so for most should be right.
                        Fanart = Fanart ?? "",
                        Hash = Xbmc.Hash(SeriesName)
                    };
                    // need to check is season and make sure isn't already added

                    if (isTVshow && !seasons.Contains(tvShow) )
                    {
                        seasons.Add(tvShow);
                    }
                }
            }
            catch (Exception ex)
            {
                _parent.Trace("TV Shows Exception Caught " + ex);
            }
            return seasons;
        }


public Collection<ApiTvSeason> GetTvSeasons()
{
            var seasons = new Collection<ApiTvSeason>();

            // 
            return seasons;
            //

            try
            {
                // Load full database...
                getallItems("/MCWS/v1/Files/Search?Action=mpl&ActiveFile=-1&Zone=-1&ZoneType=ID&Query=[Media%20Sub%20Type]=[TV%20Show]");

                // iterate through Items
                foreach (var Field in Allitems)
                {
                    var result = "";
                    var isTVshow = false;
                    if (Field.TryGetValue("Media Sub Type", out result))
                    {
                        if (result == "TV Show")
                        {
                            isTVshow = true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    var SeriesName = "";
                    if (Field.TryGetValue("Series", out result))
                    {
                        SeriesName = result;
                    }
                    long SeasonNumber = 0;
                    string Season = "";
                    if (Field.TryGetValue("Season", out Season))
                    {
                        SeasonNumber = long.TryParse(Season, out SeasonNumber) ? SeasonNumber : 0;
                    }
                    string FileKey = "";
                    string Thumb2 = "";
                    string Fanart2 = "";
                    if (Field.TryGetValue("Key", out FileKey))
                    {
                        Thumb2 = "http://" + _parent.IP + ":" + _parent.Port + "/MCWS/v1/File/GetImage?File=" + FileKey + "&FileType=Key&Type=Thumbnail&Format=jpg&Token=" + _parent.JRiverAuthToken ?? "";
                        Fanart2 = "http://" + _parent.IP + ":" + _parent.Port + "/MCWS/v1/File/GetImage?File=" + FileKey + "&FileType=Key&Type=Full&Format=jpg&Token=" + _parent.JRiverAuthToken ?? "";
                    }
                    var filename = Field.ValueOrDefault("Filename");
                    if (filename != null & filename != "")
                    {
                        var filePath = Path.GetDirectoryName(Field.ValueOrDefault("Filename"));
                        var fanartPath = Path.Combine(filePath, "fanart.jpg");
                        var ThumbPath = Path.Combine(filePath, "poster.jpg");
                        System.IO.FileInfo fi = new System.IO.FileInfo(fanartPath);
                        System.IO.FileInfo fiThumb = new System.IO.FileInfo(ThumbPath);
                        if (fi.Exists)
                        {
                            Fanart2 = fanartPath;  //if fanart.jpg exisits in directory with movie use this otherwise default to JRiver Thumb
                        }
                        if (fiThumb.Exists)
                        {
                            Thumb2 = ThumbPath;
                        }
                    }
                    string tvdb = "0";

                    string ShowName = Field.ValueOrDefault("Series");

                    if (ShowName == "")
                    {
                        ShowName = "Unknown Series";
                    }

                    var tvShow = new ApiTvSeason
                    {
                        SeasonNumber = SeasonNumber,
                        IdShow = Xbmc.IDstringtoNumber(ShowName),
                        Show = SeriesName ?? "",
                        Thumb = Thumb2 ?? "",
                        EpisodeCount = 1,   //bit of a hack but if date sorted - latest episode should be highest - so for most should be right.
                        Fanart = Fanart2 ?? "",      
                        Hash = Xbmc.Hash(ShowName)
                    };
                    // need to check is season and make sure isn't already added

                    if (isTVshow && !seasons.Contains(tvShow))
                    {
                        seasons.Add(tvShow);
                    }
                }
            }
            catch (Exception ex)
            {
                _parent.Trace("TV Shows Exception Caught " + ex);
            }
            return seasons;
        }

            
public Collection<ApiTvEpisode> GetTvEpisodes()
{

    var episodes = new Collection<ApiTvEpisode>();
            //
            return episodes;
            //

            try
    {
                getallItems("/MCWS/v1/Files/Search?Action=mpl&ActiveFile=-1&Zone=-1&ZoneType=ID&Query=[Media%20Sub%20Type]=[TV%20Show]");
                _parent.Trace("GetTVEpisodes : Parent IP: " + _parent.IP);
                // iterate through Items
                foreach (var Field in Allitems)
                {
                    var result = "";
                    var isTVshow = false;
                    if (Field.TryGetValue("Media Sub Type", out result))
                    {
                        if (result == "TV Show")
                        {
                            isTVshow = true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    long PlayCount = 0;
                    string Plays = "";
                    if (Field.TryGetValue("Number Plays", out Plays))
                    {
                        PlayCount = long.TryParse(Plays, out PlayCount) ? PlayCount : 0;
                    }
                    long SeasonNumber = 0;
                    string Season = "";
                    if (Field.TryGetValue("Season", out Season))
                    {
                        SeasonNumber = long.TryParse(Season, out SeasonNumber) ? SeasonNumber : 0;
                    }
                    long EpisodeNumber = 0;
                    string EpisodeNo = "";
                    if (Field.TryGetValue("Episode", out EpisodeNo))
                    {
                        EpisodeNumber = long.TryParse(EpisodeNo, out EpisodeNumber) ? EpisodeNumber : 0;
                    }
                    string FileKey = "";
                    string Thumb2 = "";
                    string Fanart2 = "";
                    string LogoArt = "";
                    string BannerArt = "";
                    if (Field.TryGetValue("Key", out FileKey))
                    {
                        Thumb2 = "http://" + _parent.IP + ":" + _parent.Port + "/MCWS/v1/File/GetImage?File=" + FileKey + "&FileType=Key&Type=Thumbnail&Format=jpg&Token=" + _parent.JRiverAuthToken ?? "";
                        Fanart2 = "http://" + _parent.IP + ":" + _parent.Port + "/MCWS/v1/File/GetImage?File=" + FileKey + "&FileType=Key&Type=Full&Format=jpg&Token=" + _parent.JRiverAuthToken ?? "";
                    }
                    var filename = Field.ValueOrDefault("Filename");
                    if (filename != null & filename != "")
                    {
                        var filePath = Path.GetDirectoryName(Field.ValueOrDefault("Filename"));
                        var fanartPath = Path.Combine(filePath, "fanart.jpg");
                        System.IO.FileInfo fi = new System.IO.FileInfo(fanartPath);
                        if (fi.Exists)
                        {
                            Fanart2 = fanartPath;  //if fanart.jpg exisits in directory with movie use this otherwise default to JRiver Thumb
                        }

                    }
                    DateTime myDateTime = new DateTime();
                    try
                    {
                        string datestring = "01/01/1900";

                        if (Field.TryGetValue("Date Created", out datestring))
                        {
                            myDateTime = DateTime.Parse(datestring);
                        }
                    }
                    catch (Exception ex)
                    {
                 //       _parent.Trace("JRiver Error:  dateTime Exception: " + ex);
                        myDateTime = DateTime.Parse("1900-01-01");
                    }
                    string sqlFormattedDate = myDateTime.ToString("s");

                    string tvdb = "0";
                    string ShowName = Field.ValueOrDefault("Series");

                    if (ShowName == "")
                    {
                        ShowName = "Unknown Series";
                    }

                    if (isTVshow)
                    {
                        var tvShow = new ApiTvEpisode
                        {
                            Title = Field.ValueOrDefault("Name"),
                            Plot = Field.ValueOrDefault("Comment"),
                            Rating = "",
                            Mpaa = Field.ValueOrDefault("MPAA Rating"),
                            Date = sqlFormattedDate,
                            Director = Field.ValueOrDefault("Director"),
                            PlayCount = PlayCount,
                            Studio = Field.ValueOrDefault("Studio"),
                            IdEpisode = Xbmc.IDtoNumber(FileKey),
                            IdShow = Xbmc.IDstringtoNumber(ShowName),
                            Season = SeasonNumber,
                            Episode = EpisodeNumber,
                            Path = Field.ValueOrDefault("Filename"),
                            ShowTitle = ShowName,
                            Thumb = Thumb2,
                            Fanart = Fanart2,
                            FileName = filename,
                            Hash = Xbmc.Hash(FileKey)
                        };
                            episodes.Add(tvShow);
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

            return shows;
            // No refresh possible for Jriver that I can see

            try
            {
                getallItems("/MCWS/v1/Files/Search?Action=mpl&ActiveFile=-1&Zone=-1&ZoneType=ID&Query=[Media%20Sub%20Type]=[TV%20Show]");
                _parent.Trace("GetTVEpisodes : Parent IP: " + _parent.IP);
                // iterate through Items
                foreach (var Field in Allitems)
                {
                    var result = "";
                    var isTVshow = false;
                    if (Field.TryGetValue("Media Sub Type", out result))
                    {
                        if (result == "TV Show")
                        {
                            isTVshow = true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    long PlayCount = 0;
                    string Plays = "";
                    if (Field.TryGetValue("Number Plays", out Plays))
                    {
                        PlayCount = long.TryParse(Plays, out PlayCount) ? PlayCount : 0;
                    }
                    long SeasonNumber = 0;
                    string Season = "";
                    if (Field.TryGetValue("Season", out Season))
                    {
                        SeasonNumber = long.TryParse(Season, out SeasonNumber) ? SeasonNumber : 0;
                    }
                    long EpisodeNumber = 0;
                    string EpisodeNo = "";
                    if (Field.TryGetValue("Episode", out EpisodeNo))
                    {
                        EpisodeNumber = long.TryParse(EpisodeNo, out EpisodeNumber) ? EpisodeNumber : 0;
                    }
                    string FileKey = "";
                    string Thumb2 = "";
                    string Fanart2 = "";
                    if (Field.TryGetValue("Key", out FileKey))
                    {
                        Thumb2 = "http://" + _parent.IP + ":" + _parent.Port + "/MCWS/v1/File/GetImage?File=" + FileKey + "&FileType=Key&Type=Thumbnail&Format=jpg&Token=" + _parent.JRiverAuthToken ?? "";
                        Fanart2 = "http://" + _parent.IP + ":" + _parent.Port + "/MCWS/v1/File/GetImage?File=" + FileKey + "&FileType=Key&Type=Full&Format=jpg&Token=" + _parent.JRiverAuthToken ?? "";
                    }
                    
                    string tvdb = "0";
                    if (Field.TryGetValue("TheTVDB Series ID", out tvdb))
                    {
                        _parent.Trace("TVDB found");
                    }
                    _parent.Trace(" *****************");
                    _parent.Trace(Field.ValueOrDefault("Series"));
                    _parent.Trace(Field.ValueOrDefault("Comment"));
                    _parent.Trace(Field.ValueOrDefault("MPAA Rating"));
                    _parent.Trace(Field.ValueOrDefault("Studio"));
                    _parent.Trace("2"+Field.ValueOrDefault("Genre"));
                    _parent.Trace("3"+Field.ValueOrDefault("Filename"));
                    _parent.Trace("4"+Field.ValueOrDefault("Genre"));
                    _parent.Trace("5"+Xbmc.IDtoNumber(tvdb).ToString());
                    _parent.Trace("6"+Field.ValueOrDefault(@"Date (readable)"));
                    _parent.Trace(Thumb2);
                    _parent.Trace(Fanart2);
                    _parent.Trace(Xbmc.Hash(FileKey) );
  
                     var tvShow = new ApiTvShow
                    {
                        Title = Field.ValueOrDefault("Series"),
                        Plot = Field.ValueOrDefault("Comment"),
                        Rating = "",
                        Mpaa = Field.ValueOrDefault("MPAA Rating"),
                        Studio = Field.ValueOrDefault("Studio"),
                        Path = Field.ValueOrDefault("Filename"),
                        IdScraper = "",
                        Genre = Field.ValueOrDefault("Genre"),
                        IdShow = Xbmc.IDtoNumber(tvdb),
                        TotalCount = 0,
                        Premiered = Field.ValueOrDefault("Date (readable)"),
                        Thumb = Thumb2,
                        Fanart = Fanart2,
                        Banner = "NoBannerAddCodetoCheckPath",
                        Logo = "NoLogoCheckCodetoCheckLogo",
                        Hash = Xbmc.Hash(FileKey)
                    };
                    if (isTVshow && !shows.Contains(tvShow))
                    {
                        shows.Add(tvShow);
                    }
                }
            }
            catch (Exception ex)
            {
                _parent.Trace("TV Shows REFRESH Exception Caught " + ex);
            }
      return shows;
}


public Collection<ApiTvShow> GetTvShows()
{
            //var MovieId = GetMainSelection("TV");
            var shows = new Collection<ApiTvShow>();
            return shows;
            //

            try
            {
                getallItems("/MCWS/v1/Files/Search?Action=mpl&ActiveFile=-1&Zone=-1&ZoneType=ID&Query=[Media%20Sub%20Type]=[TV%20Show]");
                _parent.Trace("GetTVEpisodes : Parent IP: " + _parent.IP);
                // iterate through Items
                foreach (var Field in Allitems)
                {
                    var result = "";
                    var isTVshow = false;
                    if (Field.TryGetValue("Media Sub Type", out result))
                    {
                        if (result == "TV Show")
                        {
                            isTVshow = true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    long PlayCount = 0;
                    string Plays = "";
                    if (Field.TryGetValue("Number Plays", out Plays))
                    {
                        PlayCount = long.TryParse(Plays, out PlayCount) ? PlayCount : 0;
                    }
                    long SeasonNumber = 0;
                    string Season = "";
                    if (Field.TryGetValue("Season", out Season))
                    {
                        SeasonNumber = long.TryParse(Season, out SeasonNumber) ? SeasonNumber : 0;
                    }
                    long EpisodeNumber = 0;
                    string EpisodeNo = "";
                    if (Field.TryGetValue("Episode", out EpisodeNo))
                    {
                        EpisodeNumber = long.TryParse(EpisodeNo, out EpisodeNumber) ? EpisodeNumber : 0;
                    }
                    string FileKey = "";
                    string Thumb2 = "";
                    string Fanart2 = "";
                    string LogoArt = "";
                    string BannerArt = "";
                    if (Field.TryGetValue("Key", out FileKey))
                    {
                        Thumb2 = "http://" + _parent.IP + ":" + _parent.Port + "/MCWS/v1/File/GetImage?File=" + FileKey + "&FileType=Key&Type=Thumbnail&Format=jpg&Token=" + _parent.JRiverAuthToken ?? "";
                        Fanart2 = "http://" + _parent.IP + ":" + _parent.Port + "/MCWS/v1/File/GetImage?File=" + FileKey + "&FileType=Key&Type=Full&Format=jpg&Token=" + _parent.JRiverAuthToken ?? "";
                    }
                    var filename = Field.ValueOrDefault("Filename");
                    if (filename != null & filename != "")
                    {
                        var filePath = Path.GetDirectoryName(Field.ValueOrDefault("Filename"));
                        var fanartPath = Path.Combine(filePath, "fanart.jpg");
                        var LogoPath = Path.Combine(filePath, "logo.png");
                        var ThumbPath = Path.Combine(filePath, "poster.jpg");
                        var BannerPath = Path.Combine(filePath, "banner.jpg");
                        System.IO.FileInfo fi = new System.IO.FileInfo(fanartPath);
                        System.IO.FileInfo fiLogo = new System.IO.FileInfo(LogoPath);
                        System.IO.FileInfo fiThumb = new System.IO.FileInfo(ThumbPath);
                        System.IO.FileInfo fiBanner = new System.IO.FileInfo(BannerPath);
                     //   _parent.Trace("JRiver: ** filePath ** :" + filePath);
                     //   _parent.Trace("JRiver: ** fanArt.Jpg ** :" + fanartPath);
                     //   _parent.Trace("JRiver: ** Logo.Png ** :" + LogoPath);
                        if (fi.Exists)
                        {
                            Fanart2 = fanartPath;  //if fanart.jpg exisits in directory with movie use this otherwise default to JRiver Thumb
                        }
                        if (fiLogo.Exists)
                        {
                            LogoArt = LogoPath;
                        }
                        if (fiThumb.Exists)
                        {
                            Thumb2 = ThumbPath;
                        }
                        if (fiBanner.Exists)
                        {
                            BannerArt = BannerPath;
                        }
                    }
                    string ShowName = Field.ValueOrDefault("Series");

                    if (ShowName == "")
                    {
                        ShowName = "Unknown Series";
                    }
                    long idShow = Xbmc.IDstringtoNumber(ShowName);

                    var tvShow = new ApiTvShow
                    {
                        Title = ShowName,
                        Plot = Field.ValueOrDefault("Comment"),
                        Rating = "",
                        Mpaa = Field.ValueOrDefault("MPAA Rating"),
                        Studio = Field.ValueOrDefault("Studio"),
                        Path = Field.ValueOrDefault("Filename"),
                        IdScraper = "",
                        Genre = Field.ValueOrDefault("Genre"),
                        IdShow = idShow,
                        TotalCount = 0,
                        Premiered = Field.ValueOrDefault("Date (readable)"),
                        Thumb = Thumb2,
                        Fanart = Fanart2,
                        Banner = BannerArt,
                        Logo = LogoArt,
                        Hash = Xbmc.Hash(FileKey)
                    };

                    if (isTVshow && !shows.Any(a => a.IdShow == idShow))
                    {
                        _parent.Trace("JRiver: New Show Found: Adding Show:" + ShowName);
                        shows.Add(tvShow);
                    }
                }
            }
            catch (Exception ex)
            {
                _parent.Trace("TV Shows REFRESH Exception Caught " + ex);
            }
            return shows;
        }

public Collection<ApiMovie> GetMoviesRefresh()
{
        var movies = new Collection<ApiMovie>();

            //
            return movies;
            //

            try
                {
                getallItems("/MCWS/v1/Files/Search?Action=mpl&ActiveFile=-1&Zone=-1&ZoneType=ID&Query=[Media%20Sub%20Type]=[Movie]");
                _parent.Trace("GetMoviesRefresh  : Parent IP: " + _parent.IP);
                    // iterate through Items
                    foreach (var Field in Allitems)
                    {
                        var isMovie = false;
                        //_parent.Trace(Field.Values.ToString());
                        var Logoutput = "";
                        Field.TryGetValue("Name", out Logoutput);
                       // _parent.Trace("************* Logging : " + Logoutput);

                        var result = "";

                        if (Field.TryGetValue("Media Sub Type", out result))
                        {
                            if (result == "Movie")
                            {
                                isMovie = true;
                                _parent.Trace("Movie Found: " + Logoutput + " and isMovie true");
                            }
                            else
                            {
                                continue;
                            }
                        }

                        long PlayCount = 0;
                        string Plays = "";
                        if (Field.TryGetValue("Number Plays", out Plays))
                        {
                            PlayCount = long.TryParse(Plays, out PlayCount) ? PlayCount : 0;
                        }
                        long SeasonNumber = 0;
                        string Season = "";
                        if (Field.TryGetValue("Season", out Season))
                        {
                            SeasonNumber = long.TryParse(Season, out SeasonNumber) ? SeasonNumber : 0;
                        }
                        long EpisodeNumber = 0;
                        string EpisodeNo = "";
                        if (Field.TryGetValue("Episode", out EpisodeNo))
                        {
                            EpisodeNumber = long.TryParse(EpisodeNo, out EpisodeNumber) ? EpisodeNumber : 0;
                        }
                        string FileKey = "";
                        string Thumb2 = "";
                        string Fanart2 = "";
                        if (Field.TryGetValue("Key", out FileKey))
                        {
                            Thumb2 = "http://" + _parent.IP + ":" + _parent.Port + "/MCWS/v1/File/GetImage?File=" + FileKey + "&FileType=Key&Type=Thumbnail&Format=jpg&Token=" + _parent.JRiverAuthToken ?? "";
                            Fanart2 = "http://" + _parent.IP + ":" + _parent.Port + "/MCWS/v1/File/GetImage?File=" + FileKey + "&FileType=Key&Type=Full&Format=jpg&Token=" + _parent.JRiverAuthToken ?? "";

                        }

                    DateTime myDateTime = new DateTime();
                    try
                    {
                        string datestring = "01/01/1900";
                        
                        if (Field.TryGetValue("Date Created", out datestring))
                        {
                            myDateTime = DateTime.Parse(datestring);
                        }
                    }
                    catch (Exception ex)
                    {
                       // _parent.Trace("JRiver Caught Error:  dateTime:Exception: " + ex);
                        myDateTime = DateTime.Parse("1900-01-01");
                    }

                        string sqlFormattedDate = myDateTime.ToString("s");

                    //      List<string> MovieIcons = new List<string>();

                    // if null equals null-  doesn't make much sense but no harm.  Perhaps change to empty later.
                    // needs to be empty otherwise will fail with null exception down further
                    //

                    //     MovieIcons = GetMovieIcons(Movieitem);
                    int DurationNumber = 0;
                    string Duration = "";
                    if (Field.TryGetValue("Duration", out Duration))
                    {
                        DurationNumber = int.TryParse(Duration, out DurationNumber) ? DurationNumber : 0;
                    }

                    var movie = new ApiMovie
                        {
                            Title = Field.ValueOrDefault("Name"),
                            Plot = Field.ValueOrDefault("Comment"),
                            Votes = "0",
                            Rating = "Unrated",
                            Year = 0,
                            Tagline = Field.ValueOrDefault("Tag Line"),
                            IdScraper = Field.ValueOrDefault("IMDB Id"),
                            Length = new TimeSpan(0, 0, 0, DurationNumber).ToString() ?? "Unknown",
                            Mpaa = Field.ValueOrDefault("MPAA Rating"),
                            Genre = Field.ValueOrDefault("Genre"),
                            Director = Field.ValueOrDefault("Director"),
                            OriginalTitle = Field.ValueOrDefault("Original Title"),
                            Studio = Field.ValueOrDefault("Studio"),
                            IdFile = Xbmc.IDtoNumber(FileKey),
                            IdMovie = Xbmc.IDtoNumber(FileKey),
                            FileName = Field.ValueOrDefault("Filename"),
                            Path = Field.ValueOrDefault("Filename"),
                            PlayCount = PlayCount,
                            Thumb = Thumb2,
                            Banner = "NoBanner",
                            Logo = "NoLogo",
                            Fanart = Fanart2,
                            Hash = Xbmc.Hash(FileKey),
                            DateAdded = sqlFormattedDate,
                            MovieIcons = ""
                        };
                        if (isMovie)
                        {
                            movies.Add(movie);
                        }

                    }
                }
                catch (Exception ex)
                {
                    _parent.Trace("Uncaught Exception with Movie Name :" + ex);


                }
            
            return movies;
    
}


public Collection<ApiTvEpisode> GetTvEpisodesRefresh()
{
            var episodes = new Collection<ApiTvEpisode>();
            return episodes;

            // no refresh possible

            try
            {
                getallItems("/MCWS/v1/Files/Search?Action=mpl&ActiveFile=-1&Zone=-1&ZoneType=ID&Query=[Media%20Sub%20Type]=[TV%20Show]");
                _parent.Trace("GetTVEpisodes : Parent IP: " + _parent.IP);
                // iterate through Items
                foreach (var Field in Allitems)
                {
                    var result = "";
                    var isTVshow = false;
                    if (Field.TryGetValue("Media Sub Type", out result))
                    {
                        if (result == "TV Show")
                        {
                            isTVshow = true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    long PlayCount = 0;
                    string Plays = "";
                    if (Field.TryGetValue("Number Plays", out Plays))
                    {
                        PlayCount = long.TryParse(Plays, out PlayCount) ? PlayCount : 0;
                    }
                    long SeasonNumber = 0;
                    string Season = "";
                    if (Field.TryGetValue("Season", out Season))
                    {
                        SeasonNumber = long.TryParse(Season, out SeasonNumber) ? SeasonNumber : 0;
                    }
                    long EpisodeNumber = 0;
                    string EpisodeNo = "";
                    if (Field.TryGetValue("Episode", out EpisodeNo))
                    {
                        EpisodeNumber = long.TryParse(EpisodeNo, out EpisodeNumber) ? EpisodeNumber : 0;
                    }
                    string FileKey = "";
                    string Thumb = "";
                    string Fanart = "";
                    if (Field.TryGetValue("Key", out FileKey))
                    {
                        Thumb = "http://" + _parent.IP + ":" + _parent.Port + "/MCWS/v1/File/GetImage?File=" + FileKey + "&FileType=Key&Type=Thumbnail&Format=jpg&Token=" + _parent.JRiverAuthToken ?? "";
                        Fanart = "http://" + _parent.IP + ":" + _parent.Port + "/MCWS/v1/File/GetImage?File=" + FileKey + "&FileType=Key&Type=Full&Format=jpg&Token=" + _parent.JRiverAuthToken ?? "";

                    }
                    DateTime myDateTime = new DateTime();
                    try
                    {
                        string datestring = "01/01/1900";

                        if (Field.TryGetValue("Date Created", out datestring))
                        {
                            myDateTime = DateTime.Parse(datestring);
                        }
                    }
                    catch (Exception ex)
                    {
                       // _parent.Trace("JRiver Error:  dateTime Exception: " + ex);
                        myDateTime = DateTime.Parse("1900-01-01");
                    }

                    string sqlFormattedDate = myDateTime.ToString("s");
                    string tvdb = "0";
                    if (Field.TryGetValue("TheTVDB Series ID", out tvdb))
                    {
                        tvdb = tvdb;
                    }


                    var tvShow = new ApiTvEpisode
                    {
                        Title = Field.ValueOrDefault("Name"),
                        Plot = Field.ValueOrDefault("Comment"),
                        Rating = "",
                        Mpaa = Field.ValueOrDefault("MPAA Rating"),
                        Date = sqlFormattedDate,
                        Director = Field.ValueOrDefault("Director"),
                        PlayCount = PlayCount,
                        Studio = Field.ValueOrDefault("Studio"),
                        IdEpisode = Xbmc.IDtoNumber(FileKey),
                        IdShow = Xbmc.IDtoNumber(tvdb),
                        Season = SeasonNumber,
                        Episode = EpisodeNumber,
                        Path = Field.ValueOrDefault("Filename"),
                        ShowTitle = Field.ValueOrDefault("Series"),
                        Thumb = Thumb,
                        Fanart = Fanart,
                        Hash = Xbmc.Hash(FileKey)
                    };
                    if (isTVshow)
                    {
                        episodes.Add(tvShow);
                    }
                }
            }
            catch (Exception Ex)
            {
                _parent.Trace("Another tV Episodes exception" + Ex);
            }

            return episodes;
        }

public Collection<ApiMovie> GetMovies()
{
            var movies = new Collection<ApiMovie>();

            //testing only

            return movies;

            // testing okay
            // delete

            try
            {
                getallItems("/MCWS/v1/Files/Search?Action=mpl&ActiveFile=-1&Zone=-1&ZoneType=ID&Query=[Media%20Sub%20Type]=[Movie]");
                var isMovie = false;
                _parent.Trace("GetMoviesRefresh  : Parent IP: " + _parent.IP);
                // iterate through Items
                foreach (var Field in Allitems)
                {

                    var Logoutput = "";
                    Field.TryGetValue("Name", out Logoutput);
                    _parent.Trace("************* Logging : " + Logoutput);

                    var result = "";
                    if (Field.TryGetValue("Media Sub Type", out result))
                    {
                        if (result == "Movie")
                        {
                            isMovie = true;
                            _parent.Trace("Movie:  " + isMovie);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    long PlayCount = 0;
                    string Plays = "";
                    if (Field.TryGetValue("Number Plays", out Plays))
                    {
                        PlayCount = long.TryParse(Plays, out PlayCount) ? PlayCount : 0;
                    }
                    long SeasonNumber = 0;
                    string Season = "";
                    if (Field.TryGetValue("Season", out Season))
                    {
                        SeasonNumber = long.TryParse(Season, out SeasonNumber) ? SeasonNumber : 0;
                    }
                    long EpisodeNumber = 0;
                    string EpisodeNo = "";
                    if (Field.TryGetValue("Episode", out EpisodeNo))
                    {
                        EpisodeNumber = long.TryParse(EpisodeNo, out EpisodeNumber) ? EpisodeNumber : 0;
                    }
                    string FileKey = "";
                    string Thumb2 = "";
                    string Fanart2 = "";
                    string LogoArt = "";
                    string BannerArt = "";
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
                        var LogoPath = Path.Combine(filePath, "logo.png");
                        var ThumbPath = Path.Combine(filePath, "poster.jpg");
                        var BannerPath = Path.Combine(filePath, "banner.jpg");
                        System.IO.FileInfo fi = new System.IO.FileInfo(fanartPath);
                        System.IO.FileInfo fiLogo = new System.IO.FileInfo(LogoPath);
                        System.IO.FileInfo fiThumb = new System.IO.FileInfo(ThumbPath);
                        System.IO.FileInfo fiBanner = new System.IO.FileInfo(BannerPath);

                        _parent.Trace("JRiver: ** filePath ** :" + filePath);
                        _parent.Trace("JRiver: ** fanArt.Jpg ** :" + fanartPath);
                        _parent.Trace("JRiver: ** Logo.Png ** :" + LogoPath);

                        if (fi.Exists)
                        {
                            Fanart2 = fanartPath;  //if fanart.jpg exisits in directory with movie use this otherwise default to JRiver Thumb
                        }

                        if (fiLogo.Exists)
                        {
                            LogoArt = LogoPath;
                        }

                        if (fiThumb.Exists)
                        {
                            Thumb2 = ThumbPath;
                        }
                        if (fiBanner.Exists)
                        {
                            BannerArt = BannerPath;
                        }

                    }
                    //** uses directories for fanart if present


                    DateTime myDateTime = new DateTime();
                    try
                    {
                        string datestring = "01/01/1900";

                        if (Field.TryGetValue("Date Created", out datestring))
                        {
                            myDateTime = DateTime.Parse(datestring);
                        }
                    }
                    catch (Exception ex)
                    {
                       // _parent.Trace("JRiver Error:  dateTime Exception: " + ex);
                        myDateTime = DateTime.Parse("1900-01-01");
                    }

                    string sqlFormattedDate = myDateTime.ToString("s");

                    //      List<string> MovieIcons = new List<string>();

                    // if null equals null-  doesn't make much sense but no harm.  Perhaps change to empty later.
                    // needs to be empty otherwise will fail with null exception down further
                    //

                    //     MovieIcons = GetMovieIcons(Movieitem);
                    int DurationNumber = 0;
                    string Duration = "";
                    if (Field.TryGetValue("Duration", out Duration))
                    {
                        DurationNumber = int.TryParse(Duration, out DurationNumber) ? DurationNumber : 0;
                    }

                    var movie = new ApiMovie
                    {
                        Title = Field.ValueOrDefault("Name"),
                        Plot = Field.ValueOrDefault("Comment"),
                        Votes = "0",
                        Rating = "Unrated",
                        Year = 0,
                        Tagline = Field.ValueOrDefault("Tag Line"),
                        IdScraper = Field.ValueOrDefault("IMDB Id"),
                        Length = new TimeSpan(0, 0, 0, DurationNumber).ToString() ?? "Unknown",
                        Mpaa = Field.ValueOrDefault("MPAA Rating"),
                        Genre = Field.ValueOrDefault("Genre"),
                        Director = Field.ValueOrDefault("Director"),
                        OriginalTitle = Field.ValueOrDefault("Original Title"),
                        Studio = Field.ValueOrDefault("Studio"),
                        IdFile = Xbmc.IDtoNumber(FileKey),
                        IdMovie = Xbmc.IDtoNumber(FileKey),
                        FileName = Field.ValueOrDefault("Filename"),
                        Path = Field.ValueOrDefault("Filename"),
                        PlayCount = PlayCount,
                        Thumb = Thumb2,
                        Banner = BannerArt,
                        Logo = LogoArt,
                        Fanart = Fanart2,
                        Hash = Xbmc.Hash(FileKey),
                        DateAdded = sqlFormattedDate,
                        MovieIcons = ""
                    };
                    if (isMovie)
                    {
                        movies.Add(movie);
                        _parent.Trace(movies.ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                _parent.Trace("Exception with Movie Name :" + ex);

            }

            return movies;
        }
    }}
