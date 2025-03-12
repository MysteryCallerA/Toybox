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

namespace Toybox.gui
{
    public class MenuSystem {

		public List<MenuStack> Content = new();
		public MenuControlManager Controls = new();
		public MenuSelector Selector;

		public MenuSystem() {
		}

		public void Draw(Renderer r) {
			foreach (var stack in Content) {
				stack.Draw(r);
			}
			Selector.Draw(r);
		}

		public void Update() {
			Selector.UpdateFunction(Controls, this);
			foreach (var stack in Content) {
				stack.Update();
			}
			Selector.UpdateGraphic();
		}
		
	}
}
