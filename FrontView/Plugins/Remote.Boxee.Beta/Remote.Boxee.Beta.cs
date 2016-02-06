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
using System.Reflection;
using System.Runtime.InteropServices;
using Plugin;
using Remote.XBMC.Camelot.XbmcHttpApi;

namespace Remote.Boxee.Beta
{
    public class BoxeeBeta : IYatse2RemotePlugin
    {
        public BoxeeBeta()
        {
            Name = "BOXEE - Beta";
        }

        public int Version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.Build;
            }
        }

        public bool IsCompatible(int yatseVersion)
        {
            return false; // TODO : Real check against supplied version
        }

        public string Name {get;set;}

        internal static class NativeMethods
        {
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetForegroundWindow(IntPtr hWnd);
        }

        public ApiConnection GetRemote()
        {
            return new XbmcHttp();
        }

        public string GetHashFromFileName(string fileName)
        {
            var thumbparts = fileName.Split('/');
            var hash = thumbparts[thumbparts.Length - 1].Trim().Replace(".tbn", "");
            return hash;
        }

        public ApiSupportedFunctions SupportedFunctions()
        {
            return new ApiSupportedFunctions
            {
                MovieLibrary = true,
                AudioLibrary = true,
                TvShowLibrary = true,
                PictureLibrary = true
            };
        }
    }
}
