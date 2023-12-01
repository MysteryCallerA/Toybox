using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Toybox.utils.text;

namespace Toybox.tools {
	public class SlowStepPanel {

		public bool Visible = false;

		private Text TextRenderer;
		private TextMeasurer TextM = new TextMeasurer();
		private Rectangle HoverRect;
		private int HoverWord;
		private bool PlayingSingleFrame = false;
		private bool PlayingSlow = false;
		private int SlowTimer = 0;
		private int SlowTime = 10;

		private const string TextPaused = "Slow\nPlay\n>>>";
		private const string TextPlaying = "Slow\nPause";

		public SlowStepPanel(Font f) {
			TextRenderer = new Text(f) { Content = TextPlaying, Scale = 2 };
		}

		private bool Paused {
			get { return !Resources.Game.Running; }
			set { Resources.Game.Running = !value; }
		}

		public void TogglePause() {
			Paused = !Paused;
			if (Paused) TextRenderer.Content = TextPaused;
			else {
				TextRenderer.Content = TextPlaying;
				PlayingSlow = false;
			}
		}

		public void PressStep() {
			PlayingSingleFrame = true;
			Paused = false;
		}

		public void PressSlow() {
			PlayingSlow = !PlayingSlow;
			SlowTimer = 0;
		}

		public void Update() {
			if (PlayingSingleFrame) {
				PlayingSingleFrame = false;
				Paused = true;
			}
			if (PlayingSlow) {
				SlowTimer++;
				if (SlowTimer > SlowTime) {
					SlowTimer = 0;
					PressStep();
				}
			}

			if (!Visible) return;

			TextM.Update(TextRenderer);
			TextRenderer.Position = new Point(Resources.Camera.GetScreenBounds().Right - TextM.Size.X, 1);
			HoverRect = TextM.PickWord(Resources.MouseInput.Position, out HoverWord) ?? Rectangle.Empty;

			if (HoverWord == -1) return;
			if (Resources.MouseInput.LeftPress) {
				if (HoverWord == 1) TogglePause();
				else if (HoverWord == 2) PressStep();
				else if (HoverWord == 0) PressSlow();
			}
		}

		public void Draw(Renderer r, Camera c) {
			if (!Visible) return;
			if (HoverRect != Rectangle.Empty) {
				r.DrawRectDirect(HoverRect, Color.Gray);
			}

			TextRenderer.Draw(r.Batch);
		}

	}
}
