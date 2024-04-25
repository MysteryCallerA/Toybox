using Microsoft.Xna.Framework;

namespace Toybox.scenes {
	public class Scene {

		public Scene() {
		}

		public virtual void Update() {
		}

		//PostUpdates are for things like removing/adding entities after finished main Update.
		public virtual void PostUpdate() {
		}

		public virtual void Draw(Renderer r, Camera c) {
		}

		public virtual void DrawHitboxes(Renderer r, Camera c) {
		}

		//Used by the DebugHighlighter to find clicked-on entities.
		public virtual Entity FindEntity(Point pos) {
			return null;
		}
	}
}
