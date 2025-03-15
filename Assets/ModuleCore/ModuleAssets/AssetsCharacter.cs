using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

public class AssetsCharacter : ModuleAssets<DataCharacter> {

	public override void Add(DataCharacter data) {
		base.Add(data);
		VisualCharacter.I.UpdateVisual(data);
	}
	public override void Add(IList<DataCharacter> data) {
		base.Add(data);
		foreach (DataCharacter item in data) { VisualCharacter.I.UpdateVisual(item); }
	}
	public override void Remove(DataCharacter data) {
		if (!Datas.Contains(data)) { return; }
		base.Remove(data);
		VisualCharacter.I.ReleaseVisual(data);
	}

	public static DataCharacter Find(ulong obj) {
		return I.Datas.Find(character => character.clientId == obj);
	}
}
