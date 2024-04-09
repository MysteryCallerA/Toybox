using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.scenes.world {
	public abstract class WorldSegment {

		public string Name;

		public WorldSegment(string name) {
			Name = name;
		}

		public abstract void Draw(Renderer r, Camera c);
		public abstract void Update();

	}
}
