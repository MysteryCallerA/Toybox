using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.input;

namespace Toybox.components.control {

	public class MouseDrag: CameraComponent, EntityComponent {

		public VirtualKey Key;
		public Func<Point> MousePos;

		private Point MouseDragOffset;
		private Point MouseDragStartPos;

		public MouseDrag(VirtualKey k, Func<Point> mousepos) {
			Key = k;
			MousePos = mousepos;
		}

		public bool Dragging {
			get; private set;
		}

		public void Apply(Camera c) {
			if (Dragging) {
				var movecam = MousePos.Invoke() - MouseDragOffset;
				movecam /= new Point(c.ScreenPixelSize * 3, c.ScreenPixelSize * 3);
				c.GamePosition = MouseDragStartPos - movecam;
				if (!Key.Down) {
					Dragging = false;
				}
			}

			if (!c.GetScreenBounds().Contains(MousePos.Invoke())) {
				return;
			}

			if (Key.Pressed) {
				MouseDragOffset = MousePos.Invoke();
				MouseDragStartPos = c.GamePosition;
				Dragging = true;
			}
		}

		public void Apply(Entity e) {
			throw new NotImplementedException();
		}
	}
}
