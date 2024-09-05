using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.utils.tween {
	public abstract class Tween {

		public int Frames { get; protected set; }

		public abstract float Get(int frame);

	}
}
