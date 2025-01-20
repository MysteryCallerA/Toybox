using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;
using Toybox.utils.input;

namespace Toybox.gui {
	public class MenuSystem {

		public bool Enabled = false;
		public Point Position = Point.Zero;
		public InteractiveMenu ActiveMenu { get; protected set; }
		protected List<InteractiveMenu> Menus = new();
		public Stack<InteractiveMenu> History = new();

		public MenuControls SharedControls;
		public MenuSelector SharedSelector;
		public VirtualKey KeyOpenMenu;
		public VirtualKey KeyCloseMenu;

		public virtual void AddMenu(InteractiveMenu m, string name = "") {
			Menus.Add(m);
			if (name != "") m.Name = name;
			m.InheritSystemProps(this);
		}

		public void SwitchMenu(InteractiveMenu m, bool addToHistory) {
			if (m == ActiveMenu) return;
			if (addToHistory) History.Push(ActiveMenu);
			ActiveMenu = m;
			if (m == null) return;

			ActiveMenu.Position = Position;
			ActiveMenu.PartialUpdate();
		}

		public bool TrySwitchMenu(string name, bool addToHistory) {
			foreach (var m in Menus) {
				if (m.Name != name) continue;
				SwitchMenu(m, addToHistory);
				return true;
			}
			return false;
		}

		public virtual void Draw(Renderer r) {
			if (!Enabled) return;
			ActiveMenu?.Draw(r);
		}

		public virtual void Update() {
			UpdateControl();
			if (!Enabled || ActiveMenu == null) return;

			ActiveMenu.Position = Position;
			ActiveMenu.Update(this);
		}

		protected virtual void UpdateControl() {
			if (!Enabled) {
				if (KeyOpenMenu != null && KeyOpenMenu.Pressed) {
					OpenMenu();
					KeyOpenMenu.DropPress();
				}
				return;
			}

			if (KeyCloseMenu != null && KeyCloseMenu.Pressed) {
				CloseMenu();
				KeyCloseMenu.DropPress();
			}
		}

		public virtual void OpenMenu() {
			Enabled = true;
		}

		public virtual void CloseMenu() {
			Enabled = false;

			var menu = ActiveMenu;
			while (History.Count > 0) {
				menu = History.Pop();
			}
			ActiveMenu = menu;
		}

		public virtual void BackMenu() {
			if (History.Count > 0) {
				ActiveMenu = History.Pop();
			} else {
				CloseMenu();
			}
		}

	}
}
