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


//
// Source code courtesy of dakkar from http://www.meedios.com/forum
//

using System;
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
    public partial class VirtElementFlow : VirtualizingPanel, IScrollInfo
    {
        #region Fields
        private ContainerUIElement3D _modelContainer;
        private Viewport3D _viewport;
        public int ItemCount;
        #endregion

        #region Properties

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public int StoredSelectedIndex
        {
            get { return (int)GetValue(StoredSelectedIndexProperty); }
            set { SetValue(StoredSelectedIndexProperty, value); }
        }

        public int NumberVisible
        {
            get { return (int)GetValue(NumberVisibleProperty); }
            set { SetValue(NumberVisibleProperty, value); }
        }

        public double TiltAngle
        {
            get { return (double)GetValue(TiltAngleProperty); }
            set { SetValue(TiltAngleProperty, value); }
        }

        public double ItemGap
        {
            get { return (double)GetValue(ItemGapProperty); }
            set { SetValue(ItemGapProperty, value); }
        }

        public double FrontItemGap
        {
            get { return (double)GetValue(FrontItemGapProperty); }
            set { SetValue(FrontItemGapProperty, value); }
        }

        public double PopoutDistance
        {
            get { return (double)GetValue(PopoutDistanceProperty); }
            set { SetValue(PopoutDistanceProperty, value); }
        }

        public ViewStateBase CurrentView
        {
            get { return (ViewStateBase)GetValue(CurrentViewProperty); }
            set { SetValue(CurrentViewProperty, value); }
        }

        public double ElementWidth
        {
            get { return (double)GetValue(ElementWidthProperty); }
            set { SetValue(ElementWidthProperty, value); }
        }

        public double ElementHeight
        {
            get { return (double)GetValue(ElementHeightProperty); }
            set { SetValue(ElementHeightProperty, value); }
        }

        public PerspectiveCamera Camera
        {
            get { return (PerspectiveCamera)GetValue(CameraProperty); }
            set { SetValue(CameraProperty, value); }
        }

        private ResourceDictionary InternalResources { get; set; }

        internal int VisibleChildrenCount
        {
            get { return _modelContainer.Children.Count; }
        }

        public bool HasReflection { get; set; }

        #endregion

        #region Dependency Properties

        public static DependencyProperty CameraProperty = DependencyProperty.Register(
            "Camera", typeof(PerspectiveCamera), typeof(VirtElementFlow),
            new PropertyMetadata(null, OnCameraChanged));

        public static readonly DependencyProperty CurrentViewProperty =
            DependencyProperty.Register("CurrentView", typeof(ViewStateBase), typeof(VirtElementFlow),
                                        new FrameworkPropertyMetadata(null, OnCurrentViewChanged));

        public static readonly DependencyProperty ElementHeightProperty =
            DependencyProperty.Register("ElementHeight", typeof(double), typeof(VirtElementFlow),
                                        new FrameworkPropertyMetadata(300.0));

        public static readonly DependencyProperty ElementWidthProperty =
            DependencyProperty.Register("ElementWidth", typeof(double), typeof(VirtElementFlow),
                                        new FrameworkPropertyMetadata(400.0));

        public static readonly DependencyProperty FrontItemGapProperty =
            DependencyProperty.Register("FrontItemGap", typeof(double), typeof(VirtElementFlow),
                                        new PropertyMetadata(0.65, OnFrontItemGapChanged));

        public static readonly DependencyProperty HasReflectionProperty =
            DependencyProperty.Register("HasReflection", typeof(bool), typeof(VirtElementFlow),
                                        new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty ItemGapProperty =
            DependencyProperty.Register("ItemGap", typeof(double), typeof(VirtElementFlow),
                                        new PropertyMetadata(0.25, OnItemGapChanged));

        public static readonly DependencyProperty NumberVisibleProperty =
            DependencyProperty.Register("NumberVisible", typeof(int), typeof(VirtElementFlow),
                                        new PropertyMetadata(8, OnNumberVisibleChanged));

        private static readonly DependencyProperty LinkedElementProperty =
            DependencyProperty.Register("LinkedElement", typeof(UIElement), typeof(VirtElementFlow));

        private static readonly DependencyProperty LinkedModelProperty =
            DependencyProperty.Register("LinkedModel", typeof(ModelUIElement3D), typeof(VirtElementFlow));

        public static readonly DependencyProperty PopoutDistanceProperty =
            DependencyProperty.Register("PopoutDistance", typeof(double), typeof(VirtElementFlow),
                                        new PropertyMetadata(1.0, OnPopoutDistanceChanged));

        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(VirtElementFlow),
                                        new PropertyMetadata(-2, OnSelectedIndexChanged));
            //Selector.SelectedIndexProperty.AddOwner(typeof(ElementFlow),
            //new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            //    OnSelectedIndexChanged));

        public static readonly DependencyProperty TiltAngleProperty =
            DependencyProperty.Register("TiltAngle", typeof(double), typeof(VirtElementFlow),
                                        new PropertyMetadata(45.0, OnTiltAngleChanged));

        public static readonly DependencyProperty StoredSelectedIndexProperty =
            DependencyProperty.Register("StoredSelectedIndex", typeof(int), typeof(VirtElementFlow),
                                        new PropertyMetadata(-2, OnStoredSelectedIndexChanged));

        #endregion

        #region Initialization

        public VirtElementFlow()
        {
            CanVerticallyScroll = false;
            LoadViewport();
            SetupEventHandlers();
            CurrentView = new VirtCoverFlow();
        }

        private void SetupEventHandlers()
        {
            _modelContainer.MouseLeftButtonDown += OnContainerLeftButtonDown;
        }

        private void LoadViewport()
        {
            _viewport = Application.LoadComponent(new Uri("/FluidKit;component/Controls/VirtFlow/Viewport.xaml", UriKind.Relative)) as Viewport3D;
            if (_viewport != null)
            {
                InternalResources = _viewport.Resources;
                RenderOptions.SetEdgeMode(_viewport, EdgeMode.Aliased);
                RenderOptions.SetCachingHint(_viewport, CachingHint.Cache);
                RenderOptions.SetBitmapScalingMode(_viewport, BitmapScalingMode.LowQuality);
                // Container for containing the mesh models of elements
                _modelContainer = _viewport.FindName("ModelContainer") as ContainerUIElement3D;
            }
        }

        #endregion

        #region DependencyProperty PropertyChange Callbacks

        private static void OnTiltAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            return;
        }

        private static void OnStoredSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            return;
        }

        private static void OnItemGapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            return;
        }

        private static void OnNumberVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            return;
        }

        private static void OnFrontItemGapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            return;
        }

        private static void OnPopoutDistanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            return;
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ef = d as VirtElementFlow;
            var selectorControl = ItemsControl.GetItemsOwner(ef) as Selector;
            if (selectorControl != null)
            {
                if (ef != null)
                    selectorControl.SelectedIndex = ef.SelectedIndex;
    }
            return;
        }

        private static void OnCurrentViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ef = d as VirtElementFlow;
            var newView = e.NewValue as ViewStateBase;
            if (newView == null) throw new ArgumentNullException("e", @"The CurrentView cannot be null");
            newView.SetOwner(ef);
        }

        private static void OnCameraChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ef = d as VirtElementFlow;
            var camera = e.NewValue as PerspectiveCamera;
            if (camera != null && ef != null) ef._viewport.Camera = camera;
        }

        #endregion

        #region Event Handlers

        protected override void OnInitialized(EventArgs e)
        {
            AddVisualChild(_viewport);
            var itemsControl = ItemsControl.GetItemsOwner(this);
            if (itemsControl != null) ItemCount = itemsControl.HasItems ? itemsControl.Items.Count : 0;      
        }

        public void OnContainerLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var model = e.Source as ModelUIElement3D;
            var selectedIndexPosition = GetPositionIndex(SelectedIndex);
            if (selectedIndexPosition == NumberVisible)
            {
                if (_modelContainer.Children.IndexOf(model) != selectedIndexPosition) SelectedIndex = _modelContainer.Children.IndexOf(model) - selectedIndexPosition + SelectedIndex;
            }
            else
            {
                SelectedIndex = _modelContainer.Children.IndexOf(model);
            }
            InvalidateMeasure();
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                SelectAdjacentItem(false);
            }
            else if (e.Delta > 0)
            {
                SelectAdjacentItem(true);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            return;
        }

        #endregion

        #region Item Animation

        internal Storyboard PrepareTemplateStoryboard(int index)
        {
            // Initialize storyboard
            var sb = ((Storyboard) InternalResources["ElementAnimator"]).Clone();

            var rotAnim = sb.Children[0] as Rotation3DAnimation;
            if (rotAnim != null) Storyboard.SetTargetProperty(rotAnim, BuildTargetPropertyPath(index, "Rotation"));

            var xAnim = sb.Children[1] as DoubleAnimation;
            if (xAnim != null) Storyboard.SetTargetProperty(xAnim, BuildTargetPropertyPath(index, "Translation-X"));

            var yAnim = sb.Children[2] as DoubleAnimation;
            if (yAnim != null) Storyboard.SetTargetProperty(yAnim, BuildTargetPropertyPath(index, "Translation-Y"));

            var zAnim = sb.Children[3] as DoubleAnimation;
            if (zAnim != null) Storyboard.SetTargetProperty(zAnim, BuildTargetPropertyPath(index, "Translation-Z"));

            return sb;
        }

        internal Storyboard PrepareTemplateStoryboard2(int index)
        {
            // Initialize storyboard
            var sb = ((Storyboard) InternalResources["ElementInsert"]).Clone();

            var rotAnim = sb.Children[0] as Rotation3DAnimation;
            if (rotAnim != null) Storyboard.SetTargetProperty(rotAnim, BuildTargetPropertyPath(index, "Rotation"));

            var xAnim = sb.Children[1] as DoubleAnimation;
            if (xAnim != null) Storyboard.SetTargetProperty(xAnim, BuildTargetPropertyPath(index, "Translation-X"));

            var yAnim = sb.Children[2] as DoubleAnimation;
            if (yAnim != null) Storyboard.SetTargetProperty(yAnim, BuildTargetPropertyPath(index, "Translation-Y"));

            var zAnim = sb.Children[3] as DoubleAnimation;
            if (zAnim != null) Storyboard.SetTargetProperty(zAnim, BuildTargetPropertyPath(index, "Translation-Z"));

            return sb;
        }

        private PropertyPath BuildTargetPropertyPath(int index, string animType)
        {
            var childDesc = TypeDescriptor.GetProperties(_modelContainer).Find("Children", true);
            var pathString = string.Empty;
            switch (animType)
            {
                case "Rotation":
                    pathString = "(0)[0].(1)[" + index + "].(2).(3).(4)[0].(5)";
                    break;
                case "Translation-X":
                    pathString = "(0)[0].(1)[" + index + "].(2).(3).(4)[1].(6)";
                    break;
                case "Translation-Y":
                    pathString = "(0)[0].(1)[" + index + "].(2).(3).(4)[1].(7)";
                    break;
                case "Translation-Z":
                    pathString = "(0)[0].(1)[" + index + "].(2).(3).(4)[1].(8)";
                    break;
            }

            return new PropertyPath(pathString,
                                    Viewport3D.ChildrenProperty,
                                    childDesc,
                                    ModelUIElement3D.ModelProperty,
                                    Model3D.TransformProperty,
                                    Transform3DGroup.ChildrenProperty,
                                    RotateTransform3D.RotationProperty,
                                    TranslateTransform3D.OffsetXProperty,
                                    TranslateTransform3D.OffsetYProperty,
                                    TranslateTransform3D.OffsetZProperty);
        }

        internal void AnimateElement(Storyboard sb)
        {
            sb.Begin(_viewport);
        }

        #endregion

        #region Layout overrides

        protected override int VisualChildrenCount
        {
            get
            {
                var count = base.VisualChildrenCount;
                count = (count == 0) ? 0 : 1;
                return count;
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var eltSize = new Size(ElementWidth, ElementHeight);
            for (var i = 0; i < Children.Count; i++)
            {
                var child = Children[i];
                child.Arrange(new Rect(new Point(), eltSize));
            }
            _viewport.Arrange(new Rect(new Point(), finalSize));
            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var itemsControl = ItemsControl.GetItemsOwner(this);
            var itemCount = itemsControl.HasItems ? itemsControl.Items.Count : 0;
            var firstVisibleItemIndex = Math.Max(SelectedIndex - NumberVisible, 0);
            var lastVisibleItemIndex = Math.Min(SelectedIndex + NumberVisible, itemCount - 1);

            var children = InternalChildren;
            var generator = ItemContainerGenerator;
            var startPos = generator.GeneratorPositionFromIndex(firstVisibleItemIndex);
            var childIndex = (startPos.Offset == 0) ? startPos.Index : startPos.Index + 1;
            var inserted = 0;
            using (generator.StartAt(startPos, GeneratorDirection.Forward, true))
            {
                for (var itemIndex = firstVisibleItemIndex; itemIndex <= lastVisibleItemIndex; ++itemIndex, ++childIndex)
                {
                    bool newlyRealized;
                    // Get or create the child
                    var child = generator.GenerateNext(out newlyRealized) as UIElement;
                    if (newlyRealized)
                    {
                        // Figure out if we need to insert the child at the end or somewhere in the middle
                        if (childIndex >= children.Count)
                        {
                            AddInternalChild(child);
                            inserted++;
                        }
                        else
                        {
                            InsertInternalChild(childIndex, child);
                            inserted--;
                        }
                        if (child != null) generator.PrepareItemContainer(child);
                    }
                }
            }
            CleanUpItems(firstVisibleItemIndex, lastVisibleItemIndex);

            if (StoredSelectedIndex != SelectedIndex)
            {
                StoredSelectedIndex = SelectedIndex;
                CurrentView.Anima(GetPositionIndex(SelectedIndex), NumberVisible, inserted);
            }
            if (availableSize.Width == double.PositiveInfinity || availableSize.Height == double.PositiveInfinity) return new Size();
            return availableSize;
        }

        public int GetPositionIndex(int index)
        {
            var generator = ItemContainerGenerator;
            //var itemsControl = ItemsControl.GetItemsOwner(this);
            var position = generator.GeneratorPositionFromIndex(index);
            return position.Index;
        }

        private void CleanUpItems(int minDesiredGenerated, int maxDesiredGenerated)
        {
            var generator = ItemContainerGenerator;
            for (var i = InternalChildren.Count - 1; i >= 0; i--)
            {
                var childGeneratorPos = new GeneratorPosition(i, 0);
                var itemIndex = generator.IndexFromGeneratorPosition(childGeneratorPos);
                if (itemIndex < minDesiredGenerated || itemIndex > maxDesiredGenerated)
                {
                    RemoveInternalChildRange(i, 1);
                    generator.Remove(childGeneratorPos, 1);
                }
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            return index == 0 ? _viewport : null;
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
            if (visualAdded != null) OnVisualAdded(visualAdded as UIElement);
            if (visualRemoved != null) OnVisualRemoved(visualRemoved as UIElement);
        }

        #endregion

        #region Utility functions

        private void SelectAdjacentItem(bool isNext)
        {
            var index = isNext == false ? Math.Max(-1, SelectedIndex - 1) : Math.Min(ItemCount - 1, SelectedIndex + 1);

            if (index != -1)
            {
                SelectedIndex = index;
            }
        }

        private void OnVisualRemoved(UIElement elt)
        {
            var model = elt.GetValue(LinkedModelProperty) as ModelUIElement3D;
            if (model != null)
            {
                _modelContainer.Children.Remove(model);
                model.ClearValue(LinkedElementProperty);
            }
            elt.ClearValue(LinkedModelProperty);
        }

        private void OnVisualAdded(UIElement elt)
        {

            if (elt is Viewport3D) return;
            var index = Children.IndexOf(elt);
            var model = CreateMeshModel(elt);
            _modelContainer.Children.Insert(index, model);

            model.SetValue(LinkedElementProperty, elt);
            elt.SetValue(LinkedModelProperty, model);
        }

        #endregion

        #region IScrollInfo Membri di

        public ScrollViewer ScrollOwner { get; set; }

        private bool _canHScroll = true;
        public bool CanHorizontallyScroll
        {
            get { return _canHScroll; }
            set { _canHScroll = value; }
        }

        public bool CanVerticallyScroll { get; set; }

        public void LineUp()
        {
        }

        public void LineDown()
        {
        }

        public void LineLeft()
        {
        }

        public void LineRight()
        {
        }

        public Rect MakeVisible(Visual visual, Rect rectangle)
        {
            var igenerator = ItemContainerGenerator;
            if (igenerator == null) return Rect.Empty;
            var generator = igenerator.GetItemContainerGeneratorForPanel(this);
            if (generator == null) return Rect.Empty;
            var itemIndex = generator.IndexFromContainer(visual);
            // in virtualizing panels visual might already be detached from ItemContainerGenerator, itemIndex would be -1.
            if (itemIndex == -1)
            {
                // so the item's content is looked up in the base list.
                var cc = visual as ContentControl;
                if (cc != null)
                {
                    var c = cc.DataContext; // or Content? Content can still be null while DataContext is already non-null...
                    var itemsControl = ItemsControl.GetItemsOwner(this);
                    itemIndex = (itemsControl != null && itemsControl.HasItems) ? itemsControl.Items.IndexOf(c) : -1;
                }
            }
            if (itemIndex != -1)
            {
                if (SelectedIndex != itemIndex)
                {
                    SelectedIndex = itemIndex;
                    if (StoredSelectedIndex != SelectedIndex)
                    {
                        InvalidateMeasure();
                        //StoredSelectedIndex = SelectedIndex;
                    }
                }
            }

            return rectangle;
        }

        public void MouseWheelUp()
        {
        }

        public void MouseWheelDown()
        {
        }

        public void MouseWheelLeft()
        {
        }

        public void MouseWheelRight()
        {
        }

        public void PageUp()
        {
        }

        public void PageDown()
        {
        }

        public void PageLeft()
        {
        }

        public void PageRight()
        {
        }

        private Point _offset;
        private Size _extent = new Size(0, 0);
        private Size _viewportsize = new Size(5000, 500);

        public void SetHorizontalOffset(double offset)
        {
            return;
        }

        public void SetVerticalOffset(double offset)
        {
            return;
        }

        public double HorizontalOffset
        {
            get { return _offset.X; }
        }

        public double VerticalOffset
        {
            get { return _offset.Y; }
        }

        public double ExtentHeight
        {
            get { return _extent.Height; }
        }

        public double ExtentWidth
        {
            get { return _extent.Width; }
        }

        public double ViewportHeight
        {
            get { return _viewportsize.Height; }
        }

        public double ViewportWidth
        {
            get { return _viewportsize.Width; }
        }

        #endregion
    }
}