using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 角色运动控制器
	/// </summary>
	public abstract class CharacterMovement : MonoBehaviour {
		/// <summary> 设置动作 </summary>
		public abstract void SetKinesis(IKinesis kinesis);
		/// <summary> 设置方向 </summary>
		public abstract void SetDirection(Vector2 moveDirection);
		/// <summary>  H*-2*G的平方根=达到所需高度所需的速度 </summary>
		public abstract void SetJump();
		/// <summary> 停止移动 </summary>
		public abstract void StopMovement();
	}
}
