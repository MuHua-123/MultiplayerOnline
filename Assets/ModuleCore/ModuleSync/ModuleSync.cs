using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

public class ModuleSync : ModuleSingle<ModuleSync> {
	protected override void Awake() => NoReplace();
}
