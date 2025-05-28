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
public class AssetsModule : ModuleSingle<AssetsModule> {
	/// <summary> 默认路径 </summary>
	public static string DefaultPath => GetDefaultPath();
	/// <summary> 模组路径 </summary>
	public static string ModulePath => $"{DefaultPath}/Modules";

	/// <summary> 模组路径 </summary>
	private static string GetDefaultPath() {
#if UNITY_EDITOR
		string exclude = "/Assets/StreamingAssets";
		string streaming = Application.streamingAssetsPath;
		string root = streaming.Remove(streaming.Length - exclude.Length);
		return $"{root}/Library/com.unity.addressables/aa/Windows/{GetBuildTarget()}";
#else
		return $"{Application.streamingAssetsPath}/aa/{GetBuildTarget()}";
#endif
	}
	/// <summary> 平台路径 </summary>
	private static string GetBuildTarget() {
		// if (Application.platform == RuntimePlatform.WindowsEditor) { return "StandaloneWindows64"; }
		// if (Application.platform == RuntimePlatform.WindowsPlayer) { return "StandaloneWindows64"; }
		return "StandaloneWindows64";
	}

	/// <summary> 模组数据 </summary>
	public List<DataModule> defaults = new List<DataModule>();
	/// <summary> 模组数据 </summary>
	public List<DataModule> modules = new List<DataModule>();

	protected override void Awake() => Replace(false);

	private void Start() => UpdateModuleConfig();

	/// <summary> 更新模组列表 </summary>
	public void UpdateModuleConfig() {
		defaults = new List<DataModule>();
		modules = new List<DataModule>();
		// 获取默认路径下的所有文件夹
		string[] defaultDirectories = Directory.GetDirectories(DefaultPath);
		// 遍历文件夹
		foreach (string directory in defaultDirectories) { ReadDefault(directory); }
		// 获取模组路径下的所有文件夹
		string[] moduleDirectories = Directory.GetDirectories(ModulePath);
		// 遍历模组文件夹
		foreach (string directory in moduleDirectories) { ReadModule(directory); }
		// 加载默认模组
		foreach (DataModule module in defaults) { LoadingModuleConfig(module); }
	}
	/// <summary> 读取模组文件夹 </summary>
	public void ReadDefault(string directory) {
		string targetFile = Path.Combine(directory, "catalog_0.1.json");
		if (!File.Exists(targetFile)) { return; }
		DataModule moduleConfig = new DataModule();
		moduleConfig.name = Path.GetFileName(directory);
		moduleConfig.catalogPath = targetFile;
		defaults.Add(moduleConfig);
	}
	/// <summary> 读取模组文件夹 </summary>
	public void ReadModule(string directory) {
		string targetFile = Path.Combine(directory, "catalog_0.1.json");
		if (!File.Exists(targetFile)) { return; }
		DataModule moduleConfig = new DataModule();
		moduleConfig.name = Path.GetFileName(directory);
		moduleConfig.catalogPath = targetFile;
		modules.Add(moduleConfig);
	}

	/// <summary> 加载模组 </summary>
	public void LoadingModuleConfig(DataModule moduleConfig) {
		StartCoroutine(ILoadingModuleConfig(moduleConfig));
	}
	public IEnumerator ILoadingModuleConfig(DataModule moduleConfig) {
		string filePath = moduleConfig.catalogPath;
		AsyncOperationHandle<IResourceLocator> handle = Addressables.LoadContentCatalogAsync(filePath, false);
		while (!handle.IsDone) { yield return new WaitForEndOfFrame(); }
		if (handle.Status == AsyncOperationStatus.Failed) { Debug.LogError($"无法加载资源目录!({filePath})"); yield break; }
		moduleConfig.locator = handle.Result;
	}

	/// <summary> 卸载模组 </summary>
	public void UnloadModuleConfig(DataModule moduleConfig) {
		Addressables.RemoveResourceLocator(moduleConfig.locator);
	}
}
