﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.debug {
	public class DebugHighlighter {

		public object HighlightedObject;
		private Rectangle Highlight;

		public void Update(Scene s) {
			var pos = Resources.Camera.Project(Camera.Space.Screen, Camera.Space.Subpixel, Resources.MouseInput.Position);
			var find = s.FindEntity(pos);
			if (find == null) {
				Highlight = Rectangle.Empty;
				HighlightedObject = Resources.Game;
			} else {
				HighlightedObject = find;
				Highlight = find.GetHitbox();
			}

			if (Resources.MouseInput.LeftPress) {
				Resources.Debug.SetInfoTarget(HighlightedObject);
			}
		}

		public void Draw(Renderer r, Camera c) {
			if (Highlight == Rectangle.Empty) return;

			var h = c.Project(Camera.Space.Subpixel, Camera.Space.Screen, Highlight);
			h.Inflate(1, 1);
			r.DrawRectDirect(h, Color.White * 0.5f);
		}

	}
}
