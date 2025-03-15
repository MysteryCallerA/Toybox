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

		/// <summary> Attempts to adjust selection down. </summary>
		public void SelectDown(List<MenuElement> content, int selection, out int newSelection, out bool dirPossible, out bool wrappedAround);

		/// <summary> Attempts to adjust selection up. </summary>
		public void SelectUp(List<MenuElement> content, int selection, out int newSelection, out bool dirPossible, out bool wrappedAround);

		/// <summary> Attempts to adjust selection left. </summary>
		public void SelectLeft(List<MenuElement> content, int selection, out int newSelection, out bool dirPossible, out bool wrappedAround);

		/// <summary> Attempts to adjust selection right. </summary>
		public void SelectRight(List<MenuElement> content, int selection, out int newSelection, out bool dirPossible, out bool wrappedAround);

	}
}
