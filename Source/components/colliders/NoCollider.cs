using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.components.colliders {
	public class NoCollider:EntityCollider {

		public NoCollider(Entity parent):base(parent) {
		}

		public override void ApplyMove(Point move) {
			Parent.Position += move;
		}

		public override IEnumerable<Collision> FindCollisions(Rectangle box) {
			yield break;
		}

		public override Collision? FindSolid(Rectangle box) {
			return null;
		}

		public override Collision? FindSolid(Point p) {
			return null;
		}

		public override bool IsCollisionSolid(Collision c) {
			return false;
		}

		public override bool IsCollisionSolid(Collision c, Point referencePos) {
			return false;
		}

	}
}
