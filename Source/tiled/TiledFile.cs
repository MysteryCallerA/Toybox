﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;
using Toybox.maps.tiles;

namespace Toybox.tiled
{

    public class TiledFile {

		private Dictionary<string, TiledTileLayer> TileLayers = new Dictionary<string, TiledTileLayer>();
		private Dictionary<string, TiledObjectLayer> ObjectLayers = new Dictionary<string, TiledObjectLayer>();
		private List<TiledTileset> Tilesets = new List<TiledTileset>();
		private List<string> GroupNames = new List<string>(); //TODO this needs a better solution to handle multiple layers of groups, possibly a tree?
		//Actual data could be stored in a TiledGroup class and groups could contain groups
		//Then you could esily iterate through groups when loading

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

			foreach (XmlNode node in map.ChildNodes) {
				ParseNode(node, workingDir, contentRoot);
			}
		}

		private void ParseNode(XmlNode node, string workingDir, string contentRoot, string groupName = "") {
			if (node.Name == "layer") {
				var layer = new TiledTileLayer(node);
				TileLayers.Add(groupName + layer.Name, layer);
			} else if (node.Name == "objectgroup") {
				var layer = new TiledObjectLayer(node);
				ObjectLayers.Add(groupName + layer.Name, layer);
			} else if (node.Name == "tileset") {
				AddTileset(new TiledTileset(node, workingDir, contentRoot));
			} else if (node.Name == "group") {
				var name = node.Attributes["name"].Value;
				GroupNames.Add(name);
				foreach (XmlNode groupNode in node.ChildNodes) {
					ParseNode(groupNode, workingDir, contentRoot, groupName + name + ".");
				}
			}
		}

		/// <summary> Adds new tileset to Tilesets while keeping list sorted by FirstGid. </summary>
		private void AddTileset(TiledTileset tileset) {
			if (Tilesets.Count == 0 || tileset.FirstGid > Tilesets.Last().FirstGid) {
				Tilesets.Add(tileset);
			} else {
				for (int i = 0; i < Tilesets.Count; i++) {
					if (tileset.FirstGid < Tilesets[i].FirstGid) {
						Tilesets.Insert(i, tileset);
						return;
					}
				}
				Tilesets.Add(tileset);
			}
		}

		public bool TryGetTilemap(string layerName, out Tilemap t, string groupName = "") {
			if (groupName != "") layerName = groupName + "." + layerName;

			var output = TileLayers.TryGetValue(layerName, out var layer);
			if (output) {
				t = layer.GetTilemap(Tilesets);
			} else t = null;
			return output;
		}

		public bool TryGetObjects(string layerName, out List<TiledObject> objects, string groupName = "") {
			if (groupName != "") layerName = groupName + "." + layerName;

			if (!ObjectLayers.ContainsKey(layerName)) {
				objects = null;
				return false;
			}
			objects = ObjectLayers[layerName].Content;
			return true;
		}

		public ReadOnlyCollection<string> GetGroupNames() {
			return GroupNames.AsReadOnly();
		}

	}
}
