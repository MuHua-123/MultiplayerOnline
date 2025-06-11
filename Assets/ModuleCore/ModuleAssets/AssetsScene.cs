using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using MuHua;

/// <summary>
/// 场景 - 资源管理
/// </summary>
public class AssetsScene : ModuleSingle<AssetsScene> {

	/// <summary> aa查找标签 </summary>
	public const string DefaultTag = "default";
	/// <summary> aa查找标签 </summary>
	public const string ExtendTag = "extend";

	/// <summary> 菜单场景 </summary>
	public static AssetReference MenuScene;

	/// <summary> 默认场景 </summary>
	public List<DataScene> defaultScenes = new List<DataScene>();
	/// <summary> 扩展场景 </summary>
	public List<DataScene> extendScenes = new List<DataScene>();

	protected override void Awake() => NoReplace(false);

	/// <summary> 加载默认场景 </summary>
	public IEnumerator ILoadDefaultScene() {
		yield return ILoadScenes(DefaultTag, defaultScenes, null);
		for (int i = 0; i < defaultScenes.Count; i++) {
			DataScene dataScene = defaultScenes[i];
			if (dataScene.name == "MenuScene") { MenuScene = dataScene.scene; }
		}
	}

	/// <summary> 加载扩展场景 </summary>
	public void LoadExtendScene(Action callback) =>
		StartCoroutine(ILoadScenes(ExtendTag, extendScenes, callback));

	/// <summary>
	/// 通用协程：加载场景
	/// </summary>
	private IEnumerator ILoadScenes(string tag, List<DataScene> targetList, Action callback) {
		targetList.Clear();
		var handle = Addressables.LoadAssetsAsync<ConstScene>(tag, obj => {
			if (obj?.configs != null) { targetList.AddRange(obj.configs); }
		}, true);

		yield return handle;

		if (handle.Status == AsyncOperationStatus.Failed) {
			Debug.LogError($"Failed to load {tag} scene configuration!");
			yield break;
		}

		Debug.Log($"{tag} scene configuration loading completed!");
		callback?.Invoke();
	}
}
