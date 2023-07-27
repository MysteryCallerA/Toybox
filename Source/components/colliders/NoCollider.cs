using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.components.colliders {
	public class NoCollider:EntityCollider {

		public override bool BotClear(Rectangle hitbox) {
			return true;
		}

		public override bool LeftClear(Rectangle hitbox) {
			return true;
		}

		public override void Move(Entity e, Point dif) {
			e.Position += dif;
		}

		public override bool PointClear(Point p) {
			return true;
		}

		public override bool RightClear(Rectangle hitbox) {
			return true;
		}

		public override bool TopClear(Rectangle hitbox) {
			return true;
		}
	}
}
