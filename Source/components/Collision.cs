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
	}

	public class CollisionType { //TODO add a tag-based system to this so collision types can have subtypes

		/// <summary> Used only for debugging. CollisionTypes should be instantiated once, in a static class, then you can compare them directly. </summary>
		public readonly string Name;

		public readonly int Id;
		private static int NextId;

		public CollisionType(string name) {
			Name = name;
			Id = NextId;
			NextId++;
		}

		public override bool Equals(object obj) {
			if (obj == null || GetType() != obj.GetType()) return false;
			return Id == ((CollisionType)obj).Id;
		}

		public override int GetHashCode() {
			return Id.GetHashCode();
		}

		public static readonly CollisionType Clear = new CollisionType("Clear");
		public static readonly CollisionType Solid = new CollisionType("Solid");
	}

}
