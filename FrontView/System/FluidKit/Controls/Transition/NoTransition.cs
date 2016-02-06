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

//using System.Windows.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

//using System.Windows.Shapes;

namespace FluidKit.Controls
{
	public class NoTransition : Transition
	{
	    private Rectangle _nextRect;
		private Rectangle _prevRect;
		private Grid _rectContainer;
        		
        public NoTransition() : this(Direction.LeftToRight)
		{
		}

        public NoTransition(Direction direction)
		{
			Direction = direction;
		}

        public Direction Direction { get; set; }

	    private void SetupElements()
		{
			_rectContainer = new Grid {ClipToBounds = true};


	        _prevRect = new Rectangle {RenderTransform = new TranslateTransform()};

	        _nextRect = new Rectangle {RenderTransform = new TranslateTransform()};

	        _rectContainer.Children.Add(_nextRect);
			_rectContainer.Children.Add(_prevRect);

			var scope = GetNameScope();

			scope.RegisterName("PrevElement", _prevRect);
			scope.RegisterName("NextElement", _nextRect);
		}

		public override void Setup(Brush prevBrush, Brush nextBrush)
		{
			SetupElements();

			_prevRect.Fill = prevBrush;
			_nextRect.Fill = nextBrush;
			Owner.AddTransitionElement(_rectContainer);
		}

		public override Storyboard PrepareStoryboard()
		{

            Storyboard animator = ((Storyboard)TransitionResources["SlideAnim"]).Clone();

            var prevAnim = animator.Children[0] as DoubleAnimation;
            var nextAnim = animator.Children[1] as DoubleAnimation;
            Storyboard.SetTargetName(prevAnim, "PrevElement");
            Storyboard.SetTargetName(nextAnim, "NextElement");
		    if (prevAnim != null) prevAnim.Duration = new Duration(new TimeSpan(1));
            if (nextAnim != null) nextAnim.Duration = new Duration(new TimeSpan(1));

            return animator;
		}
	}
}