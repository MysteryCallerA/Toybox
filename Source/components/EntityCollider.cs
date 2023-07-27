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

		public Vector2 Speed = Vector2.Zero;

		public void Update(Entity e) {
			Move(e, Speed.ToPoint());
		}

		public abstract void Move(Entity e, Point dif);

		public abstract bool LeftClear(Rectangle hitbox);

		public abstract bool RightClear(Rectangle hitbox);

		public abstract bool TopClear(Rectangle hitbox);

		public abstract bool BotClear(Rectangle hitbox);

		public abstract bool PointClear(Point p);

	}
}
