using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.gui.core {
	public class InteractiveMenu:MenuBox {

		protected MenuSystem ParentSystem;

		internal void Update(MenuSystem parent) {
			ParentSystem = parent;
			Update();
		}

		/// <summary> Called automatically when added to a MenuSystem. </summary>
		public virtual void InheritSystemProps(MenuSystem m) {
		}

	}
}
