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
using System.Windows.Controls;
using System.Windows.Forms;
using FrontView.Classes;

namespace FrontView
{
    public partial class Yatse2Window
    {

        private void lst_TvSeasons_flow_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var timediff = (DateTime.Now - _mouseDownTime).TotalMilliseconds;
            if (timediff > 75 && timediff < SystemInformation.DoubleClickTime)
            {
                var selitem = (Yatse2TvSeason)lst_TvSeasons_flow.SelectedItem;
                if (selitem == null) return;

                if (selitem.SeasonNumber == -1)
                    Load_TvEpisodes(selitem.Show);
                else
                    Load_TvEpisodesSeason(selitem.Show, selitem.SeasonNumber);
                ShowGrid(grd_TvEpisodes);

            }
            _mouseDownTime = DateTime.Now;
        }

        private void lst_TvSeasons_flow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selitem = (Yatse2TvSeason)lst_TvSeasons_flow.SelectedItem;
            if (selitem == null) return;

            if (selitem.SeasonNumber > 0)
            {
                _yatse2Properties.TvSeasonName = GetLocalizedString(77) + " " + selitem.SeasonNumber;
                _yatse2Properties.TvSeasonEpCount = selitem.EpisodeCount.ToString(CultureInfo.InvariantCulture);
            }
            if (selitem.SeasonNumber == 0)
            {
                _yatse2Properties.TvSeasonName = GetLocalizedString(122);
                _yatse2Properties.TvSeasonEpCount = selitem.EpisodeCount.ToString(CultureInfo.InvariantCulture);
            }

            if (selitem.SeasonNumber == -1)
            {
                _yatse2Properties.TvSeasonName = GetLocalizedString(123);
                var temp = _database.GetTvEpisodeFromTvShow(_remoteInfo.Id, selitem.Show);
                _yatse2Properties.TvSeasonEpCount = temp.Count.ToString(CultureInfo.InvariantCulture);
            }

        }

    }
}