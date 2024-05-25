using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.graphic;
using static Toybox.maps.tiles.Tilemap;

namespace Toybox.maps.tiles {
	public static class TilemapEdit {

		public static void Set(this Tilemap t, int x, int y, Tile? tile) {
			var mappos = t.PixelToMap(x, y);
			while (mappos.X < 0) {
				mappos.X++;
				t.X -= t.TileWidth;
				t.Map.Insert(0, new List<Tile>());
			}
			while (mappos.X >= t.Map.Count) {
				t.Map.Add(new List<Tile>());
			}
			while (mappos.Y < 0) {
				mappos.Y++;
				t.Y -= t.TileHeight;
				for (int col = 0; col < t.Map.Count; col++) {
					t.Map[col].Insert(0, new Tile());
				}
			}
			while (mappos.Y >= t.Map[mappos.X].Count) {
				t.Map[mappos.X].Add(new Tile());
			}

			if (!tile.HasValue) {
				t.Map[mappos.X][mappos.Y] = new Tile();
			} else {
				t.Map[mappos.X][mappos.Y] = tile.Value;
			}
		}

		/// <summary> zone must be in Pixel space. </summary>
		public static void Set(this Tilemap t, Rectangle zone, Tile tile) {
			zone = Rectangle.Intersect(zone, t.Bounds);
			zone = t.PixelToMap(zone);
			for (int x = zone.X; x < zone.Right; x++) {
				for (int y = zone.Y; y < zone.Bottom; y++) {
					t.Map[x][y] = tile;
				}
			}
		}

		
	}
}
