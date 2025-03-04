using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.utils.data {
	public class ObservableList<T>:List<T> {

		public Action<T> OnAdd;
		public Action<T> OnRemove;

		public new void Add(T i) {
			OnAdd?.Invoke(i);
			base.Add(i);
		}

		public new void AddRange(IEnumerable<T> items) {
			if (OnAdd != null) {
				foreach (var i in items) {
					OnAdd.Invoke(i);
				}
			}
			base.AddRange(items);
		}

		public new void Clear() {
			if (OnRemove != null) {
				foreach (var i in this) {
					OnRemove.Invoke(i);
				}
			}
			base.Clear();
		}

		public new void Insert(int index, T i) {
			OnAdd?.Invoke(i);
			base.Insert(index, i);
		}

		public new void Remove(T i) {
			OnRemove?.Invoke(i);
			base.Remove(i);
		}

		public new void RemoveAt(int index) {
			if (OnRemove != null && index >= 0 && index < Count) {
				OnRemove.Invoke(this[index]);
			}
			base.RemoveAt(index);
		}
	}
}
