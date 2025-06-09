using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 输入 - 控制
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public abstract class InputControl : MonoBehaviour {

	protected virtual void Awake() {
		ModuleInput.OnInputMode += ModuleInput_OnInputMode;
		ModuleInput.OnTemporarilyDisable += ModuleInput_OnTemporarilyDisable;
	}

	protected virtual void OnDestroy() {
		ModuleInput.OnInputMode -= ModuleInput_OnInputMode;
		ModuleInput.OnTemporarilyDisable -= ModuleInput_OnTemporarilyDisable;
	}

	/// <summary> 输入模式 </summary>
	protected abstract void ModuleInput_OnInputMode(EnumInputMode mode);
	/// <summary> 临时禁用 </summary>
	protected abstract void ModuleInput_OnTemporarilyDisable(bool obj);
}
