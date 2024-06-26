using Microsoft.Xna.Framework;

namespace Toybox.components {

	public readonly struct Collision {
		public readonly int Type;
		public readonly Rectangle Hitbox;

		public static int DefaultValue = 0;

		public Collision(int type, Rectangle h) {
			Type = type;
			Hitbox = h;
		}
		public Collision() : this(DefaultValue, Rectangle.Empty) { }
	}

}
