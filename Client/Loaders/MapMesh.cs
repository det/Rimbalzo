using System;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Shared;

namespace Client
{
	struct WallColorGroup {
		public Color Color;
		public CompiledMesh<VertexTNP3> Mesh;
	}
	

	public class MapMesh
	{
		List<WallColorGroup> colorGroups;
		Texture texture;
		Material material;

		static float bottomLength = 0f;
		static float midLength = .125f;
		static float topLength = .25f;
		
		public MapMesh (MapCompiled map)
		{
			texture = new Texture("Images/wall.png");		
			material = new ObjMaterial("Models/wall.mtl").Lookup("wall");
			colorGroups = new List<WallColorGroup>();			

			var colorMap = new Dictionary<Color, MeshBuilder<VertexTNP3>>();			
			foreach (var wall in map.Walls) {
				MeshBuilder<VertexTNP3> builder;
				if (!colorMap.TryGetValue(wall.Color, out builder)) {
					builder = new MeshBuilder<VertexTNP3>();
					colorMap.Add(wall.Color, builder);
				}
				
				for (int i = 0; i < wall.Verts1.Length-1; i++) makeQuads(wall, builder, i, i+1);
				if (wall.Shape == ShapeData.Closed) makeQuads(wall, builder, wall.Verts1.Length-1, 0);
			}

			foreach (var pair in colorMap) {
				WallColorGroup g;
				g.Color = pair.Key;
				g.Mesh = pair.Value.ToMesh().Compile();
				colorGroups.Add(g);
			}
		}

		void makeQuads(WallCompiled wall, MeshBuilder<VertexTNP3> builder, int a, int b)
		{
			Quad<VertexTNP3> quad;
			var bottom = wall.Thickness * bottomLength;
			var mid = wall.Thickness * midLength;
			var top = wall.Thickness * topLength;		
			
			Vector3 v1, v2, v3, v4, normal;
			Vector2 t1, t2, t3, t4;			

			t1 = new Vector2(.5f, wall.TexesBegin1[a]);
			v1 = new Vector3(wall.Verts1[a].X, wall.Verts1[a].Y, bottom); 
			t2 = new Vector2(.5f, wall.TexesEnd1[b]);
			v2 = new Vector3(wall.Verts1[b].X, wall.Verts1[b].Y, bottom);
			t3 = new Vector2(1f, wall.TexesBegin1[a]);
			v3 = new Vector3(wall.Verts1[a].X, wall.Verts1[a].Y, mid); 
			t4 = new Vector2(1f, wall.TexesEnd1[b]);
			v4 = new Vector3(wall.Verts1[b].X, wall.Verts1[b].Y, mid);
			
			normal = Vector3.Cross(v3 - v1, v4 - v1);
			normal.Normalize();
			
			quad.A.Normal = normal;
			quad.A.Position = v2;
			quad.A.TexCoord = t2;
			quad.B.Normal = normal;
			quad.B.Position = v1;
			quad.B.TexCoord = t1;				
			quad.C.Normal = normal;
			quad.C.Position = v3;
			quad.C.TexCoord = t3;
			quad.D.Normal = normal;
			quad.D.Position = v4;
			quad.D.TexCoord = t4;
			builder.Add(quad);
			
			t1 = new Vector2(.5f, wall.TexesBegin4[a]);
			v1 = new Vector3(wall.Verts4[a].X, wall.Verts4[a].Y, bottom);
			t2 = new Vector2(.5f, wall.TexesEnd4[b]);
			v2 = new Vector3(wall.Verts4[b].X, wall.Verts4[b].Y, bottom);
			t3 = new Vector2(1f, wall.TexesBegin4[a]);
			v3 = new Vector3(wall.Verts4[a].X, wall.Verts4[a].Y, mid); 
			t4 = new Vector2(1f, wall.TexesEnd4[b]);
			v4 = new Vector3(wall.Verts4[b].X, wall.Verts4[b].Y, mid);
			
			normal = Vector3.Cross(v1 - v3, v4 - v3);
			normal.Normalize();
			
			quad.A.Normal = normal;
			quad.A.Position = v1;
			quad.A.TexCoord = t1;
			quad.B.Normal = normal;
			quad.B.Position = v2;
			quad.B.TexCoord = t2;				
			quad.C.Normal = normal;
			quad.C.Position = v4;
			quad.C.TexCoord = t4;
			quad.D.Normal = normal;
			quad.D.Position = v3;
			quad.D.TexCoord = t3;
			builder.Add(quad);
			
			t1 = new Vector2(.5f, wall.TexesBegin2[a]);
			v1 = new Vector3(wall.Verts2[a].X, wall.Verts2[a].Y, top); 
			t2 = new Vector2(.5f, wall.TexesEnd2[b]);
			v2 = new Vector3(wall.Verts2[b].X, wall.Verts2[b].Y, top);
			t3 = new Vector2(1f, wall.TexesBegin3[a]);
			v3 = new Vector3(wall.Verts3[a].X, wall.Verts3[a].Y, top);
			t4 = new Vector2(1f, wall.TexesEnd3[b]);
			v4 = new Vector3(wall.Verts3[b].X, wall.Verts3[b].Y, top);
			
			normal = Vector3.Cross(v3 - v1, v4 - v1);
			normal.Normalize();
			
			quad.A.Normal = normal;
			quad.A.Position = v2;
			quad.A.TexCoord = t2;
			quad.B.Normal = normal;
			quad.B.Position = v1;
			quad.B.TexCoord = t1;				
			quad.C.Normal = normal;
			quad.C.Position = v3;
			quad.C.TexCoord = t3;
			quad.D.Normal = normal;
			quad.D.Position = v4;
			quad.D.TexCoord = t4;
			builder.Add(quad);
			
			t1 = new Vector2(0f, wall.TexesBegin1[a]);
			v1 = new Vector3(wall.Verts1[a].X, wall.Verts1[a].Y, mid);
			t2 = new Vector2(0f, wall.TexesEnd1[b]);
			v2 = new Vector3(wall.Verts1[b].X, wall.Verts1[b].Y, mid);
			t3 = new Vector2(.5f, wall.TexesBegin2[a]);
			v3 = new Vector3(wall.Verts2[a].X, wall.Verts2[a].Y, top);
			t4 = new Vector2(.5f, wall.TexesEnd2[b]);
			v4 = new Vector3(wall.Verts2[b].X, wall.Verts2[b].Y, top);
			
			normal = Vector3.Cross(v3 - v1, v4 - v1);
			normal.Normalize();
			
			quad.A.Normal = normal;
			quad.A.Position = v2;
			quad.A.TexCoord = t2;
			quad.B.Normal = normal;
			quad.B.Position = v1;
			quad.B.TexCoord = t1;				
			quad.C.Normal = normal;
			quad.C.Position = v3;
			quad.C.TexCoord = t3;
			quad.D.Normal = normal;
			quad.D.Position = v4;
			quad.D.TexCoord = t4;
			builder.Add(quad);	
			
			t1 = new Vector2(0f, wall.TexesBegin4[a]);
			v1 = new Vector3(wall.Verts4[a].X, wall.Verts4[a].Y, mid);
			t2 = new Vector2(0f, wall.TexesEnd4[b]);
			v2 = new Vector3(wall.Verts4[b].X, wall.Verts4[b].Y, mid);
			t3 = new Vector2(.5f, wall.TexesBegin3[a]);
			v3 = new Vector3(wall.Verts3[a].X, wall.Verts3[a].Y, top); 
			t4 = new Vector2(.5f, wall.TexesEnd3[b]);
			v4 = new Vector3(wall.Verts3[b].X, wall.Verts3[b].Y, top);
			
			normal = Vector3.Cross(v1 - v3, v4 - v3);
			normal.Normalize();
			
			quad.A.Normal = normal;
			quad.A.Position = v1;
			quad.A.TexCoord = t1;
			quad.B.Normal = normal;
			quad.B.Position = v2;
			quad.B.TexCoord = t2;				
			quad.C.Normal = normal;
			quad.C.Position = v4;
			quad.C.TexCoord = t4;
			quad.D.Normal = normal;
			quad.D.Position = v3;
			quad.D.TexCoord = t3;
			builder.Add(quad);
		}

		public void Render()
		{
			material.Setup();
			texture.Bind();
			
			foreach (var colorGroup in colorGroups) {
				Programs.Material.TeamColor = colorGroup.Color;
				colorGroup.Mesh.Render();
			}
		}
	}
}