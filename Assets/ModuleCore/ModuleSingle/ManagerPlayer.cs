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
	public Character character;
	public void CreateCharacter() => CreateCharacter(ref character);
	public void Move(Vector2 moveDirection) => Move(character, moveDirection);
	public void Jump(Vector2 moveDirection) => Jump(character, moveDirection);
	#endregion

	/// <summary> 创建角色 </summary>
	public static void CreateCharacter(ref Character character) {
		ModuleVisual.I.Character.UpdateVisual(ref character);
	}

	/// <summary> 移动动作 </summary>
	public static void Move(Character character, Vector2 moveDirection) {
		KinesisMove move = new KinesisMove(character);
		move.Speed(moveDirection, 6, 15);
		character.TransitionKinesis(move.kinesis);
	}
	public static void Move(Character character, Vector2 moveDirection, Vector3 position, Vector3 eulerAngles) {
		KinesisMove move = new KinesisMove(character);
		move.Speed(moveDirection, 6, 15);
		move.Initialize(position, eulerAngles);
		character.TransitionKinesis(move.kinesis);
	}

	/// <summary> 跳跃动作 </summary>
	public static void Jump(Character character, Vector2 moveDirection) {
		KinesisJump jump = new KinesisJump(character);
		jump.Speed(moveDirection, 1, 6, 15);
		character.TransitionKinesis(jump.kinesis);
	}
	public static void Jump(Character character, Vector2 moveDirection, Vector3 position, Vector3 eulerAngles) {
		KinesisJump jump = new KinesisJump(character);
		jump.Speed(moveDirection, 1, 6, 15);
		jump.Initialize(position, eulerAngles);
		character.TransitionKinesis(jump.kinesis);
	}
}
