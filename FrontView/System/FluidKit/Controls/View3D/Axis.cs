using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace FluidKit.Controls.View3D
{
	public class Axis : MapItemVisual3D
	{
		public Axis()
		{
			Points = new List<Point3D>();

			// X-Axis
			Points.Add(new Point3D(-300, 0, 0));
			Points.Add(new Point3D(300, 0, 0));

			// Y-Axis
			Points.Add(new Point3D(0, -300, 0));
			Points.Add(new Point3D(0, 300, 0));

			// Z-Axis
			Points.Add(new Point3D(0, 0, -300));
			Points.Add(new Point3D(0, 0, 300));

			// -- Letters --
			// X
			Points.Add(new Point3D(300, 10, 10));
			Points.Add(new Point3D(300, 20, 0));
			Points.Add(new Point3D(300, 20, 10));
			Points.Add(new Point3D(300, 10, 0));

			// Y
			Points.Add(new Point3D(0, 290, 20));
			Points.Add(new Point3D(0, 295, 15));
			Points.Add(new Point3D(0, 300, 20));
			Points.Add(new Point3D(0, 295, 10));

			// Z
			Points.Add(new Point3D(0, 10, 300));
			Points.Add(new Point3D(0, 20, 300));
			Points.Add(new Point3D(0, 10, 290));
			Points.Add(new Point3D(0, 20, 290));

			Faces = new List<int[]>();
			// Axis
			Faces.Add(new int[] { 0, 1 });
			Faces.Add(new int[] { 2, 3 });
			Faces.Add(new int[] { 4, 5 });

			// Letters
			Faces.Add(new int[] { 6, 7 });
			Faces.Add(new int[] { 8, 9 });

			Faces.Add(new int[] { 10, 11 });
			Faces.Add(new int[] { 11, 12 });
			Faces.Add(new int[] { 11, 13 });

			Faces.Add(new int[] { 14, 15 });
			Faces.Add(new int[] { 15, 16 });
			Faces.Add(new int[] { 16, 17 });

			FacePoints = 2;
			EdgePen = new Pen(Brushes.Black, 1);

		}
	}
}