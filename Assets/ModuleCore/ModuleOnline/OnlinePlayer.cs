using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using MuHua;

/// <summary>
/// 玩家同步
/// </summary>
public class OnlinePlayer : NetworkBehaviour {

	[HideInInspector] public BaseCharacter controller;
	[HideInInspector] public DataCharacter character;

	public Func<bool> baseMotionTransition;

	protected override void OnSynchronize<T>(ref BufferSerializer<T> serializer) {
		serializer.SerializeValue(ref character);
		base.OnSynchronize(ref serializer);
	}
	public override void OnNetworkSpawn() {
		if (IsOwner) { CreateCharacterServerRpc(); return; }
		ModuleVisual.I.Character.UpdateVisual(ref controller);
		Move(character);
	}
	public override void OnDestroy() {
		base.OnDestroy();
		ModuleVisual.I.Character.ReleaseVisual(controller);
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
		character = new DataCharacter();
		ManagerPlayer.CreateCharacter(ref controller);
	}
	#endregion

	#region 移动动作
	[ServerRpc]
	public void MoveServerRpc(Vector2 moveInput) {
		baseMotionTransition = () => Move(moveInput);
	}
	[ClientRpc]
	public void MoveClientRpc(DataCharacter character) {
		if (!IsHost) { baseMotionTransition = () => Move(character); }
	}
	public bool Move(Vector2 moveInput) {
		character.Update(moveInput, controller);
		bool isComplete = Move(character);
		if (isComplete) { MoveClientRpc(character); }
		return isComplete;
	}
	public bool Move(DataCharacter character) {
		Vector3 position = character.position;
		Vector3 eulerAngles = character.eulerAngles;
		Vector2 moveInput = character.moveInput;
		return ManagerPlayer.Move(controller, moveInput, position, eulerAngles);
	}
	#endregion

	#region 跳跃动作
	[ServerRpc]
	public void JumpServerRpc(Vector2 moveInput) {
		baseMotionTransition = () => Jump(moveInput);
	}
	[ClientRpc]
	public void JumpClientRpc(DataCharacter character) {
		if (!IsHost) { baseMotionTransition = () => Jump(character); }
	}
	public bool Jump(Vector2 moveInput) {
		character.Update(moveInput, controller);
		bool isComplete = Jump(character);
		if (isComplete) { JumpClientRpc(character); }
		return isComplete;
	}
	public bool Jump(DataCharacter character) {
		Vector3 position = character.position;
		Vector3 eulerAngles = character.eulerAngles;
		Vector2 moveInput = character.moveInput;
		return ManagerPlayer.Jump(controller, moveInput, position, eulerAngles);
	}
	#endregion

	#region 工具
	private static OnlinePlayer onlinePlayer;
	public static OnlinePlayer Find() {
		if (onlinePlayer != null) { return onlinePlayer; }
		NetworkObject network = NetworkManager.Singleton.LocalClient.PlayerObject;
		onlinePlayer = network?.GetComponent<OnlinePlayer>();
		return onlinePlayer;
	}
	#endregion

}
