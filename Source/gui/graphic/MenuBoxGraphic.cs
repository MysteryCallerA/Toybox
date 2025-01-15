﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;
using Toybox.utils.math;

namespace Toybox.gui.graphic {
	public class MenuBoxGraphic:MenuElement {

		public Color BackColor = Color.Gray;
		public Color BorderColor = Color.Transparent;
		public int BorderSize = 0;

		public MenuBoxGraphic(Point size) {
			InnerSize = size;
			Fit = FitType.Static;
		}

		public MenuBoxGraphic() {
			Fit = FitType.FitOuter;
		}

		public override void Draw(Renderer r) {
			var bounds = ContentBounds;

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

		protected internal override void UpdateFunction() {
		}

		protected internal override void UpdateContainedElementPositions() {
		}

		protected override void GetContentSize(out Point contentSize) {
			contentSize = Point.Zero;
		}
	}
}