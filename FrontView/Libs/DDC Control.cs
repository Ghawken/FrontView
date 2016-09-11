using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Interop;
using System.Diagnostics;
using FrontView.Classes;

namespace FrontView.Libs.DDCControl
{
    class NativeCalls
    {
        [DllImport("user32.dll", EntryPoint = "MonitorFromWindow", SetLastError = true)]
        public static extern IntPtr MonitorFromWindow(
            [In] IntPtr hwnd, uint dwFlags);

        [DllImport("dxva2.dll", EntryPoint = "GetNumberOfPhysicalMonitorsFromHMONITOR", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR(
            IntPtr hMonitor, ref uint pdwNumberOfPhysicalMonitors);

        [DllImport("dxva2.dll", EntryPoint = "GetPhysicalMonitorsFromHMONITOR", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetPhysicalMonitorsFromHMONITOR(
            IntPtr hMonitor,
            uint dwPhysicalMonitorArraySize,
            [Out] NativeStructures.PHYSICAL_MONITOR[] pPhysicalMonitorArray);

        [DllImport("dxva2.dll", EntryPoint = "DestroyPhysicalMonitors", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyPhysicalMonitors(
            uint dwPhysicalMonitorArraySize, [Out] NativeStructures.PHYSICAL_MONITOR[] pPhysicalMonitorArray);

        [DllImport("dxva2.dll", EntryPoint = "GetMonitorTechnologyType", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMonitorTechnologyType(
            IntPtr hMonitor, ref NativeStructures.MC_DISPLAY_TECHNOLOGY_TYPE pdtyDisplayTechnologyType);

        [DllImport("dxva2.dll", EntryPoint = "GetMonitorCapabilities", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMonitorCapabilities(
            IntPtr hMonitor, ref uint pdwMonitorCapabilities, ref uint pdwSupportedColorTemperatures);

        [DllImport("dxva2.dll", EntryPoint = "SetMonitorBrightness", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetMonitorBrightness(
            IntPtr hMonitor, short brightness);

        [DllImport("dxva2.dll", EntryPoint = "GetMonitorBrightness", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMonitorBrightness(
            IntPtr hMonitor, ref short pdwMinimumBrightness, ref short pdwCurrentBrightness, ref short pdwMaximumBrightness);

        [DllImport("dxva2.dll", EntryPoint = "SetMonitorContrast", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetMonitorContrast(
    IntPtr hMonitor, short contrast);

        [DllImport("dxva2.dll", EntryPoint = "GetMonitorContrast", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMonitorContrast(
            IntPtr hMonitor, ref short pdwMinimumBrightness, ref short pdwCurrentContrast, ref short pdwMaximumBrightness);

    }
    public class NativeConstants
    {
        public const int MONITOR_DEFAULTTOPRIMARY = 1;

        public const int MONITOR_DEFAULTTONEAREST = 2;

        public const int MONITOR_DEFAULTTONULL = 0;
    }

    public class NativeStructures
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct PHYSICAL_MONITOR
        {
            public IntPtr hPhysicalMonitor;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szPhysicalMonitorDescription;
        }

        public enum MC_DISPLAY_TECHNOLOGY_TYPE
        {
            MC_SHADOW_MASK_CATHODE_RAY_TUBE,

            MC_APERTURE_GRILL_CATHODE_RAY_TUBE,

            MC_THIN_FILM_TRANSISTOR,

            MC_LIQUID_CRYSTAL_ON_SILICON,

            MC_PLASMA,

            MC_ORGANIC_LIGHT_EMITTING_DIODE,

            MC_ELECTROLUMINESCENT,

            MC_MICROELECTROMECHANICAL,

            MC_FIELD_EMISSION_DEVICE,
        }
    }


    class BrightnessControl
    {

        private IntPtr hWnd;
        private NativeStructures.PHYSICAL_MONITOR[] pPhysicalMonitorArray;
        private uint pdwNumberOfPhysicalMonitors;

        public BrightnessControl(IntPtr hWnd)
        {
            this.hWnd = hWnd;
            SetupMonitors();
        }

        private void SetupMonitors()
        {
            try
            {
                IntPtr hMonitor = NativeCalls.MonitorFromWindow(hWnd, NativeConstants.MONITOR_DEFAULTTOPRIMARY);
                bool numberOfPhysicalMonitorsFromHmonitor = NativeCalls.GetNumberOfPhysicalMonitorsFromHMONITOR(hMonitor, ref pdwNumberOfPhysicalMonitors);
                pPhysicalMonitorArray = new NativeStructures.PHYSICAL_MONITOR[pdwNumberOfPhysicalMonitors];
                bool physicalMonitorsFromHmonitor = NativeCalls.GetPhysicalMonitorsFromHMONITOR(hMonitor, pdwNumberOfPhysicalMonitors, pPhysicalMonitorArray);
                Setup.Logger.Instance().LogDump("DDCControl", "SetupMonitor Called: #Monitors:" + pdwNumberOfPhysicalMonitors);
            }
            catch (Exception ex)
            {
                Setup.Logger.Instance().LogDump("DDCControl", "SetupMonitor Exception: #Monitors:" + ex);

            }
        }

        public bool GetMonitorCapabilities(int monitorNumber)
        {
            try
            {
                uint pdwMonitorCapabilities = 0u;
                uint pdwSupportedColorTemperatures = 0u;
                var monitorCapabilities = NativeCalls.GetMonitorCapabilities(pPhysicalMonitorArray[monitorNumber].hPhysicalMonitor, ref pdwMonitorCapabilities, ref pdwSupportedColorTemperatures);
                Setup.Logger.Instance().LogDump("DDCControl", "GetMonitorCapabilites Called: #MonitorCapabilites:" + pdwMonitorCapabilities);
                Setup.Logger.Instance().LogDump("DDCControl", "GetMonitorCapabilites Called: #ColorTemp:" + pdwSupportedColorTemperatures);

                NativeStructures.MC_DISPLAY_TECHNOLOGY_TYPE type = NativeStructures.MC_DISPLAY_TECHNOLOGY_TYPE.MC_SHADOW_MASK_CATHODE_RAY_TUBE;
                var monitorTechnologyType = NativeCalls.GetMonitorTechnologyType(pPhysicalMonitorArray[monitorNumber].hPhysicalMonitor, ref type);
                Setup.Logger.Instance().LogDump("DDCControl", "GetMonitorCapabilities Called: #type:" + type);
                return true;
            }
            catch (Exception ex)
            {
                Setup.Logger.Instance().LogDump("DDCControl", "GetMonitorCapabilites Exception:" + ex);
                return false;
            }
        }

        public bool SetBrightness(short brightness, int monitorNumber)
        {
            try
            {
                Setup.Logger.Instance().LogDump("DDCControl", "SetBrightness Called:" + brightness + ":" + monitorNumber);
                var brightnessWasSet = NativeCalls.SetMonitorBrightness(pPhysicalMonitorArray[monitorNumber].hPhysicalMonitor, (short)brightness);
                if (brightnessWasSet)
                    Setup.Logger.Instance().LogDump("DDCControl", "SetBrightness brightnessWasSet True:" + brightness + ":" + monitorNumber);
                return brightnessWasSet;
            }
            catch (Exception ex)
            {
                Setup.Logger.Instance().LogDump("DDCControl", "SetBrightness Exception:" + ex);
                return false;
            }
            
        }

        public bool SetContrast(short contrast, int monitorNumber)
        {
            try
            {
                Setup.Logger.Instance().LogDump("DDCControl", "SetContrast Called:" + contrast + ":" + monitorNumber);
                var brightnessWasSet = NativeCalls.SetMonitorContrast(pPhysicalMonitorArray[monitorNumber].hPhysicalMonitor, (short)contrast);
                if (brightnessWasSet)
                    Setup.Logger.Instance().LogDump("DDCControl", "SetContrast brightnesswasSet True:" + contrast + ":" + monitorNumber);

                return brightnessWasSet;
            }
            catch (Exception ex)
            {
                Setup.Logger.Instance().LogDump("DDCControl", "SetContrast Exception:" + ex);
                return false;
            }
        }

        public BrightnessInfo GetBrightnessCapabilities(int monitorNumber)
        {

            short current = -1, minimum = -1, maximum = -1;
            try
            {
                
                bool getBrightness = NativeCalls.GetMonitorBrightness(pPhysicalMonitorArray[monitorNumber].hPhysicalMonitor, ref minimum, ref current, ref maximum);
                Setup.Logger.Instance().LogDump("DDCControl", "GetBrightness Called: Current:" + current + ":" + monitorNumber);
                return new BrightnessInfo { minimum = minimum, maximum = maximum, current = current };
            }
            catch (Exception ex)
            {
                Setup.Logger.Instance().LogDump("DDCControl", "GetBrightness Exception"+ex);
                return new BrightnessInfo { minimum = minimum, maximum = maximum, current = current };
            }
        }

        public BrightnessInfo GetContrastCapabilities(int monitorNumber)
        {
            short current = -1, minimum = -1, maximum = -1;
            try
            {

                bool getBrightness = NativeCalls.GetMonitorContrast(pPhysicalMonitorArray[monitorNumber].hPhysicalMonitor, ref minimum, ref current, ref maximum);
                Setup.Logger.Instance().LogDump("DDCControl", "GetContrast Called: Current:" + current + ":" + monitorNumber);
                return new BrightnessInfo { minimum = minimum, maximum = maximum, current = current };
            }
            catch (Exception ex)
            {
                Setup.Logger.Instance().LogDump("DDCControl", "GetContrast Called: Current:" + ex);
                return new BrightnessInfo { minimum = minimum, maximum = maximum, current = current };
            }
        }
        public void DestroyMonitors()
        {
            var destroyPhysicalMonitors = NativeCalls.DestroyPhysicalMonitors(pdwNumberOfPhysicalMonitors, pPhysicalMonitorArray);
            
        }

        public uint GetMonitors()
        {
            return pdwNumberOfPhysicalMonitors;
        }
    }
}
