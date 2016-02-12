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
	public class CogWheelShape : Shape
	{
		#region ctr

		#endregion

		#region Properties

		#region TeethProperty

		public static readonly DependencyProperty TeethProperty =
			DependencyProperty.Register(
				"Teeth",
				typeof (int),
				typeof (CogWheelShape),
				new FrameworkPropertyMetadata(2, FrameworkPropertyMetadataOptions.AffectsRender, null,
				                              OnCoerceTeeth));

		public int Teeth
		{
			get { return (int) GetValue(TeethProperty); }
			set { SetValue(TeethProperty, value); }
		}


		//Making sure that we have at minimum 2 Teeth;

		private static object OnCoerceTeeth(DependencyObject obj, object baseValue)
		{
			CogWheelShape shape = (CogWheelShape) obj;
			int value = (int) baseValue;

			if (value < 2)
				value = 2;

			return value;
		}

		#endregion

		#region RotationAngleProperty

		public static readonly DependencyProperty RotationAngleProperty =
			DependencyProperty.Register(
				"RotationAngle",
				typeof (double),
				typeof (CogWheelShape),
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
			CogWheelShape shape = (CogWheelShape) obj;
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
				typeof (CogWheelShape),
				new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender, null,
				                              OnCoercePercentageValue));

		public int InnerRadiusOffset
		{
			get { return (int) GetValue(InnerRadiusOffsetProperty); }
			set { SetValue(InnerRadiusOffsetProperty, value); }
		}

		#endregion

		#region MiddleRadiusOffsetProperty

		public static readonly DependencyProperty MiddleRadiusOffsetProperty =
			DependencyProperty.Register(
				"MiddleRadiusOffset",
				typeof (int),
				typeof (CogWheelShape),
				new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender, null,
				                              OnCoercePercentageValue));

		public int MiddleRadiusOffset
		{
			get { return (int) GetValue(MiddleRadiusOffsetProperty); }
			set { SetValue(MiddleRadiusOffsetProperty, value); }
		}

		#endregion

		#region BevelProperty

		public static readonly DependencyProperty BevelProperty =
			DependencyProperty.Register(
				"Bevel",
				typeof (int),
				typeof (CogWheelShape),
				new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender, null,
				                              OnCoercePercentageValue));

		public int Bevel
		{
			get { return (int) GetValue(BevelProperty); }
			set { SetValue(BevelProperty, value); }
		}

		#endregion

		//Restrict percentage values between 0 and 100 for Bevel, MiddleRadiusOffset, InnerRadiusOffset;

		private static object OnCoercePercentageValue(DependencyObject obj, object baseValue)
		{
			CogWheelShape shape = (CogWheelShape) obj;
			int value = (int) baseValue;

			if (value < 0)
				value = 0;
			if (value > 100)
				value = 100;

			return value;
		}

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

		private void Swap(ref Point val1, ref Point val2)
		{
			Point temp = val1;
			val1 = val2;
			val2 = temp;
		}


		private StreamGeometry CreateGeometry()
		{
			// Twice as much corner points for every Tooth

			int cornerPoints = Teeth*2;

			// Incrementing angle based on amount of cornerPoints

			float incrementingAngle = 360f/cornerPoints;


			//Outer radius based on the minium width or height of the shape

			float outerRadius =
				(float)
				Math.Min(RenderSize.Width/2 - StrokeThickness/2, RenderSize.Height/2 - StrokeThickness/2);


			//middleRadius calculation taking MiddleRadiusOffset as a percentage offset from outerRadius into account 

			float middleRadiusOffset = (1 - MiddleRadiusOffset/100f);
			float middleRadius = outerRadius*middleRadiusOffset;

			//innerRadius calculation taking InnerRadiusOffset as a percentage offset from middleRadius into account 

			float innerRadiusOffset = (1 - InnerRadiusOffset/100f);
			float innerRadius = middleRadius*innerRadiusOffset;

			//Bevel Angle calculation as  percentage amount of the half the incrementingAngle;

			float bevel = (1 - Bevel/100f);
			float bevelAngle = incrementingAngle/2*bevel;


			//rotationAngle for possible rotation adjustments for convencience of shape use

			float rotationAngle = (float) RotationAngle;


			//Array of three times as much cornerpoints to store intermediate points on each radius for the given incrementingAngle

			Point[] points = new Point[cornerPoints*3];


			//Calculate and store points for geometry


			float anglePointX, anglePointY;
			float angleBevelPointX, angleBevelPointY;
			float angle;
			float x, y;

			for (int i = 0; i < cornerPoints; i++)
			{
				angle = i*incrementingAngle + rotationAngle;

				anglePointX = GetCos(angle);
				anglePointY = GetSin(angle);

				angleBevelPointX = GetCos(angle - bevelAngle);
				angleBevelPointY = GetSin(angle - bevelAngle);


				//For every iteration step calculate one point on each radius for the given angle 


				//BevelPoint on outer radius

				x = angleBevelPointX*outerRadius;
				y = angleBevelPointY*outerRadius;

				points[i*3] = new Point(x, y);

				//point on middle radius

				x = anglePointX*middleRadius;
				y = anglePointY*middleRadius;

				points[i*3 + 1] = new Point(x, y);

				//point on inner radius

				x = anglePointX*innerRadius;
				y = anglePointY*innerRadius;

				points[i*3 + 2] = new Point(x, y);


				//every second iteration swap stored points on inner and outer radius to reflect 
				//the direction of points for ease of later geometry creation 

				if (i%2 != 0)
				{
					Swap(ref points[i*3], ref points[i*3 + 2]);
				}

				//Alternate positive and negative bevelAngle;
				bevelAngle *= -1;
			}


			//Create the geometry 

			StreamGeometry geometry = new StreamGeometry();

			using (StreamGeometryContext ctx = geometry.Open())
			{
				ctx.BeginFigure(points[0], true, true);

				for (int i = 1; i < points.Length; i++)
				{
					ctx.LineTo(points[i], true, true);

					//ctx.ArcTo(points[i],new Size(outerRadius,outerRadius),0,false,SweepDirection.Clockwise,true,true);
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