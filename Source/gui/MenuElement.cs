using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.gui {
	public abstract class MenuElement {

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
		public void Update() {
			UpdateFunction();
			UpdateSize(Resources.Camera.Bounds.Size);
			UpdateContainedElementPositions();
		}

		public virtual void Draw(Renderer r) {
		}

		/// <summary> Anything the element does, goes here. Call UpdateFunction on any contained elements. </summary>
		protected abstract void UpdateFunction();

		protected void UpdateSize(Point containerSize) {
			var innerSize = new Point(containerSize.X - HWhitespace, containerSize.Y - VWhitespace);
			int totalx = 0, totaly = 0;

			if (HFit == FitType.FitOuter) {
				totalx = containerSize.X;
			} else if (HFit == FitType.Static) {
				innerSize.X = InnerSize.X;
				totalx = InnerSize.X + HWhitespace;
			}
			if (VFit == FitType.FitOuter) {
				totaly = containerSize.Y;
			} else if (VFit == FitType.Static) {
				innerSize.Y = InnerSize.Y;
				totaly = InnerSize.Y + VWhitespace;
			}

			GetContentSize(innerSize, out Point contentSize);

			if (HFit == FitType.FitContent) {
				innerSize.X = contentSize.X;
				totalx = contentSize.X + HWhitespace;
			}
			if (VFit == FitType.FitContent) {
				innerSize.Y = contentSize.Y;
				totaly = contentSize.Y + VWhitespace;
			}

			TotalSize = new Point(totalx, totaly);
			InnerSize = innerSize;
		}

		/// <summary> Set Position of any contained elements based on their alignment settings. Call UpdateContainedElementPositions on each element. </summary>
		protected abstract void UpdateContainedElementPositions();

		/// <summary> Call UpdateSize of any contained MenuElements. Output the total size of all content. </summary>
		protected abstract void GetContentSize(Point innerSize, out Point contentSize);

		public Point ContentOrigin {
			get { return new Point(Position.X + MarginLeft + PaddingLeft, Position.Y + MarginTop + PaddingTop); }
		}
		public Rectangle ContentBounds {
			get { return new Rectangle(ContentOrigin, InnerSize); }
		}
		public Rectangle PanelBounds {
			get { return new Rectangle(new Point(Position.X + MarginLeft, Position.Y + MarginTop), new Point(InnerSize.X + PaddingLeft + PaddingRight, InnerSize.Y + PaddingTop + PaddingBottom)); }
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
