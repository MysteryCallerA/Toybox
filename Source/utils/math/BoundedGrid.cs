using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.utils.math {
	public class BoundedGrid:Grid {

		/// <summary> Number of columns and rows to limit the grid to. </summary>
		public Point Size;

		public BoundedGrid(Point cellSize, Point size):base(cellSize) { Size = size; }

		public bool IsCellInBounds(Point cell) {
			if (cell.X < 0 || cell.Y < 0 || cell.X >= Size.X || cell.Y >= Size.Y) return false;
			return true;
		}

		public bool TryGetCell(Point pos, out Point cell) {
			pos.X = ToColumn(pos.X);
			pos.Y = ToRow(pos.Y);
			cell = pos;
			if (!IsCellInBounds(pos)) return false;
			return true;
		}

	}
}
