using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;
using Toybox.gui.layout;

namespace Toybox.gui {

	public class MenuBox:MenuElement {

		public List<MenuElement> Content = new();
		public MenuElement BackPanel;
		public IMenuLayout Layout = new MenuVerticalLayout();

		public MenuBox() {

		}

		public override void Draw(Renderer r) {
			BackPanel?.Draw(r);
			foreach (var e in Content) {
				e.Draw(r);
			}
		}

		protected internal override void UpdateFunction(MenuControls c) {
			var control = Controls ?? c;

			foreach (var e in Content) {
				e.UpdateFunction(control);
			}
		}

		protected internal override void UpdateState() {
			base.UpdateState();
			foreach (var e in Content) {
				e.UpdateState();
			}
		}

		protected override void UpdateContentSize(Point contentContainerSize, out Point contentSize) {
			Layout.UpdateContentSize(Content, InnerSize, out contentSize);
			BackPanel?.UpdateSize(PanelSize);
		}

		protected internal override void UpdateContainedElementPositions() {
			Layout.UpdateContentPosition(Content, this);
			if (BackPanel != null) BackPanel.Position = PanelOrigin;
		}

		public override void Cascade(Action<MenuElement> a) {
			base.Cascade(a);
			foreach (var e in Content) {
				e.Cascade(a);
			}
		}
	}
}
