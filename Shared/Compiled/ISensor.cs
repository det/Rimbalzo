using System;
using System.Drawing;
using System.Collections.Generic;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;


namespace Shared
{
	public interface ISensor
	{
		List<Vertices> Triangles { get; }
		Color Color { get; }
		Vector2 Rotation { get; }
	}
}

