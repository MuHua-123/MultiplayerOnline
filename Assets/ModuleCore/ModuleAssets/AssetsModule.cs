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

	/// <summary> 默认路径 </summary>
	public static string DefaultPath => GetDefaultPath();
	/// <summary> 模组路径 </summary>
	public static string ExtendPath => $"{DefaultPath}/Modules";

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

	/// <summary> 默认模组数据 </summary>
	public List<DataModule> defaults = new List<DataModule>();
	/// <summary> 扩展模组数据 </summary>
	public List<DataModule> extends = new List<DataModule>();

	protected override void Awake() => Replace(false);

	/// <summary> 加载默认模组列表 </summary>
	public IEnumerator ILoadDefaultModule() {
		defaults.Clear();
		EnsureDirectoryExists(DefaultPath);
		// 加载默认模组
		foreach (var directory in Directory.GetDirectories(DefaultPath)) {
			var moduleConfig = ReadModule(directory);
			if (moduleConfig != null) { defaults.Add(moduleConfig); }
		}
		foreach (var module in defaults) { yield return ManagerVersion.I.ILoadModule(module); }
		// 加载扩展模块列表
		LoadExtendModule();
	}
	/// <summary> 加载扩展模组列表 </summary>
	public void LoadExtendModule() {
		extends.Clear();
		EnsureDirectoryExists(ExtendPath);
		foreach (var directory in Directory.GetDirectories(ExtendPath)) {
			var moduleConfig = ReadModule(directory);
			if (moduleConfig != null) { extends.Add(moduleConfig); }
		}
	}
	/// <summary> 读取模组文件夹 </summary>
	public DataModule ReadModule(string directory) {
		// 查询所有 catalog_*.json 文件
		var files = Directory.GetFiles(directory, "catalog_*.json", SearchOption.TopDirectoryOnly);
		if (files.Length == 0) return null;
		// 取第一个匹配的文件
		string catalog = files[0];
		// 获取 * 的内容
		string version = Path.GetFileNameWithoutExtension(catalog).Replace("catalog_", string.Empty);
		// 创建模组信息
		return new DataModule { name = Path.GetFileName(directory), catalogPath = catalog, version = version };
	}
	/// <summary> 确保目录存在 </summary>
	private void EnsureDirectoryExists(string path) {
		if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
	}
}
/// <summary>
/// 模组数据
/// </summary>
public class DataModule {
	/// <summary> 是否启用 </summary>
	public bool isEnable;
	/// <summary> 模组名字 </summary>
	public string name;
	/// <summary> 模组版本 </summary>
	public string version;
	/// <summary> 目录路径 </summary>
	public string catalogPath;
	/// <summary> 资源定位器 </summary>
	public IResourceLocator locator;
}