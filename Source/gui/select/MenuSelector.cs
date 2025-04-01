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
using Toybox.utils.tween;

namespace Toybox.gui.select {
	public class MenuSelector {

		public MenuControl UpKey = MenuControl.Up;
		public MenuControl DownKey = MenuControl.Down;
		public MenuControl LeftKey = MenuControl.Left;
		public MenuControl RightKey = MenuControl.Right;
		public MenuControl BackKey = MenuControl.Back;

		public Tween MoveTween;
		private Point TweenStart;
		private MenuElement TweenEnd;
		private int TweenFrame = 0;
		private bool WillSkipTween = false;
		private bool Tweening = false;

		public MenuElement Graphic;
		public IMenuSelectorNav Nav = new BasicSelectorNav();
		public int SelectionId;
		public MenuStack SelectedStack;
		public readonly Dictionary<MenuBox, int> SelectionMemory = new();

		public MenuElement PrevSelectedElement { get; private set; }
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

		internal void UpdateFunction(MenuControlManager c, MenuStack parent) {
			SelectedStack = parent;
			
			if (SelectedStack.Top != PrevSelectedBox) RememberSelection();
			SelectionId = Math.Clamp(SelectionId, 0, SelectedStack.Top.Content.Count - 1);

			var select = GetSelected();
			if (select == null) return;

			UpdateControls(c);
			select = GetSelected();
			select.UpdateFunction(c, SelectedStack);

			if (select != PrevSelectedElement) UpdateSelectedState(select);

			if (Tweening) {
				TweenFrame++;
				if (TweenFrame >= MoveTween.Frames) Tweening = false;
			}
		}

		internal void UpdateGraphic() {
			var select = GetSelected();
			if (select == null) return;

			if (Tweening) {
				var pos = TweenEnd.PanelOrigin - TweenStart;
				var multi = MoveTween.Get(TweenFrame);
				Graphic.Position = new Point((int)(pos.X * multi) + TweenStart.X, (int)(pos.Y * multi) + TweenStart.Y);
			} else {
				Graphic.Position = select.PanelOrigin;
			}
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
			if (MoveTween != null && PrevSelectedElement != null) {
				StartTween(newSelection);
			}

			PrevSelectedElement?.State.Remove(MenuState.Selected);
			newSelection?.State.Add(MenuState.Selected);
			PrevSelectedElement = newSelection;

			if (newSelection == null) return;
			SelectedStack.Top.Layout.SelectionChanged(newSelection);
		}

		private void StartTween(MenuElement select) {
			if (WillSkipTween) {
				WillSkipTween = false;
				return;
			}
			TweenStart = Graphic.Position;
			TweenEnd = select;
			TweenFrame = 0;
			Tweening = true;
		}

		public void SkipTween() {
			if (MoveTween == null) return;
			if (Tweening) {
				Tweening = false;
			} else {
				WillSkipTween = true;
			}
		}


		//-------- Controls --------

		private void UpdateControls(MenuControlManager c) {
			if (c == null) return;

			if (c.TryGet(BackKey, out var back) && back.Pressed) {
				if (Back()) back.DropPress();
			}
			if (c.TryGet(UpKey, out var up) && up.Pressed) {
				Nav.SelectUp(this, out var valid);
				if (valid) up.DropPress();
			}
			if (c.TryGet(DownKey, out var down) && down.Pressed) {
				Nav.SelectDown(this, out var valid);
				if (valid) down.DropPress();
			}
			if (c.TryGet(LeftKey, out var left) && left.Pressed) {
				Nav.SelectLeft(this, out var valid);
				if (valid) left.DropPress();
			}
			if (c.TryGet(RightKey, out var right) && right.Pressed) {
				Nav.SelectRight(this, out var valid);
				if (valid) right.DropPress();
			}
		}

		public void SelectUp() { Nav.SelectUp(this, out _); }
		public void SelectDown() { Nav.SelectDown(this, out _); }
		public void SelectLeft() { Nav.SelectLeft(this, out _); }
		public void SelectRight() {	Nav.SelectRight(this, out _); }

		public bool Back() {
			if (SelectedStack.Count == 0) return false;
			SelectedStack.Drop();
			return true;
		}


	}
}
