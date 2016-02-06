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

using System.IO;
using System.Windows;
using Setup;

namespace Yatse2SetupManager
{

    public partial class MainWindow
    {
        private bool _repoLoaded;
        private readonly LocalRepository _repository = new LocalRepository();

        public MainWindow()
        {
            InitializeComponent();

            txb_Repository.Text = @"F:\_DevZone\DotNET\_Repository";
            txb_SourceDir.Text = @"F:\_DevZone\DotNET\_YatseSVN\Yatse2\bin\x86\Release";

        }

        private void cb_Platform_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _repository.SetPlatform(cb_Platform.SelectedItem.ToString());
            var liste = _repository.GetBuildList();
            lst_Versions.Items.Clear();
            foreach (var version in liste.Version)
            {
                lst_Versions.Items.Add(version);
            }
        }

        private void btn_LoadRepository_Click(object sender, RoutedEventArgs e)
        {
            if (!_repository.LoadRepository(txb_Repository.Text))
        	{
        	    MessageBox.Show("Error invalid repository");
        	    _repoLoaded = false;
                return;
        	}

            _repoLoaded = true;
            cb_Platform.Items.Clear();
            cb_Platform.Items.Add("x64");
            cb_Platform.Items.Add("x86");
        }


        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (lst_Versions.SelectedItem == null)
                return;

            _repository.RemoveFromRepository((VersionInfo) lst_Versions.SelectedItem);

            cb_Platform_SelectionChanged(null, null);
            lst_Versions_SelectionChanged(null, null);
        }


        private void btn_AddVersion_Click(object sender, RoutedEventArgs e)
        {
        	if (!File.Exists(txb_SourceDir.Text + @"\Yatse2.exe"))
        	{
        	    MessageBox.Show("Invalid source directory");
                return;
        	}

            if (!_repoLoaded)
            {
                MessageBox.Show("Please load repository before");
                return;
            }

            if (cb_Platform.SelectedItem == null)
            {
                MessageBox.Show("Please choose the target platform");
                return;
            }

            var build = Tools.GetFileRevision(txb_SourceDir.Text + @"\Yatse2.exe");
            var res = MessageBox.Show("Will add build : "  + build + " to repository : " + cb_Platform.SelectedItem,"Yatse2SetupManager",MessageBoxButton.YesNo);
            if (res == MessageBoxResult.No)
                return;

            var result = _repository.AddToRepository( txb_SourceDir.Text, false, txb_WhatsNew.Text);
            if (result != null)
            {
                MessageBox.Show(result);
            }
            
            cb_Platform_SelectionChanged(null, null);
            lst_Versions_SelectionChanged(null, null);
        }



        private void lst_Versions_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lst_Versions.SelectedItem == null)
            {
                txb_WhatsNew.Text = "";
                lst_FileList.Items.Clear();
                return;

            }

            var fileinfo = _repository.GetVersionInfo((VersionInfo) lst_Versions.SelectedItem);
            if (fileinfo == null)
                return;
            txb_WhatsNew.Text = fileinfo.Description;
            lst_FileList.Items.Clear();
            foreach (var info in fileinfo.FileInfos)
            {
                lst_FileList.Items.Add(info);
            }
        }

        private void btn_UpdateNews_Click(object sender, RoutedEventArgs e)
        {
            var result = _repository.UpdateVersionDescription ((VersionInfo) lst_Versions.SelectedItem, txb_WhatsNew.Text);
            if (!result)
            {
                MessageBox.Show("Error while updating");
            }

        }

        private void btn_AddBeta_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(txb_SourceDir.Text + @"\Yatse2.exe"))
            {
                MessageBox.Show("Invalid source directory");
                return;
            }

            if (!_repoLoaded)
            {
                MessageBox.Show("Please load repository before");
                return;
            }

            if (cb_Platform.SelectedItem == null)
            {
                MessageBox.Show("Please choose the target platform");
                return;
            }

            var build = Tools.GetFileRevision(txb_SourceDir.Text + @"\Yatse2.exe");
            var res = MessageBox.Show("Will add build : " + build + " BETA ! to repository : " + cb_Platform.SelectedItem, "Yatse2SetupManager", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.No)
                return;

            var result = _repository.AddToRepository( txb_SourceDir.Text, true, txb_WhatsNew.Text);
            if (result != null)
            {
                MessageBox.Show(result);
            }

            cb_Platform_SelectionChanged(null, null);
            lst_Versions_SelectionChanged(null, null);
        }
    }
}
