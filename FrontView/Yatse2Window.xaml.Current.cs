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

using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace FrontView
{
    public partial class Yatse2Window
    {

        private void btn_Current_Tv_ShowDetails_Click(object sender, RoutedEventArgs e)
        {
            var stbShowDetails = (Storyboard)TryFindResource("stb_CurrentTv_ShowDetails");
            if (stbShowDetails != null)
                stbShowDetails.Begin(this);
            if (_config.Hack480)
                _yatse2Properties.Currently.IsTvDetails480 = true;
            else
                _yatse2Properties.Currently.IsTvDetails = true;
        }

        private void btn_Current_Tv_HideDetails_Click(object sender, RoutedEventArgs e)
        {
            var stbHideDetails = (Storyboard)TryFindResource("stb_CurrentTv_HideDetails");
            if (stbHideDetails != null)
                stbHideDetails.Begin(this);
            _yatse2Properties.Currently.IsTvDetails = false;
        }

        private void txb_Current_Tv_ToggleDetail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_yatse2Properties.Currently.IsTvDetails || _yatse2Properties.Currently.IsTvDetails480)
            {
                btn_Current_Tv_HideDetails_Click(sender, e);
            }
            else
            {
                btn_Current_Tv_ShowDetails_Click(sender, e);
            }
        }

        private void btn_Current_Movie_ShowDetails_Click(object sender, RoutedEventArgs e)
        {
            var stbShowDetails = (Storyboard)TryFindResource("stb_CurrentMovie_ShowDetails");
            if (stbShowDetails != null)
                stbShowDetails.Begin(this);
            if (_config.Hack480)
                _yatse2Properties.Currently.IsMovieDetails480 = true;
            else
                _yatse2Properties.Currently.IsMovieDetails = true;
        }

        private void btn_Current_Movie_HideDetails_Click(object sender, RoutedEventArgs e)
        {
            var stbHideDetails = (Storyboard)TryFindResource("stb_CurrentMovie_HideDetails");
            if (stbHideDetails != null)
                stbHideDetails.Begin(this);
            _yatse2Properties.Currently.IsMovieDetails = false;
        }

        private void txb_Current_Movie_ToggleDetail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_yatse2Properties.Currently.IsMovieDetails || _yatse2Properties.Currently.IsMovieDetails480)
            {
                btn_Current_Movie_HideDetails_Click(sender, e);
            }
            else
            {
                btn_Current_Movie_ShowDetails_Click(sender, e);
            }
        }

        private void btn_Current_Music_ShowDetails_Click(object sender, RoutedEventArgs e)
        {
            var stbShowDetails = (Storyboard)TryFindResource("stb_CurrentMusic_ShowDetails");
            if (stbShowDetails != null)
                stbShowDetails.Begin(this);
            if (_config.Hack480)
                _yatse2Properties.Currently.IsMusicDetails480 = true;
            else
                _yatse2Properties.Currently.IsMusicDetails = true;
        }

        private void btn_Current_Music_HideDetails_Click(object sender, RoutedEventArgs e)
        {
            var stbHideDetails = (Storyboard)TryFindResource("stb_CurrentMusic_HideDetails");
            if (stbHideDetails != null)
                stbHideDetails.Begin(this);
            _yatse2Properties.Currently.IsMusicDetails = false;
        }

        private void txb_Current_Music_ToggleDetail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_yatse2Properties.Currently.IsMusicDetails || _yatse2Properties.Currently.IsMusicDetails480)
            {
                btn_Current_Music_HideDetails_Click(sender, e);
            }
            else
            {
                btn_Current_Music_ShowDetails_Click(sender, e);
            }
        }


        private void btn_Current_Music_Previous_Click(object sender, RoutedEventArgs e)
        {
            _remote.Player.SkipPrevious();
        }

        private void btn_Current_Music_Play_Click(object sender, RoutedEventArgs e)
        {
            _remote.Player.PlayPause();
        }

        private void btn_Current_Music_Stop_Click(object sender, RoutedEventArgs e)
        {
            _remote.Player.Stop();
        }

        private void btn_Current_Music_Pause_Click(object sender, RoutedEventArgs e)
        {
            _remote.Player.PlayPause();
        }

        private void btn_Current_Music_Next_Click(object sender, RoutedEventArgs e)
        {
            _remote.Player.SkipNext();
        }

        private void btn_Current_Music_Mute_Click(object sender, RoutedEventArgs e)
        {

            if (_config.UseReceiverIPforVolume == false)
            {
                _remote.Player.ToggleMute();
            }
            else
            {
                receiver.MuteUnmute();
            }
            
        }

        private void sld_Current_Music_Progress_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            var mousePosition = e.GetPosition(sld_Current_Music_Progress).X;
            var ratio = mousePosition / sld_Current_Music_Progress.ActualWidth;
            var newvalue = ratio * sld_Current_Music_Progress.Maximum;



            _remote.Player.SeekPercentage((int)newvalue);
        }

        private void sld_Current_Music_Volume_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            
            
            
            var mousePosition = e.GetPosition(sld_Current_Music_Volume).Y;
            var ratio = mousePosition / sld_Current_Music_Volume.ActualHeight;
            var newvalue = 100 - ratio * sld_Current_Music_Volume.Maximum;

            if (_config.UseReceiverIPforVolume == false)
            {
                _remote.Player.SetVolume((int)newvalue);
            }
            else
            {
                receiver.VolumeUp();
            }
            


        }
       
    }
}