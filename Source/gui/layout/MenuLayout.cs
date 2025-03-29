using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;

namespace Toybox.gui.layout
{
	public abstract class MenuLayout {

		protected internal MenuBox Parent;

		public abstract void UpdateContentSize(Point contentContainerSize, out Point contentSize);
		public abstract void UpdateContentPosition();

		/// <summary> Attempts to adjust selection down. </summary>
		public abstract void SelectDown(int selection, out int newSelection, out bool dirPossible, out bool wrappedAround);

		/// <summary> Attempts to adjust selection up. </summary>
		public abstract void SelectUp(int selection, out int newSelection, out bool dirPossible, out bool wrappedAround);

		/// <summary> Attempts to adjust selection left. </summary>
		public abstract void SelectLeft(int selection, out int newSelection, out bool dirPossible, out bool wrappedAround);

		/// <summary> Attempts to adjust selection right. </summary>
		public abstract void SelectRight(int selection, out int newSelection, out bool dirPossible, out bool wrappedAround);

		public virtual void DrawContent(Renderer r) {
			foreach (var e in Parent.Content) {
				e.Draw(r);
			}
		}

		public virtual void SelectionChanged(MenuElement e) {
		}

	}
}
