using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.rendermodels {
	/// <summary> Pixel perfect rendering that increases scale to best fit the window. </summary>
	public class Fit:RenderModel {

		public int RenderScale = 1;

		private readonly int InitWorldWidth, InitWorldHeight;

		public Fit(int worldwidth, int worldheight) {
			InitWorldWidth = worldwidth;
			InitWorldHeight = worldheight;
		}

		public override void Initialize(GraphicsDevice g, Camera c) {
			c.Width = InitWorldWidth;
			c.Height = InitWorldHeight;
			c.Render = new RenderTarget2D(g, c.Width, c.Height);
			Apply(g, c);
		}

		public override void Apply(GraphicsDevice g, Camera c) {
			var screen = g.Viewport.Bounds;
			var width = (int)Math.Floor((float)screen.Width / c.Width);
			var height = (int)Math.Floor((float)screen.Height / c.Height);
			RenderScale = Math.Min(width, height);
			if (RenderScale < 1) RenderScale = 1;

			CenterScreen(g, c);
		}

		public override Point RenderToScreen(Point p, Camera c) {
			return new Point((p.X * RenderScale) + ScreenX, (p.Y * RenderScale) + ScreenY);
		}

		public override int GetScreenWidth(Camera c) {
			return c.Render.Width * RenderScale;
		}

		public override int GetScreenHeight(Camera c) {
			return c.Render.Height * RenderScale;
		}

		public override Point ScreenToRender(Point p, Camera c) {
			return new Point((p.X - ScreenX) / RenderScale, (p.Y - ScreenY) / RenderScale);
		}

		public override int GetScreenPixelSize(Camera c) {
			return c.PixelScale * RenderScale;
		}

		public override Rectangle RenderToScreen(Rectangle r, Camera c) {
			return new Rectangle(RenderToScreen(r.Location, c), new Point(r.Width * RenderScale, r.Height * RenderScale));
		}

		public override Rectangle ScreenToRender(Rectangle r, Camera c) {
			return new Rectangle(ScreenToRender(r.Location, c), new Point(r.Width / RenderScale, r.Height / RenderScale));
		}

		public override int GetRenderScale() {
			return RenderScale;
		}
	}
}
