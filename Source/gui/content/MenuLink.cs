using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;

namespace Toybox.gui.content {
	public class MenuLink:MenuElement {

		public MenuBox Link;
		private MenuElement _Content;
		public MenuControl ConfirmKey = MenuControl.Confirm;

		public MenuLink(MenuBox link, MenuElement content) {
			Link = link;
			Content = content;
		}

		public override void Draw(Renderer r) {
			Content.Draw(r);
		}

		protected internal override void UpdateFunction(MenuControlManager c, MenuStack stack) {
			Content.UpdateFunction(c, stack);

			if (c == null || stack == null) return;
			if (c.TryGet(ConfirmKey, out var key)) {
				if (key.Pressed) {
					Activate(stack);
					key.DropPress();
				}
			}
		}

		protected internal override void UpdateState() {
			base.UpdateState();
			Content.UpdateState();
		}

		protected override void UpdateContentSize(Point contentContainerSize, out Point contentSize) {
			if (Content == null) {
				contentSize = Point.Zero;
				return;
			}
			Content.UpdateSize(contentContainerSize);
			contentSize = Content.OuterSize;
		}

		protected internal override void UpdateContentPositions() {
			if (Content == null) return;
			Content.Position = ContentOrigin;
			Content.UpdateContentPositions();
		}

		public override void Cascade(Action<MenuElement> a) {
			base.Cascade(a);
			Content.Cascade(a);
		}

		public void Activate(MenuStack stack) {
			if (Link == null) return;
			stack.Push(Link);
		}

		public MenuElement Content {
			get { return _Content; }
			set {
				_Content = value;
				_Content.Parent = this;
			}
		}
	}
}
