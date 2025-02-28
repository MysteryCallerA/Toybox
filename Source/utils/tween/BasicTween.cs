using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.utils.tween {
	public class BasicTween:Tween {

		public Func<float, float> Function;

		public BasicTween(Func<float, float> easingFunction, int frames) {
			Frames = frames;
			Function = easingFunction;
		}

		public override float Get(int frame) {
			if (frame < 0) frame = 0;
			else if (frame > Frames) frame = Frames;
			return Function.Invoke((float)frame / Frames);
		}
	}
}
