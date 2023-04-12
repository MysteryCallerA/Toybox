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
		public Func<Point, bool> CheckSolid;

		public int Speed = 1;
		public int FallSpeed = 1;

		public JumpMover(VirtualKey left, VirtualKey right, VirtualKey jump, Func<Point, bool> checkSolid) {
			LeftKey = left;
			RightKey = right;
			JumpKey = jump;
			CheckSolid = checkSolid;
		}

		public void Apply(Entity e) {
			var hitbox = e.GetHitbox();

			int speed = 0;
			if (LeftKey.Down) speed -= Speed;
			if (RightKey.Down) speed += Speed;
			e.X += speed;

			if (speed < 0) {
				Direction = AnimationDirection.Left;
				State = AnimationState.Moving;
			} else if (speed > 0) {
				Direction = AnimationDirection.Right;
				State = AnimationState.Moving;
			} else {
				State = AnimationState.Idle;
			}

			var movingto = e.Position + new Point(0, FallSpeed);
			if (!CheckSolid.Invoke(movingto + new Point(0, hitbox.Height))) {
				e.Position = movingto;
			}
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
