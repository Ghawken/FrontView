using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace FluidKit.Controls.View3D
{
	public class Cone : MapItemVisual3D
	{
		public Cone()
		{
			CreateModel();
		}

		private void CreateModel()
		{
			int steps = (int)(1 / 0.05);
			double side = 100;

			// Generate the vertices
			Points = new List<Point3D>();
			for (int i = 0; i <= steps; i++)
			{
				double t = (double)i / steps;
				double x = side * Math.Cos(t * 2 * Math.PI);
				double y = side * Math.Sin(t * 2 * Math.PI);

				Points.Add(new Point3D(x, y, -side));
			}

			Points.Add(new Point3D(0, 0, -side));
			Points.Add(new Point3D(0, 0, side));

			// Generate the faces
			Faces = new List<int[]>();
			for (int i = 0; i < steps; i++)
			{
				int[] face = { i, i + 1, Points.Count-1};
				int[] sectorBottom = { i, i+1, Points.Count - 2};
				Faces.Add(face);
				Faces.Add(sectorBottom);
			}

			FacePoints = 3;
		}
	}
}