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
	/// <summary> 当前场景 </summary>
	public static DataScene CurrentScene;



	protected override void Awake() => NoReplace(false);



	/// <summary> 加载场景 </summary>
	public void LoadScene(DataScene dataScene, Action complete = null) {
		StartCoroutine(ILoadScene(dataScene, complete));
	}
	/// <summary> 加载场景 </summary>
	public IEnumerator ILoadScene(DataScene dataScene, Action complete) {
		CurrentScene = dataScene;
		// 检查场景数据
		if (CurrentScene.assetReference == null) { Debug.LogError("无效场景!"); yield break; }
		// 创建加载句柄
		AsyncOperationHandle<SceneInstance> handle = CurrentScene.assetReference.LoadSceneAsync();
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
