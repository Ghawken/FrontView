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

    public class VersionInfo : IEquatable<VersionInfo>

    {
        public int Build { get; set; }
        public bool Beta { get; set; }

        public bool Equals(VersionInfo other)
        {
            return (other.Build == Build && other.Beta == Beta);
        }

        public override string ToString()
        {
            return Build + (Beta ? " (Beta)" : "") ;
        }

    }


    [XmlRootAttribute("Versions", Namespace="", IsNullable=false)]
    public class VersionList
    {
        public Collection<VersionInfo> Version { get; set; }

        public VersionList()
        {
            Version = new Collection<VersionInfo>();
        }

        public bool Load(string file)
        {
            VersionList versionList;
            try
            {
                var deserializer = new XmlSerializer(typeof(VersionList));
                using (TextReader textReader = new StreamReader(file))
                {
                    versionList = (VersionList)deserializer.Deserialize(textReader);
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

            Version = versionList.Version;

            return true;
        }

        public void Save(string file)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(VersionList));
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


