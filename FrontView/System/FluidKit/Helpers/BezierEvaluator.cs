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
using System.Windows.Media.Media3D;

namespace FluidKit.Helpers
{
	public class BezierEvaluator
	{
		private Point3D _point1;

		private Point3D _point2;

		private Point3D _point3;

		private Point3D _point4;

		public Point3D Point1
		{
			get { return _point1; }
			set { _point1 = value; }
		}

		public Point3D Point2
		{
			get { return _point2; }
			set { _point2 = value; }
		}

		public Point3D Point3
		{
			get { return _point3; }
			set { _point3 = value; }
		}

		public Point3D Point4
		{
			get { return _point4; }
			set { _point4 = value; }
		}

		public Point3D Evaluate(double t)
		{
			// Basis functions
			double b0 = Math.Pow(1 - t, 3);
			double b1 = 3*t*Math.Pow(1 - t, 2);
			double b2 = 3*Math.Pow(t, 2)*(1 - t);
			double b3 = Math.Pow(t, 3);

			Point3D point = new Point3D();
			point.X = Point1.X*b0 + Point2.X*b1 + Point3.X*b2 + Point4.X*b3;
			point.Y = Point1.Y*b0 + Point2.Y*b1 + Point3.Y*b2 + Point4.Y*b3;
			point.Z = Point1.Z*b0 + Point2.Z*b1 + Point3.Z*b2 + Point4.Z*b3;

			return point;
		}
	}
}