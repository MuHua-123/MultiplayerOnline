using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

/// <summary>
/// 菜单页面
/// </summary>
public class UIMenuPage : ModuleUIPage {
	public UIWindowManager windowManager;

	public override VisualElement Element => root.Q<VisualElement>("MenuPage");
	public Button Button1 => Q<Button>("Button1");//创建服务器
	public Button Button2 => Q<Button>("Button2");//创建主机
	public Button Button3 => Q<Button>("Button3");//连接服务器
	public Button Button4 => Q<Button>("Button4");//单机模式
	public Button Button5 => Q<Button>("Button5");//模组管理

	private void Awake() {
		Button1.clicked += SingleManager.I.StartServer;
		Button2.clicked += SingleManager.I.StartHost;
		Button3.clicked += ModuleUI.OpenOnlineWindow;
		// Button4.clicked += SingleManager.I.Standalone;
		Button4.clicked += () => { ModuleUI.Jump(DataPage.Scene); };
		Button5.clicked += ModuleUI.OpenModuleWindow;

		ModuleUI.OnJumpPage += ModuleUI_OnJumpPage;
	}

	private void ModuleUI_OnJumpPage(DataPage type) {
		Element.EnableInClassList("document-page-hide", type != DataPage.Menu);
		if (type != DataPage.Menu) { return; }
	}
}
