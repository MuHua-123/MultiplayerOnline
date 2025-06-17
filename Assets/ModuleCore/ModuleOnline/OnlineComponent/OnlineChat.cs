using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/// <summary>
/// 聊天 - 联机管理器
/// </summary>
public class OnlineChat : NetworkBehaviour, IChatHandle {

	#region 发送消息
	public void Sending(string content) {
		SendingServerRpc(content);
	}
	[ServerRpc]
	public void SendingServerRpc(string content) {
		DataChat chat = new DataChat();
		chat.id = OwnerClientId.ToString();
		chat.name = OwnerClientId.ToString();
		chat.time = DateTime.Now.ToString("HH:mm");
		chat.content = content;
		SendingClientRpc(chat);
		if (!IsServer || IsHost) { return; }
		ManagerChat.I.Receive(chat);
	}
	[ClientRpc]
	public void SendingClientRpc(DataChat chat) {
		chat.isOwner = IsOwner;
		ManagerChat.I.Receive(chat);
	}
	#endregion
}
