using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Toybox.tiled {
	public class TiledObject {

		public string Name;
		public Point Position;
		public Point Size;

		public Rectangle Bounds { get { return new Rectangle(Position, Size); } }

		public TiledObject(XmlNode data) {
			Name = data.Attributes["name"].Value;
			Position = new Point(int.Parse(data.Attributes["x"].Value), int.Parse(data.Attributes["y"].Value));
			Size = new Point(int.Parse(data.Attributes["width"].Value), int.Parse(data.Attributes["height"].Value));
		}

	}
}
