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

namespace FrontView
{
    public partial class Yatse2Window
    {

        private void btn_Remote_Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshLibrary();
        }

        private void btn_Remote_Close_Click(object sender, RoutedEventArgs e)
        {
            grd_Remote.Visibility = Visibility.Hidden;
        }

        private void btn_Remote_Home_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Home();
        }

        private void btn_Remote_Movies_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Video();
        }

        private void btn_Remote_Music_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Music();
        }

        private void btn_Remote_Tv_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Tv();
        }

        private void btn_Remote_Pictures_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Pictures();
        }

        private void btn_Remote_DVD_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.PlayDrive();
        }

        private void btn_Remote_Eject_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.EjectDrive();
        }

        private void btn_Remote_Subtitles_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Subtitles();
        }

        private void btn_Remote_Context_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Title();
        }

        private void btn_Remote_Return_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Return();
        }

        private void btn_Remote_Up_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Up();
        }

        private void btn_Remote_Info_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Info();
        }

        private void btn_Remote_Left_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Left();
        }

        private void btn_Remote_Enter_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Enter();
        }

        private void btn_Remote_Right_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Right();
        }

        private void btn_Remote_Down_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Down();
        }

        private void btn_Remote_VolDown_Click(object sender, RoutedEventArgs e)
        {
          
           
            
            if (_config.UseReceiverIPforVolume == false)
            {
                _remote.Remote.VolDown();
            }
            else
            {
                receiver.VolumeDown();
            }


        }

        private void btn_Remote_Mute_Click(object sender, RoutedEventArgs e)
        {
            if (_config.UseReceiverIPforVolume == false)
            {
                _remote.Remote.ToggleMute();
            }
            else
            {
                receiver.MuteUnmute();
            }
            
            
        }

        private void btn_Remote_VolUp_Click(object sender, RoutedEventArgs e)
        {
            if (_config.UseReceiverIPforVolume == false)
            {
                _remote.Remote.VolUp();
            }
            else
            {
                receiver.VolumeUp();
            }
            
            
        }

        private void btn_Remote_Previous_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Previous();
        }

        private void btn_Remote_Rewind_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Rewind();
        }

        private void btn_Remote_Stop_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Stop();
        }

        private void btn_Remote_Play_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Play();
        }

        private void btn_Remote_Forward_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Forward();
        }

        private void btn_Remote_Next_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Next();
        }

        private void btn_Remote_1_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.One();
        }

        private void btn_Remote_2_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Two();
        }

        private void btn_Remote_3_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Three();
        }

        private void btn_Remote_4_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Four();
        }

        private void btn_Remote_5_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Five();
        }

        private void btn_Remote_6_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Six();
        }

        private void btn_Remote_7_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Seven();
        }

        private void btn_Remote_8_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Eight();
        }

        private void btn_Remote_9_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Nine();
        }

        private void btn_Remote_0_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Zero();
        }


        private void btn_Remote_Star_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Star();
        }

        private void btn_Remote_Hash_Click(object sender, RoutedEventArgs e)
        {
            _remote.Remote.Hash();
        }
    }
}