using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 跳跃动作
	/// </summary>
	public class KinesisJump : IKinesis {

		public KinesisAnimator animator;// 动作动画控制器
		public KinesisController controller;

		public KinesisJump(KinesisController controller) {
			this.controller = controller;
			animator = controller.animator;
		}

		public bool Transition(IKinesis kinesis) {
			return true;
		}
		public void StartKinesis() {
			animator?.Transition("JumpStart");
		}
		public void UpdateKinesis() {

		}
		public void FinishKinesis() {

		}

		public void AnimationEffects() { }
		public void AnimationEnd() { }
		public void AnimationExit() { }
	}
}
