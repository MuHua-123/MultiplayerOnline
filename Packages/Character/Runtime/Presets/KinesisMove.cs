using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 移动动作
	/// </summary>
	public class KinesisMove : IKinesis {

		public Vector2 moveDirection;//  移动方向
		public KinesisMovement movement;
		public KinesisController controller;

		public KinesisMove(KinesisController controller, Vector2 moveDirection, Vector3 position) {
			this.controller = controller;
			this.moveDirection = moveDirection;
			movement = controller.movement;
			movement.transform.position = position;
		}

		public bool Transition(IKinesis kinesis) {
			KinesisMove move = kinesis as KinesisMove;
			if (move == null) { return true; }
			moveDirection = move.moveDirection;
			movement.SetDirection(moveDirection);
			return false;
		}
		public void StartKinesis() {
			movement.SetDirection(moveDirection);
		}
		public void UpdateKinesis() {
			if (!movement.IsStop) { return; }
			controller.TransitionKinesis(new KinesisIdle());
		}
		public void FinishKinesis() {
			movement.StopMovement();
		}

		public void AnimationEffects() { }
		public void AnimationEnd() { }
		public void AnimationExit() { }

	}
}
