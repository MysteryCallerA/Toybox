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
		public MenuControl UpKey = MenuControl.Up;
		public MenuControl DownKey = MenuControl.Down;
		public MenuControl LeftKey = MenuControl.Left;
		public MenuControl RightKey = MenuControl.Right;
		public MenuControl BackKey = MenuControl.Back;

		private Dictionary<MenuBox, int> SelectionMemory = new();
		private MenuStack SelectedStack;
		private MenuBox SelectedBox;
		private int SelectionId;
		private MenuElement PrevSelectedElement;

		public MenuSelector(MenuElement graphic) {
			Graphic = graphic;
		}

		public MenuSelector(Font f) {
			Graphic = new MenuText(f, ">") {
				VAlign = MenuElement.VAlignType.Center
			};
			Graphic.UpdateSize(Point.Zero);
			Graphic.XOffset = -Graphic.OuterSize.X - 5;
		}

		private MenuElement SelectedElement {
			get {
				if (SelectionId < 0 || SelectionId >= SelectedBox.Content.Count) return null;
				return SelectedBox.Content[SelectionId];
			}
		}

		public void Draw(Renderer r) {
			if (SelectedStack == null) return;
			if (SelectedElement == null) return;
			Graphic.Draw(r);
		}

		internal void UpdateFunction(MenuControlManager c, MenuSystem parent) {
			if (SelectedStack == null) {
				Init(parent);
				return;
			}

			CheckIfBoxChanged();
			if (SelectionId >= SelectedBox.Content.Count) SelectionId = SelectedBox.Content.Count - 1;

			UpdateControls(c, parent);
			SelectedElement?.UpdateFunction(c, parent, SelectedStack);

			UpdateSelectedState();
		}

		internal void UpdateGraphic() {
			if (SelectedStack == null) return;
			if (SelectedElement == null) return;

			var bounds = SelectedElement.PanelBounds;
			Graphic.Position = bounds.Location;
			Graphic.UpdateState();
			Graphic.UpdateSize(bounds.Size);
			Graphic.UpdateContentPositions();
		}

		private void UpdateControls(MenuControlManager c, MenuSystem parent) {
			if (c == null) return;

			if (c.TryGet(BackKey, out var back) && back.Pressed) {
				if (Back()) back.DropPress();
			}
			if (c.TryGet(UpKey, out var up) && up.Pressed) {
				if (SelectUp()) up.DropPress();
			}
			if (c.TryGet(DownKey, out var down) && down.Pressed) {
				if (SelectDown()) down.DropPress();
			}
			if (c.TryGet(LeftKey, out var left) && left.Pressed) {
				if (SelectLeft()) left.DropPress();
			}
			if (c.TryGet(RightKey, out var right) && right.Pressed) {
				if (SelectRight()) right.DropPress();
			}
		}

		public bool SelectUp() {
			SelectedBox.GetSelectionUp(SelectionId, out SelectionId, out var output);
			return output;
		}

		public bool SelectDown() {
			SelectedBox.GetSelectionDown(SelectionId, out SelectionId, out var output);
			return output;
		}

		public bool SelectLeft() {
			SelectedBox.GetSelectionLeft(SelectionId, out SelectionId, out var output);
			return output;
		}

		public bool SelectRight() {
			SelectedBox.GetSelectionRight(SelectionId, out SelectionId, out var output);
			return output;
		}

		public bool Back() {
			if (SelectedStack.Count <= 1) return false;
			SelectedStack.Drop();
			return true;
		}

		private void Init(MenuSystem parent) {
			if (parent.Content.Count == 0) return;
			var stack = parent.Content[0];
			if (stack.Top == null) return;
			var box = stack.Top;

			for (int id = 0; id < box.Content.Count; id++) {
				var e = box.Content[id];
				if (e.Selectable) {
					SelectionId = id;
					SelectedStack = stack;
					SelectedBox = box;
					return;
				}
			}
		}

		private void CheckIfBoxChanged() {
			if (SelectedStack.Top == SelectedBox) return;

			SelectionMemory[SelectedBox] = SelectionId;
			SelectedBox = SelectedStack.Top;
			if (SelectionMemory.TryGetValue(SelectedBox, out int id)) {
				SelectionId = id;
			} else {
				SelectionId = 0;
			}
		}

		private void UpdateSelectedState() {
			if (SelectedElement == PrevSelectedElement) return;
			PrevSelectedElement?.State.Remove(MenuState.Selected);
			SelectedElement?.State.Add(MenuState.Selected);
			PrevSelectedElement = SelectedElement;
		}


	}
}
