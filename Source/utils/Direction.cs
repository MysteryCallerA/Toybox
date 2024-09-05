using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.utils {

	public enum Direction {
		Neutral, Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft
	}

	public static class DirectionOps {

		public static Direction Vertical(this Direction d) {
			if (d == Direction.UpLeft || d == Direction.Up || d == Direction.UpRight) return Direction.Up;
			if (d == Direction.DownLeft || d == Direction.Down || d == Direction.DownRight) return Direction.Down;
			return Direction.Neutral;
		}

		public static Direction Horizontal(this Direction d) {
			if (d == Direction.UpLeft || d == Direction.Left || d == Direction.DownLeft) return Direction.Left;
			if (d == Direction.UpRight || d == Direction.Right || d == Direction.DownRight) return Direction.Right;
			return Direction.Neutral;
		}

		public static Direction SwitchedUp(this Direction d) {
			if (d == Direction.UpLeft || d == Direction.Up || d == Direction.UpRight) return d;
			if (d == Direction.Left || d == Direction.DownLeft) return Direction.UpLeft;
			if (d == Direction.Right || d == Direction.DownRight) return Direction.UpRight;
			return Direction.Up;
		}

		public static Direction SwitchedDown(this Direction d) {
			if (d == Direction.DownLeft || d == Direction.Down || d == Direction.DownRight) return d;
			if (d == Direction.UpLeft || d == Direction.Left) return Direction.DownLeft;
			if (d == Direction.UpRight || d == Direction.Right) return Direction.DownRight;
			return Direction.Down;
		}

		public static Direction SwitchedLeft(this Direction d) {
			if (d == Direction.UpLeft || d == Direction.Left || d == Direction.DownLeft) return d;
			if (d == Direction.UpRight || d == Direction.Up) return Direction.UpLeft;
			if (d == Direction.DownRight || d == Direction.Down) return Direction.DownLeft;
			return Direction.Left;
		}

		public static Direction SwitchedRight(this Direction d) {
			if (d == Direction.UpRight || d == Direction.Right || d == Direction.DownRight) return d;
			if (d == Direction.UpLeft || d == Direction.Up) return Direction.UpRight;
			if (d == Direction.DownLeft || d == Direction.Down) return Direction.DownRight;
			return Direction.Right;
		}

		public static Point ToPoint(this Direction d) {
			switch (d) {
				case Direction.Up: return new Point(0, -1);
				case Direction.Down: return new Point(0, 1);
				case Direction.Left: return new Point(-1, 0);
				case Direction.Right: return new Point(1, 0);
				case Direction.UpLeft: return new Point(-1, -1);
				case Direction.UpRight: return new Point(1, -1);
				case Direction.DownLeft: return new Point(-1, 1);
				case Direction.DownRight: return new Point(1, 1);
			}
			return Point.Zero;
		}

		public static Direction ToDirection(this Point p) {
			if (p.X > 0) {
				if (p.Y > 0) return Direction.DownRight;
				if (p.Y < 0) return Direction.UpRight;
				return Direction.Right;
			}
			if (p.X < 0) {
				if (p.Y > 0) return Direction.DownLeft;
				if (p.Y < 0) return Direction.UpLeft;
				return Direction.Left;
			}
			if (p.Y > 0) return Direction.Down;
			if (p.Y < 0) return Direction.Up;
			return Direction.Neutral;
		}

	}

}
