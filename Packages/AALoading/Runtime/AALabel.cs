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
	/// 按标签加载可寻址资源
	/// </summary>
	public class AALabel<T> {
		public readonly string label;
		public Action<float> OnProgress;
		public Action<string> OnError;
		public Action<T> OnComplete;
		public T result;

		/// <summary> 按标签加载可寻址资源 </summary>
		public AALabel(string label, Action<T> OnComplete = null) {
			this.label = label;
			this.OnComplete = OnComplete;
		}
		/// <summary> 获取异步句柄 </summary>
		public AsyncOperationHandle<T> Handle() {
			return Addressables.LoadAssetAsync<T>(label);
		}
		/// <summary> 加载资源 </summary>
		public async void Load() => await ALoad();
		/// <summary> 加载资源 </summary>
		public async Task ALoad() {
			AsyncOperationHandle<T> handle = Handle();
			if (handle.Status == AsyncOperationStatus.Failed) {
				OnError?.Invoke($"无法加载资源!(label={label} , type={typeof(T)})"); return;
			}
			while (!handle.IsDone) {
				float downloadProgress = handle.GetDownloadStatus().Percent;
				float loadProgress = handle.PercentComplete;
				float totalProgress = (downloadProgress + loadProgress) / 2.0f;
				// Debug.Log($"下载进度: {downloadProgress * 100}% , 加载进度: {loadProgress * 100}% , 总进度: {totalProgress * 100}%");
				OnProgress?.Invoke(totalProgress);
				await Task.Delay(100);
			}
			OnComplete?.Invoke(handle.Result);
		}
		/// <summary> 加载资源 </summary>
		public IEnumerator ILoad() {
			AsyncOperationHandle<T> handle = Handle();
			if (handle.Status == AsyncOperationStatus.Failed) {
				OnError?.Invoke($"无法加载资源!(label={label} , type={typeof(T)})"); yield break;
			}
			while (!handle.IsDone) {
				float downloadProgress = handle.GetDownloadStatus().Percent;
				float loadProgress = handle.PercentComplete;
				float totalProgress = (downloadProgress + loadProgress) / 2.0f;
				OnProgress?.Invoke(totalProgress);
				yield return new WaitForEndOfFrame();
			}
			result = handle.Result;
			OnComplete?.Invoke(handle.Result);
		}
	}
}