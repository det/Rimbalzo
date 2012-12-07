using System;
using System.Drawing;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Client
{
	public class TraceProgram
	{
		public int Program;
		int modelviewMatrixLoc;
		int projectionMatrixLoc;

		public TraceProgram ()
		{
			var fragShader = GL.CreateShader(ShaderType.FragmentShader);
			var vertShader = GL.CreateShader(ShaderType.VertexShader);
			
			using (var sr = new StreamReader("Shaders/trace.frag")) {
				GL.ShaderSource(fragShader, sr.ReadToEnd());
			}
			using (var sr = new StreamReader("Shaders/trace.vert")) {
				GL.ShaderSource(vertShader, sr.ReadToEnd());
			}
						
			string info;
			GL.CompileShader(fragShader);			
			GL.GetShaderInfoLog(fragShader, out info);
			Console.WriteLine(info.TrimEnd(System.Environment.NewLine.ToCharArray()));
			GL.CompileShader(vertShader);
			GL.GetShaderInfoLog(vertShader, out info);
			Console.WriteLine(info.TrimEnd(System.Environment.NewLine.ToCharArray()));
			
			Program = GL.CreateProgram();
			GL.AttachShader(Program, fragShader);
			GL.AttachShader(Program, vertShader);
			
			GL.BindAttribLocation(Program, 0, "vert_position");
			GL.LinkProgram(Program);
			
			Use();
			projectionMatrixLoc = GL.GetUniformLocation(Program, "projection_matrix");
			modelviewMatrixLoc = GL.GetUniformLocation(Program, "modelview_matrix");
		}
		
		public void Use()
		{
			GL.UseProgram(Program);
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

