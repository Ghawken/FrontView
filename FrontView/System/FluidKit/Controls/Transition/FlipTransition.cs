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

namespace FluidKit.Controls
{
	public class FlipTransition : Transition
	{
		private readonly Model3DGroup _cubeModelContainer;
		private readonly Model3DGroup _rootModel;
		private readonly Viewport3D _viewport;
		private Direction _direction = Direction.TopToBottom;

		public FlipTransition()
		{
			_viewport = TransitionResources["3DCube"] as Viewport3D;

		    if (_viewport != null) _rootModel = ((ModelVisual3D) _viewport.Children[0]).Content as Model3DGroup;
		    if (_rootModel != null) _cubeModelContainer = _rootModel.Children[1] as Model3DGroup;
		}

		public Direction Rotation
		{
			get { return _direction; }
			set { _direction = value; }
		}

		private void RegisterNameScope()
		{
			// Transforms
			var groupTrans = _rootModel.Transform as Transform3DGroup;
		    if (groupTrans != null)
		    {
		        var rotator = ((RotateTransform3D) groupTrans.Children[0]).Rotation as
		                      AxisAngleRotation3D;
		        var translator = groupTrans.Children[1] as TranslateTransform3D;

		        // We are setting names in code since x:Name in the ResourceDictionary is not supported
		        NameScope scope = GetNameScope();
		        if (rotator != null) scope.RegisterName("XFORM_Rotate", rotator);
		        if (translator != null) scope.RegisterName("XFORM_Translate", translator);
		    }
		}

		public override void Setup(Brush prevBrush, Brush nextBrush)
		{
			RegisterNameScope();
			Owner.AddTransitionElement(_viewport);

			AdjustViewport(prevBrush, nextBrush);
		}

		private void AdjustViewport(Brush prevBrush, Brush nextBrush)
		{
			// Adjusting the positions according to the Container's Width/Height
			double aspect = Owner.ActualWidth / Owner.ActualHeight;

			PrepareCubeFaces(aspect, prevBrush, nextBrush);

			// Camera
			AdjustCamera(aspect);

			AdjustTransforms(aspect);
		}

		private void PrepareCubeFaces(double aspect, Brush prevBrush, Brush nextBrush)
		{
			// Front face
			GeometryModel3D face1 = PrepareCubeFace(prevBrush, nextBrush, aspect);

			_cubeModelContainer.Children.Add(face1);
		}

		private void AdjustTransforms(double aspect)
		{
			// Axis of rotation
			var rotateXForm =
				((Transform3DGroup) _rootModel.Transform).Children[0] as RotateTransform3D;
			var translateXForm =
				((Transform3DGroup) _rootModel.Transform).Children[1] as TranslateTransform3D;

			var animator = _viewport.Resources["STORYBOARD_CubeAnimator"] as Storyboard;
		    if (animator != null)
		    {
		        var rotateAnim = animator.Children[0] as DoubleAnimationUsingKeyFrames;
		        var translateAnim =
		            animator.Children[1] as DoubleAnimationUsingKeyFrames;
		        if (rotateAnim != null) rotateAnim.Duration = Duration;
		        if (translateAnim != null)
		        {
		            translateAnim.Duration = Duration;

		            switch (Rotation)
		            {
		                case Direction.LeftToRight:
		                    if (rotateAnim != null) rotateAnim.KeyFrames[1].Value = 180;
		                    if (rotateXForm != null)
		                    {
		                        rotateXForm.CenterX = -aspect / 2;
		                        ((AxisAngleRotation3D) rotateXForm.Rotation).Axis = new Vector3D(0, 1, 0);
		                    }

		                    if (translateXForm != null) translateXForm.OffsetY = 0;
		                    Storyboard.SetTargetProperty(translateAnim,
		                                                 new PropertyPath(TranslateTransform3D.OffsetXProperty));
		                    translateAnim.KeyFrames[1].Value = aspect;
		                    break;

		                case Direction.RightToLeft:
		                    if (rotateAnim != null) rotateAnim.KeyFrames[1].Value = -180;
		                    if (rotateXForm != null)
		                    {
		                        rotateXForm.CenterX = aspect / 2;
		                        ((AxisAngleRotation3D) rotateXForm.Rotation).Axis = new Vector3D(0, 1, 0);
		                    }

		                    Storyboard.SetTargetProperty(translateAnim,
		                                                 new PropertyPath(TranslateTransform3D.OffsetXProperty));
		                    translateAnim.KeyFrames[1].Value = -aspect;
		                    break;

		                case Direction.TopToBottom:
		                    if (rotateAnim != null) rotateAnim.KeyFrames[1].Value = 90;
		                    if (rotateXForm != null)
		                    {
		                        rotateXForm.CenterY = 0.5;
		                        ((AxisAngleRotation3D) rotateXForm.Rotation).Axis = new Vector3D(1, 0, 0);
		                    }

		                    Storyboard.SetTargetProperty(translateAnim,
		                                                 new PropertyPath(TranslateTransform3D.OffsetYProperty));
		                    translateAnim.KeyFrames[1].Value = -1;
		                    break;

		                case Direction.BottomToTop:
		                    if (rotateAnim != null) rotateAnim.KeyFrames[1].Value = -90;
		                    if (rotateXForm != null)
		                    {
		                        rotateXForm.CenterY = -0.5;
		                        ((AxisAngleRotation3D) rotateXForm.Rotation).Axis = new Vector3D(1, 0, 0);
		                    }

		                    Storyboard.SetTargetProperty(translateAnim,
		                                                 new PropertyPath(TranslateTransform3D.OffsetYProperty));
		                    translateAnim.KeyFrames[1].Value = 1;
		                    break;
		            }
		        }
		    }
		}

		private void AdjustCamera(double aspect)
		{
			var camera = _viewport.Camera as PerspectiveCamera;
		    if (camera != null)
		    {
		        double angle = camera.FieldOfView / 2;
		        double cameraZPos = (aspect / 2) / Math.Tan(angle * Math.PI / 180);
		        camera.Position = new Point3D(0, 0, cameraZPos);
		    }
		}

		private GeometryModel3D PrepareCubeFace(Brush prevBrush, Brush nextBrush, double aspect)
		{
			// Create the mesh
			MeshGeometry3D mesh = ((MeshGeometry3D) _viewport.Resources["CubeFace"]).Clone();
			mesh.Positions[0] = new Point3D(-aspect / 2, 0.5, 0);
			mesh.Positions[1] = new Point3D(aspect / 2, 0.5, 0);
			mesh.Positions[2] = new Point3D(aspect / 2, -0.5, 0);
			mesh.Positions[3] = new Point3D(-aspect / 2, -0.5, 0);

			// Apply material
			var material = new DiffuseMaterial(prevBrush);

			// Create the model
			var model = new GeometryModel3D(mesh, material);
			nextBrush.RelativeTransform = new ScaleTransform(-1, 1, 0.5, 0.5);
			model.BackMaterial = new DiffuseMaterial(nextBrush);
			return model;
		}

		public override Storyboard PrepareStoryboard()
		{
			var animator = _viewport.Resources["STORYBOARD_CubeAnimator"] as Storyboard;
			return animator;
		}

		public override void Cleanup()
		{
			var model = _cubeModelContainer.Children[0] as GeometryModel3D;
		    if (model != null)
		    {
		        ((DiffuseMaterial) model.Material).Brush = null;
		        ((DiffuseMaterial) model.BackMaterial).Brush = null;
		    }
		    _cubeModelContainer.Children.Clear();
		}

	}
}