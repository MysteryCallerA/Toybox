using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Toybox.graphic {
	public class TextureGrid {

		public Texture2D Graphic;
		public Point Origin;
		public readonly int CellWidth;
		public readonly int CellHeight;
		public readonly int Columns;
		public readonly int Rows;

		public TextureGrid(Texture2D t, int cellWidth, int cellHeight) {
			CellWidth = cellWidth;
			CellHeight = cellHeight;
			Columns = t.Width / cellWidth;
			Rows = t.Height / cellHeight;
			Graphic = t;
		}

		public Rectangle GetCell(Point cellpos) {
			return new Rectangle(cellpos.X * CellWidth, cellpos.Y * CellHeight, CellWidth, CellHeight);
		}

		public TextureSelection ToTextureSelection(Point cell) {
			return new TextureSelection(Graphic, GetCell(cell));
		}

		public Point CellSize {
			get { return new Point(CellWidth, CellHeight); }
		}

		public void GetDrawRects(Point cell, Point destPos, out Rectangle source, out Rectangle dest) {
			source = GetCell(cell);
			dest = new Rectangle(destPos.X - Origin.X, destPos.Y - Origin.Y, source.Width, source.Height);
		}

	}

	public class TextureGridState:TextureGrid, IGraphicState {

		public Point Cell;

		public TextureGridState(Texture2D t, int cellWidth, int cellHeight):base(t, cellWidth, cellHeight) {
		}
		public TextureGridState(TextureGrid t):base(t.Graphic, t.CellWidth, t.CellHeight) {
		}

		public void GetDrawData(Point destPos, out Texture2D graphic, out Rectangle source, out Rectangle dest) {
			GetDrawRects(Cell, destPos, out source, out dest);
			graphic = Graphic;
		}
	}
}
