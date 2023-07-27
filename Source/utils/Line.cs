using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Toybox.utils {

	public struct Line {//NOTE collisions might not be pixel perfect?

		public Point Start;
		public Point End;

		public Line(Point start, Point end) {
			Start = start;
			End = end;
		}

		public Point Delta {
			get { return End - Start; }
		}

		public int Xdif {
			get { return End.X - Start.X; }
		}

		public int Ydif {
			get { return End.Y - Start.Y; }
		}

		public float Slope {
			get { return (float)Ydif / Xdif; }
		}

		public float YInt {
			get { return Start.Y - (Slope * Start.X); }
		}

		public override string ToString() {
			return $"PointRay{{X:{Start.X},Y:{Start.Y}}}->{{X:{End.X},Y{End.Y}}}";
		}

		public float SolveY(int x) {
			return SolveY(x, Slope, YInt);
		}

		public static float SolveY(int x, float slope, float yint) {
			return (x * slope) + yint;
		}

		public float SolveX(int y) {
			return SolveX(y, Slope, YInt);
		}

		public static float SolveX(int y, float slope, float yint) {
			return (y - yint) / slope;
		}

		public bool IsVertical {
			get { return Start.X == End.X; }
		}

		public bool IsHorizontal {
			get { return Start.Y == End.Y; }
		}

		public bool InDomain(float x) {
			if (End.X > Start.X) {
				if (x < End.X && x >= Start.X) return true;
			} else if (End.X < Start.X) {
				if (x >= End.X && x < Start.X) return true;
			} else {
				if (x == Start.X) return true;
			}
			return false;
		}

		public bool InRange(float y) {
			if (End.Y > Start.Y) {
				if (y < End.Y && y >= Start.Y) return true;
			} else if (End.Y < Start.Y) {
				if (y >= End.Y && y < Start.Y) return true;
			} else {
				if (y == Start.Y) return true;
			}
			return false;
		}

		public IEnumerable<Point> CollideGrid(int gridsize) {
			foreach (var output in RaycastGrid(gridsize)) {
				if (End.X > Start.X) {
					if (output.X > End.X) yield break;
				} else if (End.X < Start.X) {
					if (output.X < End.X) yield break;
				}
				if (End.Y > Start.Y) {
					if (output.Y > End.Y) yield break;
				} else if (End.Y < Start.Y) {
					if (output.Y < End.Y) yield break;
				}
				yield return output;
			}
		}

		/// <summary> Yields Points where the ray intersects a grid. This will yield infinitely. </summary>
		public IEnumerable<Point> RaycastGrid(int gridsize) {
			if (End == Start) yield break;

			var slope = Slope;
			var yint = YInt;
			Vector2 xcollision = Vector2.Zero, ycollision = Vector2.Zero;

			if (End.X > Start.X) {
				int x = (int)Math.Ceiling((float)Start.X / gridsize) * gridsize;
				xcollision = new Vector2(x, SolveY(x, slope, yint));
			} else if (End.X < Start.X) {
				int x = (int)(Math.Floor((float)Start.X / gridsize) * gridsize) - 1;
				xcollision = new Vector2(x, SolveY(x, slope, yint));
			}
			if (End.Y > Start.Y) {
				int y = (int)Math.Ceiling((float)Start.Y / gridsize) * gridsize;
				ycollision = new Vector2(SolveX(y, slope, yint), y);
			} else if (End.Y < Start.Y) {
				int y = (int)(Math.Floor((float)Start.Y / gridsize) * gridsize) - 1;
				ycollision = new Vector2(SolveX(y, slope, yint), y);
			}

			if (IsVertical) {
				ycollision.X = Start.X;
				while (true) {
					yield return ycollision.ToPoint();
					ycollision = new Vector2(Start.X, GetNextGridYCollision((int)ycollision.Y, gridsize, slope, yint).Y);
				}
			}
			if (IsHorizontal) {
				xcollision.Y = Start.Y;
				while (true) {
					yield return xcollision.ToPoint();
					xcollision = new Vector2(GetNextGridXCollision((int)xcollision.X, gridsize, slope, yint).X, Start.Y);
				}
			}

			if (End.X > Start.X) {
				while (true) {
					if (xcollision.X < ycollision.X) {
						yield return xcollision.ToPoint();
						xcollision = GetNextGridXCollision((int)xcollision.X, gridsize, slope, yint);
					} else {
						yield return ycollision.ToPoint();
						ycollision = GetNextGridYCollision((int)ycollision.Y, gridsize, slope, yint);
					}
				}
			}

			while (true) {
				if (xcollision.X > ycollision.X) {
					yield return xcollision.ToPoint();
					xcollision = GetNextGridXCollision((int)xcollision.X, gridsize, slope, yint);
				} else {
					yield return ycollision.ToPoint();
					ycollision = GetNextGridYCollision((int)ycollision.Y, gridsize, slope, yint);
				}
			}
		}

		/// <summary> Note that prevx must be gridaligned. </summary>
		private Vector2 GetNextGridXCollision(int prevx, int gridsize, float slope, float yint) {
			if (End.X > Start.X) {
				return new Vector2(prevx + gridsize, SolveY(prevx + gridsize, slope, yint));
			} else if (End.X < Start.X) {
				return new Vector2(prevx - gridsize, SolveY(prevx - gridsize, slope, yint));
			}
			throw new Exception("Line is vertical, no XCollisions exist.");
		}

		/// <summary> Note that prevy must be gridaligned. </summary>
		private Vector2 GetNextGridYCollision(int prevy, int gridsize, float slope, float yint) {
			if (End.Y > Start.Y) {
				return new Vector2(SolveX(prevy + gridsize, slope, yint), prevy + gridsize);
			} else if (End.Y < Start.Y) {
				return new Vector2(SolveX(prevy - gridsize, slope, yint), prevy - gridsize);
			}
			throw new Exception("Line is horizontal, no YCollisions exist.");
		}

		public Point? CollideRect(Rectangle r) {
			var slope = Slope;
			var yint = YInt;
			Vector2? xcollide = null, ycollide = null;

			if (End.X > Start.X) {
				xcollide = new Vector2(r.X, SolveY(r.X, slope, yint));
			} else if (End.X < Start.X) {
				xcollide = new Vector2(r.Right - 1, SolveY(r.Right - 1, slope, yint));
			}
			if (xcollide.HasValue && (xcollide.Value.Y >= r.Bottom || xcollide.Value.Y < r.Top || !InDomain(xcollide.Value.X))) xcollide = null;

			if (End.Y > Start.Y) {
				ycollide = new Vector2(SolveX(r.Y, slope, yint), r.Y);
			} else if (End.Y < Start.Y) {
				ycollide = new Vector2(SolveX(r.Bottom - 1, slope, yint), r.Bottom - 1);
			}
			if (ycollide.HasValue && (ycollide.Value.X >= r.Right || ycollide.Value.X < r.Left || !InRange(ycollide.Value.Y))) ycollide = null;

			if (IsVertical && ycollide.HasValue) ycollide = new Vector2(Start.X, ycollide.Value.Y);
			if (IsHorizontal && xcollide.HasValue) xcollide = new Vector2(xcollide.Value.X, Start.Y);

			if (!xcollide.HasValue) {
				if (!ycollide.HasValue) return null;
				return ycollide.Value.ToPoint();
			}
			if (!ycollide.HasValue) return xcollide.Value.ToPoint();

			if (End.X > Start.X) {
				if (xcollide.Value.X < ycollide.Value.X) return xcollide.Value.ToPoint();
				return ycollide.Value.ToPoint();
			} else {
				if (xcollide.Value.X > ycollide.Value.X) return xcollide.Value.ToPoint();
				return ycollide.Value.ToPoint();
			}
		}

	}
}
