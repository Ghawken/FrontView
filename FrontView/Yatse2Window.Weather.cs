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

using Setup;
using FrontView.Libs;

namespace FrontView
{
    public partial class Yatse2Window
    {

        private void InitWeather()
        {
            _weather.Configure(Helper.CachePath + @"Weather", _config.WeatherUnit, "1170858033", "5e4459f60104f2ec");
            _weather.AutoResfresh(_config.WeatherLoc);
        }

        private void RefreshWeather()
        {
            Logger.Instance().Log("FrontView+", "Refresh weather");
            var weatherData = _weather.GetForecastWeatherData(_config.WeatherLoc);
            if (weatherData == null)
            {
                Logger.Instance().Log("FrontView+", "RefreshWeather : No forecast weather data");
                return;
            }
            _yatse2Properties.Weather.LoadForecastData(weatherData, _yatse2Properties.Skin);

        }

    }
}