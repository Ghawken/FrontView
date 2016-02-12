using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace FluidKit.Controls.View3D
{
	public class Sphere : MapItemVisual3D
	{
		public Sphere()
		{
			CreateModel();
		}

		private void CreateModel()
		{
			int steps = (int)(1 / 0.05);
			double radius = 100;

			// Generate the vertices
			Points = new List<Point3D>();
			for (int i = 0; i <= steps; i++)
			{
				double s = (double)i / steps;
				for (int j = 0; j < steps; j++)
				{
					double t = (double)j / steps;
					double x = radius * Math.Cos(t * 2 * Math.PI) * Math.Sin(s * Math.PI);
					double y = radius * Math.Sin(t * 2 * Math.PI) * Math.Sin(s * Math.PI);
					double z = radius * Math.Cos(s * Math.PI);

					Points.Add(new Point3D(x, y, z));
				}
			}
			// Generate the faces
			Faces = new List<int[]>();
			for (int i = 0; i < steps; i++)
			{
				for (int j = 0; j < steps-1; j++)
				{
					int[] face = {i * steps + j, i * steps + j + 1, (i+1)*steps + j+1, (i+1)*steps + j};
					Faces.Add(face);
				}

				// Last face in the row
				int[] lastFace = Faces[Faces.Count-1];
				int[] firstFace = Faces[i*steps];
				int[] remFace = {firstFace[0], firstFace[3], lastFace[2], lastFace[1]};
				Faces.Add(remFace);
			}
			FacePoints = 4;

		}
	}
}