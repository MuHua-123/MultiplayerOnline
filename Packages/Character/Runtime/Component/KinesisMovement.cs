using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuHua {
	/// <summary>
	/// 动作运动控制器
	/// </summary>
	[RequireComponent(typeof(CharacterController))]
	public class KinesisMovement : MonoBehaviour {
		public float moveSpeed = 5;// 移动速度
		public float acceleration = 10.0f;// 加速度
		[Range(0.0f, 0.3f)]
		public float rotationSmoothTime = 0.12f;// 旋转平滑

		protected float currentSpeed;// 当前速度
		protected Vector2 moveDirection;//  移动方向
		protected float animationBlend;// 动画混合速度
		protected float targetRotation = 0.0f;// 旋转目标
		protected float rotationVelocity;// 旋转速度
		protected float verticalVelocity;// 垂直速度
		protected IKinesis kinesis;// 当前动作
		protected KinesisAnimator animator;// 动作动画控制器
		protected CharacterController controller;// 角色控制器

		public virtual bool IsStop => currentSpeed == 0;

		public virtual void Awake() {
			animator = GetComponent<KinesisAnimator>();
			controller = GetComponent<CharacterController>();
		}
		public virtual void Update() {
			PlanarMovement();
		}

		/// <summary> 设置动作 </summary>
		public virtual void SetKinesis(IKinesis kinesis) => this.kinesis = kinesis;
		/// <summary> 设置方向 </summary>
		public virtual void SetDirection(Vector2 moveDirection) => this.moveDirection = moveDirection;
		/// <summary> 停止移动 </summary>
		public virtual void StopMovement() {
			currentSpeed = 0;
			moveDirection = Vector2.zero;
			animationBlend = 0;
			animator?.SetFloat("MoveSpeed", animationBlend);
		}

		/// <summary> 平面移动 </summary>
		public virtual void PlanarMovement() {
			// 设定目标速度
			float targetSpeed = moveSpeed;

			// 一种简单的加速和减速设计，易于拆卸、更换或迭代

			// 如果没有输入，将目标速度设置为0
			if (moveDirection == Vector2.zero && currentSpeed == 0) { return; }
			if (moveDirection == Vector2.zero) targetSpeed = 0.0f;

			// 当前水平速度的参考
			// float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

			// float speedOffset = 0.1f;

			// 加速或减速至目标速度
			// if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset) {
			// 产生弯曲的结果，而不是线性的结果，从而产生更有机的速度变化
			// 注意Lerp中的T是夹紧的，所以我们不需要夹紧我们的速度
			currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * acceleration);

			// round speed to 3 decimal places
			currentSpeed = Mathf.Round(currentSpeed * 1000f) / 1000f;
			// }
			// else { currentSpeed = targetSpeed; }

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

			// 如果使用角色，请更新动画师
			animator?.SetFloat("MoveSpeed", animationBlend);
		}
	}
}
