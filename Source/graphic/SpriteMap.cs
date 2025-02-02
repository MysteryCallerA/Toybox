using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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

		public void GetDrawRects(int frame, Point destPos, out Rectangle source, out Rectangle dest) {
			source = Frames[frame];
			dest = new Rectangle(destPos.X - Origin.X, destPos.Y - Origin.Y, source.Width, source.Height);
		}

	}

	public class SpriteMapState:IGraphicState {
		public SpriteMap SpriteMap;
		public int Frame;

		public SpriteMapState(SpriteMap s) {
			SpriteMap = s;
		}

		public List<Rectangle> Frames {	get { return SpriteMap.Frames; } }
		public Dictionary<string, Animation> Animations { get { return SpriteMap.Animations; } }
		public Texture2D Graphic { get { return SpriteMap.Graphic; } }
		public Point Origin { get { return SpriteMap.Origin; } }

		public void GetDrawData(Point destPos, out Texture2D graphic, out Rectangle source, out Rectangle dest) {
			SpriteMap.GetDrawRects(Frame, destPos, out source, out dest);
			graphic = SpriteMap.Graphic;
		}

	}
}
