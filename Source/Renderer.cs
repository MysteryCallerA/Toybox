using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Toybox {
	
	public class Renderer {

		public SpriteBatch Batch;
		public Texture2D Blank;

		public Renderer(SpriteBatch s, Texture2D blankTexture) {
			Batch = s;
			Blank = blankTexture;
		}

		public void DrawRectDirect(Rectangle r, Color c) {
			Batch.Draw(Blank, r, c);
		}

		public Rectangle DrawRect(Rectangle r, Color c, Camera cam, Camera.Space fromSpace) {
			r = cam.Project(fromSpace, Camera.Space.Render, r);
			Batch.Draw(Blank, r, c);
			return r;
		}

		public void DrawRectStatic(Rectangle r, Color c, Camera cam, Camera.Space fromSpace) {
			r = cam.Project(fromSpace, Camera.Space.Pixel, r);
			Batch.Draw(Blank, r, c);
		}



		public void DrawDirect(Texture2D t, Rectangle dest, Rectangle source, Color c, SpriteEffects effect = SpriteEffects.None) {
			Batch.Draw(t, dest, source, c, 0, Vector2.Zero, effect, 0);
		}

		public void Draw(Texture2D t, Rectangle dest, Rectangle source, Color c, Camera cam, Camera.Space fromSpace, SpriteEffects effect = SpriteEffects.None) {
			dest = cam.Project(fromSpace, Camera.Space.Render, dest);
			Batch.Draw(t, dest, source, c, 0, Vector2.Zero, effect, 0);
		}

		public void DrawStatic(Texture2D t, Rectangle dest, Rectangle source, Color c, Camera cam, Camera.Space fromSpace, SpriteEffects effect = SpriteEffects.None) {
			dest = cam.Project(fromSpace, Camera.Space.Pixel, dest);
			Batch.Draw(t, dest, source, c, 0, Vector2.Zero, effect, 0);
		}



		public void DrawLineDirect(Vector2 start, Vector2 end, Color c) {
			var dist = Vector2.Distance(start, end);
			var angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
			Batch.Draw(Blank, start, null, c, angle, Vector2.Zero, new Vector2(dist, 1), SpriteEffects.None, 0);
		}

		public void DrawLine(Vector2 start, Vector2 end, Color c, Camera cam, Camera.Space fromSpace) {
			var rect = cam.Project(fromSpace, Camera.Space.Render, new Rectangle(start.ToPoint(), (end - start).ToPoint()));
			var dist = (float)Math.Sqrt(rect.Width * rect.Width + rect.Height * rect.Height);
			var angle = (float)Math.Atan2(rect.Height, rect.Width);
			Batch.Draw(Blank, rect.Location.ToVector2(), null, c, angle, Vector2.Zero, new Vector2(dist, 1), SpriteEffects.None, 0);
		}

		public void DrawLineStatic(Vector2 start, Vector2 end, Color c, Camera cam, Camera.Space fromSpace) {
			var rect = cam.Project(fromSpace, Camera.Space.Pixel, new Rectangle(start.ToPoint(), (end - start).ToPoint()));
			var dist = (float)Math.Sqrt(rect.Width * rect.Width + rect.Height * rect.Height);
			var angle = (float)Math.Atan2(rect.Height, rect.Width);
			Batch.Draw(Blank, rect.Location.ToVector2(), null, c, angle, Vector2.Zero, new Vector2(dist, 1), SpriteEffects.None, 0);
		}
	
	}
}
