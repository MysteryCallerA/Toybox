﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;
using Toybox.utils.math;

namespace Toybox.gui.graphic {

	public class CheckboxGraphic:MenuElement {

		public bool State;
		public int BorderThickness = 1;
		public Color BoxColor = Color.White;
		public Color CheckColor = Color.White;

		public CheckboxGraphic(bool state) {
			Fit = FitType.Static;
			InnerSize = new Point(10, 10);
			State = state;
		}

		public override void Draw(Renderer r) {
			if (State) {
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

		protected internal override void UpdateFunction() {
		}

		protected override void GetContentSize(out Point contentSize) {
			contentSize = Point.Zero;
		}

		protected internal override void UpdateContainedElementPositions() {
		}

	}
}
