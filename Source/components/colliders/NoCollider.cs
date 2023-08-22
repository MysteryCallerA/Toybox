using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.components.colliders {
	public class NoCollider:EntityCollider {

		/*public override bool BotClear(Entity e) {
			return true;
		}

		public override bool LeftClear(Entity e) {
			return true;
		}

		public override void Move(Entity e, Point dif) {
			e.Position += dif;
		}

		public override bool PointClear(Point p) {
			return true;
		}

		public override bool RightClear(Entity e) {
			return true;
		}

		public override bool TopClear(Entity e) {
			return true;
		}*/

		public readonly Entity Parent;

		public NoCollider(Entity parent) {
			Parent = parent;
		}

		public override void ApplyMove(Point move) {
			Parent.Position += move;
		}

		public override Collision? FindSolid(Rectangle box) {
			return null;
		}
	}
}
