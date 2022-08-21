using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Utils.save;

namespace Toybox {
	public abstract class SceneLayer:IDisposable, IXmlSaveable {

		public string Name;
		public bool Visible = true;
		public bool Active = true;

		public SceneLayer(string name) {
			Name = name;
		}

		public void Draw(SpriteBatch s, Camera c) {
			if (!Visible) return;
			DoDraw(s, c);
		}

		public void Update() {
			if (!Active) return;
			DoUpdate();
		}

		protected abstract void DoDraw(SpriteBatch s, Camera c);

		protected abstract void DoUpdate();

		public abstract LayerType GetLayerType();

		public virtual void Dispose() {

		}

		public abstract void Save(XmlWriter writer);

		public enum LayerType {
			Tile, Entity
		}

		public abstract Rectangle GetBounds();

	}
}
