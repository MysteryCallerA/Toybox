using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;

namespace Toybox.gui.style {
	public class MenuStateManager {

		private MenuElement Parent;
		private readonly HashSet<MenuState> Content = new();
		private readonly HashSet<MenuState> InheritedState = new();

		public MenuStateManager(MenuElement e) {
			Parent = e;
		}

		public void Add(MenuState s) {
			if (Content.Contains(s)) return;
			Content.Add(s);
			Parent.UpdateStyle();
		}

		public void Remove(MenuState s) {
			if (!Content.Contains(s)) return;
			Content.Remove(s);
			Parent.UpdateStyle();
		}

		public bool Matches(MenuState[] s) {
			UpdateInheritedState();
			foreach (var state in s) {
				if (!InheritedState.Contains(state) && !Content.Contains(state)) return false;
			}
			return true;
		}

		private void UpdateInheritedState() {
			InheritedState.Clear();
			var pointer = Parent;
			while (pointer.Parent != null) {
				pointer = pointer.Parent;
				foreach (var state in pointer.State.Content) {
					InheritedState.Add(state);
				}
			}
		}

	}
}
