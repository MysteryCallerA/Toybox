using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Toybox.maps.entities;
using Utils.save;

namespace Toybox {
	public class Scene {

		public bool KeepSceneInMemory = false;
		public string Name;

		public Scene() {
		}

		public virtual void Update() {
		}

		public virtual void PostUpdate() {
		}

		public virtual void Draw(Renderer r, Camera c) {
		}

		public virtual Entity FindEntity(Point pos) {
			return null;
		}
	}
}
