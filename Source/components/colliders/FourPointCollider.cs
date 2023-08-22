using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Toybox.utils;

namespace Toybox.components.colliders
{
	/*public class FourPointCollider:EntityCollider {

		public Func<Point, Collision> CheckSolid;
		public Func<Point, IEnumerable<Collision>> GetCollisions; //TODO mabye migrate everything to this function instead

		public FourPointCollider(Func<Point, Collision> checkSolid) {
			CheckSolid = checkSolid;
		}

		public override void Move(Entity e, Point dif) {
			if (dif == Point.Zero) return;
			while (dif != Point.Zero) {
				bool hcollision = false;
				bool vcollision = false;
				if (dif.X > 0) {
					if (RightClear(e)) {
						dif.X--;
						e.X++;
					} else hcollision = true;
				} else if (dif.X < 0) {
					if (LeftClear(e)) {
						dif.X++;
						e.X--;
					} else hcollision = true;
				}

				if (dif.Y > 0) {
					if (BotClear(e)) {
						dif.Y--;
						e.Y++;
					} else vcollision = true;
				} else if (dif.Y < 0) {
					if (TopClear(e)) {
						dif.Y++;
						e.Y--;
					} else vcollision = true;
				}

				if ((hcollision || dif.X == 0) && (vcollision || dif.Y == 0)) break;
			}

			if (dif.X != 0) e.Speed.X = 0;
			if (dif.Y != 0) e.Speed.Y = 0;
		}

		private static readonly Point HOffset = new Point(1, 0);
		private static readonly Point VOffset = new Point(0, 1);

		public override bool LeftClear(Entity e) {
			var c = Collision.Max(CheckSolid.Invoke(e.Hitbox.TopLeft - HOffset), CheckSolid.Invoke(e.Hitbox.BotLeftInner - HOffset));
			return !CollisionIsSolid(c, e);
		}

		public override bool RightClear(Entity e) {
			var c = Collision.Max(CheckSolid.Invoke(e.Hitbox.TopRightInner + HOffset), CheckSolid.Invoke(e.Hitbox.BotRightInner + HOffset));
			return !CollisionIsSolid(c, e);
		}

		public override bool TopClear(Entity e) {
			var c = Collision.Max(CheckSolid.Invoke(e.Hitbox.TopLeft - VOffset), CheckSolid.Invoke(e.Hitbox.TopRightInner - VOffset));
			return !CollisionIsSolid(c, e);
		}

		public override bool BotClear(Entity e) {
			var c = Collision.Max(CheckSolid.Invoke(e.Hitbox.BotLeftInner + VOffset), CheckSolid.Invoke(e.Hitbox.BotRightInner + VOffset));
			return !CollisionIsSolid(c, e);
		}

		public override bool PointClear(Point p) {
			return !CollisionIsSolid(CheckSolid.Invoke(p));
		}

		public void UpdateCollisions(Entity e) {
			
		}
	}*/
}
