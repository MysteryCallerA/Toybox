using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.utils.math {
	public class Grid {

		public Point CellSize;
		public Point Origin;

		public Grid(Point cellSize) { CellSize = cellSize; }

		public Point ToCell(Point pos) {
			pos.X = ToColumn(pos.X);
			pos.Y = ToRow(pos.Y);
			return pos;
		}

		public int ToColumn(int x) {
			return (int)Math.Floor((float)(x - Origin.X) / CellSize.X) * CellSize.X;
		}

		public int ToRow(int y) {
			return (int)Math.Floor((float)(y - Origin.Y) / CellSize.Y) * CellSize.Y;
		}

		public Point ToWorld(Point cell) {
			cell.X = ToWorldX(cell.X);
			cell.Y = ToWorldY(cell.Y);
			return cell;
		}

		public int ToWorldX(int column) {
			return (column * CellSize.X) + Origin.X;
		}

		public int ToWorldY(int row) {
			return (row * CellSize.Y) + Origin.Y;
		}

		public Rectangle GetCellWorldBounds(Point cell) {
			return new Rectangle(ToWorld(cell), CellSize);
		}

	}
}
