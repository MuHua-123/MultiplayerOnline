using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;
using MuHua;

/// <summary>
/// 版本 - 管理器
/// </summary>
public class ManagerVersion : ModuleSingle<ManagerVersion> {

	protected override void Awake() => NoReplace(false);

	public DataGameVersion VersionInfo() {
		DataGameVersion gameVersion = new DataGameVersion();
		// 收集默认模组版本信息
		foreach (var module in AssetsModule.I.defaults) {
			gameVersion.defaults.Add(new DataModuleVersion { name = module.name, version = module.version });
		}
		// 收集扩展模组版本信息
		foreach (var module in AssetsModule.I.extends) {
			gameVersion.extends.Add(new DataModuleVersion { name = module.name, version = module.version });
		}
		return gameVersion;
	}

	#region 模组控制
	/// <summary> 加载模组 </summary>
	public void LoadModule(DataModule moduleConfig) {
		StartCoroutine(ILoadModule(moduleConfig));
	}
	/// <summary> 协程：加载模组 </summary>
	public IEnumerator ILoadModule(DataModule moduleConfig) {
		string filePath = moduleConfig.catalogPath;
		AsyncOperationHandle<IResourceLocator> handle = Addressables.LoadContentCatalogAsync(filePath, false);
		yield return handle;
		if (handle.Status == AsyncOperationStatus.Failed) {
			Debug.LogError($"无法加载资源目录!({filePath})");
			yield break;
		}
		moduleConfig.locator = handle.Result;
	}
	/// <summary> 卸载模组 </summary>
	public void UnloadModule(DataModule moduleConfig) {
		if (moduleConfig?.locator != null) { Addressables.RemoveResourceLocator(moduleConfig.locator); }
	}
	#endregion
}

