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

		public Point TopLeft { get { return new Point(Left, Top); } }
		public Point TopRight { get { return new Point(Right, Top); } }
		public Point BotLeft { get { return new Point(Left, Bottom); } }
		public Point BotRight { get { return new Point(Right, Bottom); } }

		public Point TopRightInner { get { return new Point(Right - 1, Top); } }
		public Point BotLeftInner { get { return new Point(Left, Bottom - 1); } }
		public Point BotRightInner { get { return new Point(Right - 1, Bottom - 1); } }

		public Rectangle BoxAbove { get { return new Rectangle(X, Y - 1, Width, 1); } }
		public Rectangle BoxBelow { get { return new Rectangle(X, Bottom, Width, 1); } }
		public Rectangle BoxLeft { get { return new Rectangle(X - 1, Y, 1, Height); } }
		public Rectangle BoxRight { get { return new Rectangle(Right, Y, 1, Height); } }


	}
}
