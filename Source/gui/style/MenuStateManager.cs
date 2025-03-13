using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;

namespace Toybox.gui.style {
	public class MenuStateManager {

		public Action<MenuElement> OnChanged;

		private bool StateChanged = false;
		private bool ParentStateChanged = false;

		private readonly MenuElement Parent;
		private readonly HashSet<MenuState> Content = new();
		private readonly HashSet<MenuState> InheritedState = new();
		private readonly HashSet<MenuState> NewStates = new();

		public MenuStateManager(MenuElement e) {
			Parent = e;
		}

		public void Add(MenuState s) {
			if (Content.Contains(s)) return;
			Content.Add(s);
			StateChanged = true;
			if (!InheritedState.Contains(s)) {
				NewStates.Add(s);
			}
		}

		public void Remove(MenuState s) {
			if (!Content.Contains(s)) return;
			Content.Remove(s);
			StateChanged = true;
			if (!InheritedState.Contains(s)) {
				NewStates.Remove(s);
			}
		}

		public bool Get(MenuState s, bool includeInherited = true) {
			if (Content.Contains(s)) return true;
			if (includeInherited && InheritedState.Contains(s)) return true;
			return false;
		}

		public bool Get(MenuState s, out bool isNew, bool includeInherited = true) {
			if (Get(s, includeInherited)) {
				isNew = NewStates.Contains(s);
				return true;
			}
			isNew = false;
			return false;
		}

		public void Update() {
			if (!StateChanged && !ParentStateChanged) return;

			UpdateInheritedState();
			OnChanged?.Invoke(Parent);
			if (StateChanged) Parent.Cascade(ApplyParentStateChanged);

			StateChanged = false;
			ParentStateChanged = false;
		}

		private void ApplyParentStateChanged(MenuElement e) {
			e.State.ParentStateChanged = true;
		}

		private void UpdateInheritedState() {
			InheritedState.Clear();
			if (Parent.Parent == null) return;
			var inheritFrom = Parent.Parent.State;

			foreach (var s in inheritFrom.Content) {
				InheritedState.Add(s);
			}
			foreach (var s in inheritFrom.InheritedState) {
				InheritedState.Add(s);
			}
			foreach (var s in inheritFrom.NewStates) {
				NewStates.Add(s);
			}
		}

	}
}
