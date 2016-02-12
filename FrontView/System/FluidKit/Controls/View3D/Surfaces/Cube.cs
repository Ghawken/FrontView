using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace FluidKit.Controls.View3D
{
	public class Cube : MapItemVisual3D
	{
		public Cube()
		{
			CreateModel();
		}

		private void CreateModel()
		{
			double side = 100;
			Points = new List<Point3D>
			                       	{
			                       		new Point3D(side, -side, side),
			                       		new Point3D(side, side, side),
			                       		new Point3D(-side, side, side),
			                       		new Point3D(-side, -side, side),
			                       		new Point3D(side, -side, -side),
			                       		new Point3D(side, side, -side),
										new Point3D(-side, side, -side),
										new Point3D(-side, -side, -side)
			                       	};

			Faces = new List<int[]>
			                    	{
			                    		new[] {0, 4, 5, 1},
										new[] {1, 5, 6, 2},
										new[] {2, 6, 7, 3},
										new[] {3, 7, 4, 0},
										new[] {4, 5, 6, 7},
			                    		new[] {0, 1, 2, 3},
			                    	};

			FacePoints = 4;
		}
	}
}