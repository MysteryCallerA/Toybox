using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.content;
using Toybox.gui.core;
using Toybox.gui.style;
using Toybox.utils.text;

namespace Toybox.gui.select {
	public class MenuSelector {

		public MenuElement Graphic;
		public MenuBox SelectedBox { get; private set; }
		public int SelectionId { get; private set; }

		public MenuControl UpKey = MenuControl.Up;
		public MenuControl DownKey = MenuControl.Down;
		public MenuControl LeftKey = MenuControl.Left;
		public MenuControl RightKey = MenuControl.Right;

		public MenuSelector(MenuElement graphic) {
			Graphic = graphic;
		}

		public MenuSelector(Font f) {
			Graphic = new MenuText(f, ">");
			Graphic.VAlign = MenuElement.VAlignType.Center;
			Graphic.UpdateSize(Point.Zero);
			Graphic.XOffset = -Graphic.OuterSize.X - 5;
		}

		public void Draw(Renderer r) {
			Graphic.Draw(r);
		}

		public void Update(MenuControlManager c, MenuSystem parent) {
			UpdateControls(c, parent);

			var s = GetSelected();
			var bounds = s.PanelBounds;
			Graphic.Position = bounds.Location;
			Graphic.UpdateState();
			Graphic.UpdateSize(bounds.Size);
			Graphic.UpdateContentPositions();
		}

		private void UpdateControls(MenuControlManager c, MenuSystem parent) {
			if (c == null) return;
			if (SelectedBox == null) return;

			if (c.TryGet(UpKey, out var up) && up.Pressed) {
				SelectUp();
				up.DropPress();
			}
			if (c.TryGet(DownKey, out var down) && down.Pressed) {
				SelectDown();
				down.DropPress();
			}
			if (c.TryGet(LeftKey, out var left) && left.Pressed) {
				SelectLeft();
				left.DropPress();
			}
			if (c.TryGet(RightKey, out var right) && right.Pressed) {
				SelectRight();
				right.DropPress();
			}
		}

		public void SelectUp() {
			SelectedBox.Layout.SelectUp(SelectedBox.Content, SelectionId, out var nextid);
			SelectionId = nextid;
		}

		public void SelectDown() {
			SelectedBox.Layout.SelectDown(SelectedBox.Content, SelectionId, out var nextid);
			SelectionId = nextid;
		}

		public void SelectLeft() {
			SelectedBox.Layout.SelectLeft(SelectedBox.Content, SelectionId, out var nextid);
			SelectionId = nextid;
		}

		public void SelectRight() {
			SelectedBox.Layout.SelectRight(SelectedBox.Content, SelectionId, out var nextid);
			SelectionId = nextid;
		}

		public void Focus(MenuBox b) {
			SelectedBox = b;
			if (b.Content.Count == 0) return;
			SelectionId = 0;
		}

		public MenuElement GetSelected() {
			if (SelectedBox == null) return null;
			if (SelectionId < 0 || SelectionId >= SelectedBox.Content.Count) return null;
			return SelectedBox.Content[SelectionId];
		}

	}
}
