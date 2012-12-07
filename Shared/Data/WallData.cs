using System;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using System.Drawing;
using Shared;

namespace Shared
{
	public struct WallData
	{
		[XmlAttribute]
		public ShapeData Shape;

		[XmlAttribute]
		public float Thickness;

		[XmlIgnore]
		public Color Color;
		[XmlAttribute("Color")]
		public string ColorString {
			get { return Color.Name; }			
			set { Color = System.Drawing.Color.FromName(value); }
		}

		[XmlIgnore]
		public Vector2[] Verts;
		[XmlAttribute("Points")]
		public string VertsString {
			get {
				StringBuilder builder = new StringBuilder();
				if (Verts.Length == 0) { return ""; }
				else { builder.AppendFormat("{0},{1}", Verts[0].X, Verts[0].Y); }
				
				for (int i = 1; i < Verts.Length; i++) {
					builder.AppendFormat(" {0},{1}", Verts[i].X, Verts[i].Y);
				}
				return builder.ToString();
			}
			
			set {
				var verts = value.Split(' ');
				Verts = new Vector2[verts.Length];
				for (int i = 0; i < Verts.Length; i++) {
					var parts = verts[i].Split(',');
					Verts[i].X = Parse.Float(parts[0]);
					Verts[i].Y = Parse.Float(parts[1]);
				}
			}
		}
	}
}

