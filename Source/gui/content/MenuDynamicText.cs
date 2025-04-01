using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.utils.text;

namespace Toybox.gui.content {

	public class MenuDynamicText:MenuText {

		public Func<string> GetText;

		public MenuDynamicText(Font f):base(f) {
		}

		public MenuDynamicText(Font f, Func<string> getText):base(f) {
			GetText = getText;
		}

		protected internal override void UpdateState() {
			base.UpdateState();

			if (GetText != null) {
				Content = GetText.Invoke();
			}
		}

	}
}
