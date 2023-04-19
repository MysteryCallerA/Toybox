using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Toybox.maps.tiles;
using Utils.graphic;

namespace Toybox.tiled
{

    internal class TiledTileLayer {

		private const uint FlagHFlip = 0b10000000000000000000000000000000;
		private const uint FlagVFlip = 0b01000000000000000000000000000000;
		private const uint FlagDFlip = 0b00100000000000000000000000000000;
		private const uint FlagExtra = 0b00010000000000000000000000000000;
		private const uint AllFlags = 0b11110000000000000000000000000000;

		public string Name;
		public int Width;
		public int Height;
		private List<uint> Tiles = new List<uint>(); //TODO these can converted to arrays
		private List<uint> Data = new List<uint>();

		public TiledTileLayer(XmlNode data) {
			Name = data.Attributes["name"].Value;
			Width = int.Parse(data.Attributes["width"].Value);
			Height = int.Parse(data.Attributes["height"].Value);

			string[] lines = data.SelectSingleNode("data").InnerText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < lines.Length; i++) {
				string[] line = lines[i].Split(',', StringSplitOptions.RemoveEmptyEntries);
				for (int tile = 0; tile < line.Length; tile++) {
					uint value = uint.Parse(line[tile]);
					Data.Add(value);
					Tiles.Add(value & ~AllFlags);
				}
			}
		}

		public Tilemap GetTilemap(List<TiledTileset> tilesets, TextureGrid texture) {
			var tileset = FindMatchingTileset(tilesets);

			var data = new List<List<Tilemap.Tile>>();
			for (int i = 0; i < Width; i++) {
				data.Add(new List<Tilemap.Tile>());
			}

			int x = 0;
			for (int i = 0; i < Tiles.Count; i++) {
				int tiley = Math.DivRem((int)(Tiles[i] - tileset.FirstGid), texture.Columns, out int tilex);
				var effect = SpriteEffects.None;
				if ((Data[i] & FlagHFlip) != 0) effect |= SpriteEffects.FlipHorizontally;
				if ((Data[i] & FlagVFlip) != 0) effect |= SpriteEffects.FlipVertically;
				data[x].Add(new Tilemap.Tile(new Point(tilex, tiley), effect));

				x++;
				if (x >= Width) x = 0;
			}

			return new Tilemap(texture, data);
		}

		private TiledTileset FindMatchingTileset(List<TiledTileset> tilesets) {
			uint sample = 0;
			for (int i = 0; i < Tiles.Count; i++) {
				if (Tiles[i] > 0) {
					sample = Tiles[i];
					break;
				}
			}

			for (int i = 1; i < tilesets.Count; i++) {
				if (sample < tilesets[i].FirstGid) {
					return tilesets[i - 1];
				}
			}
			return tilesets.Last();
		}

	}
}
