using System;
using System.Drawing;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Shared;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;

namespace Shared
{
	public struct WallCompiled {
		public Vector2[] Verts1;
		public Vector2[] Verts2;		
		public Vector2[] Verts3;
		public Vector2[] Verts4;
		
		public float[] TexesBegin1;
		public float[] TexesBegin2;
		public float[] TexesBegin4;
		public float[] TexesBegin3;
		public float[] TexesEnd1;
		public float[] TexesEnd2;
		public float[] TexesEnd3;
		public float[] TexesEnd4;
				
		static float length1 = 0f;
		static float length2 = .25f;
		static float length3 = .75f;
		static float length4 = 1f;		
		
		public float Thickness;
		public Color Color;
		public ShapeData Shape;

		public WallCompiled(WallData wallData)
		{
			Thickness = wallData.Thickness;
			Color = wallData.Color;
			Shape = wallData.Shape;

			var outer = wallData.Verts;

			Verts1 = new Vector2[outer.Length];
			Verts2 = new Vector2[outer.Length];
			Verts3 = new Vector2[outer.Length];
			Verts4 = new Vector2[outer.Length];
			TexesBegin1 = new float[outer.Length];
			TexesBegin2 = new float[outer.Length];
			TexesBegin3 = new float[outer.Length];
			TexesBegin4 = new float[outer.Length];
			TexesEnd1 = new float[outer.Length];
			TexesEnd2 = new float[outer.Length];
			TexesEnd3 = new float[outer.Length];
			TexesEnd4 = new float[outer.Length];
			
			switch (wallData.Shape) {
			case ShapeData.Open:
				doEnd(outer, 0, 0, 1, true, false);
				break;
			case ShapeData.Closed:
				doAngle(outer, 0, outer.Length-1, 0, 1);
				break;
			}

			for (int i = 1; i<outer.Length-1; i++) {
				doAngle(outer, i, i-1 , i, i+1);
			}

			switch (wallData.Shape) {
			case ShapeData.Open:
				doEnd(outer, outer.Length-1, outer.Length-2, outer.Length-1, false, false);
				break;
			case ShapeData.Closed:
				doAngle(outer, outer.Length-1, outer.Length-2, outer.Length-1, 0);
				break;
			}			
		}
		
		void doAngle(Vector2[] outer, int index, int prev, int cur, int next)
		{
			var a = outer[prev];
			var b = outer[cur];
			var c = outer[next];

			var radians1 = FMath.Atan2(a.Y - b.Y, a.X - b.X);
			var radians2 = FMath.Atan2(c.Y - b.Y, c.X - b.X);
			var angle = radians2 - radians1;
			var angle2 = (FMath.Pi - angle) / 2f;
			var radians = radians1 + (angle / 2f);			
			var scale = Thickness * 2f * FMath.Sin(angle2) / FMath.Sin(angle);

			var x = FMath.Cos(radians);
			var y = FMath.Sin(radians);
			Verts1[index].X = b.X + x * scale * length1;
			Verts1[index].Y = b.Y + y * scale * length1;
			Verts2[index].X = b.X + x * scale * length2;
			Verts2[index].Y = b.Y + y * scale * length2;
			Verts3[index].X = b.X + x * scale * length3;
			Verts3[index].Y = b.Y + y * scale * length3;
			Verts4[index].X = b.X + x * scale * length4;
			Verts4[index].Y = b.Y + y * scale * length4;

			var endTex = FMath.Round(Trig.Distance(b, a) / Thickness);
			var texLength1 = FMath.Sin(angle2) * scale * length1 / Thickness;
			var texLength2 = FMath.Sin(angle2) * scale * length2 / Thickness;
			var texLength3 = FMath.Sin(angle2) * scale * length3 / Thickness;
			var texLength4 = FMath.Sin(angle2) * scale * length4 / Thickness;
			TexesBegin1[index] = texLength1;
			TexesBegin2[index] = texLength2;
			TexesBegin3[index] = texLength3;
			TexesBegin4[index] = texLength4;			
			TexesEnd1[index] = endTex - texLength1;
			TexesEnd2[index] = endTex - texLength2;
			TexesEnd3[index] = endTex - texLength3;
			TexesEnd4[index] = endTex - texLength4;
		}

		void doEnd(Vector2[] outer, int index, int cur, int next, bool isStart, bool isCapped)
		{			
			var a = outer[cur];
			var b = outer[next];

			var ab = b - a;
			ab.Normalize();
			var x = -ab.Y;
			var y = ab.X;

			Vector2 end;
			if (isStart) end = a;
			else end = b;

			Verts1[index].X = end.X - x * Thickness * length1;
			Verts1[index].Y = end.Y - y * Thickness * length1;
			Verts2[index].X = end.X - x * Thickness * length2;
			Verts2[index].Y = end.Y - y * Thickness * length2;
			Verts3[index].X = end.X - x * Thickness * length3;
			Verts3[index].Y = end.Y - y * Thickness * length3;
			Verts4[index].X = end.X - x * Thickness * length4;
			Verts4[index].Y = end.Y - y * Thickness * length4;

			var endTex = FMath.Round(Trig.Distance(a, b) / Thickness);
			TexesBegin1[index] = 0f;
			TexesBegin2[index] = 0f;
			TexesBegin3[index] = 0f;
			TexesBegin4[index] = 0f;
			TexesEnd1[index] = endTex;
			TexesEnd2[index] = endTex;
			TexesEnd3[index] = endTex;
			TexesEnd4[index] = endTex;
		}
	}
}