using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;

namespace Shared
{
	public class Wall
	{
		public Fixture[] Fixtures;

		public Wall (World world, WallCompiled wall)
		{
			if (wall.Shape == ShapeData.Closed) Fixtures = new Fixture[wall.Verts1.Length];
			else Fixtures = new Fixture[wall.Verts1.Length-1];			
			for (int i = 0; i < wall.Verts1.Length-1; i++) addQuad(world, wall, i, i+1);
			if (wall.Shape == ShapeData.Closed) addQuad(world, wall, wall.Verts1.Length-1, 0);
		}
		
		public void addQuad(World world, WallCompiled wall, int a, int b)
		{
			var verts = new Vertices(4);
			verts.Add(wall.Verts1[a]);
			verts.Add(wall.Verts4[a]);
			verts.Add(wall.Verts4[b]);
			verts.Add(wall.Verts1[b]);

			var wallBody = new Body(world);
			var wallShape = new PolygonShape(verts, 1f);
			Fixtures[a] = wallBody.CreateFixture(wallShape);
			Fixtures[a].Friction = 0f;
		}
	}
}

