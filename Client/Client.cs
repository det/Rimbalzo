using System;
using Shared;

namespace Client
{
	public static class Rimbalzo
	{		
		public static void Main ()
		{
			var map = MapData.Load("Maps/test.xml").Compile();
			var window = new OfflineWindow(map);
			window.Run();
		}
	}
}
