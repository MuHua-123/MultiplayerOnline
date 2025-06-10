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

	protected override void Awake() {
		NoReplace();
		OnlineManager.OnCompleteConnection += OnlineManager_OnCompleteConnection;
	}
	private IEnumerator Start() {
		// 加载默认模块
		yield return AssetsModule.I.ILoadDefaultModule();
		// 加载默认场景数据
		yield return AssetsScene.I.ILoadDefaultScene();
		// 加载菜单场景
		yield return ManagerScene.I.ILoadScene(AssetsScene.MenuScene, Initial);
		// 加载扩展模块列表
		AssetsModule.I.LoadExtendModule();
	}

	private void OnlineManager_OnCompleteConnection() {
		ModuleInput.Mode(EnumInputMode.ThirdPerson);
		ModuleCamera.Mode(EnumCameraMode.ThirdPerson);
	}

	/// <summary> 初始模式 </summary>
	public static void Initial() {
		ModuleUI.Jump(EnumPage.Menu);
		ModuleInput.Mode(EnumInputMode.None);
		ModuleCamera.Mode(EnumCameraMode.None);
	}
	/// <summary> 单机模式 </summary>
	public static void Single() {
		ManagerPlayer.I.CreateCharacter();
		ModuleUI.Jump(EnumPage.Preview);
		ModuleInput.Mode(EnumInputMode.ThirdPerson);
		ModuleCamera.Mode(EnumCameraMode.ThirdPerson);
	}
	/// <summary> 服务模式 </summary>
	public static void Server() {

	}
	/// <summary> 客户模式 </summary>
	public static void Client() {

	}
	/// <summary> 主机模式 </summary>
	public static void Host() {

	}
}
