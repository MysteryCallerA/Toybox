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
		protected Renderer Renderer;

		public Camera Camera;

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
			Resources.Random = new Random();
		}

		protected virtual void WindowSizeChanged(object o, EventArgs e) {
			bool needApply = false;
			if (Window.ClientBounds.Width > 4000) {
				Graphics.PreferredBackBufferWidth = 4000;
				needApply = true;
			}
			if (Window.ClientBounds.Height > 4000) {
				Graphics.PreferredBackBufferHeight = 4000;
				needApply = true;
			}
			if (needApply) {
				Graphics.ApplyChanges();
			}

			Camera.ApplyChanges(GraphicsDevice);
			//if (Resources.Console != null) {
			//	Resources.Console.UpdateBounds(GraphicsDevice.Viewport.Bounds);
			//}
		}

		/// <summary> Internal Initialize logic. Override Init() instead. </summary>
		protected sealed override void Initialize() {
			Resources.Content = Content;
			ContentLoader.GraphicsDevice = GraphicsDevice;

			base.Initialize();

			Init();
		}

		/// <summary> Called after LoadContent() </summary>
		protected abstract void Init();

		protected override void LoadContent() {
			base.LoadContent();

			Resources.Blank = new Texture2D(GraphicsDevice, 1, 1);
			Resources.Blank.SetData(new Color[] { Color.White });

			Renderer = new Renderer(new SpriteBatch(GraphicsDevice), Resources.Blank);
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
				Camera.Update();
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

		/// <summary> Uses Cameras to draw the Scene. Recommended to do drawing in Scene objects instead so Camera transforms are applied. </summary>
		protected virtual void DoDraw() {
			Scene s = GetActiveScene();
			if (s == null) return;

			Camera.DrawToBuffer(Renderer, s, GraphicsDevice);
			
			GraphicsDevice.SetRenderTarget(null);
			GraphicsDevice.Clear(ClearColor);
			Renderer.Batch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
			Renderer.Batch.Draw(Camera.Render, Camera.GetScreenBounds(), Color.White);
			Renderer.Batch.End();
		}

		private void CamPosCommand(string[] args) {
			bool gamespace = args[1] == "g";
			var x = int.Parse(args[2]);
			var y = int.Parse(args[3]);
			if (gamespace) {
				Camera.GameX = x;
				Camera.GameY = y;
			} else {
				Camera.WorldX = x;
				Camera.WorldY = y;
			}
		}

		private string GetCamInfo() {
			var output = new StringBuilder();
			output.Append("Game: " + Camera.GameX.ToString() + ", " + Camera.GameY.ToString() + ", " + Camera.GameWidth.ToString() + ", " + Camera.GameHeight.ToString());
			output.Append(Font.Newline);
			output.Append("World: " + Camera.WorldX.ToString() + ", " + Camera.WorldY.ToString() + ", " + Camera.WorldWidth.ToString() + ", " + Camera.WorldHeight.ToString());
			output.Append(Font.Newline);
			output.Append("Screen: " + GraphicsDevice.Viewport.Bounds.Width.ToString() + ", " + GraphicsDevice.Viewport.Bounds.Height.ToString());
			output.Append(Font.Newline);
			var renderscale = Camera.GetScreenBounds().Width / Camera.Render.Width;
			output.Append("GS: " + Camera.GameScale.ToString() + ", RS: " + renderscale.ToString());
			return output.ToString();
		}

		/// <summary> Return your SceneManager.ActiveScene </summary>
		protected abstract Scene GetActiveScene();

		/// <summary> Call your SceneManager.Update() </summary>
		protected abstract void UpdateScene();

	}
}
