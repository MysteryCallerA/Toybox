using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.graphic;

namespace Toybox.components.graphic {
	public class BodyGraphic:EntityComponent {

		public TextureObject Graphic;
		public Color Color = Color.White;

		public BodyGraphic(TextureObject graphic) {
			Graphic = graphic;
		}

		public void Apply(Entity e) {

		}

		public void Draw(Entity e, Renderer r, Camera c) {
			var pos = e.Position - Graphic.Origin.Location;
			r.Draw(Graphic.Texture, new Rectangle(pos, Graphic.Source.Size), Graphic.Source, Color, c, Camera.Space.Pixel, Graphic.Effect);
		}
	}
}
