using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Toybox.tools.info {
	public class DebugObjectInfo:IDebugInfo {

		private object Content;

		public DebugObjectInfo(object o) {
			Content = o;
		}

		public string GetText() {
			var s = new StringBuilder();

			s.Append(Content.GetType().Name);
			foreach (var f in Content.GetType().GetFields()) {
				s.AppendLine();
				s.Append(f.Name + "=");
				var val = f.GetValue(Content);
				if (val == null) s.Append("null");
				else s.Append(val.ToString());
			}
			return s.ToString();
		}

		public object TargetLine(int linenum) {
			var fields = Content.GetType().GetFields();
			if (linenum < 0 || linenum >= fields.Length) return null;
			return fields[linenum].GetValue(Content);
		}
	}
}
