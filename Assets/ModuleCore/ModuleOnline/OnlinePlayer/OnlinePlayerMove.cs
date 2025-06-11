using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/// <summary>
/// 玩家移动 - 联机
/// </summary>
public class OnlinePlayerMove : NetworkBehaviour {

	public OnlinePlayer player;

	[HideInInspector] public DataMotion dataMotion;

	public ControlCharacter control => player.control;

	protected override void OnSynchronize<T>(ref BufferSerializer<T> serializer) {
		serializer.SerializeValue(ref dataMotion);
		base.OnSynchronize(ref serializer);
	}
	public void InitialNetworkSpawn() {
		MoveClient(dataMotion);
	}
	public void InitialData() {
		dataMotion = new DataMotion();
	}

	#region 移动动作
	[ServerRpc]
	public void MoveServerRpc(Vector2 moveInput) {
		player.baseMotionTransition = () => MoveServer(moveInput);
	}
	public bool MoveServer(Vector2 moveInput) {
		dataMotion.Update(moveInput, control);
		bool isComplete = MoveClient(dataMotion);
		if (isComplete) { MoveClientRpc(dataMotion); }
		return isComplete;
	}
	[ClientRpc]
	public void MoveClientRpc(DataMotion dataMotion) {
		if (!IsHost) { player.baseMotionTransition = () => MoveClient(dataMotion); }
	}
	public bool MoveClient(DataMotion dataMotion) {
		Vector3 position = dataMotion.position;
		Vector3 eulerAngles = dataMotion.eulerAngles;
		Vector2 moveInput = dataMotion.moveInput;
		return ModuleCharacter.Move(control, moveInput, true, position, eulerAngles);
	}
	#endregion

	#region 跳跃动作
	[ServerRpc]
	public void JumpServerRpc(Vector2 moveInput) {
		player.baseMotionTransition = () => JumpServer(moveInput);
	}
	public bool JumpServer(Vector2 moveInput) {
		dataMotion.Update(moveInput, control);
		bool isComplete = JumpClient(dataMotion);
		if (isComplete) { JumpClientRpc(dataMotion); }
		return isComplete;
	}
	[ClientRpc]
	public void JumpClientRpc(DataMotion dataMotion) {
		if (!IsHost) { player.baseMotionTransition = () => JumpClient(dataMotion); }
	}
	public bool JumpClient(DataMotion dataMotion) {
		Vector3 position = dataMotion.position;
		Vector3 eulerAngles = dataMotion.eulerAngles;
		Vector2 moveInput = dataMotion.moveInput;
		return ModuleCharacter.Jump(control, moveInput, true, position, eulerAngles);
	}
	#endregion
}
