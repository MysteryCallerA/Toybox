using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;
using Toybox.gui.select;
using Toybox.gui.style;
using Toybox.utils.data;

namespace Toybox.gui {
	public class MenuSystem {

		private List<MenuBox> Content = new();
		public Point Position;
		public Point Size;
		public MenuControlManager Controls = new();
		public MenuSelector Selector;

		private readonly List<MenuBox> ToAdd = new();
		private readonly List<MenuBox> ToRemove = new();

		public MenuSystem() {
		}

		public void Draw(Renderer r) {
			foreach (var m in Content) {
				m.Draw(r);
			}
			Selector.Draw(r);
		}

		public void Update() {
			ApplyChanges();

			foreach (var m in Content) {
				if (m.UseSystemPosition) m.Position = Position;

				//m.UpdateFunction(Controls, this);
				m.UpdateState();
				m.UpdateSize(Size);
				m.UpdateContentPositions();
			}
			Selector.Update(Controls, this);

			ApplyChanges();
		}

		public void AddBox(MenuBox b) {
			ToAdd.Add(b);
		}

		public void RemoveBox(string tag) {
			foreach (var b in Content) {
				if (b.Tags.Contains(tag)) {
					ToRemove.Add(b);
				}
			}
		}

		private void ApplyChanges() {
			if (ToRemove.Count > 0) {
				foreach (var b in ToRemove) {
					Content.Remove(b);
				}
				ToRemove.Clear();
			}

			if (ToAdd.Count > 0) {
				foreach (var b in ToAdd) {
					Content.Add(b);
				}
				ToAdd.Clear();
			}
		}
		
	}
}
