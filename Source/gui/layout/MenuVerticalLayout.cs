using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;

namespace Toybox.gui.layout {
	public class MenuVerticalLayout:IMenuLayout {

		public int Spacing = 0;

		public void UpdateContentSize(List<MenuElement> content, Point contentContainerSize, out Point contentSize) {
			if (content.Count == 0) {
				contentSize = Point.Zero;
				return;
			}

			var bounds = contentContainerSize;
			int vsize = Spacing * (content.Count - 1);
			int hsize = 0;
			int fitouterCount = 0;

			foreach (var e in content) {
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
			foreach (var e in content) {
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

		public void UpdateContentPosition(List<MenuElement> content, MenuElement container) {
			if (content.Count == 0) return;

			var bounds = container.ContentBounds;
			int y = bounds.Y;

			foreach (var e in content) {
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
			for (int i = content.Count - 1; i >= 0; i--) {
				var e = content[i];
				if (e.VAlign == MenuElement.VAlignType.Bottom) {
					e.Position = new Point(e.Position.X, y - e.OuterSize.Y);
				}
				e.UpdateContentPositions();
				y = e.Position.Y - Spacing;
			}
		}

		public void SelectDown(List<MenuElement> content, int selection, out int newSelection, out bool dirPossible, out bool wrappedAround) {
			newSelection = selection + 1;
			if (newSelection >= content.Count) {
				newSelection = 0;
				wrappedAround = true;
			} else {
				wrappedAround = false;
			}
			dirPossible = true;
		}

		public void SelectUp(List<MenuElement> content, int selection, out int newSelection, out bool dirPossible, out bool wrappedAround) {
			newSelection = selection - 1;
			if (newSelection < 0) {
				newSelection = content.Count - 1;
				wrappedAround = true;
			} else {
				wrappedAround = false;
			}
			dirPossible = true;
		}

		public void SelectLeft(List<MenuElement> content, int selection, out int newSelection, out bool dirPossible, out bool wrappedAround) {
			newSelection = selection;
			dirPossible = false;
			wrappedAround = false;
		}

		public void SelectRight(List<MenuElement> content, int selection, out int newSelection, out bool dirPossible, out bool wrappedAround) {
			newSelection = selection;
			dirPossible = false;
			wrappedAround = false;
		}
	}
}
