using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Toybox.maps;
using Toybox.maps.tiles;

namespace Toybox.tiled {
	public class TiledDataGroup {

		public string GroupName = "";
		public List<TiledDataGroup> DataGroups = new List<TiledDataGroup>();
		internal Dictionary<string, TiledTileLayer> TileLayers = new Dictionary<string, TiledTileLayer>();
		internal Dictionary<string, TiledObjectLayer> ObjectLayers = new Dictionary<string, TiledObjectLayer>();
		public Dictionary<string, string> Properties = new Dictionary<string, string>();
		private TiledFile File;

		public TiledDataGroup() {
		}

		protected void ParseNodes(XmlNodeList nodes, string workingDir, string contentRoot, TiledFile file) {
			File = file;
			foreach (XmlNode n in nodes) {
				if (n.Name == "layer") {
					var layer = new TiledTileLayer(n, file.TileSize);
					TileLayers.Add(layer.Name, layer);
				} else if (n.Name == "objectgroup") {
					var layer = new TiledObjectLayer(n);
					ObjectLayers.Add(layer.Name, layer);
				} else if (n.Name == "tileset") {
					File.AddTileset(new TiledTileset(n, workingDir, contentRoot));
				} else if (n.Name == "group") {
					var group = new TiledDataGroup() { GroupName = n.Attributes["name"].Value };
					group.ParseNodes(n.ChildNodes, workingDir, contentRoot, file);
					DataGroups.Add(group);
				} else if (n.Name == "properties") {
					ParseProps(n);
				}
			}
		}

		private void ParseProps(XmlNode n) {
			var nodes = n.ChildNodes;
			foreach (XmlNode prop in nodes) {
				Properties.Add(prop.Attributes["name"].Value, prop.Attributes["value"].Value);
			}
		}

		public bool TryGetTilemap(string layerName, out Tilemap t) {
			var output = TileLayers.TryGetValue(layerName, out var layer);
			if (output) {
				t = layer.GetTilemap(File.Tilesets);
			} else t = null;
			return output;
		}

		public bool TryGetObjects(string layerName, out List<TiledObject> objects) {
			if (!ObjectLayers.ContainsKey(layerName)) {
				objects = null;
				return false;
			}
			objects = ObjectLayers[layerName].Content;
			return true;
		}

		public bool TryGetZoneMap(string layerName, out ZoneMap z) {
			z = new ZoneMap();
			if (!ObjectLayers.ContainsKey(layerName)) {
				return false;
			}
			var layer = ObjectLayers[layerName].Content;
			foreach (var zone in layer) {
				z.Add(zone.Name, zone.Bounds);
			}
			return true;
		}

	}


}
