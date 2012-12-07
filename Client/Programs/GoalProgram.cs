using System;
using System.Drawing;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Client
{
	public class GoalProgram
	{
		public int Program;
		float clock;
		float fade;
		int teamColorLoc;
		int rotationLoc;
		int modelviewMatrixLoc;
		int projectionMatrixLoc;

		public GoalProgram ()
		{
			var fragShader = GL.CreateShader(ShaderType.FragmentShader);
			var vertShader = GL.CreateShader(ShaderType.VertexShader);
			
			using (var sr = new StreamReader("Shaders/goal.frag")) {
				GL.ShaderSource(fragShader, sr.ReadToEnd());
			}
			using (var sr = new StreamReader("Shaders/goal.vert")) {
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
			teamColorLoc = GL.GetUniformLocation(Program, "team_color");
			rotationLoc = GL.GetUniformLocation(Program, "rotation");
			projectionMatrixLoc = GL.GetUniformLocation(Program, "projection_matrix");
			modelviewMatrixLoc = GL.GetUniformLocation(Program, "modelview_matrix");
		}
		
		public void Step(float seconds)
		{
			clock += seconds;
			var phase = clock % 2f;
			if (phase < 1f) fade = phase;
			else fade = (2f - phase);
			fade = (fade + 1f) / 2f;
		}
		
		public void Use()
		{
			GL.UseProgram(Program);
		}
		
		public Color TeamColor {
			set {
				float red = value.R * fade / 255f;
				float green = value.G * fade / 255f;
				float blue = value.B * fade / 255f;
				GL.Uniform3(teamColorLoc, red, green, blue);
			}
		}

		public Vector2 Rotation {
			set {
				GL.Uniform2(rotationLoc, value);
			}
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

