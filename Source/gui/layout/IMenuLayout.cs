using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;

namespace Toybox.gui.layout
{
    public interface IMenuLayout {

		public void UpdateContentSize(List<MenuElement> content, MenuElement container, out Point contentSize);
		public void UpdateContentPosition(List<MenuElement> content, MenuElement container);

		public void SelectDown(List<MenuElement> content, int selection, out int newSelection);
		public void SelectUp(List<MenuElement> content, int selection, out int newSelection);
		public void SelectLeft(List<MenuElement> content, int selection, out int newSelection);
		public void SelectRight(List<MenuElement> content, int selection, out int newSelection);

	}
}
