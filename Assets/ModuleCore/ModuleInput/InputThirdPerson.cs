using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 第三人称输入器
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class InputThirdPerson : MonoBehaviour {
	public Vector2 moveInput;
	public Vector2 moveDirection;
	public bool isRotating = false;
	public Vector2 delta;

	private bool isEnable;
	private Vector3 eulerAngles;
	private Vector3 originalEulerAngles;

	private CameraController Controller => ModuleCamera.CurrentCamera;

	private void Awake() {
		ModuleInput.OnInputMode += ModuleInput_OnInputMode;
	}

	private void ModuleInput_OnInputMode(EnumInputMode mode) {
		isEnable = mode != EnumInputMode.None;
	}

	private void Update() {
		if (!isEnable) { return; }
		originalEulerAngles = Vector3.Lerp(originalEulerAngles, eulerAngles, Time.deltaTime * 10);
		Controller.EulerAngles = originalEulerAngles;
	}

	#region 输入系统
	public void OnMove(InputValue inputValue) {
		if (!isEnable) { return; }
		// 获取移动输入
		moveInput = inputValue.Get<Vector2>();
		// 获取相机的前向和右向
		Vector3 cameraForward = Controller.Forward;
		Vector3 cameraRight = Controller.Right;

		// 忽略相机的y轴
		cameraForward.y = 0;
		cameraRight.y = 0;

		// 归一化向量
		cameraForward.Normalize();
		cameraRight.Normalize();

		// 计算相对于相机的移动方向
		// Vector3 direction = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;
		Vector3 direction = ModuleCamera.TransferDirection(Controller.Forward, Controller.Right, moveInput);
		Vector2 moveDirection = new Vector3(direction.x, direction.z);

		ModuleInput.I.Move(moveDirection);
	}
	public void OnJump(InputValue inputValue) {
		Vector3 direction = ModuleCamera.TransferDirection(Controller.Forward, Controller.Right, moveInput);
		Vector2 moveDirection = new Vector3(direction.x, direction.z);
		ModuleInput.I.Jump(moveDirection);
	}
	public void OnEnableRotating(InputValue inputValue) {
		if (!isEnable) { return; }
		isRotating = inputValue.isPressed;
		eulerAngles = originalEulerAngles = Controller.EulerAngles;
	}
	public void OnRotateCamera(InputValue inputValue) {
		if (!isEnable || !isRotating) { return; }
		delta = inputValue.Get<Vector2>();
		// 计算旋转角度
		float x = Screen.width / Screen.height;
		eulerAngles += new Vector3(-delta.y, delta.x * x, 0);
	}
	#endregion
}
