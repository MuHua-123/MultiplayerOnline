using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets.ResourceLocators;

/// <summary>
/// 模组数据
/// </summary>
public class DataModuleConfig {
	public bool isEnable;// 是否启用
	public string name;// 模组名字
	public string catalogPath;// 目录路径
	public IResourceLocator locator;// 资源定位器
}
