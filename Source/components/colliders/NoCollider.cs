using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.components.colliders {
	public class NoCollider:EntityCollider {

		public readonly Entity Parent;

		public NoCollider(Entity parent) {
			Parent = parent;
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

		public override bool IsCollisionSolid(in Collision c) {
			return false;
		}
	}
}
