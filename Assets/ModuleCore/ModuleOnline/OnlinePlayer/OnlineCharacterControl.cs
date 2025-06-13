using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/// <summary>
/// 角色控制 - 联机
/// </summary>
public class OnlineCharacterControl : NetworkBehaviour {

	public OnlinePlayer player;

	[HideInInspector] public DataMotion dataMotion;

	public Func<bool> baseMotionTransition;

	public ControlCharacter control => player.control;

	protected override void OnSynchronize<T>(ref BufferSerializer<T> serializer) {
		serializer.SerializeValue(ref dataMotion);
		base.OnSynchronize(ref serializer);
	}
	public void InitialSpawn() {
		MoveClient(dataMotion);
	}
	public void InitialData() {
		dataMotion = new DataMotion();
	}

	public void Update() {
		if (baseMotionTransition == null || control == null) { return; }
		if (baseMotionTransition()) { baseMotionTransition = null; }
	}

	#region 移动动作
	[ServerRpc]
	public void MoveServerRpc(Vector2 moveInput) {
		baseMotionTransition = () => MoveServer(moveInput);
	}
	public bool MoveServer(Vector2 moveInput) {
		dataMotion.Update(moveInput, control);
		bool isComplete = MoveClient(dataMotion);
		if (isComplete) { MoveClientRpc(dataMotion); }
		return isComplete;
	}
	[ClientRpc]
	public void MoveClientRpc(DataMotion dataMotion) {
		if (!IsHost) { baseMotionTransition = () => MoveClient(dataMotion); }
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
		baseMotionTransition = () => JumpServer(moveInput);
	}
	public bool JumpServer(Vector2 moveInput) {
		dataMotion.Update(moveInput, control);
		bool isComplete = JumpClient(dataMotion);
		if (isComplete) { JumpClientRpc(dataMotion); }
		return isComplete;
	}
	[ClientRpc]
	public void JumpClientRpc(DataMotion dataMotion) {
		if (!IsHost) { baseMotionTransition = () => JumpClient(dataMotion); }
	}
	public bool JumpClient(DataMotion dataMotion) {
		Vector3 position = dataMotion.position;
		Vector3 eulerAngles = dataMotion.eulerAngles;
		Vector2 moveInput = dataMotion.moveInput;
		return ModuleCharacter.Jump(control, moveInput, true, position, eulerAngles);
	}
	#endregion
}
