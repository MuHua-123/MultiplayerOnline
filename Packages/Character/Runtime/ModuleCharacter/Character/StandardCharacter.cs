using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 标准角色
	/// </summary>
	public class StandardCharacter : ModuleCharacter {

		private CharacterKinesis currentKinesis;

		public override CharacterKinesis Current => currentKinesis;

		public override void TransitionKinesis(CharacterKinesis kinesis) {
			//不可以转换
			if (currentKinesis != null && !currentKinesis.Transition(kinesis)) { return; }
			//进行转换
			currentKinesis?.FinishKinesis();
			currentKinesis = kinesis;
			currentKinesis?.StartKinesis();
		}

		public override void AnimationEffects() {
			throw new System.NotImplementedException();
		}
		public override void AnimationEnd() {
			throw new System.NotImplementedException();
		}
		public override void AnimationExit() {
			throw new System.NotImplementedException();
		}
	}
}
