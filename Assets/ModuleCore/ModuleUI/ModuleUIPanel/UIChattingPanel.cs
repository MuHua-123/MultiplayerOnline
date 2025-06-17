using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

/// <summary>
/// 聊天 - 面板
/// </summary>
public class UIChattingPanel : ModuleUIPanel {

	public UIScrollViewListV<UIChat, DataChat> scrollView;

	public VisualElement ScrollView => Q<VisualElement>("ScrollView");
	public UITextField Input => Q<UITextField>("Input");
	public Button Send => Q<Button>("Send");

	public UIChattingPanel(VisualElement element, VisualElement canvas, VisualTreeAsset templateAsset) : base(element) {
		scrollView = new UIScrollViewListV<UIChat, DataChat>(ScrollView, canvas, templateAsset,
			(data, element) => new UIChat(data, element, this), UIScrollViewV.UIDirection.FromBottomToTop);
		scrollView.Release();

		Send.clicked += () => SendContent();
		Input.RegisterCallback<FocusInEvent>((evt) => { ModuleInput.TemporarilyDisable(true); });
		Input.RegisterCallback<FocusOutEvent>((evt) => { ModuleInput.TemporarilyDisable(false); });

		ManagerChat.OnNewChat += ManagerChat_OnNewChat;
	}

	public void Release() => scrollView.Dispose();

	public void Update() => scrollView.Update();

	private void ManagerChat_OnNewChat(DataChat chat) {
		scrollView.Create(chat);
		scrollView.UpdateValue(0);
	}

	/// <summary> 发送内容 </summary>
	private void SendContent() {
		if (Input.value == "") { return; }
		ManagerChat.I.Sending(Input.value);
		Input.value = "";
	}

	#region UI项定义
	/// <summary>
	/// 聊天 UI项
	/// </summary>
	public class UIChat : ModuleUIItem<DataChat> {
		public readonly UIChattingPanel parent;

		public Label Name => Q<Label>("Name");
		public Label Content => Q<Label>("Content");

		public UIChat(DataChat value, VisualElement element, UIChattingPanel parent) : base(value, element) {
			this.parent = parent;
			string value1 = $"{value.name} : <size=16>{value.time}</size>";
			string value2 = $"<size=16>{value.time}</size> : {value.name}";

			Name.text = value.isOwner ? value2 : value1;
			Content.text = value.content;

			Name.EnableInClassList("chat-template-name1", !value.isOwner);
			Name.EnableInClassList("chat-template-name2", value.isOwner);
			Content.EnableInClassList("chat-template-content1", !value.isOwner);
			Content.EnableInClassList("chat-template-content2", value.isOwner);
		}
	}
	#endregion
}
