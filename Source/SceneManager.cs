using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toybox.scenes;

namespace Toybox
{

    public abstract class SceneManager<T> where T : Scene {

		public bool AlwaysKeepScenesInMemory = false;

		public Dictionary<string, T> LoadedScenes = new Dictionary<string, T>();
		public T ActiveScene;

		public SceneManager() {

		}

		public void Update() {
			ActiveScene.Update();
		}

		public void Draw(Renderer r, Camera c) {
			ActiveScene.Draw(r, c);
		}

		public abstract T Load(string sceneName);

		public virtual void Switch(string sceneName) {
			UnloadActiveScene();

			if (LoadedScenes.ContainsKey(sceneName)) {
				ActiveScene = LoadedScenes[sceneName];
				return;
			}

			ActiveScene = Load(sceneName);
			ActiveScene.Name = sceneName;
		}

		protected virtual void UnloadActiveScene() {
			if (ActiveScene == null) return;

			if (ActiveScene.KeepSceneInMemory || AlwaysKeepScenesInMemory) {
				LoadedScenes.Add(ActiveScene.Name, ActiveScene);
				return;
			}

			//TODO should mabye consider disposing scenes and their content?
		}

	}
}
