using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using MuHua;
using System;

/// <summary>
/// 模组 - 资源管理
/// </summary>
public class AssetsModule : ModuleSingle<AssetsModule> {

	/// <summary> 默认模组数据 </summary>
	public List<DataModule> defaults = new List<DataModule>();
	/// <summary> 扩展模组数据 </summary>
	public List<DataModule> modules = new List<DataModule>();

	protected override void Awake() => Replace(false);

	/// <summary> 加载默认模组列表 </summary>
	public IEnumerator ILoadDefaultModule() {
		defaults.Clear();
		EnsureDirectoryExists(ModuleAssets.DefaultPath);
		// 加载默认模组
		foreach (var directory in Directory.GetDirectories(ModuleAssets.DefaultPath)) {
			var moduleConfig = ReadModule(directory);
			if (moduleConfig != null) { defaults.Add(moduleConfig); }
		}
		foreach (var module in defaults) { yield return ILoadingModuleConfig(module); }
	}

	/// <summary> 加载扩展模组列表 </summary>
	public void LoadExtendModule() {
		modules.Clear();
		EnsureDirectoryExists(ModuleAssets.ModulePath);
		foreach (var directory in Directory.GetDirectories(ModuleAssets.ModulePath)) {
			var moduleConfig = ReadModule(directory);
			if (moduleConfig != null) { modules.Add(moduleConfig); }
		}
	}

	/// <summary> 读取模组文件夹 </summary>
	public DataModule ReadModule(string directory) {
		string catalog = Path.Combine(directory, ModuleAssets.CatalogName);
		if (!File.Exists(catalog)) return null;
		return new DataModule { name = Path.GetFileName(directory), catalogPath = catalog };
	}

	/// <summary> 加载模组 </summary>
	public void LoadingModuleConfig(DataModule moduleConfig) {
		StartCoroutine(ILoadingModuleConfig(moduleConfig));
	}

	/// <summary> 协程：加载模组 </summary>
	public IEnumerator ILoadingModuleConfig(DataModule moduleConfig) {
		string filePath = moduleConfig.catalogPath;
		var handle = Addressables.LoadContentCatalogAsync(filePath, false);
		yield return handle;

		if (handle.Status == AsyncOperationStatus.Failed) {
			Debug.LogError($"无法加载资源目录!({filePath})");
			yield break;
		}
		moduleConfig.locator = handle.Result;
	}

	/// <summary> 卸载模组 </summary>
	public void UnloadModuleConfig(DataModule moduleConfig) {
		if (moduleConfig?.locator != null) { Addressables.RemoveResourceLocator(moduleConfig.locator); }
	}

	/// <summary> 确保目录存在 </summary>
	private void EnsureDirectoryExists(string path) {
		if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
	}
}
