using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// 场景配置预设
/// </summary>
[CreateAssetMenu(fileName = "SceneConfig", menuName = "数据模块/场景配置")]
public class ConstSceneConfig : ScriptableObject {
	public Texture2D texture;// 预览图片
	public AssetReference scene;// 参考
}
