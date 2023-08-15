using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.components {
	public interface EntityComponent {

		public void Apply(Entity e);

		public void Draw(Entity e, Renderer r, Camera c) {
		}

	}
}
