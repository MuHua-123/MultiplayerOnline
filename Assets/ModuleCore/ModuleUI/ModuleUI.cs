using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

/// <summary>
/// UI模块
/// </summary>
public class ModuleUI : ModuleSingle<ModuleUI> {
	public static EnumPage page;
	public static event Action<EnumPage> OnJumpPage;

	public UIDocument document;// 绑定文档
	public UIWindowManager windowManager;

	/// <summary> 根目录文档 </summary>
	public VisualElement root => document.rootVisualElement;

	protected override void Awake() => NoReplace();

	/// <summary> 跳转页面 </summary>
	public static void Jump(EnumPage pageType) => OnJumpPage?.Invoke(pageType);
	/// <summary> 打开联机列表窗口 </summary>
	public static void OpenOnlineWindow() => I.windowManager.onlineWindow.SetActive(true);
	/// <summary> 打开模组设置窗口 </summary>
	public static void OpenModuleWindow() => I.windowManager.moduleWindow.SetActive(true);
}
