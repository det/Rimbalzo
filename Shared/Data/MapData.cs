using System;
using System.Xml.Serialization;
using System.IO;

namespace Shared
{
	[XmlRoot("Map")]
	public class MapData
	{
		[XmlElement("Wall")]
		public WallData[] Walls;

		[XmlElement("Spawn")]
		public SpawnData[] Spawns;
		
		[XmlElement("Ball")]
		public BallData[] Balls;
	
		[XmlElement("Goal")]
		public GoalData[] Goals;

		[XmlElement("Goalie")]
		public GoalieData[] Goalies;
		
		[XmlElement("Speedup")]
		public SpeedupData[] Speedups;

		public void Save(string path)
		{
			var formatter = new XmlSerializer(typeof(MapData), new Type[] {typeof(System.Drawing.Color), typeof(Microsoft.Xna.Framework.Vector2)});
			var stream = File.Open(path, FileMode.Create);
			formatter.Serialize(stream, this);
			stream.Close();
		}
		
		public static MapData Load(string path)
		{
			var formatter = new XmlSerializer(typeof(MapData), new Type[] {typeof(System.Drawing.Color), typeof(Microsoft.Xna.Framework.Vector2)});
			var stream = File.OpenRead(path);
			var map = (MapData) formatter.Deserialize(stream);
			stream.Close();
			return map;
		}
		
		public MapCompiled Compile() {
			return new MapCompiled(this);
		}
	}
}

