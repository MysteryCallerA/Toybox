using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.utils.input;

namespace Toybox.gui.core {
	public class MenuControlManager {

		private readonly Dictionary<MenuControl, VirtualKey> Controls = new();

		public void Set(MenuControl id, VirtualKey key) {
			if (Controls.ContainsKey(id)) {
				Controls[id] = key;
			} else {
				Controls.Add(id, key);
			}
		}

		public bool TryGet(MenuControl id, out VirtualKey key) {
			return Controls.TryGetValue(id, out key);
		}


	}
}
