using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 角色动作
	/// </summary>
	public interface IKinesis {
		/// <summary> 动作过渡 </summary>
		public bool Transition(IKinesis kinesis);
		/// <summary> 开始动作 </summary>
		public void StartKinesis();
		/// <summary> 更新动作 </summary>
		public void UpdateKinesis();
		/// <summary> 完成动作 </summary>
		public void FinishKinesis();

		/// <summary> 触发动画特效 </summary>
		public void AnimationEffects();
		/// <summary> 动画结束(有后摇) </summary>
		public void AnimationEnd();
		/// <summary> 动画退出(无后摇) </summary>
		public void AnimationExit();
	}
}
