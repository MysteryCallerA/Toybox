using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Toybox.tools.info {
	public class DebugDictionaryInfo:IDebugInfo {

		private IDictionary Content;
		private List<object> Names = new List<object>();

		public DebugDictionaryInfo(object o) {
			Content = o as IDictionary;
		}

		public string GetText() {
			var s = new StringBuilder();
			var target = Content as IEnumerable;

			s.Append(Content.GetType().Name);
			foreach (var v in target) {
				s.AppendLine();
				s.Append(v.ToString());
			}
			return s.ToString();
		}

		public void SetField(int linenum, object value) {
		}

		public object TargetLine(int linenum) {
			if (linenum < 0) return null;
			int i = 0;
			foreach (var line in Content.Values) {
				if (i == linenum) return line;
				i++;
			}
			return null;
		}
	}
}
