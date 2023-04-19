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

			dif.X += e.TrueX % 1;
			dif.Y += e.TrueY % 1;
			e.TrueX = (float)Math.Truncate(e.TrueX);
			e.TrueY = (float)Math.Truncate(e.TrueY);

			var subPixels = new Vector2(dif.X % 1, dif.Y % 1);
			var intdif = new Point((int)Math.Truncate(dif.X), (int)Math.Truncate(dif.Y));

			if (intdif == Point.Zero) {
				e.TrueX += subPixels.X;
				e.TrueY += subPixels.Y;
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
