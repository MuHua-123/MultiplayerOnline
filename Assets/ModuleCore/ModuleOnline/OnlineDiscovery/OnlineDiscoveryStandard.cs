using System;
using System.Collections.Generic;
using System.Net;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using MuHua;

/// <summary>
/// 标准 - 网络发现
/// </summary>
[RequireComponent(typeof(NetworkManager))]
public class OnlineDiscoveryStandard : OnlineDiscovery<DataDiscoveryBroadcast, DataDiscoveryResponse> {

	[Tooltip("如果为true,则OnlineDiscovery将使服务器可见,并在网络代码开始作为服务器运行时立即响应客户端广播")]
	public bool StartWithServer = true;
	[Tooltip("服务器名字")]
	public string ServerName = "ServerName";

	/// <summary> 服务器端口 </summary>
	private ushort port;
	/// <summary> 服务器版本 </summary>
	private string serverVersion;
	/// <summary> 网络管理器 </summary>
	private NetworkManager NetworkManager;

	protected override void Awake() {
		NoReplace(false);
		NetworkManager = GetComponent<NetworkManager>();
	}
	public override void StartServer() {
		// 只在配置允许、未启动过且未运行时启动发现服务
		if (!StartWithServer || IsRunning) { return; }
		// 只有在NetworkManager已是服务器时才启动
		if (NetworkManager == null || !NetworkManager.IsServer) { return; }
		base.StartServer();
		// 端口
		port = ((UnityTransport)NetworkManager.NetworkConfig.NetworkTransport).ConnectionData.Port;
		// 版本信息
		serverVersion = JsonTool.ToJson(ManagerVersion.I.VersionInfo());
	}

	public void OnApplicationQuit() {
		StopDiscovery();
	}

	protected override bool ProcessBroadcast(IPEndPoint sender, DataDiscoveryBroadcast broadCast, out DataDiscoveryResponse response) {
		response = new DataDiscoveryResponse() {
			serverName = ServerName,
			port = port,
			serverVersion = serverVersion
		};
		return true;
	}

	protected override void ResponseReceived(IPEndPoint sender, DataDiscoveryResponse response) {
		response.address = sender.Address;
		ServerFound(sender, response);
	}
}