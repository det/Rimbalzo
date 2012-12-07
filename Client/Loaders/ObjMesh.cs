using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Shared;

namespace Client
{
	struct MaterialGroup {
		public Material Material;
		public CompiledMesh<VertexTNP3> Mesh;		
	}

	public class ObjMesh
	{	
		List<MaterialGroup> materialGroups;

		public ObjMesh()
		{
			materialGroups = new List<MaterialGroup>();			
		}
		
		public ObjMesh(string path) : this()
		{
			Load(path);
		}

		public void Load(string path)
		{
			
			var reader = new StreamReader(path);

			var positions = new List<Vector3>();
			var norms = new List<Vector3>();
			var texes = new List<Vector2>();
			var objMaterial = new ObjMaterial();
			var builderMap = new Dictionary<Material, MeshBuilder<VertexTNP3>>();

			var material = Material.Default();
			var builder = new MeshBuilder<VertexTNP3>();			
			builderMap[material] = builder;
			
			while (true) {
				var line = reader.ReadLine();
				if (line == null) { break; }

				var parts = line.Split(' ');
				switch (parts[0]) {
					
				case "v":					
					var v1 = Parse.Float(parts[1]);
					var v2 = Parse.Float(parts[2]);
					var v3 = Parse.Float(parts[3]);
					var vert = new Vector3(v1, v2, v3);
					positions.Add(vert);
					break;
					
				case "vn":					
					var n1 = Parse.Float(parts[1]);
					var n2 = Parse.Float(parts[2]);
					var n3 = Parse.Float(parts[3]);
					var norm = new Vector3(n1, n2, n3);
					norms.Add(norm);
					break;

				case "vt":
					var t1 = Parse.Float(parts[1]);
					var t2 = Parse.Float(parts[2]);
					var tex = new Vector2(t1, t2);
					texes.Add(tex);
					break;

				case "f":
					var g1 = parts[1].Split('/');
					var g2 = parts[2].Split('/');
					var g3 = parts[3].Split('/');
					var tri = new Triangle<VertexTNP3>();

					if (g1[0] != "") tri.A.Position = positions[Parse.Int(g1[0]) - 1];
					if (g1[1] != "") tri.A.TexCoord = texes[Parse.Int(g1[1]) - 1];
					if (g1[2] != "") tri.A.Normal = norms[Parse.Int(g1[2]) - 1];
					if (g2[0] != "") tri.B.Position = positions[Parse.Int(g2[0]) - 1];
					if (g2[1] != "") tri.B.TexCoord = texes[Parse.Int(g2[1]) - 1];
					if (g2[2] != "") tri.B.Normal = norms[Parse.Int(g2[2]) - 1];
					if (g3[0] != "") tri.C.Position = positions[Parse.Int(g3[0]) - 1];
					if (g3[1] != "") tri.C.TexCoord = texes[Parse.Int(g3[1]) - 1];
					if (g3[2] != "") tri.C.Normal = norms[Parse.Int(g3[2]) - 1];
					builder.Add(tri);
					break;

				case "mtllib":
					var dirname = System.IO.Path.GetDirectoryName(path);
					var mtlPath = Path.Combine(dirname, parts[1]);
					objMaterial.Load(mtlPath);
					break;

				case "usemtl":
					material = objMaterial.Lookup(parts[1]);
					if (!builderMap.TryGetValue(material, out builder)) {
						builder = new MeshBuilder<VertexTNP3>();
						builderMap[material] = builder;						
					}
					break;
				}
			}
			
			foreach (var pair in builderMap) {
				if (!pair.Value.IsEmpty) {
					MaterialGroup mg;
					mg.Material = pair.Key;
					mg.Mesh = pair.Value.ToMesh().Compile();
					materialGroups.Add(mg);
				}
			}
		}
		
		public void Render()
		{			
			foreach (var materialGroup in materialGroups) {
				materialGroup.Material.Setup();
				materialGroup.Mesh.Render();
			}

		}
	}
}
