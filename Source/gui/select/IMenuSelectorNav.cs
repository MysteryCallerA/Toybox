using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.gui.select {
	public interface IMenuSelectorNav {

		public void SelectUp(MenuSelector parent, out bool inputWasPossible);
		public void SelectDown(MenuSelector parent, out bool inputWasPossible);
		public void SelectLeft(MenuSelector parent, out bool inputWasPossible);
		public void SelectRight(MenuSelector parent, out bool inputWasPossible);

	}
}
