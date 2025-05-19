using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 标准运动控制器
	/// </summary>
	[RequireComponent(typeof(CharacterController))]
	public class MovementStandard : CharacterMovement {
		public CharacterAnimator animator;// 动作动画控制器
		public float moveSpeed = 5;// 移动速度
		public float acceleration = 10.0f;// 加速度
		[Range(0.0f, 0.3f)]
		public float rotationSmoothTime = 0.12f;// 旋转平滑

		[Header("跳跃")]
		public float JumpHeight = 1.2f;
		public bool Grounded = true;
		public float GroundedOffset = -0.14f;
		public float GroundedRadius = 0.28f;
		public LayerMask GroundLayers = -1;

		protected float currentSpeed;// 当前速度
		protected Vector2 moveDirection;//  移动方向
		protected float animationBlend;// 动画混合速度
		protected float targetRotation = 0.0f;// 旋转目标
		protected float rotationVelocity;// 旋转速度
		protected float verticalVelocity;// 垂直速度
		protected IKinesis kinesis;// 当前动作

		protected CharacterController controller;// 角色控制器
		public virtual bool IsStop => currentSpeed == 0;
		public virtual float Gravity => Physics.gravity.y;

		public virtual void Awake() {
			controller = GetComponent<CharacterController>();
		}
		public virtual void Update() {
			PlanarMovement();
			GroundedCheck();
		}

		/// <summary> 设置动作 </summary>
		public override void SetKinesis(IKinesis kinesis) => this.kinesis = kinesis;
		/// <summary> 设置方向 </summary>
		public override void SetDirection(Vector2 moveDirection) => this.moveDirection = moveDirection;
		/// <summary>  H*-2*G的平方根=达到所需高度所需的速度 </summary>
		public override void SetJump() => verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
		/// <summary> 停止移动 </summary>
		public override void StopMovement() {
			currentSpeed = 0;
			moveDirection = Vector2.zero;
			animationBlend = 0;
			animator?.SetFloat("MoveSpeed", animationBlend);
		}

		/// <summary> 平面移动 </summary>
		protected virtual void PlanarMovement() {
			// 设定目标速度
			float targetSpeed = moveSpeed;

			// // 如果没有输入，将目标速度设置为0
			// if (moveDirection == Vector2.zero && currentSpeed == 0) { return; }
			if (moveDirection == Vector2.zero) targetSpeed = 0.0f;

			// 当前速度
			currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * acceleration);

			// round speed to 3 decimal places
			currentSpeed = Mathf.Round(currentSpeed * 1000f) / 1000f;

			animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * acceleration);
			if (animationBlend < 0.01f) animationBlend = 0f;

			// 使输入方向标准化
			Vector3 inputDirection = new Vector3(moveDirection.x, 0.0f, moveDirection.y).normalized;

			// 如果有移动输入，则在玩家移动时旋转玩家
			if (moveDirection != Vector2.zero) {
				targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
				float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);

				// 相对于相机位置旋转到面向输入方向
				transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
			}

			Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

			// 移动
			controller.Move(targetDirection.normalized * (currentSpeed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
			// 更新动画器
			animator?.SetFloat("MoveSpeed", animationBlend);
		}
		/// <summary> 地面检测 </summary>
		protected virtual void GroundedCheck() {
			verticalVelocity += Gravity * Time.deltaTime;
			if (Grounded && verticalVelocity < 0.0f) { verticalVelocity = -2f; }
			Vector3 position = transform.position;
			Vector3 rayOrigin = new Vector3(position.x, position.y - GroundedOffset, position.z);
			float rayLength = GroundedRadius + 0.1f; // 射线长度稍微大于检测半径

			// 使用射线检测地面
			Grounded = Physics.Raycast(rayOrigin, Vector3.down, rayLength, GroundLayers, QueryTriggerInteraction.Ignore);

			// 可选：调试显示射线
			Debug.DrawRay(rayOrigin, Vector3.down * rayLength, Grounded ? Color.green : Color.red);
		}
	}
}
