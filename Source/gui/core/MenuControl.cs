using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.style;

namespace Toybox.gui.core {
	public class MenuControl {

		private static int NextId = 0;
		public string Name;
		public readonly int Id;

		public MenuControl(string name) {
			Name = name;
			Id = NextId;
			NextId++;
		}

		public override int GetHashCode() {
			return Id.GetHashCode();
		}

		public override bool Equals(object obj) {
			return Equals(obj as MenuControl);
		}

		public bool Equals(MenuControl c) {
			return c != null && c.Id == Id;
		}

		public override string ToString() {
			return Name;
		}

		public static readonly MenuControl Up = new("up");
		public static readonly MenuControl Down = new("down");
		public static readonly MenuControl Left = new("left");
		public static readonly MenuControl Right = new("right");
		public static readonly MenuControl Confirm = new("confirm");
		public static readonly MenuControl Back = new("back");
		public static readonly MenuControl Open = new("open");

	}
}
