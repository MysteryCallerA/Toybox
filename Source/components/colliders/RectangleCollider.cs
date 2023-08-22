using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.utils;

namespace Toybox.components.colliders {

	public class RectangleCollider:EntityCollider { //NEXT comment this

		public readonly Entity Parent;

		/// <summary> When hitting a corner perfectly, while true, it will prioritize moving horizontally. Otherwise, vertically. </summary>
		public bool HCornerPriority = true;

		/// <summary> The order you check for collisions is important here for performance. Check for easy, common things first (like tiles), then solid entities, then non-solid entities. </summary>
		public Func<Rectangle, IEnumerable<Collision>> FindCollisions;

		public List<Collision> Touching = new List<Collision>();

		private Rectangle PrevBounds;
		private enum CollisionDirection { None, Vertical, Horizontal, Both };

		public RectangleCollider(Entity parent, Func<Rectangle, IEnumerable<Collision>> findCollisions) {
			Parent = parent;
			FindCollisions = findCollisions;
		}

		public override void ApplyMove(Point move) {
			if (move == Point.Zero) return;

			PrevBounds = Parent.Hitbox.Bounds;
			Parent.Position += move;

			if (!FinalizeMove(out var c)) {
				ResolveCollision(c.Value);
			}
		}

		private bool FinalizeMove(out Collision? c) {
			Touching.Clear();
			var bounds = Parent.Hitbox.Bounds;
			var truebounds = bounds;
			//bounds.Inflate(1, 1);

			foreach (var col in FindCollisions.Invoke(bounds)) {
				Touching.Add(col);
				if (IsCollisionSolid(col, PrevBounds) && col.Hitbox.Intersects(truebounds)) {
					c = col;
					return false;
				}
			}
			c = null;
			return true;
		}

		private void ResolveCollision(Collision c) {
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
				Parent.Speed.Y = 0;
				ApplyMove(new Point(fullmove.X, move.Y));
			} else if (dir == CollisionDirection.Horizontal) {
				Parent.Speed.X = 0;
				ApplyMove(new Point(move.X, fullmove.Y));
			} else {
				Parent.Speed = Vector2.Zero;
				ApplyMove(new Point(move.X, move.Y));
			}
		}

		protected virtual bool IsCollisionSolid(Collision c, Rectangle referencePos) {
			return c.Type.IsSolid;
		}

		public override Collision? FindSolid(Rectangle box) {
			foreach (var c in FindCollisions.Invoke(box)) {
				if (IsCollisionSolid(c, Parent.Hitbox.Bounds)) return c;
			}
			return null;
		}



	}
}
