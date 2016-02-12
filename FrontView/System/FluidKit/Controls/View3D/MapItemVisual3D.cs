using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace FluidKit.Controls.View3D
{
	public class MapItemVisual3D : DrawingVisual
	{
		public List<Point3D> Points { get; set; }
		public List<int[]> Faces { get; set; }
		public int FacePoints { get; set; }
		public Brush FaceBrush { get; set; }
		public Pen EdgePen { get; set; }

		// Internal properties
		internal List<Point3D> SphericalPoints { get; set; }
		internal List<Point> ProjectedPoints { get; set; }
		internal View3DPresenter View3DParent { get; set; }

		public MapItemVisual3D(List<Point3D> points, List<int[]> faces, int facePoints)
		{
			Points = points;
			Faces = faces;
			FacePoints = facePoints;
		}

		public MapItemVisual3D()
		{
		}

		public void RenderModel()
		{
			ProjectedPoints = GetProjectedPoints(View3DParent.Theta, View3DParent.Phi, View3DParent.FocalLength);
			DrawingContext dc = RenderOpen();

			foreach (int[] face in Faces)
			{
				StreamGeometry geom = DrawFace(face, FacePoints, ProjectedPoints, View3DParent.RenderSize);
				dc.DrawGeometry(FaceBrush, EdgePen, geom);
			}
			dc.Close();
		}

		private StreamGeometry DrawFace(int[] face, int facePoints, List<Point> points, Size worldSize)
		{
			StreamGeometry geom = new StreamGeometry();
			using (StreamGeometryContext context = geom.Open())
			{
				Point start = points[face[0]];
				start.Offset(worldSize.Width / 2, worldSize.Height / 2);

				context.BeginFigure(start, true, true);
				for (int j = 1; j < facePoints; j++)
				{
					Point p = points[face[j]];
					p.Offset(worldSize.Width / 2, worldSize.Height / 2);

					context.LineTo(p, true, true);
				}
			}

			return geom;
		}

		#region Transformations
		public void Rotate(double theta, double phi)
		{
			for (int i = 0; i < Points.Count; i++)
			{
				Point3D point = ProjectTo3D(Points[i], theta, phi);
				Points[i] = point;
			}
			if (View3DParent != null) View3DParent.InvalidateVisual();
		}

		public void Translate(Point3D trans)
		{
			for (int i = 0; i < Points.Count; i++)
			{
				Point3D point = Points[i];
				point.Offset(trans.X, trans.Y, trans.Z);
				Points[i] = point;
			}
			if (View3DParent != null) View3DParent.InvalidateVisual();
		}

		public void Scale(double scaleX, double scaleY, double scaleZ)
		{
			Matrix3D matrix = Matrix3D.Identity;
			matrix.Scale(new Vector3D(scaleX, scaleY, scaleZ));
			for (int i = 0; i < Points.Count; i++)
			{
				Point3D point = Point3D.Multiply(Points[i], matrix);
				Points[i] = point;
			}
			if (View3DParent != null) View3DParent.InvalidateVisual();
		}
		#endregion

		internal List<Point> GetProjectedPoints(double theta, double phi, double focalLength)
		{
			// Project to 3D
			SphericalPoints = new List<Point3D>();
			foreach (var point in Points)
			{
				Point3D point3D = ProjectTo3D(point, theta, phi);
				SphericalPoints.Add(point3D);
			}

			// Depth sort faces
			DepthSortFaces(SphericalPoints, focalLength);

			// Project to 2D
			List<Point> points2D = new List<Point>();

			foreach (var point3D in SphericalPoints)
			{
				Point point2D = ProjectTo2D(point3D, focalLength);
				points2D.Add(point2D);
			}

			return points2D;
		}

		private void DepthSortFaces(List<Point3D> points3D, double focalLength)
		{
			Faces.Sort(new Comparison<int[]>(delegate(int[] f1, int[] f2)
			                                 	{
													double d1 = DistanceToCamera(f1, focalLength, points3D);
													double d2 = DistanceToCamera(f2, focalLength, points3D);

													if (d1 < d2) return 1;
													if (d1 > d2) return -1;
													return 0;
			                                 	}));
		}

		private double DistanceToCamera(int[] face, double focalLength, List<Point3D> points3D)
		{
			Point3D center = new Point3D();
			for (int i = 0; i < FacePoints; i++)
			{
				center.X += points3D[face[i]].X;
				center.Y += points3D[face[i]].Y;
				center.Z += points3D[face[i]].Z;
			}

			center.X /= FacePoints;
			center.Y /= FacePoints;
			center.Z /= FacePoints;

			double distance = (focalLength - center.X) * (focalLength - center.X) +
							  center.Y * center.Y +
							  center.Z * center.Z;
			distance = Math.Sqrt(distance);
			return distance;
		}

		internal static Point3D ProjectTo3D(Point3D point3, double theta, double phi)
		{
			theta *= 0.01745;
			phi *= 0.01745;
			double projX = point3.X * Math.Cos(theta) * Math.Sin(phi) +
						   point3.Y * Math.Sin(theta) * Math.Sin(phi) +
						   point3.Z * Math.Cos(phi);

			double projY = -point3.X * Math.Sin(theta) + point3.Y * Math.Cos(theta);
			double projZ = -point3.X * Math.Cos(theta) * Math.Cos(phi) -
						   point3.Y * Math.Sin(theta) * Math.Cos(phi) +
						   point3.Z * Math.Sin(phi);

			return new Point3D(projX, projY, projZ);
		}

		internal static Point ProjectTo2D(Point3D point, double fLen)
		{
			double distortion = fLen / (fLen - point.X);

			return new Point(point.Y * distortion, -point.Z * distortion);
		}
	}
}