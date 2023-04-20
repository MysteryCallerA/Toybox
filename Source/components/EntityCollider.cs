using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.components {
	public abstract class EntityCollider {

		public abstract void Move(Entity e, Vector2 dif);

		public abstract bool LeftClear(Rectangle hitbox);

		public abstract bool RightClear(Rectangle hitbox);

		public abstract bool TopClear(Rectangle hitbox);

		public abstract bool BotClear(Rectangle hitbox);

		public abstract bool PointClear(Point p);

	}
}
