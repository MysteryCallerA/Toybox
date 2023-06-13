using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.tools.info
{
	public class DebugEnumerableInfo:IDebugInfo {

		private IEnumerable Content;

		public DebugEnumerableInfo(object o) {
			Content = o as IEnumerable;
		}

		public string GetText() {
			var s = new StringBuilder();

			s.Append(Content.GetType().Name);
			var i = 0;
			foreach (var line in Content) {
				s.AppendLine();
				s.Append(i.ToString() + "=");
				if (line == null) s.Append("null");
				else s.Append(line.ToString());
				i++;
			}
			return s.ToString();
		}

		public void SetField(int linenum, object value) {
		}

		public object TargetLine(int linenum) {
			if (linenum < 0) return null;
			int i = 0;
			foreach (var line in Content) {
				if (i == linenum) return line;
				i++;
			}
			return null;
		}
	}
}
