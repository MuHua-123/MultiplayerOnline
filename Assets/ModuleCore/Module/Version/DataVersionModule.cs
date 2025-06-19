using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 模组版本 - 数据
/// </summary>
[Serializable]
public class DataVersionModule {
	/// <summary> 模组名字 </summary>
	public string name;
	/// <summary> 模组版本 </summary>
	public string version;

	public override bool Equals(object obj) {
		if (obj is DataVersionModule other) {
			return name == other.name && version == other.version;
		}
		return false;
	}
	public override int GetHashCode() {
		int hash = 17;
		hash = hash * 31 + (name?.GetHashCode() ?? 0);
		hash = hash * 31 + (version?.GetHashCode() ?? 0);
		return hash;
	}
}
