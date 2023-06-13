using Microsoft.Xna.Framework.Input;
using Utils.text;

namespace Toybox.debug {
	public class DebugManager {

		public bool Enabled = true;

		public Keys KeyHighlight = Keys.F1;
		public Keys KeyToggleInfo = Keys.F2;

		public DebugInfoPanel InfoPanel;
		public DebugHighlighter Highlighter;

		private DebugState CurrentState = DebugState.Inactive;
		private enum DebugState {
			Inactive, Highlighting, InfoInteract, InfoPassive
		}

		public DebugManager(Font f) {
			InfoPanel = new DebugInfoPanel(f);
			Highlighter = new DebugHighlighter();
		}

		public void Update() {
			if (Resources.TextInput.Pressed(KeyHighlight)) CurrentState = DebugState.Highlighting;
			if (Resources.TextInput.Pressed(KeyToggleInfo)) {
				if (CurrentState == DebugState.InfoInteract) CloseInfoInteract();
				else CurrentState = DebugState.InfoInteract;
			}

			if (CurrentState == DebugState.Inactive) return;

			if (CurrentState == DebugState.Highlighting) Highlighter.Update(Resources.Scene);
			else if (CurrentState == DebugState.InfoInteract) InfoPanel.Update();
		}

		public void Draw(Renderer r, Camera c) {
			if (CurrentState == DebugState.Highlighting) Highlighter.Draw(r, c);
			else if (CurrentState == DebugState.InfoInteract) InfoPanel.Draw(r, c);
		}

		private void CloseInfoInteract() {
			CurrentState = DebugState.Inactive;
			//TODO once passive watch variables are implemented, add a check here and set state to InfoPassive instead
		}

		public bool Active {
			get { return CurrentState != DebugState.Inactive; }
		}

		public void SetInfoTarget(object o) {
			InfoPanel.SetTarget(o, true);
			CurrentState = DebugState.InfoInteract;
		}

	}
}
