using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using MuHua;


/// <summary>
/// 玩家管理器
/// </summary>
public class ManagerCharacter : ModuleSingle<ManagerCharacter> {

	/// <summary> 单机玩家 </summary>
	public SingleCharacterHandle singleHandle;

	/// <summary> 联机玩家 </summary>
	public OnlinePlayer OnlinePlayer => OnlinePlayer.CurrentPlayer;
	public OnlineHandle characterHandle => OnlinePlayer.onlineHandle;

	/// <summary> 当前角色处理器 </summary>
	public ICharacterHandle handle => OnlinePlayer != null ? characterHandle : singleHandle;
	/// <summary> 当前玩家控制器 </summary>
	public CCharacter CurrentControl => handle.Control;

	protected override void Awake() => NoReplace(false);

	private void Start() => singleHandle = new SingleCharacterHandle();

	public void Update() => singleHandle.Update();

	#region 单机
	/// <summary> 创建角色 </summary>
	public void Create() => handle.Create();
	#endregion

	#region 输入
	/// <summary> 玩家操作：移动 </summary>
	public void Move(Vector2 moveDirection) => handle.Move(moveDirection);
	/// <summary> 玩家操作：跳跃 </summary>
	public void Jump(Vector2 moveDirection) => handle.Jump(moveDirection);
	#endregion
}
/// <summary>
/// 角色处理器
/// </summary>
public interface ICharacterHandle {
	/// <summary> 角色控制器 </summary>
	public CCharacter Control { get; }
	/// <summary> 创建 </summary>
	public void Create();
	/// <summary> 移动 </summary>
	public void Move(Vector2 moveInput);
	/// <summary> 跳跃 </summary>
	public void Jump(Vector2 moveInput);
}
/// <summary>
/// 单机 - 角色处理器
/// </summary>
public class SingleCharacterHandle : ICharacterHandle {

	public CCharacter control;
	public Func<bool> baseMotionTransition;

	public CCharacter Control => control;

	public void Update() {
		if (baseMotionTransition == null) { return; }
		if (baseMotionTransition()) { baseMotionTransition = null; }
	}
	public void Create() {
		ModuleCharacter.CreateCharacter(ref control);
	}
	public void Move(Vector2 moveInput) {
		baseMotionTransition = () => ModuleCharacter.Move(control, moveInput, true);
	}
	public void Jump(Vector2 moveInput) {
		baseMotionTransition = () => ModuleCharacter.Jump(control, moveInput, true);
	}
}