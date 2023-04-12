using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.graphic;

namespace Toybox.maps {
	public class EditTilemap:Tilemap {

		public EditTilemap(TextureGrid t) : base(t) {
		}

		public void Set(int x, int y, Tile? tile) {
			var mappos = PixelToMap(x, y);
			while (mappos.X < 0) {
				mappos.X++;
				X -= TileWidth;
				Map.Insert(0, new List<Tile>());
			}
			while (mappos.X >= Map.Count) {
				Map.Add(new List<Tile>());
			}
			while (mappos.Y < 0) {
				mappos.Y++;
				Y -= TileHeight;
				for (int col = 0; col < Map.Count; col++) {
					Map[col].Insert(0, new Tile());
				}
			}
			while (mappos.Y >= Map[mappos.X].Count) {
				Map[mappos.X].Add(new Tile());
			}

			if (!tile.HasValue) {
				Map[mappos.X][mappos.Y] = new Tile();
			} else {
				Map[mappos.X][mappos.Y] = tile.Value;
			}
		}
	}
}
