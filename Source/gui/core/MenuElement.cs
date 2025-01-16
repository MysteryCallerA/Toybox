using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.gui.core {
	public abstract class MenuElement {

		public bool Selectable = false;

		public Point TotalSize { get; private set; }
		public Point InnerSize;
		public Point Position;

		public enum FitType { Static, FitContent, FitOuter }
		public FitType HFit = FitType.FitContent;
		public FitType VFit = FitType.FitContent;

		public int MarginLeft, MarginRight, MarginTop, MarginBottom;
		public int PaddingLeft, PaddingRight, PaddingTop, PaddingBottom;

		public enum VAlignType { Top, Bottom, Center }
		public enum HAlignType { Left, Right, Center }
		public VAlignType VAlign = VAlignType.Top;
		public HAlignType HAlign = HAlignType.Left;

		/// <summary> Call Update on only the outermost element to update the entire menu structure. </summary>
		public virtual void Update() {
			UpdateFunction();
			UpdateSize(Resources.Camera.Bounds.Size);
			UpdateContainedElementPositions();
		}

		public virtual void Draw(Renderer r) {
		}

		/// <summary> Anything the element does, goes here. Call UpdateFunction on any contained elements. </summary>
		protected internal abstract void UpdateFunction();

		protected internal void UpdateSize(Point containerSize) {
			if (HFit == FitType.FitOuter) {
				TotalSize = new Point(containerSize.X, TotalSize.Y);
				InnerSize.X = containerSize.X - HWhitespace;
			} else if (HFit == FitType.Static) {
				TotalSize = new Point(InnerSize.X + HWhitespace, TotalSize.Y);
			}
			if (VFit == FitType.FitOuter) {
				TotalSize = new Point(TotalSize.X, containerSize.Y);
				InnerSize.Y = containerSize.Y - VWhitespace;
			} else if (VFit == FitType.Static) {
				TotalSize = new Point(TotalSize.X, InnerSize.Y + VWhitespace);
			}

			GetContentSize(out Point contentSize);

			if (HFit == FitType.FitContent) {
				TotalSize = new Point(contentSize.X + HWhitespace, TotalSize.Y);
				InnerSize.X = contentSize.X;
			}
			if (VFit == FitType.FitContent) {
				TotalSize = new Point(TotalSize.X, contentSize.Y + VWhitespace);
				InnerSize.Y = contentSize.Y;
			}
			FinalizeSize();
		}

		/// <summary> Update size of things that need the final calculated size (eg. BackPanels) </summary>
		protected virtual void FinalizeSize() { }

		/// <summary> Set Position of any contained elements based on their alignment settings. Call UpdateContainedElementPositions on each element. </summary>
		protected internal abstract void UpdateContainedElementPositions();

		/// <summary> Call UpdateSize of any contained MenuElements. Output the total size of all content. </summary>
		protected abstract void GetContentSize(out Point contentSize);

		public Point ContentOrigin {
			get { return new Point(Position.X + MarginLeft + PaddingLeft, Position.Y + MarginTop + PaddingTop); }
		}
		public Rectangle ContentBounds {
			get { return new Rectangle(ContentOrigin, InnerSize); }
		}
		public Point PanelOrigin {
			get { return new Point(Position.X + MarginLeft, Position.Y + MarginTop); }
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
	}
}
