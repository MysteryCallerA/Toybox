using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.utils.tween {
	public class ArcTween:Tween { //TODO  could be merged into statictween?

		private float[] Values;

		public ArcTween(int riseFrames, Func<float, float> riseFunction, float riseMultiplier, int fallFrames, Func<float, float> fallFunction) {
			Frames = riseFrames + fallFrames;
			Values = new float[riseFrames + fallFrames + 1];

			for (int i = 0; i <= riseFrames; i++) {
				Values[i] = -riseFunction.Invoke((float)i / riseFrames) * riseMultiplier;
			}

			var start = Values[riseFrames];
			var m = 1 + riseMultiplier;
			for (int i = 1; i <= fallFrames; i++) {
				Values[i + riseFrames] = (fallFunction.Invoke((float)i / fallFrames) * m) + start;
			}
		}

		private ArcTween(int frames, float[] values) {
			Frames = frames;
			Values = values;
		}

		public override float Get(int frame) {
			if (frame < 0) frame = 0;
			else if (frame > Frames) frame = Frames;
			return Values[frame];
		}

		public ArcTween MultiplyBy(float m) {
			var values = new float[Values.Length];
			for (int i = 0; i < Values.Length; i++) {
				values[i] = Values[i] * m;
			}
			return new ArcTween(Frames, values);
		}

	}
}
