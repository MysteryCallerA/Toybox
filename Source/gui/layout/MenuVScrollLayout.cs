using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;

namespace Toybox.gui.layout {
	public class MenuVScrollLayout:MenuVerticalLayout {

		public int Scroll = 0;

		public override void UpdateContentSize(Point contentContainerSize, out Point contentSize) {
			if (Parent.Content.Count == 0) {
				contentSize = Point.Zero;
				return;
			}

			var bounds = new Point(contentContainerSize.X, 3);
			int hsize = 0;

			foreach (var e in Parent.Content) {
				e.UpdateSize(bounds);
				var esize = e.OuterSize;
				if (esize.X > hsize) hsize = esize.X;
			}

			contentSize = new Point(hsize, contentContainerSize.Y);
		}

		public override void UpdateContentPosition() {
			if (Parent.Content.Count == 0) return;

			var bounds = Parent.ContentBounds;
			int y = bounds.Y - Scroll;

			foreach (var e in Parent.Content) {
				if (e.HAlign == MenuElement.HAlignType.Left) {
					e.Position = new Point(bounds.X, y);
				} else if (e.HAlign == MenuElement.HAlignType.Right) {
					e.Position = new Point(bounds.Right - e.OuterSize.X, y);
				} else if (e.HAlign == MenuElement.HAlignType.Center) {
					e.Position = new Point(bounds.Center.X - (e.OuterSize.X / 2), y);
				}
				y += e.OuterSize.Y + Spacing;
				e.UpdateContentPositions();
			}
		}

		public override void DrawContent(Renderer r) {
			r.End();
			r.Begin(Parent.ContentBounds);
			base.DrawContent(r);
			r.End();
			r.Begin();
		}

		public override void SelectionChanged(MenuElement e) {
			var bounds = Parent.ContentBounds;
			var element = e.OuterBounds;
			if (element.Bottom > bounds.Bottom) {
				Scroll += element.Bottom - bounds.Bottom;
			} else if (element.Top < bounds.Top) {
				Scroll -= bounds.Top - element.Top;
			}
		}
	}
}
