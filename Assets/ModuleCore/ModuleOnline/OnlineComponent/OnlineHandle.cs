using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/// <summary>
/// 角色控制 - 联机
/// </summary>
public class OnlineHandle : NetworkBehaviour, ICharacterHandle {

	private CCharacter control;
	private DataMotion dataMotion;
	private Func<bool> baseMotionTransition;

	public CCharacter Control => control;

	protected override void OnSynchronize<T>(ref BufferSerializer<T> serializer) {
		serializer.SerializeValue(ref dataMotion);
		base.OnSynchronize(ref serializer);
	}
	public override void OnNetworkSpawn() {
		if (IsOwner) { Create(); return; }
		ModuleVisual.I.Character.UpdateVisual(ref control);
		MoveSync(dataMotion);
	}
	public override void OnDestroy() {
		base.OnDestroy();
		ModuleVisual.I.Character.ReleaseVisual(control);
	}

	public void Update() {
		if (baseMotionTransition == null || control == null) { return; }
		if (baseMotionTransition()) { baseMotionTransition = null; }
	}

	#region 创建角色
	public void Create() {
		dataMotion = new DataMotion();
		ModuleVisual.I.Character.UpdateVisual(ref control);
		CreateCharacterServerRpc();
	}
	[ServerRpc]
	public void CreateCharacterServerRpc() {
		CreateCharacterClientRpc();
		if (IsHost) { return; }
		dataMotion = new DataMotion();
		ModuleVisual.I.Character.UpdateVisual(ref control);
	}
	[ClientRpc]
	public void CreateCharacterClientRpc() {
		if (IsOwner) { return; }
		dataMotion = new DataMotion();
		ModuleVisual.I.Character.UpdateVisual(ref control);
	}
	#endregion
	#region 移动动作
	public void Move(Vector2 moveInput) {
		baseMotionTransition = () => MoveSend(moveInput);
	}
	public bool MoveSend(Vector2 moveInput) {
		if (!ModuleCharacter.Move(control, moveInput, true)) { return false; }
		dataMotion.Update(moveInput, control);
		MoveServerRpc(dataMotion);
		return true;
	}
	[ServerRpc]
	public void MoveServerRpc(DataMotion dataMotion) {
		this.dataMotion = dataMotion;
		baseMotionTransition = () => MoveSync(dataMotion);
		MoveClientRpc(dataMotion);
	}
	[ClientRpc]
	public void MoveClientRpc(DataMotion dataMotion) {
		if (IsOwner) { return; }
		baseMotionTransition = () => MoveSync(dataMotion);
	}
	public bool MoveSync(DataMotion dataMotion) {
		Vector2 moveInput = dataMotion.moveInput;
		Vector3 position = dataMotion.position;
		Vector3 eulerAngles = dataMotion.eulerAngles;
		return ModuleCharacter.Move(control, moveInput, true, position, eulerAngles);
	}
	#endregion

	#region 跳跃动作
	public void Jump(Vector2 moveInput) {
		baseMotionTransition = () => JumpSend(moveInput);
	}
	public bool JumpSend(Vector2 moveInput) {
		if (!ModuleCharacter.Jump(control, moveInput, true)) { return false; }
		dataMotion.Update(moveInput, control);
		JumpServerRpc(dataMotion);
		return true;
	}
	[ServerRpc]
	public void JumpServerRpc(DataMotion dataMotion) {
		this.dataMotion = dataMotion;
		baseMotionTransition = () => JumpSync(dataMotion);
		JumpClientRpc(dataMotion);
	}
	[ClientRpc]
	public void JumpClientRpc(DataMotion dataMotion) {
		if (IsOwner) { return; }
		baseMotionTransition = () => JumpSync(dataMotion);
	}
	public bool JumpSync(DataMotion dataMotion) {
		Vector2 moveInput = dataMotion.moveInput;
		Vector3 position = dataMotion.position;
		Vector3 eulerAngles = dataMotion.eulerAngles;
		return ModuleCharacter.Jump(control, moveInput, true, position, eulerAngles);
	}
	#endregion
}
