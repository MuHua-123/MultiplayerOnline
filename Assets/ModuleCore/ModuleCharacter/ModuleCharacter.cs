using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 角色模块
/// </summary>
public static class ModuleCharacter {
	/// <summary> 创建角色 </summary>
	public static void CreateCharacter(ref CCharacter character) {
		ModuleVisual.I.Character.UpdateVisual(ref character);
	}

	/// <summary> 角色动作：移动 </summary>
	public static bool Move(this CCharacter character, Vector2 moveDirection, bool isRotation) {
		KMove move = new KMove(character.MCharacter, moveDirection, isRotation);
		move.Settings(character.DCharacter.moveSpeed, character.DCharacter.acceleration);
		return character.MCharacter.Transition(move);
	}
	public static bool Move(this CCharacter character, Vector2 moveDirection, bool isRotation, Vector3 position, Vector3 eulerAngles) {
		KMove move = new KMove(character.MCharacter, moveDirection, isRotation);
		move.Settings(character.DCharacter.moveSpeed, character.DCharacter.acceleration);
		move.Settings(position, eulerAngles);
		return character.MCharacter.Transition(move);
	}

	/// <summary> 角色动作：跳跃 </summary>
	public static bool Jump(this CCharacter character, Vector2 moveDirection, bool isRotation) {
		KJump jump = new KJump(character.MCharacter, moveDirection, character.DCharacter.jumpHeight, isRotation);
		jump.Settings(character.DCharacter.moveSpeed, character.DCharacter.acceleration);
		return character.MCharacter.Transition(jump);
	}
	public static bool Jump(this CCharacter character, Vector2 moveDirection, bool isRotation, Vector3 position, Vector3 eulerAngles) {
		KJump jump = new KJump(character.MCharacter, moveDirection, character.DCharacter.jumpHeight, isRotation);
		jump.Settings(character.DCharacter.moveSpeed, character.DCharacter.acceleration);
		jump.Settings(position, eulerAngles);
		return character.MCharacter.Transition(jump);
	}
}
