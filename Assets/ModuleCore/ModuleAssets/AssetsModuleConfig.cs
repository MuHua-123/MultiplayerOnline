using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using MuHua;

/// <summary>
/// 模组资源管理
/// </summary>
public class AssetsModuleConfig : ModuleSingle<AssetsModuleConfig> {

	public List<DataModuleConfig> moduleConfigs = new List<DataModuleConfig>();

	public static List<DataModuleConfig> Datas => I.moduleConfigs;

	protected override void Awake() => Replace(false);

	private void Start() => UpdateModuleConfig();

	/// <summary> 更新模组列表 </summary>
	public void UpdateModuleConfig() {
		// 获取路径下的所有文件夹
		string[] directories = Directory.GetDirectories(FilePath.ModulePath);
		// 遍历模组文件夹
		foreach (string directory in directories) { ReadModuleConfig(directory); }
	}
	/// <summary> 读取模组文件夹 </summary>
	public void ReadModuleConfig(string directory) {
		string targetFile = Path.Combine(directory, "catalog_0.1.json");
		if (!File.Exists(targetFile)) { return; }
		DataModuleConfig moduleConfig = new DataModuleConfig();
		moduleConfig.name = Path.GetFileName(directory);
		moduleConfig.catalogPath = targetFile;
		moduleConfigs.Add(moduleConfig);
	}

	/// <summary> 加载模组 </summary>
	public void LoadingModuleConfig(DataModuleConfig moduleConfig) {
		StartCoroutine(ILoadingModuleConfig(moduleConfig));
	}
	public IEnumerator ILoadingModuleConfig(DataModuleConfig moduleConfig) {
		string filePath = moduleConfig.catalogPath;
		AsyncOperationHandle<IResourceLocator> handle = Addressables.LoadContentCatalogAsync(filePath, false);
		while (!handle.IsDone) { yield return new WaitForEndOfFrame(); }
		if (handle.Status == AsyncOperationStatus.Failed) { Debug.LogError($"无法加载资源目录!({filePath})"); yield break; }
		moduleConfig.locator = handle.Result;
	}

	/// <summary> 卸载模组 </summary>
	public void UnloadModuleConfig(DataModuleConfig moduleConfig) {
		Addressables.RemoveResourceLocator(moduleConfig.locator);
	}
}
