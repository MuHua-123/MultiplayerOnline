using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// 世界 - 数据
/// </summary>
[Serializable]
public class DataOnlineWorld : INetworkSerializable {

	/// <summary> 场景名字 </summary>
	public string name;

	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
		serializer.SerializeValue(ref name);
	}

}
