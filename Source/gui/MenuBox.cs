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
		public IMenuLayout Layout;

		public bool UseSystemPosition = true;
		public HashSet<string> Tags = new();

		public MenuBox():this(new MenuVerticalLayout()) { }
		public MenuBox(IMenuLayout layout) {
			Layout = layout;
			Content.OnAdd = ElementAdded;
			Content.OnRemove = ElementRemoved;
		}

		public override void Draw(Renderer r) {
			BackPanel?.Draw(r);
			foreach (var e in Content) {
				e.Draw(r);
			}
		}

		public void Update() {
			UpdateFunction(null, null);
			UpdateState();
			UpdateSize(Point.Zero);
			UpdateContentPositions();
		}

		protected internal override void UpdateFunction(MenuControlManager c, MenuSystem parent) {
			foreach (var e in Content) {
				e.UpdateFunction(c, parent);
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

		protected internal override void UpdateContentPositions() {
			Layout.UpdateContentPosition(Content, this);
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
	}
}
