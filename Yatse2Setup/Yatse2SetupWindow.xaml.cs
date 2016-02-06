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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Threading;
using Setup;
using MessageBox = System.Windows.MessageBox;

namespace Yatse2Setup
{

    public class RepoInfo
    {
        public string Source { get; set; }
        public string Platform { get; set; }
        public string Repo { get; set; }
        public string Destination { get; set; }
        public VersionInfo Version { get; set; }

    }

    public static class ExtensionMethods
    {

        private static readonly Action EmptyDelegate = delegate { };


        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Background, EmptyDelegate);
        }
    }

    public partial class Yatse2SetupWindow
    {
        private const string Repository = @"http://yatse.leetzone.org/repository";
        private bool _allowBeta;

        public bool GetDlls()
        {
            var client = new WebClient();
            try
            {
                if (File.Exists(Helper.AppPath + @"\Setup.dll"))
                    File.Delete(Helper.AppPath + @"\Setup.dll");
                client.DownloadFile(Repository + "/Download/File/Setup.dll", Helper.AppPath + @"\Setup.dll");
            }
            catch (Exception )
            {
                client.Dispose();
                return false;
            }
            client.Dispose();
            return true;

        }


        private void LoadLang(string langfile)
        {
            try
            {
                var dico = new ResourceDictionary
                               {
                                   Source = new Uri("Langs/" + langfile + ".xaml", UriKind.Relative)
                               };
                Resources.MergedDictionaries.Clear();
                Resources.MergedDictionaries.Add(dico);
            }
            catch (XamlParseException)
            {
                return;
            }
        }

        public string GetLocalizedString(int id)
        {
            var ret = (string)TryFindResource("Localized_" + id);
            return ret ?? "Non localized string";
        }

        public void CheckIf64()
        {
            if (Tools.Is64BitOperatingSystem)
                btn_Installx64.IsEnabled = false;
            // TODO : Reactivate when x64 builds
        }

        public static void InitLogger()
        {
            Logger.Instance().LogFile= Helper.AppPath + @"\Setup.log";
            Logger.Instance().RotateLogFile();

        }

        public Yatse2SetupWindow()
        {
            InitializeComponent();

            if (!GetDlls())
            {
                MessageBox.Show("Error : Yatse WebSite down, please try again later.");
                Close();
            }     

            btn_Launch.Visibility = Visibility.Hidden;

            _allowBeta = File.Exists(txb_TargetDirectory.Text + @"\Yatse2.beta");

            cb_Language.Items.Add("English");
            cb_Language.Items.Add("Français");
            cb_Language.Items.Add("Dutch");
            cb_Language.Items.Add("Norwegian");
            cb_Language.Items.Add("Russian");
            cb_Language.Items.Add("Spanish");
            cb_Language.Items.Add("Croatian");
            cb_Language.SelectedIndex = 0;

            if (File.Exists(Helper.AppPath + @"Yatse2.exe "))
                txb_TargetDirectory.Text = txb_SourceDirectory.Text = Helper.AppPath.TrimEnd('\\');

            if (File.Exists(Helper.AppPath.Replace(@"\Temp", "") + @"Yatse2.exe"))
            {
                txb_TargetDirectory.Text = txb_SourceDirectory.Text = Helper.AppPath.Replace(@"\Temp\", "");
            }
            InitLogger();
            CheckIf64();
            CheckIfUpdate();
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
        	Close();
        }

        private static void CreateDesktopShortcut(string path)
        {
            /*var key = Microsoft.Win32.Registry.LocalMachine;
            key = key.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Folders");
            if (key == null) return;
            var deskDir = key.GetValue("Common Desktop").ToString();*/
            var deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            if (File.Exists(deskDir + @"\Yatse2.lnk"))
                File.Delete(deskDir + @"\Yatse2.lnk");
            using (var shortcut = new ShellLink())
            {
                shortcut.Target = path + @"\Yatse2.exe";
                shortcut.WorkingDirectory = Path.GetDirectoryName(path + @"\Yatse2.exe");
                shortcut.Description = "Yatse2";
                shortcut.DisplayMode = ShellLink.LinkDisplayMode.EdmNormal;
                shortcut.Save(deskDir + @"\Yatse2.lnk");
            }
        }

        private void backgroundWorker_InstallCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            txb_InstallInfos.Text = !(bool)e.Result ? GetLocalizedString(8) : GetLocalizedString(9);
            if ((bool)e.Result)
            {
                btn_Launch.Visibility = Visibility.Visible;
                prg_progress.Visibility = Visibility.Hidden;
                if (chk_Install_CreateShortcut.IsChecked == true)
                    CreateDesktopShortcut(txb_TargetDirectory.Text);
            }
            btn_Cancel.IsEnabled = true;
        }

        private static void backgroundWorker_Install(object sender, DoWorkEventArgs e)
        {
            var updateInfo = (RepoInfo)e.Argument;

            var repo = new RemoteRepository();
            repo.LoadRepository(updateInfo.Repo, updateInfo.Platform, Helper.AppPath + "Updates");
            var result = repo.Install(updateInfo.Version, updateInfo.Destination); 

            repo.CleanTemporary();
            e.Result = result;

        } 

        private void Install(string platform)
        {
            var repo = new RemoteRepository();
            repo.LoadRepository(Repository, platform, Helper.AppPath + "Updates");
            _allowBeta = File.Exists(txb_TargetDirectory.Text + @"\Yatse2.beta");
            var versions = repo.GetBuildList(_allowBeta);
            if (versions == null)
            {
                MessageBox.Show(GetLocalizedString(10));
                repo.CleanTemporary();
                return;
            }
            if (versions.Version.Count < 1)
            {
                MessageBox.Show(GetLocalizedString(10));
                repo.CleanTemporary();
                return;
            }
            var lastBuild = versions.Version[versions.Version.Count - 1];

            txb_InstallInfos.Text = GetLocalizedString(6) + " " + lastBuild.Build + " (" + platform + ")\n\n" + GetLocalizedString(7);
            brd_InstallInfos.Visibility = Visibility.Visible;
            brd_Install.Visibility = Visibility.Hidden;
            brd_BuildlInfos.Visibility = Visibility.Hidden;
            btn_Cancel.IsEnabled = false;


            var updateInfo = new RepoInfo
            {
                Repo = Repository,
                Platform = platform,
                Source = txb_SourceDirectory.Text,
                Destination = txb_TargetDirectory.Text,
                Version = lastBuild
            };

            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += backgroundWorker_Install;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_InstallCompleted;

            backgroundWorker.RunWorkerAsync(updateInfo);

        }

        private void btn_Installx86_Click(object sender, RoutedEventArgs e)
        {
            Install("x86");
        }

        private void btn_Installx64_Click(object sender, RoutedEventArgs e)
        {
            Install("x64");
        }

        private void cb_Language_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        	LoadLang(cb_Language.SelectedItem.ToString());
        }

        private void btn_Browse_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = txb_TargetDirectory.Text;
                var result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    txb_TargetDirectory.Text = dialog.SelectedPath;
                    txb_SourceDirectory.Text = dialog.SelectedPath;
                    CheckIfUpdate();
                }
            }

        }

        private void CheckIfUpdate()
        {
            if (File.Exists(txb_TargetDirectory.Text + @"\Yatse2.exe") || File.Exists(txb_SourceDirectory.Text + @"\Yatse2.exe"))
            {
                brd_Install.Visibility = Visibility.Hidden;
                brd_Update.Visibility = Visibility.Visible;
                brd_InstallInfos.Visibility = Visibility.Hidden;
                brd_BuildlInfos.Visibility = Visibility.Hidden;
            }
            else
            {
                brd_Install.Visibility = Visibility.Visible;
                brd_Update.Visibility = Visibility.Hidden;
                brd_InstallInfos.Visibility = Visibility.Hidden;
                brd_BuildlInfos.Visibility = Visibility.Hidden;
            }



        }

        private void btn_Update_Click(object sender, RoutedEventArgs e)
        {
        	if (!File.Exists(txb_SourceDirectory.Text + @"\Yatse2.exe"))
        	{
        	    CheckIfUpdate();
                return;
        	}

            _allowBeta = File.Exists(txb_SourceDirectory.Text + @"\Yatse2.beta");
            var platform = "x86";
            if (Tools.IsFile64Bit(txb_SourceDirectory.Text + @"\Yatse2.exe") == true)
                platform = "x64";

            var repo = new RemoteRepository();
            repo.SetDebug(true);
            repo.LoadRepository(Repository, platform, Helper.AppPath + "Updates");

            var versions = repo.GetBuildList(_allowBeta);
            if (versions == null)
            {
                MessageBox.Show(GetLocalizedString(10));
                repo.CleanTemporary();
                return;
            }
            if (versions.Version.Count < 1)
            {
                MessageBox.Show(GetLocalizedString(10));
                repo.CleanTemporary();
                return;
            }
            var lastBuild = versions.Version[versions.Version.Count - 1];

            var version = Tools.GetFileRevision(txb_SourceDirectory.Text + @"\Yatse2.exe");
            if (version == lastBuild.Build)
            {
                MessageBox.Show(GetLocalizedString(12));
                repo.CleanTemporary();
                return;
            }

            var buildinfo = repo.GetVersionInfo(lastBuild);

            if (buildinfo == null)
            {
                MessageBox.Show(GetLocalizedString(14));
                repo.CleanTemporary();
            }
            else
            {
                txb_BuildInfos.Text = buildinfo.Description;
                brd_Update.Visibility = Visibility.Hidden;
                brd_BuildlInfos.Visibility = Visibility.Visible;
            }
        }

        private void backgroundWorker_UpdateCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            txb_InstallInfos.Text = !(bool)e.Result ? GetLocalizedString(8) : GetLocalizedString(9);
            if ((bool)e.Result)
            {
                prg_progress.Visibility = Visibility.Hidden;
                btn_Launch.Visibility = Visibility.Visible;
            }
            btn_Cancel.IsEnabled = true;
            
        }

        private static void backgroundWorker_Update(object sender, DoWorkEventArgs e)
        {
            var updateInfo = (RepoInfo) e.Argument;

            var repo = new RemoteRepository();
            repo.LoadRepository(updateInfo.Repo, updateInfo.Platform, Helper.AppPath + "Updates");
            var result = repo.Update(updateInfo.Destination, updateInfo.Version);
            repo.CleanTemporary();
            e.Result = result;

        } 

        private void btn_GoUpdate_Click(object sender, RoutedEventArgs e)
        {
 
            var platform = "x86";
            if (Tools.IsFile64Bit(txb_SourceDirectory.Text + @"\Yatse2.exe") == true)
                platform = "x64";
            var repo = new RemoteRepository();

            repo.LoadRepository(Repository, platform, Helper.AppPath + "Updates");

            var versions = repo.GetBuildList(_allowBeta);
            if (versions == null)
            {
                MessageBox.Show(GetLocalizedString(10));
                repo.CleanTemporary();
                return;
            }
            if (versions.Version.Count < 1)
            {
                MessageBox.Show(GetLocalizedString(10));
                repo.CleanTemporary();
                return;
            }
            var lastBuild = versions.Version[versions.Version.Count - 1];

            brd_Update.Visibility = Visibility.Hidden;
            brd_BuildlInfos.Visibility = Visibility.Hidden;
            brd_InstallInfos.Visibility = Visibility.Visible;
            btn_Cancel.IsEnabled = false;
            txb_InstallInfos.Text = GetLocalizedString(6) + " " + lastBuild.Build + "\n\n" + GetLocalizedString(7);

            var updateInfo = new RepoInfo
                                 {
                                     Repo = Repository,
                                     Platform = platform,
                                     Source = txb_SourceDirectory.Text,
                                     Destination = txb_TargetDirectory.Text,
                                     Version = lastBuild
                                 };

            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += backgroundWorker_Update;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_UpdateCompleted;

            backgroundWorker.RunWorkerAsync(updateInfo);

        }

        private void btn_Launch_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(txb_TargetDirectory.Text + @"\Yatse2.exe");
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	DragMove();
        }

        private void Image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            const string url = @"https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=BBUWT92HT3XUG";
            Process.Start(new ProcessStartInfo(url));
        }
    }
}

