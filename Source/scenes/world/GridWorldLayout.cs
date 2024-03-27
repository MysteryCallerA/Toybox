using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.utils.data;

namespace Toybox.scenes.world {
	public class GridWorldLayout:WorldLayout {

		public Point CellSize { get; private set; }

		private List2d<string> Map;

		public GridWorldLayout() {
		}

		public override void AddData(string data, Rectangle bounds) {
			if (Map == null) {
				CellSize = new Point(bounds.Width, bounds.Height);
				Origin = bounds.Location;
				Map = new List2d<string>();
			}
			
			var cell = PixelToCell(bounds.Location);
			if (cell.X < 0) {
				Origin.X -= CellSize.X * -cell.X;
				Map.ExpandLeft(-cell.X);
				cell.X = 0;
			}
			if (cell.Y < 0) {
				Origin.Y -= CellSize.Y * -cell.Y;
				Map.ExpandUp(-cell.Y);
				cell.Y = 0;
			}

			Map.Set(data, cell.X, cell.Y);
		}

		private Point PixelToCell(Point pixel) {
			return (pixel - Origin) / CellSize;
		}

	}
}
