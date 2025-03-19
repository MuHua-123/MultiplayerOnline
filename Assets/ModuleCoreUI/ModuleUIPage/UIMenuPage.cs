using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

public class UIMenuPage : ModuleUIPage {

	private string DefaultPort = "5000";
	private string Roamhost = "127.0.0.1";
	private string Localhost = "192.168.1.102";

	public override VisualElement Element => root.Q<VisualElement>("MenuPage");
	public Button Button1 => Q<Button>("Button1");//创建服务器
	public Button Button2 => Q<Button>("Button2");//创建主机
	public Button Button3 => Q<Button>("Button3");//连接服务器
	public Button Button4 => Q<Button>("Button4");//单机

	private void Awake() {
		Button1.clicked += Button1_clicked;
		Button2.clicked += Button2_clicked;
		Button3.clicked += Button3_clicked;
		Button4.clicked += Button4_clicked;

		ModuleUI.OnJumpPage += ModuleUI_OnJumpPage;
	}

	private void ModuleUI_OnJumpPage(UIPageType type) {
		Element.EnableInClassList("document-page-hide", type != UIPageType.Menu);
		if (type != UIPageType.Menu) { return; }
	}

	private void Button1_clicked() {
		OnlineManager.I.StartServer(Roamhost, DefaultPort, "SyncTestScene");
	}
	private void Button2_clicked() {
		OnlineManager.I.StartHost(Localhost, DefaultPort, "SyncTestScene");
		ModuleUI.Jump(UIPageType.Preview);
		ModuleInput.I.EnablePreview();
		ModuleCamera.I.EnableThirdPerson();
	}
	private void Button3_clicked() {
		OnlineManager.I.StartClient(Localhost, DefaultPort);
		ModuleUI.Jump(UIPageType.Preview);
		ModuleInput.I.EnablePreview();
		ModuleCamera.I.EnableThirdPerson();
	}
	private void Button4_clicked() {
		throw new NotImplementedException();
	}
}
