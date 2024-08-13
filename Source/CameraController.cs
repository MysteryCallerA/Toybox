using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox {
	public class CameraController {

		public Rectangle Bounds;

		public void Focus(Camera c, Point p) {
			c.X = p.X - (c.ViewSize.X / 2);
			c.Y = p.Y - (c.ViewSize.Y / 2);
		}

	}
}
