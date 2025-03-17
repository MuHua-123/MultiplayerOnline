using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using MuHua;

public class OnlinePlayer : NetworkBehaviour {

	private Vector3 position;
	private Vector2 moveInput;
	private KinesisController controller;

	protected override void OnSynchronize<T>(ref BufferSerializer<T> serializer) {
		//string json = JsonTool.ToJson(character);
		serializer.SerializeValue(ref position);
		serializer.SerializeValue(ref moveInput);
		//if (serializer.IsReader) { character = JsonTool.FromJson<DataCharacter>(json); }
		base.OnSynchronize(ref serializer);
	}
	public override void OnNetworkSpawn() {
		if (IsOwner) { CreateCharacterServerRpc(); return; }
		VisualCharacter.I.UpdateVisual(ref controller);
		KinesisMove move = new KinesisMove(controller, moveInput, position);
		controller.TransitionKinesis(move);
	}
	public override void OnDestroy() {
		base.OnDestroy();
		VisualCharacter.I.ReleaseVisual(controller);
	}

	#region 服务端
	[ServerRpc]
	public void CreateCharacterServerRpc() {
		ChangeCharacter();
		ChangeCharacterClientRpc();
	}
	[ServerRpc]
	public void MoveServerRpc(Vector2 moveInput) {
		position = controller.transform.position;
		Move(moveInput, position);
		MoveClientRpc(moveInput, position);
	}
	#endregion

	#region 客户端
	[ClientRpc]
	public void ChangeCharacterClientRpc() {
		if (!IsHost) { ChangeCharacter(); }
	}
	[ClientRpc]
	public void MoveClientRpc(Vector2 moveInput, Vector3 position) {
		if (!IsHost) { Move(moveInput, position); }
	}
	#endregion

	#region 同步功能
	public void ChangeCharacter() {
		VisualCharacter.I.UpdateVisual(ref controller);
	}
	public void Move(Vector2 moveInput, Vector3 position) {
		this.moveInput = moveInput;
		KinesisMove move = new KinesisMove(controller, moveInput, position);
		controller.TransitionKinesis(move);
	}
	#endregion

	#region 工具
	public static OnlinePlayer Find() {
		NetworkObject network = NetworkManager.Singleton.LocalClient.PlayerObject;
		return network.GetComponent<OnlinePlayer>();
	}
	#endregion

}
