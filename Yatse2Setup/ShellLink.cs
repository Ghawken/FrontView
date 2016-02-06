using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Yatse2Setup
{
	public class ShellLink : IDisposable
	{
		[ComImportAttribute]
		[GuidAttribute("0000010C-0000-0000-C000-000000000046")]
		[InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		private interface IPersist
		{
			[PreserveSig]
			void GetClassID(out Guid pClassId);
		}

		[ComImportAttribute]
		[GuidAttribute("0000010B-0000-0000-C000-000000000046")]
		[InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		private interface IPersistFile
		{
			[PreserveSig]
			void GetClassID(out Guid pClassId);
			void IsDirty();
			void Load(
				[MarshalAs(UnmanagedType.LPWStr)] string pszFileName, 
				uint dwMode);
			void Save(
				[MarshalAs(UnmanagedType.LPWStr)] string pszFileName, 
				[MarshalAs(UnmanagedType.Bool)] bool fRemember);
			void SaveCompleted(
				[MarshalAs(UnmanagedType.LPWStr)] string pszFileName);
			void GetCurFile(
				[MarshalAs(UnmanagedType.LPWStr)] out string ppszFileName);
		}

		[ComImportAttribute]
		[GuidAttribute("000214EE-0000-0000-C000-000000000046")]
		[InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		private interface IShellLinkA
		{
			void GetPath(
				[Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder pszFile, 
				int cchMaxPath, 
				ref Win32FindDataa pfd, 
				uint fFlags);
			void GetIDList(out IntPtr ppidl);
			void SetIDList(IntPtr pidl);
			void GetDescription(
				[Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder pszFile,
				int cchMaxName);
			void SetDescription(
				[MarshalAs(UnmanagedType.LPStr)] string pszName);
			void GetWorkingDirectory(
				[Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder pszDir,
				int cchMaxPath);
			void SetWorkingDirectory(
				[MarshalAs(UnmanagedType.LPStr)] string pszDir);
			void GetArguments(
				[Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder pszArgs, 
				int cchMaxPath);
			void SetArguments(
				[MarshalAs(UnmanagedType.LPStr)] string pszArgs);
			void GetHotkey(out short pwHotkey);
			void SetHotkey(short pwHotkey);
			void GetShowCmd(out uint piShowCmd);
			void SetShowCmd(uint piShowCmd);
			void GetIconLocation(
				[Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder pszIconPath, 
				int cchIconPath, 
				out int piIcon);
			void SetIconLocation(
				[MarshalAs(UnmanagedType.LPStr)] string pszIconPath, 
				int iIcon);
			void SetRelativePath(
				[MarshalAs(UnmanagedType.LPStr)] string pszPathRel, 
				uint dwReserved);
			void Resolve(
				IntPtr hWnd, 
				uint fFlags);
			void SetPath(
				[MarshalAs(UnmanagedType.LPStr)] string pszFile);
		}


		[ComImportAttribute]
		[GuidAttribute("000214F9-0000-0000-C000-000000000046")]
		[InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		private interface IShellLinkW
		{
			void GetPath(
				[Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, 
				int cchMaxPath, 
				ref Win32FindDataw pfd, 
				uint fFlags);
			void GetIDList(out IntPtr ppidl);
			void SetIDList(IntPtr pidl);
			void GetDescription(
				[Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile,
				int cchMaxName);
			void SetDescription(
				[MarshalAs(UnmanagedType.LPWStr)] string pszName);
			void GetWorkingDirectory(
				[Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir,
				int cchMaxPath);
            void SetWorkingDirectory(
				[MarshalAs(UnmanagedType.LPWStr)] string pszDir);
			void GetArguments(
				[Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, 
				int cchMaxPath);
			void SetArguments(
				[MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
			void GetHotkey(out short pwHotkey);
			void SetHotkey(short pwHotkey);
			void GetShowCmd(out uint piShowCmd);
			void SetShowCmd(uint piShowCmd);
			void GetIconLocation(
				[Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, 
				int cchIconPath, 
				out int piIcon);
			void SetIconLocation(
				[MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, 
				int iIcon);
			void SetRelativePath(
				[MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, 
				uint dwReserved);
			void Resolve(
				IntPtr hWnd, 
				uint fFlags);
			void SetPath(
				[MarshalAs(UnmanagedType.LPWStr)] string pszFile);
		}

        [GuidAttribute("00021401-0000-0000-C000-000000000046")]
		[ClassInterfaceAttribute(ClassInterfaceType.None)]
		[ComImportAttribute]
		private class CShellLink{}

		private enum EShellLinkGp : uint
		{
// ReSharper disable InconsistentNaming
			//SLGP_SHORTPATH = 1,
			SLGP_UNCPRIORITY = 2
// ReSharper restore InconsistentNaming
		}

		[Flags]
		private enum EShowWindowFlags : uint
		{
            // ReSharper disable InconsistentNaming
			//SW_HIDE = 0,
			//SW_SHOWNORMAL = 1,
			SW_NORMAL = 1,
			//SW_SHOWMINIMIZED = 2,
			//SW_SHOWMAXIMIZED = 3,
			SW_MAXIMIZE = 3,
			//SW_SHOWNOACTIVATE = 4,
			//SW_SHOW = 5,
			//SW_MINIMIZE = 6,
			SW_SHOWMINNOACTIVE = 7,
			//SW_SHOWNA = 8,
			//SW_RESTORE = 9,
			//SW_SHOWDEFAULT = 10,
			//SW_MAX = 10
            // ReSharper restore InconsistentNaming
		}

		[StructLayoutAttribute(LayoutKind.Sequential, Pack=4, Size=0, CharSet=CharSet.Unicode)]
		private struct Win32FindDataw
		{
		    private readonly uint dwFileAttributes;
            private readonly Filetime ftCreationTime;
            private readonly Filetime ftLastAccessTime;
            private readonly Filetime ftLastWriteTime;
            private readonly uint nFileSizeHigh;
            private readonly uint nFileSizeLow;
            private readonly uint dwReserved0;
            private readonly uint dwReserved1;
			[MarshalAs(UnmanagedType.ByValTStr , SizeConst = 260)] // MAX_PATH
            private readonly string cFileName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            private readonly string cAlternateFileName;
		}

		[StructLayoutAttribute(LayoutKind.Sequential, Pack=4, Size=0, CharSet=CharSet.Ansi)]
		private struct Win32FindDataa
		{
            private readonly uint dwFileAttributes;
            private readonly Filetime ftCreationTime;
            private readonly Filetime ftLastAccessTime;
            private readonly Filetime ftLastWriteTime;
            private readonly uint nFileSizeHigh;
            private readonly uint nFileSizeLow;
            private readonly uint dwReserved0;
            private readonly uint dwReserved1;
			[MarshalAs(UnmanagedType.ByValTStr , SizeConst = 260)] // MAX_PATH
            private readonly string cFileName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            private readonly string cAlternateFileName;
		}

		[StructLayoutAttribute(LayoutKind.Sequential, Pack=4, Size=0)]
		private struct Filetime 
		{
		    private readonly uint dwLowDateTime;
		    private readonly uint dwHighDateTime;
		}

        internal static class NativeMethods
		{
			[DllImport("Shell32", CharSet=CharSet.Auto)]
			internal extern static int ExtractIconEx (
				[MarshalAs(UnmanagedType.LPWStr)] 
				string lpszFile,
				int nIconIndex,
				IntPtr[] phIconLarge, 
				IntPtr[] phIconSmall,
				int nIcons);
		}

		[Flags]
		public enum EShellLinkResolveFlags : uint
		{
// ReSharper disable InconsistentNaming
			SLR_ANY_MATCH = 0x2,
			SLR_INVOKE_MSI = 0x80,
			SLR_NOLINKINFO = 0x40,	    
			SLR_NO_UI = 0x1,
			SLR_NO_UI_WITH_MSG_PUMP = 0x101,
			SLR_NOUPDATE = 0x8,																																																																																																																																																																																																												
			SLR_NOSEARCH = 0x10,
			SLR_NOTRACK = 0x20,
			SLR_UPDATE  = 0x4
 // ReSharper restore InconsistentNaming
		}

		public enum LinkDisplayMode : uint
		{
			EdmNormal = EShowWindowFlags.SW_NORMAL,
			EdmMinimized = EShowWindowFlags.SW_SHOWMINNOACTIVE,
			EdmMaximized = EShowWindowFlags.SW_MAXIMIZE
		}

		private IShellLinkW _linkW;
		private IShellLinkA _linkA;
		//private string _shortcutFile = "";

		public ShellLink()
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				_linkW = (IShellLinkW)new CShellLink();
			}
			else
			{
				_linkA = (IShellLinkA)new CShellLink();
			}
		}

		public ShellLink(string linkFile) : this()
		{
			Open(linkFile);
		}

		~ShellLink()
		{
			Dispose(false);
		}

		/*public void Dispose()
		{
			if (_linkW != null ) 
			{
				Marshal.ReleaseComObject(_linkW);
				_linkW = null;
			}
			if (_linkA != null)
			{
				Marshal.ReleaseComObject(_linkA);
				_linkA = null;
			}
		}*/
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            if (_linkW != null)
            {
                Marshal.ReleaseComObject(_linkW);
                _linkW = null;
            }
            if (_linkA != null)
            {
                Marshal.ReleaseComObject(_linkA);
                _linkA = null;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


		public string ShortCutFile { get; set;}

		public Icon LargeIcon
		{
			get
			{
				return getIcon(true);
			}
		}

		public Icon SmallIcon
		{
			get
			{
				return getIcon(false);
			}
		}

		private Icon getIcon(bool large)
		{
            int iconIndex;
			var iconPath = new StringBuilder(260, 260);
			if (_linkA == null)
			{
				_linkW.GetIconLocation(iconPath, iconPath.Capacity, out iconIndex);
			}
			else
			{
				_linkA.GetIconLocation(iconPath, iconPath.Capacity, out iconIndex);
			}
			var iconFile = iconPath.ToString();

			if (iconFile.Length == 0)
			{
				var flags = FileIcon.ShGetFileInfoConstants.SHGFI_ICON |
					FileIcon.ShGetFileInfoConstants.SHGFI_ATTRIBUTES;
				if (large)
				{
					flags = flags | FileIcon.ShGetFileInfoConstants.SHGFI_LARGEICON;
				}
				else
				{
					flags = flags | FileIcon.ShGetFileInfoConstants.SHGFI_SMALLICON;
				}
				var fileIcon = new FileIcon(Target, flags);
				return fileIcon.ShellIcon;
			}
		    var hIconEx = new[] {IntPtr.Zero};
		    if (large)
		    {
                NativeMethods.ExtractIconEx(
		            iconFile,
		            iconIndex,
		            hIconEx,
		            null,
		            1);
		    }
		    else
		    {
                NativeMethods.ExtractIconEx(
		            iconFile,
		            iconIndex,
		            null,
		            hIconEx,
		            1);
		    }
		    Icon icon = null;
		    if (hIconEx[0] != IntPtr.Zero)
		    {
		        icon = Icon.FromHandle(hIconEx[0]);
		        //UnManagedMethods.DestroyIcon(hIconEx[0]);
		    }
		    return icon;
		}

		public string IconPath
		{
			get
			{
				var iconPath = new StringBuilder(260, 260);
                int iconIndex;
				if (_linkA == null)
				{
					_linkW.GetIconLocation(iconPath, iconPath.Capacity, out iconIndex);
				}
				else
				{
					_linkA.GetIconLocation(iconPath, iconPath.Capacity, out iconIndex);
				}
				return iconPath.ToString();
			}
			set
			{
				var iconPath = new StringBuilder(260, 260);
                int iconIndex;
				if (_linkA == null)
				{
					_linkW.GetIconLocation(iconPath, iconPath.Capacity, out iconIndex);
				}
				else
				{
					_linkA.GetIconLocation(iconPath, iconPath.Capacity, out iconIndex);
				}
				if (_linkA == null)
				{
					_linkW.SetIconLocation(value, iconIndex);
				}
				else
				{
					_linkA.SetIconLocation(value, iconIndex);
				}
			}
		}

		public int IconIndex
		{
			get
			{
				var iconPath = new StringBuilder(260, 260);
                int iconIndex;
				if (_linkA == null)
				{
					_linkW.GetIconLocation(iconPath, iconPath.Capacity, out iconIndex);
				}
				else
				{
					_linkA.GetIconLocation(iconPath, iconPath.Capacity, out iconIndex);
				}
				return iconIndex;
			}
			set
			{
				var iconPath = new StringBuilder(260, 260);
				int iconIndex;
				if (_linkA == null)
				{
					_linkW.GetIconLocation(iconPath, iconPath.Capacity, out iconIndex);
				}
				else
				{
					_linkA.GetIconLocation(iconPath, iconPath.Capacity, out iconIndex);
				}
				if (_linkA == null)
				{
					_linkW.SetIconLocation(iconPath.ToString(), value);
				}
				else
				{
					_linkA.SetIconLocation(iconPath.ToString(), value);
				}
			}
		}

		public string Target
		{
			get
			{		
				var target = new StringBuilder(260, 260);
				if (_linkA == null)
				{
					var fd = new Win32FindDataw();
					_linkW.GetPath(target, target.Capacity, ref fd, (uint)EShellLinkGp.SLGP_UNCPRIORITY);
				}
				else
				{
					var fd = new Win32FindDataa();
					_linkA.GetPath(target, target.Capacity, ref fd, (uint)EShellLinkGp.SLGP_UNCPRIORITY);
				}
				return target.ToString();
			}
			set
			{
				if (_linkA == null)
				{
					_linkW.SetPath(value);
				}
				else
				{
					_linkA.SetPath(value);
				}
			}
		}

		public string WorkingDirectory
		{
			get
			{
				var path = new StringBuilder(260, 260);
				if (_linkA == null)
				{
					_linkW.GetWorkingDirectory(path, path.Capacity);
				}
				else
				{
					_linkA.GetWorkingDirectory(path, path.Capacity);
				}
				return path.ToString();
			}
			set
			{
				if (_linkA == null)
				{
					_linkW.SetWorkingDirectory(value);	
				}
				else
				{
					_linkA.SetWorkingDirectory(value);
				}
			}
		}

		public string Description
		{
			get
			{
				var description = new StringBuilder(1024, 1024);
				if (_linkA == null)
				{
					_linkW.GetDescription(description, description.Capacity);
				}
				else
				{
					_linkA.GetDescription(description, description.Capacity);
				}
				return description.ToString();
			}
			set
			{
				if (_linkA == null)
				{
					_linkW.SetDescription(value);
				}
				else
				{
					_linkA.SetDescription(value);
				}
			}
		}

		public string Arguments
		{
			get
			{				
				var arguments = new StringBuilder(260, 260);
				if (_linkA == null)
				{
					_linkW.GetArguments(arguments, arguments.Capacity);
				}
				else
				{
					_linkA.GetArguments(arguments, arguments.Capacity);
				}
				return arguments.ToString();
			}
			set
			{
				if (_linkA == null)
				{
					_linkW.SetArguments(value);
				}
				else
				{
					_linkA.SetArguments(value);
				}
			}
		}

		public LinkDisplayMode DisplayMode
		{
			get
			{
				uint cmd;
				if (_linkA == null)
				{
					_linkW.GetShowCmd(out cmd);
				}
				else
				{
					_linkA.GetShowCmd(out cmd);
				}
				return (LinkDisplayMode)cmd;
			}
			set
			{
				if (_linkA == null)
				{
					_linkW.SetShowCmd((uint)value);
				}
				else
				{
					_linkA.SetShowCmd((uint)value);
				}
			}
		}

		public Keys HotKey
		{
			get
			{
				short key;
				if (_linkA == null)
				{
					_linkW.GetHotkey(out key);
				}
				else
				{
					_linkA.GetHotkey(out key);
				}
				return (Keys)key;
			}
			set
			{
				if (_linkA == null)
				{
					_linkW.SetHotkey((short)value);
				}
				else
				{
					_linkA.SetHotkey((short)value);
				}
			}
		}

		public void Save()
		{
            Save(ShortCutFile);
		}

		public void Save(
			string linkFile
			)
		{   
    		if (_linkA == null)
			{
				((IPersistFile)_linkW).Save(linkFile, true);
                ShortCutFile = linkFile;
			}
			else
			{
				((IPersistFile)_linkA).Save(linkFile, true);
                ShortCutFile = linkFile;
			}
		}

		public void Open(
			string linkFile			
			)
		{
			Open(linkFile, 
				IntPtr.Zero, 
				(EShellLinkResolveFlags.SLR_ANY_MATCH | EShellLinkResolveFlags.SLR_NO_UI),
				1);
		}
		
		public void Open(
			string linkFile, 
			IntPtr hWnd, 
			EShellLinkResolveFlags resolveFlags
			)
		{
			Open(linkFile, 
				hWnd, 
				resolveFlags, 
				1);
		}

		public void Open(
			string linkFile,
			IntPtr hWnd, 
			EShellLinkResolveFlags resolveFlags,
			ushort timeOut
			)
		{
			uint flags;

			if ((resolveFlags & EShellLinkResolveFlags.SLR_NO_UI) 
				== EShellLinkResolveFlags.SLR_NO_UI)
			{
				flags = (uint)((int)resolveFlags | (timeOut << 16));
			}
			else
			{
				flags = (uint)resolveFlags;
			}

			if (_linkA == null)
			{
				((IPersistFile)_linkW).Load(linkFile, 0); //STGM_DIRECT)
				_linkW.Resolve(hWnd, flags);
                ShortCutFile = linkFile;
			}
			else
			{
				((IPersistFile)_linkA).Load(linkFile, 0); //STGM_DIRECT)
				_linkA.Resolve(hWnd, flags);
                ShortCutFile = linkFile;
			}
		}
	}


}
