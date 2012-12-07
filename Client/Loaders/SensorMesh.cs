using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Shared;
using FarseerPhysics.Common;

namespace Client
{
	struct GoalUniforms {
		public Color Color;
		public Vector2 Rotation;
	}
	
	struct GoalUniformGroup {
		public GoalUniforms Uniforms;
		public CompiledMesh<VertexP2> Mesh;
	}

	public class SensorMesh<T> where T : ISensor
	{
		Texture texture;
		List<GoalUniformGroup> uniformGroups;

		public SensorMesh (List<T> sensors, Texture texture)
		{
			this.texture = texture;
			uniformGroups = new List<GoalUniformGroup>();

			var uniformsMap = new Dictionary<GoalUniforms, MeshBuilder<VertexP2>>();
			foreach (var sensor in sensors) {
				GoalUniforms uniforms;
				uniforms.Color = sensor.Color;
				uniforms.Rotation.X = sensor.Rotation.X;				
				uniforms.Rotation.Y = sensor.Rotation.Y;

				MeshBuilder<VertexP2> builder;
				if (!uniformsMap.TryGetValue(uniforms, out builder)) {
					builder = new MeshBuilder<VertexP2>();
					uniformsMap.Add(uniforms, builder);
				}

				Triangle<VertexP2> tri;
				foreach (var verts in sensor.Triangles) {
					tri.A.Position.X = verts[0].X;
					tri.A.Position.Y = verts[0].Y;
					tri.B.Position.X = verts[1].X;
					tri.B.Position.Y = verts[1].Y;
					tri.C.Position.X = verts[2].X;
					tri.C.Position.Y = verts[2].Y;
					builder.Add(tri);
				}
			}
			
			foreach (var pair in uniformsMap) {
				GoalUniformGroup g;
				g.Uniforms = pair.Key;
				g.Mesh = pair.Value.ToMesh().Compile();
				uniformGroups.Add(g);
			}
		}
		
		public void Render()
		{			
			texture.Bind();
			GL.Disable(EnableCap.DepthTest);


			foreach (var uniformGroup in uniformGroups) {
				Programs.Goal.Rotation = uniformGroup.Uniforms.Rotation;
				Programs.Goal.TeamColor = uniformGroup.Uniforms.Color;
				uniformGroup.Mesh.Render();
			}
			GL.Enable(EnableCap.DepthTest);
		}

	}
}

