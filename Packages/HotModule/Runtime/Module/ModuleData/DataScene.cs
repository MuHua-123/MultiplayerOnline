using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// 场景配置数据
/// </summary>
[Serializable]
public class DataScene {
	/// <summary> 场景名字 </summary>
	public string name;
	/// <summary> 场景名字 </summary>
	public Texture2D texture;
	/// <summary> 参考 </summary>
	public AssetReference assetReference;
}
