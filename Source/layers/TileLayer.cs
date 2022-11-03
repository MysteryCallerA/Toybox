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
	public class TileLayer:SceneLayer {

		public Tilemap Map;

		public TileLayer(string name, Tilemap map) : base(name) {
			Map = map;
		}

		protected override void DoDraw(Renderer r, Camera c) {
			Map.Draw(r, c);
		}

		protected override void DoUpdate() {
		}

		public override LayerType GetLayerType() {
			return LayerType.Tile;
		}

		public override void Save(XmlWriter writer) {
			writer.WriteStartElement("tilelayer");
			writer.WriteAttributeString("name", Name);
			writer.WriteAttributeString("visible", Visible.ToString());
			//Map.Save(writer);
			writer.WriteEndElement();
		}

		public override Rectangle GetBounds() {
			return Map.GetBounds();
		}
	}
}
