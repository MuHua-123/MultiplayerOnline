using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

/// <summary>
/// 场景面板
/// </summary>
public class UIScenePanel : ModuleUIPanel {

	public Action<DataScene> callback;
	public UIScrollList<UIScene, DataScene> scrollList;

	public UIScenePanel(VisualElement element, VisualElement canvas, VisualTreeAsset templateAsset, Action<DataScene> callback) : base(element) {
		this.callback = callback;

		scrollList = new UIScrollList<UIScene, DataScene>(element, canvas, templateAsset,
			(data, element) => new UIScene(data, element, this), UIDirection.Horizontal);
	}

	public void Release() => scrollList.Release();

	public void Update() => scrollList.Update();

	public void Create() => scrollList.Create(AssetsScene.I.extendScenes);

	/// <summary> 设置回调 </summary>
	public void Settings(DataScene dataScene) => callback?.Invoke(dataScene);

	#region UI项定义
	/// <summary>
	/// 场景 UI项
	/// </summary>
	public class UIScene : ModuleUIItem<DataScene> {
		public readonly UIScenePanel parent;

		public Label Title => Q<Label>("Title");
		public VisualElement Image => Q<VisualElement>("Image");

		public UIScene(DataScene value, VisualElement element, UIScenePanel parent) : base(value, element) {
			this.parent = parent;
			Title.text = value.name;
			Image.RegisterCallback<ClickEvent>(evt => Select());
		}
		public override void DefaultState() {
			Image.EnableInClassList("sp-template-s", false);
		}
		public override void SelectState() {
			parent.Settings(value);
			Image.EnableInClassList("sp-template-s", true);
		}
	}
	#endregion
}
