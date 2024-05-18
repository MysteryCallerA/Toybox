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
		//public int Columns;

		public TiledTileset(XmlNode data, string workingDir, string contentRoot) {
			var filename = data.Attributes["source"].Value;
			FirstGid = uint.Parse(data.Attributes["firstgid"].Value);

			filename = Path.Combine(workingDir, filename);
			workingDir = Path.GetDirectoryName(filename);

			if (!File.Exists(filename)) {
				throw new Exception("File not found. Path:" + filename);
			}

			string xml = File.ReadAllText(filename);
			if (filename.EndsWith(".tsx")) {
				ParseXml(xml, workingDir, contentRoot);
				return;
			}

			throw new Exception("Unsupported file format");
		}

		private void ParseXml(string xml, string workingDir, string contentRoot) {
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xml);
			var data = doc.SelectSingleNode("tileset");

			CellWidth = int.Parse(data.Attributes["tilewidth"].Value);
			CellHeight = int.Parse(data.Attributes["tileheight"].Value);
			//Columns = int.Parse(data.Attributes["columns"].Value);

			Source = Path.Combine(workingDir, data.SelectSingleNode("image").Attributes["source"].Value);
			Source = Path.GetFullPath(Source);
			Source = Path.GetRelativePath(contentRoot, Source);
			Source = Source.Substring(0, Source.Length - Path.GetExtension(Source).Length);
		}

	}
}
