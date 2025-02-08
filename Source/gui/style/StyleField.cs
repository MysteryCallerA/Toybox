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
		public static readonly ColorField TextShadow = new("TextShadow");

	}
}
