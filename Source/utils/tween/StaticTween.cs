using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.utils.tween {
	public class StaticTween {

		public readonly int Frames;
		private float[] ReturnValues;

		public StaticTween(Func<float, float> easingFunction, int frames) {
			Frames = frames;

			float[] r = new float[frames + 1];
			for (int i = 0; i <= frames; i++) {
				r[i] = easingFunction.Invoke((float)i / frames);
			}
			ReturnValues = r;
		}

		public float Get(int frame) {
			if (frame < 0) frame = 0;
			else if (frame > Frames) frame = Frames;
			return ReturnValues[frame];
		}

		public void Reverse() {
			ReturnValues = ReturnValues.Reverse().ToArray();
		}

		public void MultiplyBy(float m) {
			for (int i = 0; i < ReturnValues.Length; i++) {
				ReturnValues[i] = ReturnValues[i] * m;
			}
		}

	}
}
