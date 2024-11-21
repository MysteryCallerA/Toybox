using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using Toybox.graphic;

namespace Toybox.load {
	public class AsepriteLoader:IAssetLoader<SpriteMap> {

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
			public List<SliceRef> Slices { get; set; }
		}
		private class TagRef {
			public string Name { get; set; }
			public int From { get; set; }
			public int To { get; set; }
		}
		private class SliceRef {
			public string Name { get; set; }
			public List<KeyRef> Keys { get; set; }
		}
		private class KeyRef {
			public RectRef Bounds { get; set; }
		}

		/// <summary> Filepath must include Content.RootDirectory and .xnb file extension. </summary>
		public SpriteMap Load(string filepath) {
			filepath = filepath.Substring(0, filepath.Length - 4);
			string contentName = filepath.Substring(Resources.Content.RootDirectory.Length);
			while (contentName.First() == Path.DirectorySeparatorChar) contentName = contentName.Substring(1);
			var graphic = Resources.Content.Load<Texture2D>(contentName);
			filepath = filepath + ".json";
			return Load(graphic, filepath);
		}

		/// <summary> Filepath must include Content.RootDirectory and .json file extension. </summary>
		public SpriteMap Load(Texture2D graphic, string filepath) {
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

			var origin = Point.Empty;
			if (data.Meta.Slices.Count > 0 && data.Meta.Slices[0].Keys.Count > 0) {
				origin = new Point(data.Meta.Slices[0].Keys[0].Bounds.X, data.Meta.Slices[0].Keys[0].Bounds.Y);
			}

			return new SpriteMap(graphic, frames, animations) { Origin = origin };
		}

		/// <summary> Filepath must include Content.RootDirectory and .xnb file extension. </summary>
		public bool IsLoadable(string filepath) {
			if (!filepath.EndsWith(".xnb")) return false;
			filepath = filepath.Substring(0, filepath.Length - 4);
			if (!File.Exists($@"{filepath}.json")) return false;
			return true;
		}
	}
}
