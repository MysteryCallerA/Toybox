using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Toybox.components;
using Toybox.rendermodels;
using Toybox.utils.math;

namespace Toybox {
	public class Camera { //TODO rename world and game and SubPixelPosition to use updated terminology

		public float TrueWorldX, TrueWorldY;

		/// <summary> Position of the camera in render pixels. </summary>
		public int WorldX {
			get { return (int)TrueWorldX; }
			set { TrueWorldX = value; }
		}

		/// <summary> Position of the camera in render pixels. </summary>
		public int WorldY {
			get { return (int)TrueWorldY; }
			set { TrueWorldY = value; }
		}

		/// <summary> Position of the camera in scaled game pixels. </summary>
		public int GameX {
			get { return (int)TrueWorldX / GameScale; }
			set { TrueWorldX = value * GameScale; }
		}

		/// <summary> Position of the camera in scaled game pixels. </summary>
		public int GameY {
			get { return (int)TrueWorldY / GameScale; }
			set { TrueWorldY = value * GameScale; }
		}

		/// <summary> The extra subpixels of the camera position. Subtract this to align static elements to the camera pixel grid. </summary>
		public Point SubPixelPosition {
			get { return new Point(WorldX % GameScale, WorldY % GameScale); }
		}

		private int _GameScale = 1;

		/// <summary> Scale individually applied to everything that's drawn. </summary>
		public int GameScale { //TODO change this so ZoomFocus is only used in a seperate function SetGameScale(Point focus)
			get { return _GameScale; }
			set { //TODO need t a way to set more easily without using focus
				if (value < 1) value = 1;

				var gamebounds = GetGameBounds();
				Point focus = new Point(gamebounds.X + (int)(gamebounds.Width * ZoomFocus.X), gamebounds.Y + (int)(gamebounds.Height * ZoomFocus.Y));
				_GameScale = value;
				GameX = focus.X - (int)(GameWidth * ZoomFocus.X);
				GameY = focus.Y - (int)(GameHeight * ZoomFocus.Y);
			}
		}

		public int RenderScale {
			get { return RenderModel.GetRenderScale(); }
		}

		/// <summary> The size of the screen in world space using render pixels. </summary>
		public int WorldWidth, WorldHeight;

		/// <summary> The size of the screen in world space using scaled game pixels. </summary>
		public int GameWidth {
			get { return WorldWidth / GameScale; }
		}

		/// <summary> The size of the screen in world space using scaled game pixels. </summary>
		public int GameHeight {
			get { return WorldHeight / GameScale; }
		}

		public Point WorldSize {
			get { return new Point(WorldWidth, WorldHeight); }
		}

		public RenderModel RenderModel;
		public Color ClearColor = Color.Black;
		public FocusModes FocusMode = FocusModes.Center;
		public Vector2 ZoomFocus = new Vector2(0.5f, 0.5f);

		public List<CameraComponent> Components = new List<CameraComponent>();

		public enum FocusModes {
			TopLeft, Center
		}

		public RenderTarget2D Render;

		public Camera(GraphicsDevice g, RenderModel render) {
			RenderModel = render;
			render.Initialize(g, this);
		}

		public void ApplyChanges(GraphicsDevice g) {
			RenderModel.Apply(g, this);
		}

		public void DrawToBuffer(Renderer r, Scene scene, GraphicsDevice g) {
			g.SetRenderTarget(Render);
			g.Clear(ClearColor);

			r.Batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
			scene.Draw(r, this);
			r.Batch.End();
		}

		public void DrawToScreen(Renderer r, Scene scene, GraphicsDevice g) {
			DrawToBuffer(r, scene, g); //needs to draw to a buffer first still to apply screen fitting/scaling

			g.SetRenderTarget(null);
			r.Batch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
			r.Batch.Draw(Render, GetScreenBounds(), Color.White);
			r.Batch.End();
		}

		public int ScreenWidth {
			get { return RenderModel.GetScreenWidth(this); }
		}

		public int ScreenHeight {
			get { return RenderModel.GetScreenHeight(this); }
		}

		public Rectangle GetScreenBounds() {
			return RenderModel.GetScreenBounds(this);
		}

		public Rectangle GetWorldBounds() {
			return new Rectangle(WorldX, WorldY, WorldWidth, WorldHeight);
		}

		public Rectangle GetGameBounds() {
			return new Rectangle(GameX, GameY, GameWidth, GameHeight);
		}

		public Point GamePosition {
			get { return new Point(GameX, GameY); }
			set {
				GameX = value.X;
				GameY = value.Y;
			}
		}

		public virtual void Update() {
			foreach (var c in Components) {
				c.Apply(this);
			}
		}

		/// <summary> Set the ZoomFocus using a point on the screen. </summary>
		public void SetZoomFocusFromScreen(Point p) {
			var screen = GetScreenBounds();
			ZoomFocus = new Vector2((float)(p.X - screen.X) / screen.Width, (float)(p.Y - screen.Y) / screen.Height);
		}

		public int ScreenPixelSize {
			get { return RenderModel.GetScreenPixelSize(this); }
		}

		public Point WorldPosition {
			get { return new Point(WorldX, WorldY); }
			set {
				WorldX = value.X;
				WorldY = value.Y;
			}
		}

		/// <summary> Give bounds in GameSpace </summary>
		public void SnapToBounds(Rectangle bounds) {
			var cam = GetGameBounds();

			if (bounds.Width < cam.Width) {
				GameX = bounds.Center.X - cam.Width / 2;
			} else {
				if (cam.Left < bounds.Left) GameX = bounds.Left;
				if (cam.Right > bounds.Right) GameX = bounds.Right - cam.Width;
			}

			if (bounds.Height < cam.Height) {
				GameY = bounds.Center.Y - cam.Height / 2;
			} else {
				if (cam.Top < bounds.Top) GameY = bounds.Top;
				if (cam.Bottom > bounds.Bottom) GameY = bounds.Bottom - cam.Height;
			}
		}


		// ------------- Projection ------------

		/// <summary> The different spaces you can project points between. </summary>
		public enum Space {
			/// <summary> Game logic space. Cooresponds to texture pixel size. </summary>
			Pixel,
			/// <summary> Subpixel game logic space. This is the true pixel size. Project here for static renders. </summary>
			Subpixel,
			/// <summary> Space that you render to. Project mouse from screen to here for interaction with static elements. </summary>
			Render,
			/// <summary> Space on the screen. </summary>
			Screen
		}

		public Point Project(Space from, Space to, Point p) {
			if (from == to) return p;

			if ((int)from < (int)to) {
				if (from == Space.Pixel) {
					p = GameToWorld(p);
					from = Space.Subpixel;
					if (to == Space.Subpixel) return p;
				}
				if (from == Space.Subpixel) {
					p = WorldToRender(p);
					if (to == Space.Render) return p;
				}
				return RenderToScreen(p);
			}

			if (from == Space.Screen) {
				p = ScreenToRender(p);
				from = Space.Render;
				if (to == Space.Render) return p;
			}
			if (from == Space.Render) {
				p = RenderToWorld(p);
				if (to == Space.Subpixel) return p;
			}
			return WorldToGame(p);
		}

		public Rectangle Project(Space from, Space to, Rectangle r) {
			if (from == to) return r;

			if ((int)from < (int)to) {
				if (from == Space.Pixel) {
					r = GameToWorld(r);
					from = Space.Subpixel;
					if (to == Space.Subpixel) return r;
				}
				if (from == Space.Subpixel) {
					r = WorldToRender(r);
					if (to == Space.Render) return r;
				}
				return RenderToScreen(r);
			}

			if (from == Space.Screen) {
				r = ScreenToRender(r);
				from = Space.Render;
				if (to == Space.Render) return r;
			}
			if (from == Space.Render) {
				r = RenderToWorld(r);
				if (to == Space.Subpixel) return r;
			}
			return WorldToGame(r);
		}


		//--------- Point Projection ----------
		private Point RenderToScreen(Point p) {
			return RenderModel.WorldToScreen(p, this);
		}

		private Point WorldToRender(Point p) {
			return p - WorldPosition;
		}

		private Point GameToWorld(Point p) {
			return new Point(p.X * GameScale, p.Y * GameScale);
		}

		private Point ScreenToRender(Point p) {
			return RenderModel.ScreenToWorld(p, this);
		}

		private Point RenderToWorld(Point p) {
			return p + WorldPosition;
		}

		private Point WorldToGame(Point p) {
			return new Point(MathOps.FloorDiv(p.X, GameScale), MathOps.FloorDiv(p.Y, GameScale));
		}

		//---------- Rect Projection -----------
		private Rectangle RenderToScreen(Rectangle r) {
			return RenderModel.WorldToScreen(r, this);
		}

		private Rectangle WorldToRender(Rectangle r) {
			r.Location -= WorldPosition;
			return r;
		}

		private Rectangle GameToWorld(Rectangle r) {
			return new Rectangle(r.X * GameScale, r.Y * GameScale, r.Width * GameScale, r.Height * GameScale);
		}

		private Rectangle ScreenToRender(Rectangle r) {
			return RenderModel.ScreenToWorld(r, this);
		}

		private Rectangle RenderToWorld(Rectangle r) {
			r.Location += WorldPosition;
			return r;
		}

		private Rectangle WorldToGame(Rectangle r) {
			return new Rectangle(MathOps.FloorDiv(r.X, GameScale), MathOps.FloorDiv(r.Y, GameScale), MathOps.FloorDiv(r.Width, GameScale), MathOps.FloorDiv(r.Height, GameScale));
		}

		//-------- Special Projection ----------
		public Rectangle ProjectSubpixelToPixelShrink(Rectangle r) {
			var output = new Rectangle(MathOps.CeilDiv(r.X, GameScale), MathOps.CeilDiv(r.Y, GameScale), 0, 0);
			output.Width = MathOps.FloorDiv(r.Right, GameScale) - output.X;
			output.Height = MathOps.FloorDiv(r.Bottom, GameScale) - output.Y;
			return output;
		}

		public Rectangle ProjectSubpixelToPixelGrow(Rectangle r) {
			var output = new Rectangle(MathOps.FloorDiv(r.X, GameScale), MathOps.FloorDiv(r.Y, GameScale), 0, 0);
			output.Width = MathOps.CeilDiv(r.Right, GameScale) - output.X;
			output.Height = MathOps.CeilDiv(r.Bottom, GameScale) - output.Y;
			return output;
		}

	}
}
