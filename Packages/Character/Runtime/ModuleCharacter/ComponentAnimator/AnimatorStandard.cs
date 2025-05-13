using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 标准动画控制器
	/// </summary>
	[RequireComponent(typeof(Animator))]
	public class AnimatorStandard : CharacterAnimator {
		protected int layerIndex;
		protected string current;
		protected Animator animator;
		protected IKinesis kinesis;

		public virtual void Awake() => animator = GetComponent<Animator>();

		/// <summary> 设置动作 </summary>
		public override void SetKinesis(IKinesis kinesis) => this.kinesis = kinesis;
		/// <summary> 动画过渡 </summary>
		public override void Transition(string name, float normalizedTransitionDuration = 0.1f) {
			if (current == name) { animator.Play(name); }
			else { animator.CrossFade(name, normalizedTransitionDuration); }
			current = name;
		}
		/// <summary> 动画过渡 </summary>
		public override void Transition(int layerIndex, string name, float normalizedTransitionDuration = 0.1f) {
			animator.SetLayerWeight(this.layerIndex, 0);
			animator.SetLayerWeight(layerIndex, 1);
			this.layerIndex = layerIndex;
			Transition(name, normalizedTransitionDuration);
		}

		/// <summary> 设置参数 </summary>
		public override void SetBool(string name, bool value) => animator.SetBool(name, value);
		/// <summary> 设置参数 </summary>
		public override void SetFloat(string name, float value) => animator.SetFloat(name, value);

		/// <summary> 触发动画特效 </summary>
		public override void AnimationEffects() => kinesis?.AnimationEffects();
		/// <summary> 动画结束(有后摇) </summary>
		public override void AnimationEnd() => kinesis?.AnimationEnd();
		/// <summary> 动画退出(无后摇) </summary>
		public override void AnimationExit() => kinesis?.AnimationExit();
	}
}
