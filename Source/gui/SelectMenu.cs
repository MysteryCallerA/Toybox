using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;
using Toybox.gui.style;

namespace Toybox.gui {
	public class SelectMenu:MenuBox {

		private int _selectionId;
		public int SelectionId {
			get { return _selectionId; }
			set {
				if (_selectionId >= 0 && _selectionId < Content.Count) {
					var e = Content[_selectionId];
					UnSelectElement(e);
					e.Cascade(UnSelectElement);
				}
				_selectionId = value;
				if (_selectionId >= 0 && _selectionId < Content.Count) {
					var e = Content[_selectionId];
					SelectElement(e);
					e.Cascade(SelectElement);
				}
			}
		}

		public MenuSelector Selector;

		public bool AllowBack = true;
		public bool AllowClose = true;
		public bool AllowAutoSwitch = true;

		public SelectMenu() {
			SelectionId = 0;
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

		public virtual bool PressUp() {
			var output = Layout.SelectUp(Content, SelectionId, out var id);
			SelectionId = id;
			return output;
		}

		public virtual bool PressDown() {
			var output = Layout.SelectDown(Content, SelectionId, out var id);
			SelectionId = id;
			return output;
		}

		public virtual bool PressLeft() {
			var output = Layout.SelectLeft(Content, SelectionId, out var id);
			SelectionId = id;
			return output;
		}

		public virtual bool PressRight() {
			var output = Layout.SelectRight(Content, SelectionId, out var id);
			SelectionId = id;
			return output;
		}

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

		private static void UnSelectElement(MenuElement e) {
			e.State.Remove(MenuState.Selected);
		}

		private static void SelectElement(MenuElement e) {
			e.State.Add(MenuState.Selected);
		}

	}
}
