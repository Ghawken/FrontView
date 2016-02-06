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
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Plugin;
using FrontView.Classes;
using FrontView.Libs;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace FrontView
{
    public partial class Yatse2Window
    {
        private void btn_AudioAlbums_Infos_Click(object sender, RoutedEventArgs e)
        {
            var selitem = (Yatse2AudioAlbum)lst_AudioAlbums_flow.SelectedItem;
            if (selitem == null) 
                return;
            Load_AudioSongs(selitem.Title);

            var stbDimmingHide = (Storyboard)TryFindResource("stb_ShowAudioAlbumsDetails");
            if (stbDimmingHide != null)
                stbDimmingHide.Begin(this);
        }

        private void grd_AudioAlbums_CurrentLetter_MouseLeave(object sender, MouseEventArgs e)
        {
            _startLetterDrag = false;
            ResetTimer();
        }

        private void grd_AudioAlbums_CurrentLetter_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _startLetterDrag = false;
            ResetTimer();
        }

        private void AudioAlbumSelectPreviousLetter(char letter)
        {
            if (letter < '.')
            {
                Helper.VirtFlowSelect(lst_AudioAlbums_flow, lst_AudioAlbums_flow.Items.Count - 1);
                return;
            }
            letter = (char)(letter - 1);

            var newIndex = 0;
            foreach (Yatse2AudioAlbum album in lst_AudioAlbums_flow.Items)
            {
                if (album.Title.ToUpperInvariant()[0] == letter)
                {
                    Helper.VirtFlowSelect(lst_AudioAlbums_flow, newIndex);
                    return;
                }
                newIndex++;
            }
            AudioAlbumSelectPreviousLetter(letter);
        }

        private void AudioAlbumSelectNextLetter(char letter)
        {
            if (letter > 'Z')
            {
                Helper.VirtFlowSelect(lst_AudioAlbums_flow, 0);
                return;
            }
            letter = (char)(letter + 1);

            var newIndex = 0;
            foreach (Yatse2AudioAlbum album in lst_AudioAlbums_flow.Items)
            {
                if (album.Title.ToUpperInvariant()[0] == letter)
                {
                    Helper.VirtFlowSelect(lst_AudioAlbums_flow, newIndex);
                    return;
                }
                newIndex++;
            }
            AudioAlbumSelectNextLetter(letter);
        }


        private void grd_AudioAlbums_CurrentLetter_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_startLetterDrag)
            {
                var distanceX = _mouseDownPoint.X - e.GetPosition(this).X;
                if (Math.Abs(distanceX) > 50.0)
                {
                    if (distanceX < 0)
                    {
                        AudioAlbumSelectNextLetter(
                            txb_AudioAlbums_CurrentLetter.Text.ToUpperInvariant().ToCharArray()[0]);
                    }
                    else
                    {
                        AudioAlbumSelectPreviousLetter(
                            txb_AudioAlbums_CurrentLetter.Text.ToUpperInvariant().ToCharArray()[0]);
                    }

                    _startLetterDrag = false;
                    ResetTimer();
                }
            }
        }

        private void brd_AudioAlbums_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startLetterDrag = true;
            _mouseDownPoint = e.GetPosition(this);
            e.Handled = true;

        }

        private void lst_AudioAlbums_flow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var timediff = (DateTime.Now - _mouseDownTime).TotalMilliseconds;
            if (timediff > 75 && timediff < SystemInformation.DoubleClickTime)
            {
                btn_AudioAlbums_Infos_Click(null, null);
            }
            _mouseDownTime = DateTime.Now;
        }

        private void btn_AudioAlbums_AddFavorites_Click(object sender, RoutedEventArgs e)
        {
            var selitem = (Yatse2AudioAlbum)lst_AudioAlbums_flow.SelectedItem;
            if (selitem == null) 
                return;

            selitem.IsFavorite = (selitem.IsFavorite == 0) ? 1 : 0;
            _database.UpdateAudioAlbum(selitem);
        }

        private void AlbumsPlay(int playmode)
        {
            var selitem = (Yatse2AudioAlbum)lst_AudioAlbums_flow.SelectedItem;
            if (selitem == null) 
                return;

            var songs = playmode == 0 ? _database.GetAudioSongFromAlbumName(_remoteInfo.Id, selitem.Title) : _database.GetAudioSongFromAlbumNameRandom(_remoteInfo.Id, selitem.Title);
            var apisongs = new Collection<ApiAudioSong>();
            foreach (var yatse2AudioSong in songs)
            {
                apisongs.Add(yatse2AudioSong.ToApi());
            }
            _remote.AudioPlayer.PlayFiles(apisongs);
        }

        private void btn_AudioAlbums_Play_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _mouseDownTime = DateTime.Now;
        }

        private void btn_AudioAlbums_Play_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var timediff = (DateTime.Now - _mouseDownTime).TotalMilliseconds;
            if (timediff < _config.LongKeyPress)
            {
                AlbumsPlay(_config.DefaultPlayMode);
            }
            else
            {
                grd_PlayMenu.Visibility = Visibility.Visible;
            }
        }


        private void lst_AudioAlbums_flow_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selitem = (Yatse2AudioAlbum)lst_AudioAlbums_flow.SelectedItem;
            if (selitem == null) 
                return;

            var temp = _database.GetAudioArtistFromName(_remoteInfo.Id, selitem.Artist);
            if (temp.Count > 0)
                _yatse2Properties.CurrentAlbumArtistFanart = temp[0].Fanart;


            var stbDiaporamaHide = (Storyboard)TryFindResource("stb_AudioAlbums_ShowCurrentLetter");
            if (stbDiaporamaHide != null)
                stbDiaporamaHide.Begin(this);
        }

        private void btn_AudioAlbums_Details_Down_Click(object sender, RoutedEventArgs e)
        {
            var index = lst_AudioAlbums_Details.SelectedIndex+1;
            if (index > (lst_AudioAlbums_Details.Items.Count -1))
            {
                index = 0;
            }

            lst_AudioAlbums_Details.SelectedIndex = index;
            lst_AudioAlbums_Details.ScrollIntoView(lst_AudioAlbums_Details.Items[index]);
        }

        private void btn_AudioAlbums_Details_Up_Click(object sender, RoutedEventArgs e)
        {
            var index = lst_AudioAlbums_Details.SelectedIndex - 1;
            if (index < 0)
            {
                index = lst_AudioAlbums_Details.Items.Count - 1;
            }
            lst_AudioAlbums_Details.SelectedIndex = index;
            lst_AudioAlbums_Details.ScrollIntoView(lst_AudioAlbums_Details.Items[index]);
        }

        private void ListBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var timediff = (DateTime.Now - _mouseDownTime).TotalMilliseconds;
            if (timediff > 75 && timediff < SystemInformation.DoubleClickTime)
            {
                var selitem = (Yatse2AudioSong)lst_AudioAlbums_Details.SelectedItem;
                if (selitem == null) 
                    return;
                var temp = new Collection<ApiAudioSong>
                               {selitem.ToApi()};
                _remote.AudioPlayer.PlayFiles(temp);
                var stbDimmingHide = (Storyboard)TryFindResource("stb_HideAudioAlbumsDetails");
                if (stbDimmingHide != null)
                    stbDimmingHide.Begin(this);
            }
            _mouseDownTime = DateTime.Now;
            e.Handled = false;
        }


        private void btn_AudioSongs_Play_Click(object sender, RoutedEventArgs e)
        {
            var selitem = (Yatse2AudioSong)lst_AudioAlbums_Details.SelectedItem;
            if (selitem == null) 
                return;
            var temp = new Collection<ApiAudioSong> { selitem.ToApi() };
            _remote.AudioPlayer.PlayFiles(temp);

        }

        private void grd_AudioAlbums_Details_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var stbDimmingHide = (Storyboard)TryFindResource("stb_HideAudioAlbumsDetails");
            if (stbDimmingHide != null)
                stbDimmingHide.Begin(this);
        }

        private void AudioAlbums_TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var stbDimmingHide = (Storyboard)TryFindResource("stb_HideAudioAlbumsDetails");
            if (stbDimmingHide != null)
                stbDimmingHide.Begin(this);
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var stbDimmingHide = (Storyboard)TryFindResource("stb_HideAudioAlbumsDetails");
            if (stbDimmingHide != null)
                stbDimmingHide.Begin(this);
        }
    }
}