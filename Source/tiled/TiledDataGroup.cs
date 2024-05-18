using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Toybox.maps.tiles;

namespace Toybox.tiled {
	public class TiledDataGroup {

		public string GroupName = "";
		public List<TiledDataGroup> DataGroups = new List<TiledDataGroup>();
		internal Dictionary<string, TiledTileLayer> TileLayers = new Dictionary<string, TiledTileLayer>();
		internal Dictionary<string, TiledObjectLayer> ObjectLayers = new Dictionary<string, TiledObjectLayer>();
		internal List<TiledTileset> Tilesets = new List<TiledTileset>();
		public Point TileSize { get; protected set; }

		public TiledDataGroup() {
		}

		protected void ParseNodes(XmlNodeList nodes, string workingDir, string contentRoot) {
			foreach (XmlNode n in nodes) {
				if (n.Name == "layer") {
					var layer = new TiledTileLayer(n);
					TileLayers.Add(layer.Name, layer);
				} else if (n.Name == "objectgroup") {
					var layer = new TiledObjectLayer(n);
					ObjectLayers.Add(layer.Name, layer);
				} else if (n.Name == "tileset") {
					AddTileset(new TiledTileset(n, workingDir, contentRoot));
				} else if (n.Name == "group") {
					var group = new TiledDataGroup { GroupName = n.Attributes["name"].Value };
					group.Tilesets = Tilesets;
					group.ParseNodes(n.ChildNodes, workingDir, contentRoot);
					group.TileSize = TileSize;
					DataGroups.Add(group);
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

		public bool TryGetTilemap(string layerName, out Tilemap t) {
			var output = TileLayers.TryGetValue(layerName, out var layer);
			if (output) {
				t = layer.GetTilemap(Tilesets);
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

	}


}
