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
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace FluidKit.Controls
{
	public partial class ElementFlow
	{
		#region Mesh Creation

		private ModelUIElement3D CreateMeshModel(Visual visual)
		{
			var model3d = InternalResources["ElementModel"] as ModelUIElement3D;
			VisualBrush brush = new VisualBrush(visual);
			RenderOptions.SetCachingHint(brush, CachingHint.Cache);

			var geom3D = model3d.Model as GeometryModel3D;
			(geom3D.Material as DiffuseMaterial).Brush = brush;
			(geom3D.Geometry as MeshGeometry3D).Positions = CreateMeshPositions();

			return model3d;
		}

		private Point3DCollection CreateMeshPositions()
		{
			double aspect = ElementWidth/ElementHeight;
			double reflectionFactor = HasReflection ? 1.0 : 0.5;

			Point3DCollection positions = new Point3DCollection();
			positions.Add(new Point3D(-aspect/2, 1*reflectionFactor, 0));
			positions.Add(new Point3D(aspect/2, 1*reflectionFactor, 0));
			positions.Add(new Point3D(aspect/2, -1*reflectionFactor, 0));
			positions.Add(new Point3D(-aspect/2, -1*reflectionFactor, 0));

			return positions;
		}

		#endregion
	}
}