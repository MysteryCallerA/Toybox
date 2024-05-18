using Microsoft.Xna.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using Toybox.components;
using Toybox.components.colliders;
using Toybox.utils.math;

namespace Toybox
{
    public abstract class Entity: IComparable<Entity> {

		public int Id = -1;
		public Point MapCell;

		public Point Position;
		public Vector2 Speed = Vector2.Zero;

		public string Name;
		public Hitbox Hitbox;
		public EntityCollider Collider;
		public bool Visible = true;
		public bool Destroy = false;

		public Entity() {
			Hitbox = new Hitbox(this);
		}

		public virtual void Update() {
			Collider.ResetState();
			Collider.ApplyMove(Speed.ToPoint());
		}

		public void Draw(Renderer r, Camera c) {
			if (!Visible) return;
			DoDraw(r, c);
		}

		protected virtual void DoDraw(Renderer r, Camera c) {
		}

		public virtual void DrawHitboxes(Renderer r, Camera c) {
			Hitbox.Draw(r, c);
		}

		public int X {
			get { return Position.X; } set { Position.X = value; }
		}

		public int Y {
			get { return Position.Y; } set { Position.Y = value; }
		}

		public override int GetHashCode() {
			if (Id == -1) return base.GetHashCode();
			return Id.GetHashCode();
		}

		public override bool Equals(object obj) {
			if (Id == -1) return base.Equals(obj);
			return Id.Equals(obj);
		}

		public int CompareTo([AllowNull] Entity other) {
			if (Depth < other.Depth) return -1;
			if (Depth > other.Depth) return 1;
			return 0;
		}

		public virtual float Depth {//TODO fix this so defualt scale is from -1 to 1 based on pos
			get { return Y; }
		}

	}
}
