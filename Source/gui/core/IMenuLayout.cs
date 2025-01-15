using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.gui.core {
	public interface IMenuLayout {

		public void UpdateContentSize(List<MenuElement> content, MenuElement container, out Point contentSize);
		public void UpdateContentPosition(List<MenuElement> content, MenuElement container);

	}
}
