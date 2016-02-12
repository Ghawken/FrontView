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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace FluidKit.Controls
{
	public class ItemSkimmingPanel : Panel
	{
		public static readonly DependencyProperty ContextContainerTemplateProperty =
			DependencyProperty.Register("ContextContainerTemplate", typeof (ControlTemplate),
			                            typeof (ItemSkimmingPanel));

		public static readonly DependencyProperty ContextItemTemplateProperty =
			DependencyProperty.Register("ContextItemTemplate", typeof (ControlTemplate),
			                            typeof (ItemSkimmingPanel));

		private static readonly DependencyProperty ShowContextPanelProperty =
			DependencyProperty.Register("ShowContextPanel", typeof (bool),
			                            typeof (ItemSkimmingPanel),
			                            new FrameworkPropertyMetadata(false,
			                                                          new PropertyChangedCallback(
			                                                          	OnShowContextPanelChanged)));

		private SkimmingContextAdorner _contextAdorner;
		private int _previousIndex = -1;
		private int _selectedIndex;
		private bool _skimmingStarted;
		private Point _startPosition;

		public ItemSkimmingPanel()
		{
			AttachEventHandlers();
		}

		private bool ShowContextPanel
		{
			get { return (bool) GetValue(ShowContextPanelProperty); }
			set { SetValue(ShowContextPanelProperty, value); }
		}

		public ControlTemplate ContextContainerTemplate
		{
			get { return (ControlTemplate) GetValue(ContextContainerTemplateProperty); }
			set { SetValue(ContextContainerTemplateProperty, value); }
		}

		public ControlTemplate ContextItemTemplate
		{
			get { return (ControlTemplate) GetValue(ContextItemTemplateProperty); }
			set { SetValue(ContextItemTemplateProperty, value); }
		}

		private static void OnShowContextPanelChanged(DependencyObject d,
		                                              DependencyPropertyChangedEventArgs args)
		{
			ItemSkimmingPanel skimmingPanel = d as ItemSkimmingPanel;
			if ((bool) args.NewValue)
			{
				AdornerLayer layer = AdornerLayer.GetAdornerLayer(skimmingPanel);
				skimmingPanel._contextAdorner = new SkimmingContextAdorner(skimmingPanel);

				layer.Add(skimmingPanel._contextAdorner);
				skimmingPanel._contextAdorner.SelectItem(skimmingPanel._selectedIndex,
				                                         skimmingPanel._previousIndex);
			}
			else
			{
				AdornerLayer layer = AdornerLayer.GetAdornerLayer(skimmingPanel);
				if (skimmingPanel._contextAdorner != null)
				{
					layer.Remove(skimmingPanel._contextAdorner);
				}
				skimmingPanel._contextAdorner = null;
			}
		}

		private void AttachEventHandlers()
		{
			PreviewMouseLeftButtonDown += ItemSkimmingPanel_PreviewMouseLeftButtonDown;
			PreviewMouseLeftButtonUp += ItemSkimmingPanel_PreviewMouseLeftButtonUp;
			PreviewMouseMove += ItemSkimmingPanel_PreviewMouseMove;
		}

		private void SelectItem(bool skimForward)
		{
			if (skimForward)
			{
				_selectedIndex = Math.Min(InternalChildren.Count - 1, _previousIndex + 1);
			}
			else
			{
				_selectedIndex = Math.Max(0, _previousIndex - 1);
			}
			SetZIndex(InternalChildren[_previousIndex], 0);
			SetZIndex(InternalChildren[_selectedIndex], 1);

			// Update adorner with the selection
			if (_selectedIndex != _previousIndex && ShowContextPanel)
			{
				_contextAdorner.SelectItem(_selectedIndex, _previousIndex);
			}

			_previousIndex = _selectedIndex;
		}

		private void UpdateSelectionIndex(int children)
		{
			_selectedIndex = _previousIndex = children - 1;
		}

		#region Layout Overrides

		protected override Size ArrangeOverride(Size finalSize)
		{
			foreach (UIElement elt in InternalChildren)
			{
				elt.Arrange(new Rect(new Point(), finalSize));
			}

			return finalSize;
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			foreach (UIElement elt in InternalChildren)
			{
				elt.Measure(availableSize);
			}

			return availableSize;
		}

		protected override void OnVisualChildrenChanged(DependencyObject visualAdded,
		                                                DependencyObject visualRemoved)
		{
			base.OnVisualChildrenChanged(visualAdded, visualRemoved);

			if (visualAdded != null) // after addition
			{
				UpdateSelectionIndex(InternalChildren.Count);
			}

			if (visualRemoved != null) // before deletion
			{
				UpdateSelectionIndex(InternalChildren.Count - 1);
			}
		}

		#endregion

		#region Event Handlers

		private void ItemSkimmingPanel_PreviewMouseLeftButtonUp(object sender,
		                                                        MouseButtonEventArgs e)
		{
			ShowContextPanel = false;
			_skimmingStarted = false;
			ReleaseMouseCapture();
		}

		private void ItemSkimmingPanel_PreviewMouseLeftButtonDown(object sender,
		                                                          MouseButtonEventArgs e)
		{
			_startPosition = e.GetPosition(this);
			if (Keyboard.Modifiers == ModifierKeys.Control)
			{
				ShowContextPanel = true;
			}
			_skimmingStarted = true;
			CaptureMouse();
		}

		private void ItemSkimmingPanel_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			if (_skimmingStarted == false)
			{
				return;
			}

			Point currPosition = e.GetPosition(this);
			double delta = currPosition.X - _startPosition.X;
			if (Math.Abs(delta) > SystemParameters.MinimumHorizontalDragDistance)
			{
				bool skimForward = delta > 0;
				SelectItem(skimForward);
				_startPosition = currPosition;
			}
		}

		#endregion
	}
}