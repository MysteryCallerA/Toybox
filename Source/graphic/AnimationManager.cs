using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Toybox.graphic {

	public class Animation {

		private static int NextId = 0;

		public readonly int Id;
		public int[] Frames;
		public int[] FrameTimes;

		public Animation(int[] frames, int frameTime):this(frames, new int[] { frameTime }) {
		}
		public Animation(int frame) : this(new int[] { frame }, new int[] { -1 }) {
		}
		public Animation(int frame, int frameTime):this(new int[] { frame }, new int[] { frameTime }) {
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

	public class AnimationManager {

		public Animation Animation;
		public float FrameTimer = 0;
		public int ElapsedFrames = 0;
		public float Speed = 1;
		public float OverallTimer = 0;
		private enum TriggerType { None, NextFrame, MinTime, MinFrameTime }

		public Action OnAnimationComplete;

		private Animation NextAnimation;
		private TriggerType NextAnimTrigger = TriggerType.None;
		private int MinTime = 0;

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

		public void Start(Animation a) {
			Animation = a;
			FrameTimer = 0;
			ElapsedFrames = 0;
			OverallTimer = 0;
		}

		public void StartNextFrame(Animation a) {
			SetupTrigger(a, TriggerType.NextFrame);
		}

		public void StartAfterMinTime(Animation a, int minTime) {
			SetupTrigger(a, TriggerType.MinTime);
			MinTime = minTime;
		}

		public void StartAfterMinFrameTime(Animation a, int minTime) {
			SetupTrigger(a, TriggerType.MinFrameTime);
			MinTime = minTime;
		}

		private void SetupTrigger(Animation a, TriggerType t) {
			NextAnimation = a;
			NextAnimTrigger = t;
		}

		public void Update() {
			if (Animation == null) return;

			OverallTimer += Speed;
			if (NextAnimTrigger == TriggerType.MinTime && OverallTimer > MinTime) {
				StartNextAnimation();
				return;
			}

			if (FrameTime == -1) return;

			FrameTimer += Speed;

			if (NextAnimTrigger == TriggerType.MinFrameTime && FrameTimer > MinTime) {
				StartNextAnimation();
				return;
			}

			if (FrameTimer > FrameTime) {
				if (NextAnimTrigger == TriggerType.NextFrame) {
					StartNextAnimation();
					return;
				}

				ElapsedFrames++;
				FrameTimer = 0;
				if (ElapsedFrames >= Frames) {
					ElapsedFrames = 0;
					OnAnimationComplete?.Invoke();
				}
			}
		}

		private void StartNextAnimation() {
			Start(NextAnimation);
			NextAnimation = null;
			NextAnimTrigger = TriggerType.None;
		}

		public void Reset() {
			ElapsedFrames = 0;
			FrameTimer = 0;
			OverallTimer = 0;
		}

	}
}
