using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Yatse2Setup
{
	public class FileIcon
	{
		private const int MaxPath = 260;
		
		[StructLayout(LayoutKind.Sequential)]
        internal struct SHFILEINFO
		{
			public IntPtr hIcon;
			public int iIcon;
			public int dwAttributes;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=MaxPath)]
			public string szDisplayName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=80)]
			public string szTypeName;
		}

        internal static class NativeMethods
        {
            [DllImport("shell32", CharSet = CharSet.Unicode)]
            public static extern IntPtr SHGetFileInfo(
                string pszPath,
                int dwFileAttributes,
                ref SHFILEINFO psfi,
                uint cbFileInfo,
                uint uFlags);

            [DllImport("user32.dll")]
            public static extern int DestroyIcon(IntPtr hIcon);

            [DllImport("kernel32", CharSet = CharSet.Unicode)]
            public extern static int FormatMessage(
                int dwFlags,
                IntPtr lpSource,
                int dwMessageId,
                int dwLanguageId,
                string lpBuffer,
                uint nSize,
                IntPtr argumentsLong);

            [DllImport("kernel32")]
            public extern static int GetLastError();

        }
		//private const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x100; 
		//private const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x2000;
		//private const int FORMAT_MESSAGE_FROM_HMODULE = 0x800;
		//private const int FORMAT_MESSAGE_FROM_STRING = 0x400;
		private const int FormatMessageFromSystem = 0x1000;
		private const int FormatMessageIgnoreInserts = 0x200;
		//private const int FORMAT_MESSAGE_MAX_WIDTH_MASK = 0xFF;

		//private string _fileName;
	    //private ShGetFileInfoConstants _flags;

	    [Flags]		
		public enum ShGetFileInfoConstants
		{
// ReSharper disable InconsistentNaming
			SHGFI_ICON = 0x100,                // get icon 
			SHGFI_DISPLAYNAME = 0x200,         // get display name 
			SHGFI_TYPENAME = 0x400,            // get type name 
			SHGFI_ATTRIBUTES = 0x800,          // get attributes 
			SHGFI_ICONLOCATION = 0x1000,       // get icon location 
			SHGFI_EXETYPE = 0x2000,            // return exe type 
			SHGFI_SYSICONINDEX = 0x4000,       // get system icon index 
			SHGFI_LINKOVERLAY = 0x8000,        // put a link overlay on icon 
			SHGFI_SELECTED = 0x10000,          // show icon in selected state 
			SHGFI_ATTR_SPECIFIED = 0x20000,    // get only specified attributes 
			SHGFI_LARGEICON = 0x0,             // get large icon 
			SHGFI_SMALLICON = 0x1,             // get small icon 
			SHGFI_OPENICON = 0x2,              // get open icon 
			SHGFI_SHELLICONSIZE = 0x4,         // get shell size icon 
			SHGFI_PIDL = 0x8,                  // pszPath is a pidl 
			SHGFI_USEFILEATTRIBUTES = 0x10,     // use passed dwFileAttribute 
			SHGFI_ADDOVERLAYS = 0x000000020,     // apply the appropriate overlays
			SHGFI_OVERLAYINDEX = 0x000000040     // Get the index of the overlay
// ReSharper restore InconsistentNaming
		}

		public ShGetFileInfoConstants Flags { get; set; }

		public string FileName { get; set; }

	    public Icon ShellIcon { get; private set; }

	    public string DisplayName { get; private set; }

	    public string TypeName { get; private set; }

	    public void GetInfo()
		{
			ShellIcon = null;
			TypeName = "";
			DisplayName = "";

			var shfi = new SHFILEINFO();
			var shfiSize = (uint)Marshal.SizeOf(shfi.GetType());

			var ret = NativeMethods.SHGetFileInfo(
                FileName, 0, ref shfi, shfiSize, (uint)(Flags));
			if (ret != (IntPtr) 0)
			{
				if (shfi.hIcon != IntPtr.Zero)
				{
					ShellIcon = Icon.FromHandle(shfi.hIcon);
				}
				TypeName = shfi.szTypeName;
				DisplayName = shfi.szDisplayName;
			}
			else
			{
			
				var err = NativeMethods.GetLastError();
				Console.WriteLine("Error {0}", err);
				var txtS = new string('\0', 256);
                var len = NativeMethods.FormatMessage(
					FormatMessageFromSystem | FormatMessageIgnoreInserts,
					IntPtr.Zero, err, 0, txtS, 256, (IntPtr)0);
				Console.WriteLine("Len {0} text {1}", len, txtS);
			}
		}

		public FileIcon()
		{
            Flags = ShGetFileInfoConstants.SHGFI_ICON | 
				ShGetFileInfoConstants.SHGFI_DISPLAYNAME |
				ShGetFileInfoConstants.SHGFI_TYPENAME |
				ShGetFileInfoConstants.SHGFI_ATTRIBUTES |
				ShGetFileInfoConstants.SHGFI_EXETYPE;
		}

		public FileIcon(string fileName) : this()
		{
			FileName = fileName;
			GetInfo();
		}

		public FileIcon(string fileName, ShGetFileInfoConstants flags)
		{
			FileName = fileName;
            Flags = flags;
			GetInfo();
		}

	}
}