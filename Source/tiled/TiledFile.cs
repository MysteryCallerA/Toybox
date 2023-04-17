using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Toybox.maps;
using Utils.graphic;

namespace Toybox.tiled {

	public class TiledFile {

		private Dictionary<string, TiledTileLayer> TileLayers = new Dictionary<string, TiledTileLayer>();
		private Dictionary<string, TiledObjectLayer> ObjectLayers = new Dictionary<string, TiledObjectLayer>();
		private List<TiledTileset> Tilesets = new List<TiledTileset>();

		public TiledFile(string contentRoot, string file) { //NEXT this just needs testing now!!!
			file = contentRoot + "\\" + file;

			if (!File.Exists(file)) {
				throw new Exception("File not found. Path:" + file);
			}

			string xml = File.ReadAllText(file);
			if (file.EndsWith(".tmx")) {
				ParseXml(xml);
				return;
			}

			throw new Exception("Unsupported file format");
		}

		private void ParseXml(string xml) {
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xml);
			var map = doc.SelectSingleNode("map");

			foreach (XmlNode node in map.ChildNodes) {
				if (node.Name == "layer") {
					var layer = new TiledTileLayer(node);
					TileLayers.Add(layer.Name, layer);
				} else if (node.Name == "objectgroup") {
					var layer = new TiledObjectLayer(node);
					ObjectLayers.Add(layer.Name, layer);
				} else if (node.Name == "tileset") {
					var tileset = new TiledTileset(node);
					Tilesets.Add(tileset);
				}
			}
		}
		

		public Tilemap GetTilemap(string layerName, TextureGrid t) {
			return TileLayers[layerName].GetTilemap(Tilesets, t);
		}

	}
}
