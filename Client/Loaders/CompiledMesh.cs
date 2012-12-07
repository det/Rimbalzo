using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using Shared;

namespace Client
{
	public class CompiledMesh<T> where T : struct, IVertex
	{
		int vao;
		int vbo;
		int ebo;
		int numVerts;

		public CompiledMesh(Mesh<T> mesh)
		{
			numVerts = mesh.Idxs.Length;

			int sizeInBytes;

			GL.GenVertexArrays(1, out vao);
			GL.BindVertexArray(vao);

			sizeInBytes = Marshal.SizeOf(new T());
			GL.GenBuffers(1, out vbo);
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(sizeInBytes * mesh.Verts.Length), mesh.Verts, BufferUsageHint.StaticDraw);			

			sizeInBytes = Marshal.SizeOf(new int());
			GL.GenBuffers(1, out ebo);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
			GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(sizeInBytes * mesh.Idxs.Length), mesh.Idxs, BufferUsageHint.StaticDraw);			

			// Hack: C# doesn't allow static methods in interfaces
			new T().SetupAttributes();

			GL.BindVertexArray(0);
		}
		
		public void Render()
		{
			GL.BindVertexArray(vao);
			GL.DrawElements(BeginMode.Triangles, numVerts, DrawElementsType.UnsignedInt, 0);
			GL.BindVertexArray(0);
		}
	}
}