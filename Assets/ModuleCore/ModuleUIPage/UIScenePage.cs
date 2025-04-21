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

	public DataSceneConfig sceneConfig;
	public UIScrollView scrollView;
	public ModuleUIItems<UISceneConfigItem, DataSceneConfig> SceneConfigs;

	public override VisualElement Element => root.Q<VisualElement>("ScenePage");
	public Button Button1 => Q<Button>("Button1");// 返回
	public Button Button2 => Q<Button>("Button2");// 开始
	public Label SceneLabel => Q<Label>("SceneLabel");// 场景标签

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
		SetSceneConfig(null);
		AssetsSceneConfig.I.UpdateSceneConfig();
	}
	private void AssetsSceneConfig_OnChange() {
		SceneConfigs.Create(AssetsSceneConfig.Datas);
	}

	/// <summary> 选中的场景配置 </summary>
	public void SetSceneConfig(DataSceneConfig sceneConfig) {
		this.sceneConfig = sceneConfig;
		SceneLabel.text = sceneConfig != null ? sceneConfig.name : "???";
	}

	#region UI项定义
	/// <summary>
	/// 模组 UI项
	/// </summary>
	public class UISceneConfigItem : ModuleUIItem<DataSceneConfig> {
		public readonly UIScenePage parent;

		public Label Title => Q<Label>("Title");
		public VisualElement Image => Q<VisualElement>("Image");

		public UISceneConfigItem(DataSceneConfig value, VisualElement element, UIScenePage parent) : base(value, element) {
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
