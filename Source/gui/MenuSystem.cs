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
		public MenuElement ActiveMenu { get; protected set; }
		protected List<MenuElement> Menus = new();
		public Stack<MenuElement> History = new();

		public MenuControls Controls;

		public virtual void AddMenu(MenuElement m, string name = "") {
			Menus.Add(m);
			if (name != "") m.Name = name;
		}

		public void SwitchMenu(MenuElement m, bool addToHistory) {
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
			if (!Enabled) {
				if (Controls != null && Controls.OpenMenu != null && Controls.OpenMenu.Pressed) {
					OpenMenu();
					Controls.OpenMenu.DropPress();
				}
			}

			if (!Enabled || ActiveMenu == null) return;

			ActiveMenu.ParentSystem = this;
			ActiveMenu.Position = Position;
			ActiveMenu.UpdateFunction(Controls);
			ActiveMenu.UpdateSize(Resources.Camera.Bounds.Size);
			ActiveMenu.UpdateContainedElementPositions();
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
