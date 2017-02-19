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
using System.Globalization;
using System.Windows;
using System.IO;
using FrontView.Libs;

namespace FrontView.Classes
{
  public class Yatse2Weather : DependencyObject
  {
    public static readonly DependencyProperty LocationProperty =
        DependencyProperty.Register("Location", typeof(string), typeof(Yatse2Weather));

    public string Location
    {
      get { return (string)GetValue(LocationProperty); }
      set { SetValue(LocationProperty, value); }
    }

    public static readonly DependencyProperty CurrentIconProperty =
        DependencyProperty.Register("CurrentIcon", typeof(string), typeof(Yatse2Weather));

    public string CurrentIcon
    {
      get { return (string)GetValue(CurrentIconProperty); }
      set { SetValue(CurrentIconProperty, value); }
    }

    public static readonly DependencyProperty CurrentBackgroundProperty =
        DependencyProperty.Register("CurrentBackground", typeof(string), typeof(Yatse2Weather));

    public string CurrentBackground
    {
      get { return (string)GetValue(CurrentBackgroundProperty); }
      set { SetValue(CurrentBackgroundProperty, value); }
    }

    public static readonly DependencyProperty CurrentTempProperty =
        DependencyProperty.Register("CurrentTemp", typeof(string), typeof(Yatse2Weather));

    public string CurrentTemp
    {
      get { return (string)GetValue(CurrentTempProperty); }
      set { SetValue(CurrentTempProperty, value); }
    }

    public static readonly DependencyProperty Day0IconDayProperty =
        DependencyProperty.Register("Day0IconDay", typeof(string), typeof(Yatse2Weather));

    public string Day0IconDay
    {
      get { return (string)GetValue(Day0IconDayProperty); }
      set { SetValue(Day0IconDayProperty, value); }
    }
    public static readonly DependencyProperty Day0IconNightProperty =
        DependencyProperty.Register("Day0IconNight", typeof(string), typeof(Yatse2Weather));

    public string Day0IconNight
    {
      get { return (string)GetValue(Day0IconNightProperty); }
      set { SetValue(Day0IconNightProperty, value); }
    }

    public static readonly DependencyProperty Day0NameProperty =
        DependencyProperty.Register("Day0Name", typeof(string), typeof(Yatse2Weather));

    public string Day0Name
    {
      get { return (string)GetValue(Day0NameProperty); }
      set { SetValue(Day0NameProperty, value); }
    }

    public static readonly DependencyProperty Day0MaxTempProperty =
        DependencyProperty.Register("Day0MaxTemp", typeof(string), typeof(Yatse2Weather));

    public string Day0MaxTemp
    {
      get { return (string)GetValue(Day0MaxTempProperty); }
      set { SetValue(Day0MaxTempProperty, value); }
    }

    public static readonly DependencyProperty Day0MinTempProperty =
        DependencyProperty.Register("Day0MinTemp", typeof(string), typeof(Yatse2Weather));

    public string Day0MinTemp
    {
      get { return (string)GetValue(Day0MinTempProperty); }
      set { SetValue(Day0MinTempProperty, value); }
    }

    public static readonly DependencyProperty Day1IconDayProperty =
        DependencyProperty.Register("Day1IconDay", typeof(string), typeof(Yatse2Weather));

    public string Day1IconDay
    {
      get { return (string)GetValue(Day1IconDayProperty); }
      set { SetValue(Day1IconDayProperty, value); }
    }
    public static readonly DependencyProperty Day1IconNightProperty =
        DependencyProperty.Register("Day1IconNight", typeof(string), typeof(Yatse2Weather));

    public string Day1IconNight
    {
      get { return (string)GetValue(Day1IconNightProperty); }
      set { SetValue(Day1IconNightProperty, value); }
    }

    public static readonly DependencyProperty Day1NameProperty =
        DependencyProperty.Register("Day1Name", typeof(string), typeof(Yatse2Weather));

    public string Day1Name
    {
      get { return (string)GetValue(Day1NameProperty); }
      set { SetValue(Day1NameProperty, value); }
    }

    public static readonly DependencyProperty Day1MaxTempProperty =
        DependencyProperty.Register("Day1MaxTemp", typeof(string), typeof(Yatse2Weather));

    public string Day1MaxTemp
    {
      get { return (string)GetValue(Day1MaxTempProperty); }
      set { SetValue(Day1MaxTempProperty, value); }
    }

    public static readonly DependencyProperty Day1MinTempProperty =
DependencyProperty.Register("Day1MinTemp", typeof(string), typeof(Yatse2Weather));

    public string Day1MinTemp
    {
      get { return (string)GetValue(Day1MinTempProperty); }
      set { SetValue(Day1MinTempProperty, value); }
    }

    public static readonly DependencyProperty Day2IconDayProperty =
        DependencyProperty.Register("Day2IconDay", typeof(string), typeof(Yatse2Weather));

    public string Day2IconDay
    {
      get { return (string)GetValue(Day2IconDayProperty); }
      set { SetValue(Day2IconDayProperty, value); }
    }
    public static readonly DependencyProperty Day2IconNightProperty =
        DependencyProperty.Register("Day2IconNight", typeof(string), typeof(Yatse2Weather));

    public string Day2IconNight
    {
      get { return (string)GetValue(Day2IconNightProperty); }
      set { SetValue(Day2IconNightProperty, value); }
    }

    public static readonly DependencyProperty Day2NameProperty =
        DependencyProperty.Register("Day2Name", typeof(string), typeof(Yatse2Weather));

    public string Day2Name
    {
      get { return (string)GetValue(Day2NameProperty); }
      set { SetValue(Day2NameProperty, value); }
    }

    public static readonly DependencyProperty Day2MaxTempProperty =
        DependencyProperty.Register("Day2MaxTemp", typeof(string), typeof(Yatse2Weather));

    public string Day2MaxTemp
    {
      get { return (string)GetValue(Day2MaxTempProperty); }
      set { SetValue(Day2MaxTempProperty, value); }
    }

    public static readonly DependencyProperty Day2MinTempProperty =
        DependencyProperty.Register("Day2MinTemp", typeof(string), typeof(Yatse2Weather));

    public string Day2MinTemp
    {
      get { return (string)GetValue(Day2MinTempProperty); }
      set { SetValue(Day2MinTempProperty, value); }
    }

    public static readonly DependencyProperty Day3IconDayProperty =
        DependencyProperty.Register("Day3IconDay", typeof(string), typeof(Yatse2Weather));

    public string Day3IconDay
    {
      get { return (string)GetValue(Day3IconDayProperty); }
      set { SetValue(Day3IconDayProperty, value); }
    }
    public static readonly DependencyProperty Day3IconNightProperty =
        DependencyProperty.Register("Day3IconNight", typeof(string), typeof(Yatse2Weather));

    public string Day3IconNight
    {
      get { return (string)GetValue(Day3IconNightProperty); }
      set { SetValue(Day3IconNightProperty, value); }
    }

    public static readonly DependencyProperty Day3NameProperty =
        DependencyProperty.Register("Day3Name", typeof(string), typeof(Yatse2Weather));

    public string Day3Name
    {
      get { return (string)GetValue(Day3NameProperty); }
      set { SetValue(Day3NameProperty, value); }
    }

    public static readonly DependencyProperty Day3MaxTempProperty =
        DependencyProperty.Register("Day3MaxTemp", typeof(string), typeof(Yatse2Weather));

    public string Day3MaxTemp
    {
      get { return (string)GetValue(Day3MaxTempProperty); }
      set { SetValue(Day3MaxTempProperty, value); }
    }

    public static readonly DependencyProperty Day3MinTempProperty =
     DependencyProperty.Register("Day3MinTemp", typeof(string), typeof(Yatse2Weather));

    public string Day3MinTemp
    {
      get { return (string)GetValue(Day3MinTempProperty); }
      set { SetValue(Day3MinTempProperty, value); }
    }

    public static readonly DependencyProperty Day4IconDayProperty =
        DependencyProperty.Register("Day4IconDay", typeof(string), typeof(Yatse2Weather));

    public string Day4IconDay
    {
      get { return (string)GetValue(Day4IconDayProperty); }
      set { SetValue(Day4IconDayProperty, value); }
    }
    public static readonly DependencyProperty Day4IconNightProperty =
        DependencyProperty.Register("Day4IconNight", typeof(string), typeof(Yatse2Weather));

    public string Day4IconNight
    {
      get { return (string)GetValue(Day4IconNightProperty); }
      set { SetValue(Day4IconNightProperty, value); }
    }

    public static readonly DependencyProperty Day4NameProperty =
        DependencyProperty.Register("Day4Name", typeof(string), typeof(Yatse2Weather));

    public string Day4Name
    {
      get { return (string)GetValue(Day4NameProperty); }
      set { SetValue(Day4NameProperty, value); }
    }

    public static readonly DependencyProperty Day4MaxTempProperty =
        DependencyProperty.Register("Day4MaxTemp", typeof(string), typeof(Yatse2Weather));

    public string Day4MaxTemp
    {
      get { return (string)GetValue(Day4MaxTempProperty); }
      set { SetValue(Day4MaxTempProperty, value); }
    }

    public static readonly DependencyProperty Day4MinTempProperty =
        DependencyProperty.Register("Day4MinTemp", typeof(string), typeof(Yatse2Weather));

    public string Day4MinTemp
    {
      get { return (string)GetValue(Day4MinTempProperty); }
      set { SetValue(Day4MinTempProperty, value); }
    }

    public void LoadCurrentData(WeatherData data, string skin)
    {
      if (data == null) return;

            var weatherArray = new string[50];
            weatherArray[46] = "chanceflurries";
            weatherArray[39] = "chancerain";
            weatherArray[6] = "chancesleet";
            weatherArray[41] = "chancesnow";
            weatherArray[39] = "chancerain";
            weatherArray[6] = "chancesleet";
            weatherArray[38] = "chancetstorms";
            weatherArray[32] = "clear";
            weatherArray[26] = "cloudy";
            weatherArray[13] = "flurries";
            weatherArray[20] = "fog";
            weatherArray[21] = "hazy";
            weatherArray[28] = "mostlycloudy";
            weatherArray[34] = "mostlysunny";
            weatherArray[30] = "partlycloudly";
            weatherArray[34] = "partlysunny";
            weatherArray[18] = "sleet";
            weatherArray[11] = "rain";
            weatherArray[42] = "snow";
            weatherArray[32] = "sunny";
            weatherArray[38] = "tstorms";
            weatherArray[46] = "nt_chanceflurries";
            weatherArray[45] = "nt_chancerain";
            weatherArray[47] = "nt_chancetstorms";
            weatherArray[31] = "nt_clear";
            weatherArray[27] = "nt_cloudy";
            weatherArray[20] = "nt_fog";
            weatherArray[21] = "nt_hazy";
            weatherArray[27] = "nt_mostlycloudy";
            weatherArray[33] = "nt_mostlysunny";
            weatherArray[29] = "nt_partlycloudy";
            weatherArray[33] = "nt_partlysunny";
           


      Location = data.LocationName;
      CurrentIcon = Helper.SkinorDefault(Helper.SkinPath, skin,  @"\Weather\Icons\" + data.Today.Icon + ".png");
      CurrentTemp = data.GetTemp(data.Today.Temperature);
            CurrentBackground = "";
            try
            {
                int weathernumber = Array.IndexOf(weatherArray, data.Today.Icon);

                if (Directory.Exists(Helper.SkinPath + @"Default\Weather\Backgrounds\" + weathernumber.ToString()) )
                {
                    var rand = new Random();
                    var files = Directory.GetFiles(Helper.SkinPath + @"\Default\Weather\Backgrounds\" + weathernumber.ToString(), "*.jpg");
                    CurrentBackground = files[rand.Next(files.Length)];
                }

            }
            catch (Exception ex)
            {
                    CurrentBackground = Helper.SkinorDefault(Helper.SkinPath, skin, @"\Weather\Backgrounds\" + data.Today.Icon + ".jpg");
            }

            if (CurrentBackground == "")
            {
                CurrentBackground = Helper.SkinorDefault(Helper.SkinPath, skin, @"\Weather\Backgrounds\" + data.Today.Icon + ".jpg");
            }

            if (CurrentBackground == "")
            {
                int pos = Array.IndexOf(weatherArray, data.Today.Icon);
                CurrentBackground = Helper.SkinorDefault(Helper.SkinPath, skin, @"\Weather\Backgrounds\" + pos.ToString() + ".jpg");
            }
   

      
    }

    public void LoadForecastData(WeatherData data, string skin)
    {
      if (data == null || data.Forecast.Count <= 0) return;

      Day0IconDay = Helper.SkinorDefault( Helper.SkinPath, skin,  @"\Weather\Icons\" + data.Forecast[0].DayIcon + ".png");
      Day0IconNight = Helper.SkinorDefault( Helper.SkinPath , skin , @"\Weather\Icons\" + data.Forecast[0].NightIcon + ".png");
      Day0Name =
        DateTime.Now.AddDays(data.Forecast[0].DayDiff)
                .ToString("dddd", CultureInfo.CurrentUICulture.DateTimeFormat)
                .ToUpperInvariant();
      Day0MaxTemp = data.GetTemp(data.Forecast[0].MaxTemp);
      Day0MinTemp = data.GetTemp(data.Forecast[0].LowTemp);
      if (data.Forecast.Count > 1)
      {
        Day1IconDay = Helper.SkinorDefault( Helper.SkinPath , skin , @"\Weather\Icons\" + data.Forecast[1].DayIcon + ".png");
        Day1IconNight = Helper.SkinorDefault(Helper.SkinPath, skin, @"\Weather\Icons\" + data.Forecast[1].NightIcon + ".png");
        Day1Name =
          DateTime.Now.AddDays(data.Forecast[1].DayDiff)
                  .ToString("dddd", CultureInfo.CurrentUICulture.DateTimeFormat)
                  .ToUpperInvariant();
        Day1MaxTemp = data.GetTemp(data.Forecast[1].MaxTemp);
        Day1MinTemp = data.GetTemp(data.Forecast[1].LowTemp);
      }
      if (data.Forecast.Count > 2)
      {
          Day2IconDay = Helper.SkinorDefault(Helper.SkinPath, skin, @"\Weather\Icons\" + data.Forecast[2].DayIcon + ".png");
        Day2IconNight = Helper.SkinorDefault(Helper.SkinPath, skin, @"\Weather\Icons\" + data.Forecast[2].NightIcon + ".png");
        Day2Name =
          DateTime.Now.AddDays(data.Forecast[2].DayDiff)
                  .ToString("dddd", CultureInfo.CurrentUICulture.DateTimeFormat)
                  .ToUpperInvariant();
        Day2MaxTemp = data.GetTemp(data.Forecast[2].MaxTemp);
        Day2MinTemp = data.GetTemp(data.Forecast[2].LowTemp);
      }
      if (data.Forecast.Count > 3)
      {
          Day3IconDay = Helper.SkinorDefault(Helper.SkinPath, skin, @"\Weather\Icons\" + data.Forecast[3].DayIcon + ".png");
        Day3IconNight = Helper.SkinorDefault(Helper.SkinPath, skin, @"\Weather\Icons\" + data.Forecast[3].NightIcon + ".png");
        Day3Name =
          DateTime.Now.AddDays(data.Forecast[3].DayDiff)
                  .ToString("dddd", CultureInfo.CurrentUICulture.DateTimeFormat)
                  .ToUpperInvariant();
        Day3MaxTemp = data.GetTemp(data.Forecast[3].MaxTemp);
        Day3MinTemp = data.GetTemp(data.Forecast[3].LowTemp);
      }
      if (data.Forecast.Count > 4)
      {
          Day4IconDay = Helper.SkinorDefault(Helper.SkinPath, skin, @"\Weather\Icons\" + data.Forecast[4].DayIcon + ".png");
        Day4IconNight = Helper.SkinorDefault(Helper.SkinPath, skin, @"\Weather\Icons\" + data.Forecast[4].NightIcon + ".png");
        Day4Name =
          DateTime.Now.AddDays(data.Forecast[4].DayDiff)
                  .ToString("dddd", CultureInfo.CurrentUICulture.DateTimeFormat)
                  .ToUpperInvariant();
        Day4MaxTemp = data.GetTemp(data.Forecast[4].MaxTemp);
        Day4MinTemp = data.GetTemp(data.Forecast[4].LowTemp);
      }
    }

  }

}