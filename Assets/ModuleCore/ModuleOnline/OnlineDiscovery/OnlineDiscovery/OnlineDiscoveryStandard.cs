using System;
using System.Collections.Generic;
using System.Net;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

/// <summary>
/// 标准 - 网络发现
/// </summary>
[RequireComponent(typeof(NetworkManager))]
public class OnlineDiscoveryStandard : OnlineDiscovery<DataDiscoveryBroadcast, DataDiscoveryResponse> {

	[SerializeField]
	[Tooltip("如果为true,则OnlineDiscovery将使服务器可见,并在网络代码开始作为服务器运行时立即响应客户端广播.")]
	bool StartWithServer = true;

	public string ServerName = "ServerName";

	private NetworkManager NetworkManager;

	public void Awake() {
		NetworkManager = GetComponent<NetworkManager>();
	}
	public override void StartServer() {
		// 只在配置允许、未启动过且未运行时启动发现服务
		if (!StartWithServer || IsRunning) { return; }
		// 只有在NetworkManager已是服务器时才启动
		if (NetworkManager == null || !NetworkManager.IsServer) { return; }
		base.StartServer();
	}

	public void OnApplicationQuit() {
		StopDiscovery();
	}

	protected override bool ProcessBroadcast(IPEndPoint sender, DataDiscoveryBroadcast broadCast, out DataDiscoveryResponse response) {
		response = new DataDiscoveryResponse() {
			ServerName = ServerName,
			Port = ((UnityTransport)NetworkManager.NetworkConfig.NetworkTransport).ConnectionData.Port,
			gameVersion = ManagerVersion.I.VersionInfo()
		};
		return true;
	}

	protected override void ResponseReceived(IPEndPoint sender, DataDiscoveryResponse response) {
		response.address = sender.Address;
		OnlineManager.ServerFound(sender, response);
	}
}