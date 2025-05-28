using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets.ResourceLocators;

/// <summary>
/// 模组数据
/// </summary>
public class DataModule {
	/// <summary> 是否启用 </summary>
	public bool isEnable;
	/// <summary> 模组名字 </summary>
	public string name;
	/// <summary> 目录路径 </summary>
	public string catalogPath;
	/// <summary> 资源定位器 </summary>
	public IResourceLocator locator;
}
