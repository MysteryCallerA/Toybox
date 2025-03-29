using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;

namespace Toybox.gui.layout {
	public class MenuVerticalLayout:MenuLayout {

		public int Spacing = 0;

		public override void UpdateContentSize(Point contentContainerSize, out Point contentSize) {
			if (Parent.Content.Count == 0) {
				contentSize = Point.Zero;
				return;
			}

			var bounds = contentContainerSize;
			int vsize = Spacing * (Parent.Content.Count - 1);
			int hsize = 0;
			int fitouterCount = 0;

			foreach (var e in Parent.Content) {
				if (e.VFit == MenuElement.FitType.FillOuter) {
					fitouterCount++;
					continue;
				}
				e.UpdateSize(bounds);
				var esize = e.OuterSize;
				vsize += esize.Y;
				if (esize.X > hsize) hsize = esize.X;
			}

			if (fitouterCount == 0) {
				contentSize = new Point(hsize, vsize);
				return;
			}

			bounds.Y -= vsize;
			foreach (var e in Parent.Content) {
				if (e.VFit != MenuElement.FitType.FillOuter) {
					continue;
				}
				e.UpdateSize(new Point(bounds.X, bounds.Y / fitouterCount));
				var esize = e.OuterSize;
				bounds.Y -= esize.Y;
				vsize += esize.Y;
				if (esize.X > hsize) hsize = esize.X;
			}

			contentSize = new Point(hsize, vsize);
		}

		public override void UpdateContentPosition() {
			if (Parent.Content.Count == 0) return;

			var bounds = Parent.ContentBounds;
			int y = bounds.Y;

			foreach (var e in Parent.Content) {
				if (e.HAlign == MenuElement.HAlignType.Left) {
					e.Position = new Point(bounds.X, y);
				} else if (e.HAlign == MenuElement.HAlignType.Right) {
					e.Position = new Point(bounds.Right - e.OuterSize.X, y);
				} else if (e.HAlign == MenuElement.HAlignType.Center) {
					e.Position = new Point(bounds.Center.X - (e.OuterSize.X / 2), y);
				}
				y += e.OuterSize.Y + Spacing;
			}

			y = bounds.Bottom;
			for (int i = Parent.Content.Count - 1; i >= 0; i--) {
				var e = Parent.Content[i];
				if (e.VAlign == MenuElement.VAlignType.Bottom) {
					e.Position = new Point(e.Position.X, y - e.OuterSize.Y);
				}
				e.UpdateContentPositions();
				y = e.Position.Y - Spacing;
			}
		}

		public override void SelectDown(int selection, out int newSelection, out bool dirPossible, out bool wrappedAround) {
			newSelection = selection + 1;
			if (newSelection >= Parent.Content.Count) {
				newSelection = 0;
				wrappedAround = true;
			} else {
				wrappedAround = false;
			}
			dirPossible = true;
		}

		public override void SelectUp(int selection, out int newSelection, out bool dirPossible, out bool wrappedAround) {
			newSelection = selection - 1;
			if (newSelection < 0) {
				newSelection = Parent.Content.Count - 1;
				wrappedAround = true;
			} else {
				wrappedAround = false;
			}
			dirPossible = true;
		}

		public override void SelectLeft(int selection, out int newSelection, out bool dirPossible, out bool wrappedAround) {
			newSelection = selection;
			dirPossible = false;
			wrappedAround = false;
		}

		public override void SelectRight(int selection, out int newSelection, out bool dirPossible, out bool wrappedAround) {
			newSelection = selection;
			dirPossible = false;
			wrappedAround = false;
		}
	}
}
