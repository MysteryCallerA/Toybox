using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Toybox.scenes.world;

namespace Toybox.tiled {
	public class WorldFile {

		public TiledWorldData Data;

		public WorldFile(string contentRoot, string file) {
			file = Path.Combine(contentRoot, file);

			if (!File.Exists(file)) {
				throw new Exception("File not found. Path:" + file);
			}

			string json = File.ReadAllText(file);
			if (file.EndsWith(".world")) {
				ParseJson(json);
				return;
			}

			throw new Exception("Unsupported file format");
		}

		private void ParseJson(string json) {
			Data = JsonSerializer.Deserialize<TiledWorldData>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
		}

		public T GetWorldLayout<T>() where T : WorldLayout, new() {
			T layout = new T();
			foreach (var map in Data.Maps) {
				layout.AddData(map.FileName, new Rectangle(map.X, map.Y, map.Width, map.Height));
			}
			return layout;
		}

	}

	public class TiledWorldData {
		public List<TiledMapRef> Maps { get; set; }
	}

	public class TiledMapRef {
		public string FileName { get; set; }
		public int Height { get; set; }
		public int Width { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
	}
}
