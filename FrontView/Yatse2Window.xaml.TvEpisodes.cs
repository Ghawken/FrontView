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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using FrontView.Classes;

namespace FrontView
{
    public partial class Yatse2Window
    {
        private void btn_TvEpisodes_Scroll_Up_Click(object sender, RoutedEventArgs e)
        {
            var index = lst_TvEpisodes_flow.SelectedIndex - 1;
            if (index < 0)
            {
                index = lst_TvEpisodes_flow.Items.Count - 1;
            }
            lst_TvEpisodes_flow.SelectedIndex = index;
            lst_TvEpisodes_flow.ScrollIntoView(lst_TvEpisodes_flow.Items[index]);
        }

        private void btn_TvEpisodes_Scroll_Down_Click(object sender, RoutedEventArgs e)
        {
            var index = lst_TvEpisodes_flow.SelectedIndex + 1;
            if (index > (lst_TvEpisodes_flow.Items.Count - 1))
            {
                index = 0;
            }

            lst_TvEpisodes_flow.SelectedIndex = index;
            lst_TvEpisodes_flow.ScrollIntoView(lst_TvEpisodes_flow.Items[index]);
        }

        private void lst_TvEpisodes_flow_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var timediff = (DateTime.Now - _mouseDownTime).TotalMilliseconds;
            if (timediff > 75 && timediff < SystemInformation.DoubleClickTime)
            {
                btn_TvEpisodes_Play_Click(null, null);
            }
            _mouseDownTime = DateTime.Now;
        }

        private void lst_TvEpisodes_flow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void btn_TvEpisodes_Play_Click(object sender, RoutedEventArgs e)
        {
            var selitem = (Yatse2TvEpisode)lst_TvEpisodes_flow.SelectedItem;
            if (selitem == null) return;

            selitem.PlayCount = selitem.PlayCount + 1;
            _database.UpdateTvEpisode(selitem);

            _remote.VideoPlayer.PlayTvEpisode(selitem.ToApi());
        }


    }
}