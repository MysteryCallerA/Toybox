using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.components.data;

namespace Toybox.components.colliders {
	public class GridCollider:EntityCollider {

		public Func<Point, IEnumerable<Collision>> FindCollisionsFunc;
		public GridPos Grid;

		public GridCollider(Entity parent, GridPos grid, Func<Point, IEnumerable<Collision>> findCollisionsFromGridCell):base(parent) {
			FindCollisionsFunc = findCollisionsFromGridCell;
			Grid = grid;
		}

		public override void ApplyMove(Point move) {
			if (move == Point.Zero) return;

			ResolveFreeMovement(move, out move);

			if (move == Point.Zero) return;

			ResolveCollisionMovement(move);
		}

		/// <summary>
		/// Free movement is any movement that doesn't require collision checks. If the movement is towards the home position of
		/// the current Grid.Position, then it moves the lesser of the full Move or distance to Grid.Position.
		/// </summary>
		/// <param name="move">Incoming movement distance this frame</param>
		/// <param name="remaining">Movement distance remaining after free movement</param>
		private void ResolveFreeMovement(Point move, out Point remaining) {
			var home = Grid.ToPos(Grid.Position);
			var delta = home - Parent.Position;

			int xok = 0, yok = 0;

			if (Math.Sign(delta.X) == Math.Sign(move.X)) {
				xok = Math.Min(Math.Abs(delta.X), Math.Abs(move.X)) * Math.Sign(move.X);
			}
			if (Math.Sign(delta.Y) == Math.Sign(move.Y)) {
				yok = Math.Min(Math.Abs(delta.Y), Math.Abs(move.Y)) * Math.Sign(move.Y);
			}

			var freeMove = new Point(xok, yok);
			Parent.Position += freeMove;
			remaining = move - freeMove;
		}

		/// <summary>
		/// Checks collisions as it moves. The Parent should already be aligned with the grid before calling this.
		/// </summary>
		/// <param name="move">Incoming movement distance this frame</param>
		private void ResolveCollisionMovement(Point move) {
			//Assume Parent is already aligned with the grid.
			while (move != Point.Zero) {
				var moveto = Grid.Position + new Point(Math.Sign(move.X), Math.Sign(move.Y));
				foreach (var col in FindCollisionsFunc.Invoke(moveto)) {
					if (IsCollisionSolid(col)) {
						ResolveCollision(move);
						return;
					}
				}
				Grid.Position = moveto;
				var stepx = Math.Min(Math.Abs(move.X), Grid.CellWidth) * Math.Sign(move.X);
				var stepy = Math.Min(Math.Abs(move.Y), Grid.CellHeight) * Math.Sign(move.Y);
				var step = new Point(stepx, stepy);
				Parent.Position += step;
				move -= step;
			}
		}

		private void ResolveCollision(Point move) {
			if (move.X > 0) CollidedRight = true;
			else if (move.X < 0) CollidedLeft = true;
			if (move.Y > 0) CollidedBottom = true;
			else if (move.Y < 0) CollidedTop = true;

			Parent.Speed = Vector2.Zero;
		}

		public override IEnumerable<Collision> FindCollisions(Rectangle box) {
			throw new NotImplementedException();
		}

		public override Collision? FindSolid(Rectangle box) {
			throw new NotImplementedException();
		}

		public override Collision? FindSolid(Point p) {
			throw new NotImplementedException();
		}
	}
}
