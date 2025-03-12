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
		private MenuElement _BackGraphic;
		private MenuElement _FrontGraphic;
		public MenuControl LeftKey = MenuControl.Left;
		public MenuControl RightKey = MenuControl.Right;

		public MenuValueBar() {
			Fit = FitType.Static;
			VAlign = VAlignType.Center;
			TargetInnerSize = new Point(50, 5);
			Padding = 1;

			BackGraphic = new MenuBoxGraphic() { Fit = FitType.FillOuter, BackColor = Color.DarkGray };
			FrontGraphic = new MenuBoxGraphic() { Fit = FitType.FillOuter, BackColor = Color.White };
		}

		public override void Draw(Renderer r) {
			BackGraphic?.Draw(r);
			FrontGraphic?.Draw(r);
		}

		protected internal override void UpdateFunction(MenuControlManager c, MenuSystem system, MenuStack stack) {
			if (c == null) return;
			if (c.TryGet(LeftKey, out var left)) {
				if (left.Pressed) {
					CurrentValue -= StepSize;
					if (CurrentValue < MinValue) CurrentValue = MinValue;
					left.DropPress();
				}
			}
			if (c.TryGet(RightKey, out var right)) {
				if (right.Pressed) {
					CurrentValue += StepSize;
					if (CurrentValue > MaxValue) CurrentValue = MaxValue;
					right.DropPress();
				}
			}
		}

		protected internal override void UpdateState() {
			base.UpdateState();
			BackGraphic?.UpdateState();
			FrontGraphic?.UpdateState();
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

		protected internal override void UpdateContentPositions() {
			if (BackGraphic != null) {
				BackGraphic.Position = PanelOrigin;
				BackGraphic.UpdateContentPositions();
			}
			if (FrontGraphic != null) {
				FrontGraphic.Position = ContentOrigin;
				FrontGraphic.UpdateContentPositions();
			}
		}

		public override void Cascade(Action<MenuElement> a) {
			base.Cascade(a);
			BackGraphic.Cascade(a);
			FrontGraphic.Cascade(a);
		}

		public MenuElement FrontGraphic {
			get { return _FrontGraphic; }
			set {
				_FrontGraphic = value;
				_FrontGraphic.Parent = this;
			}
		}

		public MenuElement BackGraphic {
			get { return _BackGraphic; }
			set {
				_BackGraphic = value;
				_BackGraphic.Parent = this;
			}
		}
	}
}
