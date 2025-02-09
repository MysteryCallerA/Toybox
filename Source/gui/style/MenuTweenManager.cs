using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;

namespace Toybox.gui.style {
	public class MenuTweenManager {

		private Dictionary<int, MenuTween> Content = new();
		private List<int> FinishedTweens = new();

		public MenuTweenManager() {
		}

		public void Add(MenuTween t) {
			if (Content.ContainsKey(t.Field.Id)) {
				if (Content[t.Field.Id].End == t.End) return;
				Content.Remove(t.Field.Id);
			}
			Content.Add(t.Field.Id, t);
		}

		public void Clear(StyleField f) {
			if (Content.ContainsKey(f.Id)) {
				Content.Remove(f.Id);
			}
		}

		public void UpdateTweens(MenuElement e) {
			if (Content.Count == 0) return;

			foreach (var tween in Content) {
				tween.Value.Apply(e);
				if (tween.Value.Finished) {
					FinishedTweens.Add(tween.Key);
				}
			}

			if (FinishedTweens.Count == 0) return;
			foreach (var id in FinishedTweens) {
				Content.Remove(id);
			}
			FinishedTweens.Clear();
		}

	}
}
