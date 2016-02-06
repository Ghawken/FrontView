// -------------------------------------------------------------------------------
// 
// This file is part of the FluidKit project: http://www.codeplex.com/fluidkit
// 
// Copyright (c) 2008, The FluidKit community 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
// 
// * Redistributions of source code must retain the above copyright notice, this 
// list of conditions and the following disclaimer.
// 
// * Redistributions in binary form must reproduce the above copyright notice, this 
// list of conditions and the following disclaimer in the documentation and/or 
// other materials provided with the distribution.
// 
// * Neither the name of FluidKit nor the names of its contributors may be used to 
// endorse or promote products derived from this software without specific prior 
// written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR 
// ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES 
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON 
// ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS 
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 
// -------------------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Shapes;

namespace FluidKit.Controls
{
	[TemplatePart(Name = "PART_CloseButton", Type = typeof (Button))]
	[TemplatePart(Name = "PART_MinimizeButton", Type = typeof (Button))]
	[TemplatePart(Name = "PART_MaximizeButton", Type = typeof (Button))]
	[TemplatePart(Name = "PART_NResizer", Type = typeof (Path))]
	[TemplatePart(Name = "PART_SResizer", Type = typeof (Path))]
	[TemplatePart(Name = "PART_EResizer", Type = typeof (Path))]
	[TemplatePart(Name = "PART_WResizer", Type = typeof (Path))]
	[TemplatePart(Name = "PART_NWResizer", Type = typeof (Rectangle))]
	[TemplatePart(Name = "PART_NEResizer", Type = typeof (Rectangle))]
	[TemplatePart(Name = "PART_SWResizer", Type = typeof (Rectangle))]
	[TemplatePart(Name = "PART_SEResizer", Type = typeof (Rectangle))]
	[TemplatePart(Name = "PART_TitleBar", Type = typeof (Panel))]
	public class GlassWindow : Window
	{
		private WindowInteropHelper _interopHelper;

		static GlassWindow()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof (GlassWindow),
			                                         new FrameworkPropertyMetadata(
			                                         	typeof (GlassWindow)));
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_interopHelper = new WindowInteropHelper(this);
			AttachToVisualTree();
		}

		private void AttachToVisualTree()
		{
			// Close Button
			Button closeButton = GetChildControl<Button>("PART_CloseButton");
			if (closeButton != null)
			{
				closeButton.Click += OnCloseButtonClick;
			}

			// Minimize Button
			Button minimizeButton = GetChildControl<Button>("PART_MinimizeButton");
			if (minimizeButton != null)
			{
				minimizeButton.Click += OnMinimizeButtonClick;
			}

			// Maximize Button
			Button maximizeButton = GetChildControl<Button>("PART_MaximizeButton");
			if (maximizeButton != null)
			{
				maximizeButton.Click += OnMaximizeButtonClick;
			}

			// Resizers
			if (ResizeMode != ResizeMode.NoResize)
			{
				Path sResizer = GetChildControl<Path>("PART_SResizer");
				if (sResizer != null)
				{
					sResizer.MouseDown += OnSizeSouth;
				}

				Path nResizer = GetChildControl<Path>("PART_NResizer");
				if (nResizer != null)
				{
					nResizer.MouseDown += OnSizeNorth;
				}

				Path eResizer = GetChildControl<Path>("PART_EResizer");
				if (eResizer != null)
				{
					eResizer.MouseDown += OnSizeEast;
				}

				Path wResizer = GetChildControl<Path>("PART_WResizer");
				if (wResizer != null)
				{
					wResizer.MouseDown += OnSizeWest;
				}

				Rectangle seResizer = GetChildControl<Rectangle>("PART_SEResizer");
				if (seResizer != null)
				{
					seResizer.MouseDown += OnSizeSouthEast;
				}

				Rectangle swResizer = GetChildControl<Rectangle>("PART_SWResizer");
				if (swResizer != null)
				{
					swResizer.MouseDown += OnSizeSouthWest;
				}

				Rectangle neResizer = GetChildControl<Rectangle>("PART_NEResizer");
				if (neResizer != null)
				{
					neResizer.MouseDown += OnSizeNorthEast;
				}

				Rectangle nwResizer = GetChildControl<Rectangle>("PART_NWResizer");
				if (nwResizer != null)
				{
					nwResizer.MouseDown += OnSizeNorthWest;
				}
			}

			// Title Bar
			Panel titleBar = GetChildControl<Panel>("PART_TitleBar");
			if (titleBar != null)
			{
				titleBar.MouseLeftButtonDown += OnTitleBarMouseDown;
			}
		}

		private void OnMaximizeButtonClick(object sender, RoutedEventArgs e)
		{
			ToggleMaximize();
		}

		private void ToggleMaximize()
		{
			this.WindowState = (this.WindowState == WindowState.Maximized)
			                   	?
			                   		WindowState.Normal
			                   	: WindowState.Maximized;
		}

		private void OnTitleBarMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (ResizeMode != ResizeMode.NoResize && e.ClickCount == 2)
			{
				ToggleMaximize();
				return;
			}
			this.DragMove();
		}

		protected T GetChildControl<T>(string ctrlName) where T : DependencyObject
		{
			T ctrl = GetTemplateChild(ctrlName) as T;
			return ctrl;
		}

		protected void MoveWindow(Rect rect)
		{
			MoveWindow(_interopHelper.Handle, (int) rect.Left,
			           (int) rect.Top, (int) rect.Width, (int) rect.Height, false);
		}

		private void OnCloseButtonClick(object sender, RoutedEventArgs e)
		{
           
            Close();
		}

		private void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}

		#region Sizing Event Handlers

		private void OnSizeSouth(object sender, MouseButtonEventArgs e)
		{
			DragSize(_interopHelper.Handle, SizingAction.South);
		}

		private void OnSizeNorth(object sender, MouseButtonEventArgs e)
		{
			DragSize(_interopHelper.Handle, SizingAction.North);
		}

		private void OnSizeEast(object sender, MouseButtonEventArgs e)
		{
			DragSize(_interopHelper.Handle, SizingAction.East);
		}

		private void OnSizeWest(object sender, MouseButtonEventArgs e)
		{
			DragSize(_interopHelper.Handle, SizingAction.West);
		}

		private void OnSizeNorthWest(object sender, MouseButtonEventArgs e)
		{
			DragSize(_interopHelper.Handle, SizingAction.NorthWest);
		}

		private void OnSizeNorthEast(object sender, MouseButtonEventArgs e)
		{
			DragSize(_interopHelper.Handle, SizingAction.NorthEast);
		}

		private void OnSizeSouthEast(object sender, MouseButtonEventArgs e)
		{
			DragSize(_interopHelper.Handle, SizingAction.SouthEast);
		}

		private void OnSizeSouthWest(object sender, MouseButtonEventArgs e)
		{
			DragSize(_interopHelper.Handle, SizingAction.SouthWest);
		}

		#endregion

		#region P/Invoke and Helper Method

		public enum SizingAction
		{
			North = 3,
			South = 6,
			East = 2,
			West = 1,
			NorthEast = 5,
			NorthWest = 4,
			SouthEast = 8,
			SouthWest = 7
		}

		private const int SC_SIZE = 0xF000;
		private const int WM_SYSCOMMAND = 0x112;

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam,
		                                         IntPtr lParam);

		[DllImport("user32")]
		private static extern Boolean MoveWindow(
			IntPtr hWnd,
			Int32 x, Int32 y,
			Int32 nWidth, Int32 nHeight, Boolean bRepaint);

		private void DragSize(IntPtr handle, SizingAction sizingAction)
		{
			if (Mouse.LeftButton == MouseButtonState.Pressed)
			{
				SendMessage(handle, WM_SYSCOMMAND, (IntPtr) (SC_SIZE + sizingAction), IntPtr.Zero);
				SendMessage(handle, 514, IntPtr.Zero, IntPtr.Zero);
			}
		}

		#endregion
	}
}