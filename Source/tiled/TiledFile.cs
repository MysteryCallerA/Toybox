using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;
using Toybox.maps.tiles;

namespace Toybox.tiled
{

    public class TiledFile:TiledDataGroup {

		public TiledFile(string contentRoot, string file) {
			file = contentRoot + "\\" + file;
			var workingDir = Path.GetDirectoryName(file);

			if (!File.Exists(file)) {
				throw new Exception("File not found. Path:" + file);
			}

			string xml = File.ReadAllText(file);
			if (file.EndsWith(".tmx")) {
				ParseXml(xml, workingDir, contentRoot);
				return;
			}

			throw new Exception("Unsupported file format");
		}

		private void ParseXml(string xml, string workingDir, string contentRoot) {
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xml);
			var map = doc.SelectSingleNode("map");

			ParseNodes(map.ChildNodes, workingDir, contentRoot);
		}

	}
}
