using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.graphic {
	public class TextureSelection:IGraphicState {

		public Rectangle Selection;
		public Texture2D Graphic;
		public Point Origin;

		public TextureSelection(Texture2D t, Rectangle selection) {
			Selection = selection;
			Graphic = t;
		}

		public void GetDrawData(Point destPos, out Texture2D graphic, out Rectangle source, out Rectangle dest) {
			source = Selection;
			dest = new Rectangle(destPos - Origin, source.Size);
			graphic = Graphic;
		}

		public Point GetSize() {
			return Selection.Size;
		}
	}
}
