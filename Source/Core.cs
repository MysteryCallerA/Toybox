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
using Toybox.debug;
using Toybox.utils.input;
using Toybox.utils.text;
using Toybox.utils;
using Toybox.scenes;
using System.Reflection;

namespace Toybox {
    public abstract class Core:Game {

		protected GraphicsDeviceManager Graphics;
		protected Renderer Renderer;

		public Camera Camera;
		public bool Running = true;
		public bool InUpdateStep { get; private set; } = false;
		public static bool DrawHitboxes = false;

		/// <summary> Sets the screen to this color every frame before drawing the Scene.
		/// <br></br>This is only for space outside the camera bounds. Use Camera.ClearColor instead for world background color.</summary>
		public Color ClearColor = Color.Black;

		public Core() {
			Graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			IsMouseVisible = true;
			Window.AllowUserResizing = true;
			Window.ClientSizeChanged += WindowSizeChanged;
			Resources.Game = this;

			Resources.TextInput = new KeyboardInputManager() { AllowHoldRepeats = true };
			Resources.MouseInput = new MouseInputManager();
			Resources.Random = new Random();

			IsFixedTimeStep = false;
			Graphics.SynchronizeWithVerticalRetrace = true;
			Graphics.ApplyChanges();
			TargetElapsedTime = TimeSpan.FromTicks((long)(TimeSpan.TicksPerMillisecond * (1000 / (double)60)));
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
		}

		/// <summary> Internal Initialize logic. Override Init() instead. </summary>
		protected sealed override void Initialize() {
			Resources.Content = Content;
			ContentLoader.GraphicsDevice = GraphicsDevice;

			base.Initialize();

			Init();

			Graphics.PreferredBackBufferHeight = Camera.WorldHeight;
			Graphics.PreferredBackBufferWidth = Camera.WorldWidth;
			Graphics.ApplyChanges();
			Camera.ApplyChanges(GraphicsDevice);

			Resources.Debug = new DebugManager(GetDefaultFont());
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
			Resources.GameTime = gameTime;
			if (!IsActive) {
				base.Update(gameTime);
				return;
			}

			var k = Keyboard.GetState();
			if (Running) UpdateInputManager(Mouse.GetState(), k);
			Resources.TextInput.UpdateControlStates(k);
			Resources.MouseInput.UpdateControlStates(Mouse.GetState());

			if (Running) {
				InUpdateStep = true;
				UpdateScene();
				InUpdateStep = false;
				PostUpdate();
				Camera.Update();
			}

			if (Resources.Debug.Enabled) Resources.Debug.Update();

			base.Update(gameTime);
		}

		/// <summary> Update your GameInputManager here. </summary>
		protected abstract void UpdateInputManager(MouseState m, KeyboardState k);

		/// <summary> Internal engine drawing function. Override DoDraw() instead. </summary>
		protected sealed override void Draw(GameTime gameTime) {
			DoDraw();

			if (Resources.Debug.Active) {
				//Draw dev tools
				Renderer.Batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
				Resources.Debug.Draw(Renderer, Camera);
				Renderer.Batch.End();
			}

			base.Draw(gameTime);
		}

		/// <summary> Uses Cameras to draw the Scene. Recommended to do drawing in Scene objects instead so Camera transforms are applied. </summary>
		protected virtual void DoDraw() {
			Scene s = GetActiveScene();
			if (s == null) return;

			Camera.DrawToBuffer(Renderer, s, GraphicsDevice);

			if (DrawHitboxes) {
				Renderer.Batch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
				Resources.Scene.DrawHitboxes(Renderer, Camera);
				Renderer.Batch.End();
			}

			GraphicsDevice.SetRenderTarget(null);
			GraphicsDevice.Clear(ClearColor);
			Renderer.Batch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
			Renderer.Batch.Draw(Camera.Render, Camera.GetScreenBounds(), Color.White);
			Renderer.Batch.End();
		}

		/// <summary> Return your SceneManager.ActiveScene </summary>
		public abstract Scene GetActiveScene();

		protected abstract Font GetDefaultFont();

		/// <summary> Calls GetActiveScene().Update() </summary>
		protected virtual void UpdateScene() {
			GetActiveScene().Update();
		}

		/// <summary> Calls GetActiveScene().PostUpdate. PostUpdates are for things like removing/adding entities after finished main Update. </summary>
		protected virtual void PostUpdate() {
			GetActiveScene().PostUpdate();
		}

	}
}
