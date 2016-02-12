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

using System.Collections.ObjectModel;
using System.IO;

namespace Setup
{

    public class LocalRepository
    {
        private bool _loaded;
        private bool _platformSet;
        private string _repoDir;
        private string _platformDir;
        private string _platform;

        public bool LoadRepository(string repository)
        {
            _loaded = false;
            if (!Directory.Exists(repository + @"\x86"))
                return false;
            if (!Directory.Exists(repository + @"\x64"))
                return false;
            _repoDir = repository;
            _loaded = true;
            _platformSet = false;
            return true;
        }

        public bool SetPlatform(string platform)
        {
            if (!_loaded)
                return false;
            /*if (!File.Exists(_repoDir + @"\xmls\Versions_" + platform + ".xml"))
                return false;*/
            _platformDir = _repoDir + @"\" + platform;
            _platform = platform;
            _platformSet = true;
            return true;
        }

        private bool Configured()
        {
            return _loaded && _platformSet;
        }

        public VersionFile GetVersionInfo(VersionInfo build)
        {
            if (!Configured())
                return null;
            if (!File.Exists(_repoDir + @"\xmls\Version_" + _platform + "_" + build.Build + ".xml"))
                return null;

            var fileinfo = new VersionFile();
            fileinfo.Load(_repoDir + @"\xmls\Version_" + _platform + "_" + build.Build + ".xml");
            return fileinfo;
        }

        public bool UpdateVersionDescription(VersionInfo build, string description)
        {
            if (!Configured())
                return false;
            if (!File.Exists(_repoDir + @"\xmls\Version_" + _platform + "_" + build.Build + ".xml"))
                return false;

            var fileinfo = new VersionFile();
            fileinfo.Load(_repoDir + @"\xmls\Version_" + _platform + "_" + build.Build + ".xml");
            fileinfo.Description = description;
            fileinfo.Save(_repoDir + @"\xmls\Version_" + _platform + "_" + build.Build + ".xml");

            return true;
        }

        public VersionList GetBuildList()
        {
            if (!Configured())
                return null;

            var liste = new VersionList();
            liste.Load(_repoDir + @"\xmls\Versions_" + _platform + ".xml");
            return liste;
        }

        public bool RemoveFromRepository(VersionInfo build)
        {
            if (!Configured())
                return false;

            if (File.Exists(_repoDir + @"\xmls\Version_" + _platform + "_" + build.Build + ".xml"))
            {
                Directory.Delete(_platformDir + @"\" + build.Build, true);
                File.Delete(_repoDir + @"\xmls\Version_" + _platform + "_" + build.Build + ".xml");
            }

            var versions = new VersionList();
            versions.Load(_repoDir + @"\xmls\Versions_" + _platform + ".xml");
            versions.Version.Remove(build);
            versions.Save(_repoDir + @"\xmls\Versions_" + _platform + ".xml");

            return true;
        }

        public string AddToRepository(string sourceDir, bool beta, string description)
        {
            if (!Configured())
                return "Error : Not configured";

            var build = Tools.GetFileRevision(sourceDir + @"\Yatse2.exe");
            if (build == 0)
                return "Error : Invalid source dir";

            if (Directory.Exists(_platformDir + @"\" + build))
            {
                return "Error : Build allready in repository";
            }

            Directory.CreateDirectory(_platformDir + @"\" + build);
            Directory.CreateDirectory(_platformDir + @"\" + build + @"\Files");

            var fileinfos = new Collection<FileInfo>();
            var files = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                if (file.Contains(".pdb") || file.Contains(".vshost.") || file.Contains(".application"))
                    continue;
                var fileinfo = new FileInfo
                {
                    FilePath = file.Replace(sourceDir, ""),
                    FileHash = FileHash.GetFileHashString(file)
                };
                if (!File.Exists(_platformDir + @"\" + build + @"\Files\" +
                              fileinfo.FileHash + ".dat"))
                {
                    File.Copy(file, _platformDir + @"\" + build + @"\Files\" +
                              fileinfo.FileHash + ".dat");
                }
                fileinfos.Add(fileinfo);
            }

            var versionFile = new VersionFile { Description = description, FileInfos = fileinfos };
            versionFile.Save(_repoDir + @"\xmls\Version_" + _platform + "_" + build + ".xml");

            var liste = new VersionList();
            liste.Load(_repoDir + @"\xmls\Versions_" + _platform + ".xml");
            liste.Version.Add(new VersionInfo { Beta = beta, Build = build });
            liste.Save(_repoDir + @"\xmls\Versions_" + _platform + ".xml");

            return null;
        }



    }
}