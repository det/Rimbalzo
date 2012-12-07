using System;
using System.Drawing;
using System.Collections.Generic;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using Microsoft.Xna.Framework;

namespace Shared
{
	public struct GoalCompiled : ISensor
	{
		List<Vertices> triangles;
		Color color;
		
		public List<Vertices> Triangles {
			get { return triangles; }
		}		
		public Color Color {
			get { return color; }
		}
		public Vector2 Rotation {
			get { return new Vector2(1f, 0f); }
		}	
		
		public GoalCompiled (GoalData data)
		{
			var verts = new Vertices(data.Verts);
			triangles = CDTDecomposer.ConvexPartition(verts);
			color = data.Color;
		}
	}
}

