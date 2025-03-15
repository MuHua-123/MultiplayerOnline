using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

public class OnlineSync : ModuleSingle<OnlineSync> {
	public SyncCharacter syncCharacter;

	protected override void Awake() => NoReplace();

	public void UpdateCharacter(DataCharacter obj) {
		syncCharacter.UpdateCharacterClientRpc(obj);
	}
}
