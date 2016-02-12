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
using System.Reflection;
using System.Windows.Controls.Primitives;
using Plugin;
using Setup;
using FrontView.Classes;

namespace FrontView.Libs
{

    public class ApiHelper
    {
        private static ApiHelper _instance;
        private static readonly Object ClassLock = typeof(ApiHelper);

        private ApiHelper()
        {
        }

        public static ApiHelper Instance()
        {
            lock (ClassLock)
            {
                if (_instance == null)
                {
                    _instance = new ApiHelper();

                }
            }
            return _instance;
        }

        private Collection<IYatse2RemotePlugin> _remotePlugins;

        public void LoadRemotePlugins(int yatseVersion)
        {
            var path = Helper.PluginPath;
            var pluginFiles = Directory.GetFiles(path, "Remote.*.dll");
            _remotePlugins = new Collection<IYatse2RemotePlugin>();
            foreach (var plugin in pluginFiles)
            {
                var args = plugin;
                Logger.Instance().Log("FrontView-Plugs", "Loading : " + plugin, true);
                try
                {
                    var asm = Assembly.LoadFile(args);
                    if (asm != null)
                    {
                        foreach (var t in asm.GetTypes())
                        {
                            if (!typeof(IYatse2RemotePlugin).IsAssignableFrom(t)) continue;

                            var plug = (IYatse2RemotePlugin) Activator.CreateInstance(t);
                            Logger.Instance().Log("FrontView-Plugs", "Plugin : " + plug.Name + " (Version : " + plug.Version + ")", true);
                            if (!plug.IsCompatible(yatseVersion))
                            {
                                Logger.Instance().Log("FrontView-Plugs",
                                                      "Plugin : " + plug.Name + " (Version : " + plug.Version + ") reports incompatibility unloading", true);
                            }
                            else 
                                _remotePlugins.Add(plug);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Instance().Log("FrontView-Plugs", "Plugin : " + plugin + " refused to load.",true);
                    Logger.Instance().Trace("Yatse2-Plugs", ex.Message);
                }
            }
        }

        public string GetPluginHashFromFileName(string fileName, string api)
        {
            if (fileName == null || api == null)
                return "";

            foreach (var remotePlugin in _remotePlugins)
            {
                if (remotePlugin.Name == api)
                    return remotePlugin.GetHashFromFileName(fileName);
            }

            return "";
        }
    
        public bool FillApiComboBox(Selector lst,string api)
        {
            if (lst == null) return false;

            lst.Items.Clear();
            var index = 0;
            foreach (var remotePlugin in _remotePlugins)
            {
                var index2 = lst.Items.Add(remotePlugin.Name);
                if (remotePlugin.Name == api)
                    index = index2;    
            }
            lst.SelectedIndex = index;
            return true;
        }

        public ApiConnection GetRemoteByComboIndex(int index)
        {
            return _remotePlugins[index].GetRemote();
        }

        public Yatse2Remote FillRemoteByComboIndex(Yatse2Remote remote, int index)
        {
            if (remote == null)
                return remote;

            remote.Api = _remotePlugins[index].Name;
            remote.ProcessName = "NotNeeded";
            return remote;
        }


        public ApiConnection GetRemoteByApi(string api)
        {
            foreach (var remotePlugin in _remotePlugins)
            {
                if (remotePlugin.Name == api)
                    return remotePlugin.GetRemote();
            }

            return null;
        }

        public IYatse2RemotePlugin GetRemotePluginByApi(string api)
        {
            foreach (var remotePlugin in _remotePlugins)
            {
                if (remotePlugin.Name == api)
                    return remotePlugin;
            }

            return null;
        }
    }


}