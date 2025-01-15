using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;

namespace Toybox.gui {

	public class Menu:MenuElement {

		public List<MenuElement> Content = new();
		public MenuElement BackPanel;
		public IMenuLayout Layout;

		public Menu() {

		}

		public override void Draw(Renderer r) {
			BackPanel?.Draw(r);
			foreach (var e in Content) {
				e.Draw(r);
			}
		}

		protected internal override void UpdateFunction() {
			foreach (var e in Content) {
				e.UpdateFunction();
			}
			BackPanel.UpdateFunction();
		}

		protected override void GetContentSize(out Point contentSize) {
			Layout.UpdateContentSize(Content, this, out contentSize);
		}

		protected override void FinalizeSize() {
			BackPanel.UpdateSize(PanelSize);
		}

		protected internal override void UpdateContainedElementPositions() {
			Layout.UpdateContentPosition(Content, this);
			BackPanel.Position = PanelOrigin;
		}
	}
}
