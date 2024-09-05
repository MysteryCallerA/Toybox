using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.utils.tween {
	public class StaticTween:Tween {

		private float[] ReturnValues;

		public StaticTween(Func<float, float> easingFunction, int frames) {
			Frames = frames;

			float[] r = new float[frames + 1];
			for (int i = 0; i <= frames; i++) {
				r[i] = easingFunction.Invoke((float)i / frames);
			}
			ReturnValues = r;
		}

		private StaticTween(int frames, float[] returnvalues) {
			Frames = frames;
			ReturnValues = returnvalues;
		}

		public override float Get(int frame) {
			if (frame < 0) frame = 0;
			else if (frame > Frames) frame = Frames;
			return ReturnValues[frame];
		}

		public StaticTween Reverse() {
			return new StaticTween(Frames, ReturnValues.Reverse().ToArray());
		}

		public StaticTween MultiplyBy(float m) {
			var values = new float[ReturnValues.Length];
			for (int i = 0; i < ReturnValues.Length; i++) {
				values[i] = ReturnValues[i] * m;
			}
			return new StaticTween(Frames, values);
		}

	}
}
