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

using System.Text.RegularExpressions;
using System.Windows;
using Setup;
using FrontView.Classes;
using FrontView.Libs;

namespace FrontView
{
    public partial class Yatse2Window
    {

        private void btn_Remotes_Default_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selitem = (Yatse2Remote)lst_Remotes.SelectedItem;
            if (selitem == null) return;

            var selid = lst_Remotes.SelectedIndex;
            _config.DefaultRemote = selitem.Id;
            _config.Save(_configFile);
            RefreshRemotes();
            lst_Remotes.SelectedIndex = selid;
        }

        private void btn_Remotes_Delete_Click(object sender, RoutedEventArgs e)
        {
            var selitem = (Yatse2Remote)lst_Remotes.SelectedItem;
            if (selitem == null) 
                return;

            var result = ShowYesNoDialog(GetLocalizedString(11));

            if (result != true) 
                return;

            _database.DeleteRemoteAndData(selitem.Id);
            if (lst_Remotes.Items.Count == 1)
            {
                _config.DefaultRemote = 0;
                _config.Save(_configFile);
            }
            if (_config.DefaultRemote == selitem.Id)
            {
                _config.DefaultRemote = ((Yatse2Remote)lst_Remotes.Items[0]).Id;
                _config.Save(_configFile);
            }

            RefreshRemotes();
        }

        private void txb_Remotes_Edit_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selitem = (Yatse2Remote)lst_Remotes.SelectedItem;
            if (selitem == null) return;

            btn_Settings_Remotes_Edit_Accept.Content = GetLocalizedString(13);

            ApiHelper.Instance().FillApiComboBox(lst_Settings_Remotes_Edit_Api, selitem.Api);
            _remoteInfoEdit = selitem;
            grd_Settings_Remotes_Edit.DataContext = _remoteInfoEdit;

            grd_Settings_Remotes_Edit.Visibility = Visibility.Visible;
            //_yatse2Properties.ShowSettingsRemotesEdit = true;
        }

        private void txb_Remotes_Add_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            btn_Remotes_Add_Click(null, null);
        }

        private void btn_Remotes_Add_Click(object sender, RoutedEventArgs e)
        {
            btn_Settings_Remotes_Edit_Accept.Content = GetLocalizedString(14);

            ApiHelper.Instance().FillApiComboBox(lst_Settings_Remotes_Edit_Api, null);
            _remoteInfoEdit = new Yatse2Remote { OS = "Unknown" };
            grd_Settings_Remotes_Edit.DataContext = _remoteInfoEdit;

            grd_Settings_Remotes_Edit.Visibility = Visibility.Visible;
            //_yatse2Properties.ShowSettingsRemotesEdit = true;
        }

        private void btn_Remotes_Connect_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            


            var selitem = (Yatse2Remote)lst_Remotes.SelectedItem;
            if (selitem == null)
                return;

            if (selitem.Id == _currentRemoteId) 
                return;

            var remote = ApiHelper.Instance().GetRemoteByApi(selitem.Api);

            var res = remote.TestConnection(selitem.IP, selitem.Port, selitem.Login, selitem.Password);


            if (res !=1 )
            { 
                ShowOkDialog(GetLocalizedString(16));
                return;
            }

            btn_Header_Remotes.Background = GetSkinImageBrush("Menu_Remote_Disconnected");
            _currentRemoteId = selitem.Id;
            InitRemote();
            RefreshRemotes();


        }

        private void btn_Remote_WOL_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selitem = (Yatse2Remote)lst_Remotes.SelectedItem;
            if (selitem == null) 
                return;

            var macAddress = Regex.Replace(selitem.MacAddress, @"[^0-9A-Fa-f]", "");
            NetworkTools.Wakeup(macAddress);
            Logger.Instance().Log("FrontView+", "Sending WOL to : " + macAddress + " ( " + selitem.Name + ")");
        }

        private void btn_Remote_Reboot_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selitem = (Yatse2Remote)lst_Remotes.SelectedItem;
            if (selitem == null) return;

            var remote = ApiHelper.Instance().GetRemoteByApi(selitem.Api);
            remote.Configure(selitem.IP, selitem.Port, selitem.Login, selitem.Password);
            remote.SystemRunning.Reboot();
            remote.Close();
        }

        private void btn_Remote_Shutdown_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selitem = (Yatse2Remote)lst_Remotes.SelectedItem;
            if (selitem == null) return;

            var remote = ApiHelper.Instance().GetRemoteByApi(selitem.Api);
            remote.Configure(selitem.IP, selitem.Port, selitem.Login, selitem.Password);
            remote.SystemRunning.Shutdown();
            remote.Close();
        }

        private void btn_Settings_Remotes_Edit_GetMacFromIP_Click(object sender, RoutedEventArgs e)
        {
            _remoteInfoEdit.MacAddress = NetworkTools.GetMacAddressFromArp(_remoteInfoEdit.IP);
        }

        private void btn_Settings_Remotes_Edit_Cancel_Click(object sender, RoutedEventArgs e)
        {
            grd_Settings_Remotes_Edit.Visibility = Visibility.Hidden;
            //_yatse2Properties.ShowSettingsRemotesEdit = false;
        }

        private void btn_Settings_Remotes_Edit_Accept_Click(object sender, RoutedEventArgs e)
        {
            _remoteInfoEdit.Validated = false;
            grd_Settings_Remotes_Edit.Visibility = Visibility.Visible;
            //_yatse2Properties.ShowSettingsRemotesEdit = false;

            var remote = ApiHelper.Instance().FillRemoteByComboIndex(_remoteInfoEdit, lst_Settings_Remotes_Edit_Api.SelectedIndex);

            if ((string)btn_Settings_Remotes_Edit_Accept.Content == GetLocalizedString(14))
            {
                var id = _database.InsertRemote(remote);
                if (lst_Remotes.Items.Count < 1 && id != 0)
                {
                    _config.DefaultRemote = id;
                    _config.Save(_configFile);
                    _currentRemoteId = _config.DefaultRemote;
                    InitRemote();
                }
            }
            else
            {
                _database.UpdateRemote(remote);
                if (_currentRemoteId == remote.Id)
                    InitRemote();
            }

            RefreshRemotes();
        }

        private void btn_Settings_Remotes_Edit_Verify_Click(object sender, RoutedEventArgs e)
        {
            var remote = ApiHelper.Instance().GetRemoteByComboIndex(lst_Settings_Remotes_Edit_Api.SelectedIndex);
            if (remote == null)
            {
                _remoteInfoEdit.Validated = false;
                return;
            }
            var res = remote.TestConnection(_remoteInfoEdit.IP, _remoteInfoEdit.Port, _remoteInfoEdit.Login,
                                            _remoteInfoEdit.Password);
            if ( res == 1)
            {
                _remoteInfoEdit.OS = remote.GetOS();
                _remoteInfoEdit.Version = remote.GetVersion();
                _remoteInfoEdit.Additional = remote.GetAdditionalInfo();
                if (_remoteInfoEdit.Additional != "")
                {
                    // Update Kodi Sources
                    UpdateKodiSource(_remoteInfoEdit.Additional);
                }
                _remoteInfoEdit.Validated = true;
            }
            else if (res == 0)
            {
                _remoteInfoEdit.Validated = false;
                ShowOkDialog(GetLocalizedString(16));
            }
            else
            {
                _remoteInfoEdit.Validated = false;
                ShowOkDialog(GetLocalizedString(116));
            }
        }

    }
}