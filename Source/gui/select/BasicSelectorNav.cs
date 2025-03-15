using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.gui.select {
	public class BasicSelectorNav:IMenuSelectorNav {

		public void SelectDown(MenuSelector parent, out bool inputWasPossible) {
			parent.SelectedStack.Top.Layout.SelectDown(parent.SelectedStack.Top.Content, parent.SelectionId, out var nextSelection, out inputWasPossible, out var wrap);
			parent.SelectionId = nextSelection;
		}

		public void SelectLeft(MenuSelector parent, out bool inputWasPossible) {
			parent.SelectedStack.Top.Layout.SelectLeft(parent.SelectedStack.Top.Content, parent.SelectionId, out var nextSelection, out inputWasPossible, out var wrap);
			parent.SelectionId = nextSelection;
		}

		public void SelectRight(MenuSelector parent, out bool inputWasPossible) {
			parent.SelectedStack.Top.Layout.SelectRight(parent.SelectedStack.Top.Content, parent.SelectionId, out var nextSelection, out inputWasPossible, out var wrap);
			parent.SelectionId = nextSelection;
		}

		public void SelectUp(MenuSelector parent, out bool inputWasPossible) {
			parent.SelectedStack.Top.Layout.SelectUp(parent.SelectedStack.Top.Content, parent.SelectionId, out var nextSelection, out inputWasPossible, out var wrap);
			parent.SelectionId = nextSelection;
		}
	}
}
