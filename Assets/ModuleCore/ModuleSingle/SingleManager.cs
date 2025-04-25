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

	public static DataRunningMode runningMode;// 运行模式
	public static DataSceneConfig sceneConfig;// 场景配置

	// private string Roamhost = "127.0.0.1";
	private string Localhost = "127.0.0.1";
	private string DefaultPort = "5000";

	protected override void Awake() {
		NoReplace();
		OnlineManager.OnCompleteConnection += OnlineManager_OnCompleteConnection;
	}
	private void Start() {
		ModuleUI.Jump(DataPage.Menu);
		ModuleInput.I.Disable();
		ModuleCamera.I.Disable();
		SceneManager.LoadScene("MenuScene");
	}

	private void OnlineManager_OnCompleteConnection() {
		ModuleInput.I.EnablePreview();
		ModuleCamera.I.EnableThirdPerson();
	}

	/// <summary> 设置运行模式 </summary>
	public static void SetRunningMode(DataRunningMode runningMode) {
		SingleManager.runningMode = runningMode;
	}
	/// <summary> 设置场景数据 </summary>
	public static void SetSceneConfig(DataSceneConfig sceneConfig) {
		SingleManager.sceneConfig = sceneConfig;
	}

	/// <summary> 开始游戏 </summary>
	public void StartGame() {
		StartCoroutine(IStartGame());
	}
	/// <summary> 开始游戏 </summary>
	public IEnumerator IStartGame() {
		// 加载场景
		yield return sceneConfig.ILoadScene(null);
		//  启动设置
		SinglePlayer.I.CreateCharacter();
		ModuleUI.Jump(DataPage.Preview);
		ModuleInput.I.EnablePreview();
		ModuleCamera.I.EnableThirdPerson();
	}





#if UNITY_EDITOR
	public static string loadPath {
		get {
			string exclude = "/Assets/StreamingAssets";
			string streaming = Application.streamingAssetsPath;
			string root = streaming.Remove(streaming.Length - exclude.Length);
			return root + "/Library/com.unity.addressables/aa/Windows/Standard01";
		}
	}
#else
    public static string loadPath => Application.streamingAssetsPath + "/aa/Standard01";
#endif

	/// <summary> 服务模式 </summary>
	public void StartServer() {
		AALoading(() => {
			OnlineManager.I.StartServer(Localhost, DefaultPort);
			ModuleUI.Jump(DataPage.None);
			ModuleCamera.I.EnableThirdPerson();
		});
	}
	/// <summary> 主机模式 </summary>
	public void StartHost() {
		AALoading(() => {
			OnlineManager.I.StartHost(Localhost, DefaultPort);
			ModuleUI.Jump(DataPage.Preview);
			ModuleInput.I.EnablePreview();
			ModuleCamera.I.EnableThirdPerson();
		});
	}
	/// <summary> 客户模式 </summary>
	public void StartClient(string address, string port) {
		AALoading(() => {
			OnlineManager.I.StartClient(address, port);
			ModuleUI.Jump(DataPage.Preview);
		});
	}
	/// <summary> 单机模式 </summary>
	public void Standalone() {
		AALoading(() => {
			SinglePlayer.I.CreateCharacter();
			ModuleUI.Jump(DataPage.Preview);
			ModuleInput.I.EnablePreview();
			ModuleCamera.I.EnableThirdPerson();
		});
	}


	/// <summary> 从资源包加载场景 </summary>
	public void AALoading(Action action) {
		AACatalogToScene cts = new AACatalogToScene($"{loadPath}/catalog_0.1.json", "Standard01");
		cts.OnProgress = (value, type) => { Debug.Log($"正在加载:{type} , 进度:{value}"); };
		cts.OnError = (value) => { Debug.LogError(value); };
		cts.OnComplete = () => { Debug.Log("加载完成。。。"); action?.Invoke(); };
		StartCoroutine(cts.ILoad());
	}
}
