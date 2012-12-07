using System;
using OpenTK.Graphics.OpenGL;
using Shared;

namespace Client
{
	public class Mesh<T> where T : struct, IVertex
	{
		public int[] Idxs;
		public T[] Verts;

		public Mesh (int[] idxs, T[] verts)			
		{
			Idxs = idxs;
			Verts = verts;
		}
		
		public CompiledMesh<T> Compile()
		{
			return new CompiledMesh<T>(this);
		}
	}
}

