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

	[HideInInspector] public DataMotion dataMotion;
	[HideInInspector] public ControlCharacter control;

	public Func<bool> baseMotionTransition;

	protected override void OnSynchronize<T>(ref BufferSerializer<T> serializer) {
		serializer.SerializeValue(ref dataMotion);
		base.OnSynchronize(ref serializer);
	}
	public override void OnNetworkSpawn() {
		if (IsOwner) { CreateCharacterServerRpc(); return; }
		ModuleVisual.I.Character.UpdateVisual(ref control);
		Move(dataMotion);
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
		dataMotion = new DataMotion();
		ModuleCharacter.CreateCharacter(ref control);
	}
	#endregion

	#region 移动动作
	[ServerRpc]
	public void MoveServerRpc(Vector2 moveInput) {
		baseMotionTransition = () => Move(moveInput);
	}
	public bool Move(Vector2 moveInput) {
		dataMotion.Update(moveInput, control);
		bool isComplete = Move(dataMotion);
		if (isComplete) { MoveClientRpc(dataMotion); }
		return isComplete;
	}
	[ClientRpc]
	public void MoveClientRpc(DataMotion dataMotion) {
		if (!IsHost) { baseMotionTransition = () => Move(dataMotion); }
	}
	public bool Move(DataMotion dataMotion) {
		Vector3 position = dataMotion.position;
		Vector3 eulerAngles = dataMotion.eulerAngles;
		Vector2 moveInput = dataMotion.moveInput;
		return ModuleCharacter.Move(control, moveInput, true, position, eulerAngles);
	}
	#endregion

	#region 跳跃动作
	[ServerRpc]
	public void JumpServerRpc(Vector2 moveInput) {
		baseMotionTransition = () => Jump(moveInput);
	}
	public bool Jump(Vector2 moveInput) {
		dataMotion.Update(moveInput, control);
		bool isComplete = Jump(dataMotion);
		if (isComplete) { JumpClientRpc(dataMotion); }
		return isComplete;
	}
	[ClientRpc]
	public void JumpClientRpc(DataMotion dataMotion) {
		if (!IsHost) { baseMotionTransition = () => Jump(dataMotion); }
	}
	public bool Jump(DataMotion dataMotion) {
		Vector3 position = dataMotion.position;
		Vector3 eulerAngles = dataMotion.eulerAngles;
		Vector2 moveInput = dataMotion.moveInput;
		return ModuleCharacter.Jump(control, moveInput, true, position, eulerAngles);
	}
	#endregion
}
/// <summary>
/// 运动 - 数据
/// </summary>
[Serializable]
public struct DataMotion : INetworkSerializable {

	/// <summary> 移动方向 </summary>
	public Vector2 moveInput;
	/// <summary> 位置 </summary>
	public Vector3 position;
	/// <summary> 旋转 </summary>
	public Vector3 eulerAngles;

	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
		serializer.SerializeValue(ref moveInput);
		serializer.SerializeValue(ref position);
		serializer.SerializeValue(ref eulerAngles);
	}

	public void Update(Vector2 moveInput, ControlCharacter character) {
		this.moveInput = moveInput;
		position = character.transform.position;
		eulerAngles = character.transform.eulerAngles;
	}
}