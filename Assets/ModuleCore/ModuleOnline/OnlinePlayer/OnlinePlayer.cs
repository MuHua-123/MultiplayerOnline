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

	public OnlinePlayerMove move;

	[HideInInspector] public ControlCharacter control;

	public Func<bool> baseMotionTransition;

	public override void OnNetworkSpawn() {
		if (IsOwner) { CreateCharacterServerRpc(); return; }
		ModuleVisual.I.Character.UpdateVisual(ref control);
		move.InitialNetworkSpawn();
	}
	public override void OnDestroy() {
		base.OnDestroy();
		ModuleVisual.I.Character.ReleaseVisual(control);
	}
	public void Update() {
		if (baseMotionTransition == null) { return; }
		if (baseMotionTransition()) { baseMotionTransition = null; }
	}

	#region 创建角色
	[ServerRpc]
	public void CreateCharacterServerRpc() {
		CreateCharacter();
		CreateCharacterClientRpc();
	}
	[ClientRpc]
	public void CreateCharacterClientRpc() {
		if (!IsHost) { CreateCharacter(); }
	}
	public void CreateCharacter() {
		move.InitialData();
		ModuleCharacter.CreateCharacter(ref control);
	}
	#endregion
}