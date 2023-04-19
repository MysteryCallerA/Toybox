using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.maps.tiles {
	
	public static class TilemapCollisions {

		public class TileData {
			public Rectangle Bounds;
			public Tilemap.Tile Tile;
		}

		public static IEnumerable<TileData> GetCollisions(this Tilemap t, Rectangle r) {
			var topleft = t.PixelToMap(r.Left, r.Top);
			var botright = t.PixelToMap(r.Right, r.Bottom);

			for (int x = topleft.X; x <= botright.X; x++) {
				for (int y = topleft.Y; y <= botright.Y; y++) {
					var tile = t.Get(x, y);
					if (tile.HasValue) {
						yield return new TileData() { Tile = tile.Value, Bounds = new Rectangle(t.MapToPixel(x, y), t.TileSize) };
					}
				}
			}
		}
	
	}
}
