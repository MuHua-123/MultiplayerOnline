using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 角色生成器
/// </summary>
public class VisualCharacter : VisualController<BaseCharacter> {
	/// <summary> 生成空间 </summary>
	public Transform space;
	/// <summary> 数据预制件 </summary>
	public Transform prefab;

	/// <summary> 更新可视化内容 </summary>
	public override void UpdateVisual(ref BaseCharacter visual) {
		Create(ref visual, prefab, space);
	}
	/// <summary> 释放可视化内容 </summary>
	public override void ReleaseVisual(BaseCharacter visual) {
		if (visual != null) { Destroy(visual.gameObject); }
	}
}
