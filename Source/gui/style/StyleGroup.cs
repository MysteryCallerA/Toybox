using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;

namespace Toybox.gui.style {
	public class StyleGroup {

		public List<Style> Content = new();

		public Style GetFirstMatch(MenuElement e) {
			foreach (var s in Content) {
				if (e.State.Matches(s.State)) return s;
			}
			return null;
		}

		public void Add(Style s) {
			Content.Add(s);
		}
	}
}
