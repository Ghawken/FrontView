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
using System.IO;
using System.Xml.Serialization;

namespace Setup
{
    public class FileInfo : IEquatable<FileInfo>
    {
        public string FilePath { get; set; }
        public string FileHash { get; set; }

        public bool Equals(FileInfo other)
        {
            return (other.FileHash == FileHash && other.FilePath == FilePath);
        }

        public override string ToString()
        {
            return "(" + FileHash + ") " +FilePath ;
        }
    }

    [XmlRootAttribute("VersionFile", Namespace="", IsNullable=false)]
    public class VersionFile
    {
        public Collection<FileInfo> FileInfos { get; set; }
        public string Description { get; set; }

        public VersionFile()
        {
            FileInfos = new Collection<FileInfo>();
            Description = "";
        }

        public bool Load(string file)
        {
            VersionFile versionFile;
            try
            {
                var deserializer = new XmlSerializer(typeof(VersionFile));
                using (TextReader textReader = new StreamReader(file))
                {
                    versionFile = (VersionFile)deserializer.Deserialize(textReader);
                }
            }
            catch (Exception ex)
            {
                if (ex is IOException || ex is InvalidOperationException)
                {
                    return false;
                }
                throw;
            }

            FileInfos = versionFile.FileInfos;
            Description = versionFile.Description;

            return true;
        }

        public void Save(string file)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(VersionFile));
                using (TextWriter textWriter = new StreamWriter(file))
                {
                    serializer.Serialize(textWriter, this);
                }
            }
            catch (IOException)
            {
            }
            return;
        }
    }
}


