using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 玩家管理器
/// </summary>
public class ManagerPlayer : ModuleSingle<ManagerPlayer> {

	protected override void Awake() => NoReplace(false);

	#region 单机
	[HideInInspector]
	public BaseCharacter character;
	public Func<bool> baseMotionTransition;
	public void Update() {
		if (baseMotionTransition == null) { return; }
		if (baseMotionTransition()) { baseMotionTransition = null; }
	}
	public void CreateCharacter() => CreateCharacter(ref character);
	public void Move(Vector2 moveDirection) => baseMotionTransition = () => Move(character, moveDirection);
	public void Jump(Vector2 moveDirection) => baseMotionTransition = () => Jump(character, moveDirection);
	#endregion

	/// <summary> 创建角色 </summary>
	public static void CreateCharacter(ref BaseCharacter character) {
		ModuleVisual.I.Character.UpdateVisual(ref character);
	}

	/// <summary> 移动动作 </summary>
	public static bool Move(BaseCharacter character, Vector2 moveDirection) {
		MotionMove motionMove = new MotionMove(character, moveDirection);
		return character.Transition(motionMove);
	}
	public static bool Move(BaseCharacter character, Vector2 moveDirection, Vector3 position, Vector3 eulerAngles) {
		MotionMove motionMove = new MotionMove(character, moveDirection, position, eulerAngles);
		return character.Transition(motionMove);
	}

	/// <summary> 跳跃动作 </summary>
	public static bool Jump(BaseCharacter character, Vector2 moveDirection) {
		MotionJump motionJump = new MotionJump(character, moveDirection, 1);
		return character.Transition(motionJump);
	}
	public static bool Jump(BaseCharacter character, Vector2 moveDirection, Vector3 position, Vector3 eulerAngles) {
		MotionJump motionJump = new MotionJump(character, moveDirection, 1, position, eulerAngles);
		return character.Transition(motionJump);
	}
}
