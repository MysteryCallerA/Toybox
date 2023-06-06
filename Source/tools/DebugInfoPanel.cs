using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.text;

namespace Toybox.debug {
	public class DebugInfoPanel {

		public bool Active = false;
		private object Target;
		private string Text = "";
		public Text TextRenderer;

		public DebugInfoPanel(Font f) {
			TextRenderer = new Text(f) { BackColor = Color.Black * 0.5f };
		}

		public void SetTarget(object t) {
			Target = t;
		}

		public void Update() {
			var s = new StringBuilder();

			s.Append(Target.GetType().Name);
			foreach (var f in Target.GetType().GetFields()) {
				s.AppendLine();
				s.Append(f.Name + "=");
				var val = f.GetValue(Target);
				if (val == null) s.Append("null");
				else s.Append(val.ToString());
			}
			Text = s.ToString();
		}

		public void Draw(Renderer r, Camera c) {
			if (Text == "") return;
			TextRenderer.Draw(r.Batch, Color.White, new Point(1, 1), Text, 2);
		}

	}
}
