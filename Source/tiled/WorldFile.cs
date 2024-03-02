using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
