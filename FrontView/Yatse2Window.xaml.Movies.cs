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
        private void lst_Movies_flow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selitem = (Yatse2Movie)lst_Movies_flow.SelectedItem;
            if (selitem == null)
                return;

            txb_Movies_CurrentLetter.Text = _config.IgnoreSortTokens ? Regex.Replace(selitem.Title, @"^(" + _config.SortTokens + ")", "").ToUpperInvariant()[0].ToString() : selitem.Title.ToUpperInvariant()[0].ToString();

            var stbDiaporamaHide = (Storyboard) TryFindResource("stb_Movies_ShowCurrentLetter");
            if (stbDiaporamaHide != null)
                stbDiaporamaHide.Begin(this);
        }

        private void btn_Movies_AddFavorites_Click(object sender, RoutedEventArgs e)
        {
            var selitem = (Yatse2Movie) lst_Movies_flow.SelectedItem;
            if (selitem == null) 
                return;

            selitem.IsFavorite = (selitem.IsFavorite == 0) ? 1 : 0;
            _database.UpdateMovie(selitem);
        }

        private void btn_Movies_Play_Click(object sender, RoutedEventArgs e)
        {
            var selitem = (Yatse2Movie) lst_Movies_flow.SelectedItem;
            if (selitem == null) 
                return;

            selitem.PlayCount = selitem.PlayCount + 1;
            _database.UpdateMovie(selitem);

            _remote.VideoPlayer.PlayMovie(selitem.ToApi());
        }

        private void btn_Movies_Infos_Click(object sender, RoutedEventArgs e)
        {
            var stbDimmingHide = (Storyboard)TryFindResource("stb_ShowMovieDetails");
            if (stbDimmingHide != null)
                stbDimmingHide.Begin(this);
        }


        private void MovieSelectPreviousLetter(char letter)
        {
            if (letter < '.')
            {
                Helper.VirtFlowSelect(lst_Movies_flow, lst_Movies_flow.Items.Count - 1);
                return;
            }
            letter = (char)(letter - 1);

            var newIndex = 0;
            foreach (Yatse2Movie movie in lst_Movies_flow.Items)
            {
                var iletter = _config.IgnoreSortTokens ? Regex.Replace(movie.Title, @"^(" + _config.SortTokens + ")", "").ToUpperInvariant() : movie.Title.ToUpperInvariant();

                if (iletter[0] == letter)
                {
                    Helper.VirtFlowSelect(lst_Movies_flow, newIndex);
                    return;
                }
                newIndex++;
            }
            MovieSelectPreviousLetter(letter);
        }

        private void MovieSelectNextLetter(char letter)
        {
            if (letter > 'Z')
            {
                Helper.VirtFlowSelect(lst_Movies_flow, 0);
                return;
            }
            letter = (char)(letter + 1);

            var newIndex = 0;
            foreach(Yatse2Movie movie in lst_Movies_flow.Items)
            {
                var iletter = _config.IgnoreSortTokens ? Regex.Replace(movie.Title, @"^(" + _config.SortTokens + ")", "").ToUpperInvariant() : movie.Title.ToUpperInvariant();
                if (iletter[0] == letter)
                {
                    Helper.VirtFlowSelect(lst_Movies_flow, newIndex);
                    return;
                }
                newIndex++;
            }
            MovieSelectNextLetter(letter);
        }

        private void Border_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _startLetterDrag = true;
            _mouseDownPoint = e.GetPosition(this);
            e.Handled = true;
        }

        private void grd_Movies_CurrentLetter_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_startLetterDrag)
            {
                var distanceX = _mouseDownPoint.X - e.GetPosition(this).X;
                
                if (Math.Abs(distanceX) > 50.0)
                {
                    if (distanceX < 0)
                    {
                        MovieSelectNextLetter(txb_Movies_CurrentLetter.Text.ToUpperInvariant().ToCharArray()[0]);
                    }
                    else
                    {
                        MovieSelectPreviousLetter(txb_Movies_CurrentLetter.Text.ToUpperInvariant().ToCharArray()[0]);
                    }

                    _startLetterDrag = false;
                    ResetTimer();
                }
            }
        }

        private void grd_Movies_CurrentLetter_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _startLetterDrag = false;
            ResetTimer();
        }

        private void grd_Movies_CurrentLetter_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
           _startLetterDrag = false;
            ResetTimer();
        }

        private void grd_Movies_Details_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var stbDimmingHide = (Storyboard)TryFindResource("stb_HideMovieDetails");
            if (stbDimmingHide != null)
                stbDimmingHide.Begin(this);
        }

        private void lst_Movies_flow_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var timediff = (DateTime.Now - _mouseDownTime).TotalMilliseconds;
            if (timediff > 75 && timediff < SystemInformation.DoubleClickTime)
            {
                var selitem = (Yatse2Movie)lst_Movies_flow.SelectedItem;
                if (selitem == null) 
                    return;
                btn_Movies_Infos_Click(null, null);
            }
            _mouseDownTime = DateTime.Now;
        }

    }
}