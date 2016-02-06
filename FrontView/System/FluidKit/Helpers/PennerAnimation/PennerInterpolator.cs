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

namespace FluidKit.Helpers.Animation
{
	public static class PennerInterpolator
	{
		public static double Linear(double from, double to, double t)
		{
			return from + ((to - from)*t);
		}

		// Quadratic 
		public static double QuadEaseIn(double from, double to, double t)
		{
			return (to - from)*Math.Pow(t, 2) + from;
		}

		public static double QuadEaseOut(double from, double to, double t)
		{
			return -1*(to - from)*t*(t - 2) + from;
		}

		// Cubic
		public static double CubicEaseIn(double from, double to, double t)
		{
			return (to - from)*Math.Pow(t, 3) + from;
		}

		public static double CubicEaseOut(double from, double to, double t)
		{
			return (to - from)*(Math.Pow(t - 1, 3) + 1) + from;
		}

		// Quartic
		public static double QuarticEaseIn(double from, double to, double t)
		{
			return (to - from)*Math.Pow(t, 4) + from;
		}

		public static double QuarticEaseOut(double from, double to, double t)
		{
			return -1*(to - from)*(Math.Pow(t - 1, 4) - 1) + from;
		}

		// Quintic
		public static double QuinticEaseIn(double from, double to, double t)
		{
			return (to - from)*Math.Pow(t, 5) + from;
		}

		public static double QuinticEaseOut(double from, double to, double t)
		{
			return (to - from)*(Math.Pow(t - 1, 5) + 1) + from;
		}

		// Sine
		public static double SineEaseIn(double from, double to, double t)
		{
			return -1*(to - from)*Math.Cos(t*(Math.PI/2)) + to;
		}

		public static double SineEaseOut(double from, double to, double t)
		{
			return (to - from)*Math.Sin(t*(Math.PI/2)) + from;
		}

		// Exponential
		public static double ExpoEaseIn(double from, double to, double t)
		{
			return (t == 0) ? from : (to - from)*Math.Pow(2, 10*(t - 1)) + from;
		}

		public static double ExpoEaseOut(double from, double to, double t)
		{
			return (t == 1) ? to : (to - from)*(-1*Math.Pow(2, -10*t) + 1) + from;
		}

		// Circular
		public static double CircularEaseIn(double from, double to, double t)
		{
			return -1*(to - from)*(Math.Sqrt(1 - t*t) - 1) + from;
		}

		public static double CircularEaseOut(double from, double to, double t)
		{
			t = t - 1;
			return (to - from)*Math.Sqrt(1 - t*t) + from;
		}

		// Elastic
		public static double ElasticEaseIn(double from, double to, double t)
		{
			if (t == 0)
			{
				return from;
			}
			if (t == 1)
			{
				return to;
			}

			double d = 100;
			double p = 0.3*d;
			double a = to - from;
			double s = p/4;

			t = t - 1;
			return -(a*Math.Pow(2, 10*t)*Math.Sin((t*d - s)*(2*Math.PI)/p)) + from;
		}

		public static double ElasticEaseOut(double from, double to, double t)
		{
			if (t == 0)
			{
				return from;
			}
			if (t == 1)
			{
				return to;
			}

			double d = 100;
			double p = d*0.3;
			double a = to - from;
			double s = p/4;
			return (a*Math.Pow(2, -10*t)*Math.Sin((t*d - s)*(2*Math.PI)/p) + to);
		}

		// Bounce
		public static double BounceEaseIn(double from, double to, double t)
		{
			double c = to - from;
			return c - BounceEaseOut(0, to, 1.0 - t) + from;
		}

		public static double BounceEaseOut(double from, double to, double t)
		{
			double c = to - from;

			if (t < (1/2.75))
			{
				return c*(7.5625*t*t) + from;
			}
			else if (t < (2/2.75))
			{
				t -= 1.5/2.75;
				return c*(7.5625*t*t + .75) + from;
			}
			else if (t < (2.5/2.75))
			{
				t -= 2.25/2.75;
				return c*(7.5625*t*t + .9375) + from;
			}
			else
			{
				t -= 2.625/2.75;
				return c*(7.5625*t*t + .984375) + from;
			}
		}

		// Slight Bounce
		public static double SlightBounceEaseIn(double from, double to, double t)
		{
			double c = to - from;
			return c - SlightBounceEaseOut(0, to, 1.0 - t) + from;
		}

		public static double SlightBounceEaseOut(double from, double to, double t)
		{
			double c = to - from;

			if (t < (2/2.75))
			{
				t /= 2.0;
				return c*(7.5625*t*t) + from;
			}
			else if (t < (2.5/2.75))
			{
				t -= 2.25/2.75;
				return c*(7.5625*t*t + .9375) + from;
			}
			else
			{
				t -= 2.625/2.75;
				return c*(7.5625*t*t + .984375) + from;
			}
		}

		// Back
		public static double BackEaseIn(double from, double to, double t)
		{
			double s = 1.70158;
			return (to - from)*(t*t*((s + 1)*t - s)) + from;
		}

		public static double BackEaseOut(double from, double to, double t)
		{
			double s = 1.70158;
			t = t - 1;
			return (to - from)*(t*t*((s + 1)*t + s) + 1) + from;
		}
	}
}