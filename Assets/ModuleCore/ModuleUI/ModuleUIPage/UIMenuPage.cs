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

	public VisualElement Menu1 => Q<VisualElement>("Menu1");
	public Button Menu1Button1 => Menu1.Q<Button>("Button1");//联机模式
	public Button Menu1Button2 => Menu1.Q<Button>("Button2");//单机模式
	public Button Menu1Button3 => Menu1.Q<Button>("Button3");//模组管理
	public Button Menu1Button4 => Menu1.Q<Button>("Button4");//游戏设置
	public Button Menu1Button5 => Menu1.Q<Button>("Button5");//退出游戏

	public VisualElement Menu2 => Q<VisualElement>("Menu2");
	public Button Menu2Button1 => Menu2.Q<Button>("Button1");//创建服务器
	public Button Menu2Button2 => Menu2.Q<Button>("Button2");//创建主机
	public Button Menu2Button3 => Menu2.Q<Button>("Button3");//连接服务器
	public Button Menu2Button4 => Menu2.Q<Button>("Button4");//返回

	private void Awake() {
		Menu1Button1.clicked += () => SwitchMenu("2");
		Menu1Button2.clicked += () => Menu1Button2_clicked();
		Menu1Button3.clicked += () => ModuleUI.OpenModuleWindow();
		Menu1Button4.clicked += () => ModuleUI.Jump(EnumPage.Settings);
		Menu1Button5.clicked += () => Application.Quit();

		Menu2Button1.clicked += () => Menu2Button1_clicked();
		Menu2Button2.clicked += () => Menu2Button2_clicked();
		Menu2Button3.clicked += () => ModuleUI.OpenOnlineWindow();
		Menu2Button4.clicked += () => SwitchMenu("1");

		ModuleUI.OnJumpPage += ModuleUI_OnJumpPage;
	}

	private void ModuleUI_OnJumpPage(EnumPage type) {
		Element.EnableInClassList("document-page-hide", type != EnumPage.Menu);
		if (type != EnumPage.Menu) { return; }
	}

	private void SwitchMenu(string index) {
		Menu1.EnableInClassList("page-menu-hide", index != "1");
		Menu2.EnableInClassList("page-menu-hide", index != "2");
	}

	private void Menu1Button2_clicked() {
		ModuleUI.Jump(EnumPage.Scene);
		SingleManager.SetRunningMode(EnumRunningMode.Single);
	}
	private void Menu2Button1_clicked() {
		ModuleUI.Jump(EnumPage.Scene);
		SingleManager.SetRunningMode(EnumRunningMode.Server);
	}
	private void Menu2Button2_clicked() {
		ModuleUI.Jump(EnumPage.Scene);
		SingleManager.SetRunningMode(EnumRunningMode.Host);
	}
}
