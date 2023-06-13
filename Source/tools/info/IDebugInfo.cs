﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.tools.info {
	public interface IDebugInfo {

		public string GetText();
		public object TargetLine(int linenum);
		public void SetField(int linenum, object value);

	}
}
