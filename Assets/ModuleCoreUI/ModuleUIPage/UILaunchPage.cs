using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

public class UILaunchPage : ModuleUIPage {

	private string DefaultPort = "5000";
	private string Roamhost = "127.0.0.1";
	private string Localhost = "127.0.0.1";

	public override VisualElement Element => root.Q<VisualElement>("LaunchPage");
	public Button Button1 => Q<Button>("Button1");//创建服务器
	public Button Button2 => Q<Button>("Button2");//创建主机
	public Button Button3 => Q<Button>("Button3");//连接服务器
	public Button Button4 => Q<Button>("Button4");//单机

	private void Awake() {
		Button1.clicked += Button1_clicked;
		Button2.clicked += Button2_clicked;
		Button3.clicked += Button3_clicked;
		Button4.clicked += Button4_clicked;
	}

	private void Button1_clicked() {
		OnlineController.I.StartServer(Roamhost, DefaultPort, "SyncScene");
	}
	private void Button2_clicked() {
		OnlineController.I.StartHost(Localhost, DefaultPort, "SyncScene");
	}
	private void Button3_clicked() {
		OnlineController.I.StartClient(Localhost, DefaultPort);
	}
	private void Button4_clicked() {
		throw new NotImplementedException();
	}
}
