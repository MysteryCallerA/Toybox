using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.gui.style {
	public class StyleValue {

		public StyleField Field;
		public int Value;
		public string Name;
		public string Type;

		public StyleValue(StyleField field, int value, string name = null, string type = null) {
			Field = field;
			Value = value;
			Name = name;
			Type = type;
		}

	}
}
