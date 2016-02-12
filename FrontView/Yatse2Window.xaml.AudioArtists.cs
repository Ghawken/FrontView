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

        private void grd_AudioArtists_Details_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var stbDiaporamaHide = (Storyboard)TryFindResource("stb_AudioArtists_HideDetails");
            if (stbDiaporamaHide != null)
                stbDiaporamaHide.Begin(this);
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var stbDiaporamaHide = (Storyboard)TryFindResource("stb_AudioArtists_HideDetails");
            if (stbDiaporamaHide != null)
                stbDiaporamaHide.Begin(this);
        }

        private void grd_AudioArtists_CurrentLetter_MouseLeave(object sender, MouseEventArgs e)
        {
            _startLetterDrag = false;
            ResetTimer();
        }

        private void grd_AudioArtists_CurrentLetter_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _startLetterDrag = false;
            ResetTimer();
        }

        private void AudioArtistSelectPreviousLetter(char letter)
        {
            if (letter < '.')
            {
                Helper.VirtFlowSelect(lst_AudioArtists_flow, lst_AudioArtists_flow.Items.Count - 1);
                return;
            }
            letter = (char)(letter - 1);

            var newIndex = 0;
            foreach (Yatse2AudioArtist artist in lst_AudioArtists_flow.Items)
            {
                if (artist.Name.ToUpperInvariant()[0] == letter)
                {
                    Helper.VirtFlowSelect(lst_AudioArtists_flow, newIndex);
                    return;
                }
                newIndex++;
            }
            AudioArtistSelectPreviousLetter(letter);
        }

        private void AudioArtistSelectNextLetter(char letter)
        {
            if (letter > 'Z')
            {
                Helper.VirtFlowSelect(lst_AudioArtists_flow, 0);
                return;
            }
            letter = (char)(letter + 1);

            var newIndex = 0;
            foreach (Yatse2AudioArtist artist in lst_AudioArtists_flow.Items)
            {
                if (artist.Name.ToUpperInvariant()[0] == letter)
                {
                    Helper.VirtFlowSelect(lst_AudioArtists_flow, newIndex);
                    return;
                }
                newIndex++;
            }
            AudioArtistSelectNextLetter(letter);
        }



        private void grd_AudioArtists_CurrentLetter_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_startLetterDrag)
            {
                var distanceX = _mouseDownPoint.X - e.GetPosition(this).X;
                if (Math.Abs(distanceX) > 50.0)
                {
                    if (distanceX < 0)
                    {
                        AudioArtistSelectNextLetter(
                            txb_AudioArtists_CurrentLetter.Text.ToUpperInvariant().ToCharArray()[0]);
                    }
                    else
                    {
                        AudioArtistSelectPreviousLetter(
                            txb_AudioArtists_CurrentLetter.Text.ToUpperInvariant().ToCharArray()[0]);
                    }

                    _startLetterDrag = false;
                    ResetTimer();
                }
            }
        }

        private void brd_AudioArtists_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startLetterDrag = true;
            _mouseDownPoint = e.GetPosition(this);
            e.Handled = true;
        }

        private void lst_AudioArtists_flow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var timediff = (DateTime.Now - _mouseDownTime).TotalMilliseconds;
            if (timediff > 75 && timediff < SystemInformation.DoubleClickTime)
            {
                var selitem = (Yatse2AudioArtist)lst_AudioArtists_flow.SelectedItem;
                if (selitem == null) return;
                Load_AudioAlbumsArtists(selitem.Name);
                _filteredAlbums = true;
                _filterAudioAlbum = "";
                btn_AudioAlbums_Filter.Background = GetSkinImageBrushSmall("Remote_Search");
                _audioAlbumsCollectionView.Refresh();
                Helper.VirtFlowSelect(lst_AudioAlbums_flow, 0);
                if (_audioAlbumsCollectionView.Count > 0)
                {
                    ShowGrid(grd_AudioAlbums);
                }
            }
            _mouseDownTime = DateTime.Now;
        }

        private void btn_AudioArtists_AddFavorites_Click(object sender, RoutedEventArgs e)
        {
            var selitem = (Yatse2AudioArtist)lst_AudioArtists_flow.SelectedItem;
            if (selitem == null) 
                return;

            selitem.IsFavorite = (selitem.IsFavorite == 0) ? 1 : 0;
            _database.UpdateAudioArtist(selitem);
        }

        private void ArtistsPlay(int playmode)
        {
            var selitem = (Yatse2AudioArtist)lst_AudioArtists_flow.SelectedItem;
            if (selitem == null) 
                return;

            var songs = playmode == 0 ? _database.GetAudioSongFromArtist(_remoteInfo.Id, selitem.Name) : _database.GetAudioSongFromArtistRandom(_remoteInfo.Id, selitem.Name);
            var apisongs = new Collection<ApiAudioSong>();
            foreach (var yatse2AudioSong in songs)
            {
                apisongs.Add(yatse2AudioSong.ToApi());
            }
            _remote.AudioPlayer.PlayFiles(apisongs);
        }


        private void btn_AudioArtists_Play_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _mouseDownTime = DateTime.Now;
        }

        private void btn_AudioArtists_Play_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var timediff = (DateTime.Now - _mouseDownTime).TotalMilliseconds;
            if (timediff < _config.LongKeyPress)
            {
                ArtistsPlay(_config.DefaultPlayMode);
            }
            else
            {
                grd_PlayMenu.Visibility = Visibility.Visible;
            }
        }



        private void lst_AudioArtists_flow_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var stbDiaporamaHide = (Storyboard)TryFindResource("stb_AudioArtists_ShowCurrentLetter");
            if (stbDiaporamaHide != null)
                stbDiaporamaHide.Begin(this);
        }

        private void btn_AudioArtists_Infos_Click(object sender, RoutedEventArgs e)
        {
            if (grd_AudioArtists_Details.Opacity > 0)
            {
                var stbDiaporamaHide = (Storyboard)TryFindResource("stb_AudioArtists_HideDetails");
                if (stbDiaporamaHide != null)
                    stbDiaporamaHide.Begin(this);
            }
            else
            {
                var stbDiaporamaHide = (Storyboard)TryFindResource("stb_AudioArtists_ShowDetails");
                if (stbDiaporamaHide != null)
                    stbDiaporamaHide.Begin(this);
            }
        }

    }
}