using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 动画过渡器
	/// </summary>
	public class AnimatorTransition {
		private int layerIndex;
		private string current;
		private Animator animator;

		public AnimatorTransition(Animator animator) { this.animator = animator; }

		/// <summary> 动画过渡 </summary>
		public void Transition(string name, float normalizedTransitionDuration = 0.1f) {
			if (current == name) { animator.Play(name); }
			else { animator.CrossFade(name, normalizedTransitionDuration); }
			current = name;
		}
		/// <summary> 动画过渡 </summary>
		public void Transition(int layerIndex, string name, float normalizedTransitionDuration = 0.1f) {
			animator.SetLayerWeight(this.layerIndex, 0);
			animator.SetLayerWeight(layerIndex, 1);
			this.layerIndex = layerIndex;
			Transition(name, normalizedTransitionDuration);
		}

		/// <summary> 设置参数 </summary>
		public void SetBool(string name, bool value) => animator.SetBool(name, value);
		/// <summary> 设置参数 </summary>
		public void SetFloat(string name, float value) => animator.SetFloat(name, value);
	}
}
