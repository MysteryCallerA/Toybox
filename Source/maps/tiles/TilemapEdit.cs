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

		public static Tilemap CreateBlankMap(TextureGrid texture, int tileWidth, int tileHeight, int columns, int rows) {
			var map = new Tilemap(texture, tileWidth, tileHeight);
			for (int i = 0; i < columns; i++) {
				map.Map.Add(CreateColumn(rows));
			}
			return map;
		}

		private static List<Tile> CreateColumn(int rows) {
			var col = new List<Tile>();
			for (int i = 0; i < rows; i++) {
				col.Add(new Tile());
			}
			return col;
		}

		public static void Set(this Tilemap t, int x, int y, Tile? tile) {
			var mappos = t.ScaledPixelToCell(x, y);
			if (!tile.HasValue && !t.InBounds(mappos.X, mappos.Y)) return;

			while (mappos.X < 0) {
				mappos.X++;
				t.X -= t.TileWidth;
				t.Map.Insert(0, CreateColumn(t.Rows));
			}
			while (mappos.X >= t.Map.Count) {
				t.Map.Add(CreateColumn(t.Rows));
			}
			while (mappos.Y < 0) {
				mappos.Y++;
				t.Y -= t.TileHeight;
				for (int col = 0; col < t.Map.Count; col++) {
					t.Map[col].Insert(0, new Tile());
				}
			}
			while (mappos.Y >= t.Map[0].Count) {
				for (int col = 0; col < t.Map.Count; col++) {
					t.Map[col].Add(new Tile());
				}
			}

			if (!tile.HasValue) {
				t.Map[mappos.X][mappos.Y] = new Tile();
			} else {
				t.Map[mappos.X][mappos.Y] = tile.Value;
			}
		}

		/// <summary> zone must be in ScaledPixel space. </summary>
		public static void Set(this Tilemap t, Rectangle zone, Tile tile) {
			zone = ToCellRect(t, Rectangle.Intersect(zone, t.Bounds));

			for (int x = zone.X; x < zone.Right; x++) {
				for (int y = zone.Y; y < zone.Bottom; y++) {
					t.Map[x][y] = tile;
				}
			}
		}

		public static void Fill(this Tilemap t, Tile tile) {
			for (int x = 0; x < t.Map.Count; x++) {
				for (int y = 0; y < t.Map[x].Count; y++) {
					t.Map[x][y] = tile;
				}
			}
		}

		/// <summary> Positions must be in ScaledPixel space. </summary>
		public static void FloodFill(this Tilemap t, Point startPos, Tile fillTile, Tilemap collisionMap, Tile[] blockingTiles) {
			var pos = t.ScaledPixelToCell(startPos.X, startPos.Y);
			var bounds = ToCellRect(t, t.Bounds);
			Queue.Enqueue(pos);
			Queue.Enqueue(new Point(pos.X, pos.Y - 1));
			Queue.Enqueue(new Point(pos.X, pos.Y + 1));
			Queue.Enqueue(new Point(pos.X - 1, pos.Y));
			Queue.Enqueue(new Point(pos.X + 1, pos.Y));

			while (Queue.Any()) {
				var p = Queue.Dequeue();
				if (!bounds.Contains(p)) continue;
				if (HashSet.Contains(p)) continue;
				var tile = collisionMap.Get(p.X, p.Y);
				if (tile.HasValue && blockingTiles.Contains(tile.Value)) continue;

				t.Map[p.X][p.Y] = fillTile;
				HashSet.Add(p);
				Queue.Enqueue(new Point(p.X, p.Y + 1));
				Queue.Enqueue(new Point(p.X, p.Y - 1));
				Queue.Enqueue(new Point(p.X + 1, p.Y));
				Queue.Enqueue(new Point(p.X - 1, p.Y));
			}

			Queue.Clear();
			HashSet.Clear();
		}

		public static void PerimeterFill(this Tilemap t, Tile touch, Tile fill, Rectangle bounds, bool doDiagonal) {
			bounds = ToCellRect(t, Rectangle.Intersect(bounds, t.Bounds));

			for (int x = bounds.X; x < bounds.Right; x++) {
				for (int y = bounds.Y; y < bounds.Bottom; y++) {
					if (t.GetId(x, y) != touch.Id) continue;
					HashSet.Add(new Point(x, y - 1));
					HashSet.Add(new Point(x, y + 1));
					HashSet.Add(new Point(x - 1, y));
					HashSet.Add(new Point(x + 1, y));
					if (doDiagonal) {
						HashSet.Add(new Point(x - 1, y - 1));
						HashSet.Add(new Point(x + 1, y - 1));
						HashSet.Add(new Point(x - 1, y + 1));
						HashSet.Add(new Point(x + 1, y + 1));
					}
				}
			}
			foreach (var p in HashSet) {
				if (t.GetId(p.X, p.Y) != touch.Id) t.Map[p.X][p.Y] = fill;
			}
			HashSet.Clear();
		}

		private static HashSet<Point> HashSet = new HashSet<Point>();
		private static Queue<Point> Queue = new Queue<Point>();

		private static Rectangle ToCellRect(Tilemap t, Rectangle scaledPixelRect) {
			var topleft = t.ScaledPixelToCell(scaledPixelRect.X, scaledPixelRect.Y);
			var botright = t.ScaledPixelToCell(scaledPixelRect.Right, scaledPixelRect.Bottom);
			return new Rectangle(topleft.X, topleft.Y, botright.X - topleft.X, botright.Y - topleft.Y);
		}

	}
}
