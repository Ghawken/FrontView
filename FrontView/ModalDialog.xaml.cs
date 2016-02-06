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

using System.Windows;
using System.Threading;
using System.Windows.Threading;

namespace FrontView
{

	public partial class ModalDialog
	{
		public ModalDialog()
		{
			InitializeComponent();
			Visibility = Visibility.Hidden;
		}

		private bool _hideRequest;
		private bool _result;
		private UIElement _parent;

		public void SetParent(UIElement parent)
		{
			_parent = parent;
		}

		public string Message
		{
			get { return (string)GetValue(MessageProperty); }
			set { SetValue(MessageProperty, value); }
		}

		public static readonly DependencyProperty MessageProperty =
			DependencyProperty.Register(
				"Message", typeof(string), typeof(ModalDialog), new UIPropertyMetadata(string.Empty));

		private bool ShowHandlerDialog(string message)
		{
			Message = message;
			Visibility = Visibility.Visible;

			_parent.IsEnabled = false;

			_hideRequest = false;
			while (!_hideRequest)
			{
				// HACK: Stop the thread if the application is about to close
				if (Dispatcher.HasShutdownStarted ||
					Dispatcher.HasShutdownFinished)
				{
					break;
				}

				// HACK: Simulate "DoEvents"
				Dispatcher.Invoke(
					DispatcherPriority.Background,
					new ThreadStart(delegate { }));
				Thread.Sleep(50);
			}

			return _result;
		}
		
        public bool ShowDialog(string message)
        {
            grd_Ok.Visibility = Visibility.Visible;
            grd_YesNo.Visibility = Visibility.Hidden;
            return ShowHandlerDialog(message);
        }

        public bool ShowYesNoDialog(string message)
        {
            grd_Ok.Visibility = Visibility.Hidden;
            grd_YesNo.Visibility = Visibility.Visible;
            return ShowHandlerDialog(message);
        }

        public void SetButtons(string ok, string yes, string no)
        {
            OkButton.Content = ok;
            YesButton.Content = yes;
            NoButton.Content = no;
        }

		private void HideHandlerDialog()
		{
			_hideRequest = true;
			Visibility = Visibility.Hidden;
			_parent.IsEnabled = true;
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			_result = true;
			HideHandlerDialog();
		}

		private void YesButton_Click(object sender, RoutedEventArgs e)
		{
            _result = true;
            HideHandlerDialog();
		}

		private void NoButton_Click(object sender, RoutedEventArgs e)
		{
            _result = false;
            HideHandlerDialog();
		}

	}
}
