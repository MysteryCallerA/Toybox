using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.components {
	public abstract class EntityCollider {

		public abstract void Move(Entity e, Point dif);

	}
}
