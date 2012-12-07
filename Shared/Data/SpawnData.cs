using System;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using System.Drawing;
using Shared;

namespace Shared
{
	public struct SpawnData
	{
		[XmlIgnore]
		public Color Color;
		[XmlAttribute("Color")]
		public string ColorString {
			get { return Color.Name; }			
			set { Color = System.Drawing.Color.FromName(value); }
		}

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

