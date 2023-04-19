using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Utils.graphic;

namespace Toybox.maps.tiles {
	public class Tilemap {

		public int X;
		public int Y;
		public TextureGrid Tileset;

		protected internal List<List<Tile>> Map = new List<List<Tile>>();//x,y

		protected Tilemap(TextureGrid t) {
			Tileset = t;
		}

		public Tilemap(TextureGrid t, List<List<Tile>> data) {
			Tileset = t;
			Map = data;
		}

		public struct Tile {
			public Point Id;
			public SpriteEffects Effect;

			public Tile() {
				Id = new Point(-1, -1);
				Effect = SpriteEffects.None;
			}

			public Tile(Point id, SpriteEffects effect) {
				Id = id;
				Effect = effect;
			}

			public bool IsEmpty {
				get { return Id.X < 0; }
			}
		}

		public int TileWidth {
			get { return Tileset.CellWidth; }
		}

		public int TileHeight {
			get { return Tileset.CellHeight; }
		}

		public Point TileSize {
			get { return new Point(TileWidth, TileHeight); }
		}

		public void SetData(List<List<Tile>> data) {
			Map = data;
		}

		public void Draw(Renderer r, Camera c) {
			if (Map.Count == 0) return;
			var bounds = c.GetGameBounds();
			var topleft = PixelToMap(bounds.Left, bounds.Top);
			var botright = PixelToMap(bounds.Right, bounds.Bottom) + new Point(1, 1);

			if (topleft.X < 0) topleft.X = 0;
			if (topleft.Y < 0) topleft.Y = 0;

			var dest = new Rectangle(MapToPixel(topleft.X, topleft.Y), TileSize);
			int starty = dest.Y;

			for (int col = topleft.X; col < botright.X && col < Map.Count; col++) {
				for (int row = topleft.Y; row < botright.Y && row < Map[col].Count; row++) {
					var tile = Map[col][row];
					if (!tile.IsEmpty) {
						r.Draw(Tileset.Texture, dest, Tileset.GetCell(tile.Id), Color.White, c, Camera.Space.Pixel, tile.Effect);
					}
					dest.Y += dest.Height;
				}
				dest.Y = starty;
				dest.X += dest.Width;
			}
		}

		public Tile? Get(Point pixelPos) {
			var mapPos = PixelToMap(pixelPos.X, pixelPos.Y);
			return Get(mapPos.X, mapPos.Y);
		}

		protected internal Tile? Get(int col, int row) {
			if (col < 0 || row < 0 || col >= Map.Count || row >= Map[col].Count) {
				return null;
			}
			var tile = Map[col][row];
			if (tile.IsEmpty) return null;
			return tile;
		}

		/// <summary> Converts PixelSpace coords to Column and Row in the Map </summary>
		protected internal Point PixelToMap(int x, int y) {
			x -= X;
			y -= Y;
			return new Point((int)Math.Floor((float)x / TileWidth), (int)Math.Floor((float)y / TileHeight));
		}

		/// <summary> Converts Map Column and Row to PixelSpace coords </summary>
		protected internal Point MapToPixel(int col, int row) {
			return new Point(col * Tileset.CellWidth + X, row * Tileset.CellHeight + Y);
		}

	}
}
