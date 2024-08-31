using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Toybox.graphic {
	public static class TextureMapBuilder {

		/// <param name="backColor"> Color of dividing lines between frames. </param>
		/// <param name="originColor"> Color of frame origins. Alpha value is used for origins overlapping pixels. </param>
		public static TextureMap Build(Texture2D t, Color backColor, Color? originColor = null) {
			var map = new TextureMap(t);
			var pixels = GraphicOps.GetPixels(t);

			var nextY = -1;
			var lineEnd = t.Width;
			var x = 0;
			var y = 0;
			var frameXStart = 0;

			for (int i = 0; i < pixels.Length; i++) {
				if (pixels[i] == backColor || i >= lineEnd) {
					var frame = BuildFrame(frameXStart, x, y, ref pixels, t.Width, backColor, originColor, out bool validFrame, out Point origin);

					if (validFrame) {
						map.Frames.Add(new TextureMap.TextureMapFrame(frame) { Origin = origin });
					}
					frameXStart = x + 1;

					//Adjust nextY line pointer to always be the bottom of the highest frame
					if (nextY == -1 || frame.Bottom + 1 < nextY) nextY = frame.Bottom + 1;
				}
				x++;

				//Adjust pointers when hit texture edge
				if (i >= lineEnd) {
					i = nextY * t.Width;
					lineEnd = i + t.Width;
					x = 0;
					y = nextY;
					nextY = -1;
					frameXStart = 0;
					i--; //-1 because will ++ with next iteration
				}
			}
			t.SetData(pixels);

			return map;
		}

		/// <param name="validFrame"> True if the frame has width and isn't a duplicate. </param>
		private static Rectangle BuildFrame(int xLeft, int xRight, int yTop, ref Color[] pixels, int textureWidth, Color backColor, Color? originColor, out bool validFrame, out Point origin) {
			var height = 0;
			validFrame = true;

			if (xRight - xLeft > 0) {
				//Starting from top-left of frame, iterate downward, looking for edge of frame
				for (int i = xLeft + (yTop * textureWidth); i < pixels.Length; i += textureWidth) {
					if (pixels[i] == backColor) break;
					height++;
				}

				//Check pixel above top-left of frame. If not a border, then it is a duplicate frame and thus invalid
				var check = xLeft + ((yTop - 1) * textureWidth);
				if (check >= 0 && pixels[check] != backColor) validFrame = false;
			} else {
				validFrame = false;
			}

			var output = new Rectangle(xLeft, yTop, xRight - xLeft, height);

			origin = Point.Zero;
			if (originColor.HasValue) {
				var alpha = originColor.Value.A;
				var color = new Color(originColor.Value.R, originColor.Value.G, originColor.Value.B);

				//iterate thru every pixel to find the origin
				int addForNextLine = textureWidth - output.Width - 1;
				int endPixel = output.Right + ((output.Bottom - 1) * textureWidth);
				int lineEnd = output.Right + (output.Top * textureWidth);
				for (int i = xLeft + (yTop * textureWidth); i < endPixel; i++) {
					if (i >= lineEnd) {
						i += addForNextLine;
						lineEnd += textureWidth;
						continue;
					}
					if (pixels[i].A == alpha) {
						var m = (float)alpha / 255;
						pixels[i] = new Color((byte)(pixels[i].R / m), (byte)(pixels[i].G / m), (byte)(pixels[i].B / m), byte.MaxValue);
						origin = new Point(i % textureWidth, i / textureWidth) - output.Location;
						break;
					}
					if (pixels[i] == color) {
						pixels[i] = Color.Transparent;
						origin = new Point(i % textureWidth, i / textureWidth) - output.Location;
						break;
					}
				}
			}

			return output;
		}

	}
}
