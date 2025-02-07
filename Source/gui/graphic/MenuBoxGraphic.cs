using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;
using Toybox.utils.math;

namespace Toybox.gui.graphic
{
	public class MenuBoxGraphic:MenuElement {

		public const string TypeName = "BoxGraphic";
		public Color BackColor = Color.Gray;
		public Color BorderColor = Color.Transparent;
		public int BorderSize = 0;
		public int OverflowLeft, OverflowRight, OverflowTop, OverflowBottom;

		public MenuBoxGraphic(Point size) {
			InnerSize = size;
			Fit = FitType.Static;
		}

		public MenuBoxGraphic() {
			Fit = FitType.FillOuter;
		}

		public override void Draw(Renderer r) {
			var pos = ContentOrigin;
			pos = new Point(pos.X - OverflowLeft, pos.Y - OverflowTop);
			var size = new Point(InnerSize.X + OverflowLeft + OverflowRight, InnerSize.Y + OverflowTop + OverflowBottom);
			var bounds = new Rectangle(pos, size);

			var back = new Rectangle(bounds.X + BorderSize, bounds.Y + BorderSize, bounds.Width - BorderSize * 2, bounds.Height - BorderSize * 2);
			r.DrawRectStatic(back, BackColor);

			if (BorderSize > 0 && BorderColor != Color.Transparent) {
				MathOps.GetEdges(bounds, BorderSize, out var top, out var bot, out var left, out var right);
				r.DrawRectStatic(top, BorderColor);
				r.DrawRectStatic(bot, BorderColor);
				r.DrawRectStatic(left, BorderColor);
				r.DrawRectStatic(right, BorderColor);
			}
		}

		protected internal override void UpdateFunction(MenuControls c) {
		}

		protected internal override void UpdateContainedElementPositions() {
		}

		protected override void UpdateContentSize(Point contentContainerSize, out Point contentSize) {
			contentSize = Point.Zero;
		}

		public override string GetTypeName() {
			return TypeName;
		}

		public override void Cascade(Action<MenuElement> a) {
		}

		public int HOverflow {
			set { OverflowLeft = value; OverflowRight = value; }
		}
		public int VOverflow {
			set { OverflowTop = value; OverflowBottom = value; }
		}
		public int Overflow {
			set { OverflowLeft = OverflowRight = OverflowBottom = OverflowTop = value; }
		}

	}
}
