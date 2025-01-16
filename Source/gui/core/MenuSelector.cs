using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.utils;

namespace Toybox.gui.core {
	public class MenuSelector {

		public MenuElement Content;
		public Point Origin;

		/// <summary> (0,0) = TopLeft, (1,1) = BottomRight </summary>
		public Vector2 AttachSide = Vector2.Zero;

		private bool NoSelection = true;

		public MenuSelector() {
		}

		public void Draw(Renderer r) {
			if (Content == null) return;
			if (NoSelection) return;
			Content.Draw(r);
		}

		public void Update(MenuElement selection) {
			if (Content == null) return;

			if (selection == null) {
				NoSelection = true;
				return;
			}
			NoSelection = false;

			var bounds = new Rectangle(selection.Position, selection.TotalSize);
			var pos = new Point(bounds.X + (int)(bounds.Width * AttachSide.X), bounds.Y + (int)(bounds.Height * AttachSide.Y));
			pos -= Origin;
			Content.Position = pos;

			Content.Update();
		}

	}
}
