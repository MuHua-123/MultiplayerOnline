using System;
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

	protected override void Awake() {
		NoReplace();
		OnlineManager.OnCompleteConnection += OnlineManager_OnCompleteConnection;
	}

	private void OnlineManager_OnCompleteConnection() {
		ModuleInput.I.EnablePreview();
		ModuleCamera.I.EnableThirdPerson();
	}

	private void Start() {
		ModuleUI.Jump(UIPageType.Menu);
		ModuleInput.I.Disable();
		ModuleCamera.I.Disable();
		SceneManager.LoadScene("MenuScene");
	}

	/// <summary> 服务模式 </summary>
	public void StartServer() {
		AALoading(() => {
			OnlineManager.I.StartServer();
			ModuleUI.Jump(UIPageType.None);
			ModuleCamera.I.EnableThirdPerson();
		});
	}
	/// <summary> 主机模式 </summary>
	public void StartHost() {
		AALoading(() => {
			OnlineManager.I.StartHost();
			ModuleUI.Jump(UIPageType.Preview);
			ModuleInput.I.EnablePreview();
			ModuleCamera.I.EnableThirdPerson();
		});
	}
	/// <summary> 客户模式 </summary>
	public void StartClient(string address, string port) {
		AALoading(() => {
			OnlineManager.I.StartClient(address, port);
			ModuleUI.Jump(UIPageType.Preview);
		});
	}
	/// <summary> 单机模式 </summary>
	public void Standalone() {
		AALoading(() => {
			SinglePlayer.I.CreateCharacter();
			ModuleUI.Jump(UIPageType.Preview);
			ModuleInput.I.EnablePreview();
			ModuleCamera.I.EnableThirdPerson();
		});
	}


	/// <summary> 从资源包加载场景 </summary>
	public void AALoading(Action action) {
		AACatalogToScene cts = new AACatalogToScene($"{loadPath}/catalog_0.1.json", "Assets/Scenes/SampleScene.unity");
		cts.OnProgress = (value, type) => { Debug.Log($"正在加载:{type} , 进度:{value}"); };
		cts.OnError = (value) => { Debug.LogError(value); };
		cts.OnComplete = () => { Debug.Log("加载完成。。。"); action?.Invoke(); };
		StartCoroutine(cts.ILoad());
	}
}
