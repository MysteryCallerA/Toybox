using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.input;

namespace Toybox.components.control {
	public class JumpMover:EntityComponent {

		public VirtualKey LeftKey;
		public VirtualKey RightKey;
		public VirtualKey JumpKey;

		public int MaxSpeed = 1;
		public float MoveAccel = 1f;
		public float MoveDecel = 1f;

		public float Gravity = 0.3f;
		public float MaxFallSpeed = 2;
		public float JumpSpeed = -3.5f;
		public float JumpEarlyTerminationSpeed = -0.5f;

		private Vector2 Speed = Vector2.Zero;
		private bool Jumping = false;

		public JumpMover(VirtualKey left, VirtualKey right, VirtualKey jump) {
			LeftKey = left;
			RightKey = right;
			JumpKey = jump;
		}

		public void Apply(ComplexEntity e) {
			var hitbox = e.GetHitbox();

			if (LeftKey.Down) Speed.X -= MoveAccel;
			if (RightKey.Down) Speed.X += MoveAccel;

			if (Math.Abs(Speed.X) > MaxSpeed) Speed.X = MaxSpeed * Math.Sign(Speed.X);

			if (!LeftKey.Down && !RightKey.Down) {
				Speed.X -= MoveDecel * Math.Sign(Speed.X);
			}

			if (e.Collider.BotClear(hitbox)) {
				Speed.Y += Gravity;
				if (Speed.Y > MaxFallSpeed) Speed.Y = MaxFallSpeed;
				if (Jumping && JumpKey.Released) {
					Jumping = false;
					if (Speed.Y < JumpEarlyTerminationSpeed) {
						Speed.Y = JumpEarlyTerminationSpeed;
					}
				}
			} else if (Speed.Y >= 0) {
				Speed.Y = 0;
				Jumping = false;
				if (JumpKey.Pressed) {
					Speed.Y = JumpSpeed;
					Jumping = true;
				}
			}

			e.Move(Speed);
		}

		public enum AnimationState {
			Idle, Moving
		}

		public enum AnimationDirection {
			Left, Right
		}

		public AnimationState State {
			get; private set;
		}

		public AnimationDirection Direction {
			get; private set;
		}
	}
}
