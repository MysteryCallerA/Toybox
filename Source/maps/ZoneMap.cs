using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.maps {
	public class ZoneMap {

		private List<ZoneEntity> Zones = new List<ZoneEntity>();

		public ZoneMap() {

		}

		public void Add(ZoneEntity e) {
			Zones.Add(e);
		}

		public void Add(string name, Rectangle r) {
			Add(new ZoneEntity(name, r));
		}

		public void Add(ICollection<ZoneEntity> ents) {
			foreach (var e in ents) {
				Add(e);
			}
		}

		/// <summary> Returns the first zone containing the point, or null if none do.  </summary>
		public ZoneEntity Find(Point p) {
			foreach (var e in Zones) {
				if (e.Rect.Contains(p)) {
					return e;
				}
			}
			return null;
		}

		public ZoneEntity FindIntersects(Rectangle r) {
			foreach (var e in Zones) {
				if (e.Rect.Intersects(r)) {
					return e;
				}
			}
			return null;
		}

		/// <summary> Returns the first zone with the supplied name, or null if none match. </summary>
		public ZoneEntity Find(string name) {
			foreach (var e in Zones) {
				if (e.Name == name) {
					return e;
				}
			}
			return null;
		}
	}

	public class ZoneEntity {
		public string Name;
		public Rectangle Rect;

		public ZoneEntity(string name, Rectangle r) {
			Name = name;
			Rect = r;
		}
	}
}
