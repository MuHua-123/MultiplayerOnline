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
	#endregion

	public static void CreateCharacter(ref KinesisController controller) {
		VisualCharacter.I.UpdateVisual(ref controller);
	}
	public static void Move(KinesisController controller, Vector2 moveDirection) {
		KinesisMove move = new KinesisMove(controller, moveDirection);
		controller.TransitionKinesis(move);
	}
	public static void Move(KinesisController controller, Vector2 moveDirection, Vector3 position, Vector3 eulerAngles) {
		KinesisMove move = new KinesisMove(controller, moveDirection, position, eulerAngles);
		controller.TransitionKinesis(move);
	}
}
