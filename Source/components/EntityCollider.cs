using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Windows.Forms.Design;

namespace Toybox.components
{
	public abstract class EntityCollider {

		public readonly Entity Parent;

		protected EntityCollider(Entity parent) {
			Parent = parent;
		}

		public void ResetState() {
			CollidedTop = false;
			CollidedBottom = false;
			CollidedLeft = false;
			CollidedRight = false;
		}

		public abstract void ApplyMove(Point move);

		public abstract Collision? FindSolid(Rectangle box);

		public abstract Collision? FindSolid(Point p);

		public abstract IEnumerable<Collision> FindCollisions(Rectangle box);

		/// <summary> Checks if a Collision should be treated as solid. </summary>
		/// <param name="referencePos"> Some collisions are different depending on the Collider's reference position. </param>
		public virtual bool IsCollisionSolid(in Collision c, Point referencePos) {
			return c.Type == CollisionType.Solid;
		}

		/// <summary> Checks if a Collision should be treated as solid. Uses Parent.Hitbox.Position as reference position. </summary>
		public virtual bool IsCollisionSolid(in Collision c) {
			return IsCollisionSolid(in c, Parent.Hitbox.Position);
		}

		public bool CollidedTop {
			get; protected set;
		} = false;
		public bool CollidedBottom {
			get; protected set;
		} = false;
		public bool CollidedLeft {
			get; protected set;
		} = false;
		public bool CollidedRight {
			get; protected set;
		} = false;

		public bool CollidedAny {
			get { return CollidedTop || CollidedBottom || CollidedLeft || CollidedRight; }
		}

	}
}
