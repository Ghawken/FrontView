using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace FluidKit.Controls
{
	internal enum LocationType
	{
		Top,
		Anchor,
		Bottom
	}

	public enum PanelOrientation
	{
		Left,
		Right
	}

	public class HalfCirclePanel : Panel
	{
		private const double MaxAngle = Math.PI / 2;

		public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(
			"ItemWidth", typeof(double), typeof(HalfCirclePanel), new FrameworkPropertyMetadata(48.0, FrameworkPropertyMetadataOptions.AffectsArrange));

		public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(
			"ItemHeight", typeof(double), typeof(HalfCirclePanel), new FrameworkPropertyMetadata(48.0, FrameworkPropertyMetadataOptions.AffectsArrange));

		public static readonly DependencyProperty ItemGapProperty = DependencyProperty.Register(
			"ItemGap", typeof(double), typeof(HalfCirclePanel), new FrameworkPropertyMetadata(60.0, FrameworkPropertyMetadataOptions.AffectsArrange));

		public static readonly DependencyProperty NavigationOffsetProperty = DependencyProperty.Register(
			"NavigationOffset", typeof(double), typeof(HalfCirclePanel), new FrameworkPropertyMetadata(-1.0, FrameworkPropertyMetadataOptions.AffectsArrange));

		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
			"Orientation", typeof(PanelOrientation), typeof(HalfCirclePanel), new FrameworkPropertyMetadata(PanelOrientation.Left, FrameworkPropertyMetadataOptions.AffectsArrange));

		public PanelOrientation Orientation
		{
			get { return (PanelOrientation)GetValue(OrientationProperty); }
			set { SetValue(OrientationProperty, value); }
		}

		public double NavigationOffset
		{
			get { return (double)GetValue(NavigationOffsetProperty); }
			set { SetValue(NavigationOffsetProperty, value); }
		}

		public double ItemWidth
		{
			get { return (double)GetValue(ItemWidthProperty); }
			set { SetValue(ItemWidthProperty, value); }
		}

		public double ItemHeight
		{
			get { return (double)GetValue(ItemHeightProperty); }
			set { SetValue(ItemHeightProperty, value); }
		}

		public double ItemGap
		{
			get { return (double)GetValue(ItemGapProperty); }
			set { SetValue(ItemGapProperty, value); }
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			if (availableSize.Width == double.PositiveInfinity || availableSize.Height == double.PositiveInfinity)
				return new Size();

			foreach (UIElement child in InternalChildren)
			{
				child.Measure(new Size(ItemWidth, ItemHeight));
			}

			return availableSize;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			if (InternalChildren.Count > 0)
			{
				// Top
				for (int i = InternalChildren.Count - 1; i > NavigationOffset; i--)
				{
					PositionChild(LocationType.Top, i, finalSize);
				}

				// Anchor
				PositionChild(LocationType.Anchor, (int)NavigationOffset, finalSize);

				// Bottom
				for (int i = 0; i < NavigationOffset; i++)
				{
					PositionChild(LocationType.Bottom, i, finalSize);
				}
			}

			return finalSize;
		}

		private void PositionChild(LocationType location, int index, Size panelSize)
		{
			double radius = panelSize.Height / 2;
			double itemGapAngle = Math.Atan(ItemGap / radius);

			var delta = NavigationOffset - index;
			if (location == LocationType.Anchor)
				delta = 0;

			var info = new ItemInfo()
			           	{
			           		Radius = radius,
			           		ItemAngle = itemGapAngle * delta,
			           		PanelSize = panelSize,
			           	};


			var rect = new Rect();

			// Lies within the field of view ?
			if (info.ItemAngle > -MaxAngle && info.ItemAngle < MaxAngle)
			{
				rect = Orientation == PanelOrientation.Left
				       	? CalculateRectForLeftOrientation(info)
				       	: CalculateRectForRightOrientation(info);
			}

			InternalChildren[index].Arrange(rect);
		}

		private Rect CalculateRectForLeftOrientation(ItemInfo info)
		{
			var x = info.Radius * Math.Cos(info.ItemAngle);
			var y = info.Radius * Math.Sin(info.ItemAngle) - ItemHeight / 2;

			return new Rect(info.PanelSize.Width - x, info.PanelSize.Height / 2 + y, ItemWidth, ItemHeight);
		}

		private Rect CalculateRectForRightOrientation(ItemInfo info)
		{
			var x = info.Radius * Math.Cos(info.ItemAngle) - ItemWidth;
			var y = info.Radius * Math.Sin(info.ItemAngle) - ItemHeight / 2;

			return new Rect(x, info.PanelSize.Height / 2 + y, ItemWidth, ItemHeight);
		}

		public void AnimateToOffset(double offset)
		{
			Storyboard sb = new Storyboard();
			var anim = new DoubleAnimation(offset, new Duration(TimeSpan.FromSeconds(0.5)));
			Storyboard.SetTarget(anim, this);
			Storyboard.SetTargetProperty(anim, new PropertyPath("NavigationOffset"));

			sb.Children.Add(anim);

			EventHandler handler = null;
			handler = delegate
			          	{
			          		NavigationOffset = offset;
			          		sb.Completed -= handler;
			          		sb.Remove();
			          	};

			sb.Completed += handler;

			sb.Begin();
		}

		private struct ItemInfo
		{
			public Size PanelSize { get; set; }
			public double ItemAngle { get; set; }
			public double Radius { get; set; }
		}
	}
}