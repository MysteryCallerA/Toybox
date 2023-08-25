using Microsoft.Xna.Framework;

namespace Toybox.components {

	public readonly struct Collision {
		public readonly CollisionType Type;
		public readonly Rectangle Hitbox;

		public Collision(CollisionType t, Rectangle h) {
			Type = t;
			Hitbox = h;
		}
		public Collision() : this(CollisionType.Clear, Rectangle.Empty) { }

		public static Collision Max(in Collision a, in Collision b) {
			if (a.Type.Priority > b.Type.Priority) return a;
			return b;
		}
	}

	public class CollisionType {

		/// <summary> When colliding with multiple things at once, higher Priority goes first. </summary>
		public readonly float Priority;

		/// <summary> True only is CollisionType is always solid. Use EntityCollider.IsCollisionSolid() to check if a Collision is solid. </summary>
		public readonly bool IsSolid;

		/// <summary> Used only for debugging. CollisionTypes should be instantiated once, in a static class, then you can compare them directly. </summary>
		public readonly string Name;

		public CollisionType(string name, float priority, bool solid) {
			Priority = priority;
			IsSolid = solid;
			Name = name;
		}

		public static CollisionType Clear = new CollisionType("Clear", 0, false);
		public static CollisionType Solid = new CollisionType("Solid", 1, true);
	}

}
