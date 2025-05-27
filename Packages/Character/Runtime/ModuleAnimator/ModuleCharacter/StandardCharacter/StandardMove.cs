using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 移动动作
	/// </summary>
	// public class StandardMove : CharacterKinesis, IKinesisMove {

	// public readonly StandardCharacter character;

	// public float moveSpeed = 2f;
	// public float acceleration = 15f;
	// public Vector2 moveDirection;//  移动方向
	// public Vector3 position;// 初始位置
	// public Vector3 eulerAngles;// 初始角度

	// public AnimatorTransition animator => character.animatorTransition;// 动画过渡器
	// public MovementTransition movement => character.movementTransition;// 运动过渡器

	// public StandardMove(StandardCharacter character) => this.character = character;

	// /// <summary> 设置速度和加速度 </summary>
	// public void Speed(Vector2 moveDirection, float moveSpeed, float acceleration) {
	// 	this.moveSpeed = moveSpeed;
	// 	this.acceleration = acceleration;
	// 	this.moveDirection = moveDirection;
	// 	position = character.transform.position;
	// 	eulerAngles = character.transform.eulerAngles;
	// }
	// /// <summary> 初始化位置 </summary>
	// public void Initialize(Vector3 position, Vector3 eulerAngles) {
	// 	this.position = position;
	// 	this.eulerAngles = eulerAngles;
	// }

	// public override bool Transition(CharacterKinesis kinesis) => true;

	// public override void StartKinesis() {
	// 	character.transform.position = position;
	// 	character.transform.eulerAngles = eulerAngles;
	// 	movement.Move(moveDirection, moveSpeed, acceleration);
	// }
	// public override void UpdateKinesis() {
	// 	// 更新动画器
	// 	animator.SetFloat("MoveSpeed", movement.animationBlend);
	// 	// 移动结束
	// 	if (movement.currentSpeed != 0) { return; }
	// 	character.TransitionKinesis(new StandardIdle(character));
	// }
	// }
}
