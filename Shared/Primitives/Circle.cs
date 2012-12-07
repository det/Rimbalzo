using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace Shared
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Circle
	{
		public Vector2 Position;
		public float Radius;		
	}
}

