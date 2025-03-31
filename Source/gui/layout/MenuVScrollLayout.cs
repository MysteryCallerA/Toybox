using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;
using Toybox.gui.graphic;
using Toybox.utils.tween;

namespace Toybox.gui.layout {
	public class MenuVScrollLayout:MenuVerticalLayout {

		private int _Scroll = 0;

		public Tween ScrollTween;
		private int TweenStart;
		private int TweenEnd;
		private int TweenFrame;

		public VScrollbarGraphic Scrollbar;
		public int VisibleVertical { get; private set; }
		public int TotalVertical { get; private set; }

		public MenuVScrollLayout() {
			Scrollbar = new VScrollbarGraphic(this);
		}

		public override void UpdateContentSize(Point contentContainerSize, out Point contentSize) {
			if (Parent.Content.Count == 0) {
				contentSize = Point.Zero;
				return;
			}

			var bounds = new Point(contentContainerSize.X, 3);
			int hsize = 0;
			int vsize = Spacing * (Parent.Content.Count - 1);

			foreach (var e in Parent.Content) {
				e.UpdateSize(bounds);
				var esize = e.OuterSize;
				if (esize.X > hsize) hsize = esize.X;
				vsize += esize.Y;
			}

			contentSize = new Point(hsize, contentContainerSize.Y);

			if (Scrollbar != null) {
				VisibleVertical = contentSize.Y;
				TotalVertical = vsize;
				int scrollWidth = contentSize.X + Parent.MarginLeft + Parent.MarginRight + Parent.PaddingLeft + Parent.PaddingRight;
				int scrollHeight = contentSize.Y + Parent.MarginTop + Parent.MarginBottom + Parent.PaddingTop + Parent.PaddingBottom;
				Scrollbar.UpdateSize(new Point(scrollWidth, scrollHeight));
			}
		}

		public override void UpdateContentPosition() {
			if (Parent.Content.Count == 0) return;

			var bounds = Parent.ContentBounds;
			int y = bounds.Y - Scroll;

			foreach (var e in Parent.Content) {
				if (e.HAlign == MenuElement.HAlignType.Left) {
					e.Position = new Point(bounds.X, y);
				} else if (e.HAlign == MenuElement.HAlignType.Right) {
					e.Position = new Point(bounds.Right - e.OuterSize.X, y);
				} else if (e.HAlign == MenuElement.HAlignType.Center) {
					e.Position = new Point(bounds.Center.X - (e.OuterSize.X / 2), y);
				}
				y += e.OuterSize.Y + Spacing;
				e.UpdateContentPositions();
			}

			if (Scrollbar != null) {
				var totalBounds = Parent.OuterBounds;
				if (Scrollbar.HAlign == MenuElement.HAlignType.Left) {
					Scrollbar.Position = new Point(totalBounds.Left - Scrollbar.OuterSize.X, totalBounds.Y);
				} else {
					Scrollbar.Position = new Point(totalBounds.Right, totalBounds.Y);
				}
				Scrollbar.UpdateContentPositions();
			}
		}

		public override void DrawContent(Renderer r) {
			Scrollbar?.Draw(r);

			r.End();
			r.Begin(Parent.ContentBounds);
			base.DrawContent(r);
			r.End();
			r.Begin();
		}

		public override void SelectionChanged(MenuElement e) {
			var bounds = Parent.ContentBounds;
			var element = e.OuterBounds;
			if (element.Bottom > bounds.Bottom) {
				Scroll += element.Bottom - bounds.Bottom;
			} else if (element.Top < bounds.Top) {
				var next = Scroll - (bounds.Top - element.Top);
				if (next < 0) next = 0;
				Scroll = next;
			}
		}

		public int Scroll {
			get { return _Scroll; }
			set {
				if (ScrollTween != null) {
					if (TweenEnd == value) return;
					TweenStart = _Scroll;
					TweenEnd = value;
					TweenFrame = 0;
					return;
				}
				_Scroll = value;
			}
		}

		public override void UpdateState() {
			if (ScrollTween != null && TweenFrame < ScrollTween.Frames) {
				TweenFrame++;
				_Scroll = (int)(ScrollTween.Get(TweenFrame) * (TweenEnd - TweenStart)) + TweenStart;
			}
		}
	}
}
