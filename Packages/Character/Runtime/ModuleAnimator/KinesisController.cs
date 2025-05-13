using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 动作控制器
	/// </summary>
	public class KinesisController : MonoBehaviour {
		public KinesisAnimator animator;
		public KinesisMovement movement;

		private IKinesis currentKinesis;

		public virtual void Awake() => TransitionKinesis(new KinesisIdle());

		public virtual void Update() => currentKinesis?.UpdateKinesis();

		/// <summary> 动作过渡 </summary>
		public virtual void TransitionKinesis(IKinesis kinesis) {
			//不可以转换
			if (currentKinesis != null && !currentKinesis.Transition(kinesis)) { return; }
			//进行转换
			currentKinesis?.FinishKinesis();
			currentKinesis = kinesis;
			currentKinesis?.StartKinesis();

			animator?.SetKinesis(currentKinesis);
			movement?.SetKinesis(currentKinesis);
		}
	}
}