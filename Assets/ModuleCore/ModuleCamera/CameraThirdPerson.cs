using UnityEngine;
using MuHua;

/// <summary>
/// 第三人称相机
/// </summary>
public class CameraThirdPerson : CameraController {
	public Camera TargetCamera;
	public Vector3 offset; // 相机与玩家的偏移量
	[Range(0, 0.5f)] public float smoothSpeed = 0.125f; // 平滑跟随速度

	public override Vector3 Position {
		get => transform.position;
		set => transform.position = value;
	}
	public override Vector3 Forward {
		get => TargetCamera.transform.forward;
		set => TargetCamera.transform.forward = value;
	}
	public override Vector3 Right {
		get => TargetCamera.transform.right;
		set => TargetCamera.transform.right = value;
	}
	public override Vector3 EulerAngles {
		get => transform.eulerAngles;
		set => transform.eulerAngles = value;
	}
	public override float Distance {
		get => throw new System.NotImplementedException();
		set => throw new System.NotImplementedException();
	}

	public override void Initialize() {
		ModuleCamera.OnCameraMode += ModuleCamera_OnCameraMode;
	}

	public override void ResetCamera() {
		// Position = HotUpdateScene.I.StartPoint.position;
		// EulerAngles = HotUpdateScene.I.StartPoint.eulerAngles;
	}

	private void ModuleCamera_OnCameraMode(EnumCameraMode mode) {
		gameObject.SetActive(mode == EnumCameraMode.ThirdPerson);
		if (mode == EnumCameraMode.ThirdPerson) { ModuleCamera.CurrentCamera = this; }
	}

	private void LateUpdate() {
		OnlinePlayer onlinePlayer = OnlinePlayer.Find();
		BaseCharacter character = onlinePlayer != null ? onlinePlayer.controller : ManagerPlayer.I.character;
		Transform player = character != null ? character.transform : null;

		if (player == null) { return; }

		// 计算目标位置
		Vector3 desiredPosition = player.position + offset;
		// 平滑过渡到目标位置
		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
		transform.position = smoothedPosition;
	}
}

