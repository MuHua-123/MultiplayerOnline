using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 移动动作
	/// </summary>
	public class KinesisMove : IKinesis {

		public Vector3 position;
		public Vector3 eulerAngles;
		public Vector2 moveDirection;//  移动方向
		public KinesisMovement movement;
		public KinesisController controller;

		public KinesisMove(KinesisController controller, Vector2 moveDirection) {
			this.controller = controller;
			this.moveDirection = moveDirection;
			movement = controller.movement;
			position = movement.transform.position;
			eulerAngles = movement.transform.eulerAngles;
		}
		public KinesisMove(KinesisController controller, Vector2 moveDirection, Vector3 position, Vector3 eulerAngles) {
			this.position = position;
			this.eulerAngles = eulerAngles;
			this.controller = controller;
			this.moveDirection = moveDirection;
			movement = controller.movement;
		}

		public bool Transition(IKinesis kinesis) {
			KinesisMove move = kinesis as KinesisMove;
			if (move == null) { return true; }
			position = move.position;
			eulerAngles = move.eulerAngles;
			moveDirection = move.moveDirection;
			movement.transform.position = position;
			movement.transform.eulerAngles = eulerAngles;
			movement.SetDirection(moveDirection);
			return false;
		}
		public void StartKinesis() {
			movement.transform.position = position;
			movement.transform.eulerAngles = eulerAngles;
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
