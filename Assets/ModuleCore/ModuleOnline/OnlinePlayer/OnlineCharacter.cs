using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/// <summary>
/// 角色 - 联机
/// </summary>
public class OnlineCharacter : NetworkBehaviour {

	public OnlinePlayer player;
	public OnlineCharacterControl characterControl;

	public override void OnNetworkSpawn() {
		if (IsOwner) { CreateCharacterServerRpc(); return; }
		ModuleVisual.I.Character.UpdateVisual(ref player.control);
		characterControl.InitialSpawn();
	}
	public override void OnDestroy() {
		base.OnDestroy();
		ModuleVisual.I.Character.ReleaseVisual(player.control);
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
		characterControl.InitialData();
		ModuleCharacter.CreateCharacter(ref player.control);
	}
	#endregion
}
