using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	// /// <summary>
	// /// 标准角色
	// /// </summary>
	// [RequireComponent(typeof(Animator))]
	// [RequireComponent(typeof(CharacterController))]
	// public class StandardCharacter : Character {

	// 	public LayerMask groundLayers;// 地面图层

	// 	[HideInInspector] public Animator animator;// 动画器
	// 	public AnimatorTransition animatorTransition;// 动画过渡器

	// 	[HideInInspector] public CharacterController controller;// 控制器
	// 	public MovementTransition movementTransition;// 运动过渡器

	// 	private CharacterKinesis currentKinesis;// 当前动作

	// 	public override CharacterKinesis Current => currentKinesis;

	// 	private void Awake() {
	// 		animator = GetComponent<Animator>();
	// 		animatorTransition = new AnimatorTransition(animator);

	// 		controller = GetComponent<CharacterController>();
	// 		movementTransition = new MovementTransition(controller, groundLayers);

	// 		TransitionKinesis(new StandardIdle(this));
	// 	}

	// 	private void Update() {
	// 		movementTransition.Update();
	// 		currentKinesis?.UpdateKinesis();
	// 	}

	// 	public override void TransitionKinesis(CharacterKinesis kinesis) {
	// 		// 不可以转换
	// 		if (currentKinesis != null && !currentKinesis.Transition(kinesis)) { return; }
	// 		// 进行转换
	// 		currentKinesis?.FinishKinesis();
	// 		currentKinesis = kinesis;
	// 		currentKinesis?.StartKinesis();
	// 	}

	// 	/// <summary> 触发动画特效 </summary>
	// 	public void AnimationEffects() => currentKinesis.AnimationEffects();
	// 	/// <summary> 动画结束(有后摇) </summary>
	// 	public void AnimationEnd() => currentKinesis.AnimationEnd();
	// 	/// <summary> 动画退出(无后摇) </summary>
	// 	public void AnimationExit() => currentKinesis.AnimationExit();
	// }
}
