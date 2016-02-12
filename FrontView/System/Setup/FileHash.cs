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
using System.Security.Cryptography;
using System.Text;

namespace Setup
{
    public class FileHash
    {
        public static string GetBase64FileHash(string path)
        {
            return Convert.ToBase64String(GetFileHash(path));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static byte[] GetFileHash(string path)
        {
            if (!File.Exists(path))
                return null;
            var md5Hasher = MD5.Create();
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                return md5Hasher.ComputeHash(fs);
        }

        public static string GetFileHashString(string path)
        {
            var result = GetFileHash(path);
            var sBuilder = new StringBuilder();
            for (var i = 0; i < result.Length; i++)
            {
                sBuilder.Append(result[i].ToString("X2"));
            }
            return sBuilder.ToString();
        }

        public static bool CheckFileHashString(string path, string hash)
        {
            var hashOfInput = GetFileHashString(path);
            var comparer = StringComparer.OrdinalIgnoreCase;
            return 0 == comparer.Compare(hashOfInput, hash);
        }

    }
}
