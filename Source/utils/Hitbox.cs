using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.utils {
	public class Hitbox {

		private readonly Entity Entity;

		public int XOffset = 0;
		public int YOffset = 0;
		public int Width;
		public int Height;

		public Hitbox(Entity e) {
			Entity = e;
		}

		public virtual int X {
			get { return Entity.X + XOffset; }
			set { Entity.X = value - XOffset; }
		}

		public virtual int Y {
			get { return Entity.Y + YOffset; }
			set { Entity.Y = value - YOffset; }
		}

		public Rectangle Bounds //TODO check everywhere this is referenced to see if it needs changing
		{
			get { return new Rectangle(X, Y, Width, Height); }
		}

		public Point Position {
			get { return new Point(X, Y); }
			set { X = value.X; Y = value.Y; }
		}

		public Point Size {
			get { return new Point(Width, Height); }
			set { Width = value.X; Height = value.Y; }
		}

		public int Left { get { return X; } }
		public int Right { get { return X + Width; } }
		public int Top { get { return Y; } }
		public int Bottom { get { return Y + Height; } }

		public Point TopLeft { get { return Position; } }
		public Point TopRight { get { return new Point(X + Width, Y); } }
		public Point BotLeft { get { return new Point(X, Y + Height); } }
		public Point BotRight { get { return new Point(X + Width, Y + Height); } }


	}
}
