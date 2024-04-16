using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Toybox.graphic {
	public class AnimationCollection {

		private readonly Dictionary<int, Animation> Animations = new();

		public AnimationCollection(params Animation[] animations) {
			foreach (var a in animations) {
				Animations.Add(a.Id, a);
			}
		}

		public Animation this[int id] {
			get { return Animations[id]; }
		}

	}

	public class Animation {

		public readonly int Id;
		public int[] Frames;
		public int[] FrameTimes;

		public Animation(int id, int[] frames, int frameTime) {
			Id = id;
			Frames = frames;
			FrameTimes = new int[] { frameTime };
		}

		public Animation(int id, int[] frames, int[] frameTimes) {
			Id = id;
			Frames = frames;
			FrameTimes = frameTimes;
		}

	}

	public class AnimationManager {

		public AnimationCollection Animations;
		private Animation Animation;
		public float FrameTimer = 0;
		public int ElapsedFrames = 0;
		public float Speed = 1;
		public Action OnAnimationComplete;

		public AnimationManager(AnimationCollection a) {
			Animations = a;
		}
		public AnimationManager() { }

		public int AnimationId { get { return Animation.Id; } }
		public int Frames { get { return Animation.Frames.Length; } }
		public int FrameTime {
			get {
				if (Animation.FrameTimes.Length == 1) return Animation.FrameTimes[0];
				return Animation.FrameTimes[ElapsedFrames];
			}
		}
		public int Frame { get { return Animation.Frames[ElapsedFrames]; } }
		public bool NoAnimation { get { return Animation == null; } }

		public void StartAnimation(Animation a) {
			Animation = a;
			FrameTimer = 0;
			ElapsedFrames = 0;
		}

		public void StartAnimation(int id) {
			StartAnimation(Animations[id]);
		}

		/// <summary> Switch playing Animation without reseting the frame and frameTimer. </summary>
		public void BlendAnimation(Animation a) {
			Animation = a;
		}

		/// <summary> Switch playing Animation without reseting the frame and frameTimer. </summary>
		public void BlendAnimation(int id) {
			BlendAnimation(Animations[id]);
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
