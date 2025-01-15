using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;
using Toybox.utils.text;

namespace Toybox.gui.content {
	public class MenuText:MenuElement {

		public Text TextRenderer;

		public MenuText(Font f, string content = "") {
			TextRenderer = new Text(f);
			Content = content;
		}

		public override void Draw(Renderer r) {
			TextRenderer.Draw(r);
		}

		protected internal override void UpdateFunction() {
		}

		protected override void GetContentSize(out Point contentSize) {
			contentSize = TextRenderer.Size;
		}

		protected internal override void UpdateContainedElementPositions() {
			var contentbounds = ContentBounds;

			if (HAlign == HAlignType.Left) {
				TextRenderer.X = contentbounds.X;
			} else if (HAlign == HAlignType.Right) {
				TextRenderer.X = contentbounds.Right - TextRenderer.Size.X;
			} else if (HAlign == HAlignType.Center) {
				TextRenderer.X = contentbounds.Center.X - (TextRenderer.Size.X / 2);
			}

			if (VAlign == VAlignType.Top) {
				TextRenderer.Y = contentbounds.Y;
			} else if (VAlign == VAlignType.Bottom) {
				TextRenderer.Y = contentbounds.Bottom - TextRenderer.Size.Y;
			} else if (VAlign == VAlignType.Center) {
				TextRenderer.Y = contentbounds.Center.Y - (TextRenderer.Size.Y / 2);
			}
		}

		public string Content {
			get { return TextRenderer.Content; }
			set { TextRenderer.Content = value; }
		}

		public Color Color {
			get { return TextRenderer.Color; }
			set { TextRenderer.Color = value; }
		}

		public Color? BackColor {
			get { return TextRenderer.BackColor; }
			set { TextRenderer.BackColor = value; }
		}

		public int Scale {
			get { return TextRenderer.Scale; }
			set { TextRenderer.Scale = value; }
		}

		public Font Font {
			get { return TextRenderer.Font; }
			set { TextRenderer.Font = value; }
		}

	}
}
