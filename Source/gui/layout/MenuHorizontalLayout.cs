using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;

namespace Toybox.gui.layout {
	public class MenuHorizontalLayout:IMenuLayout {

		public int Spacing = 0;

		public void UpdateContentSize(List<MenuElement> content, Point contentContainerSize, out Point contentSize) {
			if (content.Count == 0) {
				contentSize = Point.Zero;
				return;
			}

			var bounds = contentContainerSize;
			int hsize = Spacing * (content.Count - 1);
			int vsize = 0;
			int fitouterCount = 0;

			foreach (var e in content) {
				if (e.HFit == MenuElement.FitType.FillOuter) {
					fitouterCount++;
					continue;
				}
				e.UpdateSize(bounds);
				var esize = e.OuterSize;
				hsize += esize.X;
				if (esize.Y > vsize) vsize = esize.Y;
			}

			if (fitouterCount == 0) {
				contentSize = new Point(hsize, vsize);
				return;
			}

			bounds.X -= hsize;
			foreach (var e in content) {
				if (e.HFit != MenuElement.FitType.FillOuter) {
					continue;
				}
				e.UpdateSize(new Point(bounds.X / fitouterCount, bounds.Y));
				var esize = e.OuterSize;
				bounds.X -= esize.X;
				hsize += esize.X;
				if (esize.Y > vsize) vsize = esize.Y;
			}

			contentSize = new Point(hsize, vsize);
		}

		public void UpdateContentPosition(List<MenuElement> content, MenuElement container) {
			if (content.Count == 0) return;

			var bounds = container.ContentBounds;
			int x = bounds.X;

			foreach (var e in content) {
				if (e.VAlign == MenuElement.VAlignType.Top) {
					e.Position = new Point(x, bounds.Top);
				} else if (e.VAlign == MenuElement.VAlignType.Bottom) {
					e.Position = new Point(x, bounds.Bottom - e.OuterSize.Y);
				} else if (e.VAlign == MenuElement.VAlignType.Center) {
					e.Position = new Point(x, bounds.Center.Y - (e.OuterSize.Y / 2));
				}
				x += e.OuterSize.X + Spacing;
			}

			x = bounds.Right;
			for (int i = content.Count - 1; i >= 0; i--) {
				var e = content[i];
				if (e.HAlign == MenuElement.HAlignType.Right) {
					e.Position = new Point(x - e.OuterSize.X, e.Position.Y);
				}
				e.UpdateContentPositions();
				x = e.Position.X - Spacing;
			}
		}

		public void SelectDown(List<MenuElement> content, int selection, out int newSelection, out bool dirPossible, out bool wrappedAround) {
			newSelection = selection;
			dirPossible = false;
			wrappedAround = false;
		}

		public void SelectUp(List<MenuElement> content, int selection, out int newSelection, out bool dirPossible, out bool wrappedAround) {
			newSelection = selection;
			dirPossible = false;
			wrappedAround = false;
		}

		public void SelectLeft(List<MenuElement> content, int selection, out int newSelection, out bool dirPossible, out bool wrappedAround) {
			newSelection = selection + 1;
			if (newSelection >= content.Count) {
				newSelection = 0;
				wrappedAround = true;
			} else {
				wrappedAround = false;
			}
			dirPossible = true;
		}

		public void SelectRight(List<MenuElement> content, int selection, out int newSelection, out bool dirPossible, out bool wrappedAround) {
			newSelection = selection - 1;
			if (newSelection < 0) {
				newSelection = content.Count - 1;
				wrappedAround = true;
			} else {
				wrappedAround = false;
			}
			dirPossible = true;
		}
	}
}
