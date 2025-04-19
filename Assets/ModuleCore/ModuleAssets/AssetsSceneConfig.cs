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

	public List<DataSceneConfig> sceneConfigs;

	public static List<DataSceneConfig> Datas => I.sceneConfigs;

	protected override void Awake() => Replace(false);

	/// <summary> 更新场景列表 </summary>
	public void UpdateSceneConfig() {
		sceneConfigs = new List<DataSceneConfig>();
		var handle = Addressables.LoadAssetsAsync<ConstSceneConfig>("default", UpdateSceneConfig, false);
		handle.Completed += (operation) => {
			if (operation.Status == AsyncOperationStatus.Failed) {
				Debug.LogError($"加载场景配置时发生错误: {operation.OperationException?.Message}\n{operation.OperationException?.StackTrace}");
			}
		};
	}
	public void UpdateSceneConfig(ConstSceneConfig sceneConfig) {
		sceneConfigs.AddRange(sceneConfig.configs);
		OnChange?.Invoke();
	}
}
