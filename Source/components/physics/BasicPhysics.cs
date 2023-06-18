using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.components.physics {
	public class BasicPhysics:EntityComponent {

		public float HSpeed = 0;
		public float VSpeed = 0;

		public float Weight = 1;
		public float Gravity = 0.6f;
		public float AirFriction = 0.1f;
		public float GroundFriction = 0.2f;

		public void Bump(Vector2 dir, float force) {
			dir.Normalize();
			Vector = dir * (force / Weight);
		}

		public void Apply(ComplexEntity e) {
			e.Move(Vector.ToPoint());
			var hitbox = e.GetHitbox();
			var friction = GroundFriction;

			if (e.Collider.BotClear(hitbox)) {
				VSpeed += Gravity;
				friction = AirFriction;
			} else {
				VSpeed = 0;
			}

			if (Math.Abs(HSpeed) > 0) {
				var prevh = HSpeed;
				HSpeed = (Math.Abs(HSpeed) - friction) * Math.Sign(HSpeed);
				if (Math.Sign(prevh) != Math.Sign(HSpeed)) HSpeed = 0;
				if (HSpeed > 0 && !e.Collider.RightClear(hitbox)) HSpeed = 0;
				if (HSpeed < 0 && !e.Collider.LeftClear(hitbox)) HSpeed = 0;
			}
		}

		public Vector2 Vector {
			get { return new Vector2(HSpeed, VSpeed); }
			set { HSpeed = value.X; VSpeed = value.Y; }
		}

		public float Speed { get { return Vector.Length(); } }

	}
}
