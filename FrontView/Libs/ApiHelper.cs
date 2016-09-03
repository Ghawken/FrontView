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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Media.Imaging;

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


        public System.Windows.Media.Imaging.BitmapImage CoverArtTreatmentKodi(string destFile, string skin, string MovieIcons)
        {

            Bitmap Art;
            try
            {
                Art = (Bitmap)Image.FromFile(destFile);
            }
            catch (Exception ex)
            {
                Logger.Instance().LogDump("CoverArTKodi", "Exception Using Default destFile:" + destFile +ex);
                return new BitmapImage(new Uri(destFile));
            }

            Bitmap frame;
            try
            {

                string BlueRayCase = Helper.SkinorDefault(Helper.SkinPath ,skin, @"\Interface\Case_BlurayCase.png");
                string DVDCase = Helper.SkinorDefault(Helper.SkinPath, skin, @"\Interface\Case_dvdCase.png");
                string CasetoUse = "";

                if (MovieIcons.Contains("1080p") || MovieIcons.Contains("720p"))
                {
                    Logger.Instance().LogDump("CoverArTKodi", "MovieIcons Contains 1080p or 720p:" + MovieIcons);
                    CasetoUse = BlueRayCase;
                }
                else
                {
                    CasetoUse = DVDCase;
                    Logger.Instance().LogDump("CoverArTKodi", "MovieIcons DOES NOT Contains 1080p or 720p:" + MovieIcons);
                }

                Logger.Instance().LogDump("CoverArTKodi", "Trying case:" + CasetoUse);

                frame = (Bitmap)Image.FromFile(CasetoUse);
            }
            catch (Exception ex)
            {
                Logger.Instance().LogDump("CoverArTKodi", "Exception in frame Using Default destFile:"  +ex);
                return new BitmapImage(new Uri(destFile));
            }

            var width = 574;
            var height = 800;
            using (frame)
            {
                using (var bitmap = new Bitmap(width, height))
                {
                    using (var canvas = Graphics.FromImage(bitmap))
                    {
                        canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        canvas.DrawImage(Art, 0, 25, 545, 750);
                        canvas.DrawImage(frame,
                                         new Rectangle(0,
                                                       0,
                                                       width,
                                                       height),
                                         new Rectangle(0,
                                                       0,
                                                       frame.Width,
                                                       frame.Height),
                                         GraphicsUnit.Pixel);


                        canvas.Save();
                    }
                    try
                    {
                        string CasefileName = destFile.Remove(destFile.Length - 4) + "-Case.jpg";

                        if (!File.Exists(CasefileName))
                        {
                            bitmap.Save(CasefileName);
                            Logger.Instance().LogDump("CoverArTKodi", "Checking Existence of Case File Saving New:" + CasefileName);
                        }
                        else
                        {
                            Logger.Instance().LogDump("CoverArTKodi", "File Exisits:" + CasefileName);
                            return new BitmapImage(new Uri(CasefileName));
                        }
                        return new BitmapImage(new Uri(CasefileName));
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance().LogDump("CoverArTKodi", "Exception in frame Using Default destFile:" + destFile + "Exception :" + ex);
                        return new BitmapImage(new Uri(destFile));

                    }
                }
            }

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