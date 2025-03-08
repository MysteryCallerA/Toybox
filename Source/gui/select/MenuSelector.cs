using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;

namespace Toybox.gui.select {
	public class MenuSelector {

		public MenuElement Graphic;
		public Point Position;


		public MenuSelector() {

		}

		public void Draw(Renderer r) {
			Graphic.Draw(r);
		}

		public void Update() {
			Graphic.Position = Position; 
			Graphic.UpdateState();
			Graphic.UpdateSize(Point.Zero);
			Graphic.UpdateContentPositions();
		}

	}
}
