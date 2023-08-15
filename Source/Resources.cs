using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.debug;
using Utils.input;

namespace Toybox {
	public static class Resources {

		//public static GameConsole Console;
		public static ContentManager Content;
		public static Core Game;
		public static Texture2D Blank;
		public static KeyboardInputManager TextInput;
		public static MouseInputManager MouseInput;
		public static Random Random;
		public static DebugManager Debug;
		public static GameTime GameTime;

		public static float DeltaTime = 0;

		public static Camera Camera {
			get { return Game.Camera; }
		}

		public static Scene Scene {
			get { return Game.GetActiveScene(); }
		}

	}
}
