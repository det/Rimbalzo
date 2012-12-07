using System;
using System.Net;
using System.Net.Sockets;
using Shared;

namespace Server
{
	public class LobbyConnection: Connection
	{
		public LobbyConnection(TcpClient client) : base(client)
		{
		}
		
		public override void Disconnected(string message)
		{
			Console.Write("Disconnected!\n");
		}
		
		public override void Received()
		{
			Console.Write("Received!\n");
		}
	}
}

