using System;
using System.IO;

namespace Shared
{
	public enum ClientMessage
	{
		Login,
		CreateGame,
		JoinGame,	
	}
	
	public struct LoginMessage {
		public string Username;
		public string Password;
		
		public void Marshal(BinaryWriter writer)
		{
			writer.Write(Username);
			writer.Write(Password);			
		}
		
		public LoginMessage(BinaryReader reader)
		{
			Username = reader.ReadString();
			Password = reader.ReadString();
		}
	}
	
	public struct CreateGameMessage {
		public string Title;
		public string Map;
		
		public void Marshal(BinaryWriter writer)
		{
			writer.Write(Title);
			writer.Write(Map);			
		}
		
		public CreateGameMessage(BinaryReader reader)
		{
			Title = reader.ReadString();
			Map = reader.ReadString();
		}
	}
	
	public struct JoinGameMessage {
		public string Host;

		public void Marshal(BinaryWriter writer)
		{
			writer.Write(Host);
		}
		
		public JoinGameMessage(BinaryReader reader)
		{
			Host = reader.ReadString();
		}
	}

}

