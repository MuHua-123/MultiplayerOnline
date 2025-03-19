using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace MuHua {
	/// <summary>
	/// 从目录加载场景
	/// </summary>
	public class AACatalogToScene {
		public enum Progress { Catalog, Label, Scene }
		public readonly string filePath;
		public readonly string sceneName;
		public readonly LoadSceneMode loadSceneMode;
		public readonly bool activateOnLoad;
		public Action<float, Progress> OnProgress;
		public Action<string> OnError;
		public Action OnComplete;

		/// <summary> 加载可寻址资源目录 </summary>
		public AACatalogToScene(string filePath, string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single, bool activateOnLoad = true) {
			this.filePath = filePath;
			this.sceneName = sceneName;
			this.loadSceneMode = loadSceneMode;
			this.activateOnLoad = activateOnLoad;
		}
		/// <summary> 从目录加载场景 </summary>
		public async void Load() => await ALoad();
		/// <summary> 从目录加载场景 </summary>
		public async Task ALoad() {
			AACatalog catalog = new AACatalog(filePath);
			catalog.OnProgress = (value) => { OnProgress?.Invoke(value, Progress.Catalog); };
			catalog.OnError = OnError;
			await catalog.ALoad();
			AAScene aaScene = new AAScene(sceneName, loadSceneMode, activateOnLoad);
			aaScene.OnProgress = (value) => { OnProgress?.Invoke(value, Progress.Scene); };
			aaScene.OnError = OnError;
			aaScene.OnComplete = OnComplete;
			await aaScene.ALoad();
		}
		/// <summary> 从目录加载场景 </summary>
		public IEnumerator ILoad() {
			AACatalog catalog = new AACatalog(filePath);
			catalog.OnProgress = (value) => { OnProgress?.Invoke(value, Progress.Catalog); };
			catalog.OnError = OnError;
			yield return catalog.ILoad();
			AAScene aaScene = new AAScene(sceneName, loadSceneMode, activateOnLoad);
			aaScene.OnProgress = (value) => { OnProgress?.Invoke(value, Progress.Scene); };
			aaScene.OnError = OnError;
			aaScene.OnComplete = OnComplete;
			yield return aaScene.ILoad();
		}
	}
}