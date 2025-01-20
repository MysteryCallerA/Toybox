using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;
using Toybox.gui.graphic;
using Toybox.utils.math;

namespace Toybox.gui.content {
	public class MenuToggle:MenuElement {

		public int StateId { get; private set; }
		public List<MenuElement> States = new();

		public MenuElement CurrentState {
			get { return States[StateId]; }
		}

		public MenuToggle() {
			Fit = FitType.FitContent;
			VAlign = VAlignType.Center;
		}

		public override void Draw(Renderer r) {
			if (States.Count == 0) return;
			CurrentState.Draw(r);
		}

		protected internal override void UpdateFunction() {
			CurrentState.UpdateFunction();
		}

		protected override void GetContentSize(out Point contentSize) {
			CurrentState.UpdateSize(Point.Zero);
			contentSize = CurrentState.TotalSize;
		}

		protected internal override void UpdateContainedElementPositions() {
			CurrentState.Position = Position;
			CurrentState.UpdateContainedElementPositions();
		}

		public override bool Activate() {
			StateId++;
			if (StateId >= States.Count) StateId = 0;
			PartialUpdate();
			return true;
		}

		public static MenuToggle GetSimpleCheckbox() {
			var output = new MenuToggle();
			output.States.Add(new CheckboxGraphic(true));
			output.States.Add(new CheckboxGraphic(false));
			return output;
		}
	}
}
