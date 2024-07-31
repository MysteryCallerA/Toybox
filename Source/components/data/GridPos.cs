using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.utils.math;

namespace Toybox.components.data {
	public class GridPos {

		public int X;
		public int Y;
		public int CellWidth;
		public int CellHeight;
		public int XOffset;
		public int YOffset;

		public GridPos(int cellsize):this(cellsize, cellsize) {
		}

		public GridPos(int cellwidth, int cellheight) {
			CellWidth = cellwidth;
			CellHeight = cellheight;
		}

		public Point Position {
			get { return new Point(X, Y); }
			set { X = value.X; Y = value.Y; }
		}

		public Point CellSize {
			get { return new Point(CellWidth, CellHeight); }
			set { CellWidth = value.X; CellHeight = value.Y; }
		}

		public Point Offset {
			get { return new Point(XOffset, YOffset); }
			set { XOffset = value.X; YOffset = value.Y; }
		}

		public Point ToCell(Point pos) {
			var x = (int)Math.Floor((float)(pos.X - XOffset) / CellWidth);
			var y = (int)Math.Floor((float)(pos.Y - YOffset) / CellHeight);
			return new Point(x, y);
		}

		public Point ToPos(Point cell) {
			var x = (cell.X * CellWidth) + XOffset;
			var y = (cell.Y * CellHeight) + YOffset;
			return new Point(x, y);
		}

		public Point ToCellOrigin(Point pos) {
			return new Point(ToCellOriginX(pos.X), ToCellOriginY(pos.Y));
		}

		public int ToCellOriginX(int xpos) {
			return ((int)Math.Floor((float)(xpos - XOffset) / CellWidth) * CellWidth) + XOffset;
		}

		public int ToCellOriginY(int ypos) {
			return ((int)Math.Floor((float)(ypos - YOffset) / CellHeight) * CellHeight) + YOffset;
		}

		public void PixelScaleChanged(int prevPixelScale, int newPixelScale) {
			CellSize = new Point(9 * newPixelScale);
			Offset = new Point(0 * newPixelScale, 1 * newPixelScale);
		}

	}
}
