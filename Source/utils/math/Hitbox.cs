using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.utils.math
{
	public class Hitbox { //TODO move to components?

		private readonly Entity Entity;

		public Point Offset = Point.Zero;
		public int Width;
		public int Height;

		public Hitbox(Entity e) {
			Entity = e;
		}

		public int XOffset {
			get { return Offset.X; } set { Offset.X = value; }
		}

		public int YOffset {
			get { return Offset.Y; } set { Offset.Y = value; }
		}

		public virtual int X {
			get { return Entity.X + Offset.X; }
			set { Entity.X = value - Offset.X; }
		}

		public virtual int Y {
			get { return Entity.Y + Offset.Y; }
			set { Entity.Y = value - Offset.Y; }
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

		public int Left { get { return X; } set { X = value; } }
		public int Right { get { return X + Width; } set { X = value - Width; } }
		public int Top { get { return Y; } set { Y = value; } }
		public int Bottom { get { return Y + Height; } set { Y = value - Height; } }

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

		public Point Center {
			get { return new Point(CenterX, CenterY); }
			set { CenterX = value.X; CenterY = value.Y; }
		}
		public int CenterX {
			get { return X + Width / 2; }
			set { X = value - Width / 2; }
		}
		public int CenterY {
			get { return Y + Height / 2; }
			set { Y = value - Height / 2; }
		}

		public static Color DrawColor = Color.White * 0.3f;
		public void Draw(Renderer r, Camera c, Color? color = null) {
			r.DrawRect(Bounds, color ?? DrawColor, c, Camera.Space.Subpixel);
		}

	}
}
