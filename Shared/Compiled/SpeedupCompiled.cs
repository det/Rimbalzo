using System;
using System.Drawing;
using System.Collections.Generic;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using Microsoft.Xna.Framework;

namespace Shared
{
	public struct SpeedupCompiled : ISensor
	{
		List<Vertices> triangles;
		Color color;
		Vector2 rotation;
		
		public List<Vertices> Triangles {
			get { return triangles; }
		}		
		public Color Color {
			get { return color; }
		}		
		public Vector2 Rotation {
			get { return rotation; }
		}
	
		public Vector2 Force;

		public SpeedupCompiled (SpeedupData data)
		{
			var verts = new Vertices(data.Verts);
			triangles = CDTDecomposer.ConvexPartition(verts);
			color = data.Color;
			Force = data.Force;
			rotation.X = Force.X;
			rotation.Y = Force.Y;			
			rotation.Normalize();
		}
	}
}

