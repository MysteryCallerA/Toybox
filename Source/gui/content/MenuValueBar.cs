using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;
using Toybox.gui.graphic;

namespace Toybox.gui.content {
	public class MenuValueBar:MenuElement {

		public float CurrentValue = 0.5f;
		public float MaxValue = 1;
		public float MinValue = 0;
		public float StepSize = 0.1f;
		public MenuElement BackGraphic;
		public MenuElement FrontGraphic;

		public MenuValueBar() {
			Fit = FitType.Static;
			VAlign = VAlignType.Center;
			InnerSize = new Point(50, 5);
			Padding = 1;

			BackGraphic = new MenuBoxGraphic() { Fit = FitType.FillOuter, BackColor = Color.DarkGray };
			FrontGraphic = new MenuBoxGraphic() { Fit = FitType.FillOuter, BackColor = Color.White };
		}

		public override void Draw(Renderer r) {
			BackGraphic?.Draw(r);
			FrontGraphic?.Draw(r);
		}

		protected internal override void UpdateFunction(MenuControls c) {
			c = Controls ?? c;
			if (c.KeyLeft.Pressed) {
				CurrentValue -= StepSize;
				if (CurrentValue < MinValue) CurrentValue = MinValue;
				c.KeyLeft.DropPress();
			}
			if (c.KeyRight.Pressed) {
				CurrentValue += StepSize;
				if (CurrentValue > MaxValue) CurrentValue = MaxValue;
				c.KeyRight.DropPress();
			}
		}

		protected override void UpdateContentSize(Point contentContainerSize, out Point contentSize) {
			if (BackGraphic != null) {
				BackGraphic.UpdateSize(PanelSize);
			}
			if (FrontGraphic != null) {
				int barwidth = (int)(contentContainerSize.X * ((CurrentValue - MinValue) / (MaxValue - MinValue)));
				FrontGraphic.UpdateSize(new Point(barwidth, contentContainerSize.Y));
			}
			contentSize = Point.Zero;
		}

		protected internal override void UpdateContainedElementPositions() {
			if (BackGraphic != null) {
				BackGraphic.Position = PanelOrigin;
				BackGraphic.UpdateContainedElementPositions();
			}
			if (FrontGraphic != null) {
				FrontGraphic.Position = ContentOrigin;
				FrontGraphic.UpdateContainedElementPositions();
			}
		}
	}
}
