using System;
using System.IO;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Shared;

namespace Client
{
	public struct Material {
		public Vector4 Ambient;
		public Vector4 Diffuse;
		public Vector4 Specular;
		public float Shininess;
		
		static public Material Default()
		{
			Material material;
			material.Ambient.X = .2f;
			material.Ambient.Y = .2f;
			material.Ambient.Z = .2f;
			material.Ambient.W = 1f;
			material.Diffuse.X = .8f;
			material.Diffuse.Y = .8f;
			material.Diffuse.Z = .8f;
			material.Diffuse.W = 1f;
			material.Specular.X = 0f;
			material.Specular.Y = 0f;
			material.Specular.Z = 0f;
			material.Specular.W = 1f;
			material.Shininess = 0f;
			return material;
		}

		public void Setup()
		{
			Programs.Material.MaterialAmbient = Ambient;
			Programs.Material.MaterialDiffuse = Diffuse;
			Programs.Material.MaterialSpecular = Specular;
			Programs.Material.MaterialShininess = Shininess;
		}
	}

	public class ObjMaterial
	{
		Dictionary<string, Material> defs;

		public ObjMaterial()
		{
			defs = new Dictionary<string, Material>();
		}
		
		public ObjMaterial(string path) : this()
		{
			Load(path);
		}

		public void Load(string path) {
			var reader = new StreamReader(path);			
			var material = new Material();
			string materialName = null;

			while (true) {
				var line = reader.ReadLine();				
				if (line == null) { break; }
				line = line.Trim();
				
				var parts = line.Split(' ');				

				switch (parts[0]) {
				case "newmtl":
					if (materialName != null) { defs.Add(materialName, material); }					
					materialName = parts[1];
					material = Material.Default();
					break;
					
				case "Ka":
					material.Ambient.X = Parse.Float(parts[1]);
					material.Ambient.Y = Parse.Float(parts[2]);
					material.Ambient.Z = Parse.Float(parts[3]);
					material.Ambient.W = 1f;
					break;
					
				case "Kd":
					material.Diffuse.X = Parse.Float(parts[1]);
					material.Diffuse.Y = Parse.Float(parts[2]);
					material.Diffuse.Z = Parse.Float(parts[3]);
					material.Diffuse.W = 1f;
					break;

				case "Ks":
					material.Specular.X = Parse.Float(parts[1]);
					material.Specular.Y = Parse.Float(parts[2]);
					material.Specular.Z = Parse.Float(parts[3]);
					material.Specular.W = 1f;
					break;
					
				case "Ns":
					material.Shininess = Parse.Float(parts[1]) / 1000f * 128f;
					break;					
				}				
			}	
			if (materialName != null) { defs.Add(materialName, material); }
		}

		public Material Lookup(string materialName)
		{
			return defs[materialName];
		}
	}		
}

