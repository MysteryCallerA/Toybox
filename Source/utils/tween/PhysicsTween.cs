using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.utils.tween {
	public class PhysicsTween:Tween {

		private float[] ReturnValues;

		public PhysicsTween(float initialForce, float acceleration, float endValue, float? terminalVelocity = null) {
			var values = new List<float>();
			values.Add(0);
			float speed = initialForce;
			float next = speed;

			while (Math.Sign(endValue - initialForce) == Math.Sign(endValue - next)) {
				values.Add(next);
				speed += acceleration;
				if (terminalVelocity.HasValue) {
					var t = Math.Abs(terminalVelocity.Value);
					if (speed > t) {
						speed = t;
					} else if (speed < -t) {
						speed = -t;
					}
				}
				next += speed;
			}
			values.Add(endValue);
			ReturnValues = values.ToArray();
			Frames = ReturnValues.Length - 1;
		}

		public override float Get(int frame) {
			if (frame < 0) frame = 0;
			else if (frame > Frames) frame = Frames;
			return ReturnValues[frame];
		}
	}
}
