using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MuHua {
	/// <summary>
	/// 加载可寻址资源目录
	/// </summary>
	public class AACatalog {
		public readonly string filePath;
		public Action<float> OnProgress;
		public Action<string> OnError;
		public Action OnComplete;

		/// <summary> 加载可寻址资源目录 </summary>
		public AACatalog(string filePath, Action OnComplete = null) {
			this.filePath = filePath;
			this.OnComplete = OnComplete;
		}
		/// <summary> 获取异步句柄 </summary>
		public AsyncOperationHandle<IResourceLocator> Handle() {
			return Addressables.LoadContentCatalogAsync(filePath, true);
		}
		/// <summary> 加载目录 </summary>
		public async void Load() => await ALoad();
		/// <summary> 加载目录 </summary>
		public async Task ALoad() {
			AsyncOperationHandle<IResourceLocator> handle = Handle();
			if (handle.Status == AsyncOperationStatus.Failed) {
				OnError?.Invoke($"无法加载资源目录!({filePath})"); return;
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
		/// <summary> 加载目录 </summary>
		public IEnumerator ILoad() {
			AsyncOperationHandle<IResourceLocator> handle = Handle();
			if (handle.Status == AsyncOperationStatus.Failed) {
				OnError?.Invoke($"无法加载资源目录!({filePath})");
				yield break;
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