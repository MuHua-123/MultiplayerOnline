using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 摄像机 - 输入器
/// </summary>
public class InputCamera : InputControl {
	public bool isRotating = false;
	public Vector2 delta;

	private bool isEnable;
	private Vector3 eulerAngles;
	private Vector3 originalEulerAngles;

	private CameraController CurrentCamera => ModuleCamera.CurrentCamera;

	protected override void ModuleInput_OnInputMode(EnumInputMode mode) {
		isEnable = mode == EnumInputMode.ThirdPerson;
	}

	private void Update() {
		if (!isEnable) { return; }
		originalEulerAngles = Vector3.Lerp(originalEulerAngles, eulerAngles, Time.deltaTime * 10);
		CurrentCamera.EulerAngles = originalEulerAngles;
	}

	#region 输入系统
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
		eulerAngles += new Vector3(-delta.y, delta.x * x * 2, 0);
	}
	#endregion
}
