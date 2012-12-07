using System;
using System.IO;
using System.Drawing;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using Shared;

namespace Shared
{
	public class Ship
	{		
		public Ball Ball;
		public Color Color;
		public Fixture Fixture;
		public Fixture FixtureGravity;
		public int Id;
		public bool IsThrusting;
		public bool IsReversing;
		public bool IsBoosting;
		public bool IsLeftTurning;
		public bool IsRightTurning;
		public bool IsBraking;
		public bool IsShooting;
		public bool IsBlocked;
		public bool IsBlockedFrame;
		public Vector2 ShotVector;

		static float drag = .5f;
		static float brakeDrag = 1.5f;
		static float thrust = 24f;
		static float thrustReverse = 24f;
		static float boost = 12f;
		static float turnSpeed = FMath.Pi;
		static float restitution = .8f;
		static float radius = 1f;
		static float radiusGravity = 2f;
		
		public Ship (World world, SpawnData spawn, int id)
		{
			Ball = null;
			Id = id;
			IsThrusting = false;
			IsReversing = false;
			IsBoosting = false;
			IsLeftTurning = false;
			IsRightTurning = false;
			IsShooting = false;
			IsBlocked = false;
			IsBlockedFrame = false;

			Color = spawn.Color;
			var body = new Body(world);

			body.Rotation = FMath.Atan2(-spawn.Position.Y, -spawn.Position.X);			
			body.Position = spawn.Position;		
			var shape = new CircleShape(radius, 1f);
			body.BodyType = BodyType.Dynamic;
			body.FixedRotation = true;
			Fixture = body.CreateFixture(shape);
			Fixture.Restitution = restitution;

			var bodyGravity = new Body(world);
			bodyGravity.Position = spawn.Position;
			var shapeGravity = new CircleShape(radiusGravity, 1f);
			bodyGravity.FixedRotation = true;
			FixtureGravity = bodyGravity.CreateFixture(shapeGravity);
			FixtureGravity.IsSensor = true;
		}

		public void Step(float elasped)
		{			
			if (IsLeftTurning){
				Fixture.Body.Rotation += turnSpeed * elasped;
				Fixture.Body.Rotation %= FMath.TwoPi;
			}
			if (IsRightTurning) {
				Fixture.Body.Rotation -= turnSpeed * elasped;
				Fixture.Body.Rotation %= FMath.TwoPi;
			}

			if (IsBraking) Fixture.Body.LinearDamping = drag + brakeDrag;
			else Fixture.Body.LinearDamping = drag;
			
			Vector2 vector;
			vector.X = FMath.Cos(Fixture.Body.Rotation);
			vector.Y = FMath.Sin(Fixture.Body.Rotation);			
			var force = new Vector2(0f, 0f);
			if (IsBoosting) force += vector * (boost + thrust);
			else if (IsThrusting) force += vector * thrust;
			if (IsReversing) force -= vector * thrustReverse;
			Fixture.Body.ApplyForce(force);
			
			FixtureGravity.Body.Position = Fixture.Body.Position;
		}

		public int MarshalBytes {
			get {
				if (IsShooting) return 29;
				else return 21;
			}
		}
		
		public void Marshal(BinaryWriter writer)
		{			
			byte flags = 0;
			if (IsThrusting) flags |= 1<<0;
			if (IsReversing) flags |= 1<<1;
			if (IsBoosting) flags |= 1<<2;
			if (IsLeftTurning) flags |= 1<<3;
			if (IsRightTurning) flags |= 1<<4;
			if (IsBraking) flags |= 1<<5;
			if (IsShooting) flags |= 1<<6;
			if (IsBlocked) flags |= 1<<7;
			writer.Write(flags);
			if (IsShooting) {
				writer.Write(ShotVector.X);
				writer.Write(ShotVector.Y);
			}
			writer.Write(Fixture.Body.Position.X);
			writer.Write(Fixture.Body.Position.Y);	
			writer.Write(Fixture.Body.LinearVelocity.X);
			writer.Write(Fixture.Body.LinearVelocity.Y);			
			writer.Write(Fixture.Body.Rotation);
		}
		
		public void UnMarshal(BinaryReader reader)
		{
			Ball = null;
			byte flags = reader.ReadByte();
			IsThrusting = (flags >> 0 & 1) == 1;
			IsReversing = (flags >> 1 & 1) == 1;
			IsBoosting = (flags >> 2 & 1) == 1;
			IsLeftTurning = (flags >> 3 & 1) == 1;
			IsRightTurning = (flags >> 4 & 1) == 1;
			IsBraking = (flags >> 5 & 1) == 1;
			IsShooting = (flags >> 6 & 1) == 1;
			IsBlocked = (flags >> 7 & 1) == 1;
			float x, y;
			if (IsShooting) {				
				x = reader.ReadSingle();
				y = reader.ReadSingle();
				ShotVector = new Vector2(x, y);
			}			
			x = reader.ReadSingle();
			y = reader.ReadSingle();
			Fixture.Body.Position = new Vector2(x, y);
			x = reader.ReadSingle();
			y = reader.ReadSingle();
			Fixture.Body.LinearVelocity = new Vector2(x, y);
			Fixture.Body.Rotation = reader.ReadSingle();
		}
	}
}

