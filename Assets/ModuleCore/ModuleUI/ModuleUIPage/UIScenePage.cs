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

	public UIScrollList<UISceneConfigItem, DataScene> scrollList;

	public override VisualElement Element => root.Q<VisualElement>("ScenePage");
	public VisualElement ScrollView => Q<VisualElement>("ScrollView");
	public Button Button1 => Q<Button>("Button1");// 返回
	public Button Button2 => Q<Button>("Button2");// 开始
	public Label SceneLabel => Q<Label>("SceneLabel");// 场景标签

	private void Awake() {
		scrollList = new UIScrollList<UISceneConfigItem, DataScene>(ScrollView, root, SceneCardTemplate,
			(data, element) => new UISceneConfigItem(data, element, this), UIDirection.Horizontal);

		Button1.clicked += () => ModuleUI.Jump(EnumPage.Menu);
		Button2.clicked += () => Button2_clicked();

		ModuleUI.OnJumpPage += ModuleUI_OnJumpPage;
		AssetsScene.OnChangeConfig += AssetsSceneConfig_OnChangeConfig;
	}
	private void OnDestroy() => scrollList.Release();
	private void Update() => scrollList.Update();

	private void Button2_clicked() {
		if (!AssetsScene.I.isValid) { return; }
		AssetsScene.I.LoadScene(() => {
			ManagerPlayer.I.CreateCharacter();
			ModuleUI.Jump(EnumPage.Preview);
			ModuleInput.Mode(EnumInputMode.ThirdPerson);
			ModuleCamera.Mode(EnumCameraMode.ThirdPerson);
		});
	}
	private void ModuleUI_OnJumpPage(EnumPage type) {
		Element.EnableInClassList("document-page-hide", type != EnumPage.Scene);
		if (type != EnumPage.Scene) { return; }
		SetSceneConfig(null);
		AssetsScene.I.UpdateSceneConfig();
	}
	private void AssetsSceneConfig_OnChangeConfig() {
		scrollList.Create(AssetsScene.I.dataScenes);
	}

	/// <summary> 选中的场景配置 </summary>
	public void SetSceneConfig(DataScene sceneConfig) {
		AssetsScene.I.Settings(sceneConfig);
		SceneLabel.text = sceneConfig != null ? sceneConfig.name : "???";
	}

	#region UI项定义
	/// <summary>
	/// 模组 UI项
	/// </summary>
	public class UISceneConfigItem : ModuleUIItem<DataScene> {
		public readonly UIScenePage parent;

		public Label Title => Q<Label>("Title");
		public VisualElement Image => Q<VisualElement>("Image");

		public UISceneConfigItem(DataScene value, VisualElement element, UIScenePage parent) : base(value, element) {
			this.parent = parent;
			Title.text = value.name;
			Image.RegisterCallback<ClickEvent>(evt => Select());
		}
		public override void DefaultState() {
			Image.EnableInClassList("template-scenecard-s", false);
		}
		public override void SelectState() {
			parent.SetSceneConfig(value);
			Image.EnableInClassList("template-scenecard-s", true);
		}
	}
	#endregion
}
