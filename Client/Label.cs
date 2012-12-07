
using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using Shared;

namespace Client
{
	public class Label
	{
		int vao;
		int vbo;
		int vboLength;
		Texture texture;
		
		public Label(Texture texture, string text)
		{
			this.texture = texture;
			GL.GenVertexArrays(1, out vao);
			GL.BindVertexArray(vao);
			GL.GenBuffers(1, out vbo);			
			this.Text = text;
			new VertexTP2().SetupAttributes();
			GL.BindVertexArray(0);
		}
		
		public string Text {
			set {
				var asciiBytes = System.Text.Encoding.ASCII.GetBytes(value);
				vboLength = asciiBytes.Length * 2;
				var tris = new Triangle<VertexTP2>[vboLength];
				
				for (int i = 0; i<asciiBytes.Length; i++) {
					var b = asciiBytes[i];
					if (b < 32 || b > 126) { b = 63; }
					
					var letterIndex = (float) b - 32.0f;
					var texelWidth = 1.0f / 95.0f;
					var pixelWidth = texture.Size.Width / 95.0f;
					var height = texture.Size.Height;
					var pixelOffset = (float) i * pixelWidth;

					var t1 = new Vector2(texelWidth*letterIndex, 0f);
					var p1 = new Vector2(0f+pixelOffset, 0f);
					var t2 = new Vector2(texelWidth*letterIndex+texelWidth, 0f);
					var p2 = new Vector2(pixelWidth+pixelOffset, 0f);
					var t3 = new Vector2(texelWidth*letterIndex+texelWidth, 1f);
					var p3 = new Vector2(pixelWidth+pixelOffset, height);
					var t4 = new Vector2(texelWidth*letterIndex, 1f);
					var p4 = new Vector2(0f+pixelOffset, height);
					
					tris[i*2].A.Position = p1;
					tris[i*2].A.TexCoord = t1;
					tris[i*2].B.Position = p2;
					tris[i*2].B.TexCoord = t2;
					tris[i*2].C.Position = p3;
					tris[i*2].C.TexCoord = t3;
					tris[i*2+1].A.Position = p3;
					tris[i*2+1].A.TexCoord = t3;
					tris[i*2+1].B.Position = p4;
					tris[i*2+1].B.TexCoord = t4;
					tris[i*2+1].C.Position = p1;
					tris[i*2+1].C.TexCoord = t1;
				}

				GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
				var sizeInBytes = Marshal.SizeOf(new Triangle<VertexTP2>());
				GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(sizeInBytes * vboLength), tris, BufferUsageHint.DynamicDraw);
			}			
		}
		
		public void Render()
		{
			GL.Disable(EnableCap.DepthTest);
			texture.Bind();
			GL.BindVertexArray(vao);
			GL.DrawArrays(BeginMode.Triangles, 0, vboLength*3);
			GL.BindVertexArray(0);
			GL.Enable(EnableCap.DepthTest);
		}
	}
}
