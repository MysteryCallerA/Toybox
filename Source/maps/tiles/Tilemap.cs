using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Toybox.graphic;

namespace Toybox.maps.tiles {
	public class Tilemap {

		/// <summary> Map[x][y] <br></br> Bounds must be kept uniform. </summary>
		protected internal List<List<Tile>> Map = new();
		public int X, Y;
		public TextureGrid Tileset;
		protected internal Dictionary<Point, TileTransition> Transitions = new();
		private List<Point> EndedTransitions = new();

		protected internal Tilemap(TextureGrid t) { Tileset = t; }

		public Tilemap(TextureGrid t, List<List<Tile>> data) {
			Tileset = t;
			Map = data;
		}

		public void Draw(Renderer r, Camera c) {
			if (Map.Count == 0) return;
			var bounds = c.ScaledBounds;
			var topleft = ScaledPixelToCell(bounds.Left, bounds.Top) - new Point(1);
			var botright = ScaledPixelToCell(bounds.Right, bounds.Bottom) + new Point(1);

			if (topleft.X < 0) topleft.X = 0;
			if (topleft.Y < 0) topleft.Y = 0;
			if (botright.X > Map.Count) botright.X = Map.Count;
			if (botright.Y > Map[0].Count) botright.Y = Map[0].Count;

			var dest = new Rectangle(CellToScaledPixel(topleft.X, topleft.Y), TileSize);
			dest = c.Project(Camera.Space.Scaled, Camera.Space.Render, dest);
			int starty = dest.Y;

			for (int col = topleft.X; col < botright.X; col++) {
				for (int row = topleft.Y; row < botright.Y; row++) {
					var tile = Map[col][row];
					if (!tile.IsEmpty) {
						if (Transitions.Count != 0 && Transitions.TryGetValue(new Point(col, row), out var trans)) {
							r.Batch.Draw(Tileset.Texture, new Rectangle(dest.Location + trans.Translation, dest.Size), Tileset.GetCell(tile.Id), Color.White * trans.Opacity, 0, Vector2.Zero, tile.Effect, 0);
						} else {
							r.Batch.Draw(Tileset.Texture, dest, Tileset.GetCell(tile.Id), Color.White, 0, Vector2.Zero, tile.Effect, 0);
						}
					}
					dest.Y += dest.Height;
				}
				dest = new Rectangle(dest.X + dest.Width, starty, dest.Width, dest.Height);
			}
		}

		public void Update() {
			if (Transitions.Count == 0) return;

			foreach (var tt in Transitions) {
				EndedTransitions.Clear();
				tt.Value.FramesElapsed++;
				if (tt.Value.Ended) {
					if (tt.Value.RemoveOnEnd) Map[tt.Key.X][tt.Key.Y] = new Tile();
					EndedTransitions.Add(tt.Key);
				}
			}
			foreach (var k in EndedTransitions) {
				Transitions.Remove(k);
			}
		}

		public Tile? Get(Point pixelPos) {
			var mapPos = ScaledPixelToCell(pixelPos.X, pixelPos.Y);
			return Get(mapPos.X, mapPos.Y);
		}

		public Tile? Get(Point pixelPos, out Rectangle hitbox) {
			var mapPos = ScaledPixelToCell(pixelPos.X, pixelPos.Y);
			hitbox = new Rectangle(CellToScaledPixel(mapPos.X, mapPos.Y), TileSize);
			return Get(mapPos.X, mapPos.Y);
		}

		public Tile? Get(int col, int row) {
			if (col < 0 || row < 0 || col >= Map.Count || row >= Map[col].Count) {
				return null;
			}
			var tile = Map[col][row];
			if (tile.IsEmpty) return null;
			return tile;
		}

		protected internal Point ScaledPixelToCell(int x, int y) {
			return new Point((int)Math.Floor((float)(x - X) / TileWidth), (int)Math.Floor((float)(y - Y) / TileHeight));
		}
		protected internal Point CellToScaledPixel(int col, int row) {
			return new Point(col * Tileset.CellWidth + X, row * Tileset.CellHeight + Y);
		}

		protected internal bool InBounds(int col, int row) {
			if (col < 0) return false;
			if (row < 0) return false;
			if (col >= Map.Count) return false;
			if (row >= Map[col].Count) return false;
			return true;
		}

		public void SetData(List<List<Tile>> data) { Map = data; }

		public int Rows { get { if (Map.Count == 0) return 0; else return Map[0].Count; } }
		public int Columns { get { return Map.Count; } }
		public int TileWidth { get { return Tileset.CellWidth; } }
		public int TileHeight { get { return Tileset.CellHeight; } }
		public Point TileSize { get { return new Point(TileWidth, TileHeight); } }
		public Rectangle Bounds {
			get { return new Rectangle(X, Y, TileWidth * Map.Count, TileHeight * Rows); }
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

			public readonly bool IsEmpty {
				get { return Id.X < 0; }
			}
		}

	}
}
