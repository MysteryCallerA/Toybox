using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox {

	public struct PointRay {

		public Point Start;
		public Point End;

		public PointRay(Point start, Point end) {
			Start = start;
			End = end;
		}

		public Point Direction {
			get { return End - Start; }
		}

		public int Xdir {
			get { return End.X - Start.X; }
		}

		public int Ydir {
			get { return End.Y - Start.Y; }
		}

		public override string ToString() {
			return $"PointRay{{X:{Start.X},Y:{Start.Y}}}->{{X:{End.X},Y{End.Y}}}";
		}

	}
}
