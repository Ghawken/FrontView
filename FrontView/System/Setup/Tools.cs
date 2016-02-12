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
using System.Runtime.InteropServices;

namespace Setup
{
    public class Tools
    {
        public enum MachineType : ushort
        {
            // ReSharper disable InconsistentNaming
            IMAGE_FILE_MACHINE_UNKNOWN = 0x0,
            IMAGE_FILE_MACHINE_AM33 = 0x1d3,
            IMAGE_FILE_MACHINE_AMD64 = 0x8664,
            IMAGE_FILE_MACHINE_ARM = 0x1c0,
            IMAGE_FILE_MACHINE_EBC = 0xebc,
            IMAGE_FILE_MACHINE_I386 = 0x14c,
            IMAGE_FILE_MACHINE_IA64 = 0x200,
            IMAGE_FILE_MACHINE_M32R = 0x9041,
            IMAGE_FILE_MACHINE_MIPS16 = 0x266,
            IMAGE_FILE_MACHINE_MIPSFPU = 0x366,
            IMAGE_FILE_MACHINE_MIPSFPU16 = 0x466,
            IMAGE_FILE_MACHINE_POWERPC = 0x1f0,
            IMAGE_FILE_MACHINE_POWERPCFP = 0x1f1,
            IMAGE_FILE_MACHINE_R4000 = 0x166,
            IMAGE_FILE_MACHINE_SH3 = 0x1a2,
            IMAGE_FILE_MACHINE_SH3DSP = 0x1a3,
            IMAGE_FILE_MACHINE_SH4 = 0x1a6,
            IMAGE_FILE_MACHINE_SH5 = 0x1a8,
            IMAGE_FILE_MACHINE_THUMB = 0x1c2,
            IMAGE_FILE_MACHINE_WCEMIPSV2 = 0x169,
            // ReSharper restore InconsistentNaming
        }

        public static int GetFileRevision(string file)
        {
            if (!File.Exists(file))
            {
                return 0;
            }
            var myFi = FileVersionInfo.GetVersionInfo(file);
            return myFi.FileBuildPart;
        }

        public static bool FindAndKillProcess(string name)
        {
            foreach (var clsProcess in Process.GetProcesses())
            {
                if (!clsProcess.ProcessName.Equals(name)) continue;
                clsProcess.Kill();
                return true;
            }
            return false;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static MachineType GetFileMachineType(string path)
        {
            MachineType machineType;
            using (var fs = new FileStream(path, FileMode.Open))
            {
                using (var br = new BinaryReader(fs))
                {
                    fs.Seek(0x3c, SeekOrigin.Begin);
                    var peOffset = br.ReadInt32();
                    fs.Seek(peOffset, SeekOrigin.Begin);
                    var peHead = br.ReadUInt32();
                    if (peHead != 0x00004550)
                        return MachineType.IMAGE_FILE_MACHINE_UNKNOWN;
                    machineType = (MachineType) br.ReadUInt16();
                }
            }

            return machineType;
        }

        public static bool? IsFile64Bit(string path)
        {
            switch (GetFileMachineType(path))
            {
                case MachineType.IMAGE_FILE_MACHINE_AMD64:
                case MachineType.IMAGE_FILE_MACHINE_IA64:
                    return true;
                case MachineType.IMAGE_FILE_MACHINE_I386:
                    return false;
                default:
                    return null;
            }
        }

        public static bool Is64BitProcess
        {
            get { return IntPtr.Size == 8; }
        }

        public static bool Is64BitOperatingSystem
        {
            get
            {
                if (Is64BitProcess)
                    return true;
                bool isWow64;
                return ModuleContainsFunction("kernel32.dll", "IsWow64Process") && NativeMethods.IsWow64Process(NativeMethods.GetCurrentProcess(), out isWow64) && isWow64;
            }
        }

        static bool ModuleContainsFunction(string moduleName, string methodName)
        {
            var hModule = NativeMethods.GetModuleHandle(moduleName);
            if (hModule != IntPtr.Zero)
                return NativeMethods.GetProcAddress(hModule, methodName) != IntPtr.Zero;
            return false;
        }

        internal static class NativeMethods
        {
            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            extern public static bool IsWow64Process(IntPtr hProcess, [MarshalAs(UnmanagedType.Bool)] out bool isWow64);
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            extern public static IntPtr GetCurrentProcess();
            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            extern public static IntPtr GetModuleHandle([MarshalAs(UnmanagedType.LPWStr)] string moduleName);
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true , BestFitMapping = false, ThrowOnUnmappableChar = true)]
            internal static extern IntPtr GetProcAddress([In] IntPtr hModule, [In, MarshalAs(UnmanagedType.LPStr)] string lpProcName);
        }

    }

}