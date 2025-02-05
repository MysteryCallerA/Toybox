using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.graphic {
	public class TextureFull:IGraphicState {

		public Texture2D Graphic;
		public Point Origin;

		public TextureFull(Texture2D g) {
			Graphic = g;
		}

		public void GetDrawData(Point destPos, out Texture2D graphic, out Rectangle source, out Rectangle dest) {
			source = Graphic.Bounds;
			dest = new Rectangle(destPos.X - Origin.X, destPos.Y - Origin.Y, source.Width, source.Height);
			graphic = Graphic;
		}

		public Point GetSize() {
			return Graphic.Bounds.Size;
		}
	}
}
