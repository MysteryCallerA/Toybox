using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.utils.tween {
	public class TweenSystem {

		public Tween Opacity, X, Y;

		public TweenSystem() {
		}

		public int Frames {
			get {
				var output = 0;
				if (Opacity != null && Opacity.Frames > output) output = Opacity.Frames;
				if (X != null && X.Frames > output) output = X.Frames;
				if (Y != null && Y.Frames > output) output = Y.Frames;
				return output;
			}
		}

		public Point GetOffset(int frame) {
			var x = 0;
			var y = 0;
			if (X != null) x = (int)X.Get(frame);
			if (Y != null) y = (int)Y.Get(frame);
			return new Point(x, y);
		}

		public float GetX(int frame) {
			if (X != null) return X.Get(frame);
			return 0;
		}

		public float GetY(int frame) {
			if (Y != null) return Y.Get(frame);
			return 0;
		}

		public float GetOpacity(int frame) {
			if (Opacity != null) return Opacity.Get(frame);
			return 1;
		}

	}
}
