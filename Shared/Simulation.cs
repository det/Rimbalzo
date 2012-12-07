using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Collision.Shapes;

namespace Shared
{
	enum FixtureKind { Wall, Ship, Ball, Goal, Speedup, Gravity, Goalie }

	struct FixtureKey {
		public FixtureKind Kind;
		public int Index;
	}
	
	public class Simulation
	{		
		Dictionary<int, FixtureKey> fixtureMap;
		public World World;
		public Ship[] Ships;
		public Ball[] Balls;
		public Color[] Goals;
		public Goalie[] Goalies;
		public Vector2[] Speedups;

		public Simulation (MapCompiled map)
		{
			World = new World(Vector2.Zero);
			fixtureMap = new Dictionary<int, FixtureKey>();

			Fixture fixture;
			FixtureKey key;

			
			Goalies = new Goalie[map.Goalies.Length];
			for (int i = 0; i < map.Goalies.Length; i++) {
				Goalies[i] = new Goalie(World, map.Goalies[i]);
				key.Index = i;
				key.Kind = FixtureKind.Goalie;
				fixtureMap[Goalies[i].Fixture.FixtureId] = key;
			}
	
			Balls = new Ball[map.Balls.Length];
			for (int i = 0; i < map.Balls.Length; i++) {
				Balls[i] = new Ball(this, map.Balls[i]);
				
				key.Index = i;
				key.Kind = FixtureKind.Ball;				
				fixtureMap[Balls[i].Fixture.FixtureId] = key;				
			}

			for (int i = 0; i < map.Walls.Count; i++) {
				var wall = new Wall(World, map.Walls[i]);
				
				foreach (var wallFixture in wall.Fixtures) {
					key.Index = i;
					key.Kind = FixtureKind.Wall;
					fixtureMap[wallFixture.FixtureId] = key;
				}
			}

			
			Ships = new Ship[map.Spawns.Length];
			for (int i = 0; i < map.Spawns.Length; i++) {
				Ships[i] = new Ship(World, map.Spawns[i], i);
				key.Index = i;
				key.Kind = FixtureKind.Ship;				
				fixtureMap[Ships[i].Fixture.FixtureId] = key;
				key.Kind = FixtureKind.Gravity;				
				fixtureMap[Ships[i].FixtureGravity.FixtureId] = key;			
			}
			

			Goals = new Color[map.NumGoalTriangles];
			int goalCount = 0;
			foreach (var goal in map.Goals) {
				foreach (var tri in goal.Triangles) {
					var goalBody = new Body(World);
					var goalShape = new PolygonShape(tri, 1f);

					fixture = goalBody.CreateFixture(goalShape);
					fixture.IsSensor = true;
					key.Index = goalCount;
					key.Kind = FixtureKind.Goal;
					fixtureMap[fixture.FixtureId] = key;			
					
					Goals[goalCount] = goal.Color;
					goalCount++;					
				}
			}

			Speedups = new Vector2[map.NumSpeedupTriangles];
			int speedupCount = 0;
			foreach (var speedup in map.Speedups) {
				foreach (var tri in speedup.Triangles) {
					var speedupBody = new Body(World);
					var speedupShape = new PolygonShape(tri, 1f);

					fixture = speedupBody.CreateFixture(speedupShape);
					fixture.IsSensor = true;
					key.Index = speedupCount;
					key.Kind = FixtureKind.Speedup;
					fixtureMap[fixture.FixtureId] = key;			
					
					Speedups[speedupCount] = speedup.Force;
					speedupCount++;
				}
			}

			World.ContactManager.ContactFilter = ContactFilter;
		}
		

		public bool ContactFilter(Fixture a, Fixture b)
		{
			var key1 = fixtureMap[a.FixtureId];
			var key2 = fixtureMap[b.FixtureId];
			switch (key1.Kind) {

			case FixtureKind.Ball:
				switch (key2.Kind) {
				case FixtureKind.Ship: return !Ships[key2.Index].IsBlocked;
				}
				break;
				
			case FixtureKind.Ship:
				switch (key2.Kind) {
				case FixtureKind.Ball: return !Ships[key1.Index].IsBlocked;
				}
				break;
			}
			return true;
		}
		
		public void Step(float elapsed)
		{
			foreach (var ball in Balls) ball.Step(elapsed);
			foreach (var ship in Ships) ship.Step(elapsed);
			foreach (var goalie in Goalies) goalie.Step(elapsed, this);
			World.Step(elapsed);

			foreach (var ship in Ships) ship.IsBlockedFrame = false;
			
			var contact = World.ContactList;
			while (contact != null) {
				if (contact.IsTouching()) {
					var key1 = fixtureMap[contact.FixtureA.FixtureId];
					var key2 = fixtureMap[contact.FixtureB.FixtureId];					
					switch (key1.Kind) {

					case FixtureKind.Ball:
						switch (key2.Kind) {
						case FixtureKind.Goal:
							HandleBallGoal(key1.Index, key2.Index);
							break;
						case FixtureKind.Speedup:
							HandleBallSpeedup(key1.Index, key2.Index);
							break;
						case FixtureKind.Ship:
							HandleBallShip(key1.Index, key2.Index);
							break;
						case FixtureKind.Gravity:
							HandleBallGravity(key1.Index, key2.Index);
							break;

						}
						break;
					
					case FixtureKind.Goal:
						switch (key2.Kind) {
						case FixtureKind.Ball:
							HandleBallGoal(key2.Index, key1.Index);
							break;
						}
						break;
					
					case FixtureKind.Ship:
						switch (key2.Kind) {
						case FixtureKind.Speedup:
							HandleShipSpeedup(key1.Index, key2.Index);
							break;
						case FixtureKind.Ship:
							HandleShipShip(key1.Index, key2.Index);
							break;
						case FixtureKind.Ball:
							HandleBallShip(key2.Index, key1.Index);
							break;
						}
						break;

					case FixtureKind.Speedup:
						switch (key2.Kind) {
						case FixtureKind.Ball:
							HandleBallSpeedup(key2.Index, key1.Index);
							break;
						case FixtureKind.Ship:
							HandleShipSpeedup(key2.Index, key1.Index);
							break;
						}
						break;
						
					case FixtureKind.Gravity:
						switch (key2.Kind) {
						case FixtureKind.Ball:
							HandleBallGravity(key2.Index, key1.Index);
							break;
						}
						break;
					}
				}
				contact = contact.Next;
			}
			
			foreach (var ship in Ships) {
				if (ship.Ball != null) {
					if (ship.IsShooting) {
						ship.Ball.Fixture.Body.Position = ship.Fixture.Body.Position;
						ship.Ball.Fixture.Body.Enabled = true;
						ship.Ball.Fixture.Body.LinearVelocity = ship.Fixture.Body.LinearVelocity;
						var impulse = ship.ShotVector * 5f;
						ship.Ball.Fixture.Body.ApplyLinearImpulse(ref impulse);
						impulse = ship.ShotVector * -10f;
						ship.Fixture.Body.ApplyLinearImpulse(ref impulse);
						ship.Ball.State = BallState.Free;
						ship.Ball.Carrier = null;
						ship.Ball = null;						
					}
					ship.IsBlockedFrame = true;
				}
			}
			
			foreach (var ship in Ships)	ship.IsBlocked = ship.IsBlocked && ship.IsBlockedFrame;
		}
		
		void HandleBallGoal(int index1, int index2) {
			Balls[index1].Spawn();
		}
		
		
		void HandleBallSpeedup(int index1, int index2) {
			var impulse = Speedups[index2];
			Balls[index1].Fixture.Body.ApplyLinearImpulse(ref impulse);
		}
		
		void HandleShipSpeedup(int index1, int index2) {
			var impulse = Speedups[index2];
			Ships[index1].Fixture.Body.ApplyLinearImpulse(ref impulse);
		}
	
		void HandleBallShip(int index1, int index2) {
			if (Ships[index2].Ball == null) {
				Ships[index2].Ball = Balls[index1];				
				Ships[index2].IsBlocked = true;
				Balls[index1].Carrier = Ships[index2];
				Balls[index1].State = BallState.Carried;
				Balls[index1].Fixture.Body.Enabled = false;
			}
		}

		void HandleShipShip(int index1, int index2) {
			if (Ships[index1].Ball != null) {
				var bumpVector = Ships[index1].Fixture.Body.Position - Ships[index2].Fixture.Body.Position;
				bumpVector.Normalize();
				Ships[index1].Ball.Carrier = null;
				Ships[index1].Ball.State = BallState.Free;
				Ships[index1].Ball.Fixture.Body.Position = Ships[index1].Fixture.Body.Position;
				Ships[index1].Ball.Fixture.Body.Enabled = true;
				Ships[index1].Ball.Fixture.Body.LinearVelocity = Ships[index1].Fixture.Body.LinearVelocity * 1.5f;
				Ships[index1].Ball = null;										
				Ships[index1].IsBlockedFrame = true;
			}
			if (Ships[index2].Ball != null) {
				var bumpVector = Ships[index2].Fixture.Body.Position - Ships[index1].Fixture.Body.Position;
				bumpVector.Normalize();
				Ships[index2].Ball.Carrier = null;
				Ships[index2].Ball.State = BallState.Free;
				Ships[index2].Ball.Fixture.Body.Position = Ships[index2].Fixture.Body.Position;
				Ships[index2].Ball.Fixture.Body.Enabled = true;
				Ships[index2].Ball.Fixture.Body.LinearVelocity = Ships[index2].Fixture.Body.LinearVelocity * 1.5f;
				Ships[index2].Ball = null;										
				Ships[index2].IsBlockedFrame = true;
			}
		}		
			
		void HandleBallGravity(int index1, int index2) {
			if (!Ships[index2].IsBlocked) {
				var vector = Ships[index2].Fixture.Body.Position - Balls[index1].Fixture.Body.Position;
				var distance = vector.Length();
				var force = 16f / distance;
				vector.Normalize();
				Balls[index1].Fixture.Body.ApplyForce(vector * force);
			}
			Ships[index2].IsBlockedFrame = true;
		}		

		public byte[] ToBytes()
		{
			int length = 0;
			foreach (var ship in Ships) length += ship.MarshalBytes;
			foreach (var ball in Balls) length += ball.MarshalBytes;
			foreach (var goalie in Goalies) length += goalie.MarshalBytes;
			
			var bytes = new byte[length];
			var stream = new MemoryStream(bytes);
			var writer = new BinaryWriter(stream);
			Marshal(writer);
			return bytes;
		}
		
		public void FromBytes(byte[] bytes)
		{
			var stream = new MemoryStream(bytes);
			var reader = new BinaryReader(stream);
			UnMarshal(reader);
		}
		
		public void Marshal(BinaryWriter writer)
		{
			foreach (var ship in Ships) ship.Marshal(writer);
			foreach (var ball in Balls) ball.Marshal(writer);
			foreach (var goalie in Goalies) goalie.Marshal(writer);
		}
		
		public void UnMarshal(BinaryReader reader)
		{
			foreach (var ship in Ships) ship.UnMarshal(reader);
			foreach (var ball in Balls) ball.UnMarshal(reader);
			foreach (var goalie in Goalies) goalie.UnMarshal(reader);
		}
	}
}  