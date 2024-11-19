using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using Toybox.graphic;

namespace Toybox.load {
	public static class AsepriteData {

		private class RootRef {
			public Dictionary<string, FrameRef> Frames { get; set; }
			public MetaRef Meta { get; set; }
		}
		private class FrameRef {
			public RectRef Frame { get; set; }
			public int Duration { get; set; }
		}
		private class RectRef {
			public int X { get; set; }
			public int Y { get; set; }
			public int W { get; set; }
			public int H { get; set; }
		}
		private class MetaRef {
			public List<TagRef> FrameTags { get; set; }
		}
		private class TagRef {
			public string Name { get; set; }
			public int From { get; set; }
			public int To { get; set; }
		}

		public static SpriteMap Load(string graphicName) {
			var graphic = Resources.Content.Load<Texture2D>(graphicName);
			var path = Resources.Content.RootDirectory + graphicName + ".json";
			return Load(graphic, path);
		}

		public static SpriteMap Load(Texture2D graphic, string filepath) {
			string json = File.ReadAllText(filepath);
			var data = JsonSerializer.Deserialize<RootRef>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

			var frames = new List<Rectangle>();
			var frametimes = new List<int>();
			foreach (var f in data.Frames.Values) {
				var rect = new Rectangle(f.Frame.X, f.Frame.Y, f.Frame.W, f.Frame.H);
				for (int i = 0; i <= frames.Count; i++) {
					if (i == frames.Count) {
						frames.Add(rect);
						frametimes.Add(f.Duration);
						break;
					} else if ((rect.X < frames[i].X && rect.Y == frames[i].Y) || rect.Y < frames[i].Y) {
						frames.Insert(i, rect);
						frametimes.Insert(i, f.Duration);
						break;
					}
				}
			}

			var animations = new Dictionary<string, Animation>();
			foreach (var tag in data.Meta.FrameTags) {
				var aframes = new int[tag.To - tag.From + 1];
				var aframetime = new int[aframes.Length];
				for (int i = 0; i < aframes.Length; i++) {
					aframes[i] = i + tag.From;
					aframetime[i] = frametimes[aframes[i]];
				}
				animations.Add(tag.Name, new Animation(aframes, aframetime) { Name = tag.Name });
			}

			return new SpriteMap(graphic, frames, animations);
		}

	}
}
