using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Toybox.components.EntityCollider;

namespace Toybox.components {

	public struct Collision {
		public CollisionType Type;
		public Rectangle Hitbox;

		public Collision(CollisionType t, Rectangle h) {
			Type = t;
			Hitbox = h;
		}
		public Collision() : this(CollisionType.Clear, Rectangle.Empty) { }

		public static Collision Max(Collision a, Collision b) {
			if (a.Type.Priority > b.Type.Priority) return a;
			return b;
		}
	}

	public class CollisionType {
		public float Priority;

		public CollisionType(float priority) {
			Priority = priority;
		}

		public static CollisionType Clear = new CollisionType(0);
		public static CollisionType Solid = new CollisionType(1);
		public static CollisionType SemiSolid = new CollisionType(0.5f);
	}

}
