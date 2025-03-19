using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 相机模块
/// </summary>
public class ModuleCamera : ModuleSingle<ModuleCamera> {
	public static event Action<CameraMode> OnCameraMode;

	public CameraController thirdPerson;

	protected override void Awake() => NoReplace();

	/// <summary> 禁用相机 </summary>
	public void Disable() {
		thirdPerson.gameObject.SetActive(false);
		OnCameraMode?.Invoke(CameraMode.None);
	}
	/// <summary> 启用第三人称相机 </summary>
	public void EnableThirdPerson() {
		thirdPerson.gameObject.SetActive(true);
		OnCameraMode?.Invoke(CameraMode.None);
	}
}
