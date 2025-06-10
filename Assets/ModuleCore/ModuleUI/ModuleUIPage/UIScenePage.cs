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
	public VisualTreeAsset SceneCardTemplate;

	private DataScene sceneConfig;
	private UIScenePanel scenePanel;

	public override VisualElement Element => root.Q<VisualElement>("ScenePage");
	public VisualElement ScrollView => Q<VisualElement>("ScrollView");
	public Button Button1 => Q<Button>("Button1");// 返回
	public Button Button2 => Q<Button>("Button2");// 开始
	public Label SceneLabel => Q<Label>("SceneLabel");// 场景标签

	private void Awake() {
		scenePanel = new UIScenePanel(ScrollView, root, SceneCardTemplate, SetSceneConfig);

		Button1.clicked += () => ModuleUI.Jump(EnumPage.Menu);
		Button2.clicked += () => Button2_clicked();

		ModuleUI.OnJumpPage += ModuleUI_OnJumpPage;
	}
	private void OnDestroy() {
		scenePanel.Release();
		ModuleUI.OnJumpPage -= ModuleUI_OnJumpPage;
	}
	private void Update() => scenePanel.Update();

	private void Button2_clicked() {
		if (sceneConfig == null) { return; }
		ManagerScene.I.LoadScene(sceneConfig.scene, SingleManager.Single);
	}
	private void ModuleUI_OnJumpPage(EnumPage type) {
		Element.EnableInClassList("document-page-hide", type != EnumPage.Scene);
		if (type != EnumPage.Scene) { return; }
		SetSceneConfig(null);
		AssetsScene.I.LoadExtendScene(scenePanel.Create);
	}

	/// <summary> 选中的场景配置 </summary>
	public void SetSceneConfig(DataScene sceneConfig) {
		this.sceneConfig = sceneConfig;
		SceneLabel.text = sceneConfig != null ? sceneConfig.name : "???";
	}

}
