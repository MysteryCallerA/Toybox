using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toybox.load {
	public class AssetManager<T> {

		public Dictionary<string, T> Content = new();
		public IAssetLoader<T> Loader;

		public AssetManager(IAssetLoader<T> loader) {
			Loader = loader;
		}

		public void LoadDirectory(string path) {
			var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
			foreach (var f in files) {
				if (!Loader.IsLoadable(f)) continue;
				var asset = Loader.Load(f);
				var name = Path.GetFileNameWithoutExtension(f);
				Content.Add(name, asset);
			}
		}

		public T this[string name] {
			get {
				return Content[name];
			}
		}

	}
}
