using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.maps {
	public class ZoneMap {

		public List<ZoneEntity> Content = new List<ZoneEntity>();

		public ZoneMap() {

		}

		public void Add(ZoneEntity e) {
			Content.Add(e);
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
		public IEnumerable<ZoneEntity> CollideAll(Point p) {
			foreach (var e in Content) {
				if (e.Rect.Contains(p)) {
					yield return e;
				}
			}
		}

		public bool TryCollide(Point p, out ZoneEntity output) {
			foreach (var e in Content) {
				if (e.Rect.Contains(p)) {
					output = e;
					return true;
				}
			}
			output = null;
			return false;
		}

		public IEnumerable<ZoneEntity> CollideAll(Rectangle r) {
			foreach (var e in Content) {
				if (e.Rect.Intersects(r)) {
					yield return e;
				}
			}
		}

		public bool TryCollide(Rectangle r, out ZoneEntity output) {
			foreach (var e in Content) {
				if (e.Rect.Intersects(r)) {
					output = e;
					return true;
				}
			}
			output = null;
			return false;
		}

		public void RemoveAll(string name) {
			for (int i = 0; i < Content.Count; i++) {
				if (Content[i].Name == name) {
					Content.RemoveAt(i);
					i--;
				}
			}
		}

		/// <summary> Returns the first zone with the supplied name, or null if none match. </summary>
		public IEnumerator<ZoneEntity> Find(string name) {
			foreach (var e in Content) {
				if (e.Name == name) {
					yield return e;
				}
			}
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
