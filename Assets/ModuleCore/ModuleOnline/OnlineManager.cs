using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;
using MuHua;

/// <summary>
/// 联机 - 管理器
/// </summary>
[RequireComponent(typeof(UnityTransport))]
[RequireComponent(typeof(NetworkManager))]
public class OnlineManager : ModuleSingle<OnlineManager> {
	/// <summary> 客户端完成连接 </summary>
	public static event Action OnClientConnection;
	/// <summary> 找到服务器事件 </summary>
	public static event Action<IPEndPoint, DataDiscoveryResponse> OnServerFound;
	/// <summary> 找到服务器 </summary>
	public static void ServerFound(IPEndPoint sender, DataDiscoveryResponse response) => OnServerFound?.Invoke(sender, response);

	[Tooltip("是否启用https")]
	public bool isHttps;
	[Tooltip("网络发现工具")]
	public OnlineDiscovery<DataDiscoveryBroadcast, DataDiscoveryResponse> discovery;

	private string localhost = "127.0.0.1";
	private string defaultPort = "5000";

	private UnityTransport unityTransport => GetComponent<UnityTransport>();
	private NetworkManager networkManager => GetComponent<NetworkManager>();

	protected override void Awake() => NoReplace();

	/// <summary> 启动服务器模式 </summary>
	public void StartServer() => StartServer(localhost, defaultPort);
	/// <summary> 启动主机模式 </summary>
	public void StartHost() => StartHost(localhost, defaultPort);

	/// <summary> 启动服务器模式 </summary>
	public void StartServer(string address, string port) {
		if (isHttps) { unityTransport.SetServerSecrets(OnlineSecure.GameServerCertificate, OnlineSecure.GameServerPrivateKey); }
		unityTransport.SetConnectionData(address, ushort.Parse(port), "0.0.0.0");
		networkManager.StartServer();
		discovery.StartServer();
		Debug.Log($"服务器地址: {address}:{port}");
	}
	/// <summary> 启动主机模式 </summary>
	public void StartHost(string address, string port) {
		unityTransport.SetConnectionData(address, ushort.Parse(port), "0.0.0.0");
		networkManager.StartHost();
		discovery.StartServer();
		Debug.Log($"主机地址: {address}:{port}");
	}
	/// <summary> 启动客户端模式 </summary>
	public void StartClient(string address, string port) {
		if (isHttps) { unityTransport.SetClientSecrets(OnlineSecure.ServerCommonName, OnlineSecure.GameClientCertificate); }
		unityTransport.SetConnectionData(address, ushort.Parse(port));
		networkManager.StartClient();
		networkManager.OnConnectionEvent += NetworkManager_OnConnectionEvent;
		Debug.Log($"连接地址: {address}:{port}");
	}

	private void NetworkManager_OnConnectionEvent(NetworkManager manager, ConnectionEventData data) {
		Debug.Log($"客户端完成连接!");
		OnClientConnection?.Invoke();
	}
}
