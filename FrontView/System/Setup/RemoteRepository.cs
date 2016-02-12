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
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;

namespace Setup
{
    public class RemoteRepository
    {
        private bool _loaded;
        private string _tempDirectory;
        private string _platformUrl;
        private string _platform;
        private string _repository;
        //private readonly WebClient _client = new WebClient();
        private const int Threads = 4;
        private bool _debug;

        private bool DownloadFile(string file, string destname)
        {
            if (File.Exists(_tempDirectory + @"\" + destname))
                return true;
            var client = new WebClient();
            try
            {
                client.DownloadFile(file, _tempDirectory + @"\" + destname);
                client.Dispose();
            }
            catch (Exception e)
            {
                client.Dispose();
                Log("Download : Error (" + e.Message + ") : " + file.Replace(_repository, ""), true);
                return false;
            }
            Log("Download : " + file.Replace(_repository,""));
            return true;
        }

        public bool CleanTemporary()
        {
            try 
            {
                Directory.Delete(_tempDirectory, true);
                Log("Cleanup : Temporary deleted ");
                //_client.Dispose();
            }
            catch(Exception)
            {
                Log("Cleanup : Error deleting temporary",true);
                return false;
            }
            return true;
        }

        private void Log(string message)
        {
            Logger.Instance().Log("Setup", message, _debug);
        }

        private static void Log(string message, bool force)
        {
            Logger.Instance().Log("Setup", message, force);
        }

        public void SetDebug(bool debug)
        {
            _debug = debug;
        }

        public bool LoadRepository(string repository, string plateform, string tempDirectory)
        {
            _loaded = false;
            if (String.IsNullOrEmpty(Logger.Instance().LogFile))
            {
                var currentAssemblyDirectoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                Logger.Instance().LogFile = currentAssemblyDirectoryName + @"\Setup.log";
                Logger.Instance().Debug = _debug;
            }

            if (!Directory.Exists(tempDirectory))
            {
                try
                {
                    Directory.CreateDirectory(tempDirectory);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            var plateformUrl = repository + @"/" + plateform;
            _tempDirectory = tempDirectory;
            _platformUrl = plateformUrl;
            _platform = plateform;
            _repository = repository;

            if (DownloadFile(repository + "/Download/" + _platform + "/Versions" , "Versions_" + _platform + ".xml"))
            {
                _loaded = true;
                Log("Repository loaded  - " + plateform);
                return true;
            }
            Log("Error loading repository - " + plateform, true);
            return false;
        }

        public bool UpdateTranslations(string langDir)
        {
            if (!DownloadFile(_repository + "/Download/Translations", "Translations.xml"))
                return false;

            var xmldata = new XmlDocument();

            var xml = File.ReadAllText(_tempDirectory + @"\Translations.xml");
            if (string.IsNullOrEmpty(xml))
                return false;
            xmldata.LoadXml(xml);
            var elements = xmldata.SelectNodes("//langs/lang");
            if (elements == null)
            {
                Log("Error downloading translation list" , true);
                return false;
            }
            var update = false;
            var langVersion = new Regex(@"Version:(\d+)");
            using (var client = new WebClient())
            {
                foreach (XmlElement element in elements)
                {
                    var file = element.InnerText;
                    file = file.Replace("/langs/", "/Download/Translation/");
                        // TODO : Remove when all users are updated to more recent builds.
                    if (File.Exists(langDir + @"\" + element.GetAttribute("name") + @".xaml"))
                    {
                        var data = File.ReadAllText(langDir + @"\" + element.GetAttribute("name") + @".xaml");
                        var m = langVersion.Match(data);
                        if (m.Success)
                        {
                            if (Convert.ToInt32(m.Groups[1].ToString()) <
                                Convert.ToInt32(element.GetAttribute("version")))
                            {
                                try
                                {
                                    Log("Updating translation " + element.GetAttribute("name") + " from " + m.Groups[1] +
                                        " to " + element.GetAttribute("version"));
                                    if (File.Exists(langDir + @"\" + element.GetAttribute("name") + ".xaml"))
                                    {
                                        File.Delete(langDir + @"\" + element.GetAttribute("name") + ".bck");
                                        File.Move(langDir + @"\" + element.GetAttribute("name") + ".xaml",
                                                  langDir + @"\" + element.GetAttribute("name") + ".bck");
                                    }
                                    client.DownloadFile(_repository + file,
                                                        langDir + @"\" + element.GetAttribute("name") + ".xaml");
                                    update = true;
                                }
                                catch (WebException ex)
                                {
                                    Log(
                                        "Error downloading translation : " + element.GetAttribute("name") + ": " +
                                        ex.Message, true);
                                    continue;
                                }
                            }
                        }
                        else
                            continue;
                    }
                    else
                    {
                        try
                        {
                            Log("Downloading translation " + element.GetAttribute("name") + " version " +
                                element.GetAttribute("version"));
                            if (File.Exists(langDir + @"\" + element.GetAttribute("name") + ".xaml"))
                            {
                                File.Delete(langDir + @"\" + element.GetAttribute("name") + ".bck");
                                File.Move(langDir + @"\" + element.GetAttribute("name") + ".xaml",
                                          langDir + @"\" + element.GetAttribute("name") + ".bck");
                            }
                            client.DownloadFile(_repository + file,
                                                langDir + @"\" + element.GetAttribute("name") + ".xaml");
                            update = true;
                        }
                        catch (WebException ex)
                        {
                            Log("Error downloading translation : " + element.GetAttribute("name") + ": " + ex.Message,
                                true);
                            continue;
                        }
                    }
                }
            }
            return update;
        }


        public VersionInfo CheckUpdate(string source, bool beta)
        {
            if (!File.Exists(source + @"\Yatse2.exe"))
            {
                Log("Check Update : Missing source file Yatse2.exe",true);
                return null;
            }

            var version = Tools.GetFileRevision(source + @"\Yatse2.exe");
            Log("Check Update : Current build  : " + version,true);
            var liste = GetBuildList(beta);
            var last = liste.Version[liste.Version.Count - 1];
            if (last.Build > version)
            {
                Log("Check Update : New build found  : " + last.Build,true);
                return last;
            }

            return null;

        }

        public VersionList GetBuildList(bool beta)
        {
            if (!Configured())
                return null;

            var liste = new VersionList();
            var liste2 = new VersionList();
            liste.Load(_tempDirectory + @"\Versions_" + _platform + ".xml");
            foreach (var build in liste.Version)
            {
                if (build.Beta && beta )
                    liste2.Version.Add(build);
                else
                    if (!build.Beta)
                        liste2.Version.Add(build);
            }
            Log("Get build list : Builds founds  : " + liste2.Version.Count);
            return liste2;
        }

        private bool Configured()
        {
            return _loaded;
        }

        public VersionFile GetVersionInfo(VersionInfo build)
        {
            if (!Configured())
                return null;

            if (!DownloadFile(_repository + @"/Download/" + _platform + @"/" + build.Build + @"/Version" , "Version_" + _platform + "_" + build.Build + ".xml"))
                return null;

            var fileinfo = new VersionFile();
            fileinfo.Load(_tempDirectory + @"\Version_" + _platform + "_" + build.Build + ".xml");
            Log("Get Version Info : Build : " + build.Build);
            return fileinfo;
        }

        public bool Install(VersionInfo build,string targetDirectory)
        {
            var filesinfo = GetVersionInfo(build);

            Log("Install : Build " + build.Build + " to : " + targetDirectory,true);
            DownloadFile(_repository + "/Download/" + _platform + "/" + build.Build + "/Install", "Install");
            bool res;
            using (var q = new ThreadedDownlads(Threads, _tempDirectory, false))
            {
                foreach (var files in filesinfo.FileInfos)
                {
                    var info = new DownloadFileInfo
                                   {
                                       FileSource =
                                           _platformUrl + @"/" + build.Build + @"/Files/" + files.FileHash + ".dat",
                                       FileDestination = targetDirectory + files.FilePath,
                                       FileHash = files.FileHash
                                   };
                    q.EnqueueTask(info);
                }
                res = q.WaitEnd();
            }
            return res;
        }

        public bool Update(string directory, VersionInfo build)
        {
            
            if (!File.Exists(directory + @"\Yatse2.exe"))
                return false;

            Tools.FindAndKillProcess("Yatse2");

            var version = Tools.GetFileRevision(directory + @"\Yatse2.exe");
            if (version == build.Build)
            {
                return false;
            }

            var filesinfo = GetVersionInfo(new VersionInfo { Beta = true, Build = version });
            if (filesinfo != null)
            {
                Log("Update : Checking current install against web build : " + version);
                foreach (var files in filesinfo.FileInfos)
                {
                    if (!File.Exists(directory + files.FilePath))
                        continue;
                    if (FileHash.CheckFileHashString(directory + files.FilePath, files.FileHash)) continue;
                    if (File.Exists(directory + files.FilePath + ".bck"))
                        File.Delete(directory + files.FilePath + ".bck");
                    File.Move(directory + files.FilePath, directory + files.FilePath + ".bck");
                    Log("Update : Backup modified file : " + files.FilePath,true);
                }
            }
            else
            {
                Log("Update : Current build no more on server : " + version,true);
            }

            DownloadFile(_repository + "/Download/" + _platform + "/" + build.Build + "/Update", "Update");
            Log("Update : Updating to build : " + build.Build, true);
            filesinfo = GetVersionInfo(build);
            if (filesinfo != null)
            {
                bool res;
                using (var q = new ThreadedDownlads(Threads, _tempDirectory, false))
                {
                    foreach (var files in filesinfo.FileInfos)
                    {
                        var info = new DownloadFileInfo
                                       {
                                           FileSource =
                                               _platformUrl + @"/" + build.Build + @"/Files/" + files.FileHash + ".dat",
                                           FileDestination = directory + files.FilePath,
                                           FileHash = files.FileHash
                                       };
                        q.EnqueueTask(info);
                    }
                    res = q.WaitEnd();
                }
                return res;
            }

            Log("Update : New build no more on server : " + build.Build,true);
            return false;
        }
    }
}