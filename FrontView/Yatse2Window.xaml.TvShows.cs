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
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using FrontView.Classes;
using FrontView.Libs;

namespace FrontView
{
    public partial class Yatse2Window
    {

        private void TvShowSelectPreviousLetter(char letter)
        {
            if (letter < '.')
            {
                Helper.VirtFlowSelect(lst_TvShows_flow, lst_TvShows_flow.Items.Count - 1);
                return;
            }
            letter = (char)(letter - 1);

            var newIndex = 0;
            foreach (Yatse2TvShow tvShow in lst_TvShows_flow.Items)
            {
                var iletter = _config.IgnoreSortTokens ? Regex.Replace(tvShow.Title, @"^(" + _config.SortTokens + ")", "").ToUpperInvariant() : tvShow.Title.ToUpperInvariant();
                if (iletter[0] == letter)
                {
                    Helper.VirtFlowSelect(lst_TvShows_flow, newIndex);
                    return;
                }
                newIndex++;
            }
            TvShowSelectPreviousLetter(letter);
        }

        private void TvShowSelectNextLetter(char letter)
        {
            if (letter > 'Z')
            {
                Helper.VirtFlowSelect(lst_TvShows_flow, 0);
                return;
            }
            letter = (char)(letter + 1);

            var newIndex = 0;
            foreach (Yatse2TvShow tvShow in lst_TvShows_flow.Items)
            {
                var iletter = _config.IgnoreSortTokens ? Regex.Replace(tvShow.Title, @"^(" + _config.SortTokens + ")", "").ToUpperInvariant() : tvShow.Title.ToUpperInvariant();
                if (iletter[0] == letter)
                {
                    Helper.VirtFlowSelect(lst_TvShows_flow, newIndex);
                    return;
                }
                newIndex++;
            }
            TvShowSelectNextLetter(letter);
        }

        private void grd_TvShows_CurrentLetter_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _startLetterDrag = false;
            ResetTimer();
        }

        private void grd_TvShows_CurrentLetter_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _startLetterDrag = false;
            ResetTimer();
        }

        private void grd_TvShows_CurrentLetter_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_startLetterDrag)
            {
                var distanceX = _mouseDownPoint.X - e.GetPosition(this).X;
                if (Math.Abs(distanceX) > 50.0)
                {
                    if (distanceX < 0)
                    {
                        TvShowSelectNextLetter(txb_TvShows_CurrentLetter.Text.ToUpperInvariant().ToCharArray()[0]);
                    }
                    else
                    {
                        TvShowSelectPreviousLetter(txb_TvShows_CurrentLetter.Text.ToUpperInvariant().ToCharArray()[0]);
                    }

                    _startLetterDrag = false;
                    ResetTimer();
                }
            }
        }

        private void TvShowBorder_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _startLetterDrag = true;
            _mouseDownPoint = e.GetPosition(this);
            e.Handled = true;
        }

        private void btn_TvShows_AddFavorites_Click(object sender, RoutedEventArgs e)
        {
            var selitem = (Yatse2TvShow)lst_TvShows_flow.SelectedItem;
            if (selitem == null) return;

            selitem.IsFavorite = (selitem.IsFavorite == 0) ? 1 : 0;
            _database.UpdateTvShow(selitem);
        }

        private void btn_TvShows_Infos_Click(object sender, RoutedEventArgs e)
        {
            var stbDimmingHide = (Storyboard)TryFindResource("stb_ShowTvShowDetails");
            if (stbDimmingHide != null)
                stbDimmingHide.Begin(this);
        }

        private void lst_TvShows_flow_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var timediff = (DateTime.Now - _mouseDownTime).TotalMilliseconds;
            if (timediff > 75 && timediff < SystemInformation.DoubleClickTime)
            {
                var selitem = (Yatse2TvShow)lst_TvShows_flow.SelectedItem;
                if (selitem == null) return;

                
                Load_TvSeasons(selitem.Title);

                


                if (_tvSeasonsCollectionView.Count > 2)
                {
                    ShowGrid(grd_TvSeasons);
                }
                else
                {
                    Load_TvEpisodes(selitem.Title);
                    ShowGrid(grd_TvEpisodes);
                }
            }
            _mouseDownTime = DateTime.Now;
        }

        private void lst_TvShows_flow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selitem = (Yatse2TvShow)lst_TvShows_flow.SelectedItem;
            if (selitem == null) return;

            txb_TvShows_CurrentLetter.Text = _config.IgnoreSortTokens ? Regex.Replace(selitem.Title, @"^(" + _config.SortTokens + ")", "").ToUpperInvariant()[0].ToString() : selitem.Title.ToUpperInvariant()[0].ToString();

            var stbDiaporamaHide = (Storyboard)TryFindResource("stb_TvShows_ShowCurrentLetter");
            if (stbDiaporamaHide != null)
                stbDiaporamaHide.Begin(this);
        }

        private void grd_TvShows_Details_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var stbDimmingHide = (Storyboard)TryFindResource("stb_HideTvShowDetails"); 
            if (stbDimmingHide != null)
                stbDimmingHide.Begin(this);
        }
        
    }
}