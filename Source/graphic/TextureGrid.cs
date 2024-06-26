﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Toybox.graphic {
	public class TextureGrid:TextureObject {

		public readonly int CellWidth;
		public readonly int CellHeight;
		public readonly int Columns;
		public readonly int Rows;
		public Point SelectedCell;

		public TextureGrid(Texture2D t, int cellWidth, int cellHeight):base(t) {
			CellWidth = cellWidth;
			CellHeight = cellHeight;
			Columns = t.Width / cellWidth;
			Rows = t.Height / cellHeight;
			Origin = new Rectangle(0, 0, CellWidth, CellHeight);
		}

		public Rectangle GetCell(Point cellpos) {
			return new Rectangle(cellpos.X * CellWidth, cellpos.Y * CellHeight, CellWidth, CellHeight);
		}

		public override Rectangle Source {
			get { return GetCell(SelectedCell); }
		}

		public TextureSelection GetSelection(Point cell) {
			return new TextureSelection(Texture, GetCell(cell));
		}

		public Point CellSize {
			get { return new Point(CellWidth, CellHeight); }
		}

		public void GetDrawRects(Point cell, int x, int y, int scale, out Rectangle source, out Rectangle dest) {
			source = GetCell(cell);
			dest = new Rectangle(x - (Origin.X * scale), y - (Origin.Y * scale), source.Width * scale, source.Height * scale);
		}

	}
}
