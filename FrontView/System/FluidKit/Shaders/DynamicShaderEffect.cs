using System;
using System.Windows;
using System.Windows.Media.Effects;

namespace FluidKit.Shaders
{
	public enum DynamicShaderType
	{
		Crystallize,
		BlueShift,
	}

	public class DynamicShaderEffect : ShaderEffect
	{
		private static PixelShader _shader = new PixelShader
		{
			UriSource =
				new Uri(
				@"pack://application:,,,/FluidKit;component/Shaders/DynamicShader.ps")
		};

		private static readonly DependencyProperty ShaderIndexProperty = DependencyProperty.Register(
			"ShaderIndex", typeof(double), typeof(DynamicShaderEffect), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(0)));

		// Parameters
		public static readonly DependencyProperty Param1Property = DependencyProperty.Register(
			"Param1", typeof(double), typeof(DynamicShaderEffect), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(1)));

		public static readonly DependencyProperty Param2Property = DependencyProperty.Register(
			"Param2", typeof(double), typeof(DynamicShaderEffect), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(2)));

		public static readonly DependencyProperty Param3Property = DependencyProperty.Register(
			"Param3", typeof(double), typeof(DynamicShaderEffect), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(3)));

		public static readonly DependencyProperty DynamicShaderProperty = DependencyProperty.Register(
			"DynamicShader", typeof(DynamicShaderType), typeof(DynamicShaderEffect), new PropertyMetadata(OnDynamicShaderChanged));

		public DynamicShaderType DynamicShader
		{
			get { return (DynamicShaderType) GetValue(DynamicShaderProperty); }
			set { SetValue(DynamicShaderProperty, value); }
		}

		public double Param1
		{
			get { return (double)GetValue(Param1Property); }
			set { SetValue(Param1Property, value); }
		}

		public double Param2
		{
			get { return (double)GetValue(Param2Property); }
			set { SetValue(Param2Property, value); }
		}

		public double Param3
		{
			get { return (double)GetValue(Param3Property); }
			set { SetValue(Param3Property, value); }
		}

		private static void OnDynamicShaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DynamicShaderType type = (DynamicShaderType) e.NewValue;
			DynamicShaderEffect effect = d as DynamicShaderEffect;

			int index = Array.IndexOf(Enum.GetValues(typeof (DynamicShaderType)), type);
			effect.SetValue(ShaderIndexProperty, (double)index);
		}

		public DynamicShaderEffect()
		{
			PixelShader = _shader;
			UpdateShaderValue(ShaderIndexProperty);
			UpdateShaderValue(Param1Property);
			UpdateShaderValue(Param2Property);
		}
	}
}