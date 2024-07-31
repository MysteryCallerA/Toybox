using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.rendermodels {
	/// <summary> Pixel perfect rendering that first increases scale to best fit the window and then increases world bounds to fill remaining space. </summary>
	public class FitFill:RenderModel {

		public int RenderScale = 1;
		public int WorldWidthMax = -1, WorldHeightMax = -1; //TODO not yet implemented

		private readonly int InitWorldWidth, InitWorldHeight;

		public FitFill(int worldwidth, int worldheight) {
			InitWorldWidth = worldwidth;
			InitWorldHeight = worldheight;
		}

		public override void Initialize(GraphicsDevice g, Camera c) {
			c.Width = InitWorldWidth;
			c.Height = InitWorldHeight;
			Apply(g, c);
		}

		public override void Apply(GraphicsDevice g, Camera c) {
			var screen = g.Viewport.Bounds;

			//Solve for Render Scale
			int width = (int)Math.Floor((float)screen.Width / InitWorldWidth);
			int height = (int)Math.Floor((float)screen.Height / InitWorldHeight);
			RenderScale = Math.Min(width, height);
			if (RenderScale < 1) RenderScale = 1;

			//Fill to remaining screen space
			int screenwidth = InitWorldWidth * RenderScale;
			int screenheight = InitWorldHeight * RenderScale;
			int rwidth = screen.Width - screenwidth;
			int rheight = screen.Height - screenheight;
			int pixelsize = RenderScale * c.PixelScale;

			rwidth = (int)Math.Floor((float)rwidth / pixelsize) * c.PixelScale;
			rheight = (int)Math.Floor((float)rheight / pixelsize) * c.PixelScale;
			c.Render = new RenderTarget2D(g, InitWorldWidth + rwidth, InitWorldHeight + rheight);
			c.Width = c.Render.Width;
			c.Height = c.Render.Height;

			CenterScreen(g, c);
		}

		public override int GetScreenHeight(Camera c) {
			return c.Render.Height * RenderScale;
		}

		public override int GetScreenWidth(Camera c) {
			return c.Render.Width * RenderScale;
		}

		public override Point WorldToScreen(Point p, Camera c) {
			return new Point((p.X * RenderScale) + ScreenX, (p.Y * RenderScale) + ScreenY);
		}

		public override Point ScreenToWorld(Point p, Camera c) {
			return new Point((p.X - ScreenX) / RenderScale, (p.Y - ScreenY) / RenderScale);
		}

		public override int GetScreenPixelSize(Camera c) {
			return c.PixelScale * RenderScale;
		}

		public override Rectangle WorldToScreen(Rectangle r, Camera c) {
			return new Rectangle(WorldToScreen(r.Location, c), new Point(r.Width * RenderScale, r.Height * RenderScale));
		}

		public override Rectangle ScreenToWorld(Rectangle r, Camera c) {
			return new Rectangle(ScreenToWorld(r.Location, c), new Point(r.Width / RenderScale, r.Height / RenderScale));
		}

		public override int GetRenderScale() {
			return RenderScale;
		}
	}
}
