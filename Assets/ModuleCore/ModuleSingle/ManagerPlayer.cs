using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using MuHua;

/// <summary>
/// 玩家管理器
/// </summary>
public class ManagerPlayer : ModuleSingle<ManagerPlayer> {

	/// <summary> 单机玩家 </summary>
	[HideInInspector]
	public ControlCharacter control;
	public Func<bool> baseMotionTransition;

	/// <summary> 联机玩家 </summary>
	[HideInInspector]
	public OnlinePlayer onlinePlayer;
	public OnlinePlayer OnlinePlayer {
		get {
			if (onlinePlayer != null) { return onlinePlayer; }
			NetworkObject network = NetworkManager.Singleton.LocalClient.PlayerObject;
			onlinePlayer = network?.GetComponent<OnlinePlayer>();
			return onlinePlayer;
		}
	}

	/// <summary> 当前玩家控制器 </summary>
	public ControlCharacter CurrentControl => OnlinePlayer != null ? OnlinePlayer.control : control;

	protected override void Awake() => NoReplace(false);

	public void Update() {
		if (baseMotionTransition == null) { return; }
		if (baseMotionTransition()) { baseMotionTransition = null; }
	}

	#region 单机
	/// <summary> 创建单机角色 </summary>
	public void CreateCharacter() => ModuleCharacter.CreateCharacter(ref control);
	#endregion

	#region 输入选择器
	/// <summary> 玩家操作：移动 </summary>
	public void Move(Vector2 moveDirection) {
		if (OnlinePlayer == null) {
			baseMotionTransition = () => ModuleCharacter.Move(control, moveDirection, true);
		}
		else { onlinePlayer.move.MoveServerRpc(moveDirection); }
	}
	/// <summary> 玩家操作：跳跃 </summary>
	public void Jump(Vector2 moveDirection) {
		if (OnlinePlayer == null) {
			baseMotionTransition = () => ModuleCharacter.Jump(control, moveDirection, true);
		}
		else { onlinePlayer.move.JumpServerRpc(moveDirection); }
	}
	#endregion
}
