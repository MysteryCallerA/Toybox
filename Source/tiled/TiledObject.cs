using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Toybox.tiled {
	internal class TiledObject {

		public string Name;
		public Point Position;

		public TiledObject(XmlNode data) {
			Name = data.Attributes["name"].Value;
			Position = new Point(int.Parse(data.Attributes["x"].Value), int.Parse(data.Attributes["y"].Value));
		}

	}
}
