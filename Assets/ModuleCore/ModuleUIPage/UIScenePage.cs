using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;
using System;

/// <summary>
/// 场景页面
/// </summary>
public class UIScenePage : ModuleUIPage {
	public VisualTreeAsset SceneCardTemplate;

	public UIScrollView scrollView;
	public ModuleUIItems<UISceneConfigItem, DataSceneConfig> SceneConfigs;

	public override VisualElement Element => root.Q<VisualElement>("ScenePage");
	public Button Button1 => Q<Button>("Button1");//返回
	public Button Button2 => Q<Button>("Button2");//开始

	private void Awake() {
		VisualElement ScrollView = Q<VisualElement>("ScrollView");
		scrollView = new UIScrollView(ScrollView, root, UIDirection.Vertical);

		SceneConfigs = new ModuleUIItems<UISceneConfigItem, DataSceneConfig>(scrollView.Container, SceneCardTemplate,
		 (data, element) => new UISceneConfigItem(data, element, this));

		Button1.clicked += () => { ModuleUI.Jump(DataPage.Menu); };
		// Button2.clicked += () => { ModuleUI.Jump(DataPage.Menu); };

		ModuleUI.OnJumpPage += ModuleUI_OnJumpPage;
		AssetsSceneConfig.OnChange += AssetsSceneConfig_OnChange;
	}
	private void OnDestroy() {
		SceneConfigs.Release();
	}
	private void Update() {
		scrollView.Update();
	}

	private void ModuleUI_OnJumpPage(DataPage type) {
		Element.EnableInClassList("document-page-hide", type != DataPage.Scene);
		if (type != DataPage.Scene) { return; }
		AssetsSceneConfig.I.UpdateSceneConfig();
	}
	private void AssetsSceneConfig_OnChange() {
		SceneConfigs.Create(AssetsSceneConfig.Datas);
	}

	#region UI项定义
	/// <summary>
	/// 模组 UI项
	/// </summary>
	public class UISceneConfigItem : ModuleUIItem<DataSceneConfig> {
		public readonly UIScenePage parent;

		public Label Title => element.Q<Label>("Title");
		public VisualElement Toggle => element.Q<VisualElement>("Toggle");
		public VisualElement Check => Toggle.Q<VisualElement>("Check");

		public UISceneConfigItem(DataSceneConfig value, VisualElement element, UIScenePage parent) : base(value, element) {
			this.parent = parent;
			// Title.text = value.name;
			// Check.EnableInClassList("template-hide", !value.isEnable);
			// Toggle.RegisterCallback<ClickEvent>(EnableAndDisable);
		}
		// private void EnableAndDisable(ClickEvent evt) {
		// 	value.isEnable = !value.isEnable;
		// 	Check.EnableInClassList("template-hide", !value.isEnable);
		// 	if (value.isEnable) { AssetsModuleConfig.I.LoadingModuleConfig(value); }
		// 	else { AssetsModuleConfig.I.UnloadModuleConfig(value); }
		// }
	}
	#endregion
}
