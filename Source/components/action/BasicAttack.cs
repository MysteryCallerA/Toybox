using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.components.action {
	public class BasicAttack {

		public enum BasicAttackStep { Inactive, Attack, Hold, Return }

		public bool Attacking { get { return CurrentStep != BasicAttackStep.Inactive; } }
		public BasicAttackStep CurrentStep = BasicAttackStep.Inactive;
		public Point CurrentOffset;
		public Point TargetOffset;

		public int Timer = 0;
		public int AttackTime;
		public int HoldTime;
		public int ReturnTime;

		public BasicAttack() {

		}

		public void Start(Point targetOffset, int attackTime, int holdTime, int returnTime) {
			CurrentStep = BasicAttackStep.Attack;
			CurrentOffset = Point.Zero;
			TargetOffset = targetOffset;
			AttackTime = attackTime;
			HoldTime = holdTime;
			ReturnTime = returnTime;
			Timer = 0;
		}

		public void Update() {
			if (!Attacking) return;
			Timer++;

			if (CurrentStep == BasicAttackStep.Attack) {
				var percent = (float)Timer / AttackTime;
				CurrentOffset = new Point((int)Math.Round(TargetOffset.X * percent), (int)Math.Round(TargetOffset.Y * percent));
				if (Timer > AttackTime) {
					if (HoldTime == 0) CurrentStep = BasicAttackStep.Return;
					else CurrentStep = BasicAttackStep.Hold;
					Timer = 0;
				}
			} else if (CurrentStep == BasicAttackStep.Hold) {
				CurrentOffset = TargetOffset;
				if (Timer > HoldTime) {
					CurrentStep = BasicAttackStep.Return;
					Timer = 0;
				}
			} else if (CurrentStep == BasicAttackStep.Return) {
				var percent = (float)(ReturnTime - Timer) / ReturnTime;
				CurrentOffset = new Point((int)Math.Round(TargetOffset.X * percent), (int)Math.Round(TargetOffset.Y * percent));
				if (Timer > ReturnTime) {
					CurrentStep = BasicAttackStep.Inactive;
					CurrentOffset = Point.Zero;
				}
			}
		}

	}
}
