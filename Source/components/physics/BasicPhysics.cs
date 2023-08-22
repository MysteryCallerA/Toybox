using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.components.physics {
	public class BasicPhysics:EntityComponent {

		public float Weight = 1;
		public float Gravity = 0.6f;
		public float AirFriction = 0.1f;
		public float GroundFriction = 0.2f;

		private Vector2 ApplySpeed = Vector2.Zero;

		public void Bump(Vector2 dir, float force) {
			dir.Normalize();
			ApplySpeed = dir * (force / Weight);
		}

		public void Apply(Entity e) {
			/*e.Speed += ApplySpeed;
			var hitbox = e.Hitbox.Bounds;
			var friction = GroundFriction;

			if (e.Collider.BotClear(e)) {
				e.Speed.Y += Gravity;
				friction = AirFriction;
			} else {
				e.Speed.Y = 0;
			}

			if (Math.Abs(e.Speed.X) > 0) {
				var prevh = e.Speed.X;
				e.Speed.X = (Math.Abs(e.Speed.X) - friction) * Math.Sign(e.Speed.X);
				if (Math.Sign(prevh) != Math.Sign(e.Speed.X)) e.Speed.X = 0;
				if (e.Speed.X > 0 && !e.Collider.RightClear(e)) e.Speed.X = 0;
				if (e.Speed.X < 0 && !e.Collider.LeftClear(e)) e.Speed.X = 0;
			}*/
		}

	}
}
