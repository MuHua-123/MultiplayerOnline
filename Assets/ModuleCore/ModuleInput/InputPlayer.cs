using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputPlayer : MonoBehaviour {

	public Vector2 moveInput;

	private OnlinePlayer player;
	private DataCharacter character;

	// private void Start() {
	// 	OnlineManager.I.OnCompleteSyncScene += OnlineController_OnCompleteSyncScene;
	// }

	// private void OnlineController_OnCompleteSyncScene() {
	// 	character = AssetsCharacter.Find(player.OwnerClientId);
	// }

	#region 输入系统
	public void OnMove(InputValue inputValue) {
		// 获取移动输入
		moveInput = inputValue.Get<Vector2>();

		if (player == null) { player = OnlinePlayer.Find(); }
		player?.MoveServerRpc(moveInput);
	}
	#endregion
}
