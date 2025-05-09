using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 跳跃动作
	/// </summary>
	public class KinesisJump : IKinesis {

		public bool isJump = false;
		public KinesisAnimator animator;// 动作动画控制器
		public KinesisMovement movement;
		public KinesisController controller;

		public KinesisJump(KinesisController controller) {
			this.controller = controller;
			animator = controller.animator;
			movement = controller.movement;
		}

		public bool Transition(IKinesis kinesis) {
			return true;
		}
		public void StartKinesis() {
			isJump = false;
			movement?.SetJump();
			// 更新动画器
			animator?.Transition("JumpStart");
		}
		public void UpdateKinesis() {
			if (!movement.Grounded) { isJump = true; }
			if (movement.Grounded && isJump) {
				animator?.Transition("JumpLand", 0.05f);
				controller.TransitionKinesis(new KinesisIdle());
			}
		}
		public void FinishKinesis() {

		}

		public void AnimationEffects() {

		}
		public void AnimationEnd() {

		}
		public void AnimationExit() {

		}
	}
}
