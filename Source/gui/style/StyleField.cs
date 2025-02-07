using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.gui.style {
	public class StyleField {

		public int Id;
		private static int NextId = 0;
		public readonly string Name;

		public StyleField(string name) {
			Id = NextId;
			NextId++;
			Name = name;
		}

		public override bool Equals(object obj) {
			return Equals(obj as StyleField);
		}

		public bool Equals(StyleField s) {
			return s != null && s.Id == Id;
		}

		public override int GetHashCode() {
			return Id.GetHashCode();
		}

		public override string ToString() {
			return Id.ToString();
		}

		public static readonly StyleField Padding = new("Padding");
		public static readonly StyleField PaddingLeft = new("PaddingLeft");
		public static readonly StyleField PaddingRight = new("PaddingRight");
		public static readonly StyleField PaddingTop = new("PaddingTop");
		public static readonly StyleField PaddingBottom = new("PaddingBottom");

		public static readonly StyleField Margin = new("Margin");
		public static readonly StyleField MarginLeft = new("MarginLeft");
		public static readonly StyleField MarginRight = new("MarginRight");
		public static readonly StyleField MarginTop = new("MarginTop");
		public static readonly StyleField MarginBottom = new("MarginBottom");

		public static readonly StyleField Overflow = new("Overflow");
		public static readonly StyleField OverflowLeft = new("OverflowLeft");
		public static readonly StyleField OverflowRight = new("OverflowRight");
		public static readonly StyleField OverflowTop = new("OverflowTop");
		public static readonly StyleField OverflowBottom = new("OverflowBottom");

		public static readonly StyleField OffsetX = new("OffsetX");
		public static readonly StyleField OffsetY = new("OffsetY");

		public static readonly StyleField TextColorR = new("TextColorR");
		public static readonly StyleField TextColorG = new("TextColorG");
		public static readonly StyleField TextColorB = new("TextColorB");
		public static readonly StyleField TextColorA = new("TextColorA");

		public static readonly StyleField BackColorR = new("BackColorR");
		public static readonly StyleField BackColorG = new("BackColorG");
		public static readonly StyleField BackColorB = new("BackColorB");
		public static readonly StyleField BackColorA = new("BackColorA");

	}

	public class ColorField {

		public StyleField R;
		public StyleField G;
		public StyleField B;
		public StyleField A;

		public ColorField(StyleField r, StyleField g, StyleField b, StyleField a) {
			R = r;
			G = g;
			B = b;
			A = a;
		}

		public static readonly ColorField Text = new(StyleField.TextColorR, StyleField.TextColorG, StyleField.TextColorB, StyleField.TextColorA);
		public static readonly ColorField Back = new(StyleField.BackColorR, StyleField.BackColorG, StyleField.BackColorB, StyleField.BackColorA);

	}
}
