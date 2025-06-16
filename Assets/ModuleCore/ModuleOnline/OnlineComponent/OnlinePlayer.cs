using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using MuHua;

/// <summary>
/// 玩家 - 联机
/// </summary>
public class OnlinePlayer : NetworkBehaviour {
	[Header("联机组件")]
	public OnlineScene onlineScene;
	public OnlineHandle onlineHandle;

	private void Awake() => DontDestroyOnLoad(gameObject);

	/// <summary> 当前玩家 </summary>
	[HideInInspector]
	public static OnlinePlayer currentPlayer;
	public static OnlinePlayer CurrentPlayer => GetCurrentPlayer();
	public static OnlinePlayer GetCurrentPlayer() {
		if (currentPlayer != null) { return currentPlayer; }
		NetworkObject network = NetworkManager.Singleton.LocalClient.PlayerObject;
		currentPlayer = network?.GetComponent<OnlinePlayer>();
		return currentPlayer;
	}
}