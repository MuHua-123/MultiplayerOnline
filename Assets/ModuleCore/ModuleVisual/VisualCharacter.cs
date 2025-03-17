using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

public class VisualCharacter : ModuleSingle<VisualCharacter> {
	/// <summary> 生成空间 </summary>
	public Transform space;
	/// <summary> 数据预制件 </summary>
	public Transform prefab;

	protected override void Awake() => NoReplace();

	/// <summary> 更新可视化内容 </summary>
	public virtual void UpdateVisual(ref KinesisController visual) {
		Create(ref visual, prefab, space);
	}
	/// <summary> 释放可视化内容 </summary>
	public virtual void ReleaseVisual(KinesisController visual) {
		if (visual != null) { Destroy(visual.gameObject); }
	}

	/// <summary> 创建可视化内容 </summary>
	public static void Create<Type>(ref Type value, Transform original, Transform parent) {
		if (value != null) { return; }
		Transform temp = CreateTransform(original, parent);
		value = temp.GetComponent<Type>();
	}
	/// <summary> 创建Transform </summary>
	public static Transform CreateTransform(Transform original, Transform parent) {
		Transform temp = Transform.Instantiate(original, parent);
		temp.gameObject.SetActive(true);
		return temp;
	}
}
