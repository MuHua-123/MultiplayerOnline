using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/// <summary>
/// 场景 - 联机管理器
/// </summary>
public class OnlineScene : NetworkBehaviour {

	public DataWorld dataWorld;

	protected override void OnSynchronize<T>(ref BufferSerializer<T> serializer) {
		serializer.SerializeValue(ref dataWorld);
		base.OnSynchronize(ref serializer);
	}
	public override void OnNetworkSpawn() {
		if (IsServer) { InitialWorld(); }
		if (!IsOwner || IsHost) { return; }
		DataScene dataScene = AssetsScene.I.Find(dataWorld);
		ManagerScene.I.LoadScene(dataScene, SingleManager.Client);
	}

	private void InitialWorld() {
		dataWorld = new DataWorld { name = ManagerScene.CurrentScene.name };
	}
}
