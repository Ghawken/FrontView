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
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using FluidKit.Helpers;

namespace FluidKit.Controls
{
	public class GenieTransition : Transition
	{
		private const int SidePoints = 25;
		private GenieEffectType _effectType = GenieEffectType.IntoLamp;
		private Rectangle _screenRect;
		private GeometryModel3D _slidingScreen;
		private Viewport3D _viewport;

		public GenieTransition()
		{
			_viewport =
				Application.LoadComponent(new Uri("/FluidKit;component/Controls/Transition/Genie/Genie.xaml",
				                                  UriKind.Relative)) as Viewport3D;
			_slidingScreen = _viewport.FindName("SlidingScreen") as GeometryModel3D;
			NameScope.GetNameScope(_viewport).UnregisterName("SlidingScreen");
		}

		public GenieEffectType EffectType
		{
			get { return _effectType; }
			set { _effectType = value; }
		}

		public override Storyboard PrepareStoryboard()
		{
			double aspect = Owner.ActualWidth/Owner.ActualHeight;
			Storyboard animator = (_viewport.Resources["GenieAnim"] as Storyboard).Clone();
			GenieAnimation anim = animator.Children[0] as GenieAnimation;
			anim.Duration = this.Duration;
			anim.EffectType = EffectType;
			anim.AspectRatio = aspect;


			return animator;
		}

		public override void Setup(Brush prevBrush, Brush nextBrush)
		{
			double aspect = Owner.ActualWidth/Owner.ActualHeight;

			GetNameScope().RegisterName("SlidingScreen", _slidingScreen);

			MeshGeometry3D mesh = MeshCreator.CreateMesh(SidePoints, SidePoints);
			mesh.Positions = new RectangularMeshFiller().FillMesh(SidePoints, SidePoints, aspect);
			_slidingScreen.Geometry = mesh;

			// Back screen
			_screenRect = new Rectangle();

			// Assign brushes based on EffectType
			if (EffectType == GenieEffectType.IntoLamp)
			{
				_slidingScreen.Material = new DiffuseMaterial(prevBrush);
				_screenRect.Fill = nextBrush;
			}
			else
			{
				_slidingScreen.Material = new DiffuseMaterial(nextBrush);
				_screenRect.Fill = prevBrush;
			}

			// Camera
			PerspectiveCamera camera = _viewport.Camera as PerspectiveCamera;
			double angle = camera.FieldOfView/2;
			double cameraZPos = (aspect/2)/Math.Tan(angle*Math.PI/180);
			camera.Position = new Point3D(0, 0, cameraZPos);

			Owner.AddTransitionElement(_screenRect);
			Owner.AddTransitionElement(_viewport);

		}
	}
}