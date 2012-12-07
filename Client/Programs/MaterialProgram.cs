using System;
using System.Drawing;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Shared;

namespace Client
{
	public class MaterialProgram
	{
		int program;
		int teamColorLoc;
		int useTextureLoc;
		int modelviewMatrixLoc;
		int projectionMatrixLoc;
		
		int materialAmbientLoc;
		int materialDiffuseLoc;
		int materialSpecularLoc;
		int materialShininessLoc;
		
		int lightAmbientLoc;
		int lightDiffuseLoc;
		int lightSpecularLoc;
		int lightPositionLoc;

		public MaterialProgram ()
		{
			var fragShader = GL.CreateShader(ShaderType.FragmentShader);
			var vertShader = GL.CreateShader(ShaderType.VertexShader);

			using (var sr = new StreamReader("Shaders/material.frag")) {
				GL.ShaderSource(fragShader, sr.ReadToEnd());
			}
			using (var sr = new StreamReader("Shaders/material.vert")) {
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
			GL.BindAttribLocation(program, VertexTNP3.TexCoordIndex, "vert_texcoord");
			GL.BindAttribLocation(program, VertexTNP3.NormalIndex, "vert_normal");
			GL.BindAttribLocation(program, VertexTNP3.PositionIndex, "vert_position");
			
			GL.LinkProgram(program);
			
			Use();
			teamColorLoc = GL.GetUniformLocation(program, "team_color");
			useTextureLoc = GL.GetUniformLocation(program, "use_texture");
			projectionMatrixLoc = GL.GetUniformLocation(program, "projection_matrix");
			modelviewMatrixLoc = GL.GetUniformLocation(program, "modelview_matrix");
			
			materialAmbientLoc = GL.GetUniformLocation(program, "material_ambient");
			materialDiffuseLoc = GL.GetUniformLocation(program, "material_diffuse");
			materialSpecularLoc = GL.GetUniformLocation(program, "material_specular");
			materialShininessLoc = GL.GetUniformLocation(program, "material_shininess");
			
			lightAmbientLoc = GL.GetUniformLocation(program, "light_ambient");
			lightDiffuseLoc = GL.GetUniformLocation(program, "light_diffuse");
			lightSpecularLoc = GL.GetUniformLocation(program, "light_specular");
			lightPositionLoc = GL.GetUniformLocation(program, "light_position");			
		}
		
		public void Use()
		{
			GL.UseProgram(program);
		}
		
		public Color TeamColor {
			set {
				GL.Uniform3(teamColorLoc, (float) value.R / 255f, (float) value.G / 255f, (float) value.B / 255f);
			}
		}
		
		public bool UseTexture {
			set {
				if (value) GL.Uniform1(useTextureLoc, 1);
				else GL.Uniform1(useTextureLoc, 0);
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
		
		public Vector4 MaterialAmbient {
			set {
				GL.Uniform4(materialAmbientLoc, value);
			}
		}
		public Vector4 MaterialDiffuse {
			set {
				GL.Uniform4(materialDiffuseLoc, value);
			}
		}
		public Vector4 MaterialSpecular {
			set {
				GL.Uniform4(materialSpecularLoc, value);
			}
		}
		public float MaterialShininess {
			set {
				GL.Uniform1(materialShininessLoc, value);
			}
		}		

		public Vector4 LightAmbient {
			set {
				GL.Uniform4(lightAmbientLoc, value);
			}
		}
		public Vector4 LightDiffuse {
			set {
				GL.Uniform4(lightDiffuseLoc, value);
			}
		}
		public Vector4 LightSpecular {
			set {
				GL.Uniform4(lightSpecularLoc, value);
			}
		}
		public Vector4 LightPosition {
			set {
				GL.Uniform4(lightPositionLoc, value);
			}
		}		
		
	}
}

