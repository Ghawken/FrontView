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

        private void grd_AudioGenres_CurrentLetter_MouseLeave(object sender, MouseEventArgs e)
        {
            _startLetterDrag = false;
            ResetTimer();
        }

        private void grd_AudioGenres_CurrentLetter_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _startLetterDrag = false;
            ResetTimer();
        }

        private void AudioGenreSelectPreviousLetter(char letter)
        {
            if (letter < '.')
            {
                Helper.VirtFlowSelect(lst_AudioGenres_flow, lst_AudioGenres_flow.Items.Count - 1);
                return;
            }
            letter = (char)(letter - 1);

            var newIndex = 0;
            foreach (Yatse2AudioGenre genre in lst_AudioGenres_flow.Items)
            {
                if (genre.Name.ToUpperInvariant()[0] == letter)
                {
                    Helper.VirtFlowSelect(lst_AudioGenres_flow, newIndex);
                    return;
                }
                newIndex++;
            }
            AudioGenreSelectPreviousLetter(letter);
        }

        private void AudioGenreSelectNextLetter(char letter)
        {
            if (letter > 'Z')
            {
                Helper.VirtFlowSelect(lst_AudioGenres_flow, 0);
                return;
            }
            letter = (char)(letter + 1);

            var newIndex = 0;
            foreach (Yatse2AudioGenre genre in lst_AudioGenres_flow.Items)
            {
                if (genre.Name.ToUpperInvariant()[0] == letter)
                {
                    Helper.VirtFlowSelect(lst_AudioGenres_flow, newIndex);
                    return;
                }
                newIndex++;
            }
            AudioGenreSelectNextLetter(letter);
        }

        private void grd_AudioGenres_CurrentLetter_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_startLetterDrag)
            {
                var distanceX = _mouseDownPoint.X - e.GetPosition(this).X;
                if (Math.Abs(distanceX) > 50.0)
                {
                    if (distanceX < 0)
                    {
                        AudioGenreSelectNextLetter(
                            txb_AudioGenres_CurrentLetter.Text.ToUpperInvariant().ToCharArray()[0]);
                    }
                    else
                    {
                        AudioGenreSelectPreviousLetter(
                            txb_AudioGenres_CurrentLetter.Text.ToUpperInvariant().ToCharArray()[0]);
                    }

                    _startLetterDrag = false;
                    ResetTimer();
                }
            }
        }

        private void brd_AudioGenres_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startLetterDrag = true;
            _mouseDownPoint = e.GetPosition(this);
            e.Handled = true;

        }

        private void lst_AudioGenres_flow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var timediff = (DateTime.Now - _mouseDownTime).TotalMilliseconds;
            if (timediff > 75 && timediff < SystemInformation.DoubleClickTime)
            {
                var selitem = (Yatse2AudioGenre)lst_AudioGenres_flow.SelectedItem;
                if (selitem == null) 
                    return;

                if (_config.GenreToArtists)
                {
                    Load_AudioArtistsGenre(selitem.Name);
                    _filteredArtists = true;
                    _filterAudioArtist = "";
                    btn_AudioArtists_Filter.Background = GetSkinImageBrushSmall("Remote_Search");
                    _audioArtistsCollectionView.Refresh();
                    Helper.VirtFlowSelect(lst_AudioArtists_flow, 0);
                    if (_audioArtistsCollectionView.Count > 0)
                    {
                        ShowGrid(grd_AudioArtists);
                    }
                }
                else
                {
                    Load_AudioAlbumsGenre(selitem.Name);
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
            }
            _mouseDownTime = DateTime.Now;
        }

        private void btn_AudioGenres_AddFavorites_Click(object sender, RoutedEventArgs e)
        {
            var selitem = (Yatse2AudioGenre)lst_AudioGenres_flow.SelectedItem;
            if (selitem == null) 
                return;

            selitem.IsFavorite = (selitem.IsFavorite == 0) ? 1 : 0;
            _database.UpdateAudioGenre(selitem);
        }

        private void GenresPlay(int playmode)
        {
            var selitem = (Yatse2AudioGenre)lst_AudioGenres_flow.SelectedItem;
            if (selitem == null) 
                return;

            var songs = playmode == 0 ? _database.GetAudioSongFromGenre(_remoteInfo.Id, selitem.Name) : _database.GetAudioSongFromGenreRandom(_remoteInfo.Id, selitem.Name);
            var apisongs = new Collection<ApiAudioSong>();
            foreach (var yatse2AudioSong in songs)
            {
                apisongs.Add(yatse2AudioSong.ToApi());
            }
            _remote.AudioPlayer.PlayFiles(apisongs);
        }

        private void btn_AudioGenres_Play_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _mouseDownTime = DateTime.Now;
        }

        private void btn_AudioGenres_Play_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var timediff = (DateTime.Now - _mouseDownTime).TotalMilliseconds;
            if (timediff < _config.LongKeyPress)
            {
                GenresPlay(_config.DefaultPlayMode);
            }
            else
            {
                grd_PlayMenu.Visibility = Visibility.Visible;
            }
        }

        private void lst_AudioGenres_flow_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var stbDiaporamaHide = (Storyboard)TryFindResource("stb_AudioGenres_ShowCurrentLetter");
            if (stbDiaporamaHide != null)
                stbDiaporamaHide.Begin(this);
        }
    }
}