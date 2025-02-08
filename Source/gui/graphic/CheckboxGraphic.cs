using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;
using Toybox.utils.math;

namespace Toybox.gui.graphic {

	public class CheckboxGraphic:MenuElement {

		public const string TypeName = "CheckboxGraphic";
		public bool CheckedState;
		public int BorderThickness = 1;
		public Color BoxColor = Color.White;
		public Color CheckColor = Color.White;

		public CheckboxGraphic(bool state) {
			Fit = FitType.Static;
			TargetInnerSize = new Point(10, 10);
			CheckedState = state;
		}

		public override void Draw(Renderer r) {
			if (CheckedState) {
				DrawTrue(r);
			} else {
				DrawFalse(r);
			}
		}

		protected virtual void DrawFalse(Renderer r) {
			DrawBox(r, ContentBounds, BorderThickness);
		}

		protected virtual void DrawTrue(Renderer r) {
			DrawBox(r, ContentBounds, BorderThickness);
			DrawX(r, ContentBounds);
		}

		protected void DrawBox(Renderer r, Rectangle bounds, int thickness) {
			MathOps.GetEdges(bounds, thickness, out var top, out var bot, out var left, out var right);
			r.DrawRectStatic(top, BoxColor);
			r.DrawRectStatic(bot, BoxColor);
			r.DrawRectStatic(left, BoxColor);
			r.DrawRectStatic(right, BoxColor);
		}

		protected void DrawX(Renderer r, Rectangle bounds) {
			bounds.Inflate(-(BorderThickness + 1), -(BorderThickness + 1));
			bounds.X += 1;
			r.DrawLineStatic(new Vector2(bounds.X, bounds.Y), new Vector2(bounds.Right, bounds.Bottom), CheckColor);
			r.DrawLineStatic(new Vector2(bounds.Right, bounds.Y), new Vector2(bounds.X, bounds.Bottom), CheckColor);
		}

		protected internal override void UpdateFunction(MenuControls c) {
		}

		protected override void UpdateContentSize(Point contentContainerSize, out Point contentSize) {
			contentSize = Point.Zero;
		}

		protected internal override void UpdateContainedElementPositions() {
		}

		public override string GetTypeName() {
			return TypeName;
		}
	}
}
