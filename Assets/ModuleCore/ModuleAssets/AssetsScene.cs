using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using MuHua;

/// <summary>
/// 场景资源管理
/// </summary>
public class AssetsScene : ModuleSingle<AssetsScene> {
	/// <summary> 数据列表更改事件 </summary>
	public static event Action OnChangeConfig;
	/// <summary> aa查找ConstSceneConfig的标签 </summary>
	public const string SceneConfigTag = "default";// 

	/// <summary> 当前场景数据 </summary>
	private DataScene dataScene;
	/// <summary> 场景数据列表 </summary>
	public List<DataScene> dataScenes;

	/// <summary> 场景是否有效 </summary>
	public bool isValid => dataScene != null && dataScene.scene != null;

	protected override void Awake() => NoReplace(false);

	/// <summary> 更新场景列表 </summary>
	public void UpdateSceneConfig() {
		dataScenes = new List<DataScene>();
		Addressables.LoadAssetsAsync<ConstScene>(SceneConfigTag, UpdateSceneConfig, true);
	}
	public void UpdateSceneConfig(ConstScene sceneConfig) {
		dataScenes.AddRange(sceneConfig.configs);
		OnChangeConfig?.Invoke();
	}
	/// <summary> 设置场景 </summary>
	public void Settings(DataScene dataScene) => this.dataScene = dataScene;
	/// <summary> 加载场景 </summary>
	public void LoadScene(Action complete = null, Action<float> progress = null) => StartCoroutine(ILoadScene(complete, progress));

	/// <summary> 加载场景 </summary>
	private IEnumerator ILoadScene(Action complete, Action<float> progress) {
		// 检查场景数据
		if (dataScene == null || dataScene.scene == null) { Debug.LogError("无效场景!"); yield break; }
		// 创建加载句柄
		AsyncOperationHandle<SceneInstance> handle = dataScene.scene.LoadSceneAsync();
		// 协程加载
		while (!handle.IsDone) { yield return IHandleProgress(handle, progress); }
		//加载结束
		complete?.Invoke();
	}
	/// <summary> 处理进度 </summary>
	private IEnumerator IHandleProgress(AsyncOperationHandle<SceneInstance> handle, Action<float> progress) {
		float downloadProgress = handle.GetDownloadStatus().Percent;
		float loadProgress = handle.PercentComplete;
		float totalProgress = (downloadProgress + loadProgress) / 2.0f;
		progress?.Invoke(totalProgress);
		if (handle.Status == AsyncOperationStatus.Failed) { Debug.LogError("无法加载场景!"); }
		yield return handle;
	}
}
