using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using MuHua;

/// <summary>
/// 场景管理器
/// </summary>
public class SingleScene : ModuleSingle<SingleScene> {

	public DataSceneConfig sceneConfig;

	protected override void Awake() => NoReplace(false);

	/// <summary> 设置场景数据 </summary>
	public static void SetSceneData(DataSceneConfig sceneConfig) {
		I.sceneConfig = sceneConfig;
	}
	/// <summary> 加载场景 </summary>
	public static void LoadScene() {
		I.StartCoroutine(I.ILoadScene());
	}

	/// <summary> 加载场景 </summary>
	public IEnumerator ILoadScene() {
		// 检查场景数据
		if (sceneConfig == null || sceneConfig.scene == null) { Debug.LogError("无效场景!"); yield break; }
		// 创建加载句柄
		AsyncOperationHandle<SceneInstance> handle = sceneConfig.scene.LoadSceneAsync();
		// 协程加载
		while (!handle.IsDone) {
			if (handle.Status == AsyncOperationStatus.Failed) { Debug.LogError("无法加载场景!"); yield break; }
			float downloadProgress = handle.GetDownloadStatus().Percent;
			float loadProgress = handle.PercentComplete;
			float totalProgress = (downloadProgress + loadProgress) / 2.0f;
			Debug.Log(totalProgress);
			yield return handle;
		}
		//加载结束
	}
}
