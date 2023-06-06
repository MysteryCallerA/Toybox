using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.maps.entities {
	public interface IEntityCollection<T> where T:Entity {

		public void Add(T e);
		public void Remove(T e);
		public T Find(Point p);

	}
}
