using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可视化控制器
/// </summary>
public abstract class VisualController<T> : MonoBehaviour {
	/// <summary> 更新可视化内容 </summary>
	public abstract void UpdateVisual(ref T visual);
	/// <summary> 释放可视化内容 </summary>
	public abstract void ReleaseVisual(T visual);

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
