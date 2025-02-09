using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.gui.style {
	public class StyleField {

		internal static int NextId = 0;
		public int Id;
		public readonly string Name;

		public StyleField(string name) {
			Name = name;
			Id = NextId;
			NextId++;
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
			return Name;
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

	}

	public class ColorField {

		public readonly string Name;
		public int Id;

		public ColorField(string name) {
			Name = name;
			Id = StyleField.NextId;
			StyleField.NextId++;
		}

		public override bool Equals(object obj) {
			return Equals(obj as ColorField);
		}

		public bool Equals(ColorField s) {
			return s != null && s.Id == Id;
		}

		public override int GetHashCode() {
			return Id.GetHashCode();
		}

		public override string ToString() {
			return Name;
		}

		public static readonly ColorField Text = new("Text");
		public static readonly ColorField Back = new("Back");
		public static readonly ColorField TextShadow = new("TextShadow");

	}
}
