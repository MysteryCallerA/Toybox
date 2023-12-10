using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Toybox.graphic {
	public class AnimationManager {

		private Dictionary<string, Animation> Animations = new Dictionary<string, Animation>();

		public AnimationManager(params Animation[] animations) {
			foreach (var a in animations) {
				Animations.Add(a.Name, a);
			}
		}

		public Animation this[string name] {
			get { return Animations[name]; }
		}

	}

	public class Animation {

		public readonly string Name;
		public int[] Frames;
		public int[] FrameTimes;

		public Animation(string name, int[] frames, int frameTime) {
			Name = name;
			Frames = frames;
			FrameTimes = new int[] { frameTime };
		}

		public Animation(string name, int[] frames, int[] frameTimes) {
			Name = name;
			Frames = frames;
			FrameTimes = frameTimes;
		}

	}

	public class AnimationState {

		private Animation Animation;
		public int FrameTimer = 0;
		public int ElapsedFrames = 0;
		public int Speed = 1;
		public Action OnAnimationComplete;

		public AnimationState() {
		}

		public string Name { get { return Animation.Name; } }
		public int Frames { get { return Animation.Frames.Length; } }
		public int FrameTime {
			get {
				if (Animation.FrameTimes.Length == 1) return Animation.FrameTimes[0];
				return Animation.FrameTimes[ElapsedFrames];
			}
		}
		public int Frame { get { return Animation.Frames[ElapsedFrames]; } }

		public void StartAnimation(Animation a) {
			Animation = a;
			FrameTimer = 0;
			ElapsedFrames = 0;
		}

		/// <summary> Switch playing Animation without reseting the frame and frameTimer. </summary>
		public void BlendAnimation(Animation a) {
			Animation = a;
		}

		public void Update() {
			if (Animation == null) return;

			FrameTimer += Speed;
			if (FrameTimer > FrameTime) {
				ElapsedFrames++;
				FrameTimer = 0;
				if (ElapsedFrames >= Frames) {
					ElapsedFrames = 0;
					OnAnimationComplete?.Invoke();
				}
			}
		}

		public void Reset() {
			ElapsedFrames = 0;
			FrameTimer = 0;
		}

	}
}
