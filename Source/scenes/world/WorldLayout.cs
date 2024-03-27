using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.scenes.world {
	public abstract class WorldLayout {

		public Point Origin = Point.Zero;

		public WorldLayout() {
		}

		public abstract void AddData(string data, Rectangle bounds);

	}
}
