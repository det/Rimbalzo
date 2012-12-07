using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace Client
{
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexP2 : IVertex
	{
		public static int PositionIndex = 0;

		public Vector2 Position;
		
		public void SetupAttributes()
		{
			var stride = Marshal.SizeOf(new VertexP2());
			var posOffset = Marshal.OffsetOf(typeof(VertexP2), "Position");

			GL.EnableVertexAttribArray(PositionIndex);
			GL.VertexAttribPointer(PositionIndex, 2, VertexAttribPointerType.Float, false, stride, posOffset);			
			
		}
	}
}