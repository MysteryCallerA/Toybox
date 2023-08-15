using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.components
{
	public abstract class EntityCollider {

		public abstract void Move(Entity e, Point dif);

		public abstract bool LeftClear(Entity e);

		public abstract bool RightClear(Entity e);

		public abstract bool TopClear(Entity e);

		public abstract bool BotClear(Entity e);

		public abstract bool PointClear(Point p);

		protected virtual bool CollisionIsSolid(Collision c, Entity e) {
			return c.Type.IsSolid;
		}

		protected virtual bool CollisionIsSolid(Collision c) {
			return c.Type.IsSolid;
		}

	}
}
