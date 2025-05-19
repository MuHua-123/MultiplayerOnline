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

	}
}
