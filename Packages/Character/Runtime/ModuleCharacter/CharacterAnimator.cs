using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 角色动画控制器
	/// </summary>
	public abstract class CharacterAnimator : MonoBehaviour {
		/// <summary> 设置动作 </summary>
		public abstract void SetKinesis(IKinesis kinesis);
		/// <summary> 动画过渡 </summary>
		public abstract void Transition(string name, float normalizedTransitionDuration = 0.1f);
		/// <summary> 动画过渡 </summary>
		public abstract void Transition(int layerIndex, string name, float normalizedTransitionDuration = 0.1f);

		/// <summary> 设置参数 </summary>
		public abstract void SetBool(string name, bool value);
		/// <summary> 设置参数 </summary>
		public abstract void SetFloat(string name, float value);

		/// <summary> 触发动画特效 </summary>
		public abstract void AnimationEffects();
		/// <summary> 动画结束(有后摇) </summary>
		public abstract void AnimationEnd();
		/// <summary> 动画退出(无后摇) </summary>
		public abstract void AnimationExit();
	}
}
