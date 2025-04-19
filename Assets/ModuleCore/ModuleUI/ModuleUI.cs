using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// UI模块
/// </summary>
public class ModuleUI : ModuleSingle<ModuleUI> {
	public static event Action<UIPageType> OnJumpPage;

	protected override void Awake() => NoReplace();

	public static void Jump(UIPageType pageType) => OnJumpPage?.Invoke(pageType);
}
