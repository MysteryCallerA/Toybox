using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox {
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class DebugReadAttribute:Attribute {
	}

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class DebugWriteAttribute:Attribute {
	}
}
