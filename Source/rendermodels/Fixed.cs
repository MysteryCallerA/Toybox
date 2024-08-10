using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.rendermodels {
	/// <summary> Pixel perfect rendering at a fixed location and scale. </summary>
	public class Fixed:RenderModel {

		public int RenderScale = 1;

		private readonly int InitWorldWidth, InitWorldHeight;

		public override int GetScreenWidth(Camera c) {
			return c.Render.Width * RenderScale;
		}
		public override int GetScreenHeight(Camera c) {
			return c.Render.Height * RenderScale;
		}

		public Fixed(int worldwidth, int worldheight) {
			InitWorldWidth = worldwidth;
			InitWorldHeight = worldheight;
		}

		public override void Initialize(GraphicsDevice g, Camera c) {
			c.Width = InitWorldWidth;
			c.Height = InitWorldHeight;
			c.Render = new RenderTarget2D(g, c.Width, c.Height);
		}

		public override void Apply(GraphicsDevice g, Camera c) {

		}


		public override Point RenderToScreen(Point p, Camera c) {
			return new Point((RenderScale * p.X) + ScreenX, (RenderScale * p.Y) + ScreenY);
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
