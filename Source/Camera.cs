﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Toybox.rendermodels;
using Toybox.scenes;
using Toybox.utils.math;

namespace Toybox {
	public class Camera {

		public int PixelScale { get; private set; } = 1;
		public int X, Y;
		public int Width, Height;

		public RenderTarget2D Render;
		public RenderModel RenderModel;
		public Color ClearColor = Color.Black;

		public bool ScaleTransitionActive = false;
		private int TargetPixelScale;
		private int StartingPixelScale;
		private int ScaleTransitionTime;
		private int ScaleTransitionTimer;
		private bool ScaleTransitionReverse;

		private Rectangle ViewSource;
		public Point ViewSize { get { return ViewSource.Size; } }
		public float ScaleTransitionPixelMultiplier { get; private set; } = 1;

		public Camera(GraphicsDevice g, RenderModel render, int pixelScale = 1) {
			PixelScale = pixelScale;
			RenderModel = render;
			render.Initialize(g, this);
		}

		internal void ApplyChanges(GraphicsDevice g) {
			RenderModel.Apply(g, this);
			ViewSource = new Rectangle(0, 0, Render.Width, Render.Height);
		}

		public void DrawToBuffer(Renderer r, Scene scene, GraphicsDevice g) {
			g.SetRenderTarget(Render);
			g.Clear(ClearColor);

			r.Batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
			scene.Draw(r, this);
			r.Batch.End();
		}

		public void DrawBufferToScreen(Renderer r, GraphicsDevice g) {
			g.SetRenderTarget(null);
			g.Clear(ClearColor);
			
			r.Batch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
			r.Batch.Draw(Render, GetScreenBounds(), ViewSource, Color.White);
			r.Batch.End();
		}

		public void ChangePixelScale(int targetScale, int transitionFrames = 0) {
			if (targetScale == PixelScale) return;

			ScaleTransitionTime = transitionFrames;
			ScaleTransitionTimer = 0;
			ScaleTransitionActive = true;
			if (targetScale < PixelScale) {
				ScaleTransitionReverse = true;
				StartingPixelScale = targetScale;
				TargetPixelScale = PixelScale;
			} else {
				ScaleTransitionReverse = false;
				TargetPixelScale = targetScale;
				StartingPixelScale = PixelScale;
			}
		}

		internal void Update() {
			if (!ScaleTransitionActive) {
				return;
			}
			if (ScaleTransitionTimer == 0 && ScaleTransitionReverse) {
				PixelScale = StartingPixelScale;
			}
			ScaleTransitionTimer++;

			var step = ScaleTransitionTimer;
			if (ScaleTransitionReverse) {
				step = ScaleTransitionTime - step;
			}

			if (ScaleTransitionTimer >= ScaleTransitionTime) {
				if (!ScaleTransitionReverse) {
					PixelScale = TargetPixelScale;
				}
				ViewSource = new Rectangle(0, 0, Render.Width, Render.Height);
				ScaleTransitionPixelMultiplier = 1;
				ScaleTransitionActive = false;
				return;
			}

			int hdif = (int)Math.Round(Render.Width * ((float)StartingPixelScale / TargetPixelScale)) - Render.Width;
			int vdif = (int)Math.Round(Render.Height * ((float)StartingPixelScale / TargetPixelScale)) - Render.Height;
			float m = (float)step / ScaleTransitionTime;
			var h = (int)Math.Round(hdif * m);
			var v = (int)Math.Round(vdif * m);

			ViewSource = new Rectangle(0, 0, Render.Width + h, Render.Height + v);
			ScaleTransitionPixelMultiplier = m * ((float)StartingPixelScale / TargetPixelScale);
		}

		public int ScaledX {
			get { return X / PixelScale; }
			set { X = value * PixelScale; }
		}
		public int ScaledY {
			get { return Y / PixelScale; }
			set { Y = value * PixelScale; }
		}

		public int ScaledWidth { get { return Width / PixelScale; } }
		public int ScaledHeight { get { return Height / PixelScale; } }
		public int ScreenWidth { get { return RenderModel.GetScreenWidth(this); } }
		public int ScreenHeight { get { return RenderModel.GetScreenHeight(this); } }

		public Point Size {	get { return new Point(Width, Height); } }
		public Point ScaledSize { get { return new Point(ScaledWidth, ScaledHeight); } }

		public Rectangle Bounds { get { return new Rectangle(X, Y, Width, Height); } }
		public Rectangle ScaledBounds { get { return new Rectangle(ScaledX, ScaledY, ScaledWidth, ScaledHeight); } }
		public Rectangle GetScreenBounds() { return RenderModel.GetScreenBounds(this); }

		public Point Position {
			get { return new Point(X, Y); }
			set { X = value.X; Y = value.Y; }
		}
		public Point ScaledPosition {
			get { return new Point(ScaledX, ScaledY); }
			set { ScaledX = value.X; ScaledY = value.Y; }
		}


		// ------------- Projection ------------

		/// <summary> The different spaces you can project points between. </summary>
		public enum Space {
			/// <summary> Game logic space. Cooresponds to texture pixel size. </summary>
			Scaled,
			/// <summary> Subpixel game logic space. This is the true pixel size. Project here for static renders. </summary>
			Pixel,
			/// <summary> Space that you render to. Project mouse from screen to here for interaction with static elements. </summary>
			Render,
			/// <summary> Space on the screen. </summary>
			Screen
		}

		public Point Project(Space from, Space to, Point p) {
			if (from == to) return p;

			if ((int)from < (int)to) {
				if (from == Space.Scaled) {
					p = ScaledToPixel(p);
					from = Space.Pixel;
					if (to == Space.Pixel) return p;
				}
				if (from == Space.Pixel) {
					p = PixelToRender(p);
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
				p = RenderToPixel(p);
				if (to == Space.Pixel) return p;
			}
			return PixelToScaled(p);
		}

		public Rectangle Project(Space from, Space to, Rectangle r) {
			if (from == to) return r;

			if ((int)from < (int)to) {
				if (from == Space.Scaled) {
					r = ScaledToPixel(r);
					from = Space.Pixel;
					if (to == Space.Pixel) return r;
				}
				if (from == Space.Pixel) {
					r = PixelToRender(r);
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
				r = RenderToPixel(r);
				if (to == Space.Pixel) return r;
			}
			return PixelToScaled(r);
		}


		//--------- Point Projection ----------
		private Point ScaledToPixel(Point p) {
			return new Point(p.X * PixelScale, p.Y * PixelScale);
		}
		private Point PixelToRender(Point p) {
			return p - Position;
		}
		private Point RenderToScreen(Point p) {
			return RenderModel.RenderToScreen(p, this);
		}

		private Point ScreenToRender(Point p) {
			return RenderModel.ScreenToRender(p, this);
		}
		private Point RenderToPixel(Point p) {
			return p + Position;
		}
		private Point PixelToScaled(Point p) {
			return new Point(MathOps.FloorDiv(p.X, PixelScale), MathOps.FloorDiv(p.Y, PixelScale));
		}

		//---------- Rect Projection -----------
		private Rectangle ScaledToPixel(Rectangle r) {
			return new Rectangle(r.X * PixelScale, r.Y * PixelScale, r.Width * PixelScale, r.Height * PixelScale);
		}
		private Rectangle PixelToRender(Rectangle r) {
			r.Location -= Position;
			return r;
		}
		private Rectangle RenderToScreen(Rectangle r) {
			return RenderModel.RenderToScreen(r, this);
		}

		private Rectangle ScreenToRender(Rectangle r) {
			return RenderModel.ScreenToRender(r, this);
		}
		private Rectangle RenderToPixel(Rectangle r) {
			r.Location += Position;
			return r;
		}
		private Rectangle PixelToScaled(Rectangle r) {
			return new Rectangle(MathOps.FloorDiv(r.X, PixelScale), MathOps.FloorDiv(r.Y, PixelScale), MathOps.FloorDiv(r.Width, PixelScale), MathOps.FloorDiv(r.Height, PixelScale));
		}

		//-------- Special Projection ----------
		public Rectangle ProjectPixelToScaledShrink(Rectangle r) {
			var output = new Rectangle(MathOps.CeilDiv(r.X, PixelScale), MathOps.CeilDiv(r.Y, PixelScale), 0, 0);
			output.Width = MathOps.FloorDiv(r.Right, PixelScale) - output.X;
			output.Height = MathOps.FloorDiv(r.Bottom, PixelScale) - output.Y;
			return output;
		}

		public Rectangle ProjectPixelToScaledGrow(Rectangle r) {
			var output = new Rectangle(MathOps.FloorDiv(r.X, PixelScale), MathOps.FloorDiv(r.Y, PixelScale), 0, 0);
			output.Width = MathOps.CeilDiv(r.Right, PixelScale) - output.X;
			output.Height = MathOps.CeilDiv(r.Bottom, PixelScale) - output.Y;
			return output;
		}

		public static Point ProjectPixelScale(Point p, int fromScale, int toScale) {
			var x = Math.Floor((float)p.X / fromScale * toScale);
			var y = Math.Floor((float)p.Y / fromScale * toScale);
			return new Point((int)x, (int)y);
		}

	}
}
