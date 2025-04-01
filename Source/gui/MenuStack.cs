using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;
using Toybox.gui.select;

namespace Toybox.gui {
	public class MenuStack {

		private readonly Stack<MenuBox> Content = new();
		public MenuBox Top { get; private set; }

		public Point Position;
		public MenuControlManager Controls = new();
		public MenuSelector Selector;
		public MenuBox InitialMenu;
		public MenuControl OpenKey = MenuControl.Open;

		public void Update() {
			if (Content.Count == 0) {
				Top = null;
				if (Controls.TryGet(OpenKey, out var key) && key.Pressed) {
					if (Open()) key.DropPress();
				}
				return;
			}
			Top = Content.Peek();

			Selector?.UpdateFunction(Controls, this);

			Top.Position = Position;
			Top.UpdateState();
			Top.UpdateSize(Point.Zero);
			Top.UpdateContentPositions();

			Selector?.UpdateGraphic();
		}

		public void Draw(Renderer r) {
			if (Top == null) return;

			Top.Draw(r);
			Selector?.Draw(r);
		}

		public virtual void Push(MenuBox b) {
			Content.Push(b);
		}

		public virtual void Drop() {
			Content.Pop();
		}

		public virtual bool Open() {
			if (InitialMenu != null) {
				Push(InitialMenu);
				return true;
			}
			return false;
		}

		public int Count {
			get { return Content.Count; }
		}

	}
}
