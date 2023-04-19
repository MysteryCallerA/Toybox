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

		public override void Move(Entity e, Point dif) {
			if (dif == Point.Zero) return;

			Hitbox = e.GetHitbox();
			Point steps;
			int stepnum;
			if (dif.X == 0) {
				steps = new Point(0, Math.Sign(dif.Y));
				stepnum = Math.Abs(dif.Y);
			} else if (dif.Y == 0) {
				steps = new Point(Math.Sign(dif.X), 0);
				stepnum = Math.Abs(dif.X);
			} else if (Math.Abs(dif.X) > Math.Abs(dif.Y)) {
				steps = new Point(dif.X / Math.Abs(dif.Y), Math.Sign(dif.Y));
				stepnum = Math.Abs(dif.Y);
			} else {
				steps = new Point(Math.Sign(dif.X), dif.Y / Math.Abs(dif.X));
				stepnum = Math.Abs(dif.X);
			}

			for (int i = 1; i <= stepnum; i++) {

			}
		}

		private Point TopLeft { 
			get { return Hitbox.Location; } 
		}
		private Point TopRight {
			get { return new Point(Hitbox.Right, Hitbox.Y); }
		}
		private Point BotLeft {
			get { return new Point(Hitbox.X, Hitbox.Bottom); }
		}
		private Point BotRight {
			get { return new Point(Hitbox.Right, Hitbox.Bottom); }
		}
	}
}
