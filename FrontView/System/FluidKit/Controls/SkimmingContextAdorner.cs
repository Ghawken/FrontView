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
using System.Windows.Media;

namespace FluidKit.Controls
{
	internal class SkimmingContextAdorner : Adorner
	{
		private ContentControl _container;
		private Panel _contextPanel;
		private ItemSkimmingPanel _skimmingPanel;

		internal SkimmingContextAdorner(ItemSkimmingPanel adornedPanel)
			: base(adornedPanel)
		{
			_skimmingPanel = adornedPanel;
			this.Initialized += SkimmingContextAdorner_Initialized;
		}

		private void SkimmingContextAdorner_Initialized(object sender, EventArgs e)
		{
			CreateContextContainer();

			// Add visuals
			foreach (UIElement elt in _skimmingPanel.Children)
			{
				AddContextVisual(elt);
			}
		}

		private void CreateContextContainer()
		{
			_container = new ContentControl();
			_container.Template = _skimmingPanel.ContextContainerTemplate;
			bool applied = _container.ApplyTemplate();
			_contextPanel = _container.Template.FindName("PART_ContextPanel", _container) as Panel;

			// For visual updates
			AddVisualChild(_container);
		}

		internal void AddContextVisual(Visual visual)
		{
			// Set up the visual
			ContentControl item = new ContentControl();
			item.Background = new VisualBrush(visual);
			item.Template = _skimmingPanel.ContextItemTemplate;
			item.FocusVisualStyle = null;

			_contextPanel.Children.Add(item);
		}

		internal void SelectItem(int index, int prevIndex)
		{
			ContentControl currItem = _contextPanel.Children[index] as ContentControl;
			currItem.Focus();
		}

		#region Layout Overrides

		protected override int VisualChildrenCount
		{
			get { return 1; }
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			_container.Arrange(new Rect(0, -_container.DesiredSize.Height - 5, finalSize.Width,
			                            finalSize.Height));
			return finalSize;
		}

		protected override Size MeasureOverride(Size constraint)
		{
			_container.Measure(constraint);
			return _container.DesiredSize;
		}

		protected override Visual GetVisualChild(int index)
		{
			return _container;
		}

		#endregion
	}
}