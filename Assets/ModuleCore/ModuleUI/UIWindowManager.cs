using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

/// <summary>
/// 窗口管理器页面
/// </summary>
public class UIWindowManager : ModuleUIPage {
	public VisualTreeAsset ColumnTemplate;
	public VisualTreeAsset ToggleTemplate;

	public UIOnlineWindow onlineWindow;
	public UIModuleWindow moduleWindow;

	public override VisualElement Element => root.Q<VisualElement>("Window");

	public VisualElement OnlineWindow => Q<VisualElement>("OnlineWindow");
	public VisualElement ModuleWindow => Q<VisualElement>("ModuleWindow");

	private void Start() {
		onlineWindow = new UIOnlineWindow(OnlineWindow, root, ColumnTemplate);
		moduleWindow = new UIModuleWindow(ModuleWindow, root, ToggleTemplate);
	}
	private void OnDestroy() {
		// onlineWindow.Release();
		// moduleWindow.Release();
	}
	private void Update() {
		onlineWindow.Update();
		moduleWindow.Update();
	}
}
