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

		public MenuTween(Tween t, StyleField f, int start, int end) {
			Tween = t;
			Field = f;
			Start = start;
			End = end;
		}

		public void Apply(MenuElement e) {
			if (Finished) return;
			Frame++;
			int value = (int)Math.Round(Start + Tween.Get(Frame) * (End - Start));
			e.ApplyStyleValue(Field, value);
			if (Frame >= Tween.Frames) {
				Finished = true;
			}
		}


	}
}
