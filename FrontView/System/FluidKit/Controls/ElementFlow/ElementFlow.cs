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
using System.Linq;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace FluidKit.Controls
{
	internal enum AnimationType
	{
		Rotation,
		TranslationX,
		TranslationY,
		TranslationZ
	}

	public enum NavigationDirection
	{
		Left,
		Right,
		Up,
		Down
	}

	public partial class ElementFlow : ItemsControl
	{
		#region Fields
		private ContainerUIElement3D _modelContainer;

		#endregion

		#region Properties

		public LayoutBase Layout
		{
			get { return (LayoutBase) GetValue(LayoutProperty); }
			set { SetValue(LayoutProperty, value); }
		}

		public int SelectedIndex
		{
			get { return (int) GetValue(SelectedIndexProperty); }
			set { SetValue(SelectedIndexProperty, value); }
		}

		public bool Loop
		{
			get { return (bool) GetValue(LoopProperty); }
			set { SetValue(LoopProperty, value); }
		}

		public double TiltAngle
		{
			get { return (double) GetValue(TiltAngleProperty); }
			set { SetValue(TiltAngleProperty, value); }
		}

		public double ItemGap
		{
			get { return (double) GetValue(ItemGapProperty); }
			set { SetValue(ItemGapProperty, value); }
		}

		public double FrontItemGap
		{
			get { return (double) GetValue(FrontItemGapProperty); }
			set { SetValue(FrontItemGapProperty, value); }
		}

		public double PopoutDistance
		{
			get { return (double) GetValue(PopoutDistanceProperty); }
			set { SetValue(PopoutDistanceProperty, value); }
		}

		public double ElementWidth
		{
			get { return (double) GetValue(ElementWidthProperty); }
			set { SetValue(ElementWidthProperty, value); }
		}

		public double ElementHeight
		{
			get { return (double) GetValue(ElementHeightProperty); }
			set { SetValue(ElementHeightProperty, value); }
		}

		public PerspectiveCamera Camera
		{
			get { return (PerspectiveCamera) GetValue(CameraProperty); }
			set { SetValue(CameraProperty, value); }
		}

		/* This gives an accurate count of the number of visible children. 
		 * Panel.Children is not always accurate and is generally off-by-one.
         */

		public bool HasReflection { get; set; }
		internal Viewport3D Viewport { get; private set; }
		private static ResourceDictionary InternalResources { get; set; }

		protected override int VisualChildrenCount
		{
			get
			{
				int count = base.VisualChildrenCount;
				count = (count == 0) ? 0 : 1;
				return count;
			}
		}

		#endregion

		#region Events

		public event EventHandler SelectedIndexChanged;
		#endregion

		#region Commands

		public static RoutedCommand NavigateLeft = new RoutedCommand("NavigateLeft", typeof(ElementFlow),
																	 new InputGestureCollection()
		                                                             	{
		                                                             		new KeyGesture(Key.Left)
		                                                             	});

		public static RoutedCommand NavigateRight = new RoutedCommand("NavigateRight", typeof(ElementFlow),
																	 new InputGestureCollection()
		                                                             	{
		                                                             		new KeyGesture(Key.Right)
		                                                             	});

		public static RoutedCommand NavigateUp = new RoutedCommand("NavigateUp", typeof(ElementFlow),
																	 new InputGestureCollection()
		                                                             	{
		                                                             		new KeyGesture(Key.Up)
		                                                             	});

		public static RoutedCommand NavigateDown = new RoutedCommand("NavigateDown", typeof(ElementFlow),
																	 new InputGestureCollection()
		                                                             	{
		                                                             		new KeyGesture(Key.Down)
		                                                             	});

		#endregion

		#region Dependency Properties

		public static readonly DependencyProperty CameraProperty = DependencyProperty.Register(
			"Camera", typeof(PerspectiveCamera), typeof(ElementFlow),
			new PropertyMetadata(null, OnCameraChanged));

		public static readonly DependencyProperty LayoutProperty =
			DependencyProperty.Register("Layout", typeof(LayoutBase), typeof(ElementFlow),
										new FrameworkPropertyMetadata(null, OnLayoutChanged));

		public static readonly DependencyProperty ElementHeightProperty =
			DependencyProperty.Register("ElementHeight", typeof(double), typeof(ElementFlow),
										new FrameworkPropertyMetadata(800.0));

		public static readonly DependencyProperty ElementWidthProperty =
			DependencyProperty.Register("ElementWidth", typeof(double), typeof(ElementFlow),
										new FrameworkPropertyMetadata(400.0));

		public static readonly DependencyProperty FrontItemGapProperty =
			DependencyProperty.Register("FrontItemGap", typeof(double), typeof(ElementFlow),
										new PropertyMetadata(0.65, OnFrontItemGapChanged));

		public static readonly DependencyProperty HasReflectionProperty =
			DependencyProperty.Register("HasReflection", typeof(bool), typeof(ElementFlow),
										new FrameworkPropertyMetadata(false));

		public static readonly DependencyProperty ItemGapProperty =
			DependencyProperty.Register("ItemGap", typeof(double), typeof(ElementFlow),
										new PropertyMetadata(0.25, OnItemGapChanged));

		private static readonly DependencyProperty LinkedElementProperty =
			DependencyProperty.Register("LinkedElement", typeof(UIElement), typeof(ElementFlow));

		public static readonly DependencyProperty LinkedElementPositionProperty =
			DependencyProperty.Register("LinkedElementPosition", typeof(int), typeof(ElementFlow));

		private static readonly DependencyProperty LinkedModelProperty =
			DependencyProperty.Register("LinkedModel", typeof(ModelUIElement3D), typeof(ElementFlow));

		public static readonly DependencyProperty LoopProperty =
			DependencyProperty.Register("Loop", typeof(bool), typeof(ElementFlow),
										new PropertyMetadata(true));

		public static readonly DependencyProperty PopoutDistanceProperty =
			DependencyProperty.Register("PopoutDistance", typeof(double), typeof(ElementFlow),
										new PropertyMetadata(1.0, OnPopoutDistanceChanged));

		public static readonly DependencyProperty SelectedIndexProperty =
			Selector.SelectedIndexProperty.AddOwner(typeof(ElementFlow),
			new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
				OnSelectedIndexChanged));

		public static readonly DependencyProperty TiltAngleProperty =
			DependencyProperty.Register("TiltAngle", typeof(double), typeof(ElementFlow),
										new PropertyMetadata(45.0, OnTiltAngleChanged));

		#endregion

		static ElementFlow ()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ElementFlow), new FrameworkPropertyMetadata(typeof(ElementFlow)));

			InternalResources = 
				Application.LoadComponent(new Uri("/FluidKit;component/Controls/ElementFlow/InternalResources.xaml", 
								UriKind.Relative)) as ResourceDictionary;
		}

		public ElementFlow ()
		{
			SetupCommands();
			Layout = new CoverFlow();
		}


		#region Command Handling

		private void SetupCommands ()
		{
			CommandBindings.Add(new CommandBinding(NavigateLeft, OnNavigateLeft, CanNavigateLeft));
			CommandBindings.Add(new CommandBinding(NavigateRight, OnNavigateRight, CanNavigateRight));
			CommandBindings.Add(new CommandBinding(NavigateUp, OnNavigateUp, CanNavigateUp));
			CommandBindings.Add(new CommandBinding(NavigateDown, OnNavigateDown, CanNavigateDown));
		}

		private void CanNavigateLeft (object sender, CanExecuteRoutedEventArgs e)
		{
			if (Loop && Layout.Loop) {
				e.CanExecute = true;
			} else {
				e.CanExecute = SelectedIndex > 0;
			}
		}

		private void CanNavigateRight (object sender, CanExecuteRoutedEventArgs e)
		{
			if (Loop && Layout.Loop) {
				e.CanExecute = true;
			} else {
				e.CanExecute = SelectedIndex < Items.Count;
			}
		}

		private void CanNavigateUp (object sender, CanExecuteRoutedEventArgs e)
		{
		}

		private void CanNavigateDown (object sender, CanExecuteRoutedEventArgs e)
		{
		}

		private void OnNavigateLeft (object sender, ExecutedRoutedEventArgs e)
		{
			Navigate(NavigationDirection.Left);
		}

		private void OnNavigateRight (object sender, ExecutedRoutedEventArgs e)
		{
			Navigate(NavigationDirection.Right);
		}

		private void OnNavigateUp (object sender, ExecutedRoutedEventArgs e)
		{
		}

		private void OnNavigateDown (object sender, ExecutedRoutedEventArgs e)
		{
		}

		#endregion

		#region DependencyProperty PropertyChange Callbacks

		private static void OnTiltAngleChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ElementFlow cf = d as ElementFlow;
			cf.ReflowItems();
		}

		private static void OnItemGapChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ElementFlow ef = d as ElementFlow;
			ef.ReflowItems();
		}

		private static void OnFrontItemGapChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ElementFlow ef = d as ElementFlow;
			ef.ReflowItems();
		}

		private static void OnPopoutDistanceChanged (DependencyObject d,
													DependencyPropertyChangedEventArgs e)
		{
			ElementFlow ef = d as ElementFlow;
			ef.ReflowItems();
		}

		private static void OnSelectedIndexChanged (DependencyObject d,
												   DependencyPropertyChangedEventArgs e)
		{
			ElementFlow ef = d as ElementFlow;
			if (ef.IsLoaded == false) 
			{
				return;
			}

			int oldIndex = (int) e.OldValue;
			int newIndex = (int) e.NewValue;

			ef.SelectItemCore(newIndex);
			if (oldIndex != newIndex && ef.SelectedIndexChanged != null) 
			{
				ef.SelectedIndexChanged(ef, new EventArgs());
			}

		}

		private static void OnLayoutChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ElementFlow ef = d as ElementFlow;
			var oldView = e.OldValue as LayoutBase;
			if (oldView != null) 
			{
				oldView.Owner = null;
			}

			LayoutBase newView = e.NewValue as LayoutBase;
			if (newView == null) 
			{
				throw new ArgumentNullException("e", "The Layout cannot be null");
			}

			newView.Owner = ef;
			ef.ReflowItems();
		}

		private static void OnCameraChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ElementFlow ef = d as ElementFlow;

			PerspectiveCamera camera = e.NewValue as PerspectiveCamera;
			if (camera == null) 
			{
				throw new ArgumentNullException("e", "The Camera cannot be null");
			}

			if (ef.IsLoaded)
				ef.Viewport.Camera = camera;
		}

		#endregion

		#region Event Handlers

		private void SetupEventHandlers ()
		{
			_modelContainer.MouseLeftButtonDown += OnContainerLeftButtonDown;
			Loaded += ElementFlow_Loaded;
		}

		private void ElementFlow_Loaded (object sender, RoutedEventArgs e)
		{
			SelectItemCore(SelectedIndex);
			Viewport.Camera = Camera;
		}

		private void OnContainerLeftButtonDown (object sender, MouseButtonEventArgs e)
		{
			Focus();

			ModelUIElement3D model = e.Source as ModelUIElement3D;
			if (model != null) 
			{
				//SelectedIndex = _modelContainer.Children.IndexOf(model);
				SelectedIndex = (int) model.GetValue(LinkedElementPositionProperty);
			}
		}

		protected override void OnMouseWheel (MouseWheelEventArgs e)
		{
			Focus();

			if (e.Delta < 0) 
			{
				Navigate(NavigationDirection.Left);
			} 
			else if (e.Delta > 0) 
			{
				Navigate(NavigationDirection.Right);
			}
		}

		#endregion

		#region Item Selection

		private void SelectItemCore (int index)
		{
			ResortModelItems();

			if (index >= 0 && index < Items.Count) 
			{
				Layout.SelectElement(index);
			}
		}

		internal Storyboard PrepareTemplateStoryboard (int index)
		{
			// Initialize storyboard
			var sb = InternalResources["ElementAnimator"] as Storyboard;
			Rotation3DAnimation rotAnim = sb.Children[0] as Rotation3DAnimation;
			Storyboard.SetTargetProperty(rotAnim, BuildTargetPropertyPath(index, AnimationType.Rotation));

			if (index == SelectedIndex) {
				if (sb.Children.Count >= 7) {
					for (int x = 0; x < 3; x++) {
						// Remove items 1,2,3, making 4,5,6 to be 1,2,3
						sb.Children.RemoveAt(1);
					}
				}
			}

			while (sb.Children.Count > 4) {
				// Remove all items after 3
				sb.Children.RemoveAt(4);
			}

			DoubleAnimation xAnim = sb.Children[1] as DoubleAnimation;
			Storyboard.SetTargetProperty(xAnim, BuildTargetPropertyPath(index, AnimationType.TranslationX));

			DoubleAnimation yAnim = sb.Children[2] as DoubleAnimation;
			Storyboard.SetTargetProperty(yAnim, BuildTargetPropertyPath(index, AnimationType.TranslationY));

			DoubleAnimation zAnim = sb.Children[3] as DoubleAnimation;
			Storyboard.SetTargetProperty(zAnim, BuildTargetPropertyPath(index, AnimationType.TranslationZ));

			return sb;
		}

		private PropertyPath BuildTargetPropertyPath (int index, AnimationType animType)
		{
			PropertyDescriptor childDesc = TypeDescriptor.GetProperties(_modelContainer).Find("Children", 
														true);

			string pathString = string.Empty;

			// Find the REAL index by matching the item index with the position stored in each model item

			Visual3D model = _modelContainer.Children.FirstOrDefault(v => (int) v.GetValue(LinkedElementPositionProperty) == index);
			int realindex = _modelContainer.Children.IndexOf(model);

			switch (animType) 
			{
				case AnimationType.Rotation:
					pathString = "(0)[0].(1)[" + realindex + "].(2).(3)[0].(4)";
					break;
				case AnimationType.TranslationX:
					pathString = "(0)[0].(1)[" + realindex + "].(2).(3)[1].(5)";
					break;
				case AnimationType.TranslationY:
					pathString = "(0)[0].(1)[" + realindex + "].(2).(3)[1].(6)";
					break;
				case AnimationType.TranslationZ:
					pathString = "(0)[0].(1)[" + realindex + "].(2).(3)[1].(7)";
					break;
			}

			return new PropertyPath(pathString,
									Viewport3D.ChildrenProperty,
									childDesc,
									ModelUIElement3D.TransformProperty,
									Transform3DGroup.ChildrenProperty,
									RotateTransform3D.RotationProperty,
									TranslateTransform3D.OffsetXProperty,
									TranslateTransform3D.OffsetYProperty,
									TranslateTransform3D.OffsetZProperty);
		}

		#endregion

		#region Layout overrides

		public override void OnApplyTemplate ()
		{
			base.OnApplyTemplate();
			Viewport = GetTemplateChild("PART_Viewport") as Viewport3D;
			_modelContainer = GetTemplateChild("PART_ModelContainer") as ContainerUIElement3D;

			SetupEventHandlers();
		}

		protected override void PrepareContainerForItemOverride (DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);

			var frkElt = element as FrameworkElement;
			frkElt.Width = ElementWidth;
			frkElt.Height = ElementHeight;

			PrepareModel(frkElt, item);
		}

		protected override void ClearContainerForItemOverride (DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);

			var frkElt = element as FrameworkElement;
			ClearModel(frkElt, item);
		}

		#endregion

		#region Utility functions

		private void ResortModelItems ()
		{
			if (_modelContainer == null || _modelContainer.Children.Count <= 0)
				return;

			System.Collections.Generic.List<Visual3D> items = new System.Collections.Generic.List<Visual3D>(
				from v in _modelContainer.Children orderby (int) v.GetValue(LinkedElementPositionProperty) select v
			);
			_modelContainer.Children.Clear();

			foreach (int index in Layout.DepthSortModelIndices()) {
				_modelContainer.Children.Add(items[index]);
			}
		}

		private void ReflowItems ()
		{
			SelectItemCore(SelectedIndex);
		}

		private void Navigate (NavigationDirection direction)
		{
			int index = -1;

			switch (direction) 
			{
				case NavigationDirection.Left:
					if (Loop && Layout.Loop) {
						index = (SelectedIndex <= 0) ? Items.Count - 1 : SelectedIndex - 1;
					} else {
						index = Math.Max(-1, SelectedIndex - 1);
					}
					break;
				case NavigationDirection.Right:
					if (Loop && Layout.Loop) {
						index = (SelectedIndex >= Items.Count - 1) ? 0 : SelectedIndex + 1;
					} else {
						index = Math.Min(Items.Count - 1, SelectedIndex + 1);
					}
					break;
				case NavigationDirection.Up:
					break;
				case NavigationDirection.Down:
					break;
			}

			if (index != -1) 
			{
				SelectedIndex = index;
			}
		}

		private void ClearModel (FrameworkElement elt, object item)
		{
			ModelUIElement3D model = elt.GetValue(LinkedModelProperty) as ModelUIElement3D;
			_modelContainer.Children.Remove(model);

			// Update the positions of all model items
			int removed = (int) model.GetValue(LinkedElementPositionProperty);
			foreach (Visual3D m in _modelContainer.Children) {
				int val = (int) m.GetValue(LinkedElementPositionProperty);
				if (val > removed) {
					m.SetValue(LinkedElementPositionProperty, val - 1);
				}
			}

			model.ClearValue(LinkedElementProperty);
			elt.ClearValue(LinkedModelProperty);

			// Update SelectedIndex if needed
			if (SelectedIndex >= 0 && SelectedIndex < Items.Count) 
			{
				ReflowItems();
			}
			else 
			{
				SelectedIndex = Math.Max(0, Math.Min(SelectedIndex, Items.Count - 1));
			}
		}

		private void PrepareModel (FrameworkElement elt, object item)
		{
			int index = Items.IndexOf(item);

			// Update the positions of all model items
			foreach (Visual3D m in _modelContainer.Children) {
				int val = (int) m.GetValue(LinkedElementPositionProperty);
				if (val >= index) {
					m.SetValue(LinkedElementPositionProperty, val + 1);
				}
			}

			ModelUIElement3D model = CreateMeshModel(elt);
			model.SetValue(LinkedElementPositionProperty, index);
			_modelContainer.Children.Insert(index, model);

			model.SetValue(LinkedElementProperty, elt);
			elt.SetValue(LinkedModelProperty, model);

			if (IsLoaded) 
			{
				ReflowItems();
			}
		}

		#endregion
	}
}