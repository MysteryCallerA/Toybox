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

		public void UpdateContentSize(List<MenuElement> content, Point contentContainerSize, out Point contentSize);
		public void UpdateContentPosition(List<MenuElement> content, MenuElement container);

		/// <summary> Attempts to adjust selection down. If possible, return true. </summary>
		public bool SelectDown(List<MenuElement> content, int selection, out int newSelection);

		/// <summary> Attempts to adjust selection up. If possible, return true. </summary>
		public bool SelectUp(List<MenuElement> content, int selection, out int newSelection);

		/// <summary> Attempts to adjust selection left. If possible, return true. </summary>
		public bool SelectLeft(List<MenuElement> content, int selection, out int newSelection);

		/// <summary> Attempts to adjust selection right. If possible, return true. </summary>
		public bool SelectRight(List<MenuElement> content, int selection, out int newSelection);

	}
}
