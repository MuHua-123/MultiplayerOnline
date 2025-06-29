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
	private DataOnlineMotion motion;
	private Func<bool> baseMotionTransition;

	public CCharacter Control => control;

	public bool IsTransition => baseMotionTransition == null;

	protected override void OnSynchronize<T>(ref BufferSerializer<T> serializer) {
		serializer.SerializeValue(ref motion);
		base.OnSynchronize(ref serializer);
	}
	public override void OnNetworkSpawn() {
		if (IsOwner) { Create(); return; }
		ModuleVisual.I.Character.UpdateVisual(ref control);
		MoveSync(motion);
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
		motion = new DataOnlineMotion();
		ModuleVisual.I.Character.UpdateVisual(ref control);
		CreateCharacterServerRpc();
	}
	[ServerRpc]
	public void CreateCharacterServerRpc() {
		CreateCharacterClientRpc();
		if (IsHost) { return; }
		motion = new DataOnlineMotion();
		ModuleVisual.I.Character.UpdateVisual(ref control);
	}
	[ClientRpc]
	public void CreateCharacterClientRpc() {
		if (IsOwner) { return; }
		motion = new DataOnlineMotion();
		ModuleVisual.I.Character.UpdateVisual(ref control);
	}
	#endregion

	#region 移动&冲刺动作
	public void Move(Vector2 moveInput) {
		baseMotionTransition = () => MoveSend(moveInput, false);
	}
	public void Sprint(Vector2 moveInput) {
		baseMotionTransition = () => MoveSend(moveInput, true);
	}
	public bool MoveSend(Vector2 moveInput, bool isSprint) {
		if (!control.Move(moveInput, isSprint, true)) { return false; }
		motion.isSprint = isSprint;
		motion.moveInput = moveInput;
		motion.Update(control);
		MoveServerRpc(motion);
		return true;
	}
	[ServerRpc]
	public void MoveServerRpc(DataOnlineMotion motion) {
		this.motion = motion;
		baseMotionTransition = () => MoveSync(motion);
		MoveClientRpc(motion);
	}
	[ClientRpc]
	public void MoveClientRpc(DataOnlineMotion motion) {
		if (IsOwner) { return; }
		baseMotionTransition = () => MoveSync(motion);
	}
	public bool MoveSync(DataOnlineMotion motion) {
		bool isSprint = motion.isSprint;
		Vector2 moveInput = motion.moveInput;
		Vector3 position = motion.position;
		Vector3 eulerAngles = motion.eulerAngles;
		return control.Move(moveInput, isSprint, true, position, eulerAngles);
	}
	#endregion

	#region 跳跃动作
	public void Jump(Vector2 moveInput) {
		baseMotionTransition = () => JumpSend(moveInput);
	}
	public bool JumpSend(Vector2 moveInput) {
		if (!control.Jump(moveInput, true)) { return false; }
		motion.moveInput = moveInput;
		motion.Update(control);
		JumpServerRpc(motion);
		return true;
	}
	[ServerRpc]
	public void JumpServerRpc(DataOnlineMotion motion) {
		this.motion = motion;
		baseMotionTransition = () => JumpSync(motion);
		JumpClientRpc(motion);
	}
	[ClientRpc]
	public void JumpClientRpc(DataOnlineMotion motion) {
		if (IsOwner) { return; }
		baseMotionTransition = () => JumpSync(motion);
	}
	public bool JumpSync(DataOnlineMotion motion) {
		Vector2 moveInput = motion.moveInput;
		Vector3 position = motion.position;
		Vector3 eulerAngles = motion.eulerAngles;
		return control.Jump(moveInput, true, position, eulerAngles);
	}
	#endregion

	#region 攻击动作
	public void Attack(bool isAttack) {
		baseMotionTransition = () => AttackSend(isAttack);
	}
	public bool AttackSend(bool isAttack) {
		if (!control.Attack(isAttack)) { return false; }
		motion.isAttack = isAttack;
		motion.Update(control);
		AttackServerRpc(motion);
		return true;
	}
	[ServerRpc]
	public void AttackServerRpc(DataOnlineMotion motion) {
		this.motion = motion;
		baseMotionTransition = () => AttackSync(motion);
		AttackClientRpc(motion);
	}
	[ClientRpc]
	public void AttackClientRpc(DataOnlineMotion motion) {
		if (IsOwner) { return; }
		baseMotionTransition = () => AttackSync(motion);
	}
	public bool AttackSync(DataOnlineMotion motion) {
		bool isAttack = motion.isAttack;
		Vector3 position = motion.position;
		Vector3 eulerAngles = motion.eulerAngles;
		return control.Attack(isAttack, position, eulerAngles);
	}
	#endregion
}
