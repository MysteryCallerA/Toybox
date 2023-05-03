using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.components {
	public abstract class EntityCollider {

		public struct Collision {
			public CollisionType Type;
			public Rectangle Hitbox;
			public Collision(CollisionType t, Rectangle h) {
				Type = t;
				Hitbox = h;
			}
			public Collision(CollisionType t) {
				Type = t;
				Hitbox = Rectangle.Empty;
			}
		}

		/// <summary> Ordered by priority. Use Max() to get highest priority Collision when encountering multiple at once. </summary>
		public enum CollisionType {
			Clear, SemiSolid, Solid
		}

		public static Collision Max(Collision a, Collision b) {
			if ((int)a.Type < (int)b.Type) return b;
			return a;
		}

		public abstract void Move(Entity e, Point dif);

		public abstract bool LeftClear(Rectangle hitbox);

		public abstract bool RightClear(Rectangle hitbox);

		public abstract bool TopClear(Rectangle hitbox);

		public abstract bool BotClear(Rectangle hitbox);

		public abstract bool PointClear(Point p);

	}
}
