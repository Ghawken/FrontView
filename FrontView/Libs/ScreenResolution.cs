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
using System.Globalization;
using System.Runtime.InteropServices;
using System.Collections;

namespace FrontView.Libs
{

    [StructLayout(LayoutKind.Sequential)]
    public struct Devmode : IEquatable<Devmode>  
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string DMDeviceName;
        public short DMSpecVersion { get; set; }
        public short DMDriverVersion { get; set; }
        public short DMSize { get; set; }
        public short DMDriverExtra { get; set; }
        public int DMFields { get; set; }
        public short DMOrientation { get; set; }
        public short DMPaperSize { get; set; }
        public short DMPaperLength { get; set; }
        public short DMPaperWidth { get; set; }
        public short DMScale { get; set; }
        public short DMCopies { get; set; }
        public short DMDefaultSource { get; set; }
        public short DMPrintQuality { get; set; }
        public short DMColor { get; set; }
        public short DMDuplex { get; set; }
        public short DMYResolution { get; set; }
        public short DMTTOption { get; set; }
        public short DMCollate { get; set; }
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string DMFormName;
        public short DMUnusedPadding { get; set; }
        public short DMBitsPerPel { get; set; }
        public int DMPelsWidth { get; set; }
        public int DMPelsHeight { get; set; }
        public int DMDisplayFlags { get; set; }
        public int DMDisplayFrequency { get; set; }

        public static bool operator ==(Devmode dev1, Devmode dev2)
        {
            return dev1.Equals(dev2);
        }

        public static bool operator !=(Devmode dev1, Devmode dev2)
        {
            return !dev1.Equals(dev2);
        }

        public bool Equals(Devmode other)
        {
            return other.DMDeviceName == DMDeviceName && other.DMSpecVersion == DMSpecVersion && other.DMDriverVersion == DMDriverVersion && other.DMSize == DMSize && other.DMDriverExtra == DMDriverExtra && other.DMFields == DMFields && other.DMOrientation == DMOrientation && other.DMPaperSize == DMPaperSize && other.DMPaperLength == DMPaperLength && other.DMPaperWidth == DMPaperWidth && other.DMScale == DMScale && other.DMCopies == DMCopies && other.DMDefaultSource == DMDefaultSource && other.DMPrintQuality == DMPrintQuality && other.DMColor == DMColor && other.DMDuplex == DMDuplex && other.DMYResolution == DMYResolution && other.DMTTOption == DMTTOption && other.DMCollate == DMCollate && other.DMUnusedPadding == DMUnusedPadding && other.DMBitsPerPel == DMBitsPerPel && other.DMPelsWidth == DMPelsWidth && other.DMPelsHeight == DMPelsHeight && other.DMDisplayFlags == DMDisplayFlags && other.DMDisplayFrequency == DMDisplayFrequency;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj.GetType() == typeof (Devmode) && Equals((Devmode) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = (DMDeviceName != null ? DMDeviceName.GetHashCode() : 0);
                result = (result*397) ^ DMSpecVersion.GetHashCode();
                result = (result*397) ^ DMDriverVersion.GetHashCode();
                result = (result*397) ^ DMSize.GetHashCode();
                result = (result*397) ^ DMDriverExtra.GetHashCode();
                result = (result*397) ^ DMFields;
                result = (result*397) ^ DMOrientation.GetHashCode();
                result = (result*397) ^ DMPaperSize.GetHashCode();
                result = (result*397) ^ DMPaperLength.GetHashCode();
                result = (result*397) ^ DMPaperWidth.GetHashCode();
                result = (result*397) ^ DMScale.GetHashCode();
                result = (result*397) ^ DMCopies.GetHashCode();
                result = (result*397) ^ DMDefaultSource.GetHashCode();
                result = (result*397) ^ DMPrintQuality.GetHashCode();
                result = (result*397) ^ DMColor.GetHashCode();
                result = (result*397) ^ DMDuplex.GetHashCode();
                result = (result*397) ^ DMYResolution.GetHashCode();
                result = (result*397) ^ DMTTOption.GetHashCode();
                result = (result*397) ^ DMCollate.GetHashCode();
                result = (result*397) ^ (DMFormName != null ? DMFormName.GetHashCode() : 0);
                result = (result*397) ^ DMUnusedPadding.GetHashCode();
                result = (result*397) ^ DMBitsPerPel.GetHashCode();
                result = (result*397) ^ DMPelsWidth;
                result = (result*397) ^ DMPelsHeight;
                result = (result*397) ^ DMDisplayFlags;
                result = (result*397) ^ DMDisplayFrequency;
                return result;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayDevice : IEquatable<DisplayDevice> 
    {
        public int cb;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string DeviceName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceString;
        public int StateFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceKey;

        public DisplayDevice(int param)
        {
            cb = 0;
            StateFlags = param;
            DeviceName = new string(Convert.ToChar(" ", CultureInfo.InvariantCulture), 32);
            DeviceString = new string(Convert.ToChar(" ", CultureInfo.InvariantCulture), 128);
            DeviceID = new string(Convert.ToChar(" ", CultureInfo.InvariantCulture), 128);
            DeviceKey = new string(Convert.ToChar(" ", CultureInfo.InvariantCulture), 128);
            cb = Marshal.SizeOf(this);
        }

        public bool Equals(DisplayDevice other)
        {
            return other.cb == cb && other.DeviceName == DeviceName && other.DeviceString == DeviceString && other.StateFlags == StateFlags && other.DeviceID == DeviceID && other.DeviceKey == DeviceKey;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj.GetType() == typeof (DisplayDevice) && Equals((DisplayDevice) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = cb;
                result = (result*397) ^ (DeviceName != null ? DeviceName.GetHashCode() : 0);
                result = (result*397) ^ (DeviceString != null ? DeviceString.GetHashCode() : 0);
                result = (result*397) ^ StateFlags;
                result = (result*397) ^ (DeviceID != null ? DeviceID.GetHashCode() : 0);
                result = (result*397) ^ (DeviceKey != null ? DeviceKey.GetHashCode() : 0);
                return result;
            }
        }

        public static bool operator ==(DisplayDevice left, DisplayDevice right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DisplayDevice left, DisplayDevice right)
        {
            return !left.Equals(right);
        }
    }

    class ScreenRes
    {
        public Devmode Mode;

        public ScreenRes(Devmode mode)
        {
            Mode = mode;
        }

        public override string ToString()
        {
            return ScreenResolution.DevmodeToString(Mode);
        }
    }

    public static class ScreenResolution
    {
        public static void ChangeResolution(int devNum, int modeNum)
        { 
            var d = GetDevmode(devNum, modeNum);
            
            if (d.DMBitsPerPel != 0 && d.DMPelsWidth != 0 && d.DMPelsHeight != 0)
            {
                var result = NativeMethods.ChangeDisplaySettingsEx(GetDeviceName(devNum), ref d, IntPtr.Zero, 0, IntPtr.Zero);
                if (result > 0)
                    return;
            }
        }

        public static void ChangeResolutionMode(int devNum, Devmode mode)
        { 
            if (mode.DMBitsPerPel != 0 && mode.DMPelsWidth != 0 && mode.DMPelsHeight != 0)
            {
                var result = NativeMethods.ChangeDisplaySettingsEx(GetDeviceName(devNum), ref mode, IntPtr.Zero, 0, IntPtr.Zero);
                if (result > 0)
                    return;
            }
        }

       /* public static void TurnOffDevice(int devNum, Devmode mode)
        {
            var result = NativeMethods.TurnOffMonitorEx(GetDeviceName(devNum), int null, int null, 0, int null);
            if (result > 0)
                return; 
         }*/

        public static int[] EnumDevices()
        { 
            var devices = new ArrayList();
            var d = new DisplayDevice(0);
            var devNum = 0;
            bool result;

            do
            {
                result = NativeMethods.EnumDisplayDevices(IntPtr.Zero, devNum, ref d, 0);
                if (result)
                {
                    devices.Add(devNum);
                }
                devNum++;
            } while (result);
            return (int[])devices.ToArray(typeof(int));
        }

        public static Devmode[] EnumModes(int devNum)
        {
            var modes = new ArrayList();
            var devName = GetDeviceName(devNum);
            var devMode = new Devmode();
            var modeNum = 0;
            bool result;

            do
            {
                result = NativeMethods.EnumDisplaySettings(devName, modeNum, ref devMode);
                if (result)
                {
                    modes.Add(devMode);

                }
                modeNum++;
            } while (result);

            return (Devmode[])modes.ToArray(typeof(Devmode));
        }


        public static Devmode GetDevmode(int devNum, int modeNum)
        { 
            var devMode = new Devmode();
            var devName = GetDeviceName(devNum);
            NativeMethods.EnumDisplaySettings(devName, modeNum, ref devMode);
            return devMode;
        }

        public static string DevmodeToString(Devmode devMode)
        {
            return devMode.DMPelsWidth +
                   " x " + devMode.DMPelsHeight +
                   ", " + devMode.DMBitsPerPel +
                   " bits, " +
                   devMode.DMDisplayFrequency + " Hz";
        }

        public static string GetDeviceName(int devNum)
        {
            var d = new DisplayDevice(0);
            var result = NativeMethods.EnumDisplayDevices(IntPtr.Zero, devNum, ref d, 0);
            return (result ? d.DeviceName.Trim() : "#error#");
        }

        public static string GetDeviceString(int devNum)
        {
            var d = new DisplayDevice(0);
            var result = NativeMethods.EnumDisplayDevices(IntPtr.Zero, devNum, ref d, 0);
            return (result ? d.DeviceString.Trim() : "#error#");
        }
        
        public static bool MainDevice(int devNum)
        { 
            var d = new DisplayDevice(0);
            if (NativeMethods.EnumDisplayDevices(IntPtr.Zero, devNum, ref d, 0))
            {
                return ((d.StateFlags & 4) != 0);
            } 
            return false;
        }

        internal static class NativeMethods
        {
            [DllImport("User32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool EnumDisplayDevices(
                IntPtr lpDevice, int iDevNum,
                ref DisplayDevice lpDisplayDevice, int dwFlags);

            [DllImport("User32.dll", BestFitMapping = false)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool EnumDisplaySettings(
                string devName, int modeNum, ref Devmode devMode);

            /*[DllImport("user32.dll")]
            public static extern int ChangeDisplaySettings(
                ref Devmode devMode, int flags);*/

            [DllImport("user32.dll", BestFitMapping = false)]
            public static extern int ChangeDisplaySettingsEx(
               string devName, ref Devmode devMode, IntPtr hwnd, int dwFlags, IntPtr lParam);

          /*  [DllImport("GDI32.dll")]
            private static extern bool SetDeviceGammaRamp(Int32 hdc, void* ramp); 

     /*       [DllImport("user32.dll", BestFitMapping = false)]
            public static extern int TurnOffMonitorEx(
               string devName, int hwnd, int hwnd, int hwnd, int lParam);
            */
        }

    }
}


