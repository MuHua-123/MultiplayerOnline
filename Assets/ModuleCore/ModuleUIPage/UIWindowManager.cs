using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

/// <summary>
/// 窗口管理器页面
/// </summary>
public class UIWindowManager : ModuleUIPage {
	public VisualTreeAsset onlineTemplate;

	public UIOnlineWindow onlineWindow;

	public override VisualElement Element => root.Q<VisualElement>("Window");

	public VisualElement OnlineWindow => Q<VisualElement>("OnlineWindow");

	private void Start() {
		onlineWindow = new UIOnlineWindow(OnlineWindow, root, onlineTemplate);
	}
}
