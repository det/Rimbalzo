using System;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using System.Drawing;
using Shared;

namespace Shared
{
	public struct GoalieData
	{
		[XmlIgnore]
		public Vector2 Begin;
		[XmlAttribute("Begin")]
		public string BeginString {
			get {
				return string.Format("{0},{1}", Begin.X, Begin.Y);
			}
			
			set {
				var parts = value.Split(',');
				Begin.X = Parse.Float(parts[0]);
				Begin.Y = Parse.Float(parts[1]);
			}
		}
		
		[XmlIgnore]
		public Vector2 End;
		[XmlAttribute("End")]
		public string EndString {
			get {
				return string.Format("{0},{1}", End.X, End.Y);
			}
			
			set {
				var parts = value.Split(',');
				End.X = Parse.Float(parts[0]);
				End.Y = Parse.Float(parts[1]);
			}
		}

	}
}

