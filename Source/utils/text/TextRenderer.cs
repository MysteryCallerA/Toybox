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

		public TextRenderer(Font f) {
			Font = f;
		}

		public TextRenderer(TextRenderer t) {
			Font = t.Font;
			Scale = t.Scale;
			Color = t.Color;
			BackColor = t.BackColor;
			Mask = t.Mask;
			UseMask = t.UseMask;
			GreedyMask = t.GreedyMask;
			WordSpace = t.WordSpace;
			LineSpace = t.LineSpace;
			LetterSpace = t.LetterSpace;
			QuoteSpace = t.QuoteSpace;
		}

		public virtual void Draw(Renderer r, Point pos, string text, Color? color = null, int? scale = null) { //TODO would be nice if this could output the drawn bounds also
			if (!color.HasValue) color = Color;
			if (!scale.HasValue) scale = Scale;

			Rectangle draw = new Rectangle(pos.X, pos.Y, 0, 0);
			char prev = ' ';

			for (int i = 0; i < text.Length; i++) {
				char c = text[i];

				if (prev == Font.Newline) {
					draw.X = pos.X;
					draw.Y += (Font.CharHeight + LineSpace) * scale.Value;
					draw.Width = 0;
				} else if (i != 0) {
					draw.X += LetterSpace * scale.Value;
				}

				draw = DrawChar(r.Batch, c, new Point(draw.Right, draw.Y), color.Value, scale.Value);
				prev = c;
			}
		}

		private Rectangle DrawChar(SpriteBatch s, char c, Point pos, Color color, int scale) {
			if (c == '\r') return new Rectangle(pos, Point.Zero);
			if (c == ' ' || c == Font.Newline) {
				var r = GetCharDest(c, pos, scale);
				if (BackColor.HasValue && c == ' ') s.Draw(Font.Graphic, new Rectangle(r.X, r.Y, r.Width + LetterSpace * scale, r.Height), Font.Pixel, BackColor.Value);
				return r;
			}
			if (c == '\"') {
				var output = DrawChar(s, '\'', pos, color, scale);
				pos.X += output.Width + (QuoteSpace * scale);
				var output2 = DrawChar(s, '\'', pos, color, scale);
				return Rectangle.Union(output, output2);
			}

			Rectangle source = GetCharSource(c);
			Rectangle dest = GetCharDest(c, pos, source, scale);
			var unmasked = dest;

			if (UseMask) {
				ApplyMask(ref dest, ref source, scale);
			}

			if (BackColor.HasValue) s.Draw(Font.Graphic, new Rectangle(dest.X, dest.Y, dest.Width + LetterSpace * scale, dest.Height), Font.Pixel, BackColor.Value);
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

		internal Rectangle GetCharDest(char c, Point pos, int scale) {
			return GetCharDest(c, pos, GetCharSource(c), scale);
		}

		internal Rectangle GetCharDest(char c, Point pos, Rectangle source, int scale) {
			if (c == ' ') {
				return new Rectangle(pos.X, pos.Y, WordSpace * scale, Font.CharHeight * scale);
			}
			if (c == Font.Newline) {
				return new Rectangle(pos.X, pos.Y, scale, Font.CharHeight * scale);
			}
			if (c == '\"') {
				var output = GetCharDest('\'', pos, source, scale);
				pos.X += output.Width + (QuoteSpace * scale);
				var output2 = GetCharDest('\'', pos, source, scale);
				return Rectangle.Union(output, output2);
			}

			return new Rectangle(pos.X, pos.Y, source.Width * scale, source.Height * scale);
		}

		private void ApplyMask(ref Rectangle dest, ref Rectangle source, int scale) {
			var mask = Mask;
			if (!GreedyMask) {
				//mask.Inflate(-scale, -scale);
			}

			var masked = Rectangle.Intersect(dest, mask);
			if (masked == Rectangle.Empty) {
				dest = masked;
				return;
			}
			if (masked == dest) return;

			int right = (dest.Right - masked.Right) / scale;
			int left = (masked.Left - dest.Left) / scale;
			int top = (masked.Top - dest.Top) / scale;
			int bot = (dest.Bottom - masked.Bottom) / scale;
			source = new Rectangle(source.X + left, source.Y + top, source.Width - (left + right), source.Height - (top + bot));

			//Correct distortions
			masked.X = math.MathOps.FloorMultiple(masked.X - dest.X, scale) + dest.X;
			masked.Y = math.MathOps.FloorMultiple(masked.Y - dest.Y, scale) + dest.Y;
			masked.Width = source.Width * scale;
			masked.Height = source.Height * scale;
			dest = masked;
		}

		public int LineHeight {
			get { return Font.CharHeight; }
		}

		public Point GetSize(string t, int? scale = null) {
			if (!scale.HasValue) scale = Scale;

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
					currentline.X += LetterSpace;
				}

				var rect = GetCharDest(t[i], currentline, 1);
				currentline.X = rect.Right;
			}

			if (currentline.X > output.X) output.X = currentline.X;
			return output * new Point(scale.Value);
		}

	}
}
