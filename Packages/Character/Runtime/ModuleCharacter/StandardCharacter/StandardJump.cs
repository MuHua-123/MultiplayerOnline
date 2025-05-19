using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 跳跃动作
	/// </summary>
	public class StandardJump : CharacterKinesis, IKinesisJump {

		public readonly StandardCharacter character;

		public bool isEndJump;// 结束跳跃
		public bool isGrounded = true;// 是否接地
		public float jumpHeight;
		public float moveSpeed = 2f;
		public float originalSpeed = 2f;
		public float acceleration = 15f;
		public Vector2 moveDirection;//  移动方向
		public Vector3 position;// 初始位置
		public Vector3 eulerAngles;// 初始角度

		public AnimatorTransition animator => character.animatorTransition;// 动画过渡器
		public MovementTransition movement => character.movementTransition;// 运动过渡器

		public StandardJump(StandardCharacter character) => this.character = character;

		/// <summary> 设置速度和加速度 </summary>
		public void Speed(Vector2 moveDirection, float jumpHeight, float moveSpeed, float acceleration) {
			this.jumpHeight = jumpHeight;
			this.moveSpeed = originalSpeed = moveSpeed;
			this.acceleration = acceleration;
			this.moveDirection = moveDirection;
			position = character.transform.position;
			eulerAngles = character.transform.eulerAngles;
		}
		/// <summary> 初始化位置 </summary>
		public void Initialize(Vector3 position, Vector3 eulerAngles) {
			this.position = position;
			this.eulerAngles = eulerAngles;
		}

		public override bool Transition(CharacterKinesis kinesis) => isEndJump;

		public override void StartKinesis() {
			character.transform.position = position;
			character.transform.eulerAngles = eulerAngles;
			movement.Jump(jumpHeight);

			isEndJump = false;
			isGrounded = movement.grounded;
		}
		public override void UpdateKinesis() {
			if (isEndJump) { return; }
			// 衰退速度
			moveSpeed = Mathf.Lerp(moveSpeed, 0, Time.deltaTime);
			movement.Move(moveDirection, moveSpeed, acceleration);
			// 跳跃状态判断
			if (isGrounded == movement.grounded) { return; }
			isGrounded = movement.grounded;
			// 起跳
			if (!isGrounded) { animator?.Transition("JumpStart"); return; }
			// 落地
			animator?.Transition("JumpLand");
			isEndJump = true;
		}

		public override void AnimationEnd() {
			movement.Move(moveDirection, originalSpeed, acceleration);
		}
		public override void AnimationExit() {

		}
	}
}
