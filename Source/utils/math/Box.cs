using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.utils.math {

	/// <summary> An inheritable object version of Rectangle with some extras. </summary>
	public class Box {

		public Rectangle Bounds;

		public Box() { Bounds = new Rectangle(); }
		public Box(Rectangle r) { Bounds = r; }
		public Box(Point pos, Point size) { Bounds = new Rectangle(pos, size); }
		public Box(int x, int y, int width, int height) { Bounds = new Rectangle(x, y, width, height); }

		public int X { get { return Bounds.X; } set { Bounds.X = value; } }
		public int Y { get { return Bounds.Y; } set { Bounds.Y = value; } }
		public int Width { get { return Bounds.Width; } set { Bounds.Width = value; } }
		public int Height { get { return Bounds.Height; } set { Bounds.Height = value; } }

		public Point Size { get { return Bounds.Size; } set { Bounds.Size = value; } }
		public Point Position { get { return Bounds.Location; } set { Bounds.Location = value; } }
	}
}
