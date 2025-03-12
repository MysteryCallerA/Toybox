using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;

namespace Toybox.gui {
	public class MenuStack {

		private Stack<MenuBox> Content = new();
		public Point Position;

		public void Update() {
			if (Content.Count == 0) return;

			var box = Content.Peek();
			box.Position = Position;
			box.UpdateState();
			box.UpdateSize(Point.Zero);
			box.UpdateContentPositions();
		}

		public void Draw(Renderer r) {
			if (Content.Count == 0) return;

			var box = Content.Peek();
			box.Draw(r);
		}

		public MenuBox Top {
			get { return Content.Peek(); }
		}

		public void Push(MenuBox b) {
			Content.Push(b);
		}

		public void Drop() {
			if (Content.Count == 1) return;
			Content.Pop();
		}

		public int Count {
			get { return Content.Count; }
		}

	}
}
