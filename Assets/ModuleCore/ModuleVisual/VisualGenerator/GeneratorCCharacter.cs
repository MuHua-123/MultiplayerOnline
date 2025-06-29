using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色 - 生成器
/// </summary>
public class GeneratorCCharacter : VisualGenerator<CCharacter> {
	/// <summary> 生成空间 </summary>
	public Transform space;
	/// <summary> 数据预制件 </summary>
	public Transform prefab;

	public override CCharacter CreateVisual(Transform original) {
		CCharacter visual = null;
		UpdateVisual(ref visual, prefab);
		return visual;
	}

	public override void UpdateVisual(ref CCharacter visual, Transform original) {
		HCharacter hCharacter = Create<HCharacter>(original, space);
		visual = CCharacter.AddControl(hCharacter);
		visual.Initial(Vector3.zero, Vector3.zero);
	}

	public override void ReleaseVisual(CCharacter visual) {
		if (visual != null) { Destroy(visual.gameObject); }
	}
}
