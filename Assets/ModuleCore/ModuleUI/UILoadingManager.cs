using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

/// <summary>
/// 加载 - UI管理器
/// </summary>
public class UILoadingManager : ModuleUIPage {

	public UILoadingScene loadingScene;

	public override VisualElement Element => root.Q<VisualElement>("Loading");
	public VisualElement LoadingScene => Q<VisualElement>("LoadingScene");

	private void Awake() {
		loadingScene = new UILoadingScene(LoadingScene);
	}

	public void SettingsLoadingScene(bool isEnable, float value, string text) {
		loadingScene?.Settings(isEnable, value, text);
	}
}
