using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 文件路径
/// </summary>
public static class FilePath {
	/// <summary> 平台路径 </summary>
	public static string BuildTarget => GetBuildTarget();
	/// <summary> 模组路径 </summary>
	public static string ModulePath => GetModulePath();

	/// <summary> 平台路径 </summary>
	private static string GetBuildTarget() {
		// if (Application.platform == RuntimePlatform.WindowsEditor) { return "StandaloneWindows64"; }
		// if (Application.platform == RuntimePlatform.WindowsPlayer) { return "StandaloneWindows64"; }
		return "StandaloneWindows64";
	}
	/// <summary> 模组路径 </summary>
	private static string GetModulePath() {
#if UNITY_EDITOR
		string exclude = "/Assets/StreamingAssets";
		string streaming = Application.streamingAssetsPath;
		string root = streaming.Remove(streaming.Length - exclude.Length);
		return $"{root}/Library/com.unity.addressables/aa/Windows/{BuildTarget}";
#else
		return $"{Application.streamingAssetsPath}/aa/{BuildTarget}";
#endif
	}
}
