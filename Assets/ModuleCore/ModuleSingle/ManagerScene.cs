using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using MuHua;

/// <summary>
/// 场景 - 管理器
/// </summary>
public class ManagerScene : ModuleSingle<ManagerScene> {

	protected override void Awake() => NoReplace(false);

	/// <summary> 加载场景 </summary>
	public void LoadScene(AssetReference reference, Action complete = null) {
		StartCoroutine(ILoadScene(reference, complete));
	}
	/// <summary> 加载场景 </summary>
	public IEnumerator ILoadScene(AssetReference reference, Action complete) {
		// 检查场景数据
		if (reference == null) { Debug.LogError("无效场景!"); yield break; }
		// 创建加载句柄
		AsyncOperationHandle<SceneInstance> handle = reference.LoadSceneAsync();
		// 协程加载
		while (!handle.IsDone) { yield return IHandleProgress(handle); }
		//加载结束
		complete?.Invoke();
	}
	/// <summary> 处理进度 </summary>
	private IEnumerator IHandleProgress(AsyncOperationHandle<SceneInstance> handle) {
		float downloadProgress = handle.GetDownloadStatus().Percent;
		float loadProgress = handle.PercentComplete;
		float totalProgress = (downloadProgress + loadProgress) / 2.0f;
		// progress?.Invoke(totalProgress);
		if (handle.Status == AsyncOperationStatus.Failed) { Debug.LogError("无法加载场景!"); }
		yield return handle;
	}
}
