using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 角色 - 模块
	/// </summary>
	public abstract class ModuleCharacter : MonoBehaviour {

		public abstract CharacterKinesis Current { get; }

		/// <summary> 动作过渡 </summary>
		public abstract void TransitionKinesis(CharacterKinesis kinesis);

		/// <summary> 触发动画特效 </summary>
		public abstract void AnimationEffects();
		/// <summary> 动画结束(有后摇) </summary>
		public abstract void AnimationEnd();
		/// <summary> 动画退出(无后摇) </summary>
		public abstract void AnimationExit();

	}
}
