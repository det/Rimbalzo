using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Runtime.InteropServices;
using Shared;

namespace Client
{
	public class Image
	{
		int vao;
		int vbo;
		Texture texture;

		public Image (string path, float right, float top) : this(path, 0f, right, 0f, top) {}
		
		public Image (string path, float left, float right, float bottom, float top)
		{
			texture = new Texture(path);
			GL.GenVertexArrays(1, out vao);
			GL.BindVertexArray(vao);
			GL.GenBuffers(1, out vbo);
			SetSize(left, right, bottom, top);
			new VertexTP2().SetupAttributes();
			GL.BindVertexArray(0);
		}
		
		
		public void SetSize(float right, float top)
		{
			SetSize(0f, right, 0f, top);
		}
		
		public void SetSize(float left, float right, float bottom, float top)
		{
			var tris = new Triangle<VertexTP2>[2];
			var p1 = new Vector2(left , bottom);
			var p2 = new Vector2(right, bottom);
			var p3 = new Vector2(right, top);
			var p4 = new Vector2(left, top);
			var t1 = new Vector2(0f, 0f);
			var t2 = new Vector2(1f, 0f);
			var t3 = new Vector2(1f, 1f);
			var t4 = new Vector2(0f, 1f);
			
			tris[0].A.Position = p1;
			tris[0].A.TexCoord = t1;
			tris[0].B.Position = p2;
			tris[0].B.TexCoord = t2;
			tris[0].C.Position = p3;
			tris[0].C.TexCoord = t3;
			tris[1].A.Position = p3;
			tris[1].A.TexCoord = t3;
			tris[1].B.Position = p4;
			tris[1].B.TexCoord = t4;
			tris[1].C.Position = p1;
			tris[1].C.TexCoord = t1;
			
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
			var sizeInBytes = Marshal.SizeOf(new Triangle<VertexTP2>());
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(sizeInBytes * 2), tris, BufferUsageHint.StaticDraw);
		}
	
		public void Render()
		{
			GL.Disable(EnableCap.DepthTest);
			texture.Bind();
			GL.BindVertexArray(vao);
			GL.DrawArrays(BeginMode.Triangles, 0, 6);
			GL.BindVertexArray(0);
			GL.Enable(EnableCap.DepthTest);
		}

	}
}

