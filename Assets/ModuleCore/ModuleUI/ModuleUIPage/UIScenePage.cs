using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

/// <summary>
/// 场景页面
/// </summary>
public class UIScenePage : ModuleUIPage {
	public VisualTreeAsset SceneTemplate;

	private DataScene dataScene;
	private Action complete;
	private UIScenePanel scenePanel;

	public override VisualElement Element => root.Q<VisualElement>("ScenePage");
	public VisualElement ScrollView => Q<VisualElement>("ScrollView");
	public Button Button1 => Q<Button>("Button1");// 返回
	public Button Button2 => Q<Button>("Button2");// 开始
	public Label SceneLabel => Q<Label>("SceneLabel");// 场景标签

	private void Awake() {
		scenePanel = new UIScenePanel(ScrollView, root, SceneTemplate, SettingsScene);

		Button1.clicked += () => ModuleUI.Settings(EnumPage.Menu);
		Button2.clicked += () => Button2_clicked();

		ModuleUI.OnJumpPage += ModuleUI_OnJumpPage;
	}
	private void OnDestroy() {
		scenePanel.Release();
		ModuleUI.OnJumpPage -= ModuleUI_OnJumpPage;
	}
	private void Update() => scenePanel.Update();

	private void Button2_clicked() {
		if (dataScene == null) { return; }
		ManagerScene.I.LoadScene(dataScene, complete);
	}
	private void ModuleUI_OnJumpPage(EnumPage page) {
		Element.EnableInClassList("document-page-hide", page != EnumPage.Scene);
		if (page != EnumPage.Scene) { return; }
		SettingsScene(null);
		scenePanel.Create();
	}

	/// <summary> 设置单机模式 </summary>
	public void Single() => complete = SingleManager.Single;
	/// <summary> 设置服务模式 </summary>
	public void Server() => complete = SingleManager.Server;
	/// <summary> 设置主机模式 </summary>
	public void Host() => complete = SingleManager.Host;

	/// <summary> 选中的场景配置 </summary>
	public void SettingsScene(DataScene dataScene) {
		this.dataScene = dataScene;
		SceneLabel.text = dataScene != null ? dataScene.name : "???";
	}

}
