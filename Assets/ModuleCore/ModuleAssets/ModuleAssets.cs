using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源模块
/// </summary>
public static class ModuleAssets {
	/// <summary> 默认路径 </summary>
	public static string DefaultPath => GetDefaultPath();
	/// <summary> 模组路径 </summary>
	public static string ModulePath => $"{DefaultPath}/Modules";
	/// <summary> 目录名称 </summary>
	public static string CatalogName => "catalog_0.1.json";

	/// <summary> aa查找标签 </summary>
	public const string DefaultTag = "default";
	/// <summary> aa查找标签 </summary>
	public const string ExtendTag = "extend";


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
}
