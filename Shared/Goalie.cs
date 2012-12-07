using System;
using System.IO;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework;

namespace Shared
{
	public class Goalie
	{
		static float left = -1f;
		static float right = 1f;
		static float top = .25f;
		static float bottom = -.25f;
		static float speed = 8f;
		LineSegment<Vector2> segment;
		LineSegment<Vector2> segmentIn;
		LineSegment<Vector2> segmentOut;
		Vector2 delta;

		public Fixture Fixture;
		public Goalie (World world, GoalieData spawn)
		{
			var body = new Body(world);
			var angle = spawn.End - spawn.Begin;			
			body.Rotation = FMath.Atan2(angle.Y, angle.X);
			segment.A = spawn.Begin;
			segment.B = spawn.End;			
			var normal = new Vector2(-angle.Y, angle.X);
			normal.Normalize();
			delta = normal * .5f;
			segmentOut.A = spawn.Begin + delta;
			segmentOut.B = spawn.End + delta;
			segmentIn.A = spawn.Begin - delta;
			segmentIn.B = spawn.End - delta;
			body.Position = spawn.Begin;
			var verts = new Vertices();	
			verts.Add(new Vector2(left, bottom));
			verts.Add(new Vector2(right, bottom));
			verts.Add(new Vector2(right, top));
			verts.Add(new Vector2(left, top));			
			var shape = new PolygonShape(verts, 1f);
			body.FixedRotation = true;
			body.BodyType = BodyType.Dynamic;
			Fixture = body.CreateFixture(shape);
		}
		
		void SegmentFor(Vector2 point, out LineSegment<Vector2> outSegment, out Vector2 outDelta)
		{
			if (Trig.SideOf(segment, point) > 0f) {
				outSegment = segmentOut;
				outDelta = -delta;
			}
			else {
				outSegment = segmentIn;
				outDelta = delta;
			}
		}
		
		public void Step(float elapsed, Simulation sim)
		{
			Vector2 thisPos;
			Vector2 thisDelta;				
			Vector2 thisTarget;
			LineSegment<Vector2> thisSegment;
			float thisDistance;
			
			var target = new Vector2();
			var distance = 0f;
			var hasTarget = false;
			
			for (int i = 0; i < sim.Balls.Length; i++) {
				if (sim.Balls[i].Carrier != null) {
					thisPos = sim.Balls[i].Carrier.Fixture.Body.Position;
					SegmentFor(thisPos, out thisSegment, out thisDelta);
					thisTarget = Trig.ClosestPointOnSegment(thisSegment, thisPos);
					thisTarget += thisDelta;
				}
				else {					
					thisPos = sim.Balls[i].Fixture.Body.Position;					
					SegmentFor(thisPos, out thisSegment, out thisDelta);
					var ballStop = sim.Balls[i].BallStop;
					LineSegment<Vector2> ballVector;
					ballVector.A = thisPos;
					ballVector.B = ballStop;
					if (!Trig.SegmentSegmentIntersection(ballVector, thisSegment, out thisTarget)) {
						thisTarget = Trig.ClosestPointOnSegment(thisSegment, thisPos);
					}
					thisTarget += thisDelta;
				}
				
				thisDistance = Trig.DistanceSquared(Fixture.Body.Position, thisPos);
				if (!hasTarget || thisDistance < distance) {
					target = thisTarget;
					distance = thisDistance;
					hasTarget = true;					
				}
			}
								
			if (!hasTarget) return;
			
			var delta = target - Fixture.Body.Position;
			if (delta.X == 0 && delta.Y == 0) {
				Fixture.Body.LinearVelocity = new Vector2(0f, 0f);
				return;
			}
			
			var direction = delta;
			direction.Normalize();

			var moveDistance = Trig.DistanceSquared(Fixture.Body.Position, target);
			if (moveDistance <= speed * elapsed * speed * elapsed) {
				Fixture.Body.LinearVelocity = delta/elapsed;
				return;
			}
			
			Fixture.Body.LinearVelocity = direction * speed;
		}
	
		public int MarshalBytes {
			get { return 8; }
		}
		
		public void Marshal(BinaryWriter writer)
		{			
			writer.Write(Fixture.Body.Position.X);
			writer.Write(Fixture.Body.Position.Y);	
		}
		
		public void UnMarshal(BinaryReader reader)
		{
			float x, y;
			x = reader.ReadSingle();
			y = reader.ReadSingle();
			Fixture.Body.Position = new Vector2(x, y);
		}
	}
}

