using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 角色控制器
/// </summary>
public abstract class CCharacter : MonoBehaviour {

	/// <summary> 角色模块 </summary>
	public abstract MCharacter MCharacter { get; }
	/// <summary> 角色数据 </summary>
	public abstract DataCharacter DCharacter { get; }

	public abstract void Initial(Vector3 position, Vector3 eulerAngles);

	public static CCharacter AddControl(HCharacter hCharacter) {
		if (hCharacter is HCharacterCollision collision) { return hCharacter.gameObject.AddComponent<CCharacterCollision>(); }
		return null;
	}
}
