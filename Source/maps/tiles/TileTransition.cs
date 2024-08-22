using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.utils.tween;

namespace Toybox.maps.tiles {

	public class TileTransition {

		public int FramesElapsed = 0;
		public readonly int TargetFrames;
		public StaticTween OpacityTween;
		public StaticTween YTween;
		public StaticTween XTween;
		public bool RemoveOnEnd = false;

		public TileTransition(StaticTween opacity, StaticTween x, StaticTween y) {
			OpacityTween = opacity;
			XTween = x;
			YTween = y;
			if (opacity != null) TargetFrames = opacity.Frames;
			if (x != null) TargetFrames = Math.Max(TargetFrames, x.Frames);
			if (y != null) TargetFrames = Math.Max(TargetFrames, y.Frames);
		}

		public float Opacity {
			get {
				if (OpacityTween == null) return 1;
				return OpacityTween.Get(FramesElapsed);
			}
		}
		public int XOffset {
			get {
				if (XTween == null) return 0;
				return (int)XTween.Get(FramesElapsed);
			}
		}
		public int YOffset {
			get {
				if (YTween == null) return 0;
				return (int)YTween.Get(FramesElapsed);
			}
		}
		public Point Translation { get { return new Point(XOffset, YOffset); } }
		public bool Ended { get { return FramesElapsed >= TargetFrames; } }
	}
}
