using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 动作 - 移动
	/// </summary>
	public class MotionMove : BaseMotion {
		/// <summary> 基础角色 </summary>
		public readonly BaseCharacter character;

		/// <summary> 移动方向 </summary>
		public Vector2 moveDirection;
		/// <summary> 初始位置 </summary>
		public Vector3 position;
		/// <summary> 初始角度 </summary>
		public Vector3 eulerAngles;

		private bool isInitial = false;

		/// <summary> 运动组件 </summary>
		public BaseMovement movement => character.movement;

		public MotionMove(BaseCharacter character, Vector2 moveDirection) {
			this.character = character;
			this.moveDirection = moveDirection;
		}
		public MotionMove(BaseCharacter character, Vector2 moveDirection, Vector3 position, Vector3 eulerAngles) {
			this.character = character;
			this.moveDirection = moveDirection;
			this.position = position;
			this.eulerAngles = eulerAngles;
			isInitial = true;
		}

		public override bool Transition(BaseMotion motion) => true;

		public override void StartKinesis() {
			if (isInitial) {
				character.transform.position = position;
				character.transform.eulerAngles = eulerAngles;
			}

			character.Move(moveDirection);
		}
		public override void UpdateKinesis() {
			// 更新动画器
			character.SetFloat("MoveSpeed", movement.animationBlend);
			// 移动结束
			if (movement.currentSpeed == 0) { character.TransitionKinesis(new MotionIdle()); }
		}
	}
}
