using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Toybox.tiled {

	internal class TiledTileset {

		public string Source;
		public uint FirstGid; //NOTE this is different in each tilemap
		public int CellWidth;
		public int CellHeight;
		public int Columns;

		public TiledTileset(XmlNode data, string contentRoot) {
			var filename = data.Attributes["source"].Value;
			FirstGid = uint.Parse(data.Attributes["firstgid"].Value);

			filename = contentRoot + "\\" + filename;

			if (!File.Exists(filename)) {
				throw new Exception("File not found. Path:" + filename);
			}

			string xml = File.ReadAllText(filename);
			if (filename.EndsWith(".tsx")) {
				ParseXml(xml);
				return;
			}

			throw new Exception("Unsupported file format");
		}

		private void ParseXml(string xml) {
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xml);
			var data = doc.SelectSingleNode("tileset");

			CellWidth = int.Parse(data.Attributes["tilewidth"].Value);
			CellHeight = int.Parse(data.Attributes["tileheight"].Value);
			Columns = int.Parse(data.Attributes["columns"].Value);
			Source = data.FirstChild.Attributes["source"].Value;
			Source = Source.Substring(0, Source.LastIndexOf('.'));
		}

	}
}
