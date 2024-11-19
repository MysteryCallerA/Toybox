using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Toybox.graphic {

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
			if (Animation == a) return;
			Animation = a;
			FrameTimer = 0;
			ElapsedFrames = 0;
			OverallTimer = 0;
		}

		public void StartNextFrame(Animation a) {
			if (Animation == a) return;
			SetupTrigger(a, TriggerType.NextFrame);
		}

		public void StartAfterMinTime(Animation a, int minTime) {
			if (Animation == a) return;
			SetupTrigger(a, TriggerType.MinTime);
			MinTime = minTime;
		}

		public void StartAfterMinFrameTime(Animation a, int minTime) {
			if (Animation == a) return;
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

			if (FrameTime == -1) {
				if (NextAnimTrigger == TriggerType.NextFrame) StartNextAnimation();
				return;
			}

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
