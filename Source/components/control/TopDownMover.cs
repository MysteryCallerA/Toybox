using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.utils.input;

namespace Toybox.components.control {
	public class TopDownMover:CameraComponent, EntityComponent {

		public VirtualKey Up, Down, Left, Right;

		public int Speed = 1;

		public TopDownMover(VirtualKey up, VirtualKey down, VirtualKey left, VirtualKey right) {
			Up = up;
			Down = down;
			Left = left;
			Right = right;
		}

		public void Apply(Camera c) {
			throw new NotImplementedException();
		}

		public void Apply(Entity e) {
			Point move = Point.Zero;
			if (Up.Down) move.Y -= Speed;
			if (Down.Down) move.Y += Speed;
			if (Left.Down) move.X -= Speed;
			if (Right.Down) move.X += Speed;

			e.Position += move;
		}
	}
}
