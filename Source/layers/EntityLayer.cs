using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Toybox.maps;

namespace Toybox.layers {
	public class EntityLayer<T>:SceneLayer where T:Entity {

		public EntityMap<T> Map;

		public EntityLayer(string name, EntityMap<T> map) : base(name) {
			Map = map;
		}

		protected override void DoDraw(Renderer r, Camera c) {
			Map.Draw(r, c);
		}

		protected override void DoUpdate() {
			Map.Update();
		}

		public override LayerType GetLayerType() {
			return LayerType.Entity;
		}

		public override void Save(XmlWriter writer) {
			writer.WriteStartElement("entitylayer");
			writer.WriteAttributeString("name", Name);
			writer.WriteAttributeString("visible", Visible.ToString());
			Map.Save(writer);
			writer.WriteEndElement();
		}

		public override Rectangle GetBounds() {
			return Rectangle.Empty;
		}
	}
}
