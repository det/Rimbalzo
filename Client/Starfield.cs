using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Shared;
using System.Runtime.InteropServices;

namespace Client
{
	public class Starfield
	{
		int vao;
		int vbo;
		
		static int numStars = 512;

		public Starfield ()
		{
			var tris = new Triangle<VertexP2>[numStars * 2];
			
			var random = new Random();			
			for (int i = 0; i<numStars; i++) {
				var size = (float) random.NextDouble() * .5f + .25f;
				var w = 4096f;
				var w2 = w / 2f;
				var h = 4096f;
				var h2 = h / 2f;
				var x = (float) random.NextDouble() * w - w2;
				var y = (float) random.NextDouble() * h - h2;
				var left = x - size;
				var right = x + size;
				var bottom = y - size;
				var top = y + size;
				var p1 = new Vector2(left , bottom);
				var p2 = new Vector2(right, bottom);
				var p3 = new Vector2(right, top);
				var p4 = new Vector2(left, top);
				
				tris[i].A.Position = p1;
				tris[i].B.Position = p2;
				tris[i].C.Position = p3;
				tris[i+1].A.Position = p3;
				tris[i+1].B.Position = p4;
				tris[i+1].C.Position = p1;	
			}
			
			
			GL.GenVertexArrays(1, out vao);
			GL.BindVertexArray(vao);
			GL.GenBuffers(1, out vbo);
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
			var sizeInBytes = Marshal.SizeOf(new Triangle<VertexP2>());
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(sizeInBytes * 2 * numStars), tris, BufferUsageHint.StaticDraw);
			new VertexP2().SetupAttributes();
			GL.BindVertexArray(0);
		}
		
		public void Render()
		{			
			GL.Disable(EnableCap.DepthTest);
			GL.BindVertexArray(vao);
			GL.DrawArrays(BeginMode.Triangles, 0, numStars*2*3);
			GL.BindVertexArray(0);
			GL.Enable(EnableCap.DepthTest);
		}
	}
}