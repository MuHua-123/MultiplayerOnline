using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// 运动 - 数据
/// </summary>
[Serializable]
public struct DataOnlineMotion : INetworkSerializable {

	/// <summary> 是否攻击 </summary>
	public bool isAttack;
	/// <summary> 是否冲刺 </summary>
	public bool isSprint;
	/// <summary> 移动方向 </summary>
	public Vector2 moveInput;
	/// <summary> 位置 </summary>
	public Vector3 position;
	/// <summary> 旋转 </summary>
	public Vector3 eulerAngles;

	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
		serializer.SerializeValue(ref isAttack);
		serializer.SerializeValue(ref isSprint);
		serializer.SerializeValue(ref moveInput);
		serializer.SerializeValue(ref position);
		serializer.SerializeValue(ref eulerAngles);
	}

	public void Update(CCharacter character) {
		position = character.transform.position;
		eulerAngles = character.transform.eulerAngles;
	}
}
