using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Utils.save;

namespace Toybox {
	public abstract class Entity:IXmlSaveable, IComparable<Entity> {

		public int Id = -1;
		public Point MapCell;

		public Point Position;
		public string Name;

		public virtual void Update() {
		}

		public virtual void Draw(SpriteBatch s, Camera c) {
		}

		public abstract void Save(XmlWriter writer);

		public int X {
			get { return Position.X; }
			set { Position.X = value; }
		}

		public int Y {
			get { return Position.Y; }
			set { Position.Y = value; }
		}

		public override int GetHashCode() {
			return Id.GetHashCode();
		}

		public override bool Equals(object obj) {
			return Id.Equals(obj);
		}

		public int CompareTo([AllowNull] Entity other) {
			if (Depth < other.Depth) return -1;
			if (Depth > other.Depth) return 1;
			return 0;
		}

		public abstract Rectangle GetHitbox();

		public virtual float Depth {//TODO fix this so defualt scale is from -1 to 1 based on pos
			get { return Y; }
		}

	}
}
