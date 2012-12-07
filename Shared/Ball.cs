using System;
using System.IO;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework;

namespace Shared
{
	public enum BallState: byte { Free, Carried, Spawning }
	
	public class Ball
	{
		public static float SpawnTime = 5f;
		
		static float restitution = .8f;
		static float radius = .25f;
		static float drag = .5f;
	
		Simulation simulation;		
		
		public BallState State;
		public Ship Carrier;
		public Fixture Fixture;
		public Vector2 SpawnPos;
		public float SpawnClock;		

		public Ball (Simulation sim, BallData spawn)
		{
			simulation = sim;
			var body = new Body(sim.World);
			var shape = new CircleShape(radius, 1f);
			body.BodyType = BodyType.Dynamic;
			body.FixedRotation = true;
			body.LinearDamping = drag;
			body.IsBullet = true;
			Fixture = body.CreateFixture(shape);
			Fixture.Restitution = restitution;
			Fixture.Friction = 0f;
			Carrier = null;
			SpawnPos = spawn.Position;
			Spawn();
		}
		
		public int MarshalBytes {
			get {
				switch (State) {
				case BallState.Free: return 17;
				case BallState.Carried: return 5;
				case BallState.Spawning: return 5;
				default: return 0; // Compiler is dumb
				}
			}
		}
		
		public void Marshal(BinaryWriter writer)
		{
			writer.Write((byte) State);
			switch (State) {
			case BallState.Carried:
				writer.Write(Carrier.Id);
				break;
			case BallState.Free:
				writer.Write(Fixture.Body.Position.X);
				writer.Write(Fixture.Body.Position.Y);	
				writer.Write(Fixture.Body.LinearVelocity.X);
				writer.Write(Fixture.Body.LinearVelocity.Y);
				break;
			case BallState.Spawning:
				writer.Write(SpawnClock);
				break;
			}
		}
		
		public void UnMarshal(BinaryReader reader)
		{			
			float x, y;
			
			State = (BallState) reader.ReadByte();
			
			switch (State) {
			case BallState.Carried:				
				var id = reader.ReadInt32();
				Carrier = simulation.Ships[id];
				Fixture.Body.Enabled = false;
				simulation.Ships[id].Ball = this;
				break;
			case BallState.Free:
				Carrier = null;
				Fixture.Body.Enabled = true;
				x = reader.ReadSingle();
				y = reader.ReadSingle();
				Fixture.Body.Position = new Vector2(x, y);
				x = reader.ReadSingle();
				y = reader.ReadSingle();				
				Fixture.Body.LinearVelocity = new Vector2(x, y);
				break;			
			case BallState.Spawning:
				Carrier = null;
				Fixture.Body.Enabled = false;
				Fixture.Body.LinearVelocity = new Vector2(0f, 0f);
				Fixture.Body.Position = SpawnPos;
				SpawnClock = reader.ReadSingle();
				break;
			}
		}
		
		public Vector2 BallStop {
			get {
				return Fixture.Body.Position + Fixture.Body.LinearVelocity / drag;
			}
		}
		
		public void Step(float elapsed)
		{
			if (State == BallState.Spawning) {
				SpawnClock -= elapsed;
				if (SpawnClock < 0f) {
					State = BallState.Free;
					Fixture.Body.Enabled = true;
				}
			}
		}

		public void Spawn()
		{
			Fixture.Body.Enabled = false;
			Fixture.Body.LinearVelocity = new Vector2(0f, 0f);
			Fixture.Body.Position = SpawnPos;
			State = BallState.Spawning;
			SpawnClock = SpawnTime;
		}
	}
}

