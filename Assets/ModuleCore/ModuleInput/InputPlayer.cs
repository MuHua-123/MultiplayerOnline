using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 玩家输入
/// </summary>
public class InputPlayer : MonoBehaviour {

	public Vector2 moveInput;
	public Vector2 moveDirection;

	private CameraController controller => ModuleCamera.CurrentCamera;

	public void UpdateMove() {
		// 获取相机的前向和右向
		Vector3 cameraForward = controller.Forward;
		Vector3 cameraRight = controller.Right;

		// 忽略相机的y轴
		cameraForward.y = 0;
		cameraRight.y = 0;

		// 归一化向量
		cameraForward.Normalize();
		cameraRight.Normalize();

		// 计算相对于相机的移动方向
		Vector3 direction = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;
		Vector2 moveDirection = new Vector3(direction.x, direction.z);

		ModuleInput.I.Move(moveDirection);
	}

	#region 输入系统
	public void OnMove(InputValue inputValue) {
		// 获取移动输入
		moveInput = inputValue.Get<Vector2>();
		UpdateMove();
	}
	#endregion
}
