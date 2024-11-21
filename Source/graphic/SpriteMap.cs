using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.graphic {
	public class SpriteMap {

		public List<Rectangle> Frames;
		public Dictionary<string, Animation> Animations;
		public Texture2D Graphic;
		public Point Origin;

		public SpriteMap(Texture2D graphic, List<Rectangle> frames):this(graphic, frames, new()) {
		}

		public SpriteMap(Texture2D graphic, List<Rectangle> frames, Dictionary<string, Animation> animations) {
			Graphic = graphic;
			Frames = frames;
			Animations = animations;
		}

		public void GetDrawRects(int frame, int destX, int destY, out Rectangle source, out Rectangle dest) {
			source = Frames[frame];
			dest = new Rectangle(destX - Origin.X, destY - Origin.Y, source.Width, source.Height);
		}

	}
}
