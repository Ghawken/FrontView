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
using System.Xml.XPath;
using Setup;
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
            return Name +  " (" + LocId + ")";
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
        public  Collection<WeatherLink> PromoLinks
        {
            get { return _promoLinks; }
        }
        public WeatherCurrentDetail Today { get; set; }
        private readonly Collection<WeatherForecastDetail> _forecast= new Collection<WeatherForecastDetail>();
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
        private string _cacheDir;
        private string _currentLocId;
        private Timer _refreshTimer;
        private bool _isAutoRefresh;
        private const double CacheDuration = 30;
        private const double ForecastCacheDuration = 210;
        private const string SearchUrl = "http://xoap.weather.com/search/search?where=";
        //private const string DataUrl = "http://xoap.weather.com/weather/local/{0}?cc=*&link=xoap&prod=xoap&par={1}&key={2}&unit={3}";
        private const string DataUrl = "http://xml.weather.yahoo.com/forecastrss/{0}_{1}.xml";
        //private const string ForecastDataUrl = "http://xoap.weather.com/weather/local/{0}?cc=*&dayf=5&link=xoap&prod=xoap&par={1}&key={2}&unit={3}";
        private const string ForecastDataUrl = "http://xml.weather.yahoo.com/forecastrss/{0}_{1}.xml";
        
        public void Configure(string cacheDir, string unit, string partnerId, string licenseKey)
        {
            _partnerId = partnerId;
            _licenseKey = licenseKey;
            _unit = unit;
            _cacheDir = cacheDir;
            _refreshTimer = new Timer { Interval = 1000 };
            _refreshTimer.Elapsed += RefreshTimerTick;
            Logger.Instance().Log("Weather", "Init");
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
                _refreshTimer.Interval = 1000;
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
            LoadWeatherData(_currentLocId, false, false , true);
            if (_refreshTimer.Interval == 1000)
                _refreshTimer.Interval = 60000;
        }

        private bool LoadWeatherData(string locId, bool force, bool forecast, bool auto = false)
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
                            new Uri(String.Format(CultureInfo.InvariantCulture, url, locId, _unit.Replace("m","c").Replace("s","f"))), path);
                    }
                } 
                return true;
            }
            catch (WebException)  { }
            return false;
        }

        public WeatherData GetWeatherData(string locId)
        {
            return GetWeatherData(locId, false);
        }

        public WeatherData GetWeatherData(string locId, bool force)
        {
            var result = new WeatherData();
            if (String.IsNullOrEmpty(locId))
                return null;
            try
            {
                if (!LoadWeatherData(locId,force,false))
                    return null;
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

        public WeatherData GetForecastWeatherData(string locId)
        {
            return GetForecastWeatherData(locId, false);
        }

        public WeatherData GetForecastWeatherData(string locId, bool force)
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
                var data = new XmlDocument();
                data.LoadXml(str);
                var elements = data.SelectNodes("//search/loc");
                if (elements != null)
                    foreach (XmlElement element in elements)
                    {
                        var temp = new WeatherLocation
                                       {
                                           LocId = element.GetAttribute("id"),
                                           LocType = element.GetAttribute("type"),
                                           Name = element.InnerText
                                       };
                        result.Add(temp);
                    }
            }
            catch (XmlException) { }
            catch (XPathException ) { }
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
