using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace Client
{
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexTP2 : IVertex
	{
		public static int TexCoordIndex = 0;
		public static int PositionIndex = 1;
		
		public Vector2 TexCoord;
		public Vector2 Position;
		
		public void SetupAttributes()
		{
			var stride = Marshal.SizeOf(new VertexTP2());
			var texOffset = Marshal.OffsetOf(typeof(VertexTP2), "TexCoord");
			var posOffset = Marshal.OffsetOf(typeof(VertexTP2), "Position");
			
			GL.EnableVertexAttribArray(TexCoordIndex);
			GL.VertexAttribPointer(TexCoordIndex, 2, VertexAttribPointerType.Float, false, stride, texOffset);
			GL.EnableVertexAttribArray(PositionIndex);
			GL.VertexAttribPointer(PositionIndex, 2, VertexAttribPointerType.Float, false, stride, posOffset);			
		}
	}
}