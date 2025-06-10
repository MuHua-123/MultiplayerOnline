using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;
using MuHua;

[RequireComponent(typeof(UnityTransport))]
[RequireComponent(typeof(NetworkManager))]
public class OnlineManager : ModuleSingle<OnlineManager> {
	public static event Action OnCompleteConnection;

	public bool isHttps;
	public OnlineDiscoveryStandard discovery;

	private UnityTransport unityTransport => GetComponent<UnityTransport>();
	private NetworkManager networkManager => GetComponent<NetworkManager>();

	protected override void Awake() => NoReplace();

	/// <summary> 启动服务器模式 </summary>
	public void StartServer(string address, string port) {
		if (isHttps) { unityTransport.SetServerSecrets(OnlineSecure.GameServerCertificate, OnlineSecure.GameServerPrivateKey); }
		unityTransport.SetConnectionData(address, ushort.Parse(port), "0.0.0.0");
		networkManager.StartServer();
		Debug.Log($"服务器地址: {address}:{port}");
	}
	/// <summary> 启动主机模式 </summary>
	public void StartHost(string address, string port) {
		unityTransport.SetConnectionData(address, ushort.Parse(port), "0.0.0.0");
		networkManager.StartHost();
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
		OnCompleteConnection?.Invoke();
	}
}
