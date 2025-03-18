using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 输入模块
/// </summary>
public class ModuleInput : ModuleSingle<ModuleInput> {

	protected override void Awake() => Replace();

	public void Move(Vector2 moveInput) {
		OnlinePlayer onlinePlayer = OnlinePlayer.Find();
		if (onlinePlayer == null) { SinglePlayer.I.Move(moveInput); }
		else { onlinePlayer.MoveServerRpc(moveInput); }
	}
}
