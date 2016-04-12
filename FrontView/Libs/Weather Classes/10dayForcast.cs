using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontView.Libs.WeatherAPIWUnderground10day
{


    public class Rootobject
    {
        public Response response { get; set; }
        public Forecast forecast { get; set; }
    }

    public class Response
    {
        public string version { get; set; }
        public string termsofService { get; set; }
        public Features features { get; set; }
    }

    public class Features
    {
        public int forecast10day { get; set; }
    }

    public class Forecast
    {
        public Txt_Forecast txt_forecast { get; set; }
        public Simpleforecast simpleforecast { get; set; }
    }

    public class Txt_Forecast
    {
        public string date { get; set; }
        public Forecastday[] forecastday { get; set; }
    }

    public class Forecastday
    {
        public int period { get; set; }
        public string icon { get; set; }
        public string icon_url { get; set; }
        public string title { get; set; }
        public string fcttext { get; set; }
        public string fcttext_metric { get; set; }
        public string pop { get; set; }
    }

    public class Simpleforecast
    {
        public Forecastday1[] forecastday { get; set; }
    }

    public class Forecastday1
    {
        public Date date { get; set; }
        public int period { get; set; }
        public High high { get; set; }
        public Low low { get; set; }
        public string conditions { get; set; }
        public string icon { get; set; }
        public string icon_url { get; set; }
        public string skyicon { get; set; }
        public int pop { get; set; }
        public Qpf_Allday qpf_allday { get; set; }
        public Qpf_Day qpf_day { get; set; }
        public Qpf_Night qpf_night { get; set; }
        public Snow_Allday snow_allday { get; set; }
        public Snow_Day snow_day { get; set; }
        public Snow_Night snow_night { get; set; }
        public Maxwind maxwind { get; set; }
        public Avewind avewind { get; set; }
        public int avehumidity { get; set; }
        public int maxhumidity { get; set; }
        public int minhumidity { get; set; }
    }

    public class Date
    {
        public string epoch { get; set; }
        public string pretty { get; set; }
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public int yday { get; set; }
        public int hour { get; set; }
        public string min { get; set; }
        public int sec { get; set; }
        public string isdst { get; set; }
        public string monthname { get; set; }
        public string monthname_short { get; set; }
        public string weekday_short { get; set; }
        public string weekday { get; set; }
        public string ampm { get; set; }
        public string tz_short { get; set; }
        public string tz_long { get; set; }
    }

    public class High
    {
        public string fahrenheit { get; set; }
        public string celsius { get; set; }
    }

    public class Low
    {
        public string fahrenheit { get; set; }
        public string celsius { get; set; }
    }

    public class Qpf_Allday
    {
        public float _in { get; set; }
        public int mm { get; set; }
    }

    public class Qpf_Day
    {
        public float _in { get; set; }
        public int mm { get; set; }
    }

    public class Qpf_Night
    {
        public float _in { get; set; }
        public int mm { get; set; }
    }

    public class Snow_Allday
    {
        public float _in { get; set; }
        public float cm { get; set; }
    }

    public class Snow_Day
    {
        public float _in { get; set; }
        public float cm { get; set; }
    }

    public class Snow_Night
    {
        public float _in { get; set; }
        public float cm { get; set; }
    }

    public class Maxwind
    {
        public int mph { get; set; }
        public int kph { get; set; }
        public string dir { get; set; }
        public int degrees { get; set; }
    }

    public class Avewind
    {
        public int mph { get; set; }
        public int kph { get; set; }
        public string dir { get; set; }
        public int degrees { get; set; }
    }


}
