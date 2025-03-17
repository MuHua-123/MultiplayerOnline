using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using MuHua;

[Serializable]
public class DataCharacter : INetworkSerializable {

	//同步
	public Vector3 position;

	//执行
	public Vector2 moveInput;//方向

	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {

		serializer.SerializeValue(ref position);

		serializer.SerializeValue(ref moveInput);
	}
}
