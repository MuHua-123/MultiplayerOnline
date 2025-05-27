using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 动作 - 跳跃
	/// </summary>
	public class MotionJump : BaseMotion {
		/// <summary> 基础角色 </summary>
		public readonly BaseCharacter character;

		/// <summary> 结束跳跃 </summary>
		public bool isEndJump;
		/// <summary> 是否接地 </summary>
		public bool isGrounded;
		/// <summary> 允许转换 </summary>
		public bool isTransition;
		/// <summary> 跳跃高度 </summary>
		public float jumpHeight;
		/// <summary> 移动速度 </summary>
		public float moveSpeed;
		/// <summary> 移动方向 </summary>
		public Vector2 moveDirection;
		/// <summary> 初始位置 </summary>
		public Vector3 position;
		/// <summary> 初始角度 </summary>
		public Vector3 eulerAngles;

		private bool isInitial = false;

		/// <summary> 运动组件 </summary>
		public BaseMovement movement => character.movement;

		public MotionJump(BaseCharacter character, Vector2 moveDirection, float jumpHeight) {
			this.character = character;
			this.moveDirection = moveDirection;
			this.jumpHeight = jumpHeight;
		}
		public MotionJump(BaseCharacter character, Vector2 moveDirection, float jumpHeight, Vector3 position, Vector3 eulerAngles) {
			this.character = character;
			this.moveDirection = moveDirection;
			this.jumpHeight = jumpHeight;
			this.position = position;
			this.eulerAngles = eulerAngles;
			isInitial = true;
		}

		public override bool Transition(BaseMotion motion) => isTransition;

		public override void StartKinesis() {
			if (isInitial) {
				character.transform.position = position;
				character.transform.eulerAngles = eulerAngles;
			}
			isEndJump = false;
			isTransition = false;
			isGrounded = movement.grounded;
			moveSpeed = movement.moveSpeed;
			movement.Jump(jumpHeight);
			character.SetTrigger("JumpStart");
		}
		public override void UpdateKinesis() {
			if (isEndJump) { return; }
			// 衰退速度
			moveSpeed = Mathf.Lerp(moveSpeed, 0, Time.deltaTime * 0.8f);
			movement.Move(moveDirection, moveSpeed, movement.acceleration);
			// 跳跃状态判断
			if (isGrounded == movement.grounded) { return; }
			isGrounded = movement.grounded;
			// 起跳
			if (!isGrounded) { return; }
			// 落地
			isEndJump = true;
			character.SetTrigger("JumpLand");
			movement.Move(Vector2.zero, moveSpeed, movement.acceleration);
		}
		public override void AnimationExit() {
			isTransition = true;
			// 转换到移动
			MotionMove motionMove = new MotionMove(character, moveDirection);
			character.TransitionKinesis(motionMove);
		}
	}
}