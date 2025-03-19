using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MuHua;

/// <summary>
/// 全局运行管理
/// </summary>
public class SingleManager : ModuleSingle<SingleManager> {

#if UNITY_EDITOR
	public static string loadPath {
		get {
			string exclude = "/Assets/StreamingAssets";
			string streaming = Application.streamingAssetsPath;
			string root = streaming.Remove(streaming.Length - exclude.Length);
			return root + "/Library/com.unity.addressables/aa/Windows/StandaloneWindows64";
		}
	}
#else
    public static string loadPath => Application.streamingAssetsPath + "/aa/StandaloneWindows64";
#endif

	protected override void Awake() => NoReplace();

	private void Start() {
		ModuleUI.Jump(UIPageType.Menu);
		ModuleInput.I.Disable();
		ModuleCamera.I.Disable();
		SceneManager.LoadScene("MenuScene");
	}

	/// <summary> 从资源包加载场景 </summary>
	private IEnumerator AALoading() {
		AACatalogToScene cts = new AACatalogToScene($"{loadPath}/catalog_0.1.json", "Assets/Scenes/SampleScene.unity");
		cts.OnProgress = (value, type) => { Debug.Log($"正在加载:{type} , 进度:{value}"); };
		cts.OnError = (value) => { Debug.LogError(value); };
		cts.OnComplete = () => { Debug.Log("加载完成。。。"); };
		yield return cts.ILoad();
	}
}
