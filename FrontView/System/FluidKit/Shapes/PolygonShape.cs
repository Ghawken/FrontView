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
using System.Windows.Media;
using System.Windows.Shapes;

namespace FluidKit.Shapes
{
	public class PolygonShape : Shape
	{
		#region ctr

		#endregion

		#region Properties

		#region CornerPointsProperty

		public static readonly DependencyProperty CornerPointsProperty =
			DependencyProperty.Register(
				"CornerPoints",
				typeof (int),
				typeof (PolygonShape),
				new FrameworkPropertyMetadata(3, FrameworkPropertyMetadataOptions.AffectsRender, null,
				                              OnCoerceCornerPoints));

		public int CornerPoints
		{
			get { return (int) GetValue(CornerPointsProperty); }
			set { SetValue(CornerPointsProperty, value); }
		}


		//Making sure that we have at minimum 3 CornerPoints;

		private static object OnCoerceCornerPoints(DependencyObject obj, object baseValue)
		{
			PolygonShape shape = (PolygonShape) obj;
			int value = (int) baseValue;

			if (value < 3)
				value = 3;

			return value;
		}

		#endregion

		#region RotationAngleProperty

		public static readonly DependencyProperty RotationAngleProperty =
			DependencyProperty.Register(
				"RotationAngle",
				typeof (double),
				typeof (PolygonShape),
				new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, null,
				                              OnCoerceRotationAngle));

		public double RotationAngle
		{
			get { return (double) GetValue(RotationAngleProperty); }
			set { SetValue(RotationAngleProperty, value); }
		}


		//Making sure that Rotation is between 0 and 360;

		private static object OnCoerceRotationAngle(DependencyObject obj, object baseValue)
		{
			PolygonShape shape = (PolygonShape) obj;
			double value = (double) baseValue;

			if (value < 0)
				value = 0;

			if (value > 360)
				value = 360;

			return value;
		}

		#endregion

		#region InnerRadiusOffsetProperty

		public static readonly DependencyProperty InnerRadiusOffsetProperty =
			DependencyProperty.Register(
				"InnerRadiusOffset",
				typeof (int),
				typeof (PolygonShape),
				new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender, null,
				                              OnCoerceInnerRadiusOffset));

		public int InnerRadiusOffset
		{
			get { return (int) GetValue(InnerRadiusOffsetProperty); }
			set { SetValue(InnerRadiusOffsetProperty, value); }
		}


		//Restrict OffSetValue between 0 and 200 %;

		private static object OnCoerceInnerRadiusOffset(DependencyObject obj, object baseValue)
		{
			PolygonShape shape = (PolygonShape) obj;
			int value = (int) baseValue;

			if (value < 0)
				value = 0;
			if (value > 200)
				value = 200;

			return value;
		}

		#endregion

		#endregion

		#region Overridden Methods

		protected override Geometry DefiningGeometry
		{
			get { return CreateGeometry(); }
		}

		#endregion

		#region Private HelperFunctions

		private static float GetSin(double degAngle)
		{
			return (float) Math.Sin(Math.PI*degAngle/180);
		}

		private static float GetCos(double degAngle)
		{
			return (float) Math.Cos(Math.PI*degAngle/180);
		}

		private void Swap(ref float val1, ref float val2)
		{
			float temp = val1;
			val1 = val2;
			val2 = temp;
		}

		private StreamGeometry CreateGeometry()
		{
			// Twice as much points for the calculation of intermediate point between cornerpoints

			int cornerPoints = CornerPoints*2;

			// Incrementing angle based on amount of cornerpoints 
			float incrementingAngle = 360f/cornerPoints;


			//Outer radius based on the minium widht or height of the shape

			float outerRadius =
				(float)
				Math.Min(RenderSize.Width/2 - StrokeThickness/2, RenderSize.Height/2 - StrokeThickness/2);


			//innerRadius calculation taking innerRadiusOffset as a percentage offset into account 

			float innerRadiusOffset = (1 - InnerRadiusOffset/100f);
			float innerRadius = GetCos(incrementingAngle)*outerRadius*innerRadiusOffset;


			float rotationAnle = (float) RotationAngle;


			//Calculate and store points for geometry

			float x, y, angle;
			Point[] points = new Point[cornerPoints];

			for (int i = 0; i < cornerPoints; i++)
			{
				//Alternating point on outer and inner radius

				angle = i*incrementingAngle + rotationAnle;

				x = GetCos(angle)*outerRadius;
				y = GetSin(angle)*outerRadius;

				points[i] = new Point(x, y);

				Swap(ref outerRadius, ref innerRadius);
			}


			//Create the geometry 

			StreamGeometry geometry = new StreamGeometry();

			using (StreamGeometryContext ctx = geometry.Open())
			{
				ctx.BeginFigure(points[0], true, true);

				for (int i = 1; i < cornerPoints; i++)
				{
					ctx.LineTo(points[i], true, true);
				}
			}


			//Translate into shape center

			geometry.Transform = new TranslateTransform(outerRadius + StrokeThickness/2,
			                                            outerRadius + StrokeThickness/2);

			return geometry;
		}

		#endregion
	}
}