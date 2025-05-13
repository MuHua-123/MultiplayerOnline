using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 空闲动作
	/// </summary>
	public class KinesisIdle : IKinesis {

		public bool Transition(IKinesis kinesis) => true;
		public void StartKinesis() { }
		public void UpdateKinesis() { }
		public void FinishKinesis() { }

		public void AnimationEffects() { }
		public void AnimationEnd() { }
		public void AnimationExit() { }
	}
}
