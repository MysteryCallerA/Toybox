using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.graphic {
	public class TextureMap:TextureObject {

		public List<TextureMapFrame> Frames = new List<TextureMapFrame>();
		public int SelectedFrame = 0;

		public TextureMap(Texture2D t):base(t) {
		}

		public override Rectangle Source {
			get { return Frames[SelectedFrame].Bounds; }
		}

		public TextureMapFrame this[int i] {
			get { return Frames[i]; }
		}

		public struct TextureMapFrame {

			public Rectangle Bounds;
			public Point Origin = Point.Zero;

			public TextureMapFrame(Rectangle bounds) {
				Bounds = bounds;
			}

			public void GetDrawRects(int x, int y, int scale, out Rectangle source, out Rectangle dest) {
				source = Bounds;
				dest = new Rectangle(x - (Origin.X * scale), y - (Origin.Y * scale), source.Width * scale, source.Height * scale);
			}

		}
	}
}
