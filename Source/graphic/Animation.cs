using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.graphic {
	public class Animation {

		private static int NextId = 0;

		public readonly int Id;
		public string Name = "";
		public int[] Frames;
		public int[] FrameTimes;

		public Animation(int[] frames, int frameTime) : this(frames, new int[] { frameTime }) {
		}
		public Animation(int frame) : this(new int[] { frame }, new int[] { -1 }) {
		}
		public Animation(int frame, int frameTime) : this(new int[] { frame }, new int[] { frameTime }) {
		}

		public Animation(int[] frames, int[] frameTimes) {
			Id = NextId;
			NextId++;
			Frames = frames;
			FrameTimes = frameTimes;
		}

		public override bool Equals(object obj) {
			if (obj == null || GetType() != obj.GetType()) return false;
			return Id == ((Animation)obj).Id;
		}

		public override int GetHashCode() {
			return Id.GetHashCode();
		}
	}
}
