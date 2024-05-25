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

		internal List<TiledTileset> Tilesets = new List<TiledTileset>();
		public Color BackColor { get; private set; }
		public Point TileSize { get; protected set; }

		public TiledFile(string contentRoot, string file) {
			file = Path.Join(contentRoot, file);
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

			int tilewidth = int.Parse(map.Attributes["tilewidth"].Value);
			int tileheight = int.Parse(map.Attributes["tileheight"].Value);
			TileSize = new Point(tilewidth, tileheight);

			var color = map.Attributes["backgroundcolor"].Value;
			var trans = System.Drawing.ColorTranslator.FromHtml(color);
			BackColor = new Color(trans.R, trans.G, trans.B);

			ParseNodes(map.ChildNodes, workingDir, contentRoot, this);
		}

		/// <summary> Adds new tileset to Tilesets while keeping list sorted by FirstGid. </summary>
		internal void AddTileset(TiledTileset tileset) {
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

	}
}
