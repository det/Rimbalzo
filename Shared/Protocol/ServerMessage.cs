using System;
using System.IO;
using System.Collections.Generic;

namespace Shared
{
	public enum ServerMessage
	{
		LoginOk,
		LoginFail,
		CreateGameOk,
		JoinGameOk,
		JoinGameFail
	}
	
	public struct GameInfo {
		public string Title;
		public string[] Players;
		
		public void Marshal(BinaryWriter writer)
		{
			writer.Write(Title);
			writer.Write((uint) Players.Length);			
			foreach (var player in Players) {
				writer.Write(player);
			}
		}
		
		public GameInfo(BinaryReader reader)
		{
			Title = reader.ReadString();
			var count = reader.ReadUInt32();
			Players = new string[count];
			for (int i = 0; i < count; i++) {
				var player = reader.ReadString();
				Players[i] = player;
			}
		}
	}
	
	public struct GameList {
		public HashSet<GameInfo> Games;
		
		public void Marshal(BinaryWriter writer)
		{
			writer.Write((uint) Games.Count);			
			foreach (var game in Games) {
				game.Marshal(writer);
			}
		}
		
		public GameList(BinaryReader reader)
		{
			var count = reader.ReadUInt32();
			Games = new HashSet<GameInfo>();
			for (int i = 0; i < count; i++) {
				var game = new GameInfo(reader);
				Games.Add(game);
			}
		}
	}
}

