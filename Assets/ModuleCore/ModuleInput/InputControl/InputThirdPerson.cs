using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 第三人称输入器
/// </summary>
public class InputThirdPerson : InputControl {
	public Vector2 moveInput;
	public Vector2 moveDirection;
	public bool isRotating = false;
	public Vector2 delta;

	private bool isEnable;
	private Vector3 eulerAngles;
	private Vector3 originalEulerAngles;

	private CameraController CurrentCamera => ModuleCamera.CurrentCamera;

	protected override void ModuleInput_OnInputMode(EnumInputMode mode) {
		isEnable = mode == EnumInputMode.ThirdPerson;
		if (isEnable || moveDirection == Vector2.zero) { return; }
		moveDirection = Vector2.zero;
	}

	private void Update() {
		if (!isEnable) { return; }
		originalEulerAngles = Vector3.Lerp(originalEulerAngles, eulerAngles, Time.deltaTime * 10);
		CurrentCamera.EulerAngles = originalEulerAngles;
	}

	#region 输入系统
	public void OnMove(InputValue inputValue) {
		if (!isEnable) { return; }
		// 获取移动输入
		moveInput = inputValue.Get<Vector2>();
		// 计算相对于相机的移动方向
		moveDirection = Utilities.TransferDirection(CurrentCamera.Forward, CurrentCamera.Right, moveInput);
		ManagerPlayer.I.Move(moveDirection);
	}
	public void OnJump(InputValue inputValue) {
		if (!isEnable) { return; }
		moveDirection = Utilities.TransferDirection(CurrentCamera.Forward, CurrentCamera.Right, moveInput);
		ManagerPlayer.I.Jump(moveDirection);
	}
	public void OnEnableRotating(InputValue inputValue) {
		if (!isEnable) { return; }
		isRotating = inputValue.isPressed;
		eulerAngles = originalEulerAngles = CurrentCamera.EulerAngles;
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
