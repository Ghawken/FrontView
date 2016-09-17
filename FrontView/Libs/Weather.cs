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
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using System.Xml.XPath;
using Setup;
using System.Web;
using Timer = System.Timers.Timer;

namespace FrontView.Libs
{
    public class WeatherLocation
    {
        public string LocId { get; set; }
        public string Name { get; set; }
        public string LocType { get; set; }

        public override string ToString()
        {
            return Name + " (" + LocId + ")";
        }
    }
    public class WeatherComSearch
    {

        public class Rootobject
        {
            public RESULT[] RESULTS { get; set; }
        }

        public class RESULT
        {
            public string name { get; set; }
            public string type { get; set; }
            public string c { get; set; }
            public string zmw { get; set; }
            public string tz { get; set; }
            public string tzs { get; set; }
            public string l { get; set; }
            public string ll { get; set; }
            public string lat { get; set; }
            public string lon { get; set; }
        }
    }

    public class WeatherLink
    {
        public string Name { get; set; }
        public Uri Url { get; set; }
    }

    public class WeatherCurrentDetail
    {
        public string Temperature { get; set; }
        public string Icon { get; set; }
    }

    public class WeatherForecastDetail
    {
        public int DayDiff { get; set; }
        public string DayName { get; set; }
        public string DayDate { get; set; }
        public string MaxTemp { get; set; }
        public string LowTemp { get; set; }
        public string DayIcon { get; set; }
        public string NightIcon { get; set; }
    }

    public class WeatherData
    {
        public string LocationName { get; set; }
        public string TempUnit { get; set; }
        private readonly Collection<WeatherLink> _promoLinks = new Collection<WeatherLink>();
        public Collection<WeatherLink> PromoLinks
        {
            get { return _promoLinks; }
        }
        public WeatherCurrentDetail Today { get; set; }
        private readonly Collection<WeatherForecastDetail> _forecast = new Collection<WeatherForecastDetail>();
        public Collection<WeatherForecastDetail> Forecast
        {
            get { return _forecast; }
        }
        public string GetTemp(string temp)
        {
            return temp + "°" + TempUnit;
        }
    }

    public class Weather : IDisposable
    {
        private string _partnerId;
        private string _licenseKey;
        private string _unit;
        private string _weatherAPI;
        private string _cacheDir;
        private string _currentLocId;
        private Timer _refreshTimer;
        private bool _isAutoRefresh;
        private const double CacheDuration = 30;
        private const double ForecastCacheDuration = 210;  //appears to be in minutes
        private const string SearchUrl = "http://autocomplete.wunderground.com/aq?query=";
        //private const string DataUrl = "http://xoap.weather.com/weather/local/{0}?cc=*&link=xoap&prod=xoap&par={1}&key={2}&unit={3}";
        //private const string DataUrl = "http://xml.weather.yahoo.com/forecastrss/{0}_{1}.xml";
        private const string DataUrl = "http://api.wunderground.com/api/"; //134031614dc4d6b2/conditions";
        //private const string ForecastDataUrl = "http://xoap.weather.com/weather/local/{0}?cc=*&dayf=5&link=xoap&prod=xoap&par={1}&key={2}&unit={3}";
        //private const string ForecastDataUrl = "http://xml.weather.yahoo.com/forecastrss/{0}_{1}.xml";
        private const string ForecastDataUrl = "http://api.wunderground.com/api/"; //134031614dc4d6b2/forecast10day";
        public void Configure(string cacheDir, string unit, string partnerId, string licenseKey, string weatherAPI)
        {
            _partnerId = partnerId;
            _licenseKey = licenseKey;
            _unit = unit;
            _cacheDir = cacheDir;
            _weatherAPI = weatherAPI;
            _refreshTimer = new Timer { Interval = 5000 };
            _refreshTimer.Elapsed += RefreshTimerTick;
            Logger.Instance().Log("Weather", "Init Here");
        }

        public void Close()
        {
            if (_refreshTimer == null)
                return;
            _refreshTimer.Stop();
            _refreshTimer.Dispose();
        }

        public void AutoResfresh(string locId)
        {
            if (String.IsNullOrEmpty(locId))
                return;
            _currentLocId = locId;

            _isAutoRefresh = true;
            if (_refreshTimer != null)
            {
                _refreshTimer.Interval = 5000;
                _refreshTimer.Start();
            }

        }

        public void StopAutoRefresh()
        {
            _isAutoRefresh = false;
            _refreshTimer.Stop();
        }

        private void RefreshTimerTick(object sender, EventArgs e)
        {
            LoadWeatherData(_currentLocId, false, true, true);
            LoadWeatherData(_currentLocId, false, false, true);
            if (_refreshTimer.Interval == 5000)
                _refreshTimer.Interval = 60000;
        }
/*
        private bool LoadWeatherDataOLD(string locId, bool force, bool forecast, bool auto = false)
        {
            string path;
            string url;
            double cacheDuration;
            if (!forecast)
            {
                path = _cacheDir + "/" + locId + ".xml";
                url = DataUrl;
                cacheDuration = CacheDuration;
            }
            else
            {
                path = _cacheDir + "/" + locId + ".forecast.xml";
                url = ForecastDataUrl;
                cacheDuration = ForecastCacheDuration;
            }
            try
            {
                Logger.Instance().Trace("Weather", "Path:" + String.Format(CultureInfo.InvariantCulture, url, locId, _unit.Replace("m", "c").Replace("s", "f")));
                if (File.Exists(path))
                {
                    var lastWriteTime = File.GetLastWriteTime(path);
                    if (((DateTime.Now - lastWriteTime).TotalMinutes <= cacheDuration) && !force)
                    {
                        return true;
                    }
                }
                if ((auto && _isAutoRefresh) || (!auto && !_isAutoRefresh))
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFile(
                            new Uri(String.Format(CultureInfo.InvariantCulture, url, locId, _unit.Replace("m", "c").Replace("s", "f"))), path);
                    }
                }
                return true;
            }
            catch (WebException) { }
            return false;
        }
        */
        private bool LoadWeatherData(string locId, bool force, bool forecast, bool auto = false)
        {
            string path;
            string url;
            string str;
            double cacheDuration;

            Logger.Instance().Trace("Weather", "Weather API Equals: " + _weatherAPI);

            string newLocID = locId;
            string invalid = @";/:\";        
            foreach (char c in invalid)
            {
                newLocID = newLocID.Replace(c.ToString(), "");
            }


            if (!forecast)
            {
                path = _cacheDir + @"\"+ newLocID+".json";
                url = DataUrl + _weatherAPI + @"/conditions";
                cacheDuration = CacheDuration;
            }
            else
            {
                path = _cacheDir + @"\" + newLocID + ".forecast.json";
                url = ForecastDataUrl+_weatherAPI+@"/forecast10day";
                cacheDuration = ForecastCacheDuration;
            }
            try
            {
                Logger.Instance().Trace("Weather", "Path: " + path);
                
                if (File.Exists(path))
                {
                    var lastWriteTime = File.GetLastWriteTime(path);
                    if (((DateTime.Now - lastWriteTime).TotalMinutes <= cacheDuration) && !force)
                    {
                        Logger.Instance().Trace("Weather", "File Exists equals true, return true: Path: " + path);
                        return true;
                    }
                }
                if ((auto && _isAutoRefresh) || (!auto && !_isAutoRefresh))
                {
                    using (var client = new WebClient())
                    {
                        str = client.DownloadString(url + locId + ".json");

                        Logger.Instance().Trace("Weather:", "Checking before writing file : "+str);
                        
                        if ( str.Contains("keynotfound"))
                        {
                            Logger.Instance().Log("Weather:", "ERORR in Weather API Key Not Found");
                            return false;
                        }
                        if (str.Contains("error"))
                        {
                            Logger.Instance().Log("Weather:", "Unknown ERORR in Weather API File Not Saved");
                            return false;
                        }


                        client.DownloadFile(new Uri(url + locId + ".json"), path);

                        Logger.Instance().Trace("Weather", "Downloaded File Path equals: " + path);
                    }
                }
                return true;
            }
            catch (Exception ex) 
                {
                    Logger.Instance().Trace("Weather:", "Exception: " + ex);
                    return false;
                }
            return false;
        }


        public WeatherData GetWeatherData(string locId)
        {
            return GetWeatherDataNew(locId, false);
        }
/*
        public WeatherData GetWeatherDataOLDOLD(string locId, bool force)
        {
                
            var result = new WeatherData();
            if (String.IsNullOrEmpty(locId))
                return null;
            try
            {
             //   if (!LoadWeatherData(locId, force, false))
              //      return null;
                var data = new XmlDocument();
                data.Load(_cacheDir + "/" + locId + ".xml");

                var ns = new XmlNamespaceManager(data.NameTable);
                ns.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");


                XmlNode node = data.SelectSingleNode("/rss/channel/yweather:units", ns);
                result.TempUnit = node.Attributes["temperature"].InnerText;

                node = data.SelectSingleNode("/rss/channel/yweather:location", ns);
                result.LocationName = node.Attributes["city"].InnerText;

                node = data.SelectSingleNode("/rss/channel/item/yweather:condition", ns);

                result.Today = new WeatherCurrentDetail
                {
                    Temperature = node.Attributes["temp"].InnerText,
                    Icon = node.Attributes["code"].InnerText
                };

                result.PromoLinks.Clear();

                result.Forecast.Clear();
                return result;
            }
            catch (XmlException) { }
            catch (XPathException) { }
            catch (IOException) { }
            catch (NullReferenceException) { }
            return null;
        }
        
 */
        public WeatherData GetWeatherDataNew(string locId, bool force)
        {

            var result = new WeatherData();

            string newLocID = locId;
            string invalid = @";/:\";
            foreach (char c in invalid)
            {
                newLocID = newLocID.Replace(c.ToString(), "");
            }
            string path = _cacheDir + @"\" + newLocID + ".json";

            Logger.Instance().Trace("Weather:", "GetWeatherDataNew Running..");
            Logger.Instance().Trace("Weather:", "Weather API Equals: " + _weatherAPI);
            string str = "";
            if (String.IsNullOrEmpty(locId))
            {
                Logger.Instance().Trace("Weather:", "locID Null returning Running..");
                return null;
            }

            if (!LoadWeatherData(locId, force, false))
            {
                Logger.Instance().Trace("Weather:", "!LoadWeatherData Null returning null.");
                return null;
            }
                try
                {
                    using (StreamReader client = new StreamReader(path))
                    {
                        try
                        {
                            str = client.ReadToEnd();//DataUrl + _weatherAPI + @"/conditions" + locId + ".json");
                            Logger.Instance().Trace("Weather:", "GetWeatherData: Trying DATA FILE " + path);
                            //Logger.Instance().Trace("Weather:", "GetWeatherData: Data " + str);
                        }
                        catch (WebException ex)
                        {
                            Logger.Instance().Trace("Weather:", "No Data File Found:" + ex);
                            return null;
                        }
                    }
                    /*
                    using (var client = new WebClient())
                    {
                        try
                        {
                            str = client.DownloadString(DataUrl+_weatherAPI+@"/conditions" + locId + ".json");
                            Logger.Instance().Trace("Weather:", "GetWeatherData: Trying DataURL " + DataUrl + _weatherAPI + @"/conditions" + locId + ".json");
                            Logger.Instance().Trace("Weather:", "GetWeatherData: Data " +str);
                        }
                        catch (WebException ex)
                        {
                            Logger.Instance().Trace("Weather:", "Underground Error" + ex);
                            return null;
                        }
                    }
                     */
                }
                catch (Exception ex)
                {
                    Logger.Instance().Trace("Weather:", "Underground Error" + ex);
                    return null;
                }


                try
                {
                    string json = str;
                    
                    bool night = false ;

                    if (str.Contains("keynotfound"))
                    {
                        Logger.Instance().Log("Weather:", "ERORR in Weather API Key Not Found");
                        return null;
                    }


                    var deserializer = new JavaScriptSerializer();
                    var server = deserializer.Deserialize<WeatherAPIWUndergroundConditions.Rootobject>(json);
                    result.LocationName = server.current_observation.display_location.full;
                    result.Forecast.Clear();

                    try
                    {
                        Uri uri = new Uri(server.current_observation.icon_url);
                        Logger.Instance().Trace("Weather**", "Getting icon url to find night or not:" + uri.AbsolutePath);
                        string filename = Path.GetFileName(uri.AbsolutePath);
                        Logger.Instance().Trace("Weather**", "IS File True: icon url to find night or not:" + filename);
                        if (filename.StartsWith("nt"))
                        {
                            night = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance().Trace("Weather*", "Exception Caught in Icon Night or Day test" + ex);
                    }


                    if (_unit == "c")
                    {
                        result.TempUnit = "celsius";
                        
                        result.Today = new WeatherCurrentDetail
                            {
                                Temperature = server.current_observation.temp_c.ToString(),
                                Icon = night == true ? "nt_" + server.current_observation.icon : server.current_observation.icon 
                            };
                        return result;
                    }
                    else if (_unit == "f")
                    {
                        result.TempUnit = @"F";
                        result.Today = new WeatherCurrentDetail
                            {
                                Temperature = server.current_observation.temp_f.ToString(),
                                // Tricky - use UV to figure out whether day or night, 
                                Icon = night == true ?  "nt_"+server.current_observation.icon : server.current_observation.icon 
                            };
                        return result;
                    }

                }
                catch (Exception ex)
                {
                    Logger.Instance().Trace("Weather", "Underground Error" + ex);
                    return null;
                }
                return null;



            
        }
            
        public WeatherData GetForecastWeatherData(string locId)
        {
            return GetForecastWeatherDataNew(locId, false);
        }
/*
        public WeatherData GetForecastWeatherDataOLD(string locId, bool force)
        {
            var result = new WeatherData();
            if (String.IsNullOrEmpty(locId))
                return null;
            try
            {
                 if (!LoadWeatherData(locId, force, true))
                    return null;
                var data = new XmlDocument();
                data.Load(_cacheDir + "/" + locId + ".forecast.xml");
                var ns = new XmlNamespaceManager(data.NameTable);
                ns.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");


                XmlNode node = data.SelectSingleNode("/rss/channel/yweather:units", ns);
                result.TempUnit = node.Attributes["temperature"].InnerText;

                node = data.SelectSingleNode("/rss/channel/yweather:location", ns);
                result.LocationName = node.Attributes["city"].InnerText;

                node = data.SelectSingleNode("/rss/channel/item/yweather:condition", ns);

                result.Today = new WeatherCurrentDetail
                {
                    Temperature = node.Attributes["temp"].InnerText,
                    Icon = node.Attributes["code"].InnerText
                };


                result.Forecast.Clear();

                XmlNodeList nodes = data.SelectNodes("/rss/channel/item/yweather:forecast", ns);
                var diff = 0;
                foreach (XmlNode inode in nodes)
                {

                    var temp = new WeatherForecastDetail
                    {
                        DayDiff = diff,
                        DayName = inode.Attributes["day"].InnerText,
                        DayDate = inode.Attributes["date"].InnerText,
                        DayIcon = inode.Attributes["code"].InnerText,
                        NightIcon = inode.Attributes["code"].InnerText,
                        MaxTemp = inode.Attributes["high"].InnerText,
                        LowTemp = inode.Attributes["low"].InnerText
                    };
                    result.Forecast.Add(temp);
                    diff++;
                }
                return result;
            }
            catch (XmlException) { }
            catch (XPathException) { }
            catch (IOException) { }
            catch (NullReferenceException) { }
            return null;
        }
 */
        public WeatherData GetForecastWeatherDataNew(string locId, bool force)
        {

            bool night = false;
            var result = new WeatherData();
            Logger.Instance().Trace("Weather:", "GetForecastWeatherDataNew");
            Logger.Instance().Trace("Weather:", "Weather API Equals: " + _weatherAPI);
            string str = "";

            string newLocID = locId;
            string invalid = @";/:\";
            foreach (char c in invalid)
            {
                newLocID = newLocID.Replace(c.ToString(), "");
            }
            string path = _cacheDir + @"\" + newLocID + ".forecast.json";
            string path2 = _cacheDir + @"\" + newLocID + ".json";

            if (String.IsNullOrEmpty(locId))
            {
                Logger.Instance().Trace("Weather:", "GetForecastWeatherDataNew:  locID Null or Empty");
                return null;
            }

           // GetWeatherDataNew(locId, false);    //loads conditions --> probably need to move this

            if (!LoadWeatherData(locId, force, true))
            {  
                Logger.Instance().Trace("Weather:", "!LoadWeatherData ForecastWeatherDataNull returning null.");
                return null;
            }
            //First check Conditions as need this to get name amongst other things.

            try
            {
                using (StreamReader client = new StreamReader(path2))
                {
                    try
                    {
                        str = client.ReadToEnd();//DataUrl + _weatherAPI + @"/conditions" + locId + ".json");
                        Logger.Instance().Trace("Weather:", "GetWeatherForecast COnditions Data: Trying DATA FILE " + path2);
                        //Logger.Instance().Trace("Weather:", "GetWeatherData: Data " + str);
                    }
                    catch (WebException ex)
                    {
                        Logger.Instance().Trace("Weather:", "Underground Error" + ex);
                        return null;
                    }
                }
            
            }
            catch (Exception ex)
            {
                    Logger.Instance().Trace("Weather:", "Underground Error" + ex);
                    return null;
            }


            try
            {
                    string json = str;
                    
                    if (str.Contains("keynotfound"))
                    {
                        Logger.Instance().Log("Weather:", "ERORR in Weather API Key Not Found");
                        return null;
                    }


                    var deserializer = new JavaScriptSerializer();
                    var server = deserializer.Deserialize<WeatherAPIWUndergroundConditions.Rootobject>(json);
                    result.LocationName = server.current_observation.display_location.full;
                    result.Forecast.Clear();


                    try
                    {
                        Uri uri = new Uri(server.current_observation.icon_url);
                        Logger.Instance().Trace("Weather**", "Getting icon url to find night or not:" + uri.AbsolutePath);
                        string filename = Path.GetFileName(uri.AbsolutePath);
                        Logger.Instance().Trace("Weather**", "IS File True: icon url to find night or not:" + filename);
                        if (filename.StartsWith("nt"))
                        {
                            night = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance().Trace("Weather*", "Exception Caught in Icon Night or Day test" + ex);
                    }

                    if (_unit == "c")
                    {
                        result.TempUnit = "celsius";
                        result.Today = new WeatherCurrentDetail
                            {
                                Temperature = server.current_observation.temp_c.ToString(),
                                Icon = night == true ? "nt_" + server.current_observation.icon : server.current_observation.icon
                            };
                    }

                    else if (_unit == "f")
                    {
                        result.TempUnit = @"F";
                        result.Today = new WeatherCurrentDetail
                            {
                                Temperature = server.current_observation.temp_f.ToString(),
                                Icon = night == true ? "nt_" + server.current_observation.icon : server.current_observation.icon
                            };
                    }

                }
                catch (Exception ex)
                {
                    Logger.Instance().Trace("Weather", "Underground Error" + ex);
                    return null;
                }
            
            // Now downloading Forecast stuff...


            try
            {
                using (StreamReader client = new StreamReader(path))
                {
                    try
                    {
                        str = client.ReadToEnd();
                        Logger.Instance().Trace("Weather:", "GetForecastWeather Data Trying Data FILE: " + path);
                        //Logger.Instance().Trace("Weather:", "Result is :" + str);
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance().Trace("Weather:", "Underground Error" + ex);
                        return null;
                    }
                }
            } 
            catch (Exception ex)
            {
                Logger.Instance().Trace("Weather", "Underground Error" + ex);
            }


            try
            {
                string json = str;


                if (str.Contains("keynotfound"))
                {
                    Logger.Instance().Log("Weather:", "ERORR in Weather API Key Not Found");
                    return null;
                }

                var deserializer = new JavaScriptSerializer();
                var server = deserializer.Deserialize<WeatherAPIWUnderground10day.Rootobject>(json);

                result.Forecast.Clear();

                var diff = 0;

                if (server != null)
                {
                    foreach (var element in server.forecast.simpleforecast.forecastday)
                    {

                        var temp = new WeatherForecastDetail
                        {
                            DayDiff = diff,
                            DayName = element.date.weekday,
                            DayDate = element.date.pretty,
                            DayIcon = night == true ? "nt_" + element.icon : element.icon,
                            NightIcon = "nt_"+element.icon,
                            MaxTemp = (_unit == "c") ? element.high.celsius : element.high.fahrenheit,
                            LowTemp = (_unit == "c") ? element.low.celsius : element.low.fahrenheit,

                        };
                        result.Forecast.Add(temp);
                        diff++;
                    }
                }
                return result;


            }
            catch (Exception ex)
            {
                Logger.Instance().Trace("Weather", "Underground Error" + ex);
                return null;
            }
        }
        

        public static Collection<WeatherLocation> SearchCity(string city)
        {
            var result = new Collection<WeatherLocation>();
            string str;

            using (var client = new WebClient())
            {
                try
                {
                    str = client.DownloadString(SearchUrl + city);
                }
                catch (WebException)
                {
                    return result;
                }

            }
            try
            {
                //var data = new XmlDocument();
                //data.LoadXml(str);
                string json = str;
                var deserializer = new JavaScriptSerializer();
                var server = deserializer.Deserialize<WeatherComSearch.Rootobject>(json);


                //var elements = data.SelectNodes("//RESULTS");
                
                if (server != null)
                    foreach (var element in server.RESULTS)
                    {

                       // string tempID = element.l.Replace("/q/locid:", "");
                       // tempID = tempID.Substring(0, tempID.IndexOf(";")+1);
                        
                        
                        var temp = new WeatherLocation
                                       {
                                           LocId = element.l,
                                           LocType = element.type,
                                           Name = element.name
                                       };
                        result.Add(temp);
                    }
            }
            catch (XmlException) { }
            catch (XPathException) { }
            
            
            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Close();
            }
        }
    }
}
