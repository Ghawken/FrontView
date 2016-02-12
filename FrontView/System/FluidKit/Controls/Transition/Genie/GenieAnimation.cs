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
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using FluidKit.Helpers;

namespace FluidKit.Controls
{
	public enum GenieEffectType
	{
		IntoLamp,
		OutOfLamp
	}

	public class GenieAnimation : Point3DCollectionAnimationBase
	{
		private Point3D MapToMesh(double x, double y)
		{
			double xValue = (x*AspectRatio) - (AspectRatio/2);
			double yValue = 0.5 - y;

			return new Point3D(xValue, yValue, 0);
		}

		private void Initialize()
		{
			_bezierFiller = new BezierMeshFiller();
			_bezierFiller.LeftPoint1 = MapToMesh(LeftPoint1.X, LeftPoint1.Y);
			_bezierFiller.LeftPoint2 = MapToMesh(LeftPoint2.X, LeftPoint2.Y);
			_bezierFiller.LeftPoint3 = MapToMesh(LeftPoint3.X, LeftPoint3.Y);
			_bezierFiller.LeftPoint4 = MapToMesh(LeftPoint4.X, LeftPoint4.Y);

			// Set Right spread positions
			_bezierFiller.RightPoint1 = MapToMesh(RightPoint1.X, RightPoint1.Y);
			_bezierFiller.RightPoint2 = MapToMesh(RightPoint2.X, RightPoint2.Y);
			_bezierFiller.RightPoint3 = MapToMesh(RightPoint3.X, RightPoint3.Y);
			_bezierFiller.RightPoint4 = MapToMesh(RightPoint4.X, RightPoint4.Y);
		}

		protected override Freezable CreateInstanceCore()
		{
			GenieAnimation anim = new GenieAnimation();
			return anim;
		}

		protected override Point3DCollection GetCurrentValueCore(Point3DCollection defaultOriginValue,
		                                                         Point3DCollection defaultDestinationValue,
		                                                         AnimationClock animationClock)
		{
			double progress = animationClock.CurrentProgress.Value;
			if (progress <= 0.0)
			{
				Initialize();
			}
			return GetMeshPositions(progress);
		}

		private Point3DCollection GetMeshPositions(double progress)
		{
			Point3DCollection positions = null;

			switch (EffectType)
			{
				case GenieEffectType.IntoLamp:
					if (progress < 0.5)
					{
						positions = DoSpread(EffectState.SpreadCompress, progress);
					}
					else
					{
						positions = DoSlide(EffectState.SlideDown, progress);
					}
					break;

				case GenieEffectType.OutOfLamp:
					if (progress < 0.5)
					{
						positions = DoSlide(EffectState.SlideUp, progress);
					}
					else
					{
						positions = DoSpread(EffectState.SpreadExpand, progress);
					}
					break;
			}

			return positions;
		}

		private Point3DCollection DoSlide(EffectState direction, double progress)
		{
			double slideT = (direction == EffectState.SlideDown)
			                	?
			                		(progress - 0.5)*2
			                	: 1.0 - (progress*2);

			return _bezierFiller.FillMesh(HorizontalPoints, VerticalPoints, slideT);
		}

		private Point3DCollection DoSpread(EffectState direction, double progress)
		{
			double spreadT = (direction == EffectState.SpreadCompress)
			                 	?
			                 		progress*2
			                 	: 1.0 - ((progress - 0.5)*2);

			// Set Left spread positions
			_bezierFiller.LeftPoint1 = EvaluateSpreadPosition(0, 0, LeftPoint1, spreadT);
			_bezierFiller.LeftPoint2 = EvaluateSpreadPosition(0, 0.333, LeftPoint2, spreadT);
			_bezierFiller.LeftPoint3 = EvaluateSpreadPosition(0, 0.667, LeftPoint3, spreadT);
			_bezierFiller.LeftPoint4 = EvaluateSpreadPosition(0, 1, LeftPoint4, spreadT);

			// Set Right spread positions
			_bezierFiller.RightPoint1 = EvaluateSpreadPosition(1, 0, RightPoint1, spreadT);
			_bezierFiller.RightPoint2 = EvaluateSpreadPosition(1, 0.333, RightPoint2, spreadT);
			_bezierFiller.RightPoint3 = EvaluateSpreadPosition(1, 0.667, RightPoint3, spreadT);
			_bezierFiller.RightPoint4 = EvaluateSpreadPosition(1, 1, RightPoint4, spreadT);

			// Configure mesh
			return _bezierFiller.FillMesh(HorizontalPoints, VerticalPoints, 0);
		}

		private Point3D EvaluateSpreadPosition(double x1, double y1, Point3D point, double spreadT)
		{
			double x2 = point.X;
			double y2 = point.Y;

			LineEvaluator line = new LineEvaluator();
			line.Point1 = MapToMesh(x1, y1);
			line.Point2 = MapToMesh(x2, y2);
			return line.Evaluate(spreadT);
		}

		#region Nested type: EffectState

		private enum EffectState
		{
			SlideUp,
			SlideDown,
			SpreadCompress,
			SpreadExpand,
		}

		#endregion

		#region Dependency Properties

		public static readonly DependencyProperty AspectRatioProperty =
			DependencyProperty.Register("AspectRatio",
			                            typeof (double),
			                            typeof (GenieAnimation),
			                            new PropertyMetadata(1.33));

		private static readonly DependencyProperty EffectTypeProperty =
			DependencyProperty.Register("EffectType",
			                            typeof (GenieEffectType),
			                            typeof (GenieAnimation),
			                            new PropertyMetadata(
			                            	GenieEffectType.IntoLamp));

		private static readonly DependencyProperty HorizontalPointsProperty =
			DependencyProperty.Register("HorizontalPoints",
			                            typeof (int),
			                            typeof (
			                            	GenieAnimation),
			                            new PropertyMetadata
			                            	(25));

		public static readonly DependencyProperty LeftPoint1Property =
			DependencyProperty.Register("LeftPoint1",
			                            typeof (Point3D),
			                            typeof (GenieAnimation),
			                            new PropertyMetadata(
			                            	new Point3D(0, 0, 0)));

		public static readonly DependencyProperty LeftPoint2Property =
			DependencyProperty.Register("LeftPoint2",
			                            typeof (Point3D),
			                            typeof (GenieAnimation),
			                            new PropertyMetadata(
			                            	new Point3D(0, 0.333, 0)));

		public static readonly DependencyProperty LeftPoint3Property =
			DependencyProperty.Register("LeftPoint3",
			                            typeof (Point3D),
			                            typeof (GenieAnimation),
			                            new PropertyMetadata(
			                            	new Point3D(0, 0.667, 0)));

		public static readonly DependencyProperty LeftPoint4Property =
			DependencyProperty.Register("LeftPoint4",
			                            typeof (Point3D),
			                            typeof (GenieAnimation),
			                            new PropertyMetadata(
			                            	new Point3D(0, 1.0, 0)));

		public static readonly DependencyProperty RightPoint1Property =
			DependencyProperty.Register("RightPoint1",
			                            typeof (Point3D),
			                            typeof (GenieAnimation),
			                            new PropertyMetadata(
			                            	new Point3D(1.0, 0, 0)));

		public static readonly DependencyProperty RightPoint2Property =
			DependencyProperty.Register("RightPoint2",
			                            typeof (Point3D),
			                            typeof (GenieAnimation),
			                            new PropertyMetadata(
			                            	new Point3D(1.0, 0.333, 0)));

		public static readonly DependencyProperty RightPoint3Property =
			DependencyProperty.Register("RightPoint3",
			                            typeof (Point3D),
			                            typeof (GenieAnimation),
			                            new PropertyMetadata(
			                            	new Point3D(1.0, 0.667, 0)));

		public static readonly DependencyProperty RightPoint4Property =
			DependencyProperty.Register("RightPoint4",
			                            typeof (Point3D),
			                            typeof (GenieAnimation),
			                            new PropertyMetadata(
			                            	new Point3D(1.0, 1.0, 0)));

		public static readonly DependencyProperty VerticalPointsProperty =
			DependencyProperty.Register("VerticalPoints",
			                            typeof (int),
			                            typeof (GenieAnimation),
			                            new PropertyMetadata(25));

		#endregion

		#region Fields

		private BezierMeshFiller _bezierFiller;

		#endregion

		#region DependencyProperty getter/setter

		public GenieEffectType EffectType
		{
			get { return (GenieEffectType) GetValue(EffectTypeProperty); }
			set { SetValue(EffectTypeProperty, value); }
		}

		public int HorizontalPoints
		{
			get { return (int) GetValue(HorizontalPointsProperty); }
			set { SetValue(HorizontalPointsProperty, value); }
		}

		public int VerticalPoints
		{
			get { return (int) GetValue(VerticalPointsProperty); }
			set { SetValue(VerticalPointsProperty, value); }
		}

		public double AspectRatio
		{
			get { return (double) GetValue(AspectRatioProperty); }
			set { SetValue(AspectRatioProperty, value); }
		}

		public Point3D LeftPoint1
		{
			get { return (Point3D) GetValue(LeftPoint1Property); }
			set { SetValue(LeftPoint1Property, value); }
		}

		public Point3D LeftPoint2
		{
			get { return (Point3D) GetValue(LeftPoint2Property); }
			set { SetValue(LeftPoint2Property, value); }
		}

		public Point3D LeftPoint3
		{
			get { return (Point3D) GetValue(LeftPoint3Property); }
			set { SetValue(LeftPoint3Property, value); }
		}

		public Point3D LeftPoint4
		{
			get { return (Point3D) GetValue(LeftPoint4Property); }
			set { SetValue(LeftPoint4Property, value); }
		}

		public Point3D RightPoint1
		{
			get { return (Point3D) GetValue(RightPoint1Property); }
			set { SetValue(RightPoint1Property, value); }
		}

		public Point3D RightPoint2
		{
			get { return (Point3D) GetValue(RightPoint2Property); }
			set { SetValue(RightPoint2Property, value); }
		}

		public Point3D RightPoint3
		{
			get { return (Point3D) GetValue(RightPoint3Property); }
			set { SetValue(RightPoint3Property, value); }
		}

		public Point3D RightPoint4
		{
			get { return (Point3D) GetValue(RightPoint4Property); }
			set { SetValue(RightPoint4Property, value); }
		}

		#endregion
	}
}