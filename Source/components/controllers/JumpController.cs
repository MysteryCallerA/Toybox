using Microsoft.Xna.Framework;
using System;
using Toybox.utils;
using Utils.input;

namespace Toybox.components.controllers {
	public class JumpController { //TODO figure out easing functions and implement here

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

		private bool Jumping = false;

		public JumpController(VirtualKey left, VirtualKey right, VirtualKey jump) {
			LeftKey = left;
			RightKey = right;
			JumpKey = jump;
		}

		public void Update(ComplexEntity e) {
			var hitbox = e.Hitbox.Bounds;
			var speed = e.Collider.Speed;

			int dir = 0;
			if (LeftKey.Down) dir = -1;
			if (RightKey.Down) dir = 1;
			if (LeftKey.Down && RightKey.Down) dir = 0;

			//Acceleration / Deceleration
			if (dir == 0) {
				var before = Math.Sign(speed.X);
				speed.X -= MoveDecel * Math.Sign(speed.X);
				if (Math.Sign(speed.X) != before) speed.X = 0;
			} else {
				if (speed.X == 0) speed.X = MoveStartup * dir;
				else speed.X += MoveAccel * dir;
			}

			//Wall Bonking
			if (speed.X > 0 && !e.Collider.RightClear(hitbox)) {
				speed.X = 0;
			} else if (speed.X < 0 && !e.Collider.LeftClear(hitbox)) {
				speed.X = 0;
			}

			if (Math.Abs(speed.X) > MaxMoveSpeed) speed.X = MaxMoveSpeed * Math.Sign(speed.X);

			//Jump Buffer
			if (JumpKey.Pressed) {
				JumpBufferTimer = JumpBufferTime;
			}

			if (!OnGround(e)) {
				//Coyote Jumping
				if (AirTimer < CoyoteTime && speed.Y >= 0 && JumpKey.Pressed) {
					speed.Y = JumpSpeed;
					Jumping = true;
				}

				//Head Bonk
				if (speed.Y < 0 && !e.Collider.TopClear(hitbox)) {
					speed.Y = HeadBonkSpeed;
					Jumping = false;
				}
				
				//Early Jump Termination
				if (Jumping && JumpKey.Released) {
					Jumping = false;
					if (speed.Y < JumpEarlyTerminationSpeed) {
						speed.Y = JumpEarlyTerminationSpeed;
					}
				}

				//Gravity
				if (Math.Abs(speed.Y) < JumpApexWindow) {
					if (Jumping) speed.Y += JumpApexGravityJumping;
					else speed.Y += JumpApexGravityFalling;
				} else if (speed.Y < MaxFallSpeed) {
					speed.Y += Gravity;
				} else {
					speed.Y += TerminalGravity;
				}
				if (speed.Y > TerminalMaxFallSpeed) speed.Y = TerminalMaxFallSpeed;
				
				AirTimer++;
			} else {
				AirTimer = 0;
				speed.Y = 0;
				Jumping = false;
				//Jumping
				if (JumpBufferTimer > 0) {
					JumpBufferTimer = 0;
					speed.Y = JumpSpeed;
					Jumping = true;
				}
			}

			if (JumpBufferTimer > 0) JumpBufferTimer--;

			e.Collider.Speed = speed;

			UpdateState(e);
			UpdateAnchor(e);
		}

		protected virtual void UpdateState(ComplexEntity e) {
			bool dlock = false;
			if (IsDirectionLocked != null) dlock = IsDirectionLocked.Invoke();

			if (!dlock) {
				if (e.Collider.Speed.X < 0) Direction = AnimationDirection.Left;
				else if (e.Collider.Speed.X > 0) Direction = AnimationDirection.Right;
			}
		}

		protected virtual void UpdateAnchor(ComplexEntity e) {
			Point start;
			Point end;
			if (Direction == AnimationDirection.Left) {
				start = e.Hitbox.Bounds.Location;
				end = new Point(start.X - 1, start.Y);
			} else {
				var hitbox = e.Hitbox.Bounds;
				start = new Point(hitbox.Right, hitbox.Y);
				end = new Point(start.X + 1, start.Y);
			}

			e.Anchors[FrontAnchorName] = new Line(start, end);
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
			if (e.Collider.Speed.Y < 0) return false;
			if (e.Collider.BotClear(e.Hitbox.Bounds)) return false;
			return true;
		}
	}
}
