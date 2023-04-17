using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Toybox.tiled {

	internal class TiledTileset {

		public string Source;
		public uint FirstGid;

		public TiledTileset(XmlNode data) {
			Source = data.Attributes["source"].Value;
			FirstGid = uint.Parse(data.Attributes["firstgid"].Value);
		}

	}
}
