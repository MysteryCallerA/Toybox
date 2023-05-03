using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.components.collision {
	public class FourPointCollider:EntityCollider {

		public Func<Point, Collision> CheckSolid;

		private Point ToMove;

		public FourPointCollider(Func<Point, Collision> checkSolid) {
			CheckSolid = checkSolid;
		}

		public override void Move(Entity e, Point dif) {
			if (dif == Point.Zero) return;
			ToMove = dif;
			var hitbox = e.GetHitbox();

			ResolveHMove(ref hitbox, e);
			ResolveVMove(ref hitbox, e);

			if (ToMove.X != 0) {
				ResolveHMove(ref hitbox, e);
			}
		}

		private void ResolveHMove(ref Rectangle hitbox, Entity e) {
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

		private void ResolveVMove(ref Rectangle hitbox, Entity e) {
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
			var c = Max(CheckSolid.Invoke(new Point(hitbox.X - 1, hitbox.Y)), CheckSolid.Invoke(new Point(hitbox.X - 1, hitbox.Bottom - 1)));
			if (c.Type == CollisionType.Solid) return false;
			return true;
		}

		public override bool RightClear(Rectangle hitbox) {
			var c = Max(CheckSolid.Invoke(new Point(hitbox.Right, hitbox.Top)), CheckSolid.Invoke(new Point(hitbox.Right, hitbox.Bottom - 1)));
			if (c.Type == CollisionType.Solid) return false;
			return true;
		}

		public override bool TopClear(Rectangle hitbox) {
			var c = Max(CheckSolid.Invoke(new Point(hitbox.Left, hitbox.Top - 1)), CheckSolid.Invoke(new Point(hitbox.Right - 1, hitbox.Top - 1)));
			if (c.Type == CollisionType.Solid) return false;
			return true;
		}

		public override bool BotClear(Rectangle hitbox) {
			var c = Max(CheckSolid.Invoke(new Point(hitbox.Left, hitbox.Bottom)), CheckSolid.Invoke(new Point(hitbox.Right - 1, hitbox.Bottom)));
			if (c.Type == CollisionType.Solid) return false;
			if (c.Type == CollisionType.SemiSolid) {
				if (hitbox.Bottom <= c.Hitbox.Top) return false;
			}
			return true;
		}

		public override bool PointClear(Point p) {
			var c = CheckSolid.Invoke(p);
			if (c.Type == CollisionType.Solid) return false;
			return true;
		}
	}
}
