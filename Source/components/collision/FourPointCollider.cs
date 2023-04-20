using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.components.collision {
	public class FourPointCollider:EntityCollider {

		public Func<Point, bool> CheckSolid;
		private Rectangle Hitbox;

		public FourPointCollider(Func<Point, bool> checkSolid) {
			CheckSolid = checkSolid;
		}

		public override void Move(Entity e, Vector2 dif) {
			if (dif == Vector2.Zero) return;

			dif.X += e.TrueX % 1;
			dif.Y += e.TrueY % 1;

			var subPixels = new Vector2(dif.X % 1, dif.Y % 1);
			var intdif = new Point((int)Math.Truncate(dif.X), (int)Math.Truncate(dif.Y));
			Hitbox = e.GetHitbox();

			if (intdif == Point.Zero) {
				if (subPixels.X > 0) {
					if (RightClear(Hitbox)) e.TrueX += subPixels.X;
				} else if (subPixels.X < 0) {
					if (LeftClear(Hitbox)) e.TrueX += subPixels.X;
				}
				if (subPixels.Y > 0) {
					if (BotClear(Hitbox)) e.TrueY += subPixels.Y;
				} else if (subPixels.Y < 0) {
					if (TopClear(Hitbox)) e.TrueY += subPixels.Y;
				}
				return;
			}

			Vector2 steps;
			int stepnum;
			if (intdif.X == 0) {
				steps = new Vector2(0, Math.Sign(intdif.Y));
				stepnum = Math.Abs(intdif.Y);
			} else if (intdif.Y == 0) {
				steps = new Vector2(Math.Sign(intdif.X), 0);
				stepnum = Math.Abs(intdif.X);
			} else if (Math.Abs(intdif.X) > Math.Abs(intdif.Y)) {
				steps = new Vector2(Math.Sign(intdif.X), intdif.Y / Math.Abs(intdif.X));
				stepnum = Math.Abs(intdif.X);
			} else {
				steps = new Vector2(intdif.X / Math.Abs(intdif.Y), Math.Sign(intdif.Y));
				stepnum = Math.Abs(intdif.Y);
			}

			float xsub = 0, ysub = 0;
			for (int i = 1; i <= stepnum; i++) {
				xsub += steps.X;
				if (xsub >= 1) {
					MoveRight(e);
					xsub--;
				} else if (xsub <= -1) {
					MoveLeft(e);
					xsub++;
				}

				ysub += steps.Y;
				if (ysub >= 1) {
					MoveDown(e);
					ysub--;
				} else if (ysub <= -1) {
					MoveUp(e);
					ysub++;
				}
			}

			subPixels.X += xsub;
			subPixels.Y += ysub;
			if (subPixels.X > 0) {
				if (RightClear(Hitbox)) e.TrueX += subPixels.X;
			} else if (subPixels.X < 0) {
				if (LeftClear(Hitbox)) e.TrueX += subPixels.X;
			}
			if (subPixels.Y > 0) {
				if (BotClear(Hitbox)) e.TrueY += subPixels.Y;
			} else if (subPixels.Y < 0) {
				if (TopClear(Hitbox)) e.TrueY += subPixels.Y;
			}
		}

		public override bool LeftClear(Rectangle hitbox) {
			Point offset = new Point(-1, 0);
			return !CheckSolid.Invoke(hitbox.Location + offset) && !CheckSolid.Invoke(new Point(hitbox.X, hitbox.Bottom) + offset);
		}

		public override bool RightClear(Rectangle hitbox) {
			Point offset = new Point(1, 0);
			return !CheckSolid.Invoke(new Point(hitbox.Right, hitbox.Top) + offset) && !CheckSolid.Invoke(new Point(hitbox.Right, hitbox.Bottom) + offset);
		}

		public override bool TopClear(Rectangle hitbox) {
			Point offset = new Point(0, -1);
			return !CheckSolid.Invoke(new Point(hitbox.Right, hitbox.Top) + offset) && !CheckSolid.Invoke(hitbox.Location + offset);
		}

		public override bool BotClear(Rectangle hitbox) {
			Point offset = new Point(0, 1);
			return !CheckSolid.Invoke(new Point(hitbox.Left, hitbox.Bottom) + offset) && !CheckSolid.Invoke(new Point(hitbox.Right, hitbox.Bottom) + offset);
		}

		private void MoveLeft(Entity e) {
			if (LeftClear(Hitbox)) {
				Hitbox.X--;
				e.X--;
			}
		}

		private void MoveRight(Entity e) {
			if (RightClear(Hitbox)) {
				Hitbox.X++;
				e.X++;
			}
		}

		private void MoveDown(Entity e) {
			if (BotClear(Hitbox)) {
				Hitbox.Y++;
				e.Y++;
			}
		}

		private void MoveUp(Entity e) {
			if (TopClear(Hitbox)) {
				Hitbox.Y--;
				e.Y--;
			}
		}

		public override bool PointClear(Point p) {
			return !CheckSolid.Invoke(p);
		}
	}
}
