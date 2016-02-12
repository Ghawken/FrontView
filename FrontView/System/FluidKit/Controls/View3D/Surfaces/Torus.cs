using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace FluidKit.Controls.View3D
{
	public class Torus : MapItemVisual3D
	{
		public Torus()
		{
			CreateModel();
		}

		private void CreateModel()
		{
			int steps = (int)(1/0.1);

			Points = new List<Point3D>();
			for (int i = 0; i <= steps; i++)
			{
				double u = (double)i/steps;

				for (int j = 0; j < steps; j++)
				{
					double v = (double)j / steps;

					double y = (100 + 50 * Math.Cos(v * 2 * Math.PI)) * Math.Cos(u * 2 * Math.PI);
					double x = (100 + 50 * Math.Cos(v * 2 * Math.PI)) * Math.Sin(u * 2 * Math.PI);
					double z = 50 * Math.Sin(v * 2 * Math.PI);

					Points.Add(new Point3D(x, y, z));
				}

			}

			Faces = new List<int[]>();
			for (int i = 0; i < steps; i++)
			{
				for (int j = 0; j < steps-1; j++)
				{
					int[] face = {i*steps+j, i*steps+j+1, (i+1)*steps+j+1, (i+1)*steps+j};
					Faces.Add(face);
				}

				// Last face in the row
				int[] lastFace = Faces[Faces.Count - 1];
				int[] firstFace = Faces[i * steps];
				int[] remFace = { firstFace[0], firstFace[3], lastFace[2], lastFace[1] };
				Faces.Add(remFace);
			}

			FacePoints = 4;
		}
	}
}