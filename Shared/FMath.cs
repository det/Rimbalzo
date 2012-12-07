using System;
namespace Shared
{
	static public class FMath
	{
		public static float Pi = (float) Math.PI;
		public static float Sin(float f) { return (float) Math.Sin((float) f); }
		public static float Cos(float f) { return (float) Math.Cos((float) f); }
		public static float Acos(float f) { return (float) Math.Acos((float) f); }
		public static float Sqrt(float f) { return (float) Math.Sqrt((float) f); }
		public static float Atan2(float y, float x) { return (float) Math.Atan2((float) y, (float) x); }
		public static float Round(float f) { return (float) Math.Round((float) f); }
		public static float Abs(float f) { return (float) Math.Abs(f); }
		public static float Min(float val1, float val2) { return (float) Math.Min((float) val1, (float) val2); }
		public static float Max(float val1, float val2) { return (float) Math.Max((float) val1, (float) val2); }
		public static float TwoPi = Pi * 2f;
		public static float PiOver2 = Pi / 2f;
		public static float PiOver4 = Pi / 4f;
	}
}

