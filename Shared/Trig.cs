using System;
using Microsoft.Xna.Framework;

namespace Shared
{
	public static class Trig
	{
		static public float DistanceSquared(Vector2 a, Vector2 b)
		{
			var deltaX = b.X - a.X;
			var deltaY = b.Y - a.Y;
			return deltaX*deltaX + deltaY*deltaY;
		}
		
		static public float Distance(Vector2 a, Vector2 b)
		{
			return FMath.Sqrt(DistanceSquared(a, b));
		}

		static public float SideOf(LineSegment<Vector2> line, Vector2 point)
		{
			var diff = line.B - line.A;
			var perp = new Vector2(-diff.Y, diff.X);
			return Vector2.Dot(point - line.A, perp);
		}
		
		static public Vector2 ClosestPointOnLine(LineSegment<Vector2> line, Vector2 point)
		{
			var ap = point - line.A;
			var ab = line.B - line.A;
			var ab2 = Vector2.Dot(ab, ab);
			var apab = Vector2.Dot(ap, ab);
			var t = apab / ab2;
			return line.A + ab * t;
		}
	
		static public Vector2 ClosestPointOnSegment(LineSegment<Vector2> segment, Vector2 point)
		{
			var ap = point - segment.A;
			var ab = segment.B - segment.A;
			var ab2 = Vector2.Dot(ab, ab);
			var apab = Vector2.Dot(ap, ab);
			var t = apab / ab2;
			if (t < 0f) t = 0f;
			else if (t > 1f) t = 1f;
			return segment.A + ab * t;
		}
		
		static public bool CircleIntersectSegment(LineSegment<Vector2> segment, Circle circle, out Vector2 intersection)
		{
			intersection = ClosestPointOnSegment(segment, circle.Position);
			var distance2 = DistanceSquared(intersection, circle.Position);
			if (distance2 <= circle.Radius * circle.Radius) return true;
			else return false;
		}
		
		static public bool CircleIntersectCircle(Circle a, Circle b)
		{
			var distance2 = DistanceSquared(a.Position, b.Position);
			var radius2 = (a.Radius + b.Radius) * (a.Radius + b.Radius);
			if (distance2 <= radius2) return true;
			else return false;
				
		}

		static public bool SegmentSegmentIntersection(LineSegment<Vector2> a, LineSegment<Vector2> b, out Vector2 point)
		{
			float r, s, d;

			if ((a.B.Y - a.A.Y) / (a.B.X - a.A.X) != (b.B.Y - b.A.Y) / (b.B.X - b.A.X))
			{
				d = (((a.B.X - a.A.X) * (b.B.Y - b.A.Y)) - (a.B.Y - a.A.Y) * (b.B.X - b.A.X));
				if (d != 0)
				{
					r = (((a.A.Y - b.A.Y) * (b.B.X - b.A.X)) - (a.A.X - b.A.X) * (b.B.Y - b.A.Y)) / d;
					s = (((a.A.Y - b.A.Y) * (a.B.X - a.A.X)) - (a.A.X - b.A.X) * (a.B.Y - a.A.Y)) / d;
					if (r >= 0 && r <= 1f && s >= 0f && s <= 1f)
					{
						point = new Vector2(a.A.X + r * (a.B.X - a.A.X), a.A.Y + r * (a.B.Y - a.A.Y));
						return true;
					}
				}
			}
			
			point = new Vector2();
			return false;
		}


	}
}

