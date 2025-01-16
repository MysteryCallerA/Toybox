using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.utils.input;

namespace Toybox.gui.core {
	public class MenuController {

		public VirtualKey KeyUp;
		public VirtualKey KeyDown;
		public VirtualKey KeyLeft;
		public VirtualKey KeyRight;
		public VirtualKey KeyConfirm;
		public VirtualKey KeyCancel;

		public void Update(SelectMenu m) {
			if (KeyUp != null && KeyUp.Pressed) {
				PressUp(m);
			}
			if (KeyDown != null && KeyDown.Pressed) {
				PressDown(m);
			}
			if (KeyLeft != null && KeyLeft.Pressed) {
				PressLeft(m);
			}
			if (KeyRight != null && KeyRight.Pressed) {
				PressRight(m);
			}
		}

		public void PressUp(SelectMenu m) {
			m.Layout.SelectUp(m.Content, m.SelectionId, out m.SelectionId);
		}

		public void PressDown(SelectMenu m) {
			m.Layout.SelectDown(m.Content, m.SelectionId, out m.SelectionId);
		}

		public void PressLeft(SelectMenu m) {
			m.Layout.SelectLeft(m.Content, m.SelectionId, out m.SelectionId);
		}

		public void PressRight(SelectMenu m) {
			m.Layout.SelectRight(m.Content, m.SelectionId, out m.SelectionId);
		}

	}
}
