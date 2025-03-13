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

		public MenuControl UpKey = MenuControl.Up;
		public MenuControl DownKey = MenuControl.Down;
		public MenuControl LeftKey = MenuControl.Left;
		public MenuControl RightKey = MenuControl.Right;
		public MenuControl BackKey = MenuControl.Back;

		public MenuElement Graphic;
		public int SelectionId;
		public MenuStack SelectedStack;
		public readonly Dictionary<MenuBox, int> SelectionMemory = new();

		private MenuElement PrevSelectedElement;
		private MenuBox PrevSelectedBox;

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

		public MenuElement GetSelected() {
			if (SelectedStack == null || SelectedStack.Top == null) return null;
			if (SelectionId < 0 || SelectionId >= SelectedStack.Top.Content.Count) return null;
			return SelectedStack.Top.Content[SelectionId];
		}

		internal void UpdateFunction(MenuControlManager c, MenuSystem system) {
			if (SelectedStack == null) {
				if (system.Content.Count > 0) SelectedStack = system.Content[0];
				else return;
			}
			
			if (SelectedStack.Top != PrevSelectedBox) RememberSelection();
			SelectionId = Math.Clamp(SelectionId, 0, SelectedStack.Top.Content.Count - 1);

			var select = GetSelected();
			if (select == null) return;

			UpdateControls(c);
			select.UpdateFunction(c, system, SelectedStack);

			if (select != PrevSelectedElement) UpdateSelectedState(select);
		}

		internal void UpdateGraphic() {
			var select = GetSelected();
			if (select == null) return;

			Graphic.Position = select.PanelOrigin;
			Graphic.UpdateState();
			Graphic.UpdateSize(select.PanelSize);
			Graphic.UpdateContentPositions();
		}

		public void Draw(Renderer r) {
			if (GetSelected() == null) return;
			Graphic.Draw(r);
		}

		private void RememberSelection() {
			if (PrevSelectedBox != null) {
				SelectionMemory[PrevSelectedBox] = SelectionId;
			}
			if (SelectionMemory.TryGetValue(SelectedStack.Top, out int id)) {
				SelectionId = id;
			} else {
				SelectionId = 0;
			}
			PrevSelectedBox = SelectedStack.Top;
		}

		private void UpdateSelectedState(MenuElement newSelection) {
			PrevSelectedElement?.State.Remove(MenuState.Selected);
			newSelection?.State.Add(MenuState.Selected);
			PrevSelectedElement = newSelection;
		}


		//-------- Controls --------

		private void UpdateControls(MenuControlManager c) {
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
			SelectedStack.Top.GetSelectionUp(SelectionId, out SelectionId, out var output);
			return output;
		}

		public bool SelectDown() {
			SelectedStack.Top.GetSelectionDown(SelectionId, out SelectionId, out var output);
			return output;
		}

		public bool SelectLeft() {
			SelectedStack.Top.GetSelectionLeft(SelectionId, out SelectionId, out var output);
			return output;
		}

		public bool SelectRight() {
			SelectedStack.Top.GetSelectionRight(SelectionId, out SelectionId, out var output);
			return output;
		}

		public bool Back() {
			if (SelectedStack.Count <= 1) return false;
			SelectedStack.Drop();
			return true;
		}


	}
}
