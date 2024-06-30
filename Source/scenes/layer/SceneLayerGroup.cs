using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.scenes.layer {
	public class SceneLayerGroup<T>:IEnumerable<T> where T : SceneLayer {

		public List<T> Content = new();
		public T MainLayer;

		public SceneLayerGroup() {

		}

		public T this[int i] {
			get { return Content[i]; }
		}

		public T this[string name] {
			get { for (int i = 0; i < Content.Count; i++) if (Content[i].Name == name) return Content[i];
				return null;
			}
		}

		public void Init() {
			foreach (var layer in Content) {
				layer.Init();
			}
		}

		public void Draw(Renderer r, Camera c) {
			foreach (var layer in Content) {
				if (!layer.Visible) continue;
				layer.Draw(r, c);
			}
		}

		public void Update() {
			foreach (var layer in Content) {
				if (!layer.Active) continue;
				layer.Update();
			}
		}

		public void Switch(string layerName) {
			foreach (var layer in Content) {
				if (layer.Name != layerName) continue;
				if (MainLayer != null) {
					MainLayer.Visible = false;
					MainLayer.Active = false;
				}
				MainLayer = layer;
				MainLayer.Visible = true;
				MainLayer.Active = true;
			}
		}

		public void Add(T layer) {
			Content.Add(layer);
			MainLayer ??= layer;
		}

		public IEnumerator<T> GetEnumerator() {
			return Content.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}
