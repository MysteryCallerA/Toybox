using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.scenes.world {
	public class WorldManager<T> where T:WorldSegment {

		public List<T> Content = new();
		public WorldLayout Layout;

		public WorldManager() {

		}

		public void Draw(Renderer r, Camera c) {
			for (int i = 0; i < Content.Count; i++) {
				Content[i].Draw(r, c);
			}
		}

		public void Update() {
			for (int i = 0; i < Content.Count; i++) {
				Content[i].Update();
			}
		}

	}
}
