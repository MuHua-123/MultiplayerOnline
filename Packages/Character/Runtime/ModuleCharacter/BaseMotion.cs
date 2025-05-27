using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 基础运动
	/// </summary>
	public class BaseMotion {
		/// <summary> 动作过渡 </summary>
		public virtual bool Transition(BaseMotion motion) { return false; }
		/// <summary> 开始动作 </summary>
		public virtual void StartKinesis() { }
		/// <summary> 更新动作 </summary>
		public virtual void UpdateKinesis() { }
		/// <summary> 完成动作 </summary>
		public virtual void FinishKinesis() { }
		/// <summary> 动画结束 </summary>
		public virtual void AnimationExit() { }
	}
}