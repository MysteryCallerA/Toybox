using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.utils;

namespace Toybox.gui.core {
	public class MenuSelector {

		public MenuElement Pointer;
		public Vector2 PointerAttachSide = Vector2.Zero;
		public Point PointerOffset = Point.Zero;

		public MenuElement Highlighter;

		private bool NoSelection = true;

		public MenuSelector() {
		}

		public void Draw(Renderer r) {
			if (NoSelection) return;

			Pointer?.Draw(r);
			Highlighter?.Draw(r);
		}

		public void Update(MenuElement selection) {
			if (selection == null) {
				NoSelection = true;
				return;
			}
			NoSelection = false;

			if (Pointer != null) {
				var selectionBounds = new Rectangle(selection.Position, selection.TotalSize);
				var pos = new Point(selectionBounds.X + (int)(selectionBounds.Width * PointerAttachSide.X), selectionBounds.Y + (int)(selectionBounds.Height * PointerAttachSide.Y));
				pos += PointerOffset;
				Pointer.Position = pos;
				Pointer.Update();
			}

			if (Highlighter != null) {
				Highlighter.Position = selection.ContentOrigin;
				Highlighter.UpdateSize(selection.InnerSize);
				Highlighter.UpdateContainedElementPositions();
			}
		}

	}
}
