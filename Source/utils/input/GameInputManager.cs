using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.utils.input {
	public class GameInputManager<T> where T : Enum {

		private MouseState MState;
		private MouseState OMState;

		public Dictionary<T, VirtualKey> VirtualKeys = new Dictionary<T, VirtualKey>(); //TODO when a key is forced down, it should stay down until released and pressed again. inverse when forced up

		public VirtualKey this[T id] {
			get { return VirtualKeys[id]; }
		}

		public void UpdateControlStates(KeyboardState kstate, MouseState mstate, GamePadState? gstate = null) {
			OMState = MState;
			MState = mstate;

			foreach (var k in VirtualKeys.Values) {
				var next = GetNextState(k, kstate, mstate, gstate);
				if (!k.DroppedPress) {
					k.WasDown = k.Down;
					k.Down = next;
					continue;
				}
				if (next == false) {
					k.DroppedPress = false;
				}
			}
		}

		private bool GetNextState(VirtualKey k, KeyboardState kstate, MouseState mstate, GamePadState? gstate = null) {
			foreach (var input in k.Keys) {
				if (kstate.IsKeyDown(input)) {
					return true;
				}
			}

			if (k.LeftMouse && MState.LeftButton == ButtonState.Pressed) return true;
			if (k.RightMouse && MState.RightButton == ButtonState.Pressed) return true;
			if (k.MiddleMouse && MState.MiddleButton == ButtonState.Pressed) return true;
			if (k.Mouse4 && MState.XButton1 == ButtonState.Pressed) return true;
			if (k.Mouse5 && MState.XButton2 == ButtonState.Pressed) return true;

			int scroll = MState.ScrollWheelValue - OMState.ScrollWheelValue;
			if (k.ScrollDown && scroll < 0) return true;
			if (k.ScrollUp && scroll > 0) return true;

			if (!gstate.HasValue) return false;
			foreach (var input in k.Buttons) {
				if (gstate.Value.IsButtonDown(input)) return true;
			}

			return false;
		}

		public Point MousePosition {
			get { return MState.Position; }
		}

		public Point PrevMousePosition {
			get { return OMState.Position; }
		}

		public void Add(T t, VirtualKey v) {
			VirtualKeys.Add(t, v);
		}

	}

	public class VirtualKey {

		public bool Down = false;
		public bool WasDown = false;
		internal bool DroppedPress = false;

		public List<Keys> Keys = new List<Keys>();
		public List<Buttons> Buttons = new List<Buttons>();
		public bool LeftMouse = false;
		public bool RightMouse = false;
		public bool MiddleMouse = false;
		public bool Mouse4 = false;
		public bool Mouse5 = false;
		public bool ScrollUp = false;
		public bool ScrollDown = false;

		public VirtualKey(params Keys[] k) {
			Keys.AddRange(k);
		}

		public bool Pressed {
			get { return Down && !WasDown; }
		}

		public bool Released {
			get { return !Down && WasDown; }
		}

		public void DropPress() {
			DroppedPress = true;
			Down = false;
			WasDown = false;
		}
	}
}
