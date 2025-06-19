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
	public UIScrollViewListV<UIOnlineChat, DataOnlineChat> scrollView;

	public VisualElement ScrollView => Q<VisualElement>("ScrollView");
	public UITextField Input => Q<UITextField>("Input");
	public VisualElement Send => Q<VisualElement>("Send");

	public UIChattingPanel(VisualElement element, VisualElement canvas, VisualTreeAsset templateAsset) : base(element) {
		// 初始化滚动视图
		scrollView = new UIScrollViewListV<UIOnlineChat, DataOnlineChat>(
			ScrollView,
			canvas,
			templateAsset,
			(data, el) => new UIOnlineChat(data, el, this),
			UIScrollViewV.UIDirection.FromBottomToTop
		);
		scrollView.Release();

		// 发送按钮事件
		Send.RegisterCallback<ClickEvent>(_ => SendContent());

		// 输入框焦点事件
		Input.RegisterCallback<FocusEvent>(_ => ModuleInput.Settings(EnumInputMode.InputText));
		Input.RegisterCallback<BlurEvent>(_ => ModuleInput.Back());

		// 聊天消息监听
		ManagerChat.OnNewChat += ManagerChat_OnNewChat;
	}

	public void Release() => scrollView.Dispose();

	public void Update() => scrollView.Update();

	private void ManagerChat_OnNewChat(DataOnlineChat chat) {
		scrollView.Create(chat);
		scrollView.UpdateValue(0);
	}

	/// <summary>
	/// 发送内容
	/// </summary>
	private void SendContent() {
		if (string.IsNullOrWhiteSpace(Input.value)) return;
		ManagerChat.I.Sending(Input.value);
		Input.value = string.Empty;
		Input.Focus();
	}

	#region UI项定义
	/// <summary>
	/// 聊天 UI项
	/// </summary>
	public class UIOnlineChat : ModuleUIItem<DataOnlineChat> {
		public readonly UIChattingPanel parent;

		public Label Name => Q<Label>("Name");
		public Label Content => Q<Label>("Content");

		public UIOnlineChat(DataOnlineChat value, VisualElement element, UIChattingPanel parent) : base(value, element) {
			this.parent = parent;

			// 格式化名称显示
			string left = $"{value.name} : <size=16>{value.time}</size>";
			string right = $"<size=16>{value.time}</size> : {value.name}";
			Name.text = value.isOwner ? right : left;
			Content.text = value.content;

			// 设置样式
			Name.EnableInClassList("chat-template-name1", !value.isOwner);
			Name.EnableInClassList("chat-template-name2", value.isOwner);
			Content.EnableInClassList("chat-template-content1", !value.isOwner);
			Content.EnableInClassList("chat-template-content2", value.isOwner);
		}
	}
	#endregion
}
