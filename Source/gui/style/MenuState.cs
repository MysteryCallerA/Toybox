using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;

namespace Toybox.gui.style {
	public class MenuState {

		public readonly string Name;
		public int Id;
		private static int NextId = 0;

		public MenuState(string name) {
			Id = NextId;
			NextId++;
			Name = name;
		}

		public override bool Equals(object obj) {
			return Equals(obj as MenuState);
		}

		public bool Equals(MenuState s) {
			return s != null && s.Id == Id;
		}

		public override int GetHashCode() {
			return Id.GetHashCode();
		}

		public override string ToString() {
			return Id.ToString();
		}

		public static readonly MenuState Selected = new("Selected");
		public static readonly MenuState Pressed = new("Pressed");

	}
}
