using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OnlinePlayer : NetworkBehaviour {

	private DataCharacter character;

	#region 服务端
	[ServerRpc]
	public void MoveServerRpc(Vector2 moveInput) {
		character.moveInput = moveInput;
		character.Update();
		OnlineSync.I.UpdateCharacter(character);
	}
	#endregion

	#region 客户端
	public override void OnNetworkSpawn() {
		character = AssetsCharacter.Find(OwnerClientId);
	}
	#endregion

	#region 工具
	public static OnlinePlayer Find() {
		NetworkObject network = NetworkManager.Singleton.LocalClient.PlayerObject;
		return network.GetComponent<OnlinePlayer>();
	}
	#endregion

}
