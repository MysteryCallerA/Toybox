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

		public void Apply(ComplexEntity e) {
			e.Collider.Speed += ApplySpeed;
			var hitbox = e.Hitbox.Bounds;
			var friction = GroundFriction;

			if (e.Collider.BotClear(hitbox)) {
				e.Collider.Speed.Y += Gravity;
				friction = AirFriction;
			} else {
				e.Collider.Speed.Y = 0;
			}

			if (Math.Abs(e.Collider.Speed.X) > 0) {
				var prevh = e.Collider.Speed.X;
				e.Collider.Speed.X = (Math.Abs(e.Collider.Speed.X) - friction) * Math.Sign(e.Collider.Speed.X);
				if (Math.Sign(prevh) != Math.Sign(e.Collider.Speed.X)) e.Collider.Speed.X = 0;
				if (e.Collider.Speed.X > 0 && !e.Collider.RightClear(hitbox)) e.Collider.Speed.X = 0;
				if (e.Collider.Speed.X < 0 && !e.Collider.LeftClear(hitbox)) e.Collider.Speed.X = 0;
			}
		}

	}
}
