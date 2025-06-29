using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

/// <summary>
/// 场景 - UI加载
/// </summary>
public class UILoadingScene : ModuleUIPanel {

	public UISliderH slider;

	public VisualElement Slider => Q<VisualElement>("Slider");

	public UILoadingScene(VisualElement element) : base(element) {
		slider = new UISliderH(Slider, element);
	}

	public void Settings(bool isEnable, float value, string text) {
		element.EnableInClassList("document-page-hide", !isEnable);
		slider.UpdateValue(value, false);
		slider.Title.text = text;
	}
}
