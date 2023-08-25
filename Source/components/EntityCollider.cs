using Microsoft.Xna.Framework;

namespace Toybox.components
{
	public abstract class EntityCollider {

		public abstract void ApplyMove(Point move);

		public abstract Collision? FindSolid(Rectangle box);

		/// <summary> Checks if a Collision should be treated as solid. </summary>
		/// <param name="referencePos"> Some collisions are different depending on the Collider's reference position. </param>
		public virtual bool IsCollisionSolid(in Collision c, Point referencePos) {
			return c.Type.IsSolid;
		}

		/// <summary> Checks if a Collision should be treated as solid. Uses Parent.Hitbox.Position as reference position. </summary>
		public abstract bool IsCollisionSolid(in Collision c);

	}
}
