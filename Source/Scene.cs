using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Utils.save;

namespace Toybox {
	public class Scene:IXmlSaveable {

		public List<SceneLayer> Layers = new List<SceneLayer>();
		public bool KeepSceneInMemory = false;
		public string Name;

		public Scene() {
		}

		public Scene(List<SceneLayer> layers) {
			Layers = layers;
		}

		public virtual void Update() {
			foreach (var layer in Layers) {
				layer.Update();
			}
		}

		public virtual void Draw(SpriteBatch s, Camera c) {
			foreach (var layer in Layers) {
				layer.Draw(s, c);
			}
		}

		public virtual void SetLayerOrder(List<string> order) {
			var output = new List<SceneLayer>();
			var currentPos = new Dictionary<string, int>();
			for (int i = 0; i < Layers.Count; i++) {
				currentPos.Add(Layers[i].Name, i);
			}

			foreach (string name in order) {
				var layer = Layers[currentPos[name]];
				output.Add(layer);
			}
			Layers = output;
		}

		public virtual SceneLayer GetLayer(string name) {
			foreach (var l in Layers) {
				if (l.Name == name) return l;
			}
			return null;
		}

		public virtual void AddLayer(SceneLayer layer) {
			Layers.Add(layer);
		}

		public virtual void RemoveLayer(string name) {
			for (int i = 0; i < Layers.Count; i++) {
				if (Layers[i].Name == name) {
					Layers[i].Dispose();
					Layers.RemoveAt(i);
					return;
				}
			}
		}

		public void Save(XmlWriter writer) {
			foreach (var layer in Layers) {
				layer.Save(writer);
			}
		}

		public Rectangle GetBounds() {
			var output = Rectangle.Empty;
			foreach (var layer in Layers) {
				var b = layer.GetBounds();
				if (b == Rectangle.Empty) continue;
				output = Rectangle.Union(output, b);
			}
			return output;
		}
	}
}
