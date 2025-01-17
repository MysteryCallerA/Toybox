using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;
using Toybox.utils.input;

namespace Toybox.gui {
	public class MenuSystem {

		public bool Enabled = true;
		private SelectMenu _ActiveMenu;
		protected List<SelectMenu> Menus = new();
		public Stack<SelectMenu> History = new();

		public MenuController SharedController;
		public MenuSelector SharedSelector;
		public VirtualKey KeyOpenMenu;
		public VirtualKey KeyCloseMenu;
		public VirtualKey KeyBack;

		public SelectMenu ActiveMenu {
			get { return _ActiveMenu; }
			set {
				if (_ActiveMenu == value) return;
				_ActiveMenu = value;
				_ActiveMenu.PartialUpdate();
			}
		}

		public virtual void Draw(Renderer r) {
			if (!Enabled) return;
			ActiveMenu?.Draw(r);
		}

		public virtual void Update() {
			UpdateControl();
			if (!Enabled) return;
			ActiveMenu?.Update(this);
		}

		protected virtual void UpdateControl() {
			if (!Enabled) {
				if (KeyOpenMenu != null && KeyOpenMenu.Pressed) OpenMenu();
				return;
			}

			if (KeyCloseMenu != null && KeyCloseMenu.Pressed) CloseMenu();
			if (KeyBack != null && KeyBack.Pressed) BackMenu();
		}

		public virtual void AddMenu(SelectMenu m, string name = "") {
			Menus.Add(m);
			if (name != "") m.Name = name;
			if (SharedController != null) m.Controller = SharedController;
			if (SharedSelector != null) m.Selector = SharedSelector;
		}

		protected internal virtual void PressedConfirm(MenuElement e, int selectionPos) {
			var name = e.Name;
			foreach (var m in Menus) {
				if (m.Name != name) continue;
				History.Push(ActiveMenu);
				ActiveMenu = m;
				return;
			}
		}

		protected internal virtual void PressedCancel(MenuElement e, int selectionPos) {
			if (History.Count > 0) {
				ActiveMenu = History.Pop();
			}
		}

		public virtual void OpenMenu() {
			Enabled = true;
			KeyOpenMenu?.DropPress();
		}

		public virtual void CloseMenu() {
			Enabled = false;

			var menu = ActiveMenu;
			while (History.Count > 0) {
				menu = History.Pop();
			}
			ActiveMenu = menu;
			KeyCloseMenu?.DropPress();
		}

		public virtual void BackMenu() {
			if (History.Count > 0) {
				ActiveMenu = History.Pop();
			} else {
				CloseMenu();
			}
			KeyBack?.DropPress();
		}

	}
}
