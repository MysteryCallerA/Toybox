using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.utils.text {
	public class Text:TextRenderer {

		private bool SizeUnknown = true;
		private Point _size;

		public Text(Font f) : base(f) {
			Font = f;
		}

		public EventHandler OnContentChanged;
		private string _content = "";
		public virtual string Content {
			get { return _content; }
			set {
				//value = value.Replace("\r", String.Empty); <- dont know why this is here??
				_content = value;
				OnContentChanged?.Invoke(this, new EventArgs());
				SizeUnknown = true;
			}
		}

		public virtual int X {
			get; set;
		}

		public virtual int Y {
			get; set;
		}

		public Point Position {
			get {
				return new Point(X, Y);
			}
			set {
				X = value.X;
				Y = value.Y;
			}
		}

		public virtual Point Size {
			get {
				if (SizeUnknown) {
					_size = GetSize(Content);
				}
				return _size;
			}
		}

		public Rectangle Bounds {
			get {
				return new Rectangle(Position, Size);
			}
		}

		public void Draw(Renderer r) {
			Draw(r, Color, Position - Scroll, Content);
		}

		public virtual void Draw(Renderer r, Color color) {
			Draw(r, color, Position - Scroll, Content);
		}



		public override Font Font {
			set {
				if (base.Font != null && base.Font.Equals(value)) return;
				base.Font = value;
				SizeUnknown = true;
			}
		}

		public override int WordSpace {
			set {
				if (base.WordSpace.Equals(value)) return;
				base.WordSpace = value;
				SizeUnknown = true;
			}
		}

		public override int LineSpace {
			set {
				if (base.LineSpace.Equals(value)) return;
				base.LineSpace = value;
				SizeUnknown = true;
			}
		}

		public override int LetterSpace {
			set {
				if (base.LetterSpace.Equals(value)) return;
				base.LetterSpace = value;
				SizeUnknown = true;
			}
		}

		public override int QuoteSpace {
			set {
				if (base.QuoteSpace.Equals(value)) return;
				base.QuoteSpace = value;
				SizeUnknown = true;
			}
		}

	}
}
