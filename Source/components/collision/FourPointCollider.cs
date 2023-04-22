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
		public bool SlipCorners = true;
		public int SlipDistance = 2;

		private Vector2 ToMove;

		public FourPointCollider(Func<Point, bool> checkSolid) {
			CheckSolid = checkSolid;
		}

		public override void Move(Entity e, Vector2 dif) {
			if (dif == Vector2.Zero) return;
			ToMove = dif;
			var hitbox = e.GetHitbox();

			ResolveHMove(hitbox, e);
			ResolveVMove(hitbox, e);

			if (Math.Truncate(ToMove.X) != 0) {
				ResolveHMove(hitbox, e);
			}

			if (!SlipCorners) return;

			//NEXT somehow implement a corner slipping system???
		}

		private void ResolveHMove(Rectangle hitbox, Entity e) {
			while (ToMove.X >= 1 && RightClear(hitbox)) {
				hitbox.X++;
				e.X++;
				ToMove.X--;
			}
			while (ToMove.X <= -1 && LeftClear(hitbox)) {
				hitbox.X--;
				e.X--;
				ToMove.X++;
			}
		}

		private void ResolveVMove(Rectangle hitbox, Entity e) {
			while (ToMove.Y >= 1 && BotClear(hitbox)) {
				hitbox.Y++;
				e.Y++;
				ToMove.Y--;
			}
			while (ToMove.Y <= -1 && TopClear(hitbox)) {
				hitbox.Y--;
				e.Y--;
				ToMove.Y++;
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
