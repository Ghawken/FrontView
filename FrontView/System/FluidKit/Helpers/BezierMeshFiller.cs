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
using System.Windows.Media.Media3D;

namespace FluidKit.Helpers
{
	public class BezierMeshFiller
	{
		#region Fields

		private Point3D _leftPoint1;
		private Point3D _leftPoint2;
		private Point3D _leftPoint3;
		private Point3D _leftPoint4;

		private Point3D _rightPoint1;
		private Point3D _rightPoint2;
		private Point3D _rightPoint3;
		private Point3D _rightPoint4;

		#endregion

		#region Properties

		public Point3D LeftPoint1
		{
			get { return _leftPoint1; }
			set { _leftPoint1 = value; }
		}

		public Point3D LeftPoint2
		{
			get { return _leftPoint2; }
			set { _leftPoint2 = value; }
		}

		public Point3D LeftPoint3
		{
			get { return _leftPoint3; }
			set { _leftPoint3 = value; }
		}

		public Point3D LeftPoint4
		{
			get { return _leftPoint4; }
			set { _leftPoint4 = value; }
		}

		public Point3D RightPoint1
		{
			get { return _rightPoint1; }
			set { _rightPoint1 = value; }
		}

		public Point3D RightPoint2
		{
			get { return _rightPoint2; }
			set { _rightPoint2 = value; }
		}

		public Point3D RightPoint3
		{
			get { return _rightPoint3; }
			set { _rightPoint3 = value; }
		}

		public Point3D RightPoint4
		{
			get { return _rightPoint4; }
			set { _rightPoint4 = value; }
		}

		#endregion

		public Point3DCollection FillMesh(int xVertices, int yVertices, double startT)
		{
			BezierEvaluator lBezier = new BezierEvaluator();
			lBezier.Point1 = LeftPoint1;
			lBezier.Point2 = LeftPoint2;
			lBezier.Point3 = LeftPoint3;
			lBezier.Point4 = LeftPoint4;

			BezierEvaluator rBezier = new BezierEvaluator();
			rBezier.Point1 = RightPoint1;
			rBezier.Point2 = RightPoint2;
			rBezier.Point3 = RightPoint3;
			rBezier.Point4 = RightPoint4;

			LineEvaluator line = new LineEvaluator();

			double t = startT;
			double deltaT = (1.0D - startT)/(yVertices - 1);
			Point3DCollection positions = new Point3DCollection();
			for (int y = 0; y < yVertices; y++)
			{
				Point3D leftBezier = lBezier.Evaluate(t);
				Point3D rightBezier = rBezier.Evaluate(t);
				for (int x = 0; x < xVertices; x++)
				{
					double lineT = (double) x/(xVertices - 1);
					line.Point1 = leftBezier;
					line.Point2 = rightBezier;
					Point3D linePos = line.Evaluate(lineT);

					if (x == 0)
					{
						positions.Add(leftBezier);
					}
					else if (x == xVertices - 1)
					{
						positions.Add(rightBezier);
					}
					else
					{
						positions.Add(linePos);
					}
				}

				t += deltaT;
			}
			return positions;
		}
	}
}