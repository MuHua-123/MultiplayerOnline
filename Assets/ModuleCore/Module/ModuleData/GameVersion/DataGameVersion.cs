using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 游戏版本 - 数据
/// </summary>
[Serializable]
public class DataGameVersion {
	/// <summary> 默认模组版本 </summary>
	public List<DataModuleVersion> defaults = new List<DataModuleVersion>();
	/// <summary> 扩展模组版本 </summary>
	public List<DataModuleVersion> extends = new List<DataModuleVersion>();

	public override bool Equals(object obj) {
		if (obj is DataGameVersion other) {
			// 比较 defaults，无视顺序
			if (defaults.Count != other.defaults.Count ||
				!defaults.All(d => other.defaults.Contains(d)) ||
				!other.defaults.All(d => defaults.Contains(d)))
				return false;

			// 比较 extends，无视顺序
			if (extends.Count != other.extends.Count ||
				!extends.All(d => other.extends.Contains(d)) ||
				!other.extends.All(d => extends.Contains(d)))
				return false;

			return true;
		}
		return false;
	}

	public override int GetHashCode() {
		int hash = 17;
		// 无视顺序，聚合所有元素的哈希值
		unchecked {
			hash = hash * 23 + defaults.Aggregate(0, (acc, d) => acc + (d?.GetHashCode() ?? 0));
			hash = hash * 23 + extends.Aggregate(0, (acc, d) => acc + (d?.GetHashCode() ?? 0));
		}
		return hash;
	}
}
