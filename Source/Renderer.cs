using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
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

		public void DrawRect(Rectangle r, Color c, Camera cam, Camera.Space fromSpace) {
				r = cam.Project(fromSpace, Camera.Space.Render, r);
			Batch.Draw(Blank, r, c);
		}

		public void DrawRectStatic(Rectangle r, Color c, Camera cam, Camera.Space fromSpace) {
			r = cam.Project(fromSpace, Camera.Space.Subpixel, r);
			Batch.Draw(Blank, r, c);
		}



		public void DrawDirect(Texture2D t, Rectangle dest, Rectangle source, Color c) {
			Batch.Draw(t, dest, source, c);
		}

		public void Draw(Texture2D t, Rectangle dest, Rectangle source, Color c, Camera cam, Camera.Space fromSpace) {
			dest = cam.Project(fromSpace, Camera.Space.Render, dest);
			Batch.Draw(t, dest, source, c);
		}

		public void DrawStatic(Texture2D t, Rectangle dest, Rectangle source, Color c, Camera cam, Camera.Space fromSpace) {
			dest = cam.Project(fromSpace, Camera.Space.Subpixel, dest);
			Batch.Draw(t, dest, source, c);
		}

		//TODO -HARD- Currently when using GameScale, you can still draw at subpixel positions to allow for smoother movement
		//I want to somehow add a way to do this with RenderScale also
		//Using a RenderModel could mabye work if you remember to take GameScale into account for any in-game spatial math.
	
	}
}
