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

using System.Reflection;
using Plugin;

namespace Remote.XBMC.DharmaHttp
{
    public class XbmcDharma : IYatse2RemotePlugin
    {
        public XbmcDharma()
        {
            Name = "XBMC - Dharma H";
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
            return false;
        }

        public string Name {get;set;}

        public ApiConnection GetRemote()
        {
            return null;
        }

        public ApiSupportedFunctions SupportedFunctions()
        {
            return new ApiSupportedFunctions
            {
                MovieLibrary = false,
                AudioLibrary = false,
                TvShowLibrary = false,
                PictureLibrary = false
            };
        }

        public string GetHashFromFileName(string fileName)
        {
            return "";
        }
    }
}
