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
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace FluidKit.Helpers
{
	public static class MeshCreator
	{
		public static MeshGeometry3D CreateMesh(int xVertices, int yVertices)
		{
			Vector3DCollection normals = new Vector3DCollection();
			PointCollection textCoords = new PointCollection();
			for (int y = 0; y < yVertices; y++)
			{
				for (int x = 0; x < xVertices; x++)
				{
					// Normals
					Vector3D n1 = new Vector3D(0, 0, 1);
					normals.Add(n1);

					// Texture Coordinates
					textCoords.Add(GetTextureCoordinate(xVertices, yVertices, y, x));
				}
			}
			Int32Collection indices = GetTriangleIndices(xVertices, yVertices);

			MeshGeometry3D mesh = new MeshGeometry3D();
			mesh.Normals = normals;
			mesh.TriangleIndices = indices;
			mesh.TextureCoordinates = textCoords;
			return mesh;
		}

		private static Int32Collection GetTriangleIndices(int xVertices, int yVertices)
		{
			Int32Collection indices = new Int32Collection();
			for (int y = 0; y < yVertices - 1; y++)
			{
				for (int x = 0; x < xVertices - 1; x++)
				{
					int v1 = x + y*xVertices;
					int v2 = v1 + 1;
					int v3 = v1 + xVertices;
					int v4 = v3 + 1;
					indices.Add(v1);
					indices.Add(v3);
					indices.Add(v4);
					indices.Add(v1);
					indices.Add(v4);
					indices.Add(v2);
				}
			}
			return indices;
		}

		private static Point GetTextureCoordinate(int xVertices, int yVertices, int row, int col)
		{
			double blockWidth = 1.0D/(xVertices - 1);
			double blockHeight = 1.0D/(yVertices - 1);

			Point p = new Point();
			p.X = col*blockWidth;
			p.Y = row*blockHeight;
			return p;
		}
	}
}