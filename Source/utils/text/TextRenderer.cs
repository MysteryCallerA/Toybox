using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.utils.text {
	public class TextRenderer {

		public virtual Font Font { get; set; }
		public int Scale = 1;
		public Color Color = Color.White;
		public Color? BackColor = null;
		public Rectangle Mask;
		public bool UseMask = false;
		public bool GreedyMask = true;

		public virtual int WordSpace { get; set; } = 1;
		public virtual int LineSpace { get; set; } = 1;
		public virtual int LetterSpace { get; set; } = 1;
		public virtual int QuoteSpace { get; set; } = 1;

		public Point Scroll = Point.Zero;

		public TextRenderer(Font f) {
			Font = f;
		}

		public void Draw(Renderer r, Color color, Point pos, string text) { //TODO would be nice if this could output the drawn bounds also
			Rectangle draw = new Rectangle(pos.X, pos.Y, 0, 0);
			char prev = ' ';

			for (int i = 0; i < text.Length; i++) {
				char c = text[i];

				if (prev == Font.Newline) {
					draw.X = pos.X;
					draw.Y += (Font.CharHeight + LineSpace) * Scale;
					draw.Width = 0;
				} else if (i != 0) {
					draw.X += LetterSpace * Scale;
				}

				draw = DrawChar(r.Batch, c, new Point(draw.Right, draw.Y), color);
				prev = c;
			}
		}

		public void Draw(Renderer r, Color color, Point pos, string text, int scale) {
			var old = Scale;
			Scale = scale;
			Draw(r, color, pos, text);
			Scale = old;
		}

		private Rectangle DrawChar(SpriteBatch s, char c, Point pos, Color color) {
			if (c == '\r') return new Rectangle(pos, Point.Zero);
			if (c == ' ' || c == Font.Newline) {
				var r = GetCharDest(c, pos);
				if (BackColor.HasValue && c == ' ') s.Draw(Font.Graphic, new Rectangle(r.X, r.Y, r.Width + LetterSpace * Scale, r.Height), Font.Pixel, BackColor.Value);
				return r;
			}
			if (c == '\"') {
				var output = DrawChar(s, '\'', pos, color);
				pos.X += output.Width + (QuoteSpace * Scale);
				var output2 = DrawChar(s, '\'', pos, color);
				return Rectangle.Union(output, output2);
			}

			Rectangle source = GetCharSource(c);
			Rectangle dest = GetCharDest(c, pos, source);
			var unmasked = dest;

			if (UseMask) {
				ApplyMask(ref dest, ref source);
			}

			if (BackColor.HasValue) s.Draw(Font.Graphic, new Rectangle(dest.X, dest.Y, dest.Width + LetterSpace * Scale, dest.Height), Font.Pixel, BackColor.Value);
			s.Draw(Font.Graphic, dest, source, color);
			return unmasked;
		}

		private Rectangle GetCharSource(char c) {
			if (c == '\"') {
				return GetCharSource('\'');
			}

			if (!Font.Contains(c)) {
				return Font[Font.Missing];
			} else {
				return Font[c];
			}
		}

		internal Rectangle GetCharDest(char c, Point pos) {
			return GetCharDest(c, pos, GetCharSource(c));
		}

		internal Rectangle GetCharDest(char c, Point pos, Rectangle source) {
			if (c == ' ') {
				return new Rectangle(pos.X, pos.Y, WordSpace * Scale, Font.CharHeight * Scale);
			}
			if (c == Font.Newline) {
				return new Rectangle(pos.X, pos.Y, Scale, Font.CharHeight * Scale);
			}
			if (c == '\"') {
				var output = GetCharDest('\'', pos, source);
				pos.X += output.Width + (QuoteSpace * Scale);
				var output2 = GetCharDest('\'', pos, source);
				return Rectangle.Union(output, output2);
			}

			return new Rectangle(pos.X, pos.Y, source.Width * Scale, source.Height * Scale);
		}

		private void ApplyMask(ref Rectangle dest, ref Rectangle source) {
			var mask = Mask;
			if (!GreedyMask) {
				//mask.Inflate(-Scale, -Scale);
			}

			var masked = Rectangle.Intersect(dest, mask);
			if (masked == Rectangle.Empty) {
				dest = masked;
				return;
			}
			if (masked == dest) return;

			int right = (dest.Right - masked.Right) / Scale;
			int left = (masked.Left - dest.Left) / Scale;
			int top = (masked.Top - dest.Top) / Scale;
			int bot = (dest.Bottom - masked.Bottom) / Scale;
			source = new Rectangle(source.X + left, source.Y + top, source.Width - (left + right), source.Height - (top + bot));

			//Correct distortions
			masked.X = math.MathOps.FloorMultiple(masked.X - dest.X, Scale) + dest.X;
			masked.Y = math.MathOps.FloorMultiple(masked.Y - dest.Y, Scale) + dest.Y;
			masked.Width = source.Width * Scale;
			masked.Height = source.Height * Scale;
			dest = masked;
		}

		/// <summary> Accounts for scale. </summary>
		public int LineHeight {
			get { return Font.CharHeight * Scale; }
		}

		public Point GetSize(string t) {
			Point output = new();
			output.Y += LineHeight;
			Point currentline = new Point();
			for (int i = 0; i < t.Length; i++) {
				if (t[i] == Font.Newline) {
					output.Y += LineHeight + LineSpace;
					if (currentline.X > output.X) output.X = currentline.X;
					currentline.X = 0;
					continue;
				}
				if (currentline.X != 0) {
					currentline.X += LetterSpace * Scale;
				}

				var rect = GetCharDest(t[i], currentline);
				currentline.X = rect.Right;
			}

			if (currentline.X > output.X) output.X = currentline.X;
			return output;
		}

	}
}
