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
		public MenuBox Top { get; private set; }

		public void UpdateStart() {
			if (Content.Count == 0) {
				Top = null;
				return;
			}
			Top = Content.Peek();
		}

		public void Update() {
			if (Top == null) return;

			Top.Position = Position;
			Top.UpdateState();
			Top.UpdateSize(Point.Zero);
			Top.UpdateContentPositions();
		}

		public void Draw(Renderer r) {
			if (Top == null) return;

			Top.Draw(r);
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
