using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.rendermodels {
	/// <summary> Pixel perfect rendering that increases drawn world bounds to fill the window </summary>
	public class Fill:RenderModel {

		public Fill() {
		}

		public override void Initialize(GraphicsDevice g, Camera c) {
			Apply(g, c);
		}

		public override void Apply(GraphicsDevice g, Camera c) {
			var screen = g.Viewport.Bounds;
			int width = (int)Math.Floor((float)screen.Width / c.PixelScale) * c.PixelScale;
			int height = (int)Math.Floor((float)screen.Height / c.PixelScale) * c.PixelScale;

			c.Render = new RenderTarget2D(g, width, height);
			c.Width = width;
			c.Height = height;

			CenterScreen(g, c);
		}

		public override int GetScreenHeight(Camera c) {
			return c.Render.Height;
		}

		public override int GetScreenWidth(Camera c) {
			return c.Render.Width;
		}

		public override Point WorldToScreen(Point p, Camera c) {
			return new Point(p.X + ScreenX, p.Y + ScreenY); //NEXT this doesn't cast to RenderSpace correctly which is distinct from screen space, need to update when this becomes relevant
		}

		public override Point ScreenToWorld(Point p, Camera c) {
			return new Point(p.X - ScreenX, p.Y - ScreenY);
		}

		public override int GetScreenPixelSize(Camera c) {
			return c.PixelScale;
		}

		public override Rectangle WorldToScreen(Rectangle r, Camera c) {
			return new Rectangle(WorldToScreen(r.Location, c), new Point(r.Width, r.Height));
		}

		public override Rectangle ScreenToWorld(Rectangle r, Camera c) {
			return new Rectangle(ScreenToWorld(r.Location, c), new Point(r.Width, r.Height));
		}

		public override int GetRenderScale() {
			return 1;
		}
	}
}
