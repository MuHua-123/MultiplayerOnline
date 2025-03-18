using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 相机模块
/// </summary>
public class ModuleCamera : ModuleSingle<ModuleCamera> {

	public CameraController controller;

	/// <summary> 位置 </summary>
	public static Vector3 Position {
		get => I.controller.Position;
		set => I.controller.Position = value;
	}
	/// <summary> 正向 </summary>
	public static Vector3 Forward {
		get => I.controller.Forward;
		set => I.controller.Forward = value;
	}
	/// <summary> 右向 </summary>
	public static Vector3 Right {
		get => I.controller.Right;
		set => I.controller.Right = value;
	}
	/// <summary> 旋转 </summary>
	public static Vector3 EulerAngles {
		get => I.controller.EulerAngles;
		set => I.controller.EulerAngles = value;
	}
	/// <summary> 距离 </summary>
	public static float Distance {
		get => I.controller.Distance;
		set => I.controller.Distance = value;
	}

	protected override void Awake() => Replace();
}
