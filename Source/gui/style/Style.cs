using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.gui.core;

namespace Toybox.gui.style {
	public class Style {

		/*Menu styles are stored as a List of StyleValues
		 * each StyleValue is a field id as a type object and a value as ints
		 * colors can be split into 4 ints (rgba)
		 * MenuElements contain their own values for things and can function without using styles
		 * styles are mainly for setting up a unifying look across menus
		 * and also to allow "style switching"
		 * 
		 * MenuElements should have a MenuState value showing their current state
		 * They also contain a StyleManager
		 * The StyleManager is default to null
		 * When you do MenuElement.AddStyle(), it creates a default StyleManager for you if null
		 * Then on init, and whenever the state changes, the element asks the StyleManager for a list
		 * of StyleValues to apply. Use a yield function.
		 * The StyleManager handles figuring out which values are needed based on the element's class
		 * and name.
		 * It handles styles additively, meaning unless the value is set, it won't change or reset.
		 * 
		 * Add Overflow values to mabye all MenuElements.
		 * Overflow increases visual size without increasing actual size. Useful for tween effects.
		 * 
		 * Tweening could be handled by the StyleManager somehow.
		 * MenuElements could continue getting StyleValues while the manager is actively tweening.
		 * It would only grab StyleValues that are being tweened tho.
		 */

		public MenuState[] State;
		public List<StyleValue> Values = new();

		public Style(params MenuState[] state) {
			State = state;
		}

		public void Add(ColorField f, Color c, string name = null, string type = null) {
			Values.Add(new StyleValue(f.R, c.R, name, type));
			Values.Add(new StyleValue(f.G, c.G, name, type));
			Values.Add(new StyleValue(f.B, c.B, name, type));
			Values.Add(new StyleValue(f.A, c.A, name, type));
		}

		public void Add(StyleField f, int v, string name = null, string type = null) {
			Values.Add(new StyleValue(f, v, name, type));
		}

		public IEnumerable<StyleValue> GetRelevantValues(MenuElement e) {
			foreach (var value in Values) {
				if (value.Name != null && e.Name != value.Name) continue;
				if (value.Type != null && e.GetTypeName() != value.Type) continue;
				yield return value;
			}
		}

	}
}
