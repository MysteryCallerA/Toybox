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
		public MenuElement Content;
		public MenuControl ConfirmKey = MenuControl.Confirm;
		public string RemoveTag;

		public MenuLink(MenuBox link, MenuElement content) {
			Link = link;
			Content = content;
			if (link != null && link.Tags.Count > 0) RemoveTag = link.Tags.First();
		}

		public override void Draw(Renderer r) {
			Content.Draw(r);
		}

		protected internal override void UpdateFunction(MenuControlManager c, MenuSystem parent) {
			Content.UpdateFunction(c, parent);

			if (c == null || parent == null) return;
			if (c.TryGet(ConfirmKey, out var key)) {
				if (key.Pressed) {
					Activate(parent);
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

		public void Activate(MenuSystem parent) {
			if (Link == null) return;
			parent.RemoveBox(RemoveTag);
			parent.AddBox(Link);
		}
	}
}
