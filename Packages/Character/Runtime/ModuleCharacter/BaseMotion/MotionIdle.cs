using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 动作 - 空闲
	/// </summary>
	public class MotionIdle : BaseMotion {

		public override bool Transition(BaseMotion motion) => true;

	}
}
