using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.components;

namespace Toybox {
	
	public abstract class ComplexEntity:Entity {

		public List<EntityComponent> Components = new List<EntityComponent>();

		public override void Update() {
			base.Update();
			foreach (var c in Components) {
				c.Apply(this);
			}
		}

		public override void Draw(Renderer r, Camera c) {
			base.Draw(r, c);
			foreach (var com in Components) {
				com.Draw(this, r, c);
			}
		}

	}
}
