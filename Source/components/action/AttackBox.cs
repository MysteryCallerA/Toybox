using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.utils;
using Utils.input;

namespace Toybox.components.action
{
    public class AttackBox:EntityComponent {

		public VirtualKey AttackButton;
		public Action<AttackBox> AttackTransform;
		public string AnchorName = "front";

		public Point StartSize = new Point(5, 5);
		public Point Size;
		public Color StartColor = Color.Red;
		public Color Color;
		public Point StartOffset = new Point(0, 5);
		public Point Offset;
		public int Time = 20;

		public Line Anchor;
		public int Timer = 0;

		public HashSet<object> Hitlist = new HashSet<object>();

		public AttackBox(VirtualKey attack) {
			AttackButton = attack;
		}

		public void Apply(ComplexEntity e) {
			if (!Attacking && AttackButton.Pressed) {
				Attack();
			}

			if (Attacking) {
				Anchor = e.Anchors[AnchorName];
				AttackTransform?.Invoke(this);
				Timer++;
				if (Timer >= Time) EndAttack();
			}
		}

		public virtual void Attack() {
			if (Attacking) return;
			Attacking = true;
			Timer = 0;
			Size = StartSize;
			Color = StartColor;
			Offset = StartOffset;
			Hitlist.Clear();
		}

		protected virtual void EndAttack() {
			Attacking = false;
		}

		public void Draw(Entity e, Renderer r, Camera c) {
			if (!Attacking) return;

			float opacity = (float)Timer / Time;
			r.DrawRect(Bounds, Color * opacity, c, Camera.Space.Subpixel);
		}

		public bool Attacking {
			get; private set;
		} = false;

		public Point Position {
			get {
				if (Anchor.Xdif < 0) {
					return new Point(Anchor.Start.X - Size.X - Offset.X, Anchor.Start.Y + Offset.Y);
				}
				return Anchor.Start + Offset;
			}
		}

		public Rectangle Bounds {
			get { return new Rectangle(Position, Size); }
		}
	}
}
