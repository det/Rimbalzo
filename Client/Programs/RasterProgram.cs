using System;
using System.Drawing;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Shared;

namespace Client
{
	public class RasterProgram
	{
		int program;
		int modelviewMatrixLoc;
		int projectionMatrixLoc;

		public RasterProgram ()
		{
			var fragShader = GL.CreateShader(ShaderType.FragmentShader);
			var vertShader = GL.CreateShader(ShaderType.VertexShader);
			
			using (var sr = new StreamReader("Shaders/raster.frag")) {
				GL.ShaderSource(fragShader, sr.ReadToEnd());
			}
			using (var sr = new StreamReader("Shaders/raster.vert")) {
				GL.ShaderSource(vertShader, sr.ReadToEnd());
			}
						
			string info;
			GL.CompileShader(fragShader);			
			GL.GetShaderInfoLog(fragShader, out info);
			Console.WriteLine(info.TrimEnd(System.Environment.NewLine.ToCharArray()));
			GL.CompileShader(vertShader);
			GL.GetShaderInfoLog(vertShader, out info);
			Console.WriteLine(info.TrimEnd(System.Environment.NewLine.ToCharArray()));
			
			program = GL.CreateProgram();
			GL.AttachShader(program, fragShader);
			GL.AttachShader(program, vertShader);
			
			GL.BindAttribLocation(program, VertexTP2.PositionIndex, "vert_position");
			GL.BindAttribLocation(program, VertexTP2.TexCoordIndex, "vert_texcoord");
			GL.LinkProgram(program);
			
			Use();
			projectionMatrixLoc = GL.GetUniformLocation(program, "projection_matrix");
			modelviewMatrixLoc = GL.GetUniformLocation(program, "modelview_matrix");
		}
		
		public void Use()
		{
			GL.UseProgram(program);
		}
		
		public Matrix4 ModelviewMatrix {
			set {
				GL.UniformMatrix4(modelviewMatrixLoc, false, ref value);
			}
		}
		
		public Matrix4 ProjectionMatrix {
			set {
				GL.UniformMatrix4(projectionMatrixLoc, false, ref value);
			}
		}
	}
}

