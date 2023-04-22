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

		public FourPointCollider(Func<Point, bool> checkSolid) {
			CheckSolid = checkSolid;
		}

		public override void Move(Entity e, Vector2 dif) {
			if (dif == Vector2.Zero) return;

			var hitbox = e.GetHitbox();
			while (dif.X >= 1 && RightClear(hitbox)) {
				hitbox.X++;
				e.X++;
				dif.X--;
			}
			while (dif.X <= -1 && LeftClear(hitbox)) {
				hitbox.X--;
				e.X--;
				dif.X++;
			}

			while (dif.Y >= 1 && BotClear(hitbox)) {
				hitbox.Y++;
				e.Y++;
				dif.Y--;
			}
			while (dif.Y <= -1 && TopClear(hitbox)) {
				hitbox.Y--;
				e.Y--;
				dif.Y++;
			}
		}

		public override bool LeftClear(Rectangle hitbox) {
			return !CheckSolid.Invoke(new Point(hitbox.X - 1, hitbox.Y)) && !CheckSolid.Invoke(new Point(hitbox.X - 1, hitbox.Bottom - 1));
		}

		public override bool RightClear(Rectangle hitbox) {
			return !CheckSolid.Invoke(new Point(hitbox.Right, hitbox.Top)) && !CheckSolid.Invoke(new Point(hitbox.Right, hitbox.Bottom - 1));
		}

		public override bool TopClear(Rectangle hitbox) {
			return !CheckSolid.Invoke(new Point(hitbox.Left, hitbox.Top - 1)) && !CheckSolid.Invoke(new Point(hitbox.Right - 1, hitbox.Top - 1));
		}

		public override bool BotClear(Rectangle hitbox) {
			return !CheckSolid.Invoke(new Point(hitbox.Left, hitbox.Bottom)) && !CheckSolid.Invoke(new Point(hitbox.Right - 1, hitbox.Bottom));
		}

		public override bool PointClear(Point p) {
			return !CheckSolid.Invoke(p);
		}
	}
}
