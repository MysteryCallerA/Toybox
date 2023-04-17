using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Toybox.tiled {

	internal class TiledObjectLayer {

		public string Name;
		public Dictionary<string, TiledObject> Content = new Dictionary<string, TiledObject>();

		public TiledObjectLayer(XmlNode data) {
			Name = data.Attributes["name"].Value;

			foreach (XmlNode child in data.ChildNodes) {
				var o = new TiledObject(child);
				Content.Add(o.Name, o);
			}
		}

	}
}
