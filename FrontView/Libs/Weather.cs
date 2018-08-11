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
using System.Collections.Generic;
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
    public static class StringExt
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }


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

            return temp.Truncate(4) + "°" ;
        }
    }

    public class Weather : IDisposable
    {
        private string _partnerId;
        private string _licenseKey;
        private string _unit;
        private string _longitude;
        private string _latitude;
        private string _weatherAPI;
        private string _cacheDir;
        private string _currentLocId;
        private string _LocationName;
        private Timer _refreshTimer;
        private bool _isAutoRefresh;
        private const double CacheDuration = 30;
        private const double ForecastCacheDuration = 210;  //appears to be in minutes
        private const string SearchUrl = "http://autocomplete.wunderground.com/aq?query=";
        //private const string DataUrl = "http://xoap.weather.com/weather/local/{0}?cc=*&link=xoap&prod=xoap&par={1}&key={2}&unit={3}";
        //private const string DataUrl = "http://xml.weather.yahoo.com/forecastrss/{0}_{1}.xml";

        //private const string DataUrl = "http://api.wunderground.com/api/"; //134031614dc4d6b2/conditions";

        private const string DataUrl = "https://api.darksky.net/forecast/";
        //private const string ForecastDataUrl = "http://xoap.weather.com/weather/local/{0}?cc=*&dayf=5&link=xoap&prod=xoap&par={1}&key={2}&unit={3}";
        //private const string ForecastDataUrl = "http://xml.weather.yahoo.com/forecastrss/{0}_{1}.xml";

        private const string ForecastDataUrl = "https://api.darksky.net/forecast/"; //134031614dc4d6b2/forecast10day";

        public void Configure(string cacheDir, string unit, string partnerId, string licenseKey, string weatherAPI,string latitude, string longitude, string LocationName)
        {
            _partnerId = partnerId;
            _licenseKey = licenseKey;
            _unit = unit;
            _longitude = longitude;
            _latitude = latitude;
            _cacheDir = cacheDir;
            _LocationName = LocationName;
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

            path = _cacheDir + @"\" + newLocID + ".json";

            Logger.Instance().Trace("Weather", "Weather Longitude Equals: " + _longitude);
            Logger.Instance().Trace("Weather", "Weather latitude Equals: " + _latitude);
            Logger.Instance().Trace("Weather", "Weather Unit Equals: " + _unit);


            url = DataUrl + _weatherAPI + @"/"+_latitude+ @"," + _longitude + @"?exclude=minutely,hourly"+@"&units="+_unit;
            cacheDuration = CacheDuration;

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
                        Logger.Instance().Trace("Weather:", "Url Equals:-----------" + url);

                        str = client.DownloadString(url);

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


                        client.DownloadFile(new Uri(url ), path);

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
                            Logger.Instance().Trace("Weather:", "No Data File Exists: Exception: "+ex);
                            return null;
                        }
                    }

                }
                catch (Exception ex)
                {
                    Logger.Instance().Trace("Weather:", "DarkSky Error" + ex);
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
                    var server = deserializer.Deserialize<DarkSkyAPI.Rootobject>(json);

                    result.LocationName = _LocationName;
                    
                    result.Forecast.Clear();

                    try
                    {
                        
                        Logger.Instance().Trace("Weather**", "Getting icon url to find night or not:" + server.currently.icon);
                        Logger.Instance().Trace("Weather**", "IS File True: icon url to find night or not:" + server.currently.icon);
                        if (server.currently.icon.EndsWith("night"))
                        {
                            night = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance().Trace("Weather*", "Exception Caught in Icon Night or Day test" + ex);
                    }

                        result.TempUnit = "°";

                result.Today = new WeatherCurrentDetail
                {
                        Temperature = server.currently.temperature.ToString(),
                        Icon = convertIcons(server.currently.icon)
                };
                return result;
                    


                }
                catch (Exception ex)
                {
                    Logger.Instance().Trace("Weather", "DarkSky Error Caught:" + ex);
                    return null;
                }
                return null;



            
        }
            
        public WeatherData GetForecastWeatherData(string locId)
        {
            return GetForecastWeatherDataNew(locId, false);
        }


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
            string path = _cacheDir + @"\" + newLocID + ".json";
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
                    var server = deserializer.Deserialize<DarkSkyAPI.Rootobject>(json);
                    result.LocationName = server.timezone.ToString();
                    result.Forecast.Clear();


                try
                {

                    Logger.Instance().Trace("Weather**", "Getting icon url to find night or not:" + server.currently.icon);
                    Logger.Instance().Trace("Weather**", "IS File True: icon url to find night or not:" + server.currently.icon);
                    if (server.currently.icon.EndsWith("night"))
                    {
                        night = true;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Instance().Trace("Weather*", "Exception Caught in Icon Night or Day test" + ex);
                }

                    result.TempUnit = "°";
                        result.Today = new WeatherCurrentDetail
                            {
                                Temperature = server.currently.temperature.ToString(),
                                Icon = convertIcons(server.currently.icon)
                        };


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
                var server = deserializer.Deserialize<DarkSkyAPI.Rootobject>(json);

                result.Forecast.Clear();

                var diff = 0;

                if (server != null)
                {
                    foreach (var element in server.daily.data)
                    {

                        var currentdatetime = UnixTimeStampToDateTime(element.time);
;
                        var temp = new WeatherForecastDetail
                        {
                            DayDiff = diff,
                            DayName = currentdatetime.DayOfWeek.ToString(),
                            DayDate =currentdatetime.Date.ToString(),
                            DayIcon = convertIcons(element.icon),
                            NightIcon = convertIcons(element.icon),
                            MaxTemp = element.temperatureHigh.ToString(),
                            LowTemp =  element.temperatureLow.ToString() ,

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

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static string convertIcons(string icon)
        {

            Logger.Instance().Trace("Weather", " Converting Icons:" + icon.ToString());
            var night = false;
            var day = false;

            if (icon.EndsWith("night"))
            {
                night = true;
            }
            else if (icon.EndsWith("day"))
            {
                day = true;
            }

            icon = Switchicons(icon);

            if (night==true)
            {
                icon = "nt_" + icon;
            }

            Logger.Instance().Trace("Weather", " converted Icon=" + icon.ToString());

            return icon;

        }

        public static string Switchicons(string icon)
        {
            switch (icon)
            {
                case "clear": return "clear";
                case "partly-cloudy-day": return "partlycloudy";
                case "partly-cloudy-night": return "partlycloudy";
                case "clear-day": return "clear";
                case "clear-night": return "clear";
                case "cloudy": return "cloudy";
                case "rain": return "rain";
                case "sleet": return "sleet";
                case "snow": return "snow";
                case "wind": return "wind";
                case "fog": return "fog";
            }
            return "na";

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
