using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;
using Toybox.gui.layout;
using Toybox.utils.data;

namespace Toybox.gui {

	public class MenuBox:MenuElement {

		public ObservableList<MenuElement> Content = new();
		private MenuElement _BackPanel;
		private MenuLayout _Layout;

		public MenuBox():this(new MenuVerticalLayout()) {
		}

		public MenuBox(MenuLayout layout) {
			Content.OnAdd = ElementAdded;
			Content.OnRemove = ElementRemoved;
			Layout = layout;
		}

		public override void Draw(Renderer r) {
			BackPanel?.Draw(r);
			Layout.DrawContent(r);
		}

		public void Update() {
			UpdateFunction(null, null, null);
			UpdateState();
			UpdateSize(Point.Zero);
			UpdateContentPositions();
		}

		protected internal override void UpdateFunction(MenuControlManager c, MenuSystem system, MenuStack stack) {
			foreach (var e in Content) {
				e.UpdateFunction(c, system, stack);
			}
		}

		protected internal override void UpdateState() {
			base.UpdateState();
			foreach (var e in Content) {
				e.UpdateState();
			}
		}

		protected override void UpdateContentSize(Point contentContainerSize, out Point contentSize) {
			Layout.UpdateContentSize(InnerSize, out contentSize);
			BackPanel?.UpdateSize(PanelSize);
		}

		protected internal override void UpdateContentPositions() {
			Layout.UpdateContentPosition();
			if (BackPanel != null) BackPanel.Position = PanelOrigin;
		}

		public override void Cascade(Action<MenuElement> a) {
			base.Cascade(a);
			foreach (var e in Content) {
				e.Cascade(a);
			}
		}

		private void ElementAdded(MenuElement e) { e.Parent = this; }
		private void ElementRemoved(MenuElement e) { if (e.Parent == this) e.Parent = null; }

		public MenuElement BackPanel {
			get { return _BackPanel; }
			set {
				_BackPanel = value;
				_BackPanel.Parent = this;
			}
		}

		public MenuLayout Layout {
			get { return _Layout; }
			set {
				_Layout = value;
				_Layout.Parent = this;
			}
		}
	}
}
