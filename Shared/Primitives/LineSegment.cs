using System;
using System.Runtime.InteropServices;

namespace Shared
{
	[StructLayout(LayoutKind.Sequential)]
	public struct LineSegment<T>
	{
		public T A;
		public T B;
	}
}