using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Toybox.maps;
using Utils.graphic;

namespace Toybox {

	public class TiledFile {

		public List<TileLayer> TileLayers = new List<TileLayer>();
		public List<ObjectLayer> ObjectLayers = new List<ObjectLayer>();
		public List<Tileset> Tilesets = new List<Tileset>();

		public class TileLayer {
			public string Name;
			public int LayerPos;
			public int Width;
			public int Height;
			public List<List<int>> Tiles;
		}

		public class ObjectLayer {
			public string Name;
			public int LayerPos;
			public List<TiledObject> Objects;
		}

		public class TiledObject {
			public string Name;
			public Point Position;
		}

		public class Tileset {
			public string Name;
			public int FirstGid;
		}

		public TiledFile(string path) {
			if (!File.Exists(path)) {
				throw new Exception("File not found. Path:" + path);
			}

			string xml = File.ReadAllText(path);
			if (path.EndsWith(".tmx")) {
				ParseXml(xml);
				return;
			}

			throw new Exception("Unsupported file format");
		}

		private void ParseXml(string xml) {
			try {
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(xml);
				var map = doc.SelectSingleNode("map");
				int layerPos = 0;

				foreach (XmlNode node in map.ChildNodes) {
					if (node.Name == "layer") {
						ParseTileLayer(node, layerPos);
						layerPos++;
					} else if (node.Name == "objectgroup") {
						ParseObjectLayer(node, layerPos);
						layerPos++;
					} else if (node.Name == "tileset") {
						ParseTileset(node);
					}
				}
			} catch (Exception e) {
				throw new Exception("Error occurred while parsing Tiled file:", e);
			}
		}

		//NEXT add a way of converting these semi-loaded layer objects into useful objects like Tilemap

		private void ParseTileLayer(XmlNode node, int layerPos) {
			TileLayer layer = new TileLayer();
			layer.Name = node.Attributes["name"].Value;
			layer.LayerPos = layerPos;
			layer.Width = int.Parse(node.Attributes["width"].Value);
			layer.Height = int.Parse(node.Attributes["height"].Value);

			string[] lines = node.SelectSingleNode("data").InnerText.Split(Environment.NewLine);
			var layerData = new List<List<int>>();
			for (int i = 0; i < lines.Length; i++) {
				string[] line = lines[i].Split(',');
				var lineData = new List<int>();
				for (int tile = 0; tile < line.Length; tile++) {
					lineData.Add(int.Parse(line[tile]));
				}
				layerData.Add(lineData);
			}
			layer.Tiles = layerData;

			TileLayers.Add(layer);
		}

		private void ParseObjectLayer(XmlNode node, int layerPos) {
			ObjectLayer layer = new ObjectLayer();
			layer.Name = node.Attributes["name"].Value;
			layer.LayerPos = layerPos;
			foreach (XmlNode data in node.ChildNodes) { //TODO not sure how this will work with other kinds of objects, this might only work with points
				TiledObject o = new TiledObject();
				o.Name = data.Attributes["name"].Value;
				o.Position = new Point(int.Parse(data.Attributes["x"].Value), int.Parse(data.Attributes["y"].Value));
				layer.Objects.Add(o);
			}

			ObjectLayers.Add(layer);
		}

		private void ParseTileset(XmlNode node) {
			var tileset = new Tileset();
			tileset.Name = node.Attributes["source"].Value;
			tileset.Name = tileset.Name.Substring(0, tileset.Name.Length - 4);
			tileset.FirstGid = int.Parse(node.Attributes["firstgid"].Value);
			Tilesets.Add(tileset);
		}

		public TileLayer FindTileLayer(string layerName) {
			TileLayer layer = null;
			for (int i = 0; i < TileLayers.Count; i++) {
				if (TileLayers[i].Name == layerName) {
					layer = TileLayers[i];
					break;
				}
			}
			return layer;
		}

		private ObjectLayer FindObjectLayer(string layerName) {
			ObjectLayer layer = null;
			for (int i = 0; i < ObjectLayers.Count; i++) {
				if (ObjectLayers[i].Name == layerName) {
					layer = ObjectLayers[i];
					break;
				}
			}
			return layer;
		}

		private Tileset FindTileset(string name) {
			Tileset tileset = null;
			for (int i = 0; i < Tilesets.Count; i++) {
				if (Tilesets[i].Name == name) {
					tileset = Tilesets[i];
					break;
				}
			}
			return tileset;
		}

		public Tilemap GetTilemap(string layerName, string tilesetName, TextureGrid t) {
			var data = new List<List<Tilemap.Tile>>();
			Tileset tileset = null;
			TileLayer layer = null;
			for (int i = 0; i < Tilesets.Count; i++) {
				if (Tilesets[i].Name == tilesetName) {
					tileset = Tilesets[i];
					break;
				}
			}
			if (tileset == null) throw new Exception(tilesetName + " does not exist");
			for (int i = 0; i < TileLayers.Count; i++) {
				if (TileLayers[i].Name == layerName) {
					layer = TileLayers[i];
					break;
				}
			}
			if (layer == null) throw new Exception(layerName + " does not exist");



			return new Tilemap(t, data);
		}
	
	}
}
