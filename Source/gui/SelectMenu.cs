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
		public MenuController Controller = new MenuController();
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

			Controller.Update(this);
			
			if (Selector != null) {
				if (SelectionId < 0) Selector.Update(null);
				else Selector.Update(Content[SelectionId]);
			}
		}

		public virtual void PressConfirm() {
			if (CurrentParent != null) {
				CurrentParent.PressedConfirm(Content[SelectionId], SelectionId);
			}
		}

		public virtual void PressCancel() {
			if (CurrentParent != null) {
				CurrentParent.PressedCancel(Content[SelectionId], SelectionId);
			}
		}

	}
}
