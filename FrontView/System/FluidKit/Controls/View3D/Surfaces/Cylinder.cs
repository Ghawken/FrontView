using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace FluidKit.Controls.View3D
{
	public class Cylinder : MapItemVisual3D
	{
		public Cylinder()
		{
			CreateModel();
		}

		private void CreateModel()
		{
			int steps = (int)(1/0.05);
			double side = 100;

			// Generate the vertices
			Points = new List<Point3D>();
			for (int i = 0; i <= steps; i++)
			{
				double t = (double)i/steps;
				double x = side * Math.Cos(t * 2 * Math.PI);
				double y = side * Math.Sin(t * 2 * Math.PI);

				Points.Add(new Point3D(x, y, -side));
				Points.Add(new Point3D(x, y, side));
			}

			Points.Add(new Point3D(0, 0, -side));
			Points.Add(new Point3D(0, 0, side));

			// Generate the faces
			Faces = new List<int[]>();
			for (int i = 0; i < steps; i++)
			{
				int[] face = {i*2, i*2+1, i*2+3, i*2+2};
				int[] sectorTop = {Points.Count-2, i*2, i*2+2, Points.Count-2};
				int[] sectorBottom = {Points.Count-1, i*2+1, i*2+3, Points.Count-1};
				Faces.Add(face);
				Faces.Add(sectorTop);
				Faces.Add(sectorBottom);
			}

			FacePoints = 4;
		}
	}
}