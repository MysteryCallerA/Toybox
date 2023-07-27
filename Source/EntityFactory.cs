using MonoGame.Framework.Utilities.Deflate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.tiled;

namespace Toybox {

	public abstract class EntityFactory<T> where T:Entity {

		protected abstract T CreateEntity(string type);

		public virtual void PositionEntity(Entity e, TiledObject o) {
			e.Position = Resources.Camera.Project(Camera.Space.Pixel, Camera.Space.Subpixel, o.Position); //This projection should probably be configurable somehow
			var hitbox = e.Hitbox.Bounds;
			e.X -= hitbox.Width / 2;
			e.Y -= hitbox.Bottom - e.Y;
		}

		protected virtual void InitializeEntity(T e, TiledObject o) {
		}

		public T BuildEntity(TiledObject o) {
			var e = CreateEntity(o.Name);
			if (e == null) return null;
			PositionEntity(e, o);
			InitializeEntity(e, o);
			return e;
		}

		public List<T> BuildAll(List<TiledObject> objects) {
			var output = new List<T>();
			foreach (var o in objects) {
				var entity = BuildEntity(o);
				if (entity == null) continue;
				output.Add(entity);
			}
			return output;
		}
	
	}
}
