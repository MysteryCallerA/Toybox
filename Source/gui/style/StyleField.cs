using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.gui.style {
	public class StyleField {

		public readonly string Name;

		public StyleField(string name) {
			Name = name;
		}

		public override string ToString() {
			return Name;
		}

		public static readonly StyleField Overflow = new("Overflow");
		public static readonly StyleField OverflowLeft = new("OverflowLeft");
		public static readonly StyleField OverflowRight = new("OverflowRight");
		public static readonly StyleField OverflowTop = new("OverflowTop");
		public static readonly StyleField OverflowBottom = new("OverflowBottom");

		public static readonly StyleField OffsetX = new("OffsetX");
		public static readonly StyleField OffsetY = new("OffsetY");

	}

	public class ColorField {

		public readonly string Name;

		public ColorField(string name) {
			Name = name;
		}

		public override string ToString() {
			return Name;
		}

		public static readonly ColorField Text = new("Text");
		public static readonly ColorField Back = new("Back");

	}
}
