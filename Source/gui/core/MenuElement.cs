using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.style;

namespace Toybox.gui.core {
	public abstract class MenuElement {

		public bool Selectable = false;
		protected internal MenuSystem ParentSystem;
		protected internal MenuElement Parent;

		public StyleGroup Styles;
		public MenuStateManager State;
		public MenuTweenManager Tweens = new();

		/// <summary> Is set automatically during UpdateSize. Used for relative positioning of elements. </summary>
		public Point OuterSize { get; private set; }
		/// <summary> Is automatically set during UpdateSize. The size inner elements should conform to. </summary>
		public Point InnerSize { get; private set; }
		/// <summary> Is automatically set by container. Represents absolute outer position. </summary>
		public Point Position;
		/// <summary> Only used when Fit is Static. </summary>
		public Point TargetInnerSize;

		public enum FitType { Static, FitContent, FillOuter }
		public FitType HFit = FitType.FitContent;
		public FitType VFit = FitType.FitContent;

		public int MarginLeft, MarginRight, MarginTop, MarginBottom;
		public int PaddingLeft, PaddingRight, PaddingTop, PaddingBottom;
		public int OverflowLeft, OverflowRight, OverflowTop, OverflowBottom;
		public int XOffset, YOffset;

		public enum VAlignType { Top, Bottom, Center }
		public enum HAlignType { Left, Right, Center }
		public VAlignType VAlign = VAlignType.Top;
		public HAlignType HAlign = HAlignType.Left;

		public MenuElement() {
			State = new MenuStateManager(this);
		}

		public virtual void Draw(Renderer r) {
		}

		/// <summary> Anything the element does, goes here. Call UpdateFunction on any contained elements. </summary>
		protected internal virtual void UpdateFunction(MenuControlManager c, MenuSystem parent) { }

		/// <summary> Always called once per frame. Call base.UpdateState() and then UpdateState() on any contained elements.
		/// <br></br> Used for updating Tweens and MenuState.</summary>
		protected internal virtual void UpdateState() {
			Tweens.UpdateTweens(this);
		}

		protected internal void UpdateSize(Point containerSize) {
			if (HFit == FitType.FillOuter) {
				OuterSize = new Point(containerSize.X, OuterSize.Y);
				InnerSize = new Point(containerSize.X - HWhitespace + OverflowLeft + OverflowRight, InnerSize.Y);
			} else if (HFit == FitType.Static) {
				OuterSize = new Point(TargetInnerSize.X + HWhitespace, OuterSize.Y);
				InnerSize = new Point(TargetInnerSize.X + OverflowLeft + OverflowRight, InnerSize.Y);
			}
			if (VFit == FitType.FillOuter) {
				OuterSize = new Point(OuterSize.X, containerSize.Y);
				InnerSize = new Point(InnerSize.X, containerSize.Y - VWhitespace + OverflowTop + OverflowBottom);
			} else if (VFit == FitType.Static) {
				OuterSize = new Point(OuterSize.X, TargetInnerSize.Y + VWhitespace);
				InnerSize = new Point(InnerSize.X, TargetInnerSize.Y + OverflowTop + OverflowBottom);
			}

			UpdateContentSize(Point.Zero, out Point contentSize);

			if (HFit == FitType.FitContent || (HFit == FitType.FillOuter && contentSize.X > InnerSize.X)) {
				OuterSize = new Point(contentSize.X + HWhitespace, OuterSize.Y);
				InnerSize = new Point(contentSize.X + OverflowLeft + OverflowRight, InnerSize.Y);
			}
			if (VFit == FitType.FitContent || (VFit == FitType.FillOuter && contentSize.Y > InnerSize.Y)) {
				OuterSize = new Point(OuterSize.X, contentSize.Y + VWhitespace);
				InnerSize = new Point(InnerSize.X, contentSize.Y + OverflowTop + OverflowBottom);
			}
			UpdateContentSize(InnerSize, out _);
		}

		/// <summary> Call UpdateSize(contentContainerSize) on any contained MenuElements.<br>
		/// </br> Output the total minimum size of all content. <br>
		/// </br> If the contained elements can be any size (eg. resizable graphics), output contentSize can be zero.<br>
		/// </br> This is called twice when updating size. First time contentContainerSize = Point.Zero.<br>
		/// </br> On the first call, you should treat FillOuter as FitContent.</summary>
		protected abstract void UpdateContentSize(Point contentContainerSize, out Point contentSize);

		/// <summary> Set Position of any contained elements based on their alignment settings. Call UpdateContainedElementPositions on each element. </summary>
		protected internal abstract void UpdateContentPositions();

		public Point ContentOrigin {
			get { return new Point(Position.X + MarginLeft + PaddingLeft - OverflowLeft + XOffset, Position.Y + MarginTop + PaddingTop - OverflowTop + YOffset); }
		}
		public Rectangle ContentBounds {
			get { return new Rectangle(ContentOrigin, InnerSize); }
		}
		public Point PanelOrigin {
			get { return new Point(Position.X + MarginLeft - OverflowLeft + XOffset, Position.Y + MarginTop - OverflowTop + YOffset); }
		}
		public Point PanelSize {
			get { return new Point(InnerSize.X + PaddingLeft + PaddingRight, InnerSize.Y + PaddingTop + PaddingBottom); }
		}
		public Rectangle PanelBounds {
			get { return new Rectangle(PanelOrigin, PanelSize); }
		}

		// -------- Multi-Properties --------
		public FitType Fit { set { HFit = value; VFit = value; } }

		public int HMargin { set { MarginLeft = value; MarginRight = value; } }
		public int VMargin { set { MarginTop = value; MarginBottom = value; } }
		public int Margin { set { MarginLeft = value; MarginRight = value; MarginTop = value; MarginBottom = value; } }

		public int HPadding { set { PaddingLeft = value; PaddingRight = value; } }
		public int VPadding { set { PaddingTop = value; PaddingBottom = value; } }
		public int Padding { set { PaddingLeft = value; PaddingRight = value; PaddingTop = value; PaddingBottom = value; } }

		private int HWhitespace { get { return PaddingLeft + PaddingRight + MarginLeft + MarginRight; } }
		private int VWhitespace { get { return PaddingTop + PaddingBottom + MarginTop + MarginBottom; } }

		public int HOverflow { set { OverflowLeft = value; OverflowRight = value; } }
		public int VOverflow { set { OverflowTop = value; OverflowBottom = value; } }
		public int Overflow { set { OverflowLeft = value; OverflowRight = value; OverflowTop = value; OverflowBottom = value; } }
		//------------------------------------

		protected internal void UpdateStyle() {
			if (Styles == null) return;
			Styles.UpdateStyle(this);
		}

		public virtual void ApplyStyleValue(StyleField f, int v) {
			if (f.Equals(StyleField.OffsetX)) { XOffset = v; return; }
			if (f.Equals(StyleField.OffsetY)) { YOffset = v; return; }

			if (f.Equals(StyleField.Overflow)) { Overflow = v; return; }
			if (f.Equals(StyleField.OverflowLeft)) { OverflowLeft = v; return; }
			if (f.Equals(StyleField.OverflowRight)) { OverflowRight = v; return; }
			if (f.Equals(StyleField.OverflowTop)) { OverflowTop = v; return; }
			if (f.Equals(StyleField.OverflowBottom)) { OverflowBottom = v; return; }

			if (f.Equals(StyleField.Padding)) { Padding = v; return; }
			if (f.Equals(StyleField.PaddingLeft)) { PaddingLeft = v; return; }
			if (f.Equals(StyleField.PaddingRight)) { PaddingRight = v; return; }
			if (f.Equals(StyleField.PaddingTop)) { PaddingTop = v; return; }
			if (f.Equals(StyleField.PaddingBottom)) { PaddingBottom = v; return; }

			if (f.Equals(StyleField.Margin)) { Margin = v; return; }
			if (f.Equals(StyleField.MarginLeft)) { MarginLeft = v; return; }
			if (f.Equals(StyleField.MarginRight)) { MarginRight = v; return; }
			if (f.Equals(StyleField.MarginTop)) { MarginTop = v; return; }
			if (f.Equals(StyleField.MarginBottom)) { MarginBottom = v; return; }
		}
		public virtual void ApplyStyleValue(ColorField f, Color c) { }

		public virtual int GetStyleValue(StyleField f) {
			if (f.Equals(StyleField.OffsetX)) { return XOffset; }
			if (f.Equals(StyleField.OffsetY)) { return YOffset; }

			if (f.Equals(StyleField.Overflow)) { return OverflowLeft; }
			if (f.Equals(StyleField.OverflowLeft)) { return OverflowLeft; }
			if (f.Equals(StyleField.OverflowRight)) { return OverflowRight; }
			if (f.Equals(StyleField.OverflowTop)) { return OverflowTop; }
			if (f.Equals(StyleField.OverflowBottom)) { return OverflowBottom; }

			if (f.Equals(StyleField.Padding)) { return PaddingLeft; }
			if (f.Equals(StyleField.PaddingLeft)) { return PaddingLeft; }
			if (f.Equals(StyleField.PaddingRight)) { return PaddingRight; }
			if (f.Equals(StyleField.PaddingTop)) { return PaddingTop; }
			if (f.Equals(StyleField.PaddingBottom)) { return PaddingBottom; }

			if (f.Equals(StyleField.Margin)) { return MarginLeft; }
			if (f.Equals(StyleField.MarginLeft)) { return MarginLeft; }
			if (f.Equals(StyleField.MarginRight)) { return MarginRight; }
			if (f.Equals(StyleField.MarginTop)) { return MarginTop; }
			if (f.Equals(StyleField.MarginBottom)) { return MarginBottom; }
			return 0;
		}

		public virtual void Cascade(Action<MenuElement> a) {
			a.Invoke(this);
		}

	}
}
