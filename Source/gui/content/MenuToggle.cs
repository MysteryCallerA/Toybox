using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;
using Toybox.gui.graphic;
using Toybox.utils.data;
using Toybox.utils.math;

namespace Toybox.gui.content {
	public class MenuToggle:MenuElement {

		public int StateId { get; private set; }
		public ObservableList<MenuElement> States = new();

		public MenuElement CurrentState {
			get { return States[StateId]; }
		}

		public MenuToggle() {
			Fit = FitType.FitContent;
			VAlign = VAlignType.Center;
			States.OnAdd = ElementAdded;
			States.OnRemove = ElementRemoved;
		}

		public override void Draw(Renderer r) {
			if (States.Count == 0) return;
			CurrentState.Draw(r);
		}

		protected internal override void UpdateFunction(MenuControls c) {
			c = Controls ?? c;
			if (c == null) return;

			if (c.Confirm != null && c.Confirm.Pressed) {
				Activate();
				c.Confirm.DropPress();
			}
		}

		protected internal override void UpdateState() {
			base.UpdateState();
			foreach (var e in States) {
				e.UpdateState();
			}
		}

		protected override void UpdateContentSize(Point contentContainerSize, out Point contentSize) {
			int x = 0, y = 0;
			foreach (var e in States) {
				e.UpdateSize(contentContainerSize);
				var size = e.OuterSize;
				x = Math.Max(size.X, x);
				y = Math.Max(size.Y, y);
			}
			contentSize = new Point(x, y);
		}

		protected internal override void UpdateContainedElementPositions() {
			CurrentState.Position = Position;
			CurrentState.UpdateContainedElementPositions();
		}

		public void Activate() {
			StateId++;
			if (StateId >= States.Count) StateId = 0;
			PartialUpdate();
		}

		public static MenuToggle GetSimpleCheckbox() {
			var output = new MenuToggle();
			output.States.Add(new CheckboxGraphic(true));
			output.States.Add(new CheckboxGraphic(false));
			return output;
		}

		public override void Cascade(Action<MenuElement> a) {
			base.Cascade(a);
			foreach (var s in States) {
				s.Cascade(a);
			}
		}

		private void ElementAdded(MenuElement e) { e.Parent = this; }
		private void ElementRemoved(MenuElement e) { if (e.Parent == this) e.Parent = null; }
	}
}
