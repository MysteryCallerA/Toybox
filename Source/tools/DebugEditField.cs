using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.text;

namespace Toybox.tools {
	internal class DebugEditField {

		private Text ArrowText;
		private TextInput Text;
		public bool Active = false;
		private Type ContentType;

		public DebugEditField(Font f) {
			Text = new TextInput(f) { BackColor = Color.Black, ColorSelectBack = Color.White, ColorSelectText = Color.Black };
			ArrowText = new Text(f) { Content = "-> " };
		}

		public object GetContent() {
			if (ContentType == typeof(int)) return int.Parse(Text.Content);
			if (ContentType == typeof(float)) return float.Parse(Text.Content);
			return Text.Content;
		}

		public Point Position {
			set {
				ArrowText.Position = value;
				var size = ArrowText.GetSize();
				Text.Position = new Point(value.X + size.X, value.Y); 
			}
		}

		public void SetContent(int v) {
			Text.Content = v.ToString();
			Text.Whitelist = new HashSet<string>(TextInput.WhitelistInt);
			ContentType = typeof(int);
			Active = true;
			Text.SelectAll();
		}

		public void SetContent(float v) {
			Text.Content = v.ToString();
			Text.Whitelist = new HashSet<string>(TextInput.WhitelistFloat);
			ContentType = typeof(float);
			Active = true;
			Text.SelectAll();
		}

		public void Update() {
			if (Resources.TextInput.Pressed(Microsoft.Xna.Framework.Input.Keys.Enter)) {
				Active = false;
				return;
			}

			Text.Update(Resources.TextInput);
		}

		public void Draw(Renderer r, Camera c) {
			ArrowText.Draw(r.Batch);
			Text.Draw(r.Batch);
		}

	}
}
