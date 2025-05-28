using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

/// <summary>
/// 模组管理窗口
/// </summary>
public class UIModuleWindow : UIWindow {

	public UIScrollView scrollView;
	public ModuleUIItems<UIModuleItem, DataModule> ModuleConfigs;

	public UIModuleWindow(VisualElement element, VisualElement canvas, VisualTreeAsset templateAsset) : base(element, canvas) {

		VisualElement ScrollView = Container.Q<VisualElement>("ScrollView");
		scrollView = new UIScrollView(ScrollView, canvas, UIDirection.Vertical);

		ModuleConfigs = new ModuleUIItems<UIModuleItem, DataModule>(scrollView.Container, templateAsset,
		 (data, element) => new UIModuleItem(data, element, this));
	}
	public void Release() {
		ModuleConfigs.Release();
	}
	public override void Update() {
		base.Update();
		scrollView.Update();
	}

	/// <summary> 设置活动状态 </summary>
	public override void SetActive(bool active) {
		base.SetActive(active);
		if (!active) { return; }
		ModuleConfigs.Create(AssetsModule.I.modules);
	}

	#region UI项定义
	/// <summary>
	/// 模组 UI项
	/// </summary>
	public class UIModuleItem : ModuleUIItem<DataModule> {
		public readonly UIModuleWindow parent;

		public Label Title => element.Q<Label>("Title");
		public VisualElement Toggle => element.Q<VisualElement>("Toggle");
		public VisualElement Check => Toggle.Q<VisualElement>("Check");

		public UIModuleItem(DataModule value, VisualElement element, UIModuleWindow parent) : base(value, element) {
			this.parent = parent;
			Title.text = value.name;
			Check.EnableInClassList("template-hide", !value.isEnable);
			Toggle.RegisterCallback<ClickEvent>(EnableAndDisable);
		}
		private void EnableAndDisable(ClickEvent evt) {
			value.isEnable = !value.isEnable;
			Check.EnableInClassList("template-hide", !value.isEnable);
			if (value.isEnable) { AssetsModule.I.LoadingModuleConfig(value); }
			else { AssetsModule.I.UnloadModuleConfig(value); }
		}
	}
	#endregion
}
