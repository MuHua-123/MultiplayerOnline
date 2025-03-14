using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class SyncCharacter : NetworkBehaviour {

	public Transform prefab;

	public Dictionary<ulong, Transform> characters = new Dictionary<ulong, Transform>();

	private void Start() {
		OnlineController.I.OnStartServer += OnlineController_OnStartServer;
	}

	private void OnlineController_OnStartServer(ServerMode mode) {
		NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
		NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
		if (mode == ServerMode.Server) { return; }
		CreateCharacter(OwnerClientId);
	}

	private void OnClientConnectedCallback(ulong obj) {
		Debug.Log($"已连接: {obj}");
		CreateCharacter(obj);
	}
	private void OnClientDisconnectCallback(ulong obj) {
		Debug.Log($"已断开连接: {obj}");
		RemoveCharacter(obj);
		RemoveCharacterClientRpc(obj);
	}

	private void CreateCharacter(ulong obj) {
		Vector3 position = new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
		DataCharacter data = new DataCharacter() { position = position };
		CreateCharacter(obj, data);
		CreateCharacterClientRpc(obj, data);
	}
	private void CreateCharacter(ulong obj, DataCharacter data) {
		Transform character = Instantiate(prefab, transform);
		character.position = data.position;
		characters.Add(obj, character);
	}
	[ClientRpc]
	private void CreateCharacterClientRpc(ulong obj, DataCharacter data) {
		if (!IsHost) { CreateCharacter(obj, data); }
	}

	private void RemoveCharacter(ulong obj) {
		if (!characters.TryGetValue(obj, out Transform character)) { return; }
		Destroy(character.gameObject);
		characters.Remove(obj);
	}
	[ClientRpc]
	private void RemoveCharacterClientRpc(ulong obj) {
		if (!IsHost) { RemoveCharacter(obj); }
	}
}
