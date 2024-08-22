using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.maps.tiles {
	public static class TilemapTransitions {

		public static void SetTransition(this Tilemap t, int x, int y, TileTransition transition) {
			var cell = t.ScaledPixelToCell(x, y);
			t.Transitions.TryAdd(cell, transition);
		}

	}
}
