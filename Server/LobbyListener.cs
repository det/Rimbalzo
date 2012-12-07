using System;
using System.Net;
using System.Net.Sockets;
using Shared;

namespace Server
{
	public class LobbyListener : Listener
	{
		public LobbyListener(IPAddress address, int port) : base(address, port)
		{
		}
		
		public override void Accepted(TcpClient client)
		{
			var c = new LobbyConnection(client);
			Console.Write("Accepted!\n");
		}
		
		public override void AcceptFailed(string message)
		{
			Console.Write("AcceptFailed!");
		}
	}
}

