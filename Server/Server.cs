using System;
using System.Net;
using System.Threading;

namespace Server
{
	public static class Server
	{
		public static void Main()
		{
			var listener = new LobbyListener(IPAddress.Any, 8001);
			Thread.Sleep(Timeout.Infinite);
		}
	}
}

