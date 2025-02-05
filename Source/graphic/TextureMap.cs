using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.graphic {
	public class TextureMap {

		public Texture2D Graphic;
		public List<TextureMapFrame> Frames = new List<TextureMapFrame>();

		public TextureMap(Texture2D t) {
			Graphic = t;
		}

		public TextureMapFrame this[int i] {
			get { return Frames[i]; }
		}
	}

	public struct TextureMapFrame {

		public Rectangle Bounds;
		public Point Origin = Point.Zero;

		public TextureMapFrame(Rectangle bounds) {
			Bounds = bounds;
		}

		public void GetDrawRects(Point destPos, out Rectangle source, out Rectangle dest) {
			source = Bounds;
			dest = new Rectangle(destPos.X - Origin.X, destPos.Y - Origin.Y, source.Width, source.Height);
		}

	}

	public class TextureMapState:IGraphicState {

		public TextureMap TextureMap;
		public int Frame;

		public TextureMapState(TextureMap t) {
			TextureMap = t;
		}

		public List<TextureMapFrame> Frames { get { return TextureMap.Frames; } }
		public TextureMapFrame this[int i] { get { return TextureMap.Frames[i]; } }
		public Texture2D Graphic { get { return TextureMap.Graphic; } }

		public void GetDrawData(Point destPos, out Texture2D graphic, out Rectangle source, out Rectangle dest) {
			TextureMap.Frames[Frame].GetDrawRects(destPos, out source, out dest);
			graphic = TextureMap.Graphic;
		}

		public Point GetSize() {
			return TextureMap.Frames[Frame].Bounds.Size;
		}
	}
}
