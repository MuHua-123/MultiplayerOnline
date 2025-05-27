using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 基础角色
	/// </summary>
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(CharacterController))]
	public class BaseCharacter : MonoBehaviour {

		[Header("基本组件")]
		/// <summary> 地面图层 </summary>
		public LayerMask groundLayers;
		/// <summary> 动画器 /summary>
		public Animator animator;
		/// <summary> 控制器 </summary>
		public CharacterController controller;

		[Header("基本属性")]
		/// <summary> 移动速度 </summary>
		public float moveSpeed = 2;
		/// <summary> 加速度 </summary>
		public float acceleration = 15;

		/// <summary> 运动组件 </summary>
		public BaseMovement movement;
		/// <summary> 当前动作 </summary>
		public BaseMotion currentMotion;

		public virtual void Awake() {
			movement = new BaseMovement(controller, groundLayers);

			TransitionKinesis(new MotionIdle());
		}

		public virtual void Update() {
			movement.Update();
			currentMotion.UpdateKinesis();
		}

		/// <summary> 动作过渡 </summary>
		public virtual bool TransitionKinesis(BaseMotion motion) {
			// 不可以转换
			if (currentMotion != null && !currentMotion.Transition(motion)) { return false; }
			// 进行转换
			currentMotion?.FinishKinesis();
			currentMotion = motion;
			currentMotion?.StartKinesis();
			return true;
		}
		/// <summary> 动画结束 </summary>
		public virtual void AnimationExit() {
			currentMotion.AnimationExit();
		}

		/// <summary> 移动 </summary>
		public void Move(Vector2 moveDirection) => movement.Move(moveDirection, moveSpeed, acceleration);

		/// <summary> 设置参数 </summary>
		public void SetTrigger(string name) => animator.SetTrigger(name);
		/// <summary> 设置参数 </summary>
		public void SetBool(string name, bool value) => animator.SetBool(name, value);
		/// <summary> 设置参数 </summary>
		public void SetFloat(string name, float value) => animator.SetFloat(name, value);

	}
}