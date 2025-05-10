using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 运行模式
/// </summary>
public enum EnumRunningMode {
	None,// 无模式

	Single,// 单机模式

	Host,// 主机模式
	Server,// 服务端模式
	Client,// 客户端模式
}
