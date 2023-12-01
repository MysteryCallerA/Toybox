using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.utils.data {
	public class CompactList2d<T> {

		private List<T> Content = new List<T>();
		public int Columns { get; private set; }
		public int Rows { get; private set; }
		public T DefaultValue = default;

		public CompactList2d() {
		}

		public void Add(int col, int row, T value) {

		}

		public bool InBounds(int col, int row) {
			if (col < 0 || col >= Columns || row < 0 || row >= Rows) {
				return false;
			}
			return true;
		}


	}
}
