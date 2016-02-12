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
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace FluidKit.Shaders
{
	public class WarpEffect : ShaderEffect
	{
		public static readonly DependencyProperty L1Property = DependencyProperty.Register(
			"L1", typeof (double), typeof (WarpEffect),
			new UIPropertyMetadata(0.0, PixelShaderConstantCallback(0)));

		public static readonly DependencyProperty L2Property = DependencyProperty.Register(
			"L2", typeof (double), typeof (WarpEffect),
			new UIPropertyMetadata(0.125, PixelShaderConstantCallback(1)));

		public static readonly DependencyProperty L3Property = DependencyProperty.Register(
			"L3", typeof (double), typeof (WarpEffect),
			new UIPropertyMetadata(0.375, PixelShaderConstantCallback(2)));

		public static readonly DependencyProperty L4Property = DependencyProperty.Register(
			"L4", typeof (double), typeof (WarpEffect),
			new UIPropertyMetadata(0.5, PixelShaderConstantCallback(3)));


		public static readonly DependencyProperty R1Property = DependencyProperty.Register(
			"R1", typeof (double), typeof (WarpEffect),
			new UIPropertyMetadata(1.0, PixelShaderConstantCallback(4)));

		public static readonly DependencyProperty R2Property = DependencyProperty.Register(
			"R2", typeof (double), typeof (WarpEffect),
			new UIPropertyMetadata(0.875, PixelShaderConstantCallback(5)));

		public static readonly DependencyProperty R3Property = DependencyProperty.Register(
			"R3", typeof (double), typeof (WarpEffect),
			new UIPropertyMetadata(0.625, PixelShaderConstantCallback(6)));

		public static readonly DependencyProperty R4Property = DependencyProperty.Register(
			"R4", typeof (double), typeof (WarpEffect),
			new UIPropertyMetadata(0.5, PixelShaderConstantCallback(7)));

		public static readonly DependencyProperty InputProperty =
		ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(WarpEffect), 0);

		public Brush Input
		{
			get { return (Brush)GetValue(InputProperty); }
			set { SetValue(InputProperty, value); }
		}

		private static PixelShader _shader = new PixelShader
		                                     	{
		                                     		UriSource =
		                                     			new Uri(
		                                     			@"pack://application:,,,/FluidKit;component/Shaders/Warp.ps")
		                                     	};

		public WarpEffect()
		{
			PixelShader = _shader;
			UpdateShaderValue(InputProperty);

			UpdateShaderValue(L1Property);
			UpdateShaderValue(L2Property);
			UpdateShaderValue(L3Property);
			UpdateShaderValue(L4Property);

			UpdateShaderValue(R1Property);
			UpdateShaderValue(R2Property);
			UpdateShaderValue(R3Property);
			UpdateShaderValue(R4Property);
		}


		public double L1
		{
			get { return (double) GetValue(L1Property); }
			set { SetValue(L1Property, value); }
		}

		public double L2
		{
			get { return (double) GetValue(L2Property); }
			set { SetValue(L2Property, value); }
		}

		public double L3
		{
			get { return (double) GetValue(L3Property); }
			set { SetValue(L3Property, value); }
		}

		public double L4
		{
			get { return (double) GetValue(L4Property); }
			set { SetValue(L4Property, value); }
		}

		public double R1
		{
			get { return (double) GetValue(R1Property); }
			set { SetValue(R1Property, value); }
		}

		public double R2
		{
			get { return (double) GetValue(R2Property); }
			set { SetValue(R2Property, value); }
		}

		public double R3
		{
			get { return (double) GetValue(R3Property); }
			set { SetValue(R3Property, value); }
		}

		public double R4
		{
			get { return (double) GetValue(R4Property); }
			set { SetValue(R4Property, value); }
		}
	}
}