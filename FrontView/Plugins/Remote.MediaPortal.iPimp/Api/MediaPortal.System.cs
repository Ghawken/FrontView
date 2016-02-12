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

using Plugin;

namespace Remote.MediaPortal.iPimp.Api
{
    class MediaPortalSystem : IApiSystem
    {
        private readonly MediaPortal _parent;

        public MediaPortalSystem(MediaPortal parent)
        {
            _parent = parent;
        }

        public void Quit()
        {
            _parent.AsyncIPimpCommand(new CommandInfoIPimp { Action = "poweroption", Value = "5" });
        }

        public void Shutdown()
        {
            _parent.AsyncIPimpCommand(new CommandInfoIPimp { Action = "poweroption", Value = "4" });
        }

        public void Reboot()
        {
            _parent.AsyncIPimpCommand(new CommandInfoIPimp { Action = "poweroption", Value = "3" });
        }

    }
}
