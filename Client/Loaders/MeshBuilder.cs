using System;
using System.Collections.Generic;
using Shared;

namespace Client
{	
	public class MeshBuilder<T> where T : struct, IVertex
	{
		List<T> triVerts;
		List<int> idxs;
		Dictionary<T, int> dict;
				
		public MeshBuilder ()
		{
			triVerts = new List<T>();
			idxs = new List<int>();
			dict = new Dictionary<T, int>();
		}
		
		public void Add(T vertex)
		{
			int i;
			if (!dict.TryGetValue(vertex, out i)) {
				i = triVerts.Count;
				triVerts.Add(vertex);
				dict.Add(vertex, i);
			}
			idxs.Add(i);
			
		}
		
		public void Add(Triangle<T> tri)
		{
			Add(tri.A);
			Add(tri.B);
			Add(tri.C);
		}
		
		public void Add(Quad<T> quad)
		{
			Add(quad.A);
			Add(quad.B);
			Add(quad.C);
			Add(quad.C);
			Add(quad.D);
			Add(quad.A);
		}
		
		public Mesh<T> ToMesh()
		{
			return new Mesh<T>(idxs.ToArray(), triVerts.ToArray());
		}
		
		public bool IsEmpty {
			get {
				if (idxs.Count == 0) return true;
				else return false;
			}
		}
	}
}

