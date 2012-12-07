using System;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using System.Drawing;
using Shared;

namespace Shared
{
	public struct BallData
	{
		[XmlIgnore]
		public Vector2 Position;
		[XmlAttribute("Position")]
		public string PositionString {
			get {
				return string.Format("{0},{1}", Position.X, Position.Y);
			}
			
			set {
				var parts = value.Split(',');
				Position.X = Parse.Float(parts[0]);
				Position.Y = Parse.Float(parts[1]);
			}
		}
	}
}

