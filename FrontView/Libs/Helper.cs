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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using FluidKit.Controls;

namespace FrontView.Libs
{
    public class Helper
    {

        private static readonly Helper TheInstance = new Helper();
        private Helper() { }
       
        public static Helper Instance
        {
            get { return TheInstance; }
        }

        public string CurrentSkin { get; set; }
        public string CurrentApi { get; set; }


        internal static class NativeMethods
        {
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetForegroundWindow(IntPtr hWnd);
        }

        public static string SkinPath
        {
            get
            {
                var appLogPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var dataFolder = Path.Combine(appLogPath, "FrontView+");
                return dataFolder + @"\Skin\";
            }
        }

        public static string LangPath
        {
            get
            {
                return AppPath + @"Langs\";
            }
        }

        public static string CachePath
        {
            get
            {
                var appLogPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var dataFolder = Path.Combine(appLogPath, "FrontView+");
                return dataFolder + @"\Cache\";
                
               
            }
        }

        public static string LogPath
        {
            get
            {
                var appLogPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var dataFolder = Path.Combine(appLogPath,"FrontView+");
                return dataFolder + @"\Log\";
                
               
            }
        }

        public static string PluginPath
        {
            get
            {
                return AppPath + @"Plugins\";
            }
        }

        public static string SystemPath
        {
            get
            {
                return AppPath + @"System\";
            }
        }

        private static string _appPath = "";

        public static string AppPath
        {
            get
            {
                if (String.IsNullOrEmpty(_appPath))
                {
                    var appPath = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
                    if (appPath != null)
                    {
                        if (!appPath.EndsWith(@"\", StringComparison.OrdinalIgnoreCase)) 
                            appPath += @"\";
                        _appPath = appPath;
                    }
                }
                return _appPath;
            }
        }

        public static void GiveFocus(string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            foreach (var pFoundWindow in processes.Select(p => p.MainWindowHandle))
            {
                NativeMethods.SetForegroundWindow(pFoundWindow);
            }
        }

        public static bool VirtFlowSelect(DependencyObject listbox, int index)
        {
            var theflow = FindChild<VirtElementFlow>(listbox, "ElementFlow");
            if (theflow == null) return false;

            theflow.SelectedIndex = -1;
            theflow.StoredSelectedIndex = -2;
            theflow.SelectedIndex = index;
            theflow.InvalidateMeasure();

            return true;
        }

        public static bool VirtFlowChangeCamera(DependencyObject listbox, PerspectiveCamera camera)
        {
            var theflow = FindChild<VirtElementFlow>(listbox, "ElementFlow");
            if (theflow == null) return false;

            theflow.Camera = camera;

            return true;
        }

        public static bool VirtFlowChangeCameraPov(DependencyObject listbox, double pov)
        {
            var theflow = FindChild<VirtElementFlow>(listbox, "ElementFlow");
            if (theflow == null) return false;
            if (theflow.Camera == null) return false;

            var camera = new PerspectiveCamera(theflow.Camera.Position, theflow.Camera.LookDirection,
                                               theflow.Camera.UpDirection, pov);
            theflow.Camera = camera;
            return true;
        }

        public static object GetObjectDataFromPoint(UIElement source, Point point)
        {
            if (source != null)
            {
                var element = source.InputHitTest(point) as UIElement;
                if (element != null)
                {
                    if (element is ListBoxItem)
                        return element;
                    while (element != source)
                    {
                        if (element != null) element = VisualTreeHelper.GetParent(element) as UIElement;
                        if (element is ListBoxItem)
                            return element;
                    }
                }
            }
            return null;
        }

        public static TChildItem FindVisualChild<TChildItem>(DependencyObject obj)
   where TChildItem : DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is TChildItem)
                    return (TChildItem)child;
                var childOfChild = FindVisualChild<TChildItem>(child);
                if (childOfChild != null)
                    return childOfChild;
            }
            return null;
        }

        public static T FindChild<T>(DependencyObject parent, string childName)
    where T : DependencyObject
        {
            if (parent == null) return null;
            T foundChild = null;
            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                var childType = child as T;
                if (childType == null)
                {
                    foundChild = FindChild<T>(child, childName);
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }
            return foundChild;
        }

    }
}
