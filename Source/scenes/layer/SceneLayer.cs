using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.scenes.layer {
	public abstract class SceneLayer {

		public bool Visible = true;
		public bool Active = true;
		public string Name;

		public SceneLayer(string name) {
			Name = name;
		}

		public abstract void Init();
		public abstract void Draw(Renderer r, Camera c);
		public abstract void Update();
		public abstract void PostUpdate();

		public virtual void PixelScaleChanged(int prevPixelScale, int newPixelScale) {
		}

	}
}
