using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.utils;

namespace Toybox.components.colliders {

	public class RectangleCollider:EntityCollider {

		/// <summary> When hitting a corner perfectly, while true, it will prioritize moving horizontally. Otherwise, vertically. </summary>
		public bool HCornerPriority = true;

		/// <summary> The order you check for collisions is important here for performance. Check for easy, common things first (like tiles), then solid entities, then non-solid entities. </summary>
		public Func<Rectangle, IEnumerable<Collision>> FindCollisionsFunc;

		private Rectangle PrevBounds;
		private enum CollisionDirection { None, Vertical, Horizontal, Both };

		public RectangleCollider(Entity parent, Func<Rectangle, IEnumerable<Collision>> findCollisions):base(parent) {
			FindCollisionsFunc = findCollisions;
		}

		/// <summary> Moves parent while respecting solid collisions. </summary>
		public override void ApplyMove(Point move) {
			if (move == Point.Zero) return;

			PrevBounds = Parent.Hitbox.Bounds;
			Parent.Position += move;

			if (!FinalizeMove(out var c)) {
				ResolveCollision(c.Value);
			}
		}

		private bool FinalizeMove(out Collision? c) {
			var bounds = Parent.Hitbox.Bounds;

			foreach (var col in FindCollisionsFunc.Invoke(bounds)) {
				if (IsCollisionSolid(col, PrevBounds.Location) && col.Hitbox.Intersects(bounds)) {
					c = col;
					return false;
				}
			}
			c = null;
			return true;
		}

		private void ResolveCollision(in Collision c) {
			Point fullmove = Parent.Hitbox.Position - PrevBounds.Location;
			Point move = fullmove;

			while (move != Point.Zero) {
				move.X -= Math.Sign(move.X);
				move.Y -= Math.Sign(move.Y);

				Parent.Hitbox.Position = PrevBounds.Location + move;
				if (!c.Hitbox.Intersects(Parent.Hitbox.Bounds)) break;
			}

			var union = Rectangle.Union(c.Hitbox, Parent.Hitbox.Bounds);
			var dir = CollisionDirection.None;
			if ((c.Hitbox.Top == Parent.Hitbox.Bottom || c.Hitbox.Bottom == Parent.Hitbox.Top) && union.Width < c.Hitbox.Width + Parent.Hitbox.Width) {
				dir = CollisionDirection.Vertical;
			} else if ((c.Hitbox.Left == Parent.Hitbox.Right || c.Hitbox.Right == Parent.Hitbox.Left) && union.Height < c.Hitbox.Height + Parent.Hitbox.Height) {
				dir = CollisionDirection.Horizontal;
			} else { //Corner Collision
				if (fullmove.X < 0) {
					if (FindSolid(Parent.Hitbox.BoxLeft).HasValue) dir = CollisionDirection.Horizontal;
				} else if (fullmove.X > 0) {
					if (FindSolid(Parent.Hitbox.BoxRight).HasValue) dir = CollisionDirection.Horizontal;
				}
				if (fullmove.Y < 0) {
					if (FindSolid(Parent.Hitbox.BoxAbove).HasValue) dir = dir == CollisionDirection.Horizontal ? CollisionDirection.Both : CollisionDirection.Vertical;
				} else if (fullmove.Y > 0) {
					if (FindSolid(Parent.Hitbox.BoxBelow).HasValue) dir = dir == CollisionDirection.Horizontal ? CollisionDirection.Both : CollisionDirection.Vertical;
				}

				//TODO mabye priority should go to whichever direction is larger
				if (dir == CollisionDirection.None) dir = HCornerPriority ? CollisionDirection.Vertical : CollisionDirection.Horizontal;
			}

			Parent.Hitbox.Position = PrevBounds.Location;
			if (dir == CollisionDirection.Vertical) {
				if (Parent.Speed.Y > 0) CollidedBottom = true; else CollidedTop = true;
				Parent.Speed.Y = 0;
				ApplyMove(new Point(fullmove.X, move.Y));
			} else if (dir == CollisionDirection.Horizontal) {
				if (Parent.Speed.X > 0) CollidedRight = true; else CollidedLeft = true;
				Parent.Speed.X = 0;
				ApplyMove(new Point(move.X, fullmove.Y));
			} else {
				if (Parent.Speed.Y > 0) CollidedBottom = true; else CollidedTop = true;
				if (Parent.Speed.X > 0) CollidedRight = true; else CollidedLeft = true;
				Parent.Speed = Vector2.Zero;
				ApplyMove(new Point(move.X, move.Y));
			}
		}

		/// <summary> Returns the first solid collision. </summary>
		public override Collision? FindSolid(Rectangle box) {
			foreach (var c in FindCollisionsFunc.Invoke(box)) {
				if (IsCollisionSolid(c, Parent.Hitbox.Position)) return c;
			}
			return null;
		}

		public override IEnumerable<Collision> FindCollisions(Rectangle box) {
			return FindCollisionsFunc.Invoke(box);
		}

		public override Collision? FindSolid(Point p) {
			throw new NotImplementedException();
		}
	}
}
