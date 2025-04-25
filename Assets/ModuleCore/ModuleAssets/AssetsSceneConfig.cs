using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using MuHua;

/// <summary>
/// 场景资源管理
/// </summary>
public class AssetsSceneConfig : ModuleSingle<AssetsSceneConfig> {

	public static event Action OnChange;

	public const string SceneConfigTag = "default";// aa查找ConstSceneConfig的标签

	public List<ConstSceneConfig> sceneConfigs;

	public static List<ConstSceneConfig> Datas => I.sceneConfigs;

	protected override void Awake() => Replace(false);

	/// <summary> 更新场景列表 </summary>
	public void UpdateSceneConfig() {
		sceneConfigs = new List<ConstSceneConfig>();
		Addressables.LoadAssetsAsync<ConstSceneConfig>(SceneConfigTag, UpdateSceneConfig, true);
	}
	public void UpdateSceneConfig(ConstSceneConfig sceneConfig) {
		sceneConfigs.Add(sceneConfig);
		OnChange?.Invoke();
	}
}
