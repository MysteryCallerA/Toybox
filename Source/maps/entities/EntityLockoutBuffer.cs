using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.maps.entities {
	public class EntityLockoutBuffer<T> where T:Entity {

		private List<T> Adds = new List<T>();
		private List<T> Removes = new List<T>();

		public void QueueAdd(T e) {
			Adds.Add(e);
		}

		public void QueueRemove(T e) {
			Removes.Add(e);
		}

		public void Apply(IEntityCollection<T> col) {
			foreach (var e in Adds) {
				col.Add(e);
			}
			Adds.Clear();

			foreach (var e in Removes) {
				col.Remove(e);
			}
			Removes.Clear();
		}

	}
}
