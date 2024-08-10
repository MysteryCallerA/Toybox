using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.rendermodels {
	public abstract class RenderModel {

		protected int ScreenX, ScreenY;

		public abstract void Initialize(GraphicsDevice g, Camera c);

		public abstract void Apply(GraphicsDevice g, Camera c);

		public abstract Point RenderToScreen(Point p, Camera c);

		public abstract Point ScreenToRender(Point p, Camera c);

		public abstract Rectangle RenderToScreen(Rectangle r, Camera c);

		public abstract Rectangle ScreenToRender(Rectangle r, Camera c);

		public virtual Rectangle GetScreenBounds(Camera c) {
			return new Rectangle(ScreenX, ScreenY, GetScreenWidth(c), GetScreenHeight(c));
		}

		public abstract int GetScreenWidth(Camera c);

		public abstract int GetScreenHeight(Camera c);

		protected void CenterScreen(GraphicsDevice g, Camera c) {
			ScreenX = g.Viewport.Bounds.Center.X - (GetScreenWidth(c) / 2);
			ScreenY = g.Viewport.Bounds.Center.Y - (GetScreenHeight(c) / 2);
		}

		public abstract int GetScreenPixelSize(Camera c);

		public abstract int GetRenderScale();

	}
}
