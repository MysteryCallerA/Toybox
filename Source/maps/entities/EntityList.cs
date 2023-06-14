using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.maps.entities {

	public class EntityList:EntityList<Entity> {
	}

	public class EntityList<T>:IEnumerable<T>, IEntityCollection<T> where T : Entity {

		private List<T> Content = new List<T>();
		private EntityLockoutBuffer<T> Buffer = new EntityLockoutBuffer<T>();

		public EntityList() {

		}

		public T this[int i] {
			get { return Content[i]; }
			set { Content[i] = value; }
		}

		public int Count {
			get { return Content.Count; }
		}

		public void Update() {
			foreach (var e in Content) {
				e.Update();
			}
		}

		public void PostUpdate() {
			Buffer.Apply(this);
		}

		public void Draw(Renderer r, Camera c) {
			foreach (var e in Content) {
				e.Draw(r, c);
			}
		}

		public void Add(T e) {
			if (Resources.Game.InUpdateStep) { Buffer.QueueAdd(e); return; }
			Content.Add(e);
		}

		public void Add(ICollection<T> ents) {
			foreach (var e in ents) {
				Add(e);
			}
		}

		public T Find(string name) {
			foreach (T e in Content) {
				if (e == null) continue;
				if (e.Name == name) return e;
			}
			return null;
		}

		public T Find(Point p) {
			foreach (T e in Content) {
				if (e.GetHitbox().Contains(p)) return e;
			}
			return null;
		}

		public IEnumerable<T> GetCollisions(Rectangle r) {
			foreach (T e in Content) {
				if (e.GetHitbox().Intersects(r)) yield return e;
			}
		}

		public IEnumerator<T> GetEnumerator() {
			return Content.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public void Remove(T e) {
			if (Resources.Game.InUpdateStep) { Buffer.QueueRemove(e); return; }
			Content.Remove(e);
		}
	}
}
