using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace Client
{
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexTNP3 : IVertex
	{
		public static int TexCoordIndex = 0;
		public static int NormalIndex = 1;
		public static int PositionIndex = 2;
		
		public Vector2 TexCoord;
		public Vector3 Normal;
		public Vector3 Position;
		
		public void SetupAttributes()
		{
			var stride = Marshal.SizeOf(new VertexTNP3());
			var texOffset = Marshal.OffsetOf(typeof(VertexTNP3), "TexCoord");
			var normOffset = Marshal.OffsetOf(typeof(VertexTNP3), "Normal");
			var posOffset = Marshal.OffsetOf(typeof(VertexTNP3), "Position");
			
			GL.EnableVertexAttribArray(TexCoordIndex);
			GL.VertexAttribPointer(TexCoordIndex, 2, VertexAttribPointerType.Float, false, stride, texOffset);
			GL.EnableVertexAttribArray(NormalIndex);
			GL.VertexAttribPointer(NormalIndex, 3, VertexAttribPointerType.Float, false, stride, normOffset);
			GL.EnableVertexAttribArray(PositionIndex);
			GL.VertexAttribPointer(PositionIndex, 3, VertexAttribPointerType.Float, false, stride, posOffset);
			
		}
	}
}