using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace MuHua {
	/// <summary>
	/// 加载可寻址场景
	/// </summary>
	public class AAScene {
		public readonly string name;
		public readonly LoadSceneMode loadSceneMode;
		public readonly bool activateOnLoad;
		public Action<float> OnProgress;
		public Action<string> OnError;
		public Action OnComplete;

		/// <summary> 加载可寻址场景 </summary>
		public AAScene(string name, LoadSceneMode loadSceneMode = LoadSceneMode.Single, bool activateOnLoad = true) {
			this.name = name;
			this.loadSceneMode = loadSceneMode;
			this.activateOnLoad = activateOnLoad;
		}
		/// <summary> 获取异步句柄 </summary>
		public AsyncOperationHandle<SceneInstance> Handle() {
			return Addressables.LoadSceneAsync(name, loadSceneMode, activateOnLoad);
		}
		/// <summary> 加载场景 </summary>
		public async void Load() => await ALoad();
		/// <summary> 加载场景 </summary>
		public async Task ALoad() {
			AsyncOperationHandle<SceneInstance> handle = Handle();
			if (handle.Status == AsyncOperationStatus.Failed) {
				OnError?.Invoke($"无法加载场景!({name})"); return;
			}
			while (!handle.IsDone) {
				float downloadProgress = handle.GetDownloadStatus().Percent;
				float loadProgress = handle.PercentComplete;
				float totalProgress = (downloadProgress + loadProgress) / 2.0f;
				// Debug.Log($"下载进度: {downloadProgress * 100}% , 加载进度: {loadProgress * 100}% , 总进度: {totalProgress * 100}%");
				OnProgress?.Invoke(totalProgress);
				await Task.Delay(100);
			}
			OnComplete?.Invoke();
		}
		/// <summary> 加载场景 </summary>
		public IEnumerator ILoad() {
			AsyncOperationHandle<SceneInstance> handle = Handle();
			if (handle.Status == AsyncOperationStatus.Failed) {
				OnError?.Invoke($"无法加载场景!({name})"); yield break;
			}
			while (!handle.IsDone) {
				float downloadProgress = handle.GetDownloadStatus().Percent;
				float loadProgress = handle.PercentComplete;
				float totalProgress = (downloadProgress + loadProgress) / 2.0f;
				OnProgress?.Invoke(totalProgress);
				yield return new WaitForEndOfFrame();
			}
			OnComplete?.Invoke();
		}
	}
}
