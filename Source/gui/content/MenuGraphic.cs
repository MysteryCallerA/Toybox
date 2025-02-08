using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.graphic;
using Toybox.gui.core;

namespace Toybox.gui.content {
	public class MenuGraphic:MenuElement {

		public const string TypeName = "Graphic";
		public IGraphicState Content;
		public Color Color = Color.White;
		public int Scale = 0;

		public MenuGraphic() {
		}

		public MenuGraphic(IGraphicState g) {
			Content = g;
		}

		public override void Draw(Renderer r) {
			Content.GetDrawData(ContentOrigin, out var graphic, out var source, out var dest);
			if (Scale == 0) {
				dest = new Rectangle(dest.Location, dest.Size * new Point(Resources.Camera.PixelScale));
			} else {
				dest = new Rectangle(dest.Location, dest.Size * new Point(Scale));
			}
			r.DrawDirect(graphic, dest, source, Color, Microsoft.Xna.Framework.Graphics.SpriteEffects.None); 
		}

		protected internal override void UpdateFunction(MenuControls c) {
		}

		protected override void UpdateContentSize(Point contentContainerSize, out Point contentSize) {
			contentSize = Content.GetSize();
			if (Scale == 0) {
				contentSize *= new Point(Resources.Camera.PixelScale);
			} else {
				contentSize *= new Point(Scale);
			}
		}

		protected internal override void UpdateContainedElementPositions() {
		}

		public override string GetTypeName() {
			return TypeName;
		}
	}
}
