using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontView.Libs.DarkSkyAPI
{

    public class Rootobject
    {
        public float latitude { get; set; }
        public float longitude { get; set; }
        public string timezone { get; set; }
        public Currently currently { get; set; }
        public Daily daily { get; set; }
        public Alert[] alerts { get; set; }
        public Flags flags { get; set; }
        public int offset { get; set; }
    }

    public class Currently
    {
        public float time { get; set; }
        public string summary { get; set; }
        public string icon { get; set; }
        public decimal nearestStormDistance { get; set; }
        public decimal nearestStormBearing { get; set; }
        public decimal precipIntensity { get; set; }
        public decimal precipProbability { get; set; }
        public float temperature { get; set; }
        public float apparentTemperature { get; set; }
        public float dewPoint { get; set; }
        public float humidity { get; set; }
        public float pressure { get; set; }
        public float windSpeed { get; set; }
        public float windGust { get; set; }
        public int windBearing { get; set; }
        public float cloudCover { get; set; }
        public int uvIndex { get; set; }
        public decimal visibility { get; set; }
        public float ozone { get; set; }
    }

    public class Daily
    {
        public string summary { get; set; }
        public string icon { get; set; }
        public Datum[] data { get; set; }
    }

    public class Datum
    {
        public int time { get; set; }
        public string summary { get; set; }
        public string icon { get; set; }
        public int sunriseTime { get; set; }
        public int sunsetTime { get; set; }
        public float moonPhase { get; set; }
        public float precipIntensity { get; set; }
        public float precipIntensityMax { get; set; }
        public float precipIntensityMaxTime { get; set; }
        public float precipProbability { get; set; }
        public string precipType { get; set; }
        public float temperatureHigh { get; set; }
        public float temperatureHighTime { get; set; }
        public float temperatureLow { get; set; }
        public float temperatureLowTime { get; set; }
        public float apparentTemperatureHigh { get; set; }
        public float apparentTemperatureHighTime { get; set; }
        public float apparentTemperatureLow { get; set; }
        public int apparentTemperatureLowTime { get; set; }
        public float dewPoint { get; set; }
        public float humidity { get; set; }
        public float pressure { get; set; }
        public float windSpeed { get; set; }
        public float windGust { get; set; }
        public int windGustTime { get; set; }
        public int windBearing { get; set; }
        public float cloudCover { get; set; }
        public int uvIndex { get; set; }
        public int uvIndexTime { get; set; }
        public float visibility { get; set; }
        public float ozone { get; set; }
        public float temperatureMin { get; set; }
        public float temperatureMinTime { get; set; }
        public float temperatureMax { get; set; }
        public float temperatureMaxTime { get; set; }
        public float apparentTemperatureMin { get; set; }
        public float apparentTemperatureMinTime { get; set; }
        public float apparentTemperatureMax { get; set; }
        public float apparentTemperatureMaxTime { get; set; }
    }

    public class Flags
    {
        public string[] sources { get; set; }
        public float neareststation { get; set; }
        public string units { get; set; }
    }

    public class Alert
    {
        public string title { get; set; }
        public string[] regions { get; set; }
        public string severity { get; set; }
        public int time { get; set; }
        public int expires { get; set; }
        public string description { get; set; }
        public string uri { get; set; }
    }

}