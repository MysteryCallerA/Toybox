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
		public float FallAccel = 0.1f;
		public int JumpSpeed = -2;

		public Vector2 Speed = Vector2.Zero;

		public JumpMover(VirtualKey left, VirtualKey right, VirtualKey jump) {
			LeftKey = left;
			RightKey = right;
			JumpKey = jump;
		}

		public void Apply(ComplexEntity e) {

			if (LeftKey.Down) Speed.X -= MoveAccel;
			if (RightKey.Down) Speed.X += MoveAccel;

			if (Math.Abs(Speed.X) > MaxSpeed) Speed.X = MaxSpeed * Math.Sign(Speed.X);

			if (!LeftKey.Down && !RightKey.Down) {
				Speed.X -= MoveDecel * Math.Sign(Speed.X);
			}

			if (Speed.X < 0) {
				Direction = AnimationDirection.Left;
				State = AnimationState.Moving;
			} else if (Speed.X > 0) {
				Direction = AnimationDirection.Right;
				State = AnimationState.Moving;
			} else {
				State = AnimationState.Idle;
			}

			if (!e.Collider.BotClear(e.GetHitbox())) {
				if (JumpKey.Pressed) {
					Speed.Y = JumpSpeed;
				}
			} else {
				Speed.Y += FallAccel;
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
