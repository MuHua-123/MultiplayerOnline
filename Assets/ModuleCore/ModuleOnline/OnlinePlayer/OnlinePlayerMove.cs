using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/// <summary>
/// 玩家移动 - 联机
/// </summary>
public class OnlinePlayerMove : NetworkBehaviour {

	public OnlinePlayer player;

	public DataMotion dataMotion => player.dataMotion;
	public ControlCharacter control => player.control;

	[ServerRpc]
	public void ServerRpc(Vector2 moveInput) {
		player.baseMotionTransition = () => Server(moveInput);
	}
	public bool Server(Vector2 moveInput) {
		dataMotion.Update(moveInput, control);
		bool isComplete = Client(dataMotion);
		if (isComplete) { ClientRpc(dataMotion); }
		return isComplete;
	}
	[ClientRpc]
	public void ClientRpc(DataMotion dataMotion) {
		if (!IsHost) { player.baseMotionTransition = () => Client(dataMotion); }
	}
	public bool Client(DataMotion dataMotion) {
		Vector3 position = dataMotion.position;
		Vector3 eulerAngles = dataMotion.eulerAngles;
		Vector2 moveInput = dataMotion.moveInput;
		return ModuleCharacter.Move(control, moveInput, true, position, eulerAngles);
	}
}
