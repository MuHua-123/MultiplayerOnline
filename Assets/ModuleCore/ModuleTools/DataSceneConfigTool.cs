using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

/// <summary>
/// DataSceneConfig扩展工具
/// </summary>
public static class DataSceneConfigTool {
	/// <summary> 加载场景 </summary>
	public static IEnumerator ILoadScene(this DataSceneConfig sceneConfig, Action<float> progress) {
		// 检查场景数据
		if (sceneConfig == null || sceneConfig.scene == null) { Debug.LogError("无效场景!"); yield break; }
		// 创建加载句柄
		AsyncOperationHandle<SceneInstance> handle = sceneConfig.scene.LoadSceneAsync();
		// 协程加载
		while (!handle.IsDone) { yield return IHandleProgress(handle, progress); }
		//加载结束
	}
	/// <summary> 处理进度 </summary>
	public static IEnumerator IHandleProgress(AsyncOperationHandle<SceneInstance> handle, Action<float> progress) {
		float downloadProgress = handle.GetDownloadStatus().Percent;
		float loadProgress = handle.PercentComplete;
		float totalProgress = (downloadProgress + loadProgress) / 2.0f;
		progress?.Invoke(totalProgress);
		if (handle.Status == AsyncOperationStatus.Failed) { Debug.LogError("无法加载场景!"); }
		yield return handle;
	}
}
