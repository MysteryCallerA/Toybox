using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;
using Toybox.utils.tween;

namespace Toybox.gui.style {
	public class MenuTween {

		public Tween Tween;
		public StyleField Field;
		public int Frame = 0;
		public int Start;
		public int End;
		public bool Finished = false;

		public MenuTween(Tween t, StyleField f, int endValue) {
			Tween = t;
			Field = f;
			End = endValue;
		}

		public MenuTween(Func<float, float> easingFunction, int frames, StyleField f, int endValue) {
			Tween = new BasicTween(easingFunction, frames);
			Field = f;
			End = endValue;
		}

		public void Apply(MenuElement e) {
			if (Finished) return;
			if (Frame == 0) {
				Start = e.GetStyleValue(Field);
			}

			Frame++;
			int value = (int)Math.Round(Start + Tween.Get(Frame) * (End - Start));
			e.ApplyStyleValue(Field, value);
			if (Frame >= Tween.Frames) {
				Finished = true;
			}
		}


	}
}
