using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;

namespace Toybox.gui {
	public class SelectMenu:MenuBox {

		public int SelectionId = 0;

		public MenuSelector Selector = new MenuSelector();
		public MenuControls Controls = new MenuControls();
		private MenuSystem CurrentParent;

		public SelectMenu() {
		}

		public override void Draw(Renderer r) {
			base.Draw(r);
			Selector?.Draw(r);
		}

		public void Update(MenuSystem parent) {
			CurrentParent = parent;
			Update();
			CurrentParent = null;
		}

		public override void Update() {
			base.Update();

			UpdateControls();
			
			if (Selector != null) {
				if (SelectionId < 0) Selector.Update(null);
				else Selector.Update(Content[SelectionId]);
			}
		}

		public virtual void UpdateControls() {
			if (Controls == null) return;

			if (Controls.KeyUp != null && Controls.KeyUp.Pressed) PressUp();
			if (Controls.KeyDown != null && Controls.KeyDown.Pressed) PressDown();
			if (Controls.KeyLeft != null && Controls.KeyLeft.Pressed) PressLeft();
			if (Controls.KeyRight != null && Controls.KeyRight.Pressed) PressRight();

			if (Controls.KeyConfirm != null && Controls.KeyConfirm.Pressed) PressConfirm();
			if (Controls.KeyBack != null && Controls.KeyBack.Pressed) PressBack();
		}

		public virtual void PressUp() {
			Layout.SelectUp(Content, SelectionId, out SelectionId);
		}

		public virtual void PressDown() {
			Layout.SelectDown(Content, SelectionId, out SelectionId);
		}

		public virtual void PressLeft() {
			Layout.SelectLeft(Content, SelectionId, out SelectionId);
		}

		public virtual void PressRight() {
			Layout.SelectRight(Content, SelectionId, out SelectionId);
		}

		public virtual void PressConfirm() {
			if (CurrentParent != null) {
				CurrentParent.TrySwitchMenu(Content[SelectionId].Name, true);
			}
		}

		public virtual void PressBack() {
			if (CurrentParent != null) {
				CurrentParent.BackMenu();
			}
		}

	}
}
