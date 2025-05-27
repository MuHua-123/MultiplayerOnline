using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 角色控制器
	/// </summary>
	public abstract class Character : MonoBehaviour {

		/// <summary> 当前动作 </summary>
		public abstract CharacterKinesis Current { get; }

		/// <summary> 动作过渡 </summary>
		public abstract void TransitionKinesis(CharacterKinesis kinesis);
	}
}