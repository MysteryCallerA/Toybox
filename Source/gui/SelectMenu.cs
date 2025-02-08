using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;
using Toybox.gui.style;

namespace Toybox.gui {
	public class SelectMenu:MenuBox {

		private int PrevSelectionId = -1;
		public int SelectionId = 0;

		public MenuSelector Selector;

		public bool AllowBack = true;
		public bool AllowClose = true;
		public bool AllowAutoSwitch = true;

		public SelectMenu() {
		}

		public override void Draw(Renderer r) {
			BackPanel?.Draw(r);
			Selector?.Draw(r);
			foreach (var e in Content) {
				e.Draw(r);
			}
		}

		protected internal override void UpdateFunction(MenuControls c) {
			if (Controls != null) c = Controls;
			UpdateDirectionalControls(c);

			if (Content.Count > 0) {
				Content[SelectionId].UpdateFunction(c);
			}

			UpdateInteractionControls(c);
		}

		protected override void UpdateContentSize(Point contentContainerSize, out Point contentSize) {
			UpdateSelectedState();

			base.UpdateContentSize(contentContainerSize, out contentSize);
		}

		protected internal override void UpdateContainedElementPositions() {
			base.UpdateContainedElementPositions();

			if (Selector != null) {
				if (SelectionId < 0) Selector.Update(null);
				else Selector.Update(Content[SelectionId]);
			}
		}

		protected virtual void UpdateDirectionalControls(MenuControls c) {
			if (c == null) return;

			if (c.Up != null && c.Up.Pressed) {
				if (PressUp()) c.Up.DropPress();
			}
			if (c.Down != null && c.Down.Pressed) {
				if (PressDown()) c.Down.DropPress();
			}
			if (c.Left != null && c.Left.Pressed) {
				if (PressLeft()) c.Left.DropPress();
			}
			if (c.Right != null && c.Right.Pressed) {
				if (PressRight()) c.Right.DropPress();
			}
		}

		protected virtual void UpdateInteractionControls(MenuControls c) {
			if (c == null) return;

			if (AllowClose && c.CloseMenu != null && c.CloseMenu.Pressed) {
				if (PressClose()) {
					c.CloseMenu.DropPress();
					return;
				}
			}
			if (AllowBack && c.Back != null && c.Back.Pressed) {
				if (PressBack()) c.Back.DropPress();
			}
			if (AllowAutoSwitch && c.Confirm != null && c.Confirm.Pressed) {
				if (PressSwitchMenu()) c.Confirm.DropPress();
			}
		}

		public virtual bool PressUp() { return Layout.SelectUp(Content, SelectionId, out SelectionId); }
		public virtual bool PressDown() { return Layout.SelectDown(Content, SelectionId, out SelectionId); }
		public virtual bool PressLeft() { return Layout.SelectLeft(Content, SelectionId, out SelectionId); }
		public virtual bool PressRight() { return Layout.SelectRight(Content, SelectionId, out SelectionId); }

		public virtual bool PressSwitchMenu() {
			if (ParentSystem != null) {
				return ParentSystem.TrySwitchMenu(Content[SelectionId].Name, true);
			}
			return false;
		}

		public virtual bool PressBack() {
			if (ParentSystem != null) {
				ParentSystem.BackMenu();
				return true;
			}
			return false;
		}

		public virtual bool PressClose() {
			if (ParentSystem != null) {
				ParentSystem.CloseMenu();
				return true;
			}
			return false;
		}

		private void UpdateSelectedState() {
			if (SelectionId == PrevSelectionId) return;

			if (PrevSelectionId >= 0 && PrevSelectionId < Content.Count) {
				var e = Content[PrevSelectionId];
				UnSelectElement(e);
				e.Cascade(UnSelectElement);
			}
			if (SelectionId >= 0 && SelectionId < Content.Count) {
				var e = Content[SelectionId];
				SelectElement(e);
				e.Cascade(SelectElement);
			}
			PrevSelectionId = SelectionId;
		}

		private static void UnSelectElement(MenuElement e) {
			e.State.Remove(MenuState.Selected);
		}

		private static void SelectElement(MenuElement e) {
			e.State.Add(MenuState.Selected);
		}

	}
}
