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

	public UIScrollList<UIChat, DataChat> scrollList;

	public VisualElement ScrollView => Q<VisualElement>("ScrollView");
	public UITextField Input => Q<UITextField>("Input");
	public Button Send => Q<Button>("Send");

	public UIChattingPanel(VisualElement element, VisualElement canvas, VisualTreeAsset templateAsset) : base(element) {
		scrollList = new UIScrollList<UIChat, DataChat>(ScrollView, canvas, templateAsset,
			(data, element) => new UIChat(data, element, this), UIDirection.Vertical, UIDirection.FromLeftToRight, UIDirection.FromBottomToTop);

		Send.clicked += () => SendContent();
	}

	public void Release() => scrollList.Release();

	public void Update() => scrollList.Update();

	/// <summary> 发送内容 </summary>
	private void SendContent() {
		if (Input.value == "") { return; }
		DataChat chat = new DataChat();
		chat.name = "Test";
		chat.time = DateTime.Now.ToString("HH:mm");
		chat.content = Input.value;
		scrollList.Create(chat);
		scrollList.UpdateValue(new Vector2(0, 1));
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
			Name.text = $"{value.name} : <size=16>{value.time}</size>";
			Content.text = value.content;
		}
	}
	#endregion
}
