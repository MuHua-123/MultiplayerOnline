using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 输入模块
/// </summary>
public class ModuleInput : ModuleSingle<ModuleInput> {
	public static event Action<InputLevel> OnInputLevel;

	protected override void Awake() => NoReplace();

	/// <summary> 禁止输入等级 </summary>
	public void Disable() {
		OnInputLevel?.Invoke(InputLevel.None);
	}
	/// <summary> 启用预览等级 </summary>
	public void EnablePreview() {
		OnInputLevel?.Invoke(InputLevel.Preview);
	}

	public void Move(Vector2 moveInput) {
		OnlinePlayer onlinePlayer = OnlinePlayer.Find();
		if (onlinePlayer == null) { SinglePlayer.I.Move(moveInput); }
		else { onlinePlayer.MoveServerRpc(moveInput); }
	}
	public void Jump() {
		OnlinePlayer onlinePlayer = OnlinePlayer.Find();
		if (onlinePlayer == null) { SinglePlayer.I.Jump(); }
		// else { onlinePlayer.MoveServerRpc(moveInput); }
	}
}
