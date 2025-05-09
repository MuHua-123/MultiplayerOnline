using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 角色动作
	/// </summary>
	public abstract class CharacterKinesis {
		/// <summary> 动作过渡 </summary>
		public virtual bool Transition(CharacterKinesis kinesis) { return false; }
		/// <summary> 开始动作 </summary>
		public virtual void StartKinesis() { }
		/// <summary> 更新动作 </summary>
		public virtual void UpdateKinesis() { }
		/// <summary> 完成动作 </summary>
		public virtual void FinishKinesis() { }

		/// <summary> 触发动画特效 </summary>
		public virtual void AnimationEffects() { }
		/// <summary> 动画结束(有后摇) </summary>
		public virtual void AnimationEnd() { }
		/// <summary> 动画退出(无后摇) </summary>
		public virtual void AnimationExit() { }
	}
}