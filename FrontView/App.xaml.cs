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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using Setup;
using FrontView.Libs;
using System.Xml.Serialization;

[assembly: CLSCompliant(false)]
namespace FrontView
{
    public partial class App
    {
        public readonly FrontView.Classes.FrontViewConfig Appconfig = new FrontView.Classes.FrontViewConfig();   

        static Assembly AssemblyLoader(object sender, ResolveEventArgs args)
        {
            var system = Helper.SystemPath + args.Name.Substring(0, args.Name.IndexOf(",",StringComparison.OrdinalIgnoreCase)) + ".dll";
            if (File.Exists(system))
            {
                var myAssembly = Assembly.LoadFrom(system);
                return myAssembly;
            }

            var plugin = Helper.PluginPath + args.Name.Substring(0, args.Name.IndexOf(",", StringComparison.OrdinalIgnoreCase)) + ".dll";
            if (File.Exists(plugin))
            {
                var myAssembly = Assembly.LoadFrom(plugin);
                return myAssembly;
            }
            return null;
        }

        private static void InitLog()
        {
            Logger.Instance().LogFile = Helper.LogPath + "FrontView+.log";
            Logger.Instance().RotateLogFile();
        }
        protected override void OnStartup(StartupEventArgs e)
        {


            Current.DispatcherUnhandledException += AppDispatcherUnhandledException;
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(
                CultureInfo.CurrentCulture.IetfLanguageTag)));

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += AssemblyLoader;

            InitLog();

            base.OnStartup(e);
        }

        static void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Instance().LogException("YatseApp", e.Exception );
            e.Handled = true;
        }

        public void Application_Exit(object sender, ExitEventArgs e)
        {
           Logger.Instance().Log("FrontViewApp", "EXIT CALLED", true);
           //Yatse2Window.ni.Icon;
            

            FrontView.Classes.FrontViewConfig config;            



           string configFile = (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\FrontView+\settings.xml");
           try
           {
               var deserializer = new XmlSerializer(typeof(FrontView.Classes.FrontViewConfig));
               using (TextReader textReader = new StreamReader(configFile))
               {
                   config = (FrontView.Classes.FrontViewConfig)deserializer.Deserialize(textReader);
               }
           }
           catch (Exception ex)
           {
               if (ex is IOException || ex is InvalidOperationException)
               {
                   //Logger.Instance().Log("FrontViewConfig", "Error loading settings : " + ex.Message);
                   return;
               }
               throw;
           }
            // Yatse2.Classes.Yatse2Config Appconfig;
            //Yatse2.Classes.Yatse2Config _config = Yatse2.Classes.Yatse2Config();
          
            Logger.Instance().LogDump("FrontViewAPP EXIT", config);

            if (config.HttpSend == true && config.HttpPoweroff != "")
            {
                HttpSend.HttpsendgotoHttpsimple(config, config.HttpPoweroff);
            }
                //Yatse2Window.gotoHttpsimple(config.HttpPoweroff);

        }
    }
}
