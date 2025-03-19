using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputCamera : MonoBehaviour {

	public bool isRotating = false;
	public Vector2 delta;

	private Vector3 eulerAngles;
	private Vector3 originalEulerAngles;
	private CameraController controller;

	private void Start() => controller = ModuleCamera.I.thirdPerson;

	private void Update() {
		originalEulerAngles = Vector3.Lerp(originalEulerAngles, eulerAngles, Time.deltaTime * 10);
		controller.EulerAngles = originalEulerAngles;
	}

	#region 输入系统
	public void OnEnableRotating(InputValue inputValue) {
		isRotating = inputValue.isPressed;
		eulerAngles = originalEulerAngles = controller.EulerAngles;
	}
	public void OnRotateCamera(InputValue inputValue) {
		if (!isRotating) { return; }
		delta = inputValue.Get<Vector2>();
		// 计算旋转角度
		float x = Screen.width / Screen.height;
		eulerAngles += new Vector3(-delta.y, delta.x * x, 0);
	}
	#endregion
}
