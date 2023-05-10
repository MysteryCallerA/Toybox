using Microsoft.Xna.Framework;
using System;
using Utils.input;

namespace Toybox.components.control {
	public class JumpMover:EntityComponent { //TODO figure out easing functions and implement here

		public VirtualKey LeftKey;
		public VirtualKey RightKey;
		public VirtualKey JumpKey;
		public string FrontAnchorName = "front";
		public Func<bool> IsDirectionLocked;

		public int MaxMoveSpeed = 2;
		public float MoveStartup = 1f;
		public float MoveAccel = 0.5f;
		public float MoveDecel = 0.3f;

		public float JumpApexWindow = 0.5f;
		public float JumpApexGravityJumping = 0.15f;
		public float JumpApexGravityFalling = 0.3f;
		public float Gravity = 0.6f;
		public float MaxFallSpeed = 3;
		public float TerminalGravity = 0.1f;
		public float TerminalMaxFallSpeed = 5;
		public float JumpSpeed = -7f;
		public float JumpEarlyTerminationSpeed = -2f;
		public float HeadBonkSpeed = 0;

		private int AirTimer = 0;
		public int CoyoteTime = 3;
		private int JumpBufferTimer = 0;
		public int JumpBufferTime = 3;

		public Vector2 Speed = Vector2.Zero;
		private bool Jumping = false;

		public JumpMover(VirtualKey left, VirtualKey right, VirtualKey jump) {
			LeftKey = left;
			RightKey = right;
			JumpKey = jump;
		}

		public void Apply(ComplexEntity e) {
			var hitbox = e.GetHitbox();

			int dir = 0;
			if (LeftKey.Down) dir = -1;
			if (RightKey.Down) dir = 1;
			if (LeftKey.Down && RightKey.Down) dir = 0;

			//Acceleration / Deceleration
			if (dir == 0) {
				var before = Math.Sign(Speed.X);
				Speed.X -= MoveDecel * Math.Sign(Speed.X);
				if (Math.Sign(Speed.X) != before) Speed.X = 0;
			} else {
				if (Speed.X == 0) Speed.X = MoveStartup * dir;
				else Speed.X += MoveAccel * dir;
			}

			//Wall Bonking
			if (Speed.X > 0 && !e.Collider.RightClear(hitbox)) {
				Speed.X = 0;
			} else if (Speed.X < 0 && !e.Collider.LeftClear(hitbox)) {
				Speed.X = 0;
			}

			if (Math.Abs(Speed.X) > MaxMoveSpeed) Speed.X = MaxMoveSpeed * Math.Sign(Speed.X);

			//Jump Buffer
			if (JumpKey.Pressed) {
				JumpBufferTimer = JumpBufferTime;
			}

			if (!OnGround(e)) {
				//Coyote Jumping
				if (AirTimer < CoyoteTime && Speed.Y >= 0 && JumpKey.Pressed) {
					Speed.Y = JumpSpeed;
					Jumping = true;
				}

				//Head Bonk
				if (Speed.Y < 0 && !e.Collider.TopClear(hitbox)) {
					Speed.Y = HeadBonkSpeed;
					Jumping = false;
				}
				
				//Early Jump Termination
				if (Jumping && JumpKey.Released) {
					Jumping = false;
					if (Speed.Y < JumpEarlyTerminationSpeed) {
						Speed.Y = JumpEarlyTerminationSpeed;
					}
				}

				//Gravity
				if (Math.Abs(Speed.Y) < JumpApexWindow) {
					if (Jumping) Speed.Y += JumpApexGravityJumping;
					else Speed.Y += JumpApexGravityFalling;
				} else if (Speed.Y < MaxFallSpeed) {
					Speed.Y += Gravity;
				} else {
					Speed.Y += TerminalGravity;
				}
				if (Speed.Y > TerminalMaxFallSpeed) Speed.Y = TerminalMaxFallSpeed;
				
				AirTimer++;
			} else {
				AirTimer = 0;
				Speed.Y = 0;
				Jumping = false;
				//Jumping
				if (JumpBufferTimer > 0) {
					JumpBufferTimer = 0;
					Speed.Y = JumpSpeed;
					Jumping = true;
				}
			}

			if (JumpBufferTimer > 0) JumpBufferTimer--;

			e.Move(Speed.ToPoint());

			UpdateState(e);
			UpdateAnchor(e);
		}

		protected virtual void UpdateState(ComplexEntity e) {
			bool dlock = false;
			if (IsDirectionLocked != null) dlock = IsDirectionLocked.Invoke();

			if (!dlock) {
				if (Speed.X < 0) Direction = AnimationDirection.Left;
				else if (Speed.X > 0) Direction = AnimationDirection.Right;
			}
		}

		protected virtual void UpdateAnchor(ComplexEntity e) {
			Point start;
			Point end;
			if (Direction == AnimationDirection.Left) {
				start = e.GetHitbox().Location;
				end = new Point(start.X - 1, start.Y);
			} else {
				var hitbox = e.GetHitbox();
				start = new Point(hitbox.Right, hitbox.Y);
				end = new Point(start.X + 1, start.Y);
			}

			e.Anchors[FrontAnchorName] = new PointRay(start, end);
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

		public bool OnGround(ComplexEntity e) {
			if (Speed.Y < 0) return false;
			if (e.Collider.BotClear(e.GetHitbox())) return false;
			return true;
		}
	}
}
