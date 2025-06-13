using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/// <summary>
/// 联机单例
/// </summary>
public abstract class OnlineSingle<T> : NetworkBehaviour where T : OnlineSingle<T> {
	/// <summary> 模块单例 </summary>
	public static T I => instance;
	/// <summary> 模块单例 </summary>
	protected static T instance;
	/// <summary> 初始化 </summary>
	protected abstract void Awake();

	/// <summary> 替换，并且设置切换场景不销毁 </summary>
	protected virtual void Replace(bool isDontDestroy = true) {
		if (instance != null) { Destroy(instance.gameObject); }
		instance = (T)this;
		if (isDontDestroy) { DontDestroyOnLoad(gameObject); }
	}
	/// <summary> 不替换，并且设置切换场景不销毁 </summary>
	protected virtual void NoReplace(bool isDontDestroy = true) {
		if (isDontDestroy) { DontDestroyOnLoad(gameObject); }
		if (instance == null) { instance = (T)this; }
		else { Destroy(gameObject); }
	}
}
