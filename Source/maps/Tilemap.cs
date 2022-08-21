using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Utils.data;
using Utils.graphic;
using Utils.save;

namespace Toybox.maps {
	public class Tilemap:IXmlSaveable {

		public int X;
		public int Y;
		public TextureGrid Texture;

		private BiList2d<Point?> Map;

		public Tilemap(TextureGrid t) {
			Map = new BiList2d<Point?>();
			Texture = t;
		}

		public int TileWidth {
			get { return Texture.CellWidth; }
		}

		public int TileHeight {
			get { return Texture.CellHeight; }
		}

		public Point TileSize {
			get { return new Point(TileWidth, TileHeight); }
		}

		public void SetData(BiList2d<Point?> data) {
			Map = data;
		}

		/// <summary> Accepts unaligned points in world space. </summary>
		public void Set(int x, int y, Point? tile) {
			if (Map.IsEmpty) {
				var p = GetGridPosition(x, y);
				X = p.X;
				Y = p.Y;
			}

			Map.Set(WorldToTile(x, y), tile);
		}

		/// <summary> Gets the tile at a position in unaligned game space. Returns null if no tile. </summary>
		public Point? Get(int x, int y) {
			var tile = WorldToTile(x, y);
			if (Map.TryGet(tile.X, tile.Y, out Point? output)) {
				return output;
			}
			return null;
		}

		public void Draw(SpriteBatch s, Camera c) {
			if (Map.IsEmpty) return;
			var bounds = c.GetGameBounds();
			var topleft = WorldToTile(bounds.Left, bounds.Top);
			var botright = WorldToTile(bounds.Right, bounds.Bottom);
			botright.X += 1;
			botright.Y += 1;

			if (topleft.X < Map.Left) topleft.X = Map.Left;
			if (topleft.Y < Map.Top) topleft.Y = Map.Top;
			if (botright.X > Map.Cols) botright.X = Map.Cols;
			if (botright.Y > Map.Rows) botright.Y = Map.Rows;

			var start = TileToWorld(topleft.X, topleft.Y);
			var dest = GetTileBounds(start.X, start.Y);
			dest = new Rectangle(dest.X * c.GameScale, dest.Y * c.GameScale, dest.Width * c.GameScale, dest.Height * c.GameScale);
			dest.X -= c.WorldX;
			dest.Y -= c.WorldY;

			int starty = dest.Y;
			for (int col = topleft.X; col < botright.X; col++) {
				for (int row = topleft.Y; row < botright.Y; row++) {
					if (Map.TryGet(col, row, out Point? tile)) {
						if (tile.HasValue) {
							Texture.Draw(s, dest, tile.Value);
						}
					}
					dest.Y += dest.Height;
				}
				dest.Y = starty;
				dest.X += dest.Width;
			}
		}

		/// <summary> Returns the bounds of the tile at the given world position. </summary>
		public Rectangle GetTileBounds(int x, int y) {
			var location = GetGridPosition(x, y);
			return new Rectangle(location.X, location.Y, TileWidth, TileHeight);
		}

		/// <summary> Returns the bounds of the tile at the given world position. </summary>
		public Rectangle GetTileBounds(Point p) {
			return GetTileBounds(p.X, p.Y);
		}

		/// <summary> Returns position in the map cooresponding to world coords. </summary>
		private Point WorldToTile(int x, int y) {
			x -= X;
			y -= Y;
			return new Point((int)Math.Floor((float)x / TileWidth), (int)Math.Floor((float)y / TileHeight));
		}

		/// <summary> Returns position in world cooresponding to tile column and row. </summary>
		private Point TileToWorld(int col, int row) {
			return new Point((int)(col * Texture.CellWidth) + X, (int)(row * Texture.CellHeight) + Y);
		}

		/// <summary> Rounds the position down to match the tilemap grid. </summary>
		public Point GetGridPosition(int x, int y) {
			return new Point((int)Math.Floor((float)x / TileWidth) * TileWidth, (int)Math.Floor((float)y / TileHeight) * TileHeight);
		}

		public void Save(XmlWriter writer) {
			writer.WriteStartElement("tilemap");
			writer.WriteAttributeString("x", X.ToString());
			writer.WriteAttributeString("y", Y.ToString());

			Texture.Save(writer);

			writer.WriteStartElement("data");
			writer.WriteAttributeString("texturecols", Texture.Columns.ToString());
			writer.WriteAttributeString("left", Map.Left.ToString());
			writer.WriteAttributeString("top", Map.Top.ToString());

			for (int row = Map.Top; row <= Map.Bottom; row++) {
				writer.WriteStartElement("row");
				var output = new StringBuilder();
				output.Append(TileToFrame(Map[Map.Left, row]));
				for (int col = Map.Left + 1; col <= Map.Right; col++) {
					output.Append(",");
					output.Append(TileToFrame(Map[col, row]));
				}
				writer.WriteString(output.ToString());
				writer.WriteEndElement();
			}

			writer.WriteEndElement();

			writer.WriteEndElement();
		}

		public int TileToFrame(Point? tile) {
			if (!tile.HasValue) return -1;
			return tile.Value.X + (tile.Value.Y * Texture.Columns);
		}

		public Point? FrameToTile(int frame, int sourceCols) {
			if (frame == -1) return null;
			return new Point(frame % sourceCols, (int)Math.Floor((float)frame / sourceCols));
		}

		public Rectangle GetBounds() {
			var topleft = TileToWorld(Map.Left, Map.Top);
			var botright = TileToWorld(Map.Right, Map.Bottom);
			return new Rectangle(topleft.X, topleft.Y, botright.X + TileWidth, botright.Y + TileHeight);
		}
	}
}
