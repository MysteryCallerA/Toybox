using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.maps {
	public class EntityList {

		private List<Entity> Content = new List<Entity>();

		public EntityList() {

		}

		public void Update() {
			foreach (var e in Content) {
				e.Update();
			}
		}

		public void Draw(Renderer r, Camera c) {
			foreach (var e in Content) {
				e.Draw(r, c);
			}
		}

		public void Add(Entity e) {
			Content.Add(e);
		}

		public void Add(ICollection<Entity> ents) {
			foreach (var e in ents) {
				Add(e);
			}
		}

		public Entity Find(string name) {
			foreach (Entity e in Content) {
				if (e == null) continue;
				if (e.Name == name) return e;
			}
			return null;
		}

	}
}
