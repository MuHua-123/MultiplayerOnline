using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;
using MuHua;

[RequireComponent(typeof(UnityTransport))]
[RequireComponent(typeof(NetworkManager))]
public class OnlineController : ModuleSingle<OnlineController> {
	public bool isHttps;

	private UnityTransport unityTransport;
	private NetworkManager networkManager;

	protected override void Awake() {
		NoReplace();
		unityTransport = GetComponent<UnityTransport>();
		networkManager = GetComponent<NetworkManager>();
	}

	/// <summary> 启动服务器模式 </summary>
	public void StartServer(string address, string port) {
		if (isHttps) { unityTransport.SetServerSecrets(OnlineSecure.GameServerCertificate, OnlineSecure.GameServerPrivateKey); }
		unityTransport.SetConnectionData(address, ushort.Parse(port), "0.0.0.0");
		networkManager.StartServer();
		networkManager.SceneManager.SetClientSynchronizationMode(LoadSceneMode.Additive);
		Debug.Log($"服务器地址: {address}:{port}");
	}
	/// <summary> 启动主机模式 </summary>
	public void StartHost(string address, string port) {
		unityTransport.SetConnectionData(address, ushort.Parse(port), "0.0.0.0");
		networkManager.StartHost();
		networkManager.SceneManager.SetClientSynchronizationMode(LoadSceneMode.Additive);
		Debug.Log($"主机地址: {address}:{port}");
	}
	/// <summary> 启动客户端模式 </summary>
	public void StartClient(string address, string port) {
		if (isHttps) { unityTransport.SetClientSecrets(OnlineSecure.ServerCommonName, OnlineSecure.GameClientCertificate); }
		unityTransport.SetConnectionData(address, ushort.Parse(port));
		networkManager.StartClient();
		networkManager.SceneManager.PostSynchronizationSceneUnloading = true;
		//networkManager.SceneManager.OnSceneEvent += SceneManager_OnSceneEvent;
		Debug.Log($"连接地址: {address}:{port}");
	}
}
