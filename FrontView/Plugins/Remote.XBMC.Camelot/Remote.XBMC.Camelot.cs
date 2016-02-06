﻿// ------------------------------------------------------------------------
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
using Plugin;
using Remote.XBMC.Camelot.Api;

[assembly: CLSCompliant(false)]


namespace Remote.XBMC.Camelot
{
    public class XbmcCamelot : IYatse2RemotePlugin
    {
        public XbmcCamelot()
        {
            Name = "XBMC - Camelot";
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
            return true; // TODO : Real check against supplied version
        }

        public string Name {get;set;}

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

        public ApiConnection GetRemote()
        {
            return new Xbmc();
        }

        public string GetHashFromFileName(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                return "";
            var thumbparts = fileName.Split('/');
            var hash = thumbparts[thumbparts.Length - 1].Trim().Replace(".tbn", "");
            return hash;
        }
    }
}
