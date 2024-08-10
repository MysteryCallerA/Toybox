using Microsoft.Xna.Framework;

namespace Toybox.scenes {
	public abstract class Scene {

		public Scene() {
		}

		public abstract void Init();

		public abstract void Update();

		//PostUpdates are for things like removing/adding entities after finished main Update.
		public abstract void PostUpdate();

		public virtual void PreUpdate() {
		}

		public virtual void Draw(Renderer r, Camera c) {
		}

		public virtual void DrawHitboxes(Renderer r, Camera c) {
		}

		//Used by the DebugHighlighter to find clicked-on entities.
		public virtual Entity FindEntity(Point pos) {
			return null;
		}

		public abstract void PixelScaleChanged(int prevPixelScale, int newPixelScale);
	}
}
