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
using System.Windows.Media;

namespace FluidKit.Controls
{
	public class BalloonDecorator : Decorator
	{
		public static readonly DependencyProperty BackgroundProperty =
			DependencyProperty.Register("Background", typeof (Brush), typeof (BalloonDecorator));

		public static readonly DependencyProperty BorderBrushProperty =
			DependencyProperty.Register("BorderBrush", typeof (Brush), typeof (BalloonDecorator));

		public static readonly DependencyProperty CornerRadiusProperty =
			DependencyProperty.Register("CornerRadius", typeof (double), typeof (BalloonDecorator),
			                            new FrameworkPropertyMetadata(10.0,
			                                                          FrameworkPropertyMetadataOptions
			                                                          	.AffectsRender |
			                                                          FrameworkPropertyMetadataOptions
			                                                          	.AffectsMeasure));

		public static readonly DependencyProperty PointerLengthProperty =
			DependencyProperty.Register("PointerLength", typeof (double), typeof (BalloonDecorator),
			                            new FrameworkPropertyMetadata(10.0,
			                                                          FrameworkPropertyMetadataOptions
			                                                          	.AffectsRender |
			                                                          FrameworkPropertyMetadataOptions
			                                                          	.AffectsMeasure));

		private const double Thickness = 0;
		private const int OpeningGap = 10;

		public Brush Background
		{
			get { return (Brush) GetValue(BackgroundProperty); }
			set { SetValue(BackgroundProperty, value); }
		}

		public Brush BorderBrush
		{
			get { return (Brush) GetValue(BorderBrushProperty); }
			set { SetValue(BorderBrushProperty, value); }
		}

		public double PointerLength
		{
			get { return (double) GetValue(PointerLengthProperty); }
			set { SetValue(PointerLengthProperty, value); }
		}

		public double CornerRadius
		{
			get { return (double) GetValue(CornerRadiusProperty); }
			set { SetValue(CornerRadiusProperty, value); }
		}

		protected override Size ArrangeOverride(Size arrangeSize)
		{
			UIElement child = Child;
			if (child != null)
			{
				double pLength = PointerLength;
				Rect innerRect =
					Rect.Inflate(
						new Rect(pLength, 0, Math.Max(0, arrangeSize.Width - pLength), arrangeSize.Height),
						-1*Thickness, -1*Thickness);
				child.Arrange(innerRect);
			}

			return arrangeSize;
		}

		protected override Size MeasureOverride(Size constraint)
		{
			var child = Child;
			var size = new Size();
			if (child != null)
			{
				var innerSize = new Size(Math.Max(0, constraint.Width - PointerLength),
				                          constraint.Height);
				child.Measure(innerSize);
				size.Width += child.DesiredSize.Width;
				size.Height += child.DesiredSize.Height;
			}

			var borderSize = new Size(2*Thickness, 2*Thickness);
			size.Width += borderSize.Width + PointerLength;
			size.Height += borderSize.Height;

			return size;
		}

		protected override void OnRender(DrawingContext dc)
		{
			var rect = new Rect(0, 0, RenderSize.Width, RenderSize.Height);

			dc.PushClip(new RectangleGeometry(rect));
			dc.DrawGeometry(Background, new Pen(BorderBrush, Thickness),
			                CreateBalloonGeometry(rect));
			dc.Pop();
		}

		private StreamGeometry CreateBalloonGeometry(Rect rect)
		{
			double radius = Math.Min(CornerRadius, rect.Height/2);
			double pointerLength = PointerLength;

			// All the points on the path
			Point[] points =
				{
					new Point(pointerLength + radius, 0), new Point(rect.Width - radius, 0), // Top
					new Point(rect.Width, radius), new Point(rect.Width, rect.Height - radius), // Right
					new Point(rect.Width - radius, rect.Height), // Bottom
					new Point(pointerLength + radius, rect.Height), // Bottom
					new Point(pointerLength, rect.Height - radius), // Left
					new Point(pointerLength, radius) // Left
				};

			var geometry = new StreamGeometry {FillRule = FillRule.Nonzero};
		    using (StreamGeometryContext ctx = geometry.Open())
			{
				ctx.BeginFigure(points[0], true, true);
				ctx.LineTo(points[1], true, false);
				ctx.ArcTo(points[2], new Size(radius, radius), 0, false, SweepDirection.Clockwise,
				          true, false);
				ctx.LineTo(points[3], true, false);
				ctx.ArcTo(points[4], new Size(radius, radius), 0, false, SweepDirection.Clockwise,
				          true, false);
				ctx.LineTo(points[5], true, false);

				ctx.ArcTo(points[6], new Size(radius, radius), 0, false, SweepDirection.Clockwise,
				          true, false);

				// Pointer
				if (pointerLength > 0)
				{
					ctx.LineTo(rect.BottomLeft, true, false);
					ctx.LineTo(new Point(pointerLength, rect.Height - radius - OpeningGap), true, false);
				}
				ctx.LineTo(points[7], true, false);

				ctx.ArcTo(points[0], new Size(radius, radius), 0, false, SweepDirection.Clockwise,
				          true, false);
			}
			return geometry;
		}
	}
}