using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 场景 - 联机管理器
/// </summary>
public class OnlineScene : OnlineSingle<OnlineScene> {

	protected override void Awake() => NoReplace();

}
