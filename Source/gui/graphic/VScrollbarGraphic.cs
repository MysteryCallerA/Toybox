using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;
using Toybox.gui.layout;

namespace Toybox.gui.graphic {
	public class VScrollbarGraphic:MenuElement {

		public MenuElement FrontGraphic;
		public MenuElement BackGraphic;
		private readonly MenuVScrollLayout Source;

		private bool ScrollbarNeeded;

		public VScrollbarGraphic(MenuVScrollLayout parent) {
			BackGraphic = new MenuBoxGraphic() { BackColor = Color.DarkGray };
			FrontGraphic = new MenuBoxGraphic() { BackColor = Color.White };
			Source = parent;
			Setup();
		}

		public VScrollbarGraphic(MenuVScrollLayout parent, MenuElement back, MenuElement front) {
			Source = parent;
			BackGraphic = back;
			FrontGraphic = front;
			Setup();
		}

		private void Setup() {
			HFit = FitType.Static;
			VFit = FitType.FillOuter;
			TargetInnerSize = new Point(5, 0);
			MarginLeft = 1;
			HAlign = HAlignType.Right;
		}

		protected override void UpdateContentSize(Point contentContainerSize, out Point contentSize) {
			BackGraphic.UpdateSize(contentContainerSize);

			var innerRatio = (float)Source.VisibleVertical / Source.TotalVertical;
			if (innerRatio >= 1) {
				innerRatio = 1;
				ScrollbarNeeded = false;
			} else {
				ScrollbarNeeded = true;
			}
			FrontGraphic.UpdateSize(new Point(contentContainerSize.X, (int)(innerRatio * contentContainerSize.Y) + 1));
			contentSize = Point.Zero;
		}

		protected internal override void UpdateContentPositions() {
			BackGraphic.Position = ContentOrigin;

			var posRatio = (float)Source.Scroll / Source.TotalVertical;
			FrontGraphic.Position = new Point(BackGraphic.Position.X, (int)(posRatio * BackGraphic.InnerSize.Y) + BackGraphic.Position.Y);

			BackGraphic.UpdateContentPositions();
			FrontGraphic.UpdateContentPositions();
		}

		public override void Draw(Renderer r) {
			if (!ScrollbarNeeded) return;

			BackGraphic.Draw(r);
			FrontGraphic.Draw(r);
		}
	}
}
