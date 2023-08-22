using Microsoft.Xna.Framework;

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
		public bool IsSolid;
		public string Name;

		public CollisionType(string name, float priority, bool solid) {
			Priority = priority;
			IsSolid = solid;
			Name = name;
		}

		public static CollisionType Clear = new CollisionType("Clear", 0, false);
		public static CollisionType Solid = new CollisionType("Solid", 1, true);
	}

}
