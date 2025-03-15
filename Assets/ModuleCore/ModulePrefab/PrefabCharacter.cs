using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

public class PrefabCharacter : ModulePrefab<DataCharacter> {
	public override void UpdateVisual(DataCharacter value) {
		base.UpdateVisual(value);
		transform.position = value.position;
	}
}
