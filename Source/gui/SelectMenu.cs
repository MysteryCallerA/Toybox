using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;

namespace Toybox.gui {
	public class SelectMenu:InteractiveMenu {

		public int SelectionId = 0;

		public MenuSelector Selector;
		public Action<MenuElement> OnOptionConfirm;

		public SelectMenu() {
		}

		public override void InheritSystemProps(MenuSystem m) {
			if (Controls == null) Controls = m.SharedControls;
			if (Selector == null) Selector = m.SharedSelector;
		}

		public override void Draw(Renderer r) {
			BackPanel?.Draw(r);
			Selector?.Draw(r);
			foreach (var e in Content) {
				e.Draw(r);
			}
		}

		public override void Update() {
			UpdateControls();

			base.Update();
			
			if (Selector != null) {
				if (SelectionId < 0) Selector.Update(null);
				else Selector.Update(Content[SelectionId]);
			}
		}

		public virtual void UpdateControls() {
			if (Controls == null) return;

			if (Controls.KeyUp != null && Controls.KeyUp.Pressed) {
				if (PressUp()) Controls.KeyUp.DropPress();
			}
			if (Controls.KeyDown != null && Controls.KeyDown.Pressed) {
				if (PressDown()) Controls.KeyDown.DropPress();
			}
			if (Controls.KeyLeft != null && Controls.KeyLeft.Pressed) {
				if (PressLeft()) Controls.KeyLeft.DropPress();
			}
			if (Controls.KeyRight != null && Controls.KeyRight.Pressed) {
				if (PressRight()) Controls.KeyRight.DropPress();
			}

			if (Controls.KeyConfirm != null && Controls.KeyConfirm.Pressed) PressConfirm();
			if (Controls.KeyBack != null && Controls.KeyBack.Pressed) PressBack();
		}

		public virtual bool PressUp() {
			return Layout.SelectUp(Content, SelectionId, out SelectionId);
		}

		public virtual bool PressDown() {
			return Layout.SelectDown(Content, SelectionId, out SelectionId);
		}

		public virtual bool PressLeft() {
			return Layout.SelectLeft(Content, SelectionId, out SelectionId);
		}

		public virtual bool PressRight() {
			return Layout.SelectRight(Content, SelectionId, out SelectionId);
		}

		public virtual void PressConfirm() {
			bool activated = Content[SelectionId].Activate();
			if (activated) return;

			OnOptionConfirm?.Invoke(Content[SelectionId]);
			if (ParentSystem != null) {
				ParentSystem.TrySwitchMenu(Content[SelectionId].Name, true);
			}
		}

		public virtual void PressBack() {
			if (ParentSystem != null) {
				ParentSystem.BackMenu();
			}
		}

	}
}
