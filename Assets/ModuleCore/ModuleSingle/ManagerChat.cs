using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;
using System;

/// <summary>
/// 聊天 - 管理器
/// </summary>
public class ManagerChat : ModuleSingle<ManagerChat> {

	public static event Action<DataChat> OnNewChat;

	public List<DataChat> historys = new List<DataChat>();

	/// <summary> 单机玩家 </summary>
	public SingleChatHandle singleHandle = new SingleChatHandle();

	/// <summary> 联机玩家 </summary>
	public OnlinePlayer OnlinePlayer => OnlinePlayer.CurrentPlayer;
	public OnlineChat onlineChat => OnlinePlayer.onlineChat;

	/// <summary> 聊天处理器 </summary>
	public IChatHandle handle => OnlinePlayer != null ? onlineChat : singleHandle;

	protected override void Awake() => NoReplace(false);
	/// <summary> 发送聊天消息 </summary>
	public void Sending(string content) => handle.Sending(content);
	/// <summary> 接收聊天消息 </summary>
	public void Receive(DataChat chat) {
		historys.Add(chat);
		OnNewChat?.Invoke(chat);
	}
}
/// <summary>
/// 聊天处理器
/// </summary>
public interface IChatHandle {
	/// <summary> 发送 </summary>
	public void Sending(string content);
}
/// <summary>
/// 单机 - 聊天处理器
/// </summary>
public class SingleChatHandle : IChatHandle {
	public void Sending(string content) {
		DataChat chat = new DataChat();
		chat.id = "Test";
		chat.name = "Test";
		chat.time = DateTime.Now.ToString("HH:mm");
		chat.content = content;
		ManagerChat.I.Receive(chat);
	}
}