using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;
using MuHua;

public class SyncCharacter : NetworkBehaviour {

	private void Start() {
		OnlineManager.I.OnStartServer += OnlineController_OnStartServer;
	}

	#region 服务端
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
		DataCharacter character = new DataCharacter(obj) { position = position };
		CreateCharacter(character);
		CreateCharacterClientRpc(character);
	}
	#endregion

	#region 客户端
	protected override void OnSynchronize<T>(ref BufferSerializer<T> serializer) {
		string json = JsonTool.ToJson(AssetsCharacter.I.Datas);
		serializer.SerializeValue(ref json);
		if (serializer.IsReader) {
			List<DataCharacter> characters = JsonTool.FromJson<List<DataCharacter>>(json);
			AssetsCharacter.I.Clear();
			AssetsCharacter.I.Add(characters);
		}
		base.OnSynchronize(ref serializer);
	}
	public override void OnNetworkSpawn() {
		AssetsCharacter.I.ForEach(obj => VisualCharacter.I.UpdateVisual(obj));
	}
	[ClientRpc]
	public void CreateCharacterClientRpc(DataCharacter character) {
		if (!IsHost) { CreateCharacter(character); }
	}
	[ClientRpc]
	public void UpdateCharacterClientRpc(DataCharacter character) {
		if (!IsHost) { UpdateCharacter(character); }
	}
	[ClientRpc]
	public void RemoveCharacterClientRpc(ulong obj) {
		if (!IsHost) { RemoveCharacter(obj); }
	}
	#endregion

	#region 同步功能
	private void CreateCharacter(DataCharacter character) {
		AssetsCharacter.I.Add(character);
		VisualCharacter.I.UpdateVisual(character);
	}
	private void UpdateCharacter(DataCharacter obj) {
		DataCharacter character = AssetsCharacter.Find(obj.clientId);
		character.Update(obj);
	}
	private void RemoveCharacter(ulong obj) {
		DataCharacter character = AssetsCharacter.Find(obj);
		AssetsCharacter.I.Remove(character);
	}
	#endregion
}
