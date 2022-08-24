using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Utils.input;
using Utils;
using Utils.text;

namespace Toybox {
	public abstract class Core:Game {

		protected GraphicsDeviceManager Graphics;
		protected SpriteBatch S;

		public List<Camera> Cameras = new List<Camera>();

		/// <summary> Sets the screen to this color every frame before drawing the Scene.
		/// <br></br>This is only for space outside the camera bounds. Use Camera.ClearColor instead for world background color.</summary>
		public Color ClearColor = Color.Black;

		public Core() {
			Graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			IsMouseVisible = true;
			Window.AllowUserResizing = true;
			Window.ClientSizeChanged += WindowSizeChanged;
			IsFixedTimeStep = true;
			Resources.Game = this;

			Resources.TextInput = new KeyboardInputManager() { AllowHoldRepeats = true };
			Resources.MouseInput = new MouseInputManager();
		}

		protected virtual void WindowSizeChanged(object o, EventArgs e) {
			foreach (var camera in Cameras) {
				camera.ApplyChanges(GraphicsDevice);
			}
			//if (Resources.Console != null) {
			//	Resources.Console.UpdateBounds(GraphicsDevice.Viewport.Bounds);
			//}
		}

		/// <summary> Internal Initialize logic. Override Init() instead. </summary>
		protected sealed override void Initialize() {
			Resources.Content = Content;
			ContentLoader.GraphicsDevice = GraphicsDevice;

			Resources.Blank = new Texture2D(GraphicsDevice, 1, 1);
			Resources.Blank.SetData(new Color[] { Color.White });

			base.Initialize();

			Init();
		}

		/// <summary> Called after LoadContent() </summary>
		protected abstract void Init();

		protected override void LoadContent() {
			base.LoadContent();

			S = new SpriteBatch(GraphicsDevice);
		}

		/// <summary> Internal Update logic. Override DoUpdate() instead </summary>
		protected sealed override void Update(GameTime gameTime) {
			Resources.DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (!IsActive) {
				base.Update(gameTime);
				return;
			}

			var k = Keyboard.GetState();
			UpdateInputManager(Mouse.GetState(), k);
			Resources.TextInput.UpdateControlStates(k);
			Resources.MouseInput.UpdateControlStates(Mouse.GetState());

			//if (Resources.Console == null || !Resources.Console.Active) {
				DoUpdate(); //TODO can prob combine these? not sure why they're seperated in the first place...
				UpdateScene();
				foreach (Camera c in Cameras) {
					c.Update();
				}
			//}

			//Resources.Console?.Update(Resources.TextInput, Resources.MouseInput);

			base.Update(gameTime);
		}

		/// <summary> Update your GameInputManager here. </summary>
		protected abstract void UpdateInputManager(MouseState m, KeyboardState k);

		/// <summary> Update your game logic here. </summary>
		protected abstract void DoUpdate();

		protected void EnableConsole(Font f) {
			//R.Console = new GameConsole(f, GraphicsDevice.Viewport.Bounds);
			//R.Console.Commands.Add("campos", new Command(CamPosCommand, 3, "campos <g/w> <x> <y>", "Set camera position. g = GameSpace, w = WorldSpace."));
		}

		/// <summary> Internal engine drawing function. Override DoDraw() instead. </summary>
		protected sealed override void Draw(GameTime gameTime) {
			DoDraw();

			//Draw dev tools
			//S.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
			//Resources.Console?.Draw(S);
			//S.End();

			base.Draw(gameTime);
		}

		/// <summary> Uses Cameras to draw the Scene. </summary>
		protected virtual void DoDraw() {
			Scene s = GetActiveScene();
			if (s == null) return;

			foreach (var camera in Cameras) {
				camera.DrawToBuffer(S, s, GraphicsDevice);
			}
			GraphicsDevice.SetRenderTarget(null);
			GraphicsDevice.Clear(ClearColor);
			S.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
			foreach (var camera in Cameras) {
				S.Draw(camera.Render, camera.GetScreenBounds(), Color.White);
			}
			S.End();
		}

		/// <summary> Gets Cameras[0] </summary>
		public Camera MainCamera {
			get { return Cameras[0]; }
		}

		private void CamPosCommand(string[] args) {
			bool gamespace = args[1] == "g";
			var x = int.Parse(args[2]);
			var y = int.Parse(args[3]);
			if (gamespace) {
				MainCamera.GameX = x;
				MainCamera.GameY = y;
			} else {
				MainCamera.WorldX = x;
				MainCamera.WorldY = y;
			}
		}

		private string GetCamInfo() {
			var output = new StringBuilder();
			output.Append("Game: " + MainCamera.GameX.ToString() + ", " + MainCamera.GameY.ToString() + ", " + MainCamera.GameWidth.ToString() + ", " + MainCamera.GameHeight.ToString());
			output.Append(Font.Newline);
			output.Append("World: " + MainCamera.WorldX.ToString() + ", " + MainCamera.WorldY.ToString() + ", " + MainCamera.WorldWidth.ToString() + ", " + MainCamera.WorldHeight.ToString());
			output.Append(Font.Newline);
			output.Append("Screen: " + GraphicsDevice.Viewport.Bounds.Width.ToString() + ", " + GraphicsDevice.Viewport.Bounds.Height.ToString());
			output.Append(Font.Newline);
			var renderscale = MainCamera.GetScreenBounds().Width / MainCamera.Render.Width;
			output.Append("GS: " + MainCamera.GameScale.ToString() + ", RS: " + renderscale.ToString());
			return output.ToString();
		}

		/// <summary> Return your SceneManager.ActiveScene </summary>
		protected abstract Scene GetActiveScene();

		/// <summary> Call your SceneManager.Update() </summary>
		protected abstract void UpdateScene();

	}
}
