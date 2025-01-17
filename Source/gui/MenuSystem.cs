using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;

namespace Toybox.gui {
	public class MenuSystem {

		private SelectMenu _ActiveMenu;
		protected List<SelectMenu> Menus = new();
		public Stack<SelectMenu> History = new();

		public SelectMenu ActiveMenu {
			get { return _ActiveMenu; }
			set {
				if (_ActiveMenu == value) return;
				_ActiveMenu = value;
				_ActiveMenu.PartialUpdate();
			}
		}

		public virtual void Draw(Renderer r) {
			ActiveMenu?.Draw(r);
		}

		public virtual void Update() {
			ActiveMenu?.Update(this);
		}

		public virtual void AddMenu(SelectMenu m, string name = "") {
			Menus.Add(m);
			if (name != "") m.Name = name;
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

	}
}
