using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

public class UISyncTestPage : ModuleUIPage {
	public override VisualElement Element => root.Q<VisualElement>("SyncTestPage");
}
