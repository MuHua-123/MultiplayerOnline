using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;
using System;

/// <summary>
/// 战斗页面
/// </summary>
public class UIBattlePage : ModuleUIPage {
	public VisualTreeAsset ChatTemplate;

	public UIChattingPanel chattingPanel;

	public override VisualElement Element => root.Q<VisualElement>("BattlePage");

	public VisualElement Chatting => Q<VisualElement>("Chatting");

	private void Awake() {
		chattingPanel = new UIChattingPanel(Chatting, root, ChatTemplate);

		ModuleUI.OnJumpPage += ModuleUI_OnJumpPage;
	}
	private void OnDestroy() {
		chattingPanel.Release();
		ModuleUI.OnJumpPage -= ModuleUI_OnJumpPage;
	}
	private void Update() => chattingPanel.Update();

	private void ModuleUI_OnJumpPage(EnumPage page) {
		Element.EnableInClassList("document-page-hide", page != EnumPage.Battle);
		if (page != EnumPage.Battle) { return; }
	}
}
