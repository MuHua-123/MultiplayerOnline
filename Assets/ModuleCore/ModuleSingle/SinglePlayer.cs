using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 单机玩家
/// </summary>
public class SinglePlayer : ModuleSingle<SinglePlayer> {

	protected override void Awake() => NoReplace();

	#region 单机
	[HideInInspector] public KinesisController controller;
	public void CreateCharacter() => CreateCharacter(ref controller);
	public void Move(Vector2 moveDirection) => Move(controller, moveDirection);
	public void Jump() => Jump(controller);
	#endregion

	/// <summary> 创建角色 </summary>
	public static void CreateCharacter(ref KinesisController controller) {
		VisualCharacter.I.UpdateVisual(ref controller);
	}

	/// <summary> 移动动作 </summary>
	public static void Move(KinesisController controller, Vector2 moveDirection) {
		KinesisMove move = new KinesisMove(controller, moveDirection);
		controller.TransitionKinesis(move);
	}
	public static void Move(KinesisController controller, Vector2 moveDirection, Vector3 position, Vector3 eulerAngles) {
		KinesisMove move = new KinesisMove(controller, moveDirection, position, eulerAngles);
		controller.TransitionKinesis(move);
	}

	/// <summary> 跳跃动作 </summary>
	public static void Jump(KinesisController controller) {
		KinesisJump jump = new KinesisJump(controller);
		controller.TransitionKinesis(jump);
	}
}
