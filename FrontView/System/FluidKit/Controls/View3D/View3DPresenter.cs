using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace FluidKit.Controls.View3D
{
	public class View3DPresenter : FrameworkElement
	{
		public static readonly DependencyProperty ThetaProperty = DependencyProperty.Register(
			"Theta", typeof(double), typeof(View3DPresenter), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

		public static readonly DependencyProperty PhiProperty = DependencyProperty.Register(
			"Phi", typeof(double), typeof(View3DPresenter), new FrameworkPropertyMetadata(90.0, FrameworkPropertyMetadataOptions.AffectsRender));

		public static readonly DependencyProperty FocalLengthProperty = DependencyProperty.Register(
			"FocalLength", typeof(double), typeof(View3DPresenter), new FrameworkPropertyMetadata(3000.0, FrameworkPropertyMetadataOptions.AffectsRender));

		private Point _startPoint;
		private bool _isDragging;
		private MapItemVisual3D _hitModel;

		public double Theta
		{
			get { return (double)GetValue(ThetaProperty); }
			set { SetValue(ThetaProperty, value); }
		}

		public double Phi
		{
			get { return (double)GetValue(PhiProperty); }
			set { SetValue(PhiProperty, value); }
		}

		public double FocalLength
		{
			get { return (double)GetValue(FocalLengthProperty); }
			set { SetValue(FocalLengthProperty, value); }
		}

		public VisualCollection Children { get; private set; }
		public View3DPresenter()
		{
			Children = new VisualCollection(this);
			Children.Add(new Axis());
		}


		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			_startPoint = e.GetPosition(this);
			HitTestResult result = VisualTreeHelper.HitTest(this, _startPoint);
			_hitModel = result.VisualHit as MapItemVisual3D;
			_isDragging = true;
		}

		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			_isDragging = false;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (!_isDragging) return;

			Point newPoint = e.GetPosition(this);
			Point delta = new Point(_startPoint.X-newPoint.X, _startPoint.Y-newPoint.Y);

			if (_hitModel != null) // Move model
			{
				_hitModel.Translate(new Point3D(-delta.Y, -delta.X,0));
			}
			else // Move camera
			{
				double phi = delta.Y;
				double theta = delta.X;
				Phi += phi;
				Theta += theta;
			}
			_startPoint = newPoint;
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			return availableSize;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			return finalSize;
		}

		protected override void OnRender(DrawingContext dc)
		{
			dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));

			foreach (MapItemVisual3D child in Children)
			{
				child.RenderModel();
			}
		}

		protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
		{
			if (visualAdded != null)
			{
				MapItemVisual3D item = visualAdded as MapItemVisual3D;
				if (item != null)
				{
					item.View3DParent = this;
					InvalidateVisual();
				}
			}
			if (visualRemoved != null)
			{
				MapItemVisual3D item = visualRemoved as MapItemVisual3D;
				item.View3DParent = null;
			}
		}
		protected override Visual GetVisualChild(int index)
		{
			return Children[index];
		}

		protected override int VisualChildrenCount
		{
			get
			{
				return Children.Count;
			}
		}
	}
}