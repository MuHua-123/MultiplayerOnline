using System;
using System.Collections.Generic;
using System.Net;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

[RequireComponent(typeof(NetworkManager))]
public class OnlineDiscoveryStandard : OnlineDiscovery<DataDiscoveryBroadcast, DataDiscoveryResponse> {
	private NetworkManager m_NetworkManager;

	[SerializeField]
	[Tooltip("如果为true,则OnlineDiscovery将使服务器可见,并在网络代码开始作为服务器运行时立即响应客户端广播.")]
	bool m_StartWithServer = true;

	public string ServerName = "ServerName";

	public event Action<IPEndPoint, DataDiscoveryResponse> OnServerFound;

	private bool m_HasStartedWithServer = false;

	public void Awake() {
		m_NetworkManager = GetComponent<NetworkManager>();
	}
	public void Update() {
		if (m_StartWithServer && m_HasStartedWithServer == false && IsRunning == false) {
			if (m_NetworkManager.IsServer) {
				StartServer();
				m_HasStartedWithServer = true;
			}
		}
	}

	protected override bool ProcessBroadcast(IPEndPoint sender, DataDiscoveryBroadcast broadCast, out DataDiscoveryResponse response) {
		response = new DataDiscoveryResponse() {
			ServerName = ServerName,
			Port = ((UnityTransport)m_NetworkManager.NetworkConfig.NetworkTransport).ConnectionData.Port,
		};
		return true;
	}

	protected override void ResponseReceived(IPEndPoint sender, DataDiscoveryResponse response) {
		OnServerFound.Invoke(sender, response);
	}
}