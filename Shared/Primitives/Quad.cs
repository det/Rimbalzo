using System;
using System.Runtime.InteropServices;

namespace Shared
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Quad<T>
	{
		public T A;
		public T B;
		public T C;
		public T D;
	}
}

